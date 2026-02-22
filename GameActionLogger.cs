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
			string faction = logInfo.PlayerAssigned.ToString();
			StringBuilder sb = new StringBuilder();
			if (logInfo.ActionPlacement == ActionPositionType.Other)
			{
				string otherDesc = GameActionLogger.OtherActionDescription(logInfo);
				sb.Append(string.Format("[{0,4}]  {1,-14} {2}", this._gm.TurnCount, faction, otherDesc));
			}
			else
			{
				string placement = GameActionLogger.PlacementLabel(logInfo.ActionPlacement);
				string action = GameActionLogger.ActionLabel(logInfo);
				string detail = GameActionLogger.DetailString(logInfo);
				if (logInfo.Type == LogInfoType.TradeResources && logInfo.AdditionalGain != null)
				{
					List<string> gained = new List<string>();
					foreach (LogInfo logInfo2 in logInfo.AdditionalGain)
					{
						GainNonboardResourceLogInfo res = logInfo2 as GainNonboardResourceLogInfo;
						if (res != null && res.Amount > 0)
						{
							gained.Add(string.Format("+{0} {1}", res.Amount, res.Gained));
						}
					}
					if (gained.Count > 0)
					{
						detail = string.Join(", ", gained);
					}
				}
				sb.Append(string.Format("[{0,4}]  {1,-14} {2,-8} {3,-28} {4}", new object[]
				{
					this._gm.TurnCount,
					faction,
					placement,
					action,
					detail
				}));
				if (logInfo.PayLogInfos != null)
				{
					foreach (LogInfo pay in logInfo.PayLogInfos)
					{
						sb.Append(string.Format("\n           PAY  {0,-26} {1}", GameActionLogger.ActionLabel(pay), GameActionLogger.DetailString(pay)));
					}
				}
				if (logInfo.AdditionalGain != null)
				{
					foreach (LogInfo gain in logInfo.AdditionalGain)
					{
						if (logInfo.Type != LogInfoType.TradeResources || !(gain is GainNonboardResourceLogInfo))
						{
							sb.Append(string.Format("\n           GAIN {0,-26} {1}", GameActionLogger.ActionLabel(gain), GameActionLogger.DetailString(gain)));
						}
					}
				}
			}
			return sb.ToString();
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
			string text7;
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
												return (upgradeLogInfo.DownAction != PayType.Coin) ? string.Format("Upgrade: {0}, {1}", upgradeLogInfo.TopAction, GameActionLogger.UpgradeBottomName(upgradeLogInfo.Resource)) : "";
											}
											DeployLogInfo deployLogInfo = logInfo as DeployLogInfo;
											if (deployLogInfo != null)
											{
												string text = ((deployLogInfo.Position != null) ? string.Format("Hex({0},{1})", deployLogInfo.Position.posX, deployLogInfo.Position.posY) : "");
												string text13 = ((deployLogInfo.DeployedMech != null) ? GameActionLogger.MechName(deployLogInfo.DeployedMech) : "");
												string text2 = ((deployLogInfo.MechBonus != 0) ? string.Format(" +{0}bonus", deployLogInfo.MechBonus) : "");
												return (text13 + " " + text + text2).Trim();
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
												List<string> hexDetails = new List<string>();
												foreach (KeyValuePair<GameHex, int> keyValuePair in productionLogInfo.Hexes)
												{
													num += keyValuePair.Value;
													GameHex hex = keyValuePair.Key;
													string resource = GameActionLogger.HexTypeResource(hex.hexType);
													string millTag = ((productionLogInfo.MillUsed && hex.Building != null && hex.Building.buildingType == BuildingType.Mill) ? "[mill]" : "");
													hexDetails.Add(string.Format("({0},{1}){2}:{3}x{4}", new object[] { hex.posX, hex.posY, millTag, resource, keyValuePair.Value }));
												}
												return string.Format("{0} produced [{1}]", num, string.Join(", ", hexDetails));
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
												Mech unitAsMech = unit as Mech;
												string unitLabel = ((unitAsMech != null) ? GameActionLogger.MechName(unitAsMech) : string.Format("{0}#{1}", unit.UnitType, unit.Id));
												list.Add(unitLabel);
											}
											string text4 = ((list.Count > 0) ? (string.Join(", ", list) + "  ") : "");
											if (hexUnitResourceLogInfo.Hexes.Count == 1)
											{
												return text4 + GameActionLogger.HexLabel(hexUnitResourceLogInfo.Hexes[0]);
											}
											string text5 = GameActionLogger.HexLabel(hexUnitResourceLogInfo.Hexes[0]);
											List<string> list2 = new List<string>();
											for (int i = 1; i < hexUnitResourceLogInfo.Hexes.Count; i++)
											{
												list2.Add(GameActionLogger.HexLabel(hexUnitResourceLogInfo.Hexes[i]));
											}
											string text6 = string.Join(" & ", list2);
											return text4 + text5 + " → " + text6;
										}
										text7 = "";
									}
									else
									{
										text7 = string.Format("+{0} {1}", gainNonboardResourceLogInfo.Amount, gainNonboardResourceLogInfo.Gained);
									}
								}
								else
								{
									text7 = string.Format("Spied: {0}", sneakPeakLogInfo.SpiedFaction);
								}
							}
							else
							{
								text7 = ((workerLogInfo.Position != null) ? string.Format("+{0} at Hex({1},{2})", workerLogInfo.WorkersAmount, workerLogInfo.Position.posX, workerLogInfo.Position.posY) : string.Format("+{0}", workerLogInfo.WorkersAmount));
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
							text7 = ((list3.Count > 0) ? string.Join(", ", list3) : "");
						}
					}
					else
					{
						text7 = string.Format("-{0} {1}", payNonboardResourceLogInfo.Amount, payNonboardResourceLogInfo.Resource);
					}
				}
				else
				{
					string text8 = ((combatLogInfo.Battlefield != null) ? string.Format("Hex({0},{1})", combatLogInfo.Battlefield.posX, combatLogInfo.Battlefield.posY) : "?");
					string text9 = string.Format("W:{0}+{1}", combatLogInfo.WinnerPower.selectedPower, combatLogInfo.WinnerPower.cardsPower);
					string text10 = string.Format("D:{0}+{1}", combatLogInfo.DefeatedPower.selectedPower, combatLogInfo.DefeatedPower.cardsPower);
					string text11 = ((combatLogInfo.LostPopularity != 0) ? string.Format(" pop-{0}", combatLogInfo.LostPopularity) : "");
					string text12 = ((combatLogInfo.WinnerAbilityUsed || combatLogInfo.DefeatedAbilityUsed) ? " [ability]" : "");
					text7 = string.Concat(new string[] { text8, " ", text9, " vs ", text10, text11, text12 });
				}
			}
			catch
			{
				text7 = "";
			}
			return text7;
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

		// Token: 0x06004291 RID: 17041
		private static string OtherActionDescription(LogInfo logInfo)
		{
			if (logInfo.IsEncounter)
			{
				List<string> pays = new List<string>();
				List<string> gains = new List<string>();
				if (logInfo.PayLogInfos != null)
				{
					foreach (LogInfo p in logInfo.PayLogInfos)
					{
						pays.Add(GameActionLogger.DetailString(p));
					}
				}
				if (logInfo.AdditionalGain != null)
				{
					foreach (LogInfo g in logInfo.AdditionalGain)
					{
						gains.Add(GameActionLogger.GainSummary(g));
					}
				}
				string payPart = ((pays.Count > 0) ? string.Join(", ", pays) : "free");
				string gainPart = ((gains.Count > 0) ? string.Join(", ", gains) : "");
				string separator = ((gainPart.Length > 0) ? " → " : "");
				return string.Format("Encounter: {0}{1}{2}", payPart, separator, gainPart);
			}
			StarLogInfo starLog = logInfo as StarLogInfo;
			if (starLog != null)
			{
				return string.Format("★ Star: {0} ({1}/6)", starLog.GainedStar, starLog.starsUnlocked);
			}
			FactoryLogInfo factoryLog = logInfo as FactoryLogInfo;
			if (factoryLog != null && logInfo.Type == LogInfoType.FactoryCardGain)
			{
				return string.Format("Factory Card #{0}", factoryLog.GainedFactoryCard.CardId);
			}
			if (logInfo.Type == LogInfoType.FactoryTopAction)
			{
				List<string> pays2 = new List<string>();
				List<string> gains2 = new List<string>();
				if (logInfo.PayLogInfos != null)
				{
					foreach (LogInfo p2 in logInfo.PayLogInfos)
					{
						pays2.Add(GameActionLogger.DetailString(p2));
					}
				}
				if (logInfo.AdditionalGain != null)
				{
					foreach (LogInfo g2 in logInfo.AdditionalGain)
					{
						gains2.Add(GameActionLogger.GainSummary(g2));
					}
				}
				string payPart2 = ((pays2.Count > 0) ? string.Join(", ", pays2) : "");
				string gainPart2 = ((gains2.Count > 0) ? string.Join(", ", gains2) : "");
				string middle = ((payPart2.Length > 0 && gainPart2.Length > 0) ? " → " : "");
				return string.Format("Factory: {0}{1}{2}", payPart2, middle, gainPart2);
			}
			DeployLogInfo deployLog = logInfo as DeployLogInfo;
			if (deployLog != null)
			{
				string mechName = ((deployLog.DeployedMech != null) ? GameActionLogger.MechName(deployLog.DeployedMech) : "?");
				string pos = ((deployLog.Position != null) ? string.Format(" at ({0},{1})", deployLog.Position.posX, deployLog.Position.posY) : "");
				string bonus = ((deployLog.MechBonus != 0) ? string.Format(" +{0}bonus", deployLog.MechBonus) : "");
				return string.Format("Deploy {0}{1}{2}", mechName, pos, bonus);
			}
			if (logInfo.Type == LogInfoType.TradeResources)
			{
				List<string> pays3 = new List<string>();
				List<string> gains3 = new List<string>();
				if (logInfo.PayLogInfos != null)
				{
					foreach (LogInfo p3 in logInfo.PayLogInfos)
					{
						pays3.Add(GameActionLogger.DetailString(p3));
					}
				}
				if (logInfo.AdditionalGain != null)
				{
					foreach (LogInfo logInfo2 in logInfo.AdditionalGain)
					{
						GainNonboardResourceLogInfo res = logInfo2 as GainNonboardResourceLogInfo;
						if (res != null && res.Amount > 0)
						{
							gains3.Add(string.Format("+{0} {1}", res.Amount, res.Gained));
						}
					}
				}
				string payPart3 = ((pays3.Count > 0) ? string.Join(", ", pays3) : "");
				string gainPart3 = ((gains3.Count > 0) ? string.Join(", ", gains3) : "");
				string middle2 = ((payPart3.Length > 0 && gainPart3.Length > 0) ? " → " : "");
				return string.Format("Trade: {0}{1}{2}", payPart3, middle2, gainPart3);
			}
			GainNonboardResourceLogInfo gainLog = logInfo as GainNonboardResourceLogInfo;
			if (gainLog != null)
			{
				return string.Format("+{0} {1}", gainLog.Amount, gainLog.Gained);
			}
			return GameActionLogger.ActionLabel(logInfo) + " " + GameActionLogger.DetailString(logInfo);
		}

		// Token: 0x06004292 RID: 17042
		private static string GainSummary(LogInfo g)
		{
			GainNonboardResourceLogInfo nr = g as GainNonboardResourceLogInfo;
			if (nr != null)
			{
				return string.Format("+{0} {1}", nr.Amount, nr.Gained);
			}
			WorkerLogInfo wl = g as WorkerLogInfo;
			if (wl != null)
			{
				if (wl.Position == null)
				{
					return string.Format("+{0} workers", wl.WorkersAmount);
				}
				return string.Format("+{0} workers at ({1},{2})", wl.WorkersAmount, wl.Position.posX, wl.Position.posY);
			}
			else
			{
				DeployLogInfo dl = g as DeployLogInfo;
				if (dl != null)
				{
					return "Deploy " + ((dl.DeployedMech != null) ? GameActionLogger.MechName(dl.DeployedMech) : "mech");
				}
				BuildLogInfo bl = g as BuildLogInfo;
				if (bl != null)
				{
					if (bl.PlacedBuilding == null)
					{
						return "Build";
					}
					return "Build " + bl.PlacedBuilding.buildingType.ToString();
				}
				else
				{
					UpgradeLogInfo ul = g as UpgradeLogInfo;
					if (ul != null)
					{
						if (ul.DownAction == PayType.Coin)
						{
							return "Upgrade";
						}
						return string.Format("Upgrade: {0}, {1}", ul.TopAction, GameActionLogger.UpgradeBottomName(ul.Resource));
					}
					else
					{
						EnlistLogInfo el = g as EnlistLogInfo;
						if (el == null)
						{
							return GameActionLogger.ActionLabel(g);
						}
						if (el.TypeOfDownAction == DownActionType.Factory)
						{
							return "Enlist";
						}
						return string.Format("Enlist {0}→+{1}", el.TypeOfDownAction, el.OneTimeBonus);
					}
				}
			}
		}

		// Token: 0x06004297 RID: 17047
		private static string UpgradeBottomName(ResourceType resource)
		{
			switch (resource)
			{
			case ResourceType.oil:
				return "Upgrade";
			case ResourceType.metal:
				return "Mech";
			case ResourceType.food:
				return "Recruit";
			case ResourceType.wood:
				return "Building";
			default:
				return resource.ToString();
			}
		}

		// Token: 0x06004298 RID: 17048
		private static string HexTypeResource(HexType hexType)
		{
			switch (hexType)
			{
			case HexType.mountain:
				return "metal";
			case HexType.forest:
				return "wood";
			case (HexType)3:
				break;
			case HexType.farm:
				return "food";
			default:
				if (hexType == HexType.tundra)
				{
					return "oil";
				}
				if (hexType == HexType.village)
				{
					return "workers";
				}
				break;
			}
			return hexType.ToString();
		}

		// Token: 0x06004299 RID: 17049
		private static string MechName(Mech mech)
		{
			try
			{
				if (mech != null && mech.Owner != null && mech.Owner.matFaction != null && mech.Owner.matFaction.abilities != null && mech.Id < mech.Owner.matFaction.abilities.Count)
				{
					return mech.Owner.matFaction.abilities[mech.Id].ToString();
				}
			}
			catch
			{
			}
			return string.Format("Mech#{0}", (mech != null) ? mech.Id.ToString() : "?");
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
