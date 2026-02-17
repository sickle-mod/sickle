using System;
using System.Collections.Generic;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x02000568 RID: 1384
	public class AiKickStart
	{
		// Token: 0x06002C98 RID: 11416 RVA: 0x0004454B File Offset: 0x0004274B
		public AiKickStart(GameManager gameManager)
		{
			this.gameManager = gameManager;
		}

		// Token: 0x06002C99 RID: 11417 RVA: 0x000FB9F0 File Offset: 0x000F9BF0
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
				if (player.player.matPlayer.matType == PlayerMatType.Militant)
				{
					this.AlbionMilitant(actionOptions, player);
					return;
				}
			}
			else if (player.player.matFaction.faction == Faction.Togawa)
			{
				if (player.player.matPlayer.matType == PlayerMatType.Militant)
				{
					this.TogawaMilitant(actionOptions, player);
					return;
				}
			}
		}

		// Token: 0x06002C9A RID: 11418 RVA: 0x000FBC5C File Offset: 0x000F9E5C
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

		// Token: 0x06002C9B RID: 11419 RVA: 0x000FBD9C File Offset: 0x000F9F9C
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

		// Token: 0x06002C9C RID: 11420 RVA: 0x000FC000 File Offset: 0x000FA200
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

		// Token: 0x06002C9D RID: 11421 RVA: 0x000FC0D4 File Offset: 0x000FA2D4
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

		// Token: 0x06002C9E RID: 11422 RVA: 0x000FC210 File Offset: 0x000FA410
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

		// Token: 0x06002C9F RID: 11423 RVA: 0x000FC408 File Offset: 0x000FA608
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

		// Token: 0x06002CA0 RID: 11424 RVA: 0x000FC45C File Offset: 0x000FA65C
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

		// Token: 0x06002CA1 RID: 11425 RVA: 0x000FC690 File Offset: 0x000FA890
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

		// Token: 0x06002CA2 RID: 11426 RVA: 0x000FC7E8 File Offset: 0x000FA9E8
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

		// Token: 0x06002CA3 RID: 11427 RVA: 0x000FC9D4 File Offset: 0x000FABD4
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

		// Token: 0x06002CA4 RID: 11428 RVA: 0x000FCBC8 File Offset: 0x000FADC8
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

		// Token: 0x06002CA5 RID: 11429 RVA: 0x000FCDAC File Offset: 0x000FAFAC
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

		// Token: 0x06002CA6 RID: 11430 RVA: 0x000FD008 File Offset: 0x000FB208
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

		// Token: 0x06002CA7 RID: 11431 RVA: 0x000FD208 File Offset: 0x000FB408
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

		// Token: 0x06002CA8 RID: 11432 RVA: 0x000FD3C4 File Offset: 0x000FB5C4
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

		// Token: 0x06002CA9 RID: 11433 RVA: 0x000FD5F0 File Offset: 0x000FB7F0
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

		// Token: 0x06002CAA RID: 11434 RVA: 0x000FD724 File Offset: 0x000FB924
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

		// Token: 0x06002CAB RID: 11435 RVA: 0x000FD950 File Offset: 0x000FBB50
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

		// Token: 0x06002CAC RID: 11436 RVA: 0x000FDB44 File Offset: 0x000FBD44
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

		// Token: 0x06002CAD RID: 11437 RVA: 0x000FDCF4 File Offset: 0x000FBEF4
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

		// Token: 0x06002CAE RID: 11438 RVA: 0x000FDE8C File Offset: 0x000FC08C
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
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 1], this.HexResources(this.gameManager.gameBoard.hexMap[4, 1]), null);
						// Move worker from tundra (5,1) to mountain (2,2)
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[5, 1].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[2, 2], this.HexResources(this.gameManager.gameBoard.hexMap[5, 1]), null);
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
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[2, 2], null, null);
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

		// Token: 0x06002CAF RID: 11439 RVA: 0x000FE030 File Offset: 0x000FC230
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

		// Token: 0x06002CB0 RID: 11440 RVA: 0x000FE210 File Offset: 0x000FC410
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

		// Token: 0x06002CB1 RID: 11441 RVA: 0x000FE2B8 File Offset: 0x000FC4B8
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

		// Token: 0x06002CB2 RID: 11442 RVA: 0x000FE45C File Offset: 0x000FC65C
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
