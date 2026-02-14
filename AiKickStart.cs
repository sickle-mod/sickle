using System;
using System.Collections.Generic;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x02000568 RID: 1384
	public class AiKickStart
	{
		// Token: 0x06002C98 RID: 11416
		public AiKickStart(GameManager gameManager)
		{
			this.gameManager = gameManager;
		}

		// Token: 0x06002C99 RID: 11417
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
			else if (player.player.matFaction.faction == Faction.Polania)
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
			else
			{
				if (player.player.matFaction.faction != Faction.Nordic)
				{
					if (player.player.matFaction.faction == Faction.Rusviet)
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
					else if (player.player.matFaction.faction == Faction.Albion)
					{
						if (player.player.matPlayer.matType == PlayerMatType.Militant)
						{
							this.AlbionMilitant(actionOptions, player);
							return;
						}
					}
					else if (player.player.matFaction.faction == Faction.Togawa && player.player.matPlayer.matType == PlayerMatType.Militant)
					{
						this.TogawaMilitant(actionOptions, player);
					}
					return;
				}
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
				default:
					return;
				}
			}
		}

		// Token: 0x06002C9A RID: 11418
		private void TogawaMilitant(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
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
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
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
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[6, 7], null, null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[6, 7].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 7], this.HexResources(this.gameManager.gameBoard.hexMap[6, 7]), null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[6, 6].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[6, 7], this.HexResources(this.gameManager.gameBoard.hexMap[6, 6]), null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002C9B RID: 11419
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
				}
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[3, 1];
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

		// Token: 0x06002C9C RID: 11420
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

		// Token: 0x06002C9D RID: 11421
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

		// Token: 0x06002C9E RID: 11422
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

		// Token: 0x06002C9F RID: 11423
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

		// Token: 0x06002CA0 RID: 11424
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

		// Token: 0x06002CA1 RID: 11425
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

		// Token: 0x06002CA2 RID: 11426
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

		// Token: 0x06002CA3 RID: 11427
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
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.oil,
						ResourceType.wood
					}
				});
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Kickstart"));
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.wood,
						ResourceType.wood
					}
				});
				return;
			case 7:
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

		// Token: 0x06002CA4 RID: 11428
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

		// Token: 0x06002CA5 RID: 11429
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
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[1, 6].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[0, 6], null, null);
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

		// Token: 0x06002CA6 RID: 11430
		private void SaxMech(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
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

		// Token: 0x06002CA7 RID: 11431
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

		// Token: 0x06002CA8 RID: 11432
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
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[0, 4], this.HexResources(aiPlayer.player.character.position), null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[1, 4].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[0, 4], null, null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[1, 4].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[0, 4], null, null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CA9 RID: 11433
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
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[0, 4].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 4], null, null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CAA RID: 11434
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
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[0, 4], this.HexResources(aiPlayer.player.character.position), null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[1, 4].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[0, 4], null, null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[1, 4].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[0, 4], null, null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002CAB RID: 11435
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

		// Token: 0x06002CAC RID: 11436
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

		// Token: 0x06002CAD RID: 11437
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

		// Token: 0x06002CAE RID: 11438
		private void NordIndustrial(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Nordic Industrial Kickstart")
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
			case 1:
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
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Nordic Industrial Kickstart"));
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Nordic Industrial Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove2 = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove2);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 2], null, null);
						this.gameManager.moveManager.Clear();
						this.gameManager.actionManager.PrepareNextAction();
					}
				});
				return;
			case 4:
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
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Nordic Industrial Kickstart"));
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Nordic Industrial Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove3 = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove3);
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

		// Token: 0x06002CAF RID: 11439
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

		// Token: 0x06002CB0 RID: 11440
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

		// Token: 0x06002CB1 RID: 11441
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

		// Token: 0x06002CB2 RID: 11442
		private Dictionary<ResourceType, int> HexResources(GameHex hex)
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

		// Token: 0x04001E77 RID: 7799
		private GameManager gameManager;
	}
}
