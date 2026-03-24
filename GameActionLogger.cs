using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x020009F6 RID: 2550
	public class GameActionLogger : IDisposable
	{
		// Token: 0x060042AE RID: 17070 RVA: 0x0016B15C File Offset: 0x0016935C
		public GameActionLogger(GameManager gameManager, string gameId = null)
		{
			if (gameManager == null)
			{
				throw new ArgumentNullException("gameManager");
			}
			this._gm = gameManager;
			string text = ((!string.IsNullOrEmpty(gameId) && gameId.Length >= 8) ? gameId.Substring(0, 8) : DateTime.Now.ToString("yyyyMMdd_HHmmss"));
			string text2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SickleMod", "logs");
			Directory.CreateDirectory(text2);
			this._filePath = Path.Combine(text2, "ScytheLog_" + text + ".txt");
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

		// Token: 0x060042AF RID: 17071 RVA: 0x000527E9 File Offset: 0x000509E9
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

		// Token: 0x060042B0 RID: 17072 RVA: 0x0016B220 File Offset: 0x00169420
		private void Subscribe()
		{
			ActionLog.LogInfoCreated += this.OnLogInfoCreated;
			this._gm.ChangeTurn += this.OnChangeTurn;
			this._gm.ObtainActionInfo += this.OnObtainActionInfo;
			this._gm.GameHasEnded += this.OnGameEnded;
			this._gm.MultiplayerGameEnded += this.OnGameEnded;
		}

		// Token: 0x060042B1 RID: 17073 RVA: 0x0016B29C File Offset: 0x0016949C
		private void Unsubscribe()
		{
			ActionLog.LogInfoCreated -= this.OnLogInfoCreated;
			this._gm.ChangeTurn -= this.OnChangeTurn;
			this._gm.ObtainActionInfo -= this.OnObtainActionInfo;
			this._gm.GameHasEnded -= this.OnGameEnded;
			this._gm.MultiplayerGameEnded -= this.OnGameEnded;
		}

		// Token: 0x060042B2 RID: 17074 RVA: 0x0016B318 File Offset: 0x00169518
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

		// Token: 0x060042B3 RID: 17075 RVA: 0x0016B36C File Offset: 0x0016956C
		private void OnObtainActionInfo(string actionInfo)
		{
			if (this._disposed || string.IsNullOrEmpty(actionInfo))
			{
				return;
			}
			string playerName = "";
			string text = actionInfo;
			if (actionInfo.Length >= 2 && actionInfo[0] == '^')
			{
				char c = actionInfo[1];
				switch (c)
				{
				case 'A': playerName = "Albion"; break;
				case 'C': playerName = "Crimea"; break;
				case 'N': playerName = "Nordic"; break;
				case 'P': playerName = "Polania"; break;
				case 'R': playerName = "Rusviet"; break;
				case 'S': playerName = "Saxony"; break;
				case 'T': playerName = "Togawa"; break;
				}
				text = actionInfo.Substring(2);
			}
			if (string.IsNullOrEmpty(playerName))
			{
				this._pending.Add("           >> \"" + text.Trim() + "\"");
			}
			else
			{
				this._pending.Add(string.Format("[{0,4}]  {1,-14} >> \"{2}\"", this._gm.TurnCount, playerName, text.Trim()));
			}
			if (this._pending.Count >= 20)
			{
				this.FlushToDisk();
			}
		}

		// Token: 0x060042B4 RID: 17076 RVA: 0x00052807 File Offset: 0x00050A07
		private void OnChangeTurn()
		{
			if (this._disposed)
			{
				return;
			}
			this.MaybeEmitTurnDivider(this._gm.TurnCount);
			this.FlushToDisk();
		}

		// Token: 0x060042B5 RID: 17077 RVA: 0x0016B3E0 File Offset: 0x001695E0
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

		// Token: 0x060042B6 RID: 17078 RVA: 0x0016B49C File Offset: 0x0016969C
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

		// Token: 0x060042B7 RID: 17079 RVA: 0x0016B4EC File Offset: 0x001696EC
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

		// Token: 0x060042B8 RID: 17080 RVA: 0x0016B680 File Offset: 0x00169880
		private void WriteResumeMarker()
		{
			string marker = string.Format("\n--- RESUMED {0:yyyy-MM-dd HH:mm:ss} ", DateTime.Now) + string.Format("(turn {0}) ---\n", this._gm.TurnCount);
			GameActionLogger.TryWrite(delegate
			{
				File.AppendAllText(this._filePath, marker);
			});
		}

		// Token: 0x060042B9 RID: 17081 RVA: 0x0016B6E4 File Offset: 0x001698E4
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

		// Token: 0x060042BA RID: 17082 RVA: 0x0016B804 File Offset: 0x00169A04
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

		// Token: 0x060042BB RID: 17083 RVA: 0x00052829 File Offset: 0x00050A29
		private void MaybeEmitTurnDivider(int turnNumber)
		{
			if (turnNumber == this._lastTurnSeen)
			{
				return;
			}
			this._lastTurnSeen = turnNumber;
			this._pending.Add(string.Format("\n--- TURN {0} ---", turnNumber));
		}

		// Token: 0x060042BC RID: 17084 RVA: 0x0016B9DC File Offset: 0x00169BDC
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

		// Token: 0x060042BD RID: 17085 RVA: 0x0016BA04 File Offset: 0x00169C04
		private string FormatLogEntry(LogInfo logInfo)
		{
			string text = logInfo.PlayerAssigned.ToString();
			StringBuilder stringBuilder = new StringBuilder();
			if (logInfo.ActionPlacement == ActionPositionType.Other)
			{
				string text2 = GameActionLogger.OtherActionDescription(logInfo);
				stringBuilder.Append(string.Format("[{0,4}]  {1,-14} {2}", this._gm.TurnCount, text, text2));
			}
			else
			{
				string text3 = GameActionLogger.PlacementLabel(logInfo.ActionPlacement);
				string text4 = GameActionLogger.ActionLabel(logInfo);
				string text5 = GameActionLogger.DetailString(logInfo);
				if (logInfo.Type == LogInfoType.TradeResources && logInfo.AdditionalGain != null)
				{
					List<string> list = new List<string>();
					foreach (LogInfo logInfo2 in logInfo.AdditionalGain)
					{
						GainNonboardResourceLogInfo gainNonboardResourceLogInfo = logInfo2 as GainNonboardResourceLogInfo;
						if (gainNonboardResourceLogInfo != null && gainNonboardResourceLogInfo.Amount > 0)
						{
							list.Add(string.Format("+{0} {1}", gainNonboardResourceLogInfo.Amount, gainNonboardResourceLogInfo.Gained));
						}
					}
					if (list.Count > 0)
					{
						text5 = string.Join(", ", list);
					}
				}
				stringBuilder.Append(string.Format("[{0,4}]  {1,-14} {2,-8} {3,-28} {4}", new object[]
				{
					this._gm.TurnCount,
					text,
					text3,
					text4,
					text5
				}));
				if (logInfo.PayLogInfos != null)
				{
					foreach (LogInfo logInfo3 in logInfo.PayLogInfos)
					{
						stringBuilder.Append(string.Format("\n           PAY  {0,-26} {1}", GameActionLogger.ActionLabel(logInfo3), GameActionLogger.DetailString(logInfo3)));
					}
				}
				if (logInfo.AdditionalGain != null)
				{
					foreach (LogInfo logInfo4 in logInfo.AdditionalGain)
					{
						if (logInfo.Type != LogInfoType.TradeResources || !(logInfo4 is GainNonboardResourceLogInfo))
						{
							stringBuilder.Append(string.Format("\n           GAIN {0,-26} {1}", GameActionLogger.ActionLabel(logInfo4), GameActionLogger.DetailString(logInfo4)));
						}
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060042BE RID: 17086 RVA: 0x0016BC4C File Offset: 0x00169E4C
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

		// Token: 0x060042BF RID: 17087 RVA: 0x0016BCA4 File Offset: 0x00169EA4
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

		// Token: 0x060042C0 RID: 17088 RVA: 0x0016BD7C File Offset: 0x00169F7C
		private static string DetailString(LogInfo logInfo)
		{
			string text11;
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
												string text2 = ((deployLogInfo.DeployedMech != null) ? GameActionLogger.MechName(deployLogInfo.DeployedMech) : "");
												string text3 = ((deployLogInfo.MechBonus != 0) ? string.Format(" +{0}bonus", deployLogInfo.MechBonus) : "");
												return (text2 + " " + text + text3).Trim();
											}
											BuildLogInfo buildLogInfo = logInfo as BuildLogInfo;
											if (buildLogInfo != null)
											{
												string text4 = ((buildLogInfo.Position != null) ? string.Format("Hex({0},{1})", buildLogInfo.Position.posX, buildLogInfo.Position.posY) : "");
												return (((buildLogInfo.PlacedBuilding != null) ? buildLogInfo.PlacedBuilding.buildingType.ToString() : "") + " " + text4).Trim();
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
												List<string> list = new List<string>();
												foreach (KeyValuePair<GameHex, int> keyValuePair in productionLogInfo.Hexes)
												{
													num += keyValuePair.Value;
													GameHex key = keyValuePair.Key;
													string text5 = GameActionLogger.HexTypeResource(key.hexType);
													string text6 = ((productionLogInfo.MillUsed && key.Building != null && key.Building.buildingType == BuildingType.Mill) ? "[mill]" : "");
													list.Add(string.Format("({0},{1}){2}:{3}x{4}", new object[] { key.posX, key.posY, text6, text5, keyValuePair.Value }));
												}
												return string.Format("{0} produced [{1}]", num, string.Join(", ", list));
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
										if (logInfo.Type == LogInfoType.TradeResources)
										{
											Dictionary<ResourceType, int> dictionary = new Dictionary<ResourceType, int>();
											foreach (Dictionary<ResourceType, int> dictionary2 in hexUnitResourceLogInfo.Resources)
											{
												foreach (KeyValuePair<ResourceType, int> keyValuePair2 in dictionary2)
												{
													if (keyValuePair2.Value > 0)
													{
														if (dictionary.ContainsKey(keyValuePair2.Key))
														{
															Dictionary<ResourceType, int> dictionary3 = dictionary;
															ResourceType key2 = keyValuePair2.Key;
															Dictionary<ResourceType, int> dictionary4 = dictionary3;
															ResourceType resourceType = key2;
															dictionary4[resourceType] += keyValuePair2.Value;
														}
														else
														{
															dictionary[keyValuePair2.Key] = keyValuePair2.Value;
														}
													}
												}
											}
											List<string> list2 = new List<string>();
											foreach (KeyValuePair<ResourceType, int> keyValuePair3 in dictionary)
											{
												list2.Add(string.Format("+{0} {1}", keyValuePair3.Value, keyValuePair3.Key));
											}
											return (list2.Count > 0) ? string.Join(", ", list2) : "";
										}
										if (logInfo.Type == LogInfoType.Move || logInfo.Type == LogInfoType.MoveCoins)
										{
											if (hexUnitResourceLogInfo.Hexes.Count == 0)
											{
												return "";
											}
											List<string> list3 = new List<string>();
											foreach (Unit unit in hexUnitResourceLogInfo.Units)
											{
												Mech mech = unit as Mech;
												string text7 = ((mech != null) ? GameActionLogger.MechName(mech) : ((unit.UnitType == UnitType.Character) ? "Hero" : string.Format("{0}#{1}", unit.UnitType, unit.Id)));
												list3.Add(text7);
											}
											string text8 = ((list3.Count > 0) ? (string.Join(", ", list3) + "  ") : "");
											if (hexUnitResourceLogInfo.Hexes.Count == 1)
											{
												return text8 + GameActionLogger.HexLabel(hexUnitResourceLogInfo.Hexes[0]);
											}
											string text9 = GameActionLogger.HexLabel(hexUnitResourceLogInfo.Hexes[0]);
											List<string> list4 = new List<string>();
											for (int i = 1; i < hexUnitResourceLogInfo.Hexes.Count; i++)
											{
												list4.Add(GameActionLogger.HexLabel(hexUnitResourceLogInfo.Hexes[i]));
											}
											string text10 = string.Join(" & ", list4);
											return text8 + text9 + " → " + text10;
										}
										else
										{
											text11 = "";
										}
									}
									else
									{
										text11 = string.Format("+{0} {1}", gainNonboardResourceLogInfo.Amount, gainNonboardResourceLogInfo.Gained);
									}
								}
								else
								{
									text11 = string.Format("Spied: {0}", sneakPeakLogInfo.SpiedFaction);
								}
							}
							else
							{
								text11 = ((workerLogInfo.Position != null) ? string.Format("+{0} at Hex({1},{2})", workerLogInfo.WorkersAmount, workerLogInfo.Position.posX, workerLogInfo.Position.posY) : string.Format("+{0}", workerLogInfo.WorkersAmount));
							}
						}
						else
						{
							List<string> list5 = new List<string>();
							foreach (KeyValuePair<ResourceType, int> keyValuePair4 in payResourceLogInfo.Resources)
							{
								if (keyValuePair4.Value != 0)
								{
									list5.Add(string.Format("-{0} {1}", keyValuePair4.Value, keyValuePair4.Key));
								}
							}
							text11 = ((list5.Count > 0) ? string.Join(", ", list5) : "");
						}
					}
					else
					{
						text11 = string.Format("-{0} {1}", payNonboardResourceLogInfo.Amount, payNonboardResourceLogInfo.Resource);
					}
				}
				else
				{
					string text12 = ((combatLogInfo.Battlefield != null) ? string.Format("Hex({0},{1})", combatLogInfo.Battlefield.posX, combatLogInfo.Battlefield.posY) : "?");
					string text13 = string.Format("W:{0}+{1}", combatLogInfo.WinnerPower.selectedPower, combatLogInfo.WinnerPower.cardsPower);
					string text14 = string.Format("D:{0}+{1}", combatLogInfo.DefeatedPower.selectedPower, combatLogInfo.DefeatedPower.cardsPower);
					string text15 = ((combatLogInfo.LostPopularity != 0) ? string.Format(" pop-{0}", combatLogInfo.LostPopularity) : "");
					string text16 = ((combatLogInfo.WinnerAbilityUsed || combatLogInfo.DefeatedAbilityUsed) ? " [ability]" : "");
					text11 = string.Concat(new string[] { text12, " ", text13, " vs ", text14, text15, text16 });
				}
			}
			catch
			{
				text11 = "";
			}
			return text11;
		}

		// Token: 0x060042C1 RID: 17089 RVA: 0x0016C738 File Offset: 0x0016A938
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

		// Token: 0x060042C2 RID: 17090 RVA: 0x0016C7A0 File Offset: 0x0016A9A0
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

		// Token: 0x060042C3 RID: 17091 RVA: 0x0016C914 File Offset: 0x0016AB14
		private static string OtherActionDescription(LogInfo logInfo)
		{
			if (logInfo.IsEncounter)
			{
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				if (logInfo.PayLogInfos != null)
				{
					foreach (LogInfo logInfo2 in logInfo.PayLogInfos)
					{
						list.Add(GameActionLogger.DetailString(logInfo2));
					}
				}
				// Include the base LogInfo's own gain (it is often the first
				// encounter gain, e.g. +2 oil, +2 Coin, etc.).
				// Skip bare LogInfo and the empty placeholder (Amount=0)
				// created by CreateEmptyEncounterActionLog.
				if (logInfo.GetType() != typeof(LogInfo))
				{
					GainNonboardResourceLogInfo gnbrBase = logInfo as GainNonboardResourceLogInfo;
					if (gnbrBase == null || gnbrBase.Amount > 0)
					{
						string baseGain = GameActionLogger.GainSummary(logInfo);
						if (!string.IsNullOrEmpty(baseGain))
						{
							list2.Add(baseGain);
						}
					}
				}
				if (logInfo.AdditionalGain != null)
				{
					foreach (LogInfo logInfo3 in logInfo.AdditionalGain)
					{
						list2.Add(GameActionLogger.GainSummary(logInfo3));
					}
				}
				string text = ((list.Count > 0) ? string.Join(", ", list) : "free");
				string text2 = ((list2.Count > 0) ? string.Join(", ", list2) : "");
				string text3 = ((text2.Length > 0) ? " → " : "");
				if (logInfo.EncounterCardId > 0)
				{
					return string.Format("Encounter [Card #{0}]: {1}{2}{3}", new object[] { logInfo.EncounterCardId, text, text3, text2 });
				}
				return string.Format("Encounter: {0}{1}{2}", text, text3, text2);
			}
			else
			{
				StarLogInfo starLogInfo = logInfo as StarLogInfo;
				if (starLogInfo != null)
				{
					return string.Format("★ Star: {0} ({1}/6)", starLogInfo.GainedStar, starLogInfo.starsUnlocked);
				}
				FactoryLogInfo factoryLogInfo = logInfo as FactoryLogInfo;
				if (factoryLogInfo != null && logInfo.Type == LogInfoType.FactoryCardGain)
				{
					return string.Format("Factory Card #{0}", factoryLogInfo.GainedFactoryCard.CardId);
				}
				if (logInfo.Type == LogInfoType.FactoryTopAction)
				{
					List<string> list3 = new List<string>();
					List<string> list4 = new List<string>();
					if (logInfo.PayLogInfos != null)
					{
						foreach (LogInfo logInfo4 in logInfo.PayLogInfos)
						{
							list3.Add(GameActionLogger.DetailString(logInfo4));
						}
					}
					if (logInfo.AdditionalGain != null)
					{
						foreach (LogInfo logInfo5 in logInfo.AdditionalGain)
						{
							list4.Add(GameActionLogger.GainSummary(logInfo5));
						}
					}
					string text4 = ((list3.Count > 0) ? string.Join(", ", list3) : "");
					string text5 = ((list4.Count > 0) ? string.Join(", ", list4) : "");
					string text6 = ((text4.Length > 0 && text5.Length > 0) ? " → " : "");
					return string.Format("Factory: {0}{1}{2}", text4, text6, text5);
				}
				DeployLogInfo deployLogInfo = logInfo as DeployLogInfo;
				if (deployLogInfo != null)
				{
					string text7 = ((deployLogInfo.DeployedMech != null) ? GameActionLogger.MechName(deployLogInfo.DeployedMech) : "?");
					string text8 = ((deployLogInfo.Position != null) ? string.Format(" at ({0},{1})", deployLogInfo.Position.posX, deployLogInfo.Position.posY) : "");
					string text9 = ((deployLogInfo.MechBonus != 0) ? string.Format(" +{0}bonus", deployLogInfo.MechBonus) : "");
					return string.Format("Deploy {0}{1}{2}", text7, text8, text9);
				}
				if (logInfo.Type == LogInfoType.TradeResources)
				{
					List<string> list5 = new List<string>();
					List<string> list6 = new List<string>();
					if (logInfo.PayLogInfos != null)
					{
						foreach (LogInfo logInfo6 in logInfo.PayLogInfos)
						{
							list5.Add(GameActionLogger.DetailString(logInfo6));
						}
					}
					if (logInfo.AdditionalGain != null)
					{
						foreach (LogInfo logInfo7 in logInfo.AdditionalGain)
						{
							GainNonboardResourceLogInfo gainNonboardResourceLogInfo = logInfo7 as GainNonboardResourceLogInfo;
							if (gainNonboardResourceLogInfo != null && gainNonboardResourceLogInfo.Amount > 0)
							{
								list6.Add(string.Format("+{0} {1}", gainNonboardResourceLogInfo.Amount, gainNonboardResourceLogInfo.Gained));
							}
						}
					}
					string text10 = ((list5.Count > 0) ? string.Join(", ", list5) : "");
					string text11 = ((list6.Count > 0) ? string.Join(", ", list6) : "");
					string text12 = ((text10.Length > 0 && text11.Length > 0) ? " → " : "");
					return string.Format("Trade: {0}{1}{2}", text10, text12, text11);
				}
				GainNonboardResourceLogInfo gainNonboardResourceLogInfo2 = logInfo as GainNonboardResourceLogInfo;
				if (gainNonboardResourceLogInfo2 != null)
				{
					return string.Format("+{0} {1}", gainNonboardResourceLogInfo2.Amount, gainNonboardResourceLogInfo2.Gained);
				}
				return GameActionLogger.ActionLabel(logInfo) + " " + GameActionLogger.DetailString(logInfo);
			}
		}

		// Token: 0x060042C4 RID: 17092 RVA: 0x0016CE4C File Offset: 0x0016B04C
		private static string GainSummary(LogInfo g)
		{
			GainNonboardResourceLogInfo gainNonboardResourceLogInfo = g as GainNonboardResourceLogInfo;
			if (gainNonboardResourceLogInfo != null)
			{
				return string.Format("+{0} {1}", gainNonboardResourceLogInfo.Amount, gainNonboardResourceLogInfo.Gained);
			}
			WorkerLogInfo workerLogInfo = g as WorkerLogInfo;
			if (workerLogInfo != null)
			{
				if (workerLogInfo.Position == null)
				{
					return string.Format("+{0} workers", workerLogInfo.WorkersAmount);
				}
				return string.Format("+{0} workers at ({1},{2})", workerLogInfo.WorkersAmount, workerLogInfo.Position.posX, workerLogInfo.Position.posY);
			}
			else
			{
				DeployLogInfo deployLogInfo = g as DeployLogInfo;
				if (deployLogInfo != null)
				{
					return "Deploy " + ((deployLogInfo.DeployedMech != null) ? GameActionLogger.MechName(deployLogInfo.DeployedMech) : "mech");
				}
				BuildLogInfo buildLogInfo = g as BuildLogInfo;
				if (buildLogInfo != null)
				{
					if (buildLogInfo.PlacedBuilding == null)
					{
						return "Build";
					}
					return "Build " + buildLogInfo.PlacedBuilding.buildingType.ToString();
				}
				else
				{
					UpgradeLogInfo upgradeLogInfo = g as UpgradeLogInfo;
					if (upgradeLogInfo != null)
					{
						if (upgradeLogInfo.DownAction == PayType.Coin)
						{
							return "Upgrade";
						}
						return string.Format("Upgrade: {0}, {1}", upgradeLogInfo.TopAction, GameActionLogger.UpgradeBottomName(upgradeLogInfo.Resource));
					}
					else
					{
						EnlistLogInfo enlistLogInfo = g as EnlistLogInfo;
						if (enlistLogInfo == null)
						{
							string text = GameActionLogger.DetailString(g).Trim();
							if (string.IsNullOrEmpty(text))
							{
								return GameActionLogger.ActionLabel(g);
							}
							if (g.Type == LogInfoType.TradeResources || g.Type == LogInfoType.Produce)
							{
								return text;
							}
							return GameActionLogger.ActionLabel(g) + " " + text;
						}
						else
						{
							if (enlistLogInfo.TypeOfDownAction == DownActionType.Factory)
							{
								return "Enlist";
							}
							return string.Format("Enlist {0}→+{1}", enlistLogInfo.TypeOfDownAction, enlistLogInfo.OneTimeBonus);
						}
					}
				}
			}
		}

		// Token: 0x060042C5 RID: 17093 RVA: 0x00052857 File Offset: 0x00050A57
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

		// Token: 0x060042C6 RID: 17094 RVA: 0x0016D00C File Offset: 0x0016B20C
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

		// Token: 0x060042C7 RID: 17095 RVA: 0x0016D068 File Offset: 0x0016B268
		private static string MechName(Mech mech)
		{
			try
			{
				if (mech != null && mech.Owner != null && mech.Owner.matFaction != null && mech.Owner.matFaction.abilities != null && mech.Id < mech.Owner.matFaction.abilities.Count)
				{
					AbilityPerk abilityPerk = mech.Owner.matFaction.abilities[mech.Id];
					int num = (int)abilityPerk;
					if (num == 3 || num == 12 || num == 17 || num == 20)
					{
						return "Riverwalk";
					}
					return abilityPerk.ToString();
				}
			}
			catch
			{
			}
			return string.Format("Mech#{0}", (mech != null) ? mech.Id.ToString() : "?");
		}

		// Token: 0x04003367 RID: 13159
		private readonly GameManager _gm;

		// Token: 0x04003368 RID: 13160
		private readonly string _filePath;

		// Token: 0x04003369 RID: 13161
		private readonly List<string> _pending = new List<string>();

		// Token: 0x0400336A RID: 13162
		private bool _disposed;

		// Token: 0x0400336B RID: 13163
		private int _lastTurnSeen = -1;
	}
}
