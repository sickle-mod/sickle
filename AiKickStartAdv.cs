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

		// Token: 0x06002CE7 RID: 11495 RVA: 0x001007C0 File Offset: 0x000FE9C0
		public void KickStart(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			if (player.player.matFaction.faction == Faction.Saxony)
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
			else
			{
				if (player.player.matFaction.faction != Faction.Polania)
				{
					if (player.player.matFaction.faction == Faction.Nordic)
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
					return;
				}
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
		}

		// Token: 0x06002CE8 RID: 11496 RVA: 0x00100AD0 File Offset: 0x000FECD0
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

		// Token: 0x06002CE9 RID: 11497 RVA: 0x00100CC8 File Offset: 0x000FEEC8
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

		// Token: 0x06002CEA RID: 11498 RVA: 0x00100E2C File Offset: 0x000FF02C
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

		// Token: 0x06002CEB RID: 11499 RVA: 0x001010D4 File Offset: 0x000FF2D4
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

		// Token: 0x06002CEC RID: 11500 RVA: 0x00101344 File Offset: 0x000FF544
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

		// Token: 0x06002CED RID: 11501 RVA: 0x0010152C File Offset: 0x000FF72C
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

		// Token: 0x06002CEE RID: 11502 RVA: 0x0010162C File Offset: 0x000FF82C
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

		// Token: 0x06002CEF RID: 11503 RVA: 0x00101730 File Offset: 0x000FF930
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

		// Token: 0x06002CF0 RID: 11504 RVA: 0x001017E8 File Offset: 0x000FF9E8
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

		// Token: 0x06002CF1 RID: 11505 RVA: 0x001018C4 File Offset: 0x000FFAC4
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

		// Token: 0x06002CF2 RID: 11506 RVA: 0x00101998 File Offset: 0x000FFB98
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

		// Token: 0x06002CF3 RID: 11507 RVA: 0x00101B68 File Offset: 0x000FFD68
		private void CrimeaIndustrial(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
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
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.CombatCard], "Kickstart"));
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.CombatCard], "Kickstart"));
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CF4 RID: 11508 RVA: 0x00101C3C File Offset: 0x000FFE3C
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

		// Token: 0x06002CF5 RID: 11509 RVA: 0x00101D78 File Offset: 0x000FFF78
		private void CrimeaAgro(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
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
					tradeResource = new ResourceType[]
					{
						ResourceType.wood,
						ResourceType.wood
					}
				});
				return;
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
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 8], null, null);
						Worker worker = this.gameManager.gameBoard.hexMap[3, 8].GetOwnerWorkers()[0];
						Dictionary<GameHex, int> dictionary = new Dictionary<GameHex, int>();
						worker.position.gameBoard.MoveRange(worker, 1, out dictionary);
						GameHex gameHex = worker.position;
						foreach (GameHex gameHex2 in dictionary.Keys)
						{
							if (gameHex2.hexType == HexType.forest && gameHex.hexType != HexType.forest)
							{
								gameHex = gameHex2;
							}
						}
						this.gameManager.moveManager.SelectUnit(worker);
						this.gameManager.moveManager.MoveSelectedUnit(gameHex, null, null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 7:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove2 = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove2);
						Worker worker2 = this.gameManager.gameBoard.hexMap[3, 8].GetOwnerWorkers()[0];
						Worker worker3 = this.gameManager.gameBoard.hexMap[3, 8].GetOwnerWorkers()[1];
						Worker worker4 = this.gameManager.gameBoard.hexMap[3, 8].GetOwnerWorkers()[2];
						Dictionary<GameHex, int> dictionary2 = new Dictionary<GameHex, int>();
						worker2.position.gameBoard.MoveRange(worker2, 1, out dictionary2);
						GameHex gameHex3 = worker2.position;
						foreach (GameHex gameHex4 in dictionary2.Keys)
						{
							if (gameHex4.hexType == HexType.forest && gameHex3.hexType != HexType.forest)
							{
								gameHex3 = gameHex4;
							}
						}
						this.gameManager.moveManager.SelectUnit(worker2);
						this.gameManager.moveManager.MoveSelectedUnit(gameHex3, null, null);
						this.gameManager.moveManager.SelectUnit(worker3);
						this.gameManager.moveManager.MoveSelectedUnit(gameHex3, null, null);
						this.gameManager.moveManager.SelectUnit(worker4);
						this.gameManager.moveManager.MoveSelectedUnit(gameHex3, null, null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CF6 RID: 11510 RVA: 0x00101F70 File Offset: 0x00100170
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

		// Token: 0x06002CF7 RID: 11511 RVA: 0x00101FC4 File Offset: 0x001001C4
		private void RusvietPatriot(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
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
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[6, 3], null, null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[6, 4].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 4], this.HexResources(this.gameManager.gameBoard.hexMap[6, 4]), null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
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
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove2 = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove2);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 4], null, null);
						Worker worker = this.gameManager.gameBoard.hexMap[6, 3].GetOwnerWorkers()[0];
						Worker worker2 = this.gameManager.gameBoard.hexMap[6, 3].GetOwnerWorkers()[1];
						this.gameManager.moveManager.SelectUnit(worker);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 4], null, null);
						this.gameManager.moveManager.SelectUnit(worker2);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[6, 4], null, null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 7:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 8:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove3 = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove3);
						Worker worker3 = this.gameManager.gameBoard.hexMap[6, 3].GetOwnerWorkers()[0];
						Worker worker4 = this.gameManager.gameBoard.hexMap[6, 3].GetOwnerWorkers()[1];
						this.gameManager.moveManager.SelectUnit(worker3);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[6, 4], null, null);
						this.gameManager.moveManager.SelectUnit(worker4);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[6, 4], null, null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CF8 RID: 11512 RVA: 0x001021F8 File Offset: 0x001003F8
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

		// Token: 0x06002CF9 RID: 11513 RVA: 0x00102350 File Offset: 0x00100550
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

		// Token: 0x06002CFA RID: 11514 RVA: 0x0010253C File Offset: 0x0010073C
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

		// Token: 0x06002CFB RID: 11515 RVA: 0x00102684 File Offset: 0x00100884
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

		// Token: 0x06002CFC RID: 11516 RVA: 0x00102868 File Offset: 0x00100A68
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

		// Token: 0x06002CFD RID: 11517 RVA: 0x00102AC4 File Offset: 0x00100CC4
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

		// Token: 0x06002CFE RID: 11518 RVA: 0x00102EA0 File Offset: 0x001010A0
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

		// Token: 0x06002CFF RID: 11519 RVA: 0x0010305C File Offset: 0x0010125C
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

		// Token: 0x06002D00 RID: 11520 RVA: 0x00103278 File Offset: 0x00101478
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

		// Token: 0x06002D01 RID: 11521 RVA: 0x001033D8 File Offset: 0x001015D8
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

		// Token: 0x06002D02 RID: 11522 RVA: 0x00103590 File Offset: 0x00101790
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

		// Token: 0x06002D03 RID: 11523 RVA: 0x00103784 File Offset: 0x00101984
		private void PolaniaMech(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
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
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.oil,
						ResourceType.wood
					}
				});
				return;
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

		// Token: 0x06002D04 RID: 11524 RVA: 0x00103934 File Offset: 0x00101B34
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

		// Token: 0x06002D05 RID: 11525 RVA: 0x0010399C File Offset: 0x00101B9C
		private void NordEngine(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
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
						ResourceType.wood,
						ResourceType.wood
					}
				});
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Kickstart"));
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[4, 1].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 1], this.HexResources(this.gameManager.gameBoard.hexMap[4, 1]), null);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 1], this.HexResources(this.gameManager.gameBoard.hexMap[5, 1]), null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D06 RID: 11526 RVA: 0x00103B34 File Offset: 0x00101D34
		private void NordIndustrial(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
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
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Kickstart"));
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Kickstart"));
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
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D07 RID: 11527 RVA: 0x00103CD8 File Offset: 0x00101ED8
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

		// Token: 0x06002D08 RID: 11528 RVA: 0x00103EB8 File Offset: 0x001020B8
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

		// Token: 0x06002D09 RID: 11529 RVA: 0x00103F60 File Offset: 0x00102160
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
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 1], this.HexResources(this.gameManager.gameBoard.hexMap[4, 1]), null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[5, 1].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 2], this.HexResources(this.gameManager.gameBoard.hexMap[5, 1]), null);
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

		// Token: 0x06002D0A RID: 11530 RVA: 0x000FD248 File Offset: 0x000FB448
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
