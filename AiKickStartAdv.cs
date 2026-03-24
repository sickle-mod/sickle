using System;
using System.Collections.Generic;
using Scythe.GameLogic.Actions;

namespace Scythe.GameLogic
{
	// Token: 0x02000582 RID: 1410
	public class AiKickStartAdv
	{
		// Token: 0x06002D09 RID: 11529 RVA: 0x00044590 File Offset: 0x00042790
		public AiKickStartAdv(GameManager gameManager)
		{
			this.gameManager = gameManager;
		}

		// Token: 0x06002D0A RID: 11530 RVA: 0x00103370 File Offset: 0x00101570
		public void KickStart(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
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
						this.NordMilitant(actionOptions, player);
						return;
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
						this.RusvietEngine(actionOptions, player);
						return;
					case PlayerMatType.Patriotic:
						this.RusvietPatriot(actionOptions, player);
						return;
					case PlayerMatType.Mechanical:
						this.RusvietMech(actionOptions, player);
						return;
					case PlayerMatType.Agricultural:
						this.RusvietAgro(actionOptions, player);
						return;
					case PlayerMatType.Militant:
						this.RusvietMilitant(actionOptions, player);
						return;
					case PlayerMatType.Innovative:
						this.RusvietInnovative(actionOptions, player);
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
						this.CrimeaPatriot(actionOptions, player);
						return;
					case PlayerMatType.Mechanical:
						this.CrimeaMech(actionOptions, player);
						return;
					case PlayerMatType.Agricultural:
						this.CrimeaAgro(actionOptions, player);
						return;
					case PlayerMatType.Militant:
						this.CrimeaMilitant(actionOptions, player);
						return;
					case PlayerMatType.Innovative:
						this.CrimeaInnovative(actionOptions, player);
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
					case PlayerMatType.Militant:
						this.SaxMilitant(actionOptions, player);
						return;
					case PlayerMatType.Innovative:
						this.SaxInnovative(actionOptions, player);
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
						this.AlbionEngine(actionOptions, player);
						return;
					case PlayerMatType.Patriotic:
						this.AlbionPatriot(actionOptions, player);
						return;
					case PlayerMatType.Mechanical:
						this.AlbionMech(actionOptions, player);
						return;
					case PlayerMatType.Agricultural:
						this.AlbionAgro(actionOptions, player);
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
				else
				{
					if (player.player.matFaction.faction != Faction.Togawa)
					{
						return;
					}
					switch (player.player.matPlayer.matType)
					{
					case PlayerMatType.Industrial:
						this.TogawaIndustrial(actionOptions, player);
						return;
					case PlayerMatType.Engineering:
						this.TogawaEngineering(actionOptions, player);
						return;
					case PlayerMatType.Patriotic:
						this.TogawaPatriot(actionOptions, player);
						return;
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
			else
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
				case PlayerMatType.Militant:
					this.PolaniaMilitant(actionOptions, player);
					return;
				case PlayerMatType.Innovative:
					this.PolaniaInnovative(actionOptions, player);
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x06002D0B RID: 11531 RVA: 0x0010372C File Offset: 0x0010192C
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
						}
					});
					return;
				}
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
			default:
				return;
			}
		}

		// Token: 0x06002D0C RID: 11532 RVA: 0x00103920 File Offset: 0x00101B20
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
						}
					});
					return;
				}
				return;
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
			default:
				return;
			}
		}

		// Token: 0x06002D0D RID: 11533 RVA: 0x00103A88 File Offset: 0x00101C88
		private void AlbionIndustrial(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Albion Industrial Kickstart: Trade 2 Oil")
				{
					tradeResource = new ResourceType[] { ResourceType.oil, ResourceType.oil }
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Albion Industrial Kickstart: Move Hero->[2,1], Worker [1,1]->[1,2]")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						// Hero to [2,1]
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[2, 1], null, null);
						// Worker from [1,1] to [1,2]
						var hex11 = this.gameManager.gameBoard.hexMap[1, 1];
						if (hex11.GetOwnerWorkers().Count > 0)
						{
							this.gameManager.moveManager.SelectUnit(hex11.GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 2], this.HexResources(hex11), null);
						}
					}
				});
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Albion Industrial Kickstart: Trade 1 Oil, 1 Metal")
				{
					tradeResource = new ResourceType[] { ResourceType.oil, ResourceType.metal }
				});
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Albion Industrial Kickstart: Bolster / Upgrade"));
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Albion Industrial Kickstart: Move Hero->[1,2], Worker [2,1]->[3,1], Worker [1,2]->[2,3]")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						// Hero from [2,1] to [1,2]
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 2], null, null);
						// Worker from [2,1] to [3,1]
						var hex21 = this.gameManager.gameBoard.hexMap[2, 1];
						if (hex21.GetOwnerWorkers().Count > 0)
						{
							this.gameManager.moveManager.SelectUnit(hex21.GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 1], this.HexResources(hex21), null);
						}
						// Worker from [1,2] to [2,3]
						var hex12 = this.gameManager.gameBoard.hexMap[1, 2];
						if (hex12.GetOwnerWorkers().Count > 0)
						{
							this.gameManager.moveManager.SelectUnit(hex12.GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[2, 3], this.HexResources(hex12), null);
						}
					}
				});
				return;
			case 5:
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[2, 3];
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Albion Industrial Kickstart: Produce / Deploy at [2,3]"));
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Albion Industrial Kickstart: Move Mech+1W [2,3]->[3,3], Worker [3,3]->[3,4], Hero->[3,4]")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						
						// Mech carrying 1 worker from [2,3] to [3,3]
						var hex23 = this.gameManager.gameBoard.hexMap[2, 3];
						var hex33 = this.gameManager.gameBoard.hexMap[3, 3];
						var hex34 = this.gameManager.gameBoard.hexMap[3, 4];
						if (hex23.GetOwnerMechs().Count > 0)
						{
							Mech mech = hex23.GetOwnerMechs()[0];
							var workers = hex23.GetOwnerWorkers();
							var passengers = new List<Unit>();
							if (workers.Count > 0) passengers.Add(workers[0]);
							
							this.gameManager.moveManager.SelectUnit(mech);
							this.gameManager.moveManager.MoveSelectedUnit(hex33, this.HexResources(hex23), passengers);
							
							// Worker continues from [3,3] to [3,4]
							// Wait! Albion character has a special move? No, but maybe they have 2 moves now?
							// Actually, 1 unit move for mech/passenger, 1 for worker (if they can move separately), 1 for hero.
							// In Scythe, if you move a mech with passengers, it counts as 1 unit move.
							// Then if you unload and move the worker again, it requires another move.
							// But turn 6 is a Move action. Usually 2 units or 3 units.
							// Hero to [3,4] is 1.
							// Mech+Worker to [3,3] is 1.
							// Worker [3,3] to [3,4] is 1 (if the worker moves again).
							// Total 3 moves. Albion moves 2 (standard) or 3 (if upgraded).
							
							if (passengers.Count > 0)
							{
								this.gameManager.moveManager.SelectUnit(passengers[0]);
								this.gameManager.moveManager.MoveSelectedUnit(hex34, null, null);
							}
						}
						// Hero to [3,4]
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(hex34, null, null);
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D0E RID: 11534 RVA: 0x00103D34 File Offset: 0x00101F34
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
						}
					});
					return;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06002D0F RID: 11535 RVA: 0x00103FA8 File Offset: 0x001021A8
		private void AlbionMilitant(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[] { ResourceType.metal, ResourceType.metal }
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
						if (this.gameManager.gameBoard.hexMap[2, 1].GetOwnerWorkers().Count > 0)
						{
							this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[2, 1].GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 1], this.HexResources(this.gameManager.gameBoard.hexMap[2, 1]), null);
						}
						if (this.gameManager.gameBoard.hexMap[1, 1].GetOwnerWorkers().Count > 0)
						{
							this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[1, 1].GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[2, 1], this.HexResources(this.gameManager.gameBoard.hexMap[1, 1]), null);
						}
					}
				});
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[3, 1];
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Kickstart"));
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Kickstart"));
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 7:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						var hex = this.gameManager.gameBoard.hexMap[3, 1];
						if(hex.GetOwnerWorkers().Count > 0) {
							this.gameManager.moveManager.SelectUnit(hex.GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 1], null, null);
						}
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 1], null, null);
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D10 RID: 11536 RVA: 0x00104190 File Offset: 0x00102390
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
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D11 RID: 11537 RVA: 0x00104290 File Offset: 0x00102490
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
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 6], null, null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[6, 7].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 7], this.HexResources(this.gameManager.gameBoard.hexMap[6, 7]), null);
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

		// Token: 0x06002D12 RID: 11538 RVA: 0x00104394 File Offset: 0x00102594
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
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 6], null, null);
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D13 RID: 11539 RVA: 0x001044A8 File Offset: 0x001026A8
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

		// Token: 0x06002D14 RID: 11540 RVA: 0x00104584 File Offset: 0x00102784
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
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D15 RID: 11541 RVA: 0x00104658 File Offset: 0x00102858
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
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D16 RID: 11542 RVA: 0x00104828 File Offset: 0x00102A28
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
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[2, 6], null, null);
					}
				});
				return;
			case 6:
			{
				int maxValue = int.MaxValue;
				AiRecipe aiRecipe = new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart");
				AiRecipe aiRecipe2 = aiRecipe;
				ResourceType[] array = new ResourceType[2];
				array[0] = ResourceType.food;
				aiRecipe2.tradeResource = array;
				actionOptions.Add(maxValue, aiRecipe);
				return;
			}
			case 7:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Kickstart"));
				return;
			case 8:
			{
				int maxValue2 = int.MaxValue;
				AiRecipe aiRecipe3 = new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart");
				AiRecipe aiRecipe4 = aiRecipe3;
				ResourceType[] array2 = new ResourceType[2];
				array2[0] = ResourceType.food;
				aiRecipe4.tradeResource = array2;
				actionOptions.Add(maxValue2, aiRecipe3);
				return;
			}
			case 9:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Kickstart"));
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D17 RID: 11543 RVA: 0x00104A84 File Offset: 0x00102C84
		private void CrimeaEngine(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				// Produce
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart: Produce"));
				return;
			case 1:
				// Trade 2 grain (food)
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart: Trade 2 food")
				{
					tradeResource = new ResourceType[] { ResourceType.food, ResourceType.food }
				});
				return;
			case 2:
				// Move character to [3,7]; move worker from [3,8] to [3,7]
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart: Move character to [3,7], worker [3,8]->[3,7]")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						// Character to [3,7]
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 7], null, null);
						// Worker from [3,8] to [3,7]
						var hex38 = this.gameManager.gameBoard.hexMap[3, 8];
						if (hex38.GetOwnerWorkers().Count > 0)
						{
							this.gameManager.moveManager.SelectUnit(hex38.GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 7], null, null);
						}
					}
				});
				return;
			case 3:
				// Produce
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart: Produce"));
				return;
			case 4:
				// Move character from [3,7] to [4,7]; worker from [3,8] to [3,7]
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart: Move character [3,7]->[4,7], worker [3,8]->[3,7]")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						// Character from [3,7] to [4,7]
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 7], null, null);
						// Worker from [3,8] to [3,7]
						var hex38b = this.gameManager.gameBoard.hexMap[3, 8];
						if (hex38b.GetOwnerWorkers().Count > 0)
						{
							this.gameManager.moveManager.SelectUnit(hex38b.GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 7], null, null);
						}
					}
				});
				return;
			case 5:
				// Trade 2 oil
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart: Trade 2 oil")
				{
					tradeResource = new ResourceType[] { ResourceType.oil, ResourceType.oil }
				});
				return;
			case 6:
				// Produce
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart: Produce"));
				return;
			case 7:
				// Trade 2 metal, deploy Riverwalk (mech index 0) on [3,7]
				// Crimea Engineering mechPriority: [0]=8 (Riverwalk), [1]=4, [2]=6 (Speed), [3]=10 (Wayfare/Scout).
				// mechNext selects highest-priority undeployed; on turn 7 no mechs are out yet,
				// so index 3 (Wayfare, priority 10) would normally fire. We want Riverwalk (index 0)
				// first per the spec, so we override preferredDeployPosition and force mechNext=0.
				player.strategicAnalysis.mechNext = 0; // Riverwalk first
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[3, 7];
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiActions[player.SelectTopActionFlavor(player.gainMechActionPosition)], "Kickstart: Trade 2 metal, deploy Riverwalk at [3,7]")
				{
					tradeResource = new ResourceType[] { ResourceType.metal, ResourceType.metal }
				});
				return;
			case 8:
				// Move character to [3,6]; mech carrying 1 worker from [3,7] to [3,6]; 1 worker from [3,7] to [3,8]
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart: Move character->[3,6], mech+1worker [3,7]->[3,6], worker [3,7]->[3,8]")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						// Character to [3,6]
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 6], null, null);
						// Mech from [3,7] carrying exactly 1 worker to [3,6]
						var hex37 = this.gameManager.gameBoard.hexMap[3, 7];
						if (hex37.GetOwnerMechs().Count > 0)
						{
							Mech mech = hex37.GetOwnerMechs()[0];
							var passengers = new List<Unit>();
							var workers37 = hex37.GetOwnerWorkers();
							if (workers37.Count > 0)
							{
								passengers.Add(workers37[0]);
							}
							this.gameManager.moveManager.SelectUnit(mech);
							this.gameManager.moveManager.MoveSelectedUnit(
								this.gameManager.gameBoard.hexMap[3, 6],
								this.HexResources(mech.position),
								passengers);
						}
						// 1 remaining worker from [3,7] to [3,8]
						var hex37b = this.gameManager.gameBoard.hexMap[3, 7];
						if (hex37b.GetOwnerWorkers().Count > 0)
						{
							this.gameManager.moveManager.SelectUnit(hex37b.GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 8], null, null);
						}
					}
				});
				return;
			case 9:
				// Trade 2 metal, deploy Speed or Scout (next highest undeployed after Riverwalk).
				// At this point index 0 (Riverwalk) is deployed; mechNext will pick index 3 (Wayfare/Scout,
				// priority 10) since it is still the highest undeployed. That is the desired Speed or Scout.
				// We set preferredDeployPosition to [3,7] so the second mech lands on the production farm.
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[3, 7];
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiActions[player.SelectTopActionFlavor(player.gainMechActionPosition)], "Kickstart: Trade 2 metal, deploy Speed/Scout at [3,7]")
				{
					tradeResource = new ResourceType[] { ResourceType.metal, ResourceType.metal }
				});
				return;
			case 10:
				// Produce
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart: Produce"));
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D18 RID: 11544 RVA: 0x00104BC0 File Offset: 0x00102DC0
		private void CrimeaAgro(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[] { ResourceType.food, ResourceType.metal }
				});
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Kickstart"));
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D19 RID: 11545 RVA: 0x00104E0C File Offset: 0x0010300C
		private void RusvietIndustrial(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			int turnCount = this.gameManager.TurnCount;
			if (turnCount == 0)
			{
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			}
			if (turnCount != 1)
			{
				return;
			}
			actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
			{
				moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
				{
					GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
					this.gameManager.moveManager.SetMoveAction(gainMove);
					this.gameManager.moveManager.SelectUnit(player.player.character);
					this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 7], null, null);
				}
			});
		}

		// Token: 0x06002D1A RID: 11546 RVA: 0x00104EB8 File Offset: 0x001030B8
		private void RusvietPatriot(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Rusviet Patriotic Kickstart: Produce"));
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Rusviet Patriotic Kickstart: Trade 2 Oil")
				{
					tradeResource = new ResourceType[] { ResourceType.oil, ResourceType.oil }
				});
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Rusviet Patriotic Kickstart: Move 2 Workers [6,3]->[5,4], Upgrade")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						
						var hex63 = this.gameManager.gameBoard.hexMap[6, 3];
						var hex54 = this.gameManager.gameBoard.hexMap[5, 4];
						var workers = hex63.GetOwnerWorkers();
						if (workers.Count >= 2)
						{
							this.gameManager.moveManager.SelectUnit(workers[0]);
							this.gameManager.moveManager.MoveSelectedUnit(hex54, this.HexResources(hex63), null);
							this.gameManager.moveManager.SelectUnit(workers[1]);
							this.gameManager.moveManager.MoveSelectedUnit(hex54, null, null);
						}
						this.gameManager.moveManager.Clear();
					}
				});
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Rusviet Patriotic Kickstart: Produce"));
				return;
			case 5:
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[6, 3];
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Rusviet Patriotic Kickstart: Bolster / Deploy to [6,3]"));
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Rusviet Patriotic Kickstart: Move Hero to [6,2], Mech (3w) [6,3]->[6,2], Upgrade")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						
						// Character to [6,2] (Farm)
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[6, 2], null, null);
						
						// Mech carrying 3 workers from [6,3] to [6,2]
						var hex63 = this.gameManager.gameBoard.hexMap[6, 3];
						var hex62 = this.gameManager.gameBoard.hexMap[6, 2];
						if (hex63.GetOwnerMechs().Count > 0)
						{
							Mech mech = hex63.GetOwnerMechs()[0];
							var workers = hex63.GetOwnerWorkers();
							var passengers = new List<Unit>();
							for (int i = 0; i < Math.Min(workers.Count, 3); i++) passengers.Add(workers[i]);
							
							this.gameManager.moveManager.SelectUnit(mech);
							this.gameManager.moveManager.MoveSelectedUnit(hex62, this.HexResources(hex63), passengers);
						}
						this.gameManager.moveManager.Clear();
					}
				});
				return;
			case 7:
				{
					AiRecipe produceRecipe7 = new AiRecipe(player.AiTopActions[GainType.Produce], "Rusviet Patriotic Kickstart: Produce on [6,3], [6,2] and [5,4], enlist");
					produceRecipe7.moveAction = delegate(AiRecipe r, AiPlayer p)
					{
						GainProduce gainProduce = (GainProduce)p.AiTopActions[GainType.Produce].GetTopGainAction();
						gainProduce.Clear();
						GameHex hex63 = p.gameManager.gameBoard.hexMap[6, 3];
						GameHex hex62 = p.gameManager.gameBoard.hexMap[6, 2];
						GameHex hex54 = p.gameManager.gameBoard.hexMap[5, 4];
						// Produce in priority order: farm [6,2] (food), tundra [5,4] (oil), village [6,3] (workers)
						gainProduce.ExecuteOnce(hex62, hex62.GetOwnerWorkers().Count);
						gainProduce.ExecuteOnce(hex54, hex54.GetOwnerWorkers().Count);
						gainProduce.ExecuteOnce(hex63, hex63.GetOwnerWorkers().Count);
						gainProduce.SelectAction();
					};
					actionOptions.Add(int.MaxValue, produceRecipe7);
					return;
				}
			case 8:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Rusviet Patriotic Kickstart: Move 2 workers [6,3]->[6,4], Hero [6,2]->[6,3], Upgrade")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						
						var hex63 = this.gameManager.gameBoard.hexMap[6, 3];
						var hex64 = this.gameManager.gameBoard.hexMap[6, 4];
						var hex62 = this.gameManager.gameBoard.hexMap[6, 2];
						
						// Move 2 workers from [6,3] to [6,4]
						var workers63 = hex63.GetOwnerWorkers();
						if (workers63.Count >= 1)
						{
							this.gameManager.moveManager.SelectUnit(workers63[0]);
							this.gameManager.moveManager.MoveSelectedUnit(hex64, this.HexResources(hex63), null);
						}
						if (workers63.Count >= 2)
						{
							this.gameManager.moveManager.SelectUnit(workers63[1]);
							this.gameManager.moveManager.MoveSelectedUnit(hex64, null, null);
						}
						
						// Hero from [6,2] to [6,3]
						if (player.player.character.position == hex62)
						{
							this.gameManager.moveManager.SelectUnit(player.player.character);
							this.gameManager.moveManager.MoveSelectedUnit(hex63, null, null);
						}
						this.gameManager.moveManager.Clear();
					}
				});
				return;
			case 9:
				player.strategicAnalysis.preferredDeployPosition = null;
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Rusviet Patriotic Kickstart: Produce / Enlist"));
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D1B RID: 11547 RVA: 0x0010512C File Offset: 0x0010332C
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
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 7], null, null);
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D1C RID: 11548 RVA: 0x001052EC File Offset: 0x001034EC
		private void RusvietAgro(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Rusviet Agro Kickstart"));
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Rusviet Agro Kickstart"));
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Rusviet Agro Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[6, 3];
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Rusviet Agro Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.oil,
						ResourceType.oil
					}
				});
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Rusviet Agro Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[6, 2], null, null);
						if (this.gameManager.gameBoard.hexMap[6, 3].GetOwnerMechs().Count > 0)
						{
							Mech mech = this.gameManager.gameBoard.hexMap[6, 3].GetOwnerMechs()[0];
							List<Unit> list = new List<Unit>();
							int wcount = 0;
							foreach (Worker worker in this.gameManager.gameBoard.hexMap[6, 3].GetOwnerWorkers())
							{
								if (wcount < 4) { list.Add(worker); wcount++; }
							}
							this.gameManager.moveManager.SelectUnit(mech);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[6, 2], this.HexResources(mech.position), list);
						}
					}
				});
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Rusviet Agro Kickstart"));
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Rusviet Agro Kickstart"));
				return;
			case 7:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Rusviet Agro Kickstart"));
				return;
			case 8:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Rusviet Agro Kickstart"));
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[6, 4];
				return;
			case 9:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Rusviet Agro Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			default:
				break;
			}
		}

		// Token: 0x06002D1D RID: 11549 RVA: 0x001054D4 File Offset: 0x001036D4
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
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x0010561C File Offset: 0x0010381C
		private void SaxEngine(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
		actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Saxony Engineering Kickstart")
		{
			tradeResource = new ResourceType[2]
		});
		return;
	case 1:
		actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Saxony Engineering Kickstart"));
		return;
	case 2:
		player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[0, 6];
		actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Saxony Engineering Kickstart")
		{
			tradeResource = new ResourceType[2]
		});
		return;
	case 3:
		actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Saxony Engineering Kickstart"));
		return;
	case 4:
		player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[0, 6];
		actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Saxony Engineering Kickstart")
		{
			tradeResource = new ResourceType[]
			{
				ResourceType.metal,
				ResourceType.metal
			}
		});
		return;
	case 5:
		actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Saxony Engineering Kickstart")
		{
			moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
			{
				GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
				this.gameManager.moveManager.SetMoveAction(gainMove);
				this.gameManager.moveManager.SelectUnit(player.player.character);
				this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 6], null, null);
				this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[1, 7].GetOwnerWorkers()[0]);
				this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 6], null, null);
			}
		});
		return;
	case 6:
		actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Saxony Engineering Kickstart"));
		return;
	case 7:
		player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[1, 6];
		actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Saxony Engineering Kickstart")
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

		// Token: 0x06002D1F RID: 11551 RVA: 0x00105760 File Offset: 0x00103960
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
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D20 RID: 11552 RVA: 0x001059B8 File Offset: 0x00103BB8
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

		// Token: 0x06002D21 RID: 11553 RVA: 0x00105D88 File Offset: 0x00103F88
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

		// Token: 0x06002D22 RID: 11554 RVA: 0x00105F40 File Offset: 0x00104140
		private void PolaniaPatriot(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Polania Patriotic Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Polania Patriotic Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[1, 3].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 4], this.HexResources(this.gameManager.gameBoard.hexMap[1, 3]), null);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 3], null, null);
					}
				});
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Polania Patriotic Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Polania Patriotic Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove2 = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove2);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 4], null, null);
						List<Worker> ownerWorkers = this.gameManager.gameBoard.hexMap[0, 4].GetOwnerWorkers();
						if (ownerWorkers.Count > 0)
						{
							this.gameManager.moveManager.SelectUnit(ownerWorkers[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 4], this.HexResources(this.gameManager.gameBoard.hexMap[0, 4]), null);
						}
					}
				});
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Polania Patriotic Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Polania Patriotic Kickstart"));
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[1, 4];
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D23 RID: 11555 RVA: 0x001060F0 File Offset: 0x001042F0
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

		// Token: 0x06002D24 RID: 11556 RVA: 0x00106250 File Offset: 0x00104450
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
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D25 RID: 11557 RVA: 0x00106404 File Offset: 0x00104604
		private void PolaniaIndustrial(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Polania Industrial Kickstart")
				{
					tradeResource = new ResourceType[] { ResourceType.metal, ResourceType.metal }
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Polania Industrial Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 3], null, null);
						if (this.gameManager.gameBoard.hexMap[0, 3].GetOwnerWorkers().Count > 0)
						{
							this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[0, 3].GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 3], this.HexResources(this.gameManager.gameBoard.hexMap[0, 3]), null);
						}
					}
				});
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Polania Industrial Kickstart")
				{
					tradeResource = new ResourceType[] { ResourceType.metal, ResourceType.metal }
				});
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Polania Industrial Kickstart"));
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Polania Industrial Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove2 = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove2);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 4], null, null);
					}
				});
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Polania Industrial Kickstart")
				{
					tradeResource = new ResourceType[] { ResourceType.metal, ResourceType.metal }
				});
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Polania Industrial Kickstart"));
				return;
			case 7:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Polania Industrial Kickstart")
				{
					tradeResource = new ResourceType[] { ResourceType.metal, ResourceType.metal }
				});
				return;
			case 8:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Polania Industrial Kickstart"));
				return;
			case 9:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Polania Industrial Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove3 = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove3);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 4], null, null);
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D26 RID: 11558 RVA: 0x001065F8 File Offset: 0x001047F8
		private void PolaniaMech(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
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
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Polonia Mech Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 3], null, null);
						Worker worker = this.gameManager.gameBoard.hexMap[1, 3].GetOwnerWorkers()[0];
						this.gameManager.moveManager.SelectUnit(worker);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 4], this.HexResources(this.gameManager.gameBoard.hexMap[1, 3]), null);
					}
				});
				return;
			case 2:
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
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Polonia Mech Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove2 = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove2);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 4], null, null);
						Worker worker2 = this.gameManager.gameBoard.hexMap[0, 4].GetOwnerWorkers()[0];
						this.gameManager.moveManager.SelectUnit(worker2);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 4], this.HexResources(this.gameManager.gameBoard.hexMap[0, 4]), null);
					}
				});
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Polonia Mech Kickstart"));
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Polonia Mech Kickstart"));
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Polonia Mech Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove3 = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove3);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[2, 3], null, null);
						Mech mech = this.gameManager.gameBoard.hexMap[1, 4].GetOwnerMechs()[0];
						List<Worker> ownerWorkers = this.gameManager.gameBoard.hexMap[1, 4].GetOwnerWorkers();
						this.gameManager.moveManager.SelectUnit(mech);
						foreach (Worker worker3 in ownerWorkers)
						{
							this.gameManager.moveManager.SelectUnit(worker3);
						}
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[2, 3], this.HexResources(this.gameManager.gameBoard.hexMap[1, 4]), null);
					}
				});
				return;
			case 7:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Polonia Mech Kickstart"));
				return;
			case 8:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Polonia Mech Kickstart"));
				return;
			case 9:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Polonia Mech Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove4 = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove4);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 4], null, null);
						Mech mech2 = this.gameManager.gameBoard.hexMap[2, 3].GetOwnerMechs()[0];
						this.gameManager.moveManager.SelectUnit(mech2);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 4], null, null);
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D27 RID: 11559 RVA: 0x00106850 File Offset: 0x00104A50
		private void NordInnovative(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Nordic Innovative Kickstart: Trade 2 Oil")
				{
					tradeResource = new ResourceType[] { ResourceType.oil, ResourceType.oil }
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Nordic Innovative Kickstart: Move Worker [5,1]->[6,1], [4,1]->[4,2]")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						// Worker from [5,1] to [6,1]
						var hex51 = this.gameManager.gameBoard.hexMap[5, 1];
						if (hex51.GetOwnerWorkers().Count > 0)
						{
							this.gameManager.moveManager.SelectUnit(hex51.GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[6, 1], this.HexResources(hex51), null);
						}
						// Worker from [4,1] to [4,2]
						var hex41 = this.gameManager.gameBoard.hexMap[4, 1];
						if (hex41.GetOwnerWorkers().Count > 0)
						{
							this.gameManager.moveManager.SelectUnit(hex41.GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 2], this.HexResources(hex41), null);
						}
					}
				});
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Nordic Innovative Kickstart: Trade 1 Oil, 1 Metal / Upgrade")
				{
					tradeResource = new ResourceType[] { ResourceType.oil, ResourceType.metal }
				});
				return;
			case 3:
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[6, 1];
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Nordic Innovative Kickstart: Produce / Deploy on [6,1]"));
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Nordic Innovative Kickstart: Move Hero->[4,2], Mech+AllWorkers [6,1]->[5,2]")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						// Hero to [4,2]
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 2], null, null);
						// Mech carrying all workers from [6,1] to [5,2]
						var hex61 = this.gameManager.gameBoard.hexMap[6, 1];
						if (hex61.GetOwnerMechs().Count > 0)
						{
							Mech mech = hex61.GetOwnerMechs()[0];
							var workers = hex61.GetOwnerWorkers();
							this.gameManager.moveManager.SelectUnit(mech);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 2], this.HexResources(hex61), new List<Unit>(workers));
						}
					}
				});
				return;
			case 5:
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[4, 2];
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Nordic Innovative Kickstart: Produce / Deploy on [4,2]"));
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Nordic Innovative Kickstart: Move Hero->[3,4], Mech [4,2]->[3,4]")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						// Hero to [3,4]
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 4], null, null);
						// Mech from [4,2] to [3,4]
						var hex42 = this.gameManager.gameBoard.hexMap[4, 2];
						if (hex42.GetOwnerMechs().Count > 0)
						{
							this.gameManager.moveManager.SelectUnit(hex42.GetOwnerMechs()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 4], null, null);
						}
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D28 RID: 11560 RVA: 0x001068B8 File Offset: 0x00104AB8
		private void NordEngine(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
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
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.oil,
						ResourceType.metal
					}
				});
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Kickstart"));
				player.strategicAnalysis.preferredBuildPosition = this.gameManager.gameBoard.hexMap[5, 1];
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
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[4, 1];
				return;
			case 7:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 2], null, null);
						if (this.gameManager.gameBoard.hexMap[4, 1].GetOwnerMechs().Count > 0)
						{
							this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[4, 1].GetOwnerMechs()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 2], null, null);
						}
						if (this.gameManager.gameBoard.hexMap[4, 1].GetOwnerWorkers().Count > 0)
						{
							this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[4, 1].GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 1], null, null);
						}
					}
				});
				return;
			case 8:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 9:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[3, 1];
				return;
			default:
				return;
			}
		}


		// Token: 0x06002D29 RID: 11561 RVA: 0x00106ABC File Offset: 0x00104CBC
		private void NordIndustrial(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Nordic Industrial Kickstart")
				{
					tradeResource = new ResourceType[] { ResourceType.metal, ResourceType.metal }
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Nordic Industrial Kickstart"));
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Nordic Industrial Kickstart"));
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Nordic Industrial Kickstart"));
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Nordic Industrial Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[6, 1].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 1], this.HexResources(this.gameManager.gameBoard.hexMap[6, 1]), null);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 2], null, null);
					}
				});
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Nordic Industrial Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 4], null, null);
					}
				});
				return;
			default:
				return;
			}
		}

		private void NordMech(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Nordic Mechanical Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Nordic Mechanical Kickstart"));
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Nordic Mechanical Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Nordic Mechanical Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 1], null, null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[4, 1].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 1], this.HexResources(this.gameManager.gameBoard.hexMap[4, 1]), null);
					}
				});
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Nordic Mechanical Kickstart"));
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Nordic Mechanical Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove2 = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove2);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 2], null, null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[5, 1].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 2], this.HexResources(this.gameManager.gameBoard.hexMap[5, 1]), null);
					}
				});
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Nordic Mechanical Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 7:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Nordic Mechanical Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove3 = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove3);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 3], null, null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[5, 2].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[6, 2], this.HexResources(this.gameManager.gameBoard.hexMap[5, 2]), null);
					}
				});
				return;
			case 8:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Nordic Mechanical Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.oil,
						ResourceType.oil
					}
				});
				return;
			case 9:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Nordic Mechanical Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove4 = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove4);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 4], null, null);
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D2B RID: 11563 RVA: 0x00106E58 File Offset: 0x00105058
		private void NordAgro(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				// Trade 2 oil
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
				// Move worker from [5,1] to [6,1], move worker from [4,1] to [4,2]
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[5, 1].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[6, 1], this.HexResources(this.gameManager.gameBoard.hexMap[5, 1]), null);
						this.gameManager.moveManager.SelectUnit(this.gameManager.gameBoard.hexMap[4, 1].GetOwnerWorkers()[0]);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 2], this.HexResources(this.gameManager.gameBoard.hexMap[4, 1]), null);
					}
				});
				return;
			case 2:
				// Produce
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 3:
				// Trade 2 metal, deploy to [6,1]
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[6, 1];
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
				// Produce
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 5:
				// Move character to [4,2], move 2 workers from [6,1] to [5,1]
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 2], null, null);
						var workers61 = this.gameManager.gameBoard.hexMap[6, 1].GetOwnerWorkers();
						int moved = 0;
						var toMove = new List<Unit>();
						for (int i = 0; i < workers61.Count && moved < 2; i++)
						{
							toMove.Add(workers61[i]);
							moved++;
						}
						foreach (var w in toMove)
						{
							this.gameManager.moveManager.SelectUnit(w);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 1], null, null);
						}
					}
				});
				return;
			case 6:
				// Produce on [5,1] and [6,1]
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 7:
				// Trade 2 metal, deploy to [4,2]
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[4, 2];
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 8:
				// Move character to [3,4], mech carrying 5 workers from [6,1] to [5,2]
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 4], null, null);
						var hex61 = this.gameManager.gameBoard.hexMap[6, 1];
						if (hex61.GetOwnerMechs().Count > 0)
						{
							Mech mech = hex61.GetOwnerMechs()[0];
							var workers = hex61.GetOwnerWorkers();
							var passengers = new List<Unit>();
							for (int i = 0; i < Math.Min(workers.Count, 5); i++)
							{
								passengers.Add(workers[i]);
							}
							this.gameManager.moveManager.SelectUnit(mech);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 2], this.HexResources(hex61), passengers);
							this.gameManager.moveManager.UnloadAllWorkersFromMech(mech);
						}
					}
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D2C RID: 11564 RVA: 0x00107008 File Offset: 0x00105208
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

		// Token: 0x06002D2D RID: 11565 RVA: 0x000FFAA4 File Offset: 0x000FDCA4
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

		// Token: 0x06002D2E RID: 11566 RVA: 0x001071A8 File Offset: 0x001053A8
		private void SaxInnovative(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Saxony Innovative Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.metal,
						ResourceType.metal
					}
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Saxony Innovative Kickstart"));
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[1, 7];
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Saxony Innovative Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						Mech mech = player.player.matFaction.mechs.Find((Mech m) => m.Id == 3);
						if (mech != null)
						{
							this.gameManager.moveManager.SelectUnit(mech);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[0, 6], this.HexResources(this.gameManager.gameBoard.hexMap[1, 7]), null);
						}
						List<Worker> ownerWorkers = this.gameManager.gameBoard.hexMap[1, 7].GetOwnerWorkers();
						if (ownerWorkers.Count > 0)
						{
							this.gameManager.moveManager.SelectUnit(ownerWorkers[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[0, 6], null, null);
						}
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[1, 6], null, null);
					}
				});
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Saxony Innovative Kickstart")
				{
					tradeResource = new ResourceType[2]
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D2F RID: 11567 RVA: 0x001072E4 File Offset: 0x001054E4
		private void CrimeaMilitant(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[] { ResourceType.oil, ResourceType.oil }
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.CombatCard], "Kickstart"));
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[] { ResourceType.food, ResourceType.food }
				});
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[] { ResourceType.oil, ResourceType.oil }
				});
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Kickstart"));
				return;
			case 7:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[] { ResourceType.metal, ResourceType.metal }
				});
				return;
			case 8:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart"));
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D30 RID: 11568 RVA: 0x0004459F File Offset: 0x0004279F
		private void PolaniaMilitant(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Polania Militant Kickstart: Trade 2 Metal")
				{
					tradeResource = new ResourceType[] { ResourceType.metal, ResourceType.metal }
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Polania Militant Kickstart: Move Worker [0,4]->[1,4], Hero -> [0,4]")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						var hex04 = this.gameManager.gameBoard.hexMap[0, 4];
						var hex14 = this.gameManager.gameBoard.hexMap[1, 4];
						if (hex04.GetOwnerWorkers().Count > 0)
						{
							this.gameManager.moveManager.SelectUnit(hex04.GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(hex14, this.HexResources(hex04), null);
						}
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(hex04, null, null);
					}
				});
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Polania Militant Kickstart: Produce"));
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Polania Militant Kickstart: Trade Metal, Wood")
				{
					tradeResource = new ResourceType[] { ResourceType.metal, ResourceType.wood }
				});
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Polania Militant Kickstart: Move Worker [1,4]->[1,3], Hero -> [1,4], Deploy")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						var hex14 = this.gameManager.gameBoard.hexMap[1, 4];
						var hex13 = this.gameManager.gameBoard.hexMap[1, 3];
						if (hex14.GetOwnerWorkers().Count > 0)
						{
							this.gameManager.moveManager.SelectUnit(hex14.GetOwnerWorkers()[0]);
							this.gameManager.moveManager.MoveSelectedUnit(hex13, this.HexResources(hex14), null);
						}
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(hex14, null, null);
					}
				});
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[1, 4];
				return;
			case 5:
				player.strategicAnalysis.preferredBuildPosition = this.gameManager.gameBoard.hexMap[1, 4];
				player.strategicAnalysis.buildingNext = BuildingType.Mine;
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Polania Militant Kickstart: Build Mine at [1,4]"));
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D31 RID: 11569 RVA: 0x0004459F File Offset: 0x0004279F
		private void PolaniaInnovative(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			this.PolaniaIndustrial(actionOptions, player);
		}

		// Token: 0x06002D32 RID: 11570 RVA: 0x000445A9 File Offset: 0x000427A9
		private void NordMilitant(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			this.NordIndustrial(actionOptions, player);
		}

		private void RusvietEngine(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Rusviet Engineering: Produce 1 worker, 1 metal"));
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Rusviet Engineering: Produce 2 workers, 1 metal"));
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiActions[player.SelectTopActionFlavor(player.gainMechActionPosition)], "Rusviet Engineering: Trade 2 metal, Deploy at 6,3")
				{
					tradeResource = new ResourceType[] { ResourceType.metal, ResourceType.metal }
				});
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Rusviet Engineering: Move Mech (6,3->6,4->5,4), Hero -> Encounter")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						
						// Hero character to tundra 6,4 encounter (following user text, but hex map 5,4 is the encounter)
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 4], null, null);
						
						// Mech from 6,3 carrying 4 workers to mountain 6,4, leave 1, then continue to 5,4 with 3
						if (player.player.matFaction.mechs.Count > 0)
						{
							Mech mech = player.player.matFaction.mechs[0];
							List<Unit> workers = new List<Unit>();
							int wcount = 0;
							foreach (Worker w in this.gameManager.gameBoard.hexMap[6, 3].GetOwnerWorkers())
							{
								if (wcount < 4) { workers.Add(w); wcount++; }
							}
							
							// Move 1: 6,3 -> 6,4
							this.gameManager.moveManager.SelectUnit(mech);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[6, 4], null, workers);
							
							// Drop 1 worker at 6,4
							if (mech.LoadedWorkers.Count > 0)
							{
								this.gameManager.moveManager.UnloadWorkerFromSelectedMech(mech.LoadedWorkers[0]);
							}
							
							// Move 2: 6,4 -> 5,4 (with remaining 3 workers)
							this.gameManager.moveManager.SelectUnit(mech);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 4], null, new List<Unit>(mech.LoadedWorkers));
						}
						
					}
				});
				return;
			case 4:
			case 5:
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiActions[player.SelectTopActionFlavor(player.gainUpgradeActionPosition)], "Rusviet Engineering: Produce -> Upgrade"));
				return;
			case 7:
			case 8:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiActions[player.SelectTopActionFlavor(player.gainMechActionPosition)], "Rusviet Engineering: Trade 2 food, Deploy")
				{
					tradeResource = new ResourceType[] { ResourceType.food, ResourceType.food }
				});
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D34 RID: 11572 RVA: 0x000445B3 File Offset: 0x000427B3
		private void RusvietMilitant(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			this.RusvietAgro(actionOptions, player);
		}

		// Token: 0x06002D35 RID: 11573 RVA: 0x000445B3 File Offset: 0x000427B3
		private void RusvietInnovative(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Rusviet Innovative: Produce 1 worker, 1 metal")
				{
					// Produce at 6,4 (metal) and 6,3 (workers)
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Rusviet Innovative: Produce 2 workers, 1 metal"));
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiActions[player.SelectTopActionFlavor(player.gainMechActionPosition)], "Rusviet Innovative: Produce -> Deploy Riverwalk at (6,3)")
				{
					// Produce 3 workers, 1 metal. Deploy Riverwalk.
				});
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[6, 3];
				return;
			case 3:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Rusviet Innovative: Move Character to (5,4), Mech+Workers to (5,4) -> Enlist")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						// Hero to (5,4) tundra encounter
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 4], null, null);
						
						// Mech from (6,3) carrying workers to (5,4)
						if (player.player.matFaction.mechs.Count > 0)
						{
							Mech mech = player.player.matFaction.mechs[0];
							List<Unit> workers = new List<Unit>();
							foreach (Worker w in this.gameManager.gameBoard.hexMap[6, 3].GetOwnerWorkers())
							{
								workers.Add(w);
							}
							this.gameManager.moveManager.SelectUnit(mech);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 4], null, workers);
						}
					}
				});
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiActions[player.SelectTopActionFlavor(player.gainMechActionPosition)], "Rusviet Innovative: Produce -> Deploy Township at (6,4)")
				{
					// Produce metal at (6,4), oil at (5,4). Deploy Township.
				});
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[6, 4];
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiActions[player.SelectTopActionFlavor(player.gainUpgradeActionPosition)], "Rusviet Innovative: Trade for 2 food -> Upgrade (Bolster/Recruit)")
				{
					tradeResource = new ResourceType[] { ResourceType.food, ResourceType.food }
				});
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Rusviet Innovative: Hero to (5,6) Combat, Mech to (5,5) -> Enlist")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						// Hero to (5,6) mountain encounter/combat
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 6], null, null);
						
						// Mech from (5,4) to (5,5) village tunnel
						if (player.player.matFaction.mechs.Count > 0)
						{
							Mech mech = player.player.matFaction.mechs[0];
							this.gameManager.moveManager.SelectUnit(mech);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 5], null, null);
						}
					}
				});
				return;
			default:
				this.RusvietAgro(actionOptions, player);
				return;
			}
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x000445BD File Offset: 0x000427BD
		private void CrimeaPatriot(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0: // Trade 2 food
				player.SafeAdd(actionOptions, 1000000, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Refined Crimea Patriot Sequence: Trade Food")
				{
					tradeResource = new ResourceType[] { ResourceType.food, ResourceType.food }
				});
				return;
			case 1: // Produce, Enlist
				player.SafeAdd(actionOptions, 1000000, new AiRecipe(player.AiTopActions[GainType.Produce], "Refined Crimea Patriot Sequence: Produce/Enlist"));
				return;
			case 2: // Trade 1 food, 1 oil
				player.SafeAdd(actionOptions, 1000000, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Refined Crimea Patriot Sequence: Trade Food/Oil")
				{
					tradeResource = new ResourceType[] { ResourceType.food, ResourceType.oil }
				});
				return;
			case 3: // Produce, Enlist
				player.SafeAdd(actionOptions, 1000000, new AiRecipe(player.AiTopActions[GainType.Produce], "Refined Crimea Patriot Sequence: Produce/Enlist"));
				return;
			case 4: // Move hero to (3,7), move 1 worker to (3,7) and upgrade using oil + coercion
				player.SafeAdd(actionOptions, 1000000, new AiRecipe(player.AiTopActions[GainType.Move], "Refined Crimea Patriot Sequence: Move/Upgrade")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						// Hero to farm 3,7
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 7], null, null);
						// Worker from village 3,8 to farm 3,7
						var workers = this.gameManager.gameBoard.hexMap[3, 8].GetOwnerWorkers();
						if (workers.Count > 0)
						{
							this.gameManager.moveManager.SelectUnit(workers[0]);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 7], null, null);
						}
					}
				});
				return;
			case 5: // Produce, Enlist
				player.SafeAdd(actionOptions, 1000000, new AiRecipe(player.AiTopActions[GainType.Produce], "Refined Crimea Patriot Sequence: Produce/Enlist"));
				return;
			case 6: // Trade 2 metal
				player.SafeAdd(actionOptions, 1000000, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Refined Crimea Patriot Sequence: Trade Metal")
				{
					tradeResource = new ResourceType[] { ResourceType.metal, ResourceType.metal }
				});
				return;
			case 7: // Bolster 2 combat cards, deploy speed mech on (3,8)
				player.SafeAdd(actionOptions, 1000000, new AiRecipe(player.AiTopActions[GainType.CombatCard], "Refined Crimea Patriot Sequence: Bolster CC/Deploy"));
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[3, 8];
				return;
			case 8: // Move mech + 6 workers from (3,8) to (4,7), move hero to (4,7)
				player.SafeAdd(actionOptions, 1000000, new AiRecipe(player.AiTopActions[GainType.Move], "Refined Crimea Patriot Sequence: Move Mech/Hero -> Upgrade")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						// Hero from 3,7 to mountain 4,7
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 7], null, null);
						
						// Mech from 3,8 to 4,7 with all workers
						if (player.player.matFaction.mechs.Count > 0)
						{
							var mech = player.player.matFaction.mechs[0];
							var workers = this.gameManager.gameBoard.hexMap[3, 8].GetOwnerWorkers();
							this.gameManager.moveManager.SelectUnit(mech);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 7], null, new List<Unit>(workers));
						}
						
					}
				});
				return;
			case 9: // Produce/Enlist
				player.SafeAdd(actionOptions, 1000000, new AiRecipe(player.AiTopActions[GainType.Produce], "Refined Crimea Patriot Sequence: Produce/Enlist"));
				return;
			case 10: // Bolster 2 combat cards/deploy riverwalk on farm (Sec 1 is Bolster/Deploy)
				player.SafeAdd(actionOptions, 1000000, new AiRecipe(player.AiTopActions[GainType.CombatCard], "Refined Crimea Patriot Sequence: Bolster CC/Deploy Riverwalk"));
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[3, 7];
				return;
			default:
				return;
			}
		}



		// Token: 0x06002D37 RID: 11575 RVA: 0x000445BD File Offset: 0x000427BD
		private void CrimeaMech(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
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
						ResourceType.food,
						ResourceType.food
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
						ResourceType.oil,
						ResourceType.oil
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
						var hexFrom = this.gameManager.gameBoard.hexMap[3, 8];
						var hexTo = this.gameManager.gameBoard.hexMap[3, 7];
						var workers = hexFrom.GetOwnerWorkers();
						var list = new List<Unit>();
						for (int i = 0; i < Math.Min(workers.Count, 3); i++)
						{
							list.Add(workers[i]);
						}
						foreach (var unit in list)
						{
							this.gameManager.moveManager.SelectUnit(unit);
							this.gameManager.moveManager.MoveSelectedUnit(hexTo, null, null);
						}
					}
				});
				return;
			case 5:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[]
					{
						ResourceType.oil,
						ResourceType.oil
					}
				});
				return;
			case 6:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Kickstart"));
				return;
			case 7:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 8:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Power], "Kickstart"));
				return;
			case 9:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D38 RID: 11576 RVA: 0x000445BD File Offset: 0x000427BD
		private void CrimeaInnovative(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			switch (this.gameManager.TurnCount)
			{
			case 0:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[] { ResourceType.metal, ResourceType.metal }
				});
				return;
			case 1:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 2:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[] { ResourceType.metal, ResourceType.food }
				});
				return;
			case 3:
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[3, 8];
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 4:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						// Hero to 4,7
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 7], null, null);
						// Mech + 4 workers from 3,8 to 3,7, drop 2, continue with 2 to 4,7
						if (this.gameManager.gameBoard.hexMap[3, 8].GetOwnerMechs().Count > 0)
						{
							var mech = this.gameManager.gameBoard.hexMap[3, 8].GetOwnerMechs()[0];
							var workers = this.gameManager.gameBoard.hexMap[3, 8].GetOwnerWorkers();
							var passengers = new System.Collections.Generic.List<Unit>();
							int wcnt = 0;
							foreach (var w in workers) { if (wcnt < 4) { passengers.Add(w); wcnt++; } }

							this.gameManager.moveManager.SelectUnit(mech);
							// First leg: to 3,7 with 4 workers
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 7], null, passengers);
							// Second leg: continue to 4,7 with only 2 workers
							var subset = new System.Collections.Generic.List<Unit>();
							if (passengers.Count > 2) { subset.Add(passengers[0]); subset.Add(passengers[1]); }
							else { subset = passengers; }
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[4, 7], null, subset);
						}
					}
				});
				return;
			case 5:
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[4, 7];
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 6:
				// Skip/Standard turn - skipping in switch will use default AI logic, but user might want a specific action.
				// Since it's missing, let's just bolster for power or trade.
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.AnyResource], "Kickstart")
				{
					tradeResource = new ResourceType[] { ResourceType.food, ResourceType.food }
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
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[2, 6], null, null);
						
						Mech mech = null;
						foreach (var m in player.player.matFaction.mechs) { if (m.position != null && m.position.posX == 4 && m.position.posY == 7) { mech = m; break; } }
						if (mech != null)
						{
							this.gameManager.moveManager.SelectUnit(mech);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[2, 6], null, null);
						}
					}
				});
				return;
			case 8:
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[4, 7];
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			case 9:
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Move], "Kickstart")
				{
					moveAction = delegate(AiRecipe recipe, AiPlayer aiPlayer)
					{
						GainMove gainMove = (GainMove)player.AiTopActions[GainType.Move].GetTopGainAction();
						this.gameManager.moveManager.SetMoveAction(gainMove);
						this.gameManager.moveManager.SelectUnit(player.player.character);
						this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[3, 4], null, null);

						Mech mech = null;
						foreach (var m in player.player.matFaction.mechs) { if (m.position != null && m.position.posX == 4 && m.position.posY == 7) { mech = m; break; } }
						if (mech != null)
						{
							var workers = new System.Collections.Generic.List<Unit>();
							int wcnt = 0;
							foreach (var w in mech.position.GetOwnerWorkers()) { if (wcnt < 3) { workers.Add(w); wcnt++; } }
							this.gameManager.moveManager.SelectUnit(mech);
							this.gameManager.moveManager.MoveSelectedUnit(this.gameManager.gameBoard.hexMap[5, 5], this.HexResources(mech.position), workers);
						}
					}
				});
				return;
			case 10:
				player.strategicAnalysis.preferredDeployPosition = this.gameManager.gameBoard.hexMap[5, 5];
				actionOptions.Add(int.MaxValue, new AiRecipe(player.AiTopActions[GainType.Produce], "Kickstart"));
				return;
			default:
				return;
			}
		}

		// Token: 0x06002D39 RID: 11577 RVA: 0x000445C7 File Offset: 0x000427C7
		private void SaxMilitant(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			this.SaxAgro(actionOptions, player);
		}

		// Token: 0x06002D3A RID: 11578 RVA: 0x000445D1 File Offset: 0x000427D1
		private void AlbionEngine(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			this.AlbionIndustrial(actionOptions, player);
		}

		// Token: 0x06002D3B RID: 11579 RVA: 0x000445D1 File Offset: 0x000427D1
		private void AlbionAgro(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			this.AlbionIndustrial(actionOptions, player);
		}

		// Token: 0x06002D3C RID: 11580 RVA: 0x000445DB File Offset: 0x000427DB
		private void TogawaPatriot(SortedList<int, AiRecipe> actionOptions, AiPlayer player)
		{
			this.TogawaIndustrial(actionOptions, player);
		}

		// Token: 0x04001EAF RID: 7855
		private GameManager gameManager;
	}
}
