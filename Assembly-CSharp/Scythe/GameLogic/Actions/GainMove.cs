using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x02000614 RID: 1556
	public class GainMove : GainAction
	{
		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06003113 RID: 12563 RVA: 0x00046CAA File Offset: 0x00044EAA
		// (set) Token: 0x06003114 RID: 12564 RVA: 0x00046CB2 File Offset: 0x00044EB2
		public List<GainMove.MoveActionStep> PlayerActionSteps { get; private set; }

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06003115 RID: 12565 RVA: 0x00046CBB File Offset: 0x00044EBB
		// (set) Token: 0x06003116 RID: 12566 RVA: 0x00046CC3 File Offset: 0x00044EC3
		public HashSet<Unit> UsedUnits { get; private set; }

		// Token: 0x06003117 RID: 12567 RVA: 0x00046CCC File Offset: 0x00044ECC
		public GainMove()
			: base(0, 0, false, false, false)
		{
			this.gainType = GainType.Move;
			this.PlayerActionSteps = new List<GainMove.MoveActionStep>();
			this.UsedUnits = new HashSet<Unit>();
			this.MovesLeft = 0;
		}

		// Token: 0x06003118 RID: 12568 RVA: 0x00046CFD File Offset: 0x00044EFD
		public GainMove(GameManager gameManager)
			: base(gameManager, 0, 0, false, false, false)
		{
			this.gainType = GainType.Move;
			this.PlayerActionSteps = new List<GainMove.MoveActionStep>();
			this.UsedUnits = new HashSet<Unit>();
			this.MovesLeft = 0;
		}

		// Token: 0x06003119 RID: 12569 RVA: 0x00046D2F File Offset: 0x00044F2F
		public GainMove(GameManager gameManager, short amount, short maxLevelUpgrade = 0)
			: base(gameManager, amount, maxLevelUpgrade, false, false, false)
		{
			this.gainType = GainType.Move;
			this.PlayerActionSteps = new List<GainMove.MoveActionStep>();
			this.UsedUnits = new HashSet<Unit>();
			this.MovesLeft = amount;
		}

		// Token: 0x0600311A RID: 12570 RVA: 0x00046D61 File Offset: 0x00044F61
		public override void Upgrade()
		{
			base.Upgrade();
			this.MovesLeft = base.Amount;
		}

		// Token: 0x0600311B RID: 12571 RVA: 0x000283F8 File Offset: 0x000265F8
		public override bool CanExecute()
		{
			return true;
		}

		// Token: 0x0600311C RID: 12572 RVA: 0x00046D75 File Offset: 0x00044F75
		public override LogInfo GetLogInfo()
		{
			return new HexUnitResourceLogInfo(this.gameManager);
		}

		// Token: 0x0600311D RID: 12573 RVA: 0x00129340 File Offset: 0x00127540
		public LogInfo GetLogInfoForUnitMove(Unit unit)
		{
			HexUnitResourceLogInfo hexUnitResourceLogInfo = new HexUnitResourceLogInfo(this.gameManager);
			bool flag = false;
			hexUnitResourceLogInfo.Type = LogInfoType.Move;
			hexUnitResourceLogInfo.PlayerAssigned = this.player.matFaction.faction;
			if (unit.Owner.currentMatSection != 4)
			{
				hexUnitResourceLogInfo.ActionPlacement = ActionPositionType.Top;
			}
			else
			{
				hexUnitResourceLogInfo.ActionPlacement = ActionPositionType.Down;
			}
			hexUnitResourceLogInfo.Units.Add(unit);
			for (int i = this.PlayerActionSteps.Count - 1; i >= 0; i--)
			{
				if (this.PlayerActionSteps[i].containerUnit == unit)
				{
					if (!flag && this.PlayerActionSteps[i] is GainMove.MoveUnit)
					{
						hexUnitResourceLogInfo.Hexes.Add((this.PlayerActionSteps[i] as GainMove.MoveUnit).lastPosition);
						hexUnitResourceLogInfo.Hexes.Add((this.PlayerActionSteps[i] as GainMove.MoveUnit).movePosition);
						flag = true;
					}
					else if (this.PlayerActionSteps[i] is GainMove.MoveUnit)
					{
						if ((this.PlayerActionSteps[i] as GainMove.MoveUnit).lastPosition != hexUnitResourceLogInfo.Hexes[0])
						{
							hexUnitResourceLogInfo.Hexes[0] = (this.PlayerActionSteps[i] as GainMove.MoveUnit).lastPosition;
						}
					}
					else if (this.PlayerActionSteps[i] is GainMove.LoadResources)
					{
						hexUnitResourceLogInfo.Resources.Add((this.PlayerActionSteps[i] as GainMove.LoadResources).GetResources());
					}
					else if (this.PlayerActionSteps[i] is GainMove.LoadWorker)
					{
						if (!hexUnitResourceLogInfo.Units.Contains((this.PlayerActionSteps[i] as GainMove.LoadWorker).worker))
						{
							hexUnitResourceLogInfo.Units.Add((this.PlayerActionSteps[i] as GainMove.LoadWorker).worker);
						}
					}
					else if (this.PlayerActionSteps[i] is GainMove.WithdrawUnits)
					{
						hexUnitResourceLogInfo.Units = (this.PlayerActionSteps[i] as GainMove.WithdrawUnits).GetUnitsToWithdraw();
						hexUnitResourceLogInfo.Hexes.Add((this.PlayerActionSteps[i] as GainMove.WithdrawUnits).GetWithdrawPosition());
					}
				}
			}
			return hexUnitResourceLogInfo;
		}

		// Token: 0x0600311E RID: 12574 RVA: 0x0012957C File Offset: 0x0012777C
		public override void Execute()
		{
			if (this.UsedUnits.Count > 0)
			{
				foreach (Unit unit in this.UsedUnits)
				{
					this.UnloadUnit(unit);
				}
			}
			this.player.character.MovesLeft = this.player.character.MaxMoveCount;
			foreach (Mech mech in this.player.matFaction.mechs)
			{
				mech.MovesLeft = mech.MaxMoveCount;
			}
			foreach (Worker worker in this.player.matPlayer.workers)
			{
				worker.MovesLeft = worker.MaxMoveCount;
			}
			if (this.gameManager.IsMultiplayer && ((this.gameManager.IsMyTurn() && this.player == this.gameManager.PlayerOwner) || (this.gameManager.PlayerOwner == null && !this.player.IsHuman)))
			{
				if (this.PlayerActionSteps.All((GainMove.MoveActionStep step) => step.stepType != GainMove.StepType.WithdrawUnit))
				{
					this.gameManager.OnActionSent(new GainMoveMessage());
				}
			}
		}

		// Token: 0x0600311F RID: 12575 RVA: 0x00129720 File Offset: 0x00127920
		public override void Clear()
		{
			this.PlayerActionSteps.Clear();
			foreach (Unit unit in this.UsedUnits)
			{
				unit.lastX = -1;
				unit.lastY = -1;
			}
			this.UsedUnits.Clear();
			if (this.player != null)
			{
				this.player.matFaction.SetRiverwalkUsed(false);
			}
			this.MovesLeft = base.Amount;
			base.ActionSelected = false;
		}

		// Token: 0x06003120 RID: 12576 RVA: 0x00046D82 File Offset: 0x00044F82
		public void SelectAction()
		{
			base.ActionSelected = true;
		}

		// Token: 0x06003121 RID: 12577 RVA: 0x001297BC File Offset: 0x001279BC
		protected void UnloadUnit(Unit unit)
		{
			this.UnloadAllResources(unit);
			if (unit is Mech)
			{
				Mech mech = unit as Mech;
				while (mech.LoadedWorkers.Count != 0)
				{
					this.UnloadWorkerFromMech(mech.LoadedWorkers[0], mech);
				}
			}
		}

		// Token: 0x06003122 RID: 12578 RVA: 0x00129804 File Offset: 0x00127A04
		protected void AddResourcesToField(Unit unit, GameHex position)
		{
			if (unit == null || position == null)
			{
				return;
			}
			Dictionary<ResourceType, int> dictionary = position.resources;
			dictionary[ResourceType.food] = dictionary[ResourceType.food] + unit.resources[ResourceType.food];
			dictionary = position.resources;
			dictionary[ResourceType.oil] = dictionary[ResourceType.oil] + unit.resources[ResourceType.oil];
			dictionary = position.resources;
			dictionary[ResourceType.wood] = dictionary[ResourceType.wood] + unit.resources[ResourceType.wood];
			dictionary = position.resources;
			dictionary[ResourceType.metal] = dictionary[ResourceType.metal] + unit.resources[ResourceType.metal];
			unit.resources[ResourceType.food] = 0;
			unit.resources[ResourceType.oil] = 0;
			unit.resources[ResourceType.wood] = 0;
			unit.resources[ResourceType.metal] = 0;
		}

		// Token: 0x06003123 RID: 12579 RVA: 0x00046D8B File Offset: 0x00044F8B
		private void SaveUnitAndStep(Unit unit, GainMove.MoveActionStep step)
		{
			this.UsedUnits.Add(unit);
			this.PlayerActionSteps.Add(step);
		}

		// Token: 0x06003124 RID: 12580 RVA: 0x001298D4 File Offset: 0x00127AD4
		public bool ExchangeResources(Unit unit, int oil, int metal, int food, int wood)
		{
			GainMove.LoadResources loadResources = new GainMove.LoadResources(this.gameManager, this.player);
			if (!loadResources.Execute(unit, oil, metal, food, wood))
			{
				return false;
			}
			this.SaveUnitAndStep(unit, loadResources);
			base.ActionSelected = true;
			return true;
		}

		// Token: 0x06003125 RID: 12581 RVA: 0x00129914 File Offset: 0x00127B14
		public bool UnloadAllResources(Unit unit)
		{
			GainMove.LoadResources loadResources = new GainMove.LoadResources(this.gameManager, this.player);
			if (!loadResources.Execute(unit, -unit.resources[ResourceType.oil], -unit.resources[ResourceType.metal], -unit.resources[ResourceType.food], -unit.resources[ResourceType.wood]))
			{
				return false;
			}
			this.SaveUnitAndStep(unit, loadResources);
			base.ActionSelected = true;
			return true;
		}

		// Token: 0x06003126 RID: 12582 RVA: 0x00129984 File Offset: 0x00127B84
		public bool LoadWorkerToMech(Unit worker, Unit mech)
		{
			GainMove.LoadWorker loadWorker = new GainMove.LoadWorker(this.gameManager, this.player);
			if (!loadWorker.Execute(worker, mech))
			{
				return false;
			}
			this.SaveUnitAndStep(worker, loadWorker);
			this.UsedUnits.Add(mech);
			base.ActionSelected = true;
			return true;
		}

		// Token: 0x06003127 RID: 12583 RVA: 0x001299CC File Offset: 0x00127BCC
		public bool UnloadWorkerFromMech(Unit worker, Unit mech)
		{
			GainMove.UnloadWorker unloadWorker = new GainMove.UnloadWorker(this.gameManager, this.player);
			if (!unloadWorker.Execute(worker, mech))
			{
				return false;
			}
			this.SaveUnitAndStep(worker, unloadWorker);
			this.UsedUnits.Add(mech);
			base.ActionSelected = true;
			return true;
		}

		// Token: 0x06003128 RID: 12584 RVA: 0x00129A14 File Offset: 0x00127C14
		public bool MoveUnitToPosition(Unit unit, GameHex movePosition, int distance)
		{
			GainMove.MoveUnit moveUnit = new GainMove.MoveUnit(this.gameManager, this.player);
			if (!moveUnit.Execute(unit, movePosition, distance))
			{
				return false;
			}
			if (unit.MovesLeft == 0)
			{
				this.MovesLeft -= 1;
			}
			this.SaveUnitAndStep(unit, moveUnit);
			base.ActionSelected = true;
			return true;
		}

		// Token: 0x06003129 RID: 12585 RVA: 0x00129A68 File Offset: 0x00127C68
		public bool WithdrawDefeatedPlayer(Player player, List<Unit> unitsToWithdraw, GameHex withdrawPosition)
		{
			GainMove.WithdrawUnits withdrawUnits = new GainMove.WithdrawUnits(this.gameManager, player);
			if (!withdrawUnits.Execute(unitsToWithdraw, withdrawPosition))
			{
				return false;
			}
			this.PlayerActionSteps.Add(withdrawUnits);
			return true;
		}

		// Token: 0x0600312A RID: 12586 RVA: 0x00129A9C File Offset: 0x00127C9C
		protected static bool EnemyUnitsOnField(GameHex field)
		{
			if (field.Owner == null)
			{
				return false;
			}
			if (field.Owner.character.position == field)
			{
				return true;
			}
			using (List<Mech>.Enumerator enumerator = field.Owner.matFaction.mechs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.position == field)
					{
						return true;
					}
				}
			}
			using (List<Worker>.Enumerator enumerator2 = field.Owner.matPlayer.workers.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.position == field)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600312B RID: 12587 RVA: 0x00129B70 File Offset: 0x00127D70
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			this.MovesLeft = short.Parse(reader.GetAttribute("MovesLeft"));
			reader.ReadStartElement();
			while (reader.Name == "Unit")
			{
				int num = int.Parse(reader.GetAttribute("X"));
				int num2 = int.Parse(reader.GetAttribute("Y"));
				int num3 = int.Parse(reader.GetAttribute("Id"));
				switch (int.Parse(reader.GetAttribute("Type")))
				{
				case 0:
					this.UsedUnits.Add(this.player.character);
					break;
				case 1:
				{
					using (List<Mech>.Enumerator enumerator = this.player.matFaction.mechs.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Mech mech = enumerator.Current;
							if (mech.position.posX == num && mech.position.posY == num2 && mech.Id == num3)
							{
								this.UsedUnits.Add(mech);
								break;
							}
						}
						break;
					}
					goto IL_0118;
				}
				case 2:
					goto IL_0118;
				}
				IL_018B:
				reader.ReadStartElement();
				if (reader.Name != "Unit" && reader.Name != "Step" && reader.NodeType == XmlNodeType.EndElement)
				{
					reader.ReadEndElement();
					continue;
				}
				continue;
				IL_0118:
				foreach (Worker worker in this.player.matPlayer.workers)
				{
					if (worker.position.posX == num && worker.position.posY == num2 && worker.Id == num3)
					{
						this.UsedUnits.Add(worker);
						break;
					}
				}
				goto IL_018B;
			}
		}

		// Token: 0x0600312C RID: 12588 RVA: 0x00129D74 File Offset: 0x00127F74
		public override void WriteXml(XmlWriter writer)
		{
			base.WriteXml(writer);
			writer.WriteAttributeString("MovesLeft", this.MovesLeft.ToString());
			foreach (Unit unit in this.UsedUnits)
			{
				writer.WriteStartElement("Unit");
				writer.WriteAttributeString("X", unit.position.posX.ToString());
				writer.WriteAttributeString("Y", unit.position.posY.ToString());
				writer.WriteAttributeString("Id", unit.Id.ToString());
				writer.WriteAttributeString("Type", ((int)unit.UnitType).ToString());
				writer.WriteEndElement();
			}
		}

		// Token: 0x04002165 RID: 8549
		public short MovesLeft;

		// Token: 0x02000615 RID: 1557
		public enum StepType
		{
			// Token: 0x04002167 RID: 8551
			LoadResource,
			// Token: 0x04002168 RID: 8552
			LoadWorker,
			// Token: 0x04002169 RID: 8553
			UnloadWorker,
			// Token: 0x0400216A RID: 8554
			MoveUnit,
			// Token: 0x0400216B RID: 8555
			WithdrawUnit
		}

		// Token: 0x02000616 RID: 1558
		public abstract class MoveActionStep : IXmlSerializable
		{
			// Token: 0x17000397 RID: 919
			// (get) Token: 0x0600312D RID: 12589 RVA: 0x00046DA6 File Offset: 0x00044FA6
			// (set) Token: 0x0600312E RID: 12590 RVA: 0x00046DAE File Offset: 0x00044FAE
			public GainMove.StepType stepType { get; protected set; }

			// Token: 0x0600312F RID: 12591 RVA: 0x00027E56 File Offset: 0x00026056
			public MoveActionStep()
			{
			}

			// Token: 0x06003130 RID: 12592 RVA: 0x00046DB7 File Offset: 0x00044FB7
			public MoveActionStep(GameManager gameManager, Player player)
			{
				this.gameManager = gameManager;
				this.player = player;
			}

			// Token: 0x06003131 RID: 12593 RVA: 0x00046DCD File Offset: 0x00044FCD
			protected void SetContainerUnit(Unit unit)
			{
				this.containerUnit = unit;
				this.posX = unit.position.posX;
				this.posY = unit.position.posY;
			}

			// Token: 0x06003132 RID: 12594
			public abstract bool Execute();

			// Token: 0x06003133 RID: 12595 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
			public XmlSchema GetSchema()
			{
				return null;
			}

			// Token: 0x06003134 RID: 12596 RVA: 0x00129E5C File Offset: 0x0012805C
			public virtual void ReadXml(XmlReader reader)
			{
				reader.MoveToContent();
				if (!this.gameManager.GameLoading)
				{
					this.player = this.gameManager.GetPlayerByFaction((Faction)int.Parse(reader.GetAttribute("Faction")));
				}
				reader.ReadStartElement();
				if (reader.Name == "Unit")
				{
					int num = int.Parse(reader.GetAttribute("X"));
					int num2 = int.Parse(reader.GetAttribute("Y"));
					int num3 = int.Parse(reader.GetAttribute("Id"));
					switch (int.Parse(reader.GetAttribute("Type")))
					{
					case 0:
						this.containerUnit = this.player.character;
						return;
					case 1:
					{
						using (List<Mech>.Enumerator enumerator = this.player.matFaction.mechs.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Mech mech = enumerator.Current;
								if (mech.position.posX == num && mech.position.posY == num2 && mech.Id == num3)
								{
									this.containerUnit = mech;
									break;
								}
							}
							return;
						}
						break;
					}
					case 2:
						break;
					default:
						return;
					}
					foreach (Worker worker in this.player.matPlayer.workers)
					{
						if (worker.position.posX == num && worker.position.posY == num2 && worker.Id == num3)
						{
							this.containerUnit = worker;
							break;
						}
					}
				}
			}

			// Token: 0x06003135 RID: 12597 RVA: 0x0012A01C File Offset: 0x0012821C
			public virtual void WriteXml(XmlWriter writer)
			{
				string text = "Faction";
				int faction = (int)this.player.matFaction.faction;
				writer.WriteAttributeString(text, faction.ToString());
				writer.WriteAttributeString("Type", ((int)this.stepType).ToString());
				if (this.containerUnit != null)
				{
					writer.WriteStartElement("Unit");
					writer.WriteAttributeString("X", this.containerUnit.position.posX.ToString());
					writer.WriteAttributeString("Y", this.containerUnit.position.posY.ToString());
					writer.WriteAttributeString("Id", this.containerUnit.Id.ToString());
					writer.WriteAttributeString("Type", ((int)this.containerUnit.UnitType).ToString());
					writer.WriteEndElement();
				}
			}

			// Token: 0x0400216D RID: 8557
			protected int posX;

			// Token: 0x0400216E RID: 8558
			protected int posY;

			// Token: 0x0400216F RID: 8559
			public Unit containerUnit;

			// Token: 0x04002170 RID: 8560
			protected Player player;

			// Token: 0x04002171 RID: 8561
			protected GameManager gameManager;
		}

		// Token: 0x02000617 RID: 1559
		public class LoadResources : GainMove.MoveActionStep
		{
			// Token: 0x06003136 RID: 12598 RVA: 0x00046DF8 File Offset: 0x00044FF8
			public LoadResources()
			{
				base.stepType = GainMove.StepType.LoadResource;
			}

			// Token: 0x06003137 RID: 12599 RVA: 0x00046E07 File Offset: 0x00045007
			public LoadResources(GameManager gameManager, Player player)
				: base(gameManager, player)
			{
				base.stepType = GainMove.StepType.LoadResource;
			}

			// Token: 0x06003138 RID: 12600 RVA: 0x00046E18 File Offset: 0x00045018
			public Dictionary<ResourceType, int> GetResources()
			{
				return new Dictionary<ResourceType, int>
				{
					{
						ResourceType.oil,
						this.oil
					},
					{
						ResourceType.food,
						this.food
					},
					{
						ResourceType.metal,
						this.metal
					},
					{
						ResourceType.wood,
						this.wood
					}
				};
			}

			// Token: 0x06003139 RID: 12601 RVA: 0x00046E53 File Offset: 0x00045053
			public override bool Execute()
			{
				return this.Execute(this.containerUnit, this.oil, this.metal, this.food, this.wood);
			}

			// Token: 0x0600313A RID: 12602 RVA: 0x0012A100 File Offset: 0x00128300
			public bool Execute(Unit unit, int oil, int metal, int food, int wood)
			{
				if (!GainMove.LoadResources.CheckLogic(unit, oil, metal, food, wood))
				{
					return false;
				}
				base.SetContainerUnit(unit);
				if (this.gameManager.IsMultiplayer && ((this.gameManager.IsMyTurn() && this.player == this.gameManager.PlayerOwner) || (this.gameManager.PlayerOwner == null && !this.player.IsHuman)))
				{
					this.oil = oil;
					this.food = food;
					this.metal = metal;
					this.wood = wood;
				}
				this.containerUnit.AddToBackpack(oil, metal, food, wood);
				Dictionary<ResourceType, int> dictionary = unit.position.resources;
				dictionary[ResourceType.oil] = dictionary[ResourceType.oil] - oil;
				dictionary = unit.position.resources;
				dictionary[ResourceType.metal] = dictionary[ResourceType.metal] - metal;
				dictionary = unit.position.resources;
				dictionary[ResourceType.food] = dictionary[ResourceType.food] - food;
				dictionary = unit.position.resources;
				dictionary[ResourceType.wood] = dictionary[ResourceType.wood] - wood;
				if (((this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null && !this.gameManager.IsMyTurn()) || (!this.gameManager.IsMultiplayer && !this.gameManager.PlayerCurrent.IsHuman)) && (oil != 0 || metal != 0 || food != 0 || wood != 0))
				{
					LoadResourcesEnemyActionInfo loadResourcesEnemyActionInfo = new LoadResourcesEnemyActionInfo();
					dictionary = loadResourcesEnemyActionInfo.resources;
					dictionary[ResourceType.oil] = dictionary[ResourceType.oil] + oil;
					dictionary = loadResourcesEnemyActionInfo.resources;
					dictionary[ResourceType.metal] = dictionary[ResourceType.metal] + metal;
					dictionary = loadResourcesEnemyActionInfo.resources;
					dictionary[ResourceType.food] = dictionary[ResourceType.food] + food;
					dictionary = loadResourcesEnemyActionInfo.resources;
					dictionary[ResourceType.wood] = dictionary[ResourceType.wood] + wood;
					loadResourcesEnemyActionInfo.unit = unit;
					loadResourcesEnemyActionInfo.actionOwner = unit.Owner.matFaction.faction;
					loadResourcesEnemyActionInfo.actionType = LogInfoType.Move;
					loadResourcesEnemyActionInfo.hex = unit.position;
					if (oil < 0 || metal < 0 || food < 0 || wood < 0)
					{
						loadResourcesEnemyActionInfo.isUnload = true;
					}
					this.gameManager.EnemyLoadResources(loadResourcesEnemyActionInfo);
				}
				return true;
			}

			// Token: 0x0600313B RID: 12603 RVA: 0x00046E79 File Offset: 0x00045079
			private static bool CheckLogic(Unit unit, int oil, int metal, int food, int wood)
			{
				return unit != null && GainMove.LoadResources.CheckResource(unit, ResourceType.oil, oil) && GainMove.LoadResources.CheckResource(unit, ResourceType.metal, metal) && GainMove.LoadResources.CheckResource(unit, ResourceType.food, food) && GainMove.LoadResources.CheckResource(unit, ResourceType.wood, wood);
			}

			// Token: 0x0600313C RID: 12604 RVA: 0x00046EB2 File Offset: 0x000450B2
			private static bool CheckResource(Unit unit, ResourceType resource, int amount)
			{
				if (amount >= 0)
				{
					return unit.position.resources[resource] >= amount;
				}
				return unit.resources[resource] >= -amount;
			}

			// Token: 0x0600313D RID: 12605 RVA: 0x0012A324 File Offset: 0x00128524
			public override void ReadXml(XmlReader reader)
			{
				base.ReadXml(reader);
				reader.ReadStartElement();
				if (reader.Name == "Resources")
				{
					if (reader.GetAttribute("R0") != null)
					{
						this.oil += int.Parse(reader.GetAttribute("R0"));
					}
					if (reader.GetAttribute("R1") != null)
					{
						this.metal += int.Parse(reader.GetAttribute("R1"));
					}
					if (reader.GetAttribute("R2") != null)
					{
						this.food += int.Parse(reader.GetAttribute("R2"));
					}
					if (reader.GetAttribute("R3") != null)
					{
						this.wood += int.Parse(reader.GetAttribute("R3"));
					}
				}
				reader.ReadStartElement();
			}

			// Token: 0x0600313E RID: 12606 RVA: 0x0012A404 File Offset: 0x00128604
			public override void WriteXml(XmlWriter writer)
			{
				base.WriteXml(writer);
				writer.WriteStartElement("Resources");
				if (this.oil != 0)
				{
					writer.WriteAttributeString("R" + 0.ToString(), this.oil.ToString());
				}
				if (this.metal != 0)
				{
					writer.WriteAttributeString("R" + 1.ToString(), this.metal.ToString());
				}
				if (this.food != 0)
				{
					writer.WriteAttributeString("R" + 2.ToString(), this.food.ToString());
				}
				if (this.wood != 0)
				{
					writer.WriteAttributeString("R" + 3.ToString(), this.wood.ToString());
				}
				writer.WriteEndElement();
			}

			// Token: 0x04002172 RID: 8562
			private int oil;

			// Token: 0x04002173 RID: 8563
			private int metal;

			// Token: 0x04002174 RID: 8564
			private int food;

			// Token: 0x04002175 RID: 8565
			private int wood;
		}

		// Token: 0x02000618 RID: 1560
		public class LoadWorker : GainMove.MoveActionStep
		{
			// Token: 0x0600313F RID: 12607 RVA: 0x00046EE3 File Offset: 0x000450E3
			public LoadWorker()
			{
				base.stepType = GainMove.StepType.LoadWorker;
			}

			// Token: 0x06003140 RID: 12608 RVA: 0x00046EF2 File Offset: 0x000450F2
			public LoadWorker(GameManager gameManager, Player player)
				: base(gameManager, player)
			{
				base.stepType = GainMove.StepType.LoadWorker;
			}

			// Token: 0x06003141 RID: 12609 RVA: 0x00046F03 File Offset: 0x00045103
			public override bool Execute()
			{
				return this.Execute(this.worker, this.containerUnit);
			}

			// Token: 0x06003142 RID: 12610 RVA: 0x0012A4DC File Offset: 0x001286DC
			public bool Execute(Unit worker, Unit mech)
			{
				if (!GainMove.LoadWorker.CheckLogic(worker, mech))
				{
					return false;
				}
				this.worker = worker;
				worker.SavePosition();
				base.SetContainerUnit(mech);
				if ((this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null && this.gameManager.PlayerOwner != this.player) || (!this.gameManager.IsMultiplayer && !this.gameManager.PlayerCurrent.IsHuman) || this.gameManager.SpectatorMode)
				{
					LoadWorkerActionInfo loadWorkerActionInfo = new LoadWorkerActionInfo();
					loadWorkerActionInfo.worker = worker;
					loadWorkerActionInfo.mech = mech;
					worker.IncreaseMoveAnimationAmount();
					loadWorkerActionInfo.actionOwner = mech.Owner.matFaction.faction;
					loadWorkerActionInfo.actionType = LogInfoType.Move;
					this.gameManager.EnemyLoadWorker(loadWorkerActionInfo);
				}
				(this.containerUnit as Mech).LoadWorker(this.worker as Worker);
				return true;
			}

			// Token: 0x06003143 RID: 12611 RVA: 0x0012A5C4 File Offset: 0x001287C4
			private bool NordicRetreat()
			{
				return this.gameManager.combatManager.CombatAlreadyStarted() && this.gameManager.combatManager.GetDefeated() != null && this.gameManager.combatManager.GetDefeated().matFaction.faction == Faction.Nordic && this.gameManager.PlayerOwner == this.gameManager.combatManager.GetDefeated() && this.gameManager.combatManager.GetDefeated().matFaction.SkillUnlocked[1];
			}

			// Token: 0x06003144 RID: 12612 RVA: 0x0012A650 File Offset: 0x00128850
			public static bool CheckLogic(Unit worker, Unit mech)
			{
				return worker != null && mech != null && worker.position == mech.position && mech is Mech && worker is Worker && !(mech as Mech).LoadedWorkers.Contains(worker as Worker);
			}

			// Token: 0x06003145 RID: 12613 RVA: 0x0012A6A4 File Offset: 0x001288A4
			public override void ReadXml(XmlReader reader)
			{
				base.ReadXml(reader);
				reader.ReadStartElement();
				if (reader.Name == "Worker")
				{
					int num = int.Parse(reader.GetAttribute("X"));
					int num2 = int.Parse(reader.GetAttribute("Y"));
					int num3 = int.Parse(reader.GetAttribute("Id"));
					foreach (Worker worker in this.player.matPlayer.workers)
					{
						if (worker.position.posX == num && worker.position.posY == num2 && !worker.OnMech && worker.Id == num3)
						{
							this.worker = worker;
							break;
						}
					}
				}
				reader.ReadStartElement();
			}

			// Token: 0x06003146 RID: 12614 RVA: 0x0012A794 File Offset: 0x00128994
			public override void WriteXml(XmlWriter writer)
			{
				base.WriteXml(writer);
				writer.WriteStartElement("Worker");
				writer.WriteAttributeString("X", this.worker.position.posX.ToString());
				writer.WriteAttributeString("Y", this.worker.position.posY.ToString());
				writer.WriteAttributeString("Id", this.worker.Id.ToString());
				writer.WriteEndElement();
			}

			// Token: 0x04002176 RID: 8566
			public Unit worker;
		}

		// Token: 0x02000619 RID: 1561
		public class UnloadWorker : GainMove.MoveActionStep
		{
			// Token: 0x06003147 RID: 12615 RVA: 0x00046F17 File Offset: 0x00045117
			public UnloadWorker()
			{
				base.stepType = GainMove.StepType.UnloadWorker;
			}

			// Token: 0x06003148 RID: 12616 RVA: 0x00046F26 File Offset: 0x00045126
			public UnloadWorker(GameManager gameManager, Player player)
				: base(gameManager, player)
			{
				base.stepType = GainMove.StepType.UnloadWorker;
			}

			// Token: 0x06003149 RID: 12617 RVA: 0x00046F37 File Offset: 0x00045137
			public override bool Execute()
			{
				return this.Execute(this.worker, this.containerUnit);
			}

			// Token: 0x0600314A RID: 12618 RVA: 0x0012A818 File Offset: 0x00128A18
			public bool Execute(Unit worker, Unit mech)
			{
				if (!GainMove.UnloadWorker.CheckLogic(worker, mech))
				{
					return false;
				}
				this.worker = worker;
				base.SetContainerUnit(mech);
				if ((this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null && this.gameManager.PlayerOwner != this.player) || (!this.gameManager.IsMultiplayer && !this.gameManager.PlayerCurrent.IsHuman) || this.gameManager.SpectatorMode)
				{
					UnloadWorkerActionInfo unloadWorkerActionInfo = new UnloadWorkerActionInfo();
					unloadWorkerActionInfo.worker = worker;
					unloadWorkerActionInfo.mech = mech;
					unloadWorkerActionInfo.positionToUnload = mech.position;
					unloadWorkerActionInfo.actionOwner = mech.Owner.matFaction.faction;
					unloadWorkerActionInfo.unloadOnBattlefield = worker.position.Conflict(worker);
					worker.IncreaseMoveAnimationAmount();
					unloadWorkerActionInfo.actionType = LogInfoType.Move;
					this.gameManager.EnemyUnloadWorker(unloadWorkerActionInfo);
				}
				(this.containerUnit as Mech).UnloadWorker(this.worker as Worker);
				return true;
			}

			// Token: 0x0600314B RID: 12619 RVA: 0x00046F4B File Offset: 0x0004514B
			private static bool CheckLogic(Unit worker, Unit mech)
			{
				return worker != null && mech != null && mech is Mech && worker is Worker && (mech as Mech).LoadedWorkers.Contains(worker as Worker);
			}

			// Token: 0x0600314C RID: 12620 RVA: 0x0012A918 File Offset: 0x00128B18
			public override void ReadXml(XmlReader reader)
			{
				base.ReadXml(reader);
				reader.ReadStartElement();
				if (reader.Name == "Worker")
				{
					int num = int.Parse(reader.GetAttribute("X"));
					int num2 = int.Parse(reader.GetAttribute("Y"));
					int num3 = int.Parse(reader.GetAttribute("Id"));
					foreach (Worker worker in this.player.matPlayer.workers)
					{
						if (worker.position.posX == num && worker.position.posY == num2 && worker.OnMech && worker.Id == num3)
						{
							this.worker = worker;
							break;
						}
					}
				}
				reader.ReadStartElement();
			}

			// Token: 0x0600314D RID: 12621 RVA: 0x0012AA08 File Offset: 0x00128C08
			public override void WriteXml(XmlWriter writer)
			{
				base.WriteXml(writer);
				writer.WriteStartElement("Worker");
				writer.WriteAttributeString("X", this.worker.position.posX.ToString());
				writer.WriteAttributeString("Y", this.worker.position.posY.ToString());
				writer.WriteAttributeString("Id", this.worker.Id.ToString());
				writer.WriteEndElement();
			}

			// Token: 0x04002177 RID: 8567
			public Unit worker;
		}

		// Token: 0x0200061A RID: 1562
		public class MoveUnit : GainMove.MoveActionStep
		{
			// Token: 0x0600314E RID: 12622 RVA: 0x00046F82 File Offset: 0x00045182
			public MoveUnit()
			{
				base.stepType = GainMove.StepType.MoveUnit;
			}

			// Token: 0x0600314F RID: 12623 RVA: 0x00046F91 File Offset: 0x00045191
			public MoveUnit(GameManager gameManager, Player player)
				: base(gameManager, player)
			{
				base.stepType = GainMove.StepType.MoveUnit;
			}

			// Token: 0x06003150 RID: 12624 RVA: 0x00046FA2 File Offset: 0x000451A2
			public override bool Execute()
			{
				return this.Execute(this.containerUnit, this.movePosition, this.distance);
			}

			// Token: 0x06003151 RID: 12625 RVA: 0x0012AA8C File Offset: 0x00128C8C
			public bool Execute(Unit unit, GameHex movePosition, int distance)
			{
				if (!this.CheckLogic(this.gameManager, unit, movePosition, distance))
				{
					return false;
				}
				if (unit.NotMoved())
				{
					unit.SavePosition();
				}
				base.SetContainerUnit(unit);
				this.movePosition = movePosition;
				this.distance = distance;
				this.lastPosition = unit.position;
				this.player.DistanceTravelled += distance;
				this.containerUnit.MoveTo(this.movePosition);
				if ((this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null && !this.gameManager.IsMyTurn()) || (!this.gameManager.IsMultiplayer && !this.gameManager.PlayerCurrent.IsHuman))
				{
					if (!this.gameManager.showEnemyActions)
					{
						unit.IncreaseMoveAnimationAmount();
						this.gameManager.MoveUnit(unit, this.gameManager.gameBoard.MoveRange(unit, distance));
					}
					else if (distance != 0)
					{
						unit.IncreaseMoveAnimationAmount();
						MoveEnemyActionInfo moveEnemyActionInfo = new MoveEnemyActionInfo();
						moveEnemyActionInfo.actionOwner = this.player.matFaction.faction;
						moveEnemyActionInfo.actionType = LogInfoType.Move;
						moveEnemyActionInfo.possibleMoves = this.gameManager.gameBoard.MoveRange(unit, distance);
						moveEnemyActionInfo.unit = unit;
						moveEnemyActionInfo.destinationIsBattlefield = movePosition.Conflict(unit);
						moveEnemyActionInfo.fromHex = this.lastPosition;
						moveEnemyActionInfo.toHex = unit.position;
						if (movePosition.Conflict(unit))
						{
							moveEnemyActionInfo.moveFromPosition = unit.position;
						}
						this.gameManager.EnemyMoveUnit(moveEnemyActionInfo);
					}
				}
				if (movePosition.Owner != this.player)
				{
					if (!GainMove.EnemyUnitsOnField(movePosition))
					{
						movePosition.Owner = this.player;
					}
					else
					{
						unit.MovesLeft = 0;
						movePosition.Enemy = this.player;
						unit.SaveResources();
						if (unit is Mech && movePosition.GetOwnerUnitCount() != movePosition.GetOwnerWorkers().Count)
						{
							Mech mech = unit as Mech;
							for (int i = 0; i < mech.LoadedWorkers.Count; i++)
							{
								mech.LoadedWorkers[i].MovesLeft = 0;
							}
						}
						this.gameManager.combatManager.AddBattlefield(movePosition);
					}
				}
				if (movePosition.Token != null && movePosition.Token is TrapToken)
				{
					TrapToken trapToken = movePosition.Token as TrapToken;
					if (trapToken.Armed && trapToken.Owner != unit.Owner)
					{
						trapToken.OnTokenTriggered(unit);
						unit.MovesLeft = 0;
					}
				}
				if (movePosition.hasEncounter && !movePosition.encounterUsed && unit is Character)
				{
					unit.MovesLeft = 0;
				}
				if (this.lastPosition.IsRiverBetween(movePosition) && !this.lastPosition.CanMoveThroughTunnels(movePosition, unit) && (unit.Owner.matFaction.faction != Faction.Togawa || movePosition.Token == null || !(movePosition.Token is TrapToken)))
				{
					unit.Owner.matFaction.SetRiverwalkUsed(true);
				}
				if (!this.player.OwnedFields(false).Contains(this.lastPosition) && this.lastPosition.Enemy != this.player)
				{
					if (this.lastPosition.Building == null)
					{
						this.lastPosition.Owner = null;
					}
					else if (!this.player.OwnedFields(false).Contains(this.lastPosition))
					{
						this.lastPosition.Owner = this.lastPosition.Building.player;
					}
				}
				this.CreateActionInfo(new string[] { unit.UnitType.ToString() });
				if (unit.MovesLeft != 0)
				{
					unit.MovesLeft -= (short)distance;
				}
				return true;
			}

			// Token: 0x06003152 RID: 12626 RVA: 0x0012AE24 File Offset: 0x00129024
			protected void CreateActionInfo(params string[] infoList)
			{
				if (infoList.Length == 0)
				{
					return;
				}
				string text = "^" + this.player.matFaction.faction.ToString()[0].ToString() + "Moving " + infoList[0];
				this.gameManager.BroadcastActionInfo(text);
			}

			// Token: 0x06003153 RID: 12627 RVA: 0x0012AE80 File Offset: 0x00129080
			public bool CheckLogic(GameManager gameManager, Unit unit, GameHex movePosition, int distance)
			{
				if (unit == null || movePosition == null)
				{
					return false;
				}
				if ((!gameManager.IsMultiplayer || (gameManager.PlayerOwner != null && unit.Owner == gameManager.PlayerOwner)) && ((unit.MovesLeft == 0 && distance != 0) || (int)unit.MovesLeft < distance))
				{
					return false;
				}
				if (distance == 0)
				{
					return true;
				}
				if (movePosition.Owner != unit.Owner && unit is Worker && GainMove.EnemyUnitsOnField(movePosition))
				{
					return false;
				}
				if (unit.position.IsRiverBetween(movePosition) && distance == 1 && !this.TunnelMove(unit, movePosition))
				{
					if (!unit.Owner.matFaction.CanRiverwalkAgain() && (movePosition.Token == null || !(movePosition.Token is TrapToken)))
					{
						return false;
					}
					if (unit is Worker)
					{
						if (unit.Owner.matFaction.factionPerk != AbilityPerk.Swim)
						{
							return false;
						}
					}
					else if (!unit.Owner.matFaction.CanRiverwalk(unit.position, movePosition, unit))
					{
						return (unit.Owner.matFaction.faction == Faction.Togawa && movePosition.Token != null && movePosition.Token is TrapToken) || (unit.Owner.matFaction.faction == Faction.Albion && ((movePosition.Token != null && movePosition.Token is FlagToken) || movePosition.GetOwnerWorkers().Count > 0));
					}
				}
				if (unit.position.hexType == HexType.lake)
				{
					if (unit is Worker)
					{
						return false;
					}
					if (unit is Mech)
					{
						bool flag = true;
						foreach (Mech mech in unit.position.Owner.matFaction.mechs)
						{
							if (mech != unit && mech.position == unit.position)
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							foreach (Worker worker in unit.position.Owner.matPlayer.workers)
							{
								if (!worker.OnMech && worker.position == unit.position)
								{
									return false;
								}
							}
						}
					}
					if (!(unit is Mech) && !(unit is Character))
					{
						return true;
					}
					bool flag2 = true;
					using (List<Mech>.Enumerator enumerator = unit.position.GetOwnerMechs().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current != unit)
							{
								flag2 = false;
								break;
							}
						}
					}
					if (flag2 && unit.position == unit.Owner.character.position && unit != unit.Owner.character)
					{
						flag2 = false;
					}
					if (flag2)
					{
						foreach (KeyValuePair<ResourceType, int> keyValuePair in unit.position.resources)
						{
							if (keyValuePair.Value != 0)
							{
								return false;
							}
						}
						return true;
					}
					return true;
				}
				return true;
			}

			// Token: 0x06003154 RID: 12628 RVA: 0x00046FBC File Offset: 0x000451BC
			private bool TunnelMove(Unit unit, GameHex movePosition)
			{
				return (this.UnitHasAccessToTunnel(unit, movePosition) || this.UnitHasSaxonianUnderpassAbility(unit)) && this.EndPositionHasAccessToTunnel(unit, movePosition);
			}

			// Token: 0x06003155 RID: 12629 RVA: 0x0012B1BC File Offset: 0x001293BC
			private bool UnitHasAccessToTunnel(Unit unit, GameHex movePosition)
			{
				return unit.position.hasTunnel || (unit.position.Building != null && unit.position.Building.player == unit.Owner && unit.position.Building.buildingType == BuildingType.Mine);
			}

			// Token: 0x06003156 RID: 12630 RVA: 0x0012B214 File Offset: 0x00129414
			private bool UnitHasSaxonianUnderpassAbility(Unit unit)
			{
				return unit.Owner.matFaction.faction == Faction.Saxony && unit.UnitType != UnitType.Worker && unit.Owner.matFaction.abilities.Contains(AbilityPerk.Underpass) && unit.position.hexType == HexType.mountain;
			}

			// Token: 0x06003157 RID: 12631 RVA: 0x00046FDB File Offset: 0x000451DB
			private bool EndPositionHasAccessToTunnel(Unit unit, GameHex movePosition)
			{
				return movePosition.hasTunnel || (movePosition.Building != null && movePosition.Building.player == unit.Owner && movePosition.Building.buildingType == BuildingType.Mine);
			}

			// Token: 0x06003158 RID: 12632 RVA: 0x0012B268 File Offset: 0x00129468
			public override void ReadXml(XmlReader reader)
			{
				base.ReadXml(reader);
				reader.ReadStartElement();
				if (reader.Name == "Hex")
				{
					int num = int.Parse(reader.GetAttribute("X"));
					int num2 = int.Parse(reader.GetAttribute("Y"));
					this.distance = int.Parse(reader.GetAttribute("Distance"));
					this.movePosition = this.gameManager.gameBoard.hexMap[num, num2];
				}
				reader.ReadStartElement();
				this.lastPosition = this.containerUnit.position;
			}

			// Token: 0x06003159 RID: 12633 RVA: 0x0012B300 File Offset: 0x00129500
			public override void WriteXml(XmlWriter writer)
			{
				base.WriteXml(writer);
				writer.WriteStartElement("Hex");
				writer.WriteAttributeString("X", this.movePosition.posX.ToString());
				writer.WriteAttributeString("Y", this.movePosition.posY.ToString());
				writer.WriteAttributeString("Distance", this.distance.ToString());
				writer.WriteEndElement();
			}

			// Token: 0x04002178 RID: 8568
			public GameHex lastPosition;

			// Token: 0x04002179 RID: 8569
			public GameHex movePosition;

			// Token: 0x0400217A RID: 8570
			private int distance;
		}

		// Token: 0x0200061B RID: 1563
		public class WithdrawUnits : GainMove.MoveActionStep
		{
			// Token: 0x0600315A RID: 12634 RVA: 0x00047012 File Offset: 0x00045212
			public WithdrawUnits()
			{
				base.stepType = GainMove.StepType.WithdrawUnit;
				this.unitsToWithdraw = new List<Unit>();
			}

			// Token: 0x0600315B RID: 12635 RVA: 0x0004702C File Offset: 0x0004522C
			public WithdrawUnits(GameManager gameManager, Player player)
				: base(gameManager, player)
			{
				base.stepType = GainMove.StepType.WithdrawUnit;
				this.unitsToWithdraw = new List<Unit>();
			}

			// Token: 0x0600315C RID: 12636 RVA: 0x00047048 File Offset: 0x00045248
			public List<Unit> GetUnitsToWithdraw()
			{
				return this.unitsToWithdraw;
			}

			// Token: 0x0600315D RID: 12637 RVA: 0x00047050 File Offset: 0x00045250
			public GameHex GetWithdrawPosition()
			{
				return this.withdrawPosition;
			}

			// Token: 0x0600315E RID: 12638 RVA: 0x00047058 File Offset: 0x00045258
			public override bool Execute()
			{
				return this.Execute(this.unitsToWithdraw, this.withdrawPosition);
			}

			// Token: 0x0600315F RID: 12639 RVA: 0x0012B374 File Offset: 0x00129574
			public bool Execute(List<Unit> unitsToWithdraw, GameHex withdrawPosition)
			{
				if (!GainMove.WithdrawUnits.CheckLogic(this.gameManager, unitsToWithdraw, withdrawPosition))
				{
					return false;
				}
				this.unitsToWithdraw = new List<Unit>(unitsToWithdraw);
				this.withdrawPosition = withdrawPosition;
				this.SendActionIfNeeded();
				GainMove.MoveUnit moveUnit = new GainMove.MoveUnit(this.gameManager, this.player);
				List<UnloadWorkerActionInfo> list = new List<UnloadWorkerActionInfo>();
				foreach (Unit unit in unitsToWithdraw)
				{
					moveUnit.Execute(unit, withdrawPosition, 0);
					if (unit is Mech)
					{
						Mech mech = unit as Mech;
						while (mech.LoadedWorkers.Count != 0)
						{
							Worker worker = mech.LoadedWorkers[0];
							mech.UnloadWorker(worker);
							if ((this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null && this.gameManager.PlayerOwner != this.player) || (!this.gameManager.IsMultiplayer && !this.gameManager.PlayerCurrent.IsHuman) || this.gameManager.SpectatorMode)
							{
								UnloadWorkerActionInfo unloadWorkerActionInfo = new UnloadWorkerActionInfo();
								unloadWorkerActionInfo.worker = worker;
								unloadWorkerActionInfo.mech = mech;
								unloadWorkerActionInfo.positionToUnload = mech.position;
								unloadWorkerActionInfo.actionOwner = mech.Owner.matFaction.faction;
								unloadWorkerActionInfo.unloadOnBattlefield = worker.position.Conflict(worker);
								worker.IncreaseMoveAnimationAmount();
								unloadWorkerActionInfo.actionType = LogInfoType.Move;
								list.Add(unloadWorkerActionInfo);
							}
						}
					}
				}
				if ((this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null && this.player != this.gameManager.PlayerOwner) || (!this.gameManager.IsMultiplayer && !this.player.IsHuman))
				{
					MoveRetreatEnemyActionInfo moveRetreatEnemyActionInfo = new MoveRetreatEnemyActionInfo();
					moveRetreatEnemyActionInfo.actionOwner = this.player.matFaction.faction;
					moveRetreatEnemyActionInfo.actionType = LogInfoType.RetreatMove;
					moveRetreatEnemyActionInfo.withdrawPositionHex = withdrawPosition;
					foreach (Unit unit2 in unitsToWithdraw)
					{
						moveRetreatEnemyActionInfo.units.Add(unit2);
						unit2.IncreaseMoveAnimationAmount();
					}
					this.gameManager.EnemyRetreatMoveUnit(moveRetreatEnemyActionInfo);
					if (this.gameManager.IsMultiplayer)
					{
						foreach (UnloadWorkerActionInfo unloadWorkerActionInfo2 in list)
						{
							this.gameManager.EnemyUnloadWorker(unloadWorkerActionInfo2);
						}
					}
				}
				return true;
			}

			// Token: 0x06003160 RID: 12640 RVA: 0x0012B660 File Offset: 0x00129860
			private void SendActionIfNeeded()
			{
				if (!this.gameManager.IsMultiplayer)
				{
					return;
				}
				if (this.gameManager.PlayerOwner == null && this.player.IsHuman)
				{
					return;
				}
				if (this.gameManager.PlayerOwner != null && this.player != this.gameManager.PlayerOwner)
				{
					return;
				}
				CombatManager combatManager = this.gameManager.combatManager;
				if (this.player.matFaction.abilities.Contains(AbilityPerk.Seaworthy) && this.player.matFaction.SkillUnlocked[1] && combatManager.GetWithdrawPositions().Count > 1 && !combatManager.WorkersRetreat())
				{
					this.SendAction();
				}
			}

			// Token: 0x06003161 RID: 12641 RVA: 0x0012B70C File Offset: 0x0012990C
			private void SendAction()
			{
				foreach (Unit unit in this.unitsToWithdraw)
				{
					unit.SavePosition();
				}
				this.gameManager.OnActionSent(new WithdrawUnitsMessage(this.withdrawPosition, this.unitsToWithdraw));
			}

			// Token: 0x06003162 RID: 12642 RVA: 0x0004706C File Offset: 0x0004526C
			public bool CheckLogic()
			{
				return GainMove.WithdrawUnits.CheckLogic(this.gameManager, this.unitsToWithdraw, this.withdrawPosition);
			}

			// Token: 0x06003163 RID: 12643 RVA: 0x0012B778 File Offset: 0x00129978
			public static bool CheckLogic(GameManager gameManager, List<Unit> unitsToWithdraw, GameHex withdrawPosition)
			{
				if (unitsToWithdraw == null)
				{
					return false;
				}
				if (unitsToWithdraw.Count == 0)
				{
					return false;
				}
				if (gameManager.GameFinished)
				{
					foreach (Unit unit in unitsToWithdraw)
					{
					}
					return true;
				}
				return (withdrawPosition.Owner == null || unitsToWithdraw[0].Owner == withdrawPosition.Owner) && ((unitsToWithdraw[0].Owner.matFaction.abilities.Contains(AbilityPerk.Seaworthy) && withdrawPosition.hexType == HexType.lake) || gameManager.IsCampaign || (withdrawPosition.hexType == HexType.capital && (withdrawPosition.hexType != HexType.capital || withdrawPosition.factionBase == unitsToWithdraw[0].Owner.matFaction.faction)));
			}

			// Token: 0x06003164 RID: 12644 RVA: 0x0012B864 File Offset: 0x00129A64
			public override void ReadXml(XmlReader reader)
			{
				base.ReadXml(reader);
				if (reader.Name == "Hex")
				{
					int num = int.Parse(reader.GetAttribute("X"));
					int num2 = int.Parse(reader.GetAttribute("Y"));
					this.withdrawPosition = this.gameManager.gameBoard.hexMap[num, num2];
				}
				reader.ReadStartElement();
				while (reader.Name == "Unit")
				{
					int num3 = int.Parse(reader.GetAttribute("X"));
					int num4 = int.Parse(reader.GetAttribute("Y"));
					int num5 = int.Parse(reader.GetAttribute("Id"));
					UnitType unitType = (UnitType)int.Parse(reader.GetAttribute("UnitType"));
					Unit unit = null;
					switch (unitType)
					{
					case UnitType.Character:
						unit = this.player.character;
						break;
					case UnitType.Mech:
					{
						using (List<Mech>.Enumerator enumerator = this.player.matFaction.mechs.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Mech mech = enumerator.Current;
								if (mech.position.posX == num3 && mech.position.posY == num4 && mech.Id == num5)
								{
									unit = mech;
									while (mech.LoadedWorkers.Count != 0)
									{
										Worker worker = mech.LoadedWorkers[0];
										mech.UnloadWorker(worker);
									}
									break;
								}
							}
							break;
						}
						goto IL_016C;
					}
					case UnitType.Worker:
						goto IL_016C;
					}
					IL_01D6:
					this.unitsToWithdraw.Add(unit);
					reader.ReadStartElement();
					continue;
					IL_016C:
					foreach (Worker worker2 in this.player.matPlayer.workers)
					{
						if (worker2.position.posX == num3 && worker2.position.posY == num4 && worker2.Id == num5)
						{
							unit = worker2;
							break;
						}
					}
					goto IL_01D6;
				}
			}

			// Token: 0x06003165 RID: 12645 RVA: 0x0012BA8C File Offset: 0x00129C8C
			public override void WriteXml(XmlWriter writer)
			{
				base.WriteXml(writer);
				writer.WriteStartElement("Hex");
				writer.WriteAttributeString("X", this.withdrawPosition.posX.ToString());
				writer.WriteAttributeString("Y", this.withdrawPosition.posY.ToString());
				writer.WriteEndElement();
				foreach (Unit unit in this.unitsToWithdraw)
				{
					writer.WriteStartElement("Unit");
					writer.WriteAttributeString("X", unit.position.posX.ToString());
					writer.WriteAttributeString("Y", unit.position.posY.ToString());
					writer.WriteAttributeString("Id", unit.Id.ToString());
					writer.WriteAttributeString("UnitType", ((int)unit.UnitType).ToString());
					writer.WriteEndElement();
					if (unit is Mech)
					{
						foreach (Worker worker in ((Mech)unit).LoadedWorkers)
						{
							writer.WriteStartElement("Unit");
							writer.WriteAttributeString("X", worker.position.posX.ToString());
							writer.WriteAttributeString("Y", worker.position.posY.ToString());
							writer.WriteAttributeString("Id", worker.Id.ToString());
							writer.WriteAttributeString("UnitType", ((int)worker.UnitType).ToString());
							writer.WriteEndElement();
						}
					}
				}
			}

			// Token: 0x0400217B RID: 8571
			private List<Unit> unitsToWithdraw;

			// Token: 0x0400217C RID: 8572
			private GameHex withdrawPosition;
		}
	}
}
