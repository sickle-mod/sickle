using System;
using System.Collections.Generic;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x0200057D RID: 1405
	public class AiKickStartAdv
	{
		// Token: 0x06002CE6 RID: 11494 RVA: 0x0004455A File Offset: 0x0004275A
		public AiKickStartAdv(GameManager gameManager)
		{
			this.gameManager = gameManager;
		}

		// Token: 0x06002CE7 RID: 11495 RVA: 0x001019D4 File Offset: 0x000FFBD4
		public void KickStart(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			// Order: Polonia, Nordic, Rusviet, Crimea, Saxony, Albion, Togawa (fallback)
			// Within each: Industrial, Engineering, Patriotic, Mechanical, Agricultural, Militant, Innovative
			
			if (player.player.matFaction.faction == Faction.Polania)
			{
				switch (player.player.matPlayer.matType)
				{
				case PlayerMatType.Industrial:
					this.PolaniaIndustrial(actionOptions, player);
					return;
				case PlayerMatType.Engineering:
					this.PolaniaEngine(actionOptions, player);
					return;
				case PlayerMatType.Patriotic:
					this.PolaniaPatriot(actionOptions, player);
					return;
				case PlayerMatType.Mechanical:
					this.PolaniaMech(actionOptions, player);
					return;
				case PlayerMatType.Agricultural:
					this.PolaniaAgro(actionOptions, player);
					return;
				default:
					return;
				}
			}
			else if (player.player.matFaction.faction == Faction.Nordic)
			{
				switch (player.player.matPlayer.matType)
				{
				case PlayerMatType.Industrial:
					this.NordIndustrial(actionOptions, player);
					return;
				case PlayerMatType.Engineering:
					this.NordEngine(actionOptions, player);
					return;
				case PlayerMatType.Patriotic:
					this.NordPatriot(actionOptions, player);
					return;
				case PlayerMatType.Mechanical:
					this.NordMech(actionOptions, player);
					return;
				case PlayerMatType.Agricultural:
					this.NordAgro(actionOptions, player);
					return;
				case PlayerMatType.Militant:
					break;
				case PlayerMatType.Innovative:
					this.NordInnovative(actionOptions, player);
					return;
				default:
					return;
				}
			}
			else if (player.player.matFaction.faction == Faction.Rusviet)
			{
				switch (player.player.matPlayer.matType)
				{
				case PlayerMatType.Industrial:
					this.RusvietIndustrial(actionOptions, player);
					return;
				case PlayerMatType.Engineering:
					break;
				case PlayerMatType.Patriotic:
					this.RusvietPatriot(actionOptions, player);
					return;
				case PlayerMatType.Mechanical:
					this.RusvietMech(actionOptions, player);
					return;
				case PlayerMatType.Agricultural:
					this.RusvietAgro(actionOptions, player);
					return;
				default:
					return;
				}
			}
			else if (player.player.matFaction.faction == Faction.Crimea)
			{
				switch (player.player.matPlayer.matType)
				{
				case PlayerMatType.Industrial:
					this.CrimeaIndustrial(actionOptions, player);
					return;
				case PlayerMatType.Engineering:
					this.CrimeaEngine(actionOptions, player);
					return;
				case PlayerMatType.Patriotic:
				case PlayerMatType.Mechanical:
					break;
				case PlayerMatType.Agricultural:
					this.CrimeaAgro(actionOptions, player);
					return;
				default:
					return;
				}
			}
			else if (player.player.matFaction.faction == Faction.Saxony)
			{
				switch (player.player.matPlayer.matType)
				{
				case PlayerMatType.Industrial:
					this.SaxIndustrial(actionOptions, player);
					return;
				case PlayerMatType.Engineering:
					this.SaxEngine(actionOptions, player);
					return;
				case PlayerMatType.Patriotic:
					this.SaxPatriot(actionOptions, player);
					return;
				case PlayerMatType.Mechanical:
					this.SaxMech(actionOptions, player);
					return;
				case PlayerMatType.Agricultural:
					this.SaxAgro(actionOptions, player);
					return;
				default:
					return;
				}
			}
			else if (player.player.matFaction.faction == Faction.Albion)
			{
				switch (player.player.matPlayer.matType)
				{
				case PlayerMatType.Industrial:
					this.AlbionIndustrial(actionOptions, player);
					return;
				case PlayerMatType.Engineering:
				case PlayerMatType.Agricultural:
					break;
				case PlayerMatType.Patriotic:
					this.AlbionPatriot(actionOptions, player);
					break;
				case PlayerMatType.Mechanical:
					this.AlbionMech(actionOptions, player);
					return;
				case PlayerMatType.Militant:
					this.AlbionMilitant(actionOptions, player);
					return;
				case PlayerMatType.Innovative:
					this.AlbionInnovative(actionOptions, player);
					return;
				default:
					return;
				}
			}
			else if (player.player.matFaction.faction == Faction.Togawa)
			{
				switch (player.player.matPlayer.matType)
				{
				case PlayerMatType.Industrial:
					this.TogawaIndustrial(actionOptions, player);
					return;
				case PlayerMatType.Engineering:
					this.TogawaEngineering(actionOptions, player);
					return;
				case PlayerMatType.Patriotic:
					break;
				case PlayerMatType.Mechanical:
					this.TogawaMech(actionOptions, player);
					return;
				case PlayerMatType.Agricultural:
					this.TogawaAgro(actionOptions, player);
					return;
				case PlayerMatType.Militant:
					this.TogawaMilitant(actionOptions, player);
					return;
				case PlayerMatType.Innovative:
					this.TogawaInnovative(actionOptions, player);
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x06002CE8 RID: 11496 RVA: 0x00101CE4 File Offset: 0x000FFEE4
		private void AlbionPatriot(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 3:
				if (this.gameManager.gameBoard.validateOwnership(new List<GameHex>
				{
					this.gameManager.gameBoard.hexMap[2, 1],
					this.gameManager.gameBoard.hexMap[3, 1]
				}, player.player))
				{
					actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
					{
						moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
						{
							GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
							this.gameManager.moveManager.SetMoveAction(gainMove);
							this.gameManager.moveManager.SelectUnit(player.player.character);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[2, 1], null, null);
							this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[2, 1].GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 1], this.HexResources(this.gameManager.gameBoard.hexMap[2, 1]), null);
							this.gameManager.moveManager.Clear();
							this.gameManager.actionManager.PrepareNextAction();
						}
					});
					return;
				}
				break;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.food,
						ResourceType.food
					}
				});
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				break;
			default:
				return;
			}
		}

		// Token: 0x06002CE9 RID: 11497 RVA: 0x00101EDC File Offset: 0x001000DC
		private void AlbionMech(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				if (this.gameManager.gameBoard.validateOwnership(new List<GameHex>
				{
					this.gameManager.gameBoard.hexMap[2, 1],
					this.gameManager.gameBoard.hexMap[3, 1]
				}, player.player))
				{
					actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
					{
						moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
						{
							GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
							this.gameManager.moveManager.SetMoveAction(gainMove);
							this.gameManager.moveManager.SelectUnit(player.player.character);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[2, 1], null, null);
							this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[2, 1].GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 1], this.HexResources(this.gameManager.gameBoard.hexMap[2, 1]), null);
							this.gameManager.moveManager.Clear();
							this.gameManager.actionManager.PrepareNextAction();
						}
					});
					return;
				}
				break;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 2:
			{
				int maxValue = int.MaxValue;
				AiRecipe aiRecipe = new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart");
				AiRecipe aiRecipe2 = aiRecipe;
				ResourceType[] array = new ResourceType[2];
				array[0] = ResourceType.metal;
				aiRecipe2.tradeResource = array;
				actionOptions.Add(maxValue, aiRecipe);
				return;
			}
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				break;
			default:
				return;
			}
		}

		// Token: 0x06002CEA RID: 11498 RVA: 0x00102040 File Offset: 0x00100240
		private void AlbionIndustrial(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				if (this.gameManager.gameBoard.validateOwnership(new List<GameHex>
				{
					this.gameManager.gameBoard.hexMap[2, 1],
					this.gameManager.gameBoard.hexMap[3, 1]
				}, player.player))
				{
					actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
					{
						moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
						{
							GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
							this.gameManager.moveManager.SetMoveAction(gainMove);
							this.gameManager.moveManager.SelectUnit(player.player.character);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[2, 1], null, null);
							this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[2, 1].GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 1], this.HexResources(this.gameManager.gameBoard.hexMap[2, 1]), null);
							this.gameManager.moveManager.Clear();
							this.gameManager.actionManager.PrepareNextAction();
						}
					});
					return;
				}
				break;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[3, 1];
				return;
			case 5:
				if (this.gameManager.gameBoard.validateOwnership(new List<GameHex>
				{
					this.gameManager.gameBoard.hexMap[2, 1],
					this.gameManager.gameBoard.hexMap[3, 1],
					this.gameManager.gameBoard.hexMap[1, 1]
				}, player.player) && this.gameManager.gameBoard.hexMap[3, 1].GetOwnerMechs().Count > 1)
				{
					actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
					{
						moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
						{
							GainMove gainMove2 = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
							this.gameManager.moveManager.SetMoveAction(gainMove2);
							Mech mech = this.gameManager.gameBoard.hexMap[3, 1].GetOwnerMechs()[0];
							Mech mech2 = this.gameManager.gameBoard.hexMap[3, 1].GetOwnerMechs()[1];
							List<Unit> list = new List<Unit>();
							list.Add(mech.position.GetOwnerWorkers()[0]);
							list.Add(mech.position.GetOwnerWorkers()[1]);
							this.gameManager.moveManager.SelectUnit(mech);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[2, 1], this.HexResources(mech.position), list);
							this.gameManager.moveManager.UnloadAllWorkersFromMech(mech);
							List<Unit> list2 = new List<Unit>();
							foreach (Worker worker in mech2.position.GetOwnerWorkers())
							{
								list2.Add(worker);
							}
							this.gameManager.moveManager.SelectUnit(mech2);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 1], this.HexResources(mech2.position), list2);
							this.gameManager.moveManager.UnloadAllWorkersFromMech(mech2);
							this.gameManager.moveManager.Clear();
							this.gameManager.actionManager.PrepareNextAction();
						}
					});
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06002CEB RID: 11499 RVA: 0x001022E8 File Offset: 0x001004E8
		private void AlbionInnovative(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				if (this.gameManager.gameBoard.validateOwnership(new List<GameHex>
				{
					this.gameManager.gameBoard.hexMap[2, 1],
					this.gameManager.gameBoard.hexMap[3, 1]
				}, player.player))
				{
					actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
					{
						moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
						{
							GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
							this.gameManager.moveManager.SetMoveAction(gainMove);
							this.gameManager.moveManager.SelectUnit(player.player.character);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[2, 1], null, null);
							this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[2, 1].GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 1], this.HexResources(this.gameManager.gameBoard.hexMap[2, 1]), null);
							this.gameManager.moveManager.Clear();
							this.gameManager.actionManager.PrepareNextAction();
						}
					});
					return;
				}
				break;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 2:
			{
				int maxValue = int.MaxValue;
				AiRecipe aiRecipe = new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart");
				AiRecipe aiRecipe2 = aiRecipe;
				ResourceType[] array = new ResourceType[2];
				array[0] = ResourceType.metal;
				aiRecipe2.tradeResource = array;
				actionOptions.Add(maxValue, aiRecipe);
				return;
			}
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 5:
				if (this.gameManager.gameBoard.validateOwnership(new List<GameHex>
				{
					this.gameManager.gameBoard.hexMap[2, 1],
					this.gameManager.gameBoard.hexMap[3, 1],
					this.gameManager.gameBoard.hexMap[1, 1],
					this.gameManager.gameBoard.hexMap[1, 2]
				}, player.player))
				{
					actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
					{
						moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
						{
							GainMove gainMove2 = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
							this.gameManager.moveManager.SetMoveAction(gainMove2);
							this.gameManager.moveManager.SelectUnit(player.player.character);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 2], null, null);
							this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[1, 1].GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 2], this.HexResources(this.gameManager.gameBoard.hexMap[1, 1]), null);
							if (this.gameManager.gameBoard.hexMap[3, 1].GetOwnerMechs().Count > 0)
							{
								Mech mech = this.gameManager.gameBoard.hexMap[3, 1].GetOwnerMechs()[0];
								List<Unit> list = new List<Unit>();
								foreach (Worker worker in mech.position.GetOwnerWorkers())
								{
									list.Add(worker);
								}
								this.gameManager.moveManager.SelectUnit(mech);
								this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[2, 1], this.HexResources(mech.position), list);
								this.gameManager.moveManager.UnloadAllWorkersFromMech(mech);
							}
							this.gameManager.moveManager.Clear();
							this.gameManager.actionManager.PrepareNextAction();
						}
					});
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06002CEC RID: 11500 RVA: 0x00102558 File Offset: 0x00100758
		private void AlbionMilitant(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 2:
				if (this.gameManager.gameBoard.validateOwnership(new List<GameHex>
				{
					this.gameManager.gameBoard.hexMap[2, 1],
					this.gameManager.gameBoard.hexMap[3, 1],
					this.gameManager.gameBoard.hexMap[1, 1]
				}, player.player))
				{
					actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
					{
						moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
						{
							GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
							this.gameManager.moveManager.SetMoveAction(gainMove);
							this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[2, 1].GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 1], this.HexResources(this.gameManager.gameBoard.hexMap[2, 1]), null);
							this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[1, 1].GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[2, 1], this.HexResources(this.gameManager.gameBoard.hexMap[1, 1]), null);
							this.gameManager.moveManager.Clear();
							this.gameManager.actionManager.PrepareNextAction();
						}
					});
				}
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[3, 1];
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.food,
						ResourceType.food
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CED RID: 11501 RVA: 0x00102740 File Offset: 0x00100940
		private void TogawaIndustrial(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Kickstart"));
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[6, 6].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 6], this.HexResources(this.gameManager.gameBoard.hexMap[6, 6]), null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[6, 7].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 7], this.HexResources(this.gameManager.gameBoard.hexMap[6, 7]), null);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[6, 7], null, null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CEE RID: 11502 RVA: 0x00102840 File Offset: 0x00100A40
		private void TogawaAgro(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.oil,
						ResourceType.food
					}
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[6, 7], null, null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[6, 7].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 7], this.HexResources(this.gameManager.gameBoard.hexMap[6, 7]), null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CEF RID: 11503 RVA: 0x00102944 File Offset: 0x00100B44
		private void TogawaEngineering(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.food,
						ResourceType.food
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CF0 RID: 11504 RVA: 0x001029FC File Offset: 0x00100BFC
		private void TogawaMilitant(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[6, 6], null, null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[6, 6].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 6], this.HexResources(this.gameManager.gameBoard.hexMap[6, 6]), null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CF1 RID: 11505 RVA: 0x00102AD8 File Offset: 0x00100CD8
		private void TogawaMech(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[6, 7], null, null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[6, 7].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 7], this.HexResources(this.gameManager.gameBoard.hexMap[6, 7]), null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[6, 6].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 6], this.HexResources(this.gameManager.gameBoard.hexMap[6, 6]), null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CF2 RID: 11506 RVA: 0x00102BAC File Offset: 0x00100DAC
		private void TogawaInnovative(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[6, 7], null, null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[6, 7].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 7], this.HexResources(this.gameManager.gameBoard.hexMap[6, 7]), null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.oil,
						ResourceType.food
					}
				});
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 7:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove2 = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove2);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 6], null, null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[5, 7].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 6], this.HexResources(this.gameManager.gameBoard.hexMap[5, 7]), null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[5, 7].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 6], this.HexResources(this.gameManager.gameBoard.hexMap[5, 7]), null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CF3 RID: 11507 RVA: 0x00102D7C File Offset: 0x00100F7C
		private void CrimeaIndustrial(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				// Turn 0: Trade → 2 Oil
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.oil,
						ResourceType.oil
					}
				});
				return;
			case 1:
				// Turn 1: GainCombatCard, Spend Combat Card + 2 oil to Upgrade
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.CombatCard], "Kickstart"));
				return;
			case 2:
				// Turn 2: Trade → 2 Oil
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.oil,
						ResourceType.oil
					}
				});
				return;
			case 3:
				// Turn 3: GainCombatCard, Spend Combat Card + 2 oil to Upgrade
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.CombatCard], "Kickstart"));
				return;
			case 4:
				// Turn 4: Trade → 2 Food, Use Combat Card + 2 food to Enlist
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.food,
						ResourceType.food
					}
				});
				return;
			case 5:
				// Turn 5: GainCombatCard, Spend Combat Card + 2 oil to Upgrade
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.CombatCard], "Kickstart"));
				return;
			case 6:
				// Turn 6: Trade → 1 Food + 1 Oil, Use Combat Card to Enlist
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.food,
						ResourceType.oil
					}
				});
				return;
			case 7:
				// Turn 7: GainPower, Upgrade
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Kickstart"));
				return;
			case 8:
				// Turn 8: Trade → 1 Food + 1 Oil, Enlist
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.food,
						ResourceType.oil
					}
				});
				return;
			case 9:
				// Turn 9: Bolster → Power, Upgrade
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Kickstart"));
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CF4 RID: 11508 RVA: 0x00102E50 File Offset: 0x00101050
		private void CrimeaEngine(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.oil,
						ResourceType.food
					}
				});
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						Worker worker = this.gameManager.gameBoard.hexMap[3, 8].GetOwnerWorkers()[0];
						Worker worker2 = this.gameManager.gameBoard.hexMap[3, 8].GetOwnerWorkers()[1];
						Worker worker3 = this.gameManager.gameBoard.hexMap[3, 8].GetOwnerWorkers()[2];
						this.gameManager.moveManager.SelectUnit(worker);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 7], null, null);
						this.gameManager.moveManager.SelectUnit(worker2);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 7], null, null);
						this.gameManager.moveManager.SelectUnit(worker3);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 7], null, null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CF5 RID: 11509 RVA: 0x00102F8C File Offset: 0x0010118C
		private void CrimeaAgro(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				// Turn 1: Produce
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Crimea Agro Kickstart"));
				return;
			case 1:
				// Turn 2: Trade for 2 food
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Crimea Agro Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.food,
						ResourceType.food
					}
				});
				return;
			case 2:
				// Turn 3: Produce
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Crimea Agro Kickstart"));
				return;
			case 3:
				// Turn 4: Trade for 2 metal
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Crimea Agro Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 4:
				// Turn 5: Gain Power (bolster), enlist
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Crimea Agro Kickstart"));
				return;
			case 5:
				// Turn 6: Trade for 2 metal, deploy mech to village (3,8)
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Crimea Agro Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 6:
				// Turn 7: Move hero to (4,7) and move mech with all 4 workers from (3,8) to mountain (4,7), drop 2, carry 2 to (3,7)
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Crimea Agro Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						
						// Move hero to mountain (4,7)
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 7], null, null);
						
						// Get the mech at village (3,8)
						Mech mech = this.gameManager.gameBoard.hexMap[3, 8].GetOwnerMechs()[0];
						
						// Get all 4 workers at village (3,8)
						List<Worker> workers = this.gameManager.gameBoard.hexMap[3, 8].GetOwnerWorkers();
						Worker worker1 = workers[0];
						Worker worker2 = workers[1];
						Worker worker3 = workers[2];
						Worker worker4 = workers[3];
						
						// Move mech with all 4 workers to mountain (4,7)
						this.gameManager.moveManager.SelectUnit(mech);
						this.gameManager.moveManager.SelectUnit(worker1);
						this.gameManager.moveManager.SelectUnit(worker2);
						this.gameManager.moveManager.SelectUnit(worker3);
						this.gameManager.moveManager.SelectUnit(worker4);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 7], null, null);
						
						// Drop 2 workers at mountain (4,7) by moving them to the same hex
						this.gameManager.moveManager.SelectUnit(worker1);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 7], null, null);
						this.gameManager.moveManager.SelectUnit(worker2);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 7], null, null);
						
						// Move mech with remaining 2 workers to farm (3,7)
						this.gameManager.moveManager.SelectUnit(mech);
						this.gameManager.moveManager.SelectUnit(worker3);
						this.gameManager.moveManager.SelectUnit(worker4);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 7], null, null);
						
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 7:
				// Turn 8: Produce
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Crimea Agro Kickstart"));
				return;
			case 8:
				// Turn 9: Trade for 2 metal, deploy mech to mountain (4,7)
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Crimea Agro Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 9:
				// Turn 10: Gain Power (bolster), enlist
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Crimea Agro Kickstart"));
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CF6 RID: 11510 RVA: 0x00103184 File Offset: 0x00101384
		private void RusvietIndustrial(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			if (this.gameManager.TurnCount == 0)
			{
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
			}
		}

		// Token: 0x06002CF7 RID: 11511 RVA: 0x001031D8 File Offset: 0x001013D8
		private void RusvietPatriot(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				// Turn 1: Produce
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Rusviet Patriot Kickstart"));
				return;
			case 1:
				// Turn 2: Produce
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Rusviet Patriot Kickstart"));
				return;
			case 2:
				// Turn 3: Trade for 2 metal
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Rusviet Patriot Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 3:
				// Turn 4: Gain power (bolster), deploy mech to village (6,3)
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Rusviet Patriot Kickstart"));
				return;
			case 4:
				// Turn 5: Move mech (carrying 3 workers) from village (6,3) to tundra (6,2)
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Rusviet Patriot Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						
						// Get mech and workers at village (6,3)
						Mech mech = this.gameManager.gameBoard.hexMap[6, 3].GetOwnerMechs()[0];
						List<Worker> workers = this.gameManager.gameBoard.hexMap[6, 3].GetOwnerWorkers();
						Worker worker1 = workers[0];
						Worker worker2 = workers[1];
						Worker worker3 = workers[2];
						
						// Move mech with 3 workers to tundra (6,2)
						this.gameManager.moveManager.SelectUnit(mech);
						this.gameManager.moveManager.SelectUnit(worker1);
						this.gameManager.moveManager.SelectUnit(worker2);
						this.gameManager.moveManager.SelectUnit(worker3);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[6, 2], null, null);
						
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 5:
				// Turn 6: Move remaining worker from village (6,3) to forest (6,4)
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Rusviet Patriot Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						
						// Move remaining worker from village (6,3) to forest (6,4)
						Worker worker = this.gameManager.gameBoard.hexMap[6, 3].GetOwnerWorkers()[0];
						this.gameManager.moveManager.SelectUnit(worker);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[6, 4], null, null);
						
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 6:
				// Turn 7: Produce, enlist
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Rusviet Patriot Kickstart"));
				return;
			case 7:
				// Turn 8: Produce, enlist
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Rusviet Patriot Kickstart"));
				return;
			case 8:
				// Turn 9: Gain power, deploy mech to forest (6,4)
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Rusviet Patriot Kickstart"));
				return;
			case 9:
				// Turn 10: Produce, enlist
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Rusviet Patriot Kickstart"));
				return;
			case 10:
				// Turn 11: Produce, enlist
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Rusviet Patriot Kickstart"));
				return;
			case 11:
				// Turn 12: Gain power, deploy mech
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Rusviet Patriot Kickstart"));
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CF8 RID: 11512 RVA: 0x0010340C File Offset: 0x0010160C
		private void RusvietMech(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.oil,
						ResourceType.metal
					}
				});
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.food,
						ResourceType.food
					}
				});
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CF9 RID: 11513 RVA: 0x00103564 File Offset: 0x00101764
		private void RusvietAgro(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 4], null, null);
						Mech mech = this.gameManager.gameBoard.hexMap[6, 3].GetOwnerMechs()[0];
						List<Unit> list = new List<Unit>();
						foreach (Worker worker in mech.position.GetOwnerWorkers())
						{
							list.Add(worker);
						}
						this.gameManager.moveManager.SelectUnit(mech);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[6, 4], this.HexResources(mech.position), list);
						this.gameManager.moveManager.UnloadAllWorkersFromMech(mech);
						List<Unit> list2 = new List<Unit>();
						foreach (Worker worker2 in mech.position.GetOwnerWorkers())
						{
							list2.Add(worker2);
						}
						list2.RemoveRange(0, 1);
						this.gameManager.moveManager.SelectUnit(mech);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 4], this.HexResources(mech.position), list2);
						this.gameManager.moveManager.UnloadAllWorkersFromMech(mech);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 7:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove2 = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove2);
						Dictionary<GameHex, int> dictionary = new Dictionary<GameHex, int>();
						player.player.character.position.gameBoard.MoveRange(player.player.character, 2, out dictionary);
						GameHex gameHex = player.player.character.position;
						foreach (GameHex gameHex2 in dictionary.Keys)
						{
							if (gameHex2.hasEncounter && !gameHex2.encounterUsed)
							{
								gameHex = gameHex2;
							}
						}
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(gameHex, null, null);
						Mech mech2 = this.gameManager.gameBoard.hexMap[5, 4].GetOwnerMechs()[0];
						Dictionary<GameHex, int> dictionary2 = new Dictionary<GameHex, int>();
						mech2.position.gameBoard.MoveRange(mech2, 2, out dictionary2);
						GameHex gameHex3 = mech2.position;
						foreach (GameHex gameHex4 in dictionary2.Keys)
						{
							if (gameHex4.hexType == HexType.forest)
							{
								if (gameHex3.hexType != HexType.forest)
								{
									gameHex3 = gameHex4;
								}
								else if (gameHex4.hasTunnel && !gameHex3.hasTunnel)
								{
									gameHex3 = gameHex4;
								}
							}
						}
						List<Unit> list3 = new List<Unit>();
						foreach (Worker worker3 in mech2.position.GetOwnerWorkers())
						{
							list3.Add(worker3);
						}
						list3.RemoveRange(0, 1);
						this.gameManager.moveManager.SelectUnit(mech2);
						this.gameManager.moveManager.MoveSelectedUnit(gameHex3, null, list3);
						this.gameManager.moveManager.UnloadAllWorkersFromMech(mech2);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CFA RID: 11514 RVA: 0x00103750 File Offset: 0x00101950
		private void SaxIndustrial(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[0, 5], null, null);
						Mech mech = this.gameManager.gameBoard.hexMap[0, 6].GetOwnerMechs()[0];
						List<Unit> list = new List<Unit>();
						foreach (Worker worker in mech.position.GetOwnerWorkers())
						{
							list.Add(worker);
						}
						this.gameManager.moveManager.SelectUnit(mech);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 6], this.HexResources(this.gameManager.gameBoard.hexMap[0, 6]), list);
						this.gameManager.moveManager.UnloadAllWorkersFromMech(mech);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CFB RID: 11515 RVA: 0x00103898 File Offset: 0x00101A98
		private void SaxEngine(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.food,
						ResourceType.food
					}
				});
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[0, 6], null, null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[0, 6].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 6], this.HexResources(this.gameManager.gameBoard.hexMap[0, 6]), null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 7:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.oil,
						ResourceType.metal
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CFC RID: 11516 RVA: 0x00103A7C File Offset: 0x00101C7C
		private void SaxPatriot(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[0, 6], null, null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[0, 6].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 6], this.HexResources(this.gameManager.gameBoard.hexMap[0, 6]), null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Coin], "Kickstart"));
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove2 = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove2);
						List<Worker> ownerWorkers = this.gameManager.gameBoard.hexMap[1, 6].GetOwnerWorkers();
						this.gameManager.moveManager.SelectUnit(ownerWorkers[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[0, 6], null, null);
						this.gameManager.moveManager.SelectUnit(ownerWorkers[1]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[0, 6], null, null);
						this.gameManager.moveManager.SelectUnit(ownerWorkers[2]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 7], null, null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 7:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 8:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Kickstart"));
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[0, 6];
				return;
			case 9:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove3 = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove3);
						if (this.gameManager.gameBoard.hexMap[0, 6].GetOwnerMechs().Count > 0)
						{
							List<Unit> list = new List<Unit>();
							foreach (Worker worker in this.gameManager.gameBoard.hexMap[0, 6].GetOwnerWorkers())
							{
								list.Add(worker);
							}
							Mech mech = this.gameManager.gameBoard.hexMap[0, 6].GetOwnerMechs()[0];
							Dictionary<GameHex, int> dictionary = new Dictionary<GameHex, int>();
							mech.position.gameBoard.MoveRange(mech, 2, out dictionary);
							GameHex gameHex = this.gameManager.gameBoard.hexMap[0, 6];
							foreach (GameHex gameHex2 in dictionary.Keys)
							{
								if (gameHex2.hexType == HexType.farm)
								{
									if (gameHex.hexType != HexType.farm)
									{
										gameHex = gameHex2;
									}
									else if (gameHex2.hasTunnel && !gameHex.hasTunnel)
									{
										gameHex = gameHex2;
									}
								}
							}
							this.gameManager.moveManager.SelectUnit(mech);
							this.gameManager.moveManager.MoveSelectedUnit(gameHex, null, list);
							this.gameManager.moveManager.UnloadAllWorkersFromMech(mech);
						}
						if (this.gameManager.gameBoard.hexMap[1, 6].GetOwnerWorkers().Count > 0)
						{
							this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[1, 6].GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[0, 6], null, null);
						}
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 6], null, null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CFD RID: 11517 RVA: 0x00103CD8 File Offset: 0x00101ED8
		private void SaxMech(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			if (!player.strategicAnalysis.objectiveBalancedWorkforce)
			{
				switch (this.gameManager.TurnCount)
				{
				case 0:
					actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
					return;
				case 1:
					actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
					{
						tradeResource = new ResourceType[2]
					});
					return;
				case 2:
					actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
					{
						moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
						{
							GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
							this.gameManager.moveManager.SetMoveAction(gainMove);
							this.gameManager.moveManager.SelectUnit(player.player.character);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[0, 6], null, null);
							this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[1, 7].GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 6], this.HexResources(this.gameManager.gameBoard.hexMap[1, 7]), null);
							this.gameManager.moveManager.Clear();
							this.gameManager.actionManager.PrepareNextAction();
						}
					});
					return;
				case 3:
					actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
					{
						tradeResource = new ResourceType[2]
					});
					return;
				case 4:
					actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
					return;
				case 5:
					actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
					{
						tradeResource = new ResourceType[2]
					});
					return;
				case 6:
					actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
					return;
				case 7:
					actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
					{
						tradeResource = new ResourceType[2]
					});
					return;
				case 8:
					actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Kickstart"));
					return;
				default:
					return;
				}
			}
			else
			{
				switch (this.gameManager.TurnCount)
				{
				case 0:
					actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
					return;
				case 1:
					actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
					{
						tradeResource = new ResourceType[2]
					});
					return;
				case 2:
					actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
					{
						moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
						{
							GainMove gainMove2 = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
							this.gameManager.moveManager.SetMoveAction(gainMove2);
							this.gameManager.moveManager.SelectUnit(player.player.character);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[0, 6], null, null);
							this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[0, 6].GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 7], null, null);
							this.gameManager.moveManager.Clear();
							this.gameManager.actionManager.PrepareNextAction();
						}
					});
					return;
				case 3:
					actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
					{
						tradeResource = new ResourceType[2]
					});
					return;
				case 4:
					actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
					return;
				case 5:
					actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
					{
						tradeResource = new ResourceType[]
						{
							ResourceType.food,
							ResourceType.food
						}
					});
					return;
				case 6:
					actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
					return;
				case 7:
					actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
					{
						tradeResource = new ResourceType[]
						{
							ResourceType.food,
							ResourceType.food
						}
					});
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x06002CFE RID: 11518 RVA: 0x001040B4 File Offset: 0x001022B4
		private void SaxAgro(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[0, 6], null, null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[1, 7].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 6], this.HexResources(this.gameManager.gameBoard.hexMap[1, 7]), null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CFF RID: 11519 RVA: 0x00104270 File Offset: 0x00102470
		private void PolaniaPatriot(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 3], null, null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[0, 4].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 4], this.HexResources(this.gameManager.gameBoard.hexMap[0, 4]), null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.wood,
						ResourceType.wood
					}
				});
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove2 = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove2);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 4], null, null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[1, 4].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 3], null, null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[1, 4].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 3], null, null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 7:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 8:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D00 RID: 11520 RVA: 0x0010448C File Offset: 0x0010268C
		private void PolaniaEngine(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.wood,
						ResourceType.wood
					}
				});
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Kickstart"));
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 3], null, null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[1, 3].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 4], null, null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D01 RID: 11521 RVA: 0x001045EC File Offset: 0x001027EC
		private void PolaniaAgro(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 3], null, null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[0, 4].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 4], null, null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.wood,
						ResourceType.wood
					}
				});
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove2 = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove2);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 4], null, null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[1, 4].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 3], null, null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[1, 4].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 3], null, null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D02 RID: 11522 RVA: 0x001047A4 File Offset: 0x001029A4
		private void PolaniaIndustrial(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.wood,
						ResourceType.wood
					}
				});
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 3], null, null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[0, 4].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 4], this.HexResources(this.gameManager.gameBoard.hexMap[0, 4]), null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.wood
					}
				});
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 7:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove2 = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove2);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[1, 3].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 4], this.HexResources(this.gameManager.gameBoard.hexMap[1, 3]), null);
						Mech mech = this.gameManager.gameBoard.hexMap[1, 4].GetOwnerMechs()[0];
						Dictionary<GameHex, int> dictionary = new Dictionary<GameHex, int>();
						mech.position.gameBoard.MoveRange(mech, 2, out dictionary);
						GameHex gameHex = mech.position;
						foreach (GameHex gameHex2 in dictionary.Keys)
						{
							if (gameHex2.hexType == HexType.mountain)
							{
								if (gameHex.hexType != HexType.mountain)
								{
									gameHex = gameHex2;
								}
								else if (gameHex2.hasTunnel && !gameHex.hasTunnel)
								{
									gameHex = gameHex2;
								}
							}
						}
						List<Unit> list = new List<Unit>();
						foreach (Worker worker in this.gameManager.gameBoard.hexMap[1, 4].GetOwnerWorkers())
						{
							list.Add(worker);
						}
						this.gameManager.moveManager.SelectUnit(mech);
						this.gameManager.moveManager.MoveSelectedUnit(gameHex, this.HexResources(mech.position), list);
						this.gameManager.moveManager.UnloadAllWorkersFromMech(mech);
						if (gameHex.hasTunnel)
						{
							Dictionary<GameHex, int> dictionary2 = new Dictionary<GameHex, int>();
							mech.position.gameBoard.MoveRange(mech, 1, out dictionary2);
							GameHex gameHex3 = mech.position;
							foreach (GameHex gameHex4 in dictionary.Keys)
							{
								if (gameHex4.hexType == HexType.tundra)
								{
									if (gameHex3.hexType != HexType.tundra)
									{
										gameHex3 = gameHex4;
									}
									else if (gameHex4.hasTunnel && !gameHex3.hasTunnel)
									{
										gameHex3 = gameHex4;
									}
								}
							}
							List<Unit> list2 = new List<Unit>();
							foreach (Worker worker2 in gameHex.GetOwnerWorkers())
							{
								list2.Add(worker2);
							}
							list2.RemoveRange(0, 3);
							this.gameManager.moveManager.SelectUnit(mech);
							this.gameManager.moveManager.MoveSelectedUnit(gameHex3, this.HexResources(gameHex), list2);
							this.gameManager.moveManager.UnloadAllWorkersFromMech(mech);
						}
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D03 RID: 11523 RVA: 0x00104998 File Offset: 0x00102B98
		private void PolaniaMech(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				// Turn 1: Trade for 2 metal
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Polonia Mech Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 1:
				// Turn 2: Move hero to forest (1,3), move worker from forest (1,3) to village (1,4)
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Polonia Mech Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						
						// Move hero to forest (1,3)
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 3], null, null);
						
						// Move worker from forest (1,3) to village (1,4) - carrying resources
						Worker worker = this.gameManager.gameBoard.hexMap[1, 3].GetOwnerWorkers()[0];
						this.gameManager.moveManager.SelectUnit(worker);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 4], this.HexResources(this.gameManager.gameBoard.hexMap[1, 3]), null);
						
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 2:
				// Turn 3: Trade for 2 metal
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Polonia Mech Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 3:
				// Turn 4: Move hero to village (1,4), move worker from farm (0,4) to village (1,4)
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Polonia Mech Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						
						// Move hero to village (1,4)
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 4], null, null);
						
						// Move worker from farm (0,4) to village (1,4) - carrying resources
						Worker worker = this.gameManager.gameBoard.hexMap[0, 4].GetOwnerWorkers()[0];
						this.gameManager.moveManager.SelectUnit(worker);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 4], this.HexResources(this.gameManager.gameBoard.hexMap[0, 4]), null);
						
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 4:
				// Turn 5: Gain power, deploy mech
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Polonia Mech Kickstart"));
				return;
			case 5:
				// Turn 6: Produce
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Polonia Mech Kickstart"));
				return;
			case 6:
				// Turn 7: Move hero to mountain (2,3), move mech with all workers to mountain (2,3)
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Polonia Mech Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						
						// Move hero to mountain (2,3)
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[2, 3], null, null);
						
						// Get mech and all workers at village (1,4)
						Mech mech = this.gameManager.gameBoard.hexMap[1, 4].GetOwnerMechs()[0];
						List<Worker> workers = this.gameManager.gameBoard.hexMap[1, 4].GetOwnerWorkers();
						
						// Select mech and all workers
						this.gameManager.moveManager.SelectUnit(mech);
						foreach (Worker worker in workers)
						{
							this.gameManager.moveManager.SelectUnit(worker);
						}
						
						// Move mech with all workers to mountain (2,3) - workers carry resources
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[2, 3], this.HexResources(this.gameManager.gameBoard.hexMap[1, 4]), null);
						
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 7:
				// Turn 8: Produce
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Polonia Mech Kickstart"));
				return;
			case 8:
				// Turn 9: Gain power, deploy mech
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Polonia Mech Kickstart"));
				return;
			case 9:
				// Turn 10: Move hero to factory (3,4), move mech to factory (3,4)
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Polonia Mech Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						
						// Move hero to factory (3,4)
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 4], null, null);
						
						// Move mech to factory (3,4)
						Mech mech = this.gameManager.gameBoard.hexMap[2, 3].GetOwnerMechs()[0];
						this.gameManager.moveManager.SelectUnit(mech);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 4], null, null);
						
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D04 RID: 11524 RVA: 0x00104B48 File Offset: 0x00102D48
		private void NordInnovative(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			if (this.gameManager.TurnCount == 0)
			{
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[4, 1].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 1], this.HexResources(this.gameManager.gameBoard.hexMap[4, 1]), null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[5, 1].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 2], this.HexResources(this.gameManager.gameBoard.hexMap[5, 1]), null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
			}
		}

		// Token: 0x06002D05 RID: 11525 RVA: 0x00104BB0 File Offset: 0x00102DB0
		private void NordEngine(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				// Turn 1: Move workers to village and mountain
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Nordic Engine Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[5, 1].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[6, 1], this.HexResources(this.gameManager.gameBoard.hexMap[5, 1]), null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[4, 1].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 2], this.HexResources(this.gameManager.gameBoard.hexMap[4, 1]), null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 1:
				// Turn 2: Trade for 2 metal
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Nordic Engine Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 2:
				// Turn 3: Produce
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Nordic Engine Kickstart"));
				return;
			case 3:
				// Turn 4: Trade for 2 metal and deploy mech on village at 6,1
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Nordic Engine Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 4:
				// Turn 5: Move hero to mountain encounter at 5,2
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Nordic Engine Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 2], null, null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 5:
				// Turn 6: Produce
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Nordic Engine Kickstart"));
				return;
			case 6:
				// Turn 7: Trade for 2 metal and deploy mech on mountain at 5,2
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Nordic Engine Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 7:
				// Turn 8: Move hero to factory at 4,4
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Nordic Engine Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 4], null, null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D06 RID: 11526 RVA: 0x00104D48 File Offset: 0x00102F48
		private void NordIndustrial(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				// Turn 1: Move workers - one from forest (4,1) to village (3,1), one from tundra (5,1) to mountain (2,2)
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Nordic Industrial Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						// Move worker from forest (4,1) to village (3,1)
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[4, 1].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 2], this.HexResources(this.gameManager.gameBoard.hexMap[4, 1]), null);
						// Move worker from tundra (5,1) to mountain (2,2)
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[5, 1].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[6, 1], this.HexResources(this.gameManager.gameBoard.hexMap[5, 1]), null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 1:
				// Turn 2: Trade for 2 metal
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Nordic Industrial Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 2:
				// Turn 3: Produce and deploy mech on village at (3,1)
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Nordic Industrial Kickstart"));
				return;
			case 3:
				// Turn 4: Move hero to mountain encounter at (2,2)
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Nordic Industrial Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 2], null, null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 4:
				// Turn 5: Trade for 2 metal
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Nordic Industrial Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 5:
				// Turn 6: Produce and deploy mech on mountain at (2,2)
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Nordic Industrial Kickstart"));
				return;
			case 6:
				// Turn 7: Move hero to factory at (3,4)
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Nordic Industrial Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 4], null, null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D07 RID: 11527 RVA: 0x00104EEC File Offset: 0x001030EC
		private void NordMech(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.wood,
						ResourceType.wood
					}
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[4, 1].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 1], this.HexResources(this.gameManager.gameBoard.hexMap[4, 1]), null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[5, 1].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 2], this.HexResources(this.gameManager.gameBoard.hexMap[5, 1]), null);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 1], null, null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 7:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D08 RID: 11528 RVA: 0x001050CC File Offset: 0x001032CC
		private void NordAgro(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.wood,
						ResourceType.wood
					}
				});
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D09 RID: 11529 RVA: 0x00105174 File Offset: 0x00103374
		private void NordPatriot(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[4, 1].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 2], this.HexResources(this.gameManager.gameBoard.hexMap[4, 1]), null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[5, 1].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[6, 1], this.HexResources(this.gameManager.gameBoard.hexMap[5, 1]), null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Kickstart"));
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D0A RID: 11530 RVA: 0x000FE45C File Offset: 0x000FC65C
		public Dictionary<ResourceType, int> HexResources(GameHex hex)
		{
			Dictionary<ResourceType, int> dictionary = null;
			if (hex.GetResourceCount() > 0)
			{
				dictionary = new Dictionary<ResourceType, int>();
				dictionary.Add(ResourceType.oil, hex.resources[ResourceType.oil]);
				dictionary.Add(ResourceType.metal, hex.resources[ResourceType.metal]);
				dictionary.Add(ResourceType.food, hex.resources[ResourceType.food]);
				dictionary.Add(ResourceType.wood, hex.resources[ResourceType.wood]);
			}
			return dictionary;
		}

		// Token: 0x04001EA0 RID: 7840
		private GameManager gameManager;
	}
}
