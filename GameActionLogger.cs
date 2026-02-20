using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x020009E9 RID: 2537
	public class GameActionLogger : IDisposable
	{
		// Token: 0x06004268 RID: 17000
		public GameActionLogger(GameManager gameManager, string gameId = null)
		{
			if (gameManager == null)
			{
				throw new ArgumentNullException("gameManager");
			}
			this._gm = gameManager;
			string text = ((!string.IsNullOrEmpty(gameId) && gameId.Length >= 8) ? gameId.Substring(0, 8) : DateTime.Now.ToString("yyyyMMdd_HHmmss"));
			string tempPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SickleMod", "logs");
			Directory.CreateDirectory(tempPath);
			this._filePath = Path.Combine(tempPath, "ScytheLog_" + text + ".txt");
			if (!File.Exists(this._filePath))
			{
				this.WriteHeader();
			}
			else
			{
				this.WriteResumeMarker();
			}
			this.Subscribe();
		}

		// Token: 0x06004269 RID: 17001
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			this.Unsubscribe();
			this.FlushToDisk();
		}

		// Token: 0x0600426A RID: 17002
		private void Subscribe()
		{
			ActionLog.LogInfoCreated += this.OnLogInfoCreated;
			this._gm.ChangeTurn += this.OnChangeTurn;
			this._gm.ObtainActionInfo += this.OnObtainActionInfo;
			this._gm.GameHasEnded += this.OnGameEnded;
			this._gm.MultiplayerGameEnded += this.OnGameEnded;
		}

		// Token: 0x0600426B RID: 17003
		private void Unsubscribe()
		{
			ActionLog.LogInfoCreated -= this.OnLogInfoCreated;
			this._gm.ChangeTurn -= this.OnChangeTurn;
			this._gm.ObtainActionInfo -= this.OnObtainActionInfo;
			this._gm.GameHasEnded -= this.OnGameEnded;
			this._gm.MultiplayerGameEnded -= this.OnGameEnded;
		}

		// Token: 0x0600426C RID: 17004
		private void OnLogInfoCreated(LogInfo logInfo, int index)
		{
			if (this._disposed || logInfo == null)
			{
				return;
			}
			this.MaybeEmitTurnDivider(this._gm.TurnCount);
			this._pending.Add(this.FormatLogEntry(logInfo));
			if (this._pending.Count >= 20)
			{
				this.FlushToDisk();
			}
		}

		// Token: 0x0600426D RID: 17005
		private void OnObtainActionInfo(string actionInfo)
		{
			if (this._disposed || string.IsNullOrEmpty(actionInfo))
			{
				return;
			}
			string text = ((actionInfo.Length >= 2 && actionInfo[0] == '^') ? actionInfo.Substring(2) : actionInfo);
			this._pending.Add("           >> \"" + text.Trim() + "\"");
			if (this._pending.Count >= 20)
			{
				this.FlushToDisk();
			}
		}

		// Token: 0x0600426E RID: 17006
		private void OnChangeTurn()
		{
			if (this._disposed)
			{
				return;
			}
			this.MaybeEmitTurnDivider(this._gm.TurnCount);
			this.FlushToDisk();
		}

		// Token: 0x0600426F RID: 17007
		private void OnGameEnded()
		{
			if (this._disposed)
			{
				return;
			}
			string text = ((this._gm.GameLength != TimeSpan.Zero) ? this._gm.GameLength.ToString("hh\\:mm\\:ss") : "unknown");
			this._pending.Add("");
			this._pending.Add(string.Format("=== GAME OVER === (after {0} turns, {1})", this._gm.TurnCount, text));
			if (this._gm.StatsCalculated)
			{
				this.AppendFinalScores(this._gm.CalculateStats());
			}
			this._pending.Add("");
			this.FlushToDisk();
			this.Unsubscribe();
		}

		// Token: 0x06004270 RID: 17008
		public void FlushToDisk()
		{
			if (this._pending.Count == 0)
			{
				return;
			}
			try
			{
				File.AppendAllLines(this._filePath, this._pending);
				this._pending.Clear();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06004271 RID: 17009
		private void WriteHeader()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("=== SCYTHE GAME LOG ===");
			sb.AppendLine(string.Format("Date      : {0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
			string text = (this._gm.IsMultiplayer ? "Multiplayer" : (this._gm.IsCampaign ? "Campaign" : (this._gm.IsChallenge ? "Challenge" : "Singleplayer")));
			sb.AppendLine("Mode      : " + text);
			if (this._gm.IsRanked)
			{
				sb.AppendLine("Ranked    : Yes");
			}
			if (this._gm.IsHotSeat)
			{
				sb.AppendLine("Hot Seat  : Yes");
			}
			sb.AppendLine();
			sb.AppendLine("--- PLAYERS ---");
			this.AppendPlayerRoster(sb);
			sb.AppendLine();
			sb.AppendLine("--- ACTION LOG ---");
			sb.AppendLine(string.Format("{0,-7} {1,-14} {2,-8} {3,-28} Details", new object[] { "[Turn]", "Faction", "Placement", "Action" }));
			sb.AppendLine(new string('-', 86));
			GameActionLogger.TryWrite(delegate
			{
				File.WriteAllText(this._filePath, sb.ToString());
			});
		}

		// Token: 0x06004272 RID: 17010
		private void WriteResumeMarker()
		{
			string marker = string.Format("\n--- RESUMED {0:yyyy-MM-dd HH:mm:ss} ", DateTime.Now) + string.Format("(turn {0}) ---\n", this._gm.TurnCount);
			GameActionLogger.TryWrite(delegate
			{
				File.AppendAllText(this._filePath, marker);
			});
		}

		// Token: 0x06004273 RID: 17011
		private void AppendPlayerRoster(StringBuilder sb)
		{
			int num = 1;
			foreach (Player player in this._gm.players)
			{
				MatFaction matFaction = player.matFaction;
				string text = ((matFaction != null) ? matFaction.faction.ToString() : null) ?? "Unknown";
				string text2 = ((player.matPlayer != null) ? GameActionLogger.FriendlyMatName(player.matPlayer.matType) : "Unknown");
				string text3 = ((!string.IsNullOrEmpty(player.Name)) ? player.Name : "(unnamed)");
				string text4 = (player.IsHuman ? "Human" : string.Format("AI ({0})", player.aiDifficulty));
				sb.AppendLine(string.Format("  {0,2}. {1,-14} / {2,-18} | {3,-14} | {4}", new object[] { num, text, text2, text4, text3 }));
				num++;
			}
		}

		// Token: 0x06004274 RID: 17012
		private void AppendFinalScores(List<PlayerEndGameStats> stats)
		{
			if (stats == null || stats.Count == 0)
			{
				return;
			}
			this._pending.Add("");
			this._pending.Add("--- FINAL SCORES ---");
			this._pending.Add(string.Format("  {0,-5} {1,-14} {2,-16} {3,6}", new object[] { "Rank", "Faction", "Player", "Total" }) + "  Stars  Pop  Terr  Res  Struct  Coins");
			this._pending.Add(new string('-', 78));
			for (int i = 0; i < stats.Count; i++)
			{
				PlayerEndGameStats playerEndGameStats = stats[i];
				string text;
				string text2;
				int num;
				if (playerEndGameStats.player != null)
				{
					MatFaction matFaction = playerEndGameStats.player.matFaction;
					text = ((matFaction != null) ? matFaction.faction.ToString() : null) ?? "?";
					text2 = ((!string.IsNullOrEmpty(playerEndGameStats.player.Name)) ? playerEndGameStats.player.Name : playerEndGameStats.name);
					num = playerEndGameStats.player.Popularity;
				}
				else
				{
					Faction faction = (Faction)playerEndGameStats.faction;
					text = faction.ToString();
					text2 = playerEndGameStats.name;
					num = playerEndGameStats.popularity;
				}
				this._pending.Add(string.Format("  {0,-5} {1,-14} {2,-16} {3,6}", new object[]
				{
					i + 1,
					text,
					text2,
					playerEndGameStats.totalPoints
				}) + string.Format("  {0,5}  {1,3}  {2,4}", playerEndGameStats.starPoints, num, playerEndGameStats.territoryPoints) + string.Format("  {0,3}  {1,6}  {2,5}", playerEndGameStats.resourcePoints, playerEndGameStats.structurePoints, playerEndGameStats.coinPoints));
			}
		}

		// Token: 0x06004275 RID: 17013
		private void MaybeEmitTurnDivider(int turnNumber)
		{
			if (turnNumber == this._lastTurnSeen)
			{
				return;
			}
			this._lastTurnSeen = turnNumber;
			this._pending.Add(string.Format("\n--- TURN {0} ---", turnNumber));
		}

		// Token: 0x06004276 RID: 17014
		private static void TryWrite(Action write)
		{
			try
			{
				write();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06004277 RID: 17015
		private string FormatLogEntry(LogInfo logInfo)
		{
			string text = logInfo.PlayerAssigned.ToString();
			string text2 = GameActionLogger.PlacementLabel(logInfo.ActionPlacement);
			string text3 = GameActionLogger.ActionLabel(logInfo);
			string text4 = GameActionLogger.DetailString(logInfo);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("[{0,4}]  {1,-14} {2,-8} {3,-28} {4}", new object[]
			{
				this._gm.TurnCount,
				text,
				text2,
				text3,
				text4
			}));
			if (logInfo.PayLogInfos != null)
			{
				foreach (LogInfo logInfo2 in logInfo.PayLogInfos)
				{
					stringBuilder.Append(string.Format("\n           PAY  {0,-26} {1}", GameActionLogger.ActionLabel(logInfo2), GameActionLogger.DetailString(logInfo2)));
				}
			}
			if (logInfo.AdditionalGain != null)
			{
				foreach (LogInfo logInfo3 in logInfo.AdditionalGain)
				{
					stringBuilder.Append(string.Format("\n           GAIN {0,-26} {1}", GameActionLogger.ActionLabel(logInfo3), GameActionLogger.DetailString(logInfo3)));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06004278 RID: 17016
		private static string PlacementLabel(ActionPositionType p)
		{
			if (p == ActionPositionType.Top)
			{
				return "TOP";
			}
			if (p == ActionPositionType.Down)
			{
				return "BOTTOM";
			}
			if (p == ActionPositionType.Combat)
			{
				return "COMBAT";
			}
			if (p == ActionPositionType.BuildingBonus)
			{
				return "BLDG BONUS";
			}
			if (p == ActionPositionType.OngoingRecruitBonus)
			{
				return "ENLIST BONUS";
			}
			if (p != ActionPositionType.Other)
			{
				return p.ToString();
			}
			return "OTHER";
		}

		// Token: 0x06004279 RID: 17017
		private static string ActionLabel(LogInfo logInfo)
		{
			CombatLogInfo combatLogInfo = logInfo as CombatLogInfo;
			if (combatLogInfo != null)
			{
				Player winner = combatLogInfo.Winner;
				string text;
				if (winner == null)
				{
					text = null;
				}
				else
				{
					MatFaction matFaction = winner.matFaction;
					text = ((matFaction != null) ? matFaction.faction.ToString() : null);
				}
				string text2 = text ?? "?";
				Player defeated = combatLogInfo.Defeated;
				string text3;
				if (defeated == null)
				{
					text3 = null;
				}
				else
				{
					MatFaction matFaction2 = defeated.matFaction;
					text3 = ((matFaction2 != null) ? matFaction2.faction.ToString() : null);
				}
				string text4 = text3 ?? "?";
				return string.Concat(new string[] { "Combat (", text2, " beats ", text4, ")" });
			}
			return logInfo.Type.ToString();
		}

		// Token: 0x0600427A RID: 17018
		private static string DetailString(LogInfo logInfo)
		{
			string text8;
			try
			{
				CombatLogInfo combatLogInfo = logInfo as CombatLogInfo;
				if (combatLogInfo == null)
				{
					PayNonboardResourceLogInfo payNonboardResourceLogInfo = logInfo as PayNonboardResourceLogInfo;
					if (payNonboardResourceLogInfo == null)
					{
						PayResourceLogInfo payResourceLogInfo = logInfo as PayResourceLogInfo;
						if (payResourceLogInfo == null)
						{
							WorkerLogInfo workerLogInfo = logInfo as WorkerLogInfo;
							if (workerLogInfo == null)
							{
								SneakPeakLogInfo sneakPeakLogInfo = logInfo as SneakPeakLogInfo;
								if (sneakPeakLogInfo == null)
								{
									GainNonboardResourceLogInfo gainNonboardResourceLogInfo = logInfo as GainNonboardResourceLogInfo;
									if (gainNonboardResourceLogInfo == null)
									{
										HexUnitResourceLogInfo hexUnitResourceLogInfo = logInfo as HexUnitResourceLogInfo;
										if (hexUnitResourceLogInfo == null)
										{
											UpgradeLogInfo upgradeLogInfo = logInfo as UpgradeLogInfo;
											if (upgradeLogInfo != null)
											{
												return (upgradeLogInfo.DownAction != PayType.Coin) ? string.Format("Top:-{0} Bot:-{1}", upgradeLogInfo.TopAction, upgradeLogInfo.Resource) : "";
											}
											DeployLogInfo deployLogInfo = logInfo as DeployLogInfo;
											if (deployLogInfo != null)
											{
												string text = ((deployLogInfo.Position != null) ? string.Format("Hex({0},{1})", deployLogInfo.Position.posX, deployLogInfo.Position.posY) : "");
												string text14 = ((deployLogInfo.DeployedMech != null) ? string.Format("Mech#{0}", deployLogInfo.DeployedMech.Id) : "");
												string text2 = ((deployLogInfo.MechBonus != 0) ? string.Format(" +{0}bonus", deployLogInfo.MechBonus) : "");
												return (text14 + " " + text + text2).Trim();
											}
											BuildLogInfo buildLogInfo = logInfo as BuildLogInfo;
											if (buildLogInfo != null)
											{
												string text3 = ((buildLogInfo.Position != null) ? string.Format("Hex({0},{1})", buildLogInfo.Position.posX, buildLogInfo.Position.posY) : "");
												return (((buildLogInfo.PlacedBuilding != null) ? buildLogInfo.PlacedBuilding.buildingType.ToString() : "") + " " + text3).Trim();
											}
											EnlistLogInfo enlistLogInfo = logInfo as EnlistLogInfo;
											if (enlistLogInfo != null)
											{
												return (enlistLogInfo.TypeOfDownAction != DownActionType.Factory) ? string.Format("{0} -> +{1}", enlistLogInfo.TypeOfDownAction, enlistLogInfo.OneTimeBonus) : enlistLogInfo.TypeOfDownAction.ToString();
											}
											ProductionLogInfo productionLogInfo = logInfo as ProductionLogInfo;
											if (productionLogInfo != null)
											{
												int num = 0;
												foreach (KeyValuePair<GameHex, int> keyValuePair in productionLogInfo.Hexes)
												{
													num += keyValuePair.Value;
												}
												string text4 = (productionLogInfo.MillUsed ? " [mill]" : "");
												return string.Format("{0} hex(es), {1} produced{2}", productionLogInfo.Hexes.Count, num, text4);
											}
											StarLogInfo starLogInfo = logInfo as StarLogInfo;
											if (starLogInfo != null)
											{
												return string.Format("Star: {0}", starLogInfo.GainedStar);
											}
											PassCoinLogInfo passCoinLogInfo = logInfo as PassCoinLogInfo;
											if (passCoinLogInfo != null)
											{
												return string.Format("{0} -> {1}: {2} coins", passCoinLogInfo.from, passCoinLogInfo.to, passCoinLogInfo.amount);
											}
										}
										else if (logInfo.Type == LogInfoType.Move || logInfo.Type == LogInfoType.MoveCoins)
										{
											if (hexUnitResourceLogInfo.Hexes.Count == 0)
											{
												return "";
											}
											List<string> list = new List<string>();
											foreach (Unit unit in hexUnitResourceLogInfo.Units)
											{
												list.Add(string.Format("{0}#{1}", unit.UnitType, unit.Id));
											}
											string text5 = ((list.Count > 0) ? (string.Join(", ", list) + "  ") : "");
											if (hexUnitResourceLogInfo.Hexes.Count == 1)
											{
												return text5 + GameActionLogger.HexLabel(hexUnitResourceLogInfo.Hexes[0]);
											}
											string text6 = GameActionLogger.HexLabel(hexUnitResourceLogInfo.Hexes[0]);
											List<string> list2 = new List<string>();
											for (int i = 1; i < hexUnitResourceLogInfo.Hexes.Count; i++)
											{
												list2.Add(GameActionLogger.HexLabel(hexUnitResourceLogInfo.Hexes[i]));
											}
											string text7 = string.Join(" & ", list2);
											return text5 + text6 + " → " + text7;
										}
										text8 = "";
									}
									else
									{
										text8 = string.Format("+{0} {1}", gainNonboardResourceLogInfo.Amount, gainNonboardResourceLogInfo.Gained);
									}
								}
								else
								{
									text8 = string.Format("Spied: {0}", sneakPeakLogInfo.SpiedFaction);
								}
							}
							else
							{
								text8 = ((workerLogInfo.Position != null) ? string.Format("+{0} at Hex({1},{2})", workerLogInfo.WorkersAmount, workerLogInfo.Position.posX, workerLogInfo.Position.posY) : string.Format("+{0}", workerLogInfo.WorkersAmount));
							}
						}
						else
						{
							List<string> list3 = new List<string>();
							foreach (KeyValuePair<ResourceType, int> keyValuePair2 in payResourceLogInfo.Resources)
							{
								if (keyValuePair2.Value != 0)
								{
									list3.Add(string.Format("-{0} {1}", keyValuePair2.Value, keyValuePair2.Key));
								}
							}
							text8 = ((list3.Count > 0) ? string.Join(", ", list3) : "");
						}
					}
					else
					{
						text8 = string.Format("-{0} {1}", payNonboardResourceLogInfo.Amount, payNonboardResourceLogInfo.Resource);
					}
				}
				else
				{
					string text9 = ((combatLogInfo.Battlefield != null) ? string.Format("Hex({0},{1})", combatLogInfo.Battlefield.posX, combatLogInfo.Battlefield.posY) : "?");
					string text10 = string.Format("W:{0}+{1}", combatLogInfo.WinnerPower.selectedPower, combatLogInfo.WinnerPower.cardsPower);
					string text11 = string.Format("D:{0}+{1}", combatLogInfo.DefeatedPower.selectedPower, combatLogInfo.DefeatedPower.cardsPower);
					string text12 = ((combatLogInfo.LostPopularity != 0) ? string.Format(" pop-{0}", combatLogInfo.LostPopularity) : "");
					string text13 = ((combatLogInfo.WinnerAbilityUsed || combatLogInfo.DefeatedAbilityUsed) ? " [ability]" : "");
					text8 = string.Concat(new string[] { text9, " ", text10, " vs ", text11, text12, text13 });
				}
			}
			catch
			{
				text8 = "";
			}
			return text8;
		}

		// Token: 0x0600427B RID: 17019
		private static string FriendlyMatName(PlayerMatType mat)
		{
			switch (mat)
			{
			case PlayerMatType.Industrial:
				return "Industrial";
			case PlayerMatType.Engineering:
				return "Engineering";
			case PlayerMatType.Patriotic:
				return "Patriotic";
			case PlayerMatType.Mechanical:
				return "Mechanical";
			case PlayerMatType.Agricultural:
				return "Agricultural";
			case PlayerMatType.Militant:
				return "Militant";
			case PlayerMatType.Innovative:
				return "Innovative";
			default:
				return mat.ToString();
			}
		}

		// Token: 0x0600427C RID: 17020
		private static string HexLabel(GameHex hex)
		{
			if (hex == null)
			{
				return "?";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("({0},{1})", hex.posX, hex.posY));
			if (hex.hexType == HexType.capital && hex.factionBase != Faction.Polania)
			{
				stringBuilder.Append(string.Format("[{0}-capital]", hex.factionBase));
			}
			else
			{
				stringBuilder.Append(string.Format("[{0}]", hex.hexType));
			}
			if (hex.hasTunnel)
			{
				stringBuilder.Append("[tunnel]");
			}
			if (hex.hasEncounter && !hex.encounterUsed)
			{
				stringBuilder.Append("[encounter]");
			}
			if (hex.Building != null)
			{
				stringBuilder.Append(string.Format("[{0}]", hex.Building.buildingType));
			}
			if (hex.Token != null)
			{
				Player owner = hex.Token.Owner;
				string text;
				if (owner == null)
				{
					text = null;
				}
				else
				{
					MatFaction matFaction = owner.matFaction;
					text = ((matFaction != null) ? matFaction.faction.ToString() : null);
				}
				string text2 = text ?? "?";
				bool flag = hex.Token is TrapToken;
				stringBuilder.Append(flag ? ("[trap:" + text2 + "]") : ("[flag:" + text2 + "]"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04003345 RID: 13125
		private readonly GameManager _gm;

		// Token: 0x04003346 RID: 13126
		private readonly string _filePath;

		// Token: 0x04003347 RID: 13127
		private readonly List<string> _pending = new List<string>();

		// Token: 0x04003348 RID: 13128
		private bool _disposed;

		// Token: 0x04003349 RID: 13129
		private int _lastTurnSeen = -1;
	}
}
