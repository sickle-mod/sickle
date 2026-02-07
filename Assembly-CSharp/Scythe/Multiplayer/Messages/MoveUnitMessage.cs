using System;
using System.Collections.Generic;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002B6 RID: 694
	public class MoveUnitMessage : Message, IExecutableMessage
	{
		// Token: 0x060015AD RID: 5549 RVA: 0x0009E0D4 File Offset: 0x0009C2D4
		public MoveUnitMessage(GameHex end, GameHex start, Unit unit, int distance, Dictionary<ResourceType, int> resources, List<Unit> loadedWorkers)
		{
			this.startX = start.posX;
			this.startY = start.posY;
			this.endX = end.posX;
			this.endY = end.posY;
			this.unitType = (int)unit.UnitType;
			this.unitId = unit.Id;
			this.distance = distance;
			if (resources != null)
			{
				this.oil += resources[ResourceType.oil];
				this.metal += resources[ResourceType.metal];
				this.food += resources[ResourceType.food];
				this.wood += resources[ResourceType.wood];
			}
			if (loadedWorkers != null)
			{
				this.workersIndexes = new List<int>(loadedWorkers.Count);
				using (List<Unit>.Enumerator enumerator = loadedWorkers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Unit unit2 = enumerator.Current;
						this.workersIndexes.Add(unit2.Id);
					}
					return;
				}
			}
			this.workersIndexes = new List<int>(0);
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x0009E200 File Offset: 0x0009C400
		public void Execute(GameManager gameManager)
		{
			Unit unit = gameManager.moveManager.GetUnit((UnitType)this.unitType, this.startX, this.startY, this.unitId);
			gameManager.moveManager.SelectUnit(unit);
			GameHex gameHex = gameManager.gameBoard.hexMap[this.endX, this.endY];
			Dictionary<ResourceType, int> dictionary = new Dictionary<ResourceType, int>
			{
				{
					ResourceType.food,
					this.food
				},
				{
					ResourceType.metal,
					this.metal
				},
				{
					ResourceType.oil,
					this.oil
				},
				{
					ResourceType.wood,
					this.wood
				}
			};
			List<Unit> list = null;
			if (this.workersIndexes != null)
			{
				list = new List<Unit>(this.workersIndexes.Count);
				foreach (int num in this.workersIndexes)
				{
					Unit unit2 = gameManager.moveManager.GetUnit(UnitType.Worker, this.startX, this.startY, num);
					list.Add(unit2);
				}
			}
			gameManager.moveManager.MoveSelectedUnit(gameHex, dictionary, list);
		}

		// Token: 0x04000FEC RID: 4076
		private int startX;

		// Token: 0x04000FED RID: 4077
		private int startY;

		// Token: 0x04000FEE RID: 4078
		private int unitType;

		// Token: 0x04000FEF RID: 4079
		private int unitId;

		// Token: 0x04000FF0 RID: 4080
		private int endX;

		// Token: 0x04000FF1 RID: 4081
		private int endY;

		// Token: 0x04000FF2 RID: 4082
		private int distance;

		// Token: 0x04000FF3 RID: 4083
		private int oil;

		// Token: 0x04000FF4 RID: 4084
		private int metal;

		// Token: 0x04000FF5 RID: 4085
		private int food;

		// Token: 0x04000FF6 RID: 4086
		private int wood;

		// Token: 0x04000FF7 RID: 4087
		private List<int> workersIndexes;
	}
}
