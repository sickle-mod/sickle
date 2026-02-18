using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
    /// <summary>
    /// Writes a structured game log to a .txt file on the user's Desktop.
    ///
    /// Lifetime: construct once (e.g. in GameManager.Init or GameController.Awake),
    /// pass in the GameManager, then call Dispose() or let OnDestroy call it.
    /// No Unity, no UI namespace dependencies.
    ///
    /// File: ScytheLog_[first8ofGameId].txt
    /// If a save is resumed the same file is appended to, with a RESUMED marker.
    ///
    /// ── Output layout ────────────────────────────────────────────────────────
    ///
    ///   === SCYTHE GAME LOG ===
    ///   Date      : 2026-02-17 14:30:22
    ///   Game ID   : a3f2b1c9-...
    ///   Mode      : Singleplayer
    ///
    ///   --- PLAYERS ---
    ///    1. Rusviet     / Agricultural  | Human        | Kosynier
    ///    2. Polania     / Militant      | AI (Medium)  | Bot 1
    ///
    ///   --- ACTION LOG ---
    ///   --- TURN 1 ---
    ///   [   1]  Rusviet        TOP     Move                       Character#0, Mech#1
    ///            >> "Rusviet moved to hex (3,4)"
    ///   [   1]  Rusviet        BOTTOM  Deploy                     Mech#2
    ///            PAY  PayResource          -2 Metal
    ///   ...
    ///   === GAME OVER === (after 47 turns, 01:23:15)
    ///
    ///   --- FINAL SCORES ---
    ///   Rank  Faction        Player           Total   Stars Pop Terr Res Struct Coins
    ///   ...
    /// </summary>
    public class GameActionLogger : IDisposable
    {
        // ── Dependencies ──────────────────────────────────────────────────────
        private readonly GameManager _gm;

        // ── File state ────────────────────────────────────────────────────────
        private readonly string       _filePath;
        private readonly List<string> _pending  = new List<string>();
        private          bool         _disposed;
        private          int          _lastTurnSeen = -1;

        // ─────────────────────────────────────────────────────────────────────
        // Construction / teardown
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Create the logger and immediately write the file header.
        /// Call this after players have been added to GameManager.
        /// </summary>
        /// <param name="gameManager">The live GameManager for this game.</param>
        /// <param name="gameId">
        /// The Game.GetGameId() GUID string. Used as the filename key so that
        /// a resumed save appends to the same file rather than creating a new one.
        /// Pass null/empty to fall back to a timestamp key.
        /// </param>
        public GameActionLogger(GameManager gameManager, string gameId = null)
        {
            _gm = gameManager ?? throw new ArgumentNullException(nameof(gameManager));

            string fileKey = !string.IsNullOrEmpty(gameId) && gameId.Length >= 8
                ? gameId.Substring(0, 8)
                : DateTime.Now.ToString("yyyyMMdd_HHmmss");

            string temp = Path.GetTempPath();
            _filePath = Path.Combine(temp, $"ScytheLog_{fileKey}.txt");

            if (!File.Exists(_filePath))
                WriteHeader();
            else
                WriteResumeMarker();

            Subscribe();
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            Unsubscribe();
            FlushToDisk();
        }

        // ─────────────────────────────────────────────────────────────────────
        // Event wiring
        // ─────────────────────────────────────────────────────────────────────

        private void Subscribe()
        {
            // Static event on ActionLog: fires every time a LogInfo is committed.
            ActionLog.LogInfoCreated += OnLogInfoCreated;

            // Instance events on GameManager:
            _gm.ChangeTurn           += OnChangeTurn;       // start of each turn
            _gm.ObtainActionInfo     += OnObtainActionInfo; // plain-text description per action
            _gm.GameHasEnded         += OnGameEnded;        // singleplayer end
            _gm.MultiplayerGameEnded += OnGameEnded;        // multiplayer end
        }

        private void Unsubscribe()
        {
            ActionLog.LogInfoCreated -= OnLogInfoCreated;
            _gm.ChangeTurn           -= OnChangeTurn;
            _gm.ObtainActionInfo     -= OnObtainActionInfo;
            _gm.GameHasEnded         -= OnGameEnded;
            _gm.MultiplayerGameEnded -= OnGameEnded;
        }

        // ─────────────────────────────────────────────────────────────────────
        // Event handlers
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Primary data source. Fires every time ActionLog.CreateNewLog() commits
        /// a structured LogInfo to its history list.
        /// </summary>
        private void OnLogInfoCreated(LogInfo logInfo, int index)
        {
            if (_disposed || logInfo == null) return;

            MaybeEmitTurnDivider(_gm.TurnCount);
            _pending.Add(FormatLogEntry(logInfo));

            if (_pending.Count >= 20)
                FlushToDisk();
        }

        /// <summary>
        /// Fires for every action with the same plain-English string shown in the
        /// UI's LastActionInfoPresenter. Strips the leading faction-prefix byte
        /// (e.g. "^R") and appends as an indented annotation.
        /// </summary>
        private void OnObtainActionInfo(string actionInfo)
        {
            if (_disposed || string.IsNullOrEmpty(actionInfo)) return;

            // Strip the ^X faction prefix that LastActionInfoPresenter.ParseMessage handles.
            string clean = actionInfo.Length >= 2 && actionInfo[0] == '^'
                ? actionInfo.Substring(2)
                : actionInfo;

            _pending.Add($"           >> \"{clean.Trim()}\"");

            if (_pending.Count >= 20)
                FlushToDisk();
        }

        /// <summary>
        /// Fires at the start of each new turn. Emits a divider and hard-flushes
        /// so at most one turn of data is lost in a crash.
        /// </summary>
        private void OnChangeTurn()
        {
            if (_disposed) return;
            MaybeEmitTurnDivider(_gm.TurnCount);
            FlushToDisk();
        }

        /// <summary>
        /// Fires from GameManager.GameHasEnded (singleplayer) and
        /// GameManager.MultiplayerGameEnded (multiplayer). GameLength is assigned
        /// inside EndGame() just before either event fires, so it is available here.
        /// </summary>
        private void OnGameEnded()
        {
            if (_disposed) return;

            string duration = _gm.GameLength != TimeSpan.Zero
                ? _gm.GameLength.ToString(@"hh\:mm\:ss")
                : "unknown";

            _pending.Add("");
            _pending.Add($"=== GAME OVER === (after {_gm.TurnCount} turns, {duration})");

            // Final scores — only available after CalculateStats() has been called,
            // which happens in GameManager.CheckStars() when a player places their
            // 6th star. If somehow GameHasEnded fires before that we skip this block;
            // the raw action log is still complete.
            if (_gm.StatsCalculated)
                AppendFinalScores(_gm.CalculateStats());

            _pending.Add("");
            FlushToDisk();

            // Unsubscribe so we don't double-write if the event fires again.
            Unsubscribe();
        }

        // ─────────────────────────────────────────────────────────────────────
        // Public API
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>Force an immediate write of all buffered lines. Safe to call any time.</summary>
        public void FlushToDisk()
        {
            if (_pending.Count == 0) return;
            try
            {
                File.AppendAllLines(_filePath, _pending);
                _pending.Clear();
            }
            catch (Exception ex)
            {
                // Log to whatever debug output is available without taking a hard
                // dependency on UnityEngine.Debug.
                System.Diagnostics.Debug.WriteLine($"[GameActionLogger] Write failed: {ex.Message}");
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // File helpers
        // ─────────────────────────────────────────────────────────────────────

        private void WriteHeader()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== SCYTHE GAME LOG ===");
            sb.AppendLine($"Date      : {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            string mode = _gm.IsMultiplayer ? "Multiplayer" :
                          _gm.IsCampaign    ? "Campaign"    :
                          _gm.IsChallenge   ? "Challenge"   : "Singleplayer";
            sb.AppendLine($"Mode      : {mode}");
            if (_gm.IsRanked)     sb.AppendLine("Ranked    : Yes");
            if (_gm.IsHotSeat)    sb.AppendLine("Hot Seat  : Yes");

            sb.AppendLine();
            sb.AppendLine("--- PLAYERS ---");
            AppendPlayerRoster(sb);

            sb.AppendLine();
            sb.AppendLine("--- ACTION LOG ---");
            sb.AppendLine($"{"[Turn]",-7} {"Faction",-14} {"Placement",-8} {"Action",-28} Details");
            sb.AppendLine(new string('-', 86));

            TryWrite(() => File.WriteAllText(_filePath, sb.ToString()));
        }

        private void WriteResumeMarker()
        {
            string marker =
                $"\n--- RESUMED {DateTime.Now:yyyy-MM-dd HH:mm:ss} " +
                $"(turn {_gm.TurnCount}) ---\n";
            TryWrite(() => File.AppendAllText(_filePath, marker));
        }

        private void AppendPlayerRoster(StringBuilder sb)
        {
            int slot = 1;
            foreach (Player p in _gm.players)
            {
                string faction = p.matFaction?.faction.ToString() ?? "Unknown";

                // matPlayer.matType is the canonical PlayerMatType enum.
                // Filter out tutorial/challenge internal types to keep the log readable.
                string mat = p.matPlayer != null
                    ? FriendlyMatName(p.matPlayer.matType)
                    : "Unknown";

                string name = !string.IsNullOrEmpty(p.Name) ? p.Name : "(unnamed)";

                // IsHuman + aiDifficulty gives the full picture for AI players.
                string kind = p.IsHuman
                    ? "Human"
                    : $"AI ({p.aiDifficulty})";

                sb.AppendLine($"  {slot,2}. {faction,-14} / {mat,-18} | {kind,-14} | {name}");
                slot++;
            }
        }

        private void AppendFinalScores(List<PlayerEndGameStats> stats)
        {
            if (stats == null || stats.Count == 0) return;

            _pending.Add("");
            _pending.Add("--- FINAL SCORES ---");
            _pending.Add(
                $"  {"Rank",-5} {"Faction",-14} {"Player",-16} {"Total",6}" +
                $"  Stars  Pop  Terr  Res  Struct  Coins");
            _pending.Add(new string('-', 78));

            for (int i = 0; i < stats.Count; i++)
            {
                var s = stats[i];

                // player reference is present in singleplayer; may be null in the
                // multiplayer-deserialized path, where the standalone fields are used.
                string faction, playerName;
                int    pop;

                if (s.player != null)
                {
                    faction    = s.player.matFaction?.faction.ToString() ?? "?";
                    playerName = !string.IsNullOrEmpty(s.player.Name) ? s.player.Name : s.name;
                    pop        = s.player.Popularity;
                }
                else
                {
                    faction    = ((Faction)s.faction).ToString();
                    playerName = s.name;
                    pop        = s.popularity;
                }

                _pending.Add(
                    $"  {i + 1,-5} {faction,-14} {playerName,-16} {s.totalPoints,6}" +
                    $"  {s.starPoints,5}  {pop,3}  {s.territoryPoints,4}" +
                    $"  {s.resourcePoints,3}  {s.structurePoints,6}  {s.coinPoints,5}");
            }
        }

        private void MaybeEmitTurnDivider(int turnNumber)
        {
            if (turnNumber == _lastTurnSeen) return;
            _lastTurnSeen = turnNumber;
            _pending.Add($"\n--- TURN {turnNumber} ---");
        }

        private static void TryWrite(Action write)
        {
            try   { write(); }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine($"[GameActionLogger] {ex.Message}"); }
        }

        // ─────────────────────────────────────────────────────────────────────
        // Log entry formatting
        // ─────────────────────────────────────────────────────────────────────

        private string FormatLogEntry(LogInfo logInfo)
        {
            string faction   = logInfo.PlayerAssigned.ToString();
            string placement = PlacementLabel(logInfo.ActionPlacement);
            string action    = ActionLabel(logInfo);
            string details   = DetailString(logInfo);

            var sb = new StringBuilder();
            sb.Append($"[{_gm.TurnCount,4}]  {faction,-14} {placement,-8} {action,-28} {details}");

            // Cost lines indented below the main action.
            if (logInfo.PayLogInfos != null)
                foreach (var pay in logInfo.PayLogInfos)
                    sb.Append($"\n           PAY  {ActionLabel(pay),-26} {DetailString(pay)}");

            // Bonus gain lines (enlist bonuses, star rewards, etc.).
            if (logInfo.AdditionalGain != null)
                foreach (var gain in logInfo.AdditionalGain)
                    sb.Append($"\n           GAIN {ActionLabel(gain),-26} {DetailString(gain)}");

            return sb.ToString();
        }

        private static string PlacementLabel(ActionPositionType p) =>
            p == ActionPositionType.Top                ? "TOP"         :
            p == ActionPositionType.Down               ? "BOTTOM"      :
            p == ActionPositionType.Combat             ? "COMBAT"      :
            p == ActionPositionType.BuildingBonus      ? "BLDG BONUS"  :
            p == ActionPositionType.OngoingRecruitBonus? "ENLIST BONUS":
            p == ActionPositionType.Other              ? "OTHER"       :
            p.ToString();

        private static string ActionLabel(LogInfo logInfo)
        {
            if (logInfo is CombatLogInfo c)
            {
                string w = c.Winner?.matFaction?.faction.ToString()   ?? "?";
                string d = c.Defeated?.matFaction?.faction.ToString() ?? "?";
                return $"Combat ({w} beats {d})";
            }
            return logInfo.Type.ToString();
        }

        private static string DetailString(LogInfo logInfo)
        {
            try
            {
                switch (logInfo)
                {
                    case CombatLogInfo combat:
                    {
                        string hex  = combat.Battlefield != null
                            ? $"Hex({combat.Battlefield.posX},{combat.Battlefield.posY})" : "?";
                        string winP = $"W:{combat.WinnerPower.selectedPower}+{combat.WinnerPower.cardsPower}";
                        string defP = $"D:{combat.DefeatedPower.selectedPower}+{combat.DefeatedPower.cardsPower}";
                        string pop  = combat.LostPopularity != 0 ? $" pop-{combat.LostPopularity}" : "";
                        string abl  = (combat.WinnerAbilityUsed || combat.DefeatedAbilityUsed)
                            ? " [ability]" : "";
                        return $"{hex} {winP} vs {defP}{pop}{abl}";
                    }

                    case PayNonboardResourceLogInfo payNon:
                        // Resource = PayType (Coin/Popularity/Power/CombatCard), Amount = short
                        return $"-{payNon.Amount} {payNon.Resource}";

                    case PayResourceLogInfo payRes:
                    {
                        // Resources = Dictionary<ResourceType, int>
                        var parts = new List<string>();
                        foreach (var kv in payRes.Resources)
                            if (kv.Value != 0) parts.Add($"-{kv.Value} {kv.Key}");
                        return parts.Count > 0 ? string.Join(", ", parts) : "";
                    }

                    case WorkerLogInfo wkr:
                        // WorkersAmount + optional Position
                        return wkr.Position != null
                            ? $"+{wkr.WorkersAmount} at Hex({wkr.Position.posX},{wkr.Position.posY})"
                            : $"+{wkr.WorkersAmount}";

                    case SneakPeakLogInfo sneak:
                        return $"Spied: {sneak.SpiedFaction}";

                    case GainNonboardResourceLogInfo gain:
                        return $"+{gain.Amount} {gain.Gained}";

                    case HexUnitResourceLogInfo hexLog
                        when logInfo.Type == LogInfoType.Move ||
                             logInfo.Type == LogInfoType.MoveCoins:
                    {
                        var names = new List<string>();
                        foreach (var u in hexLog.Units)
                            names.Add($"{u.UnitType}#{u.Id}");
                        return names.Count > 0 ? string.Join(", ", names) : "";
                    }

                    case UpgradeLogInfo upg:
                        // TopAction = GainType removed from top row (e.g. Power, Coin)
                        // DownAction = PayType removed from bottom row (e.g. Oil, Metal)
                        // Resource   = ResourceType spent
                        return upg.DownAction != PayType.Coin
                            ? $"Top:-{upg.TopAction} Bot:-{upg.Resource}"
                            : "";

                    case DeployLogInfo dep:
                    {
                        string hex = dep.Position != null
                            ? $"Hex({dep.Position.posX},{dep.Position.posY})" : "";
                        string mech = dep.DeployedMech != null
                            ? $"Mech#{dep.DeployedMech.Id}" : "";
                        string bonus = dep.MechBonus != 0 ? $" +{dep.MechBonus}bonus" : "";
                        return $"{mech} {hex}{bonus}".Trim();
                    }

                    case BuildLogInfo bld:
                    {
                        string hex = bld.Position != null
                            ? $"Hex({bld.Position.posX},{bld.Position.posY})" : "";
                        string building = bld.PlacedBuilding != null
                            ? bld.PlacedBuilding.buildingType.ToString() : "";
                        return $"{building} {hex}".Trim();
                    }

                    case EnlistLogInfo enl:
                        // TypeOfDownAction = which bottom row slot was enlisted
                        // OneTimeBonus     = the one-time GainType reward
                        return enl.TypeOfDownAction != DownActionType.Factory
                            ? $"{enl.TypeOfDownAction} -> +{enl.OneTimeBonus}"
                            : enl.TypeOfDownAction.ToString();

                    case ProductionLogInfo prod:
                    {
                        int total = 0;
                        foreach (var kv in prod.Hexes) total += kv.Value;
                        string mill = prod.MillUsed ? " [mill]" : "";
                        return $"{prod.Hexes.Count} hex(es), {total} produced{mill}";
                    }

                    case StarLogInfo star:
                        return $"Star: {star.GainedStar}";

                    case PassCoinLogInfo pass:
                        // lowercase fields: from, to, amount
                        return $"{pass.from} -> {pass.to}: {pass.amount} coins";

                    default:
                        return "";
                }
            }
            catch { return ""; }
        }

        // ─────────────────────────────────────────────────────────────────────
        // Utility
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Returns a player-facing name for the mat type, stripping out the many
        /// tutorial/challenge internal variants that aren't meaningful in a log.
        /// </summary>
        private static string FriendlyMatName(PlayerMatType mat)
        {
            switch (mat)
            {
                case PlayerMatType.Industrial:   return "Industrial";
                case PlayerMatType.Engineering:  return "Engineering";
                case PlayerMatType.Patriotic:    return "Patriotic";
                case PlayerMatType.Mechanical:   return "Mechanical";
                case PlayerMatType.Agricultural: return "Agricultural";
                case PlayerMatType.Militant:     return "Militant";
                case PlayerMatType.Innovative:   return "Innovative";
                default:                         return mat.ToString();
            }
        }
    }
}
