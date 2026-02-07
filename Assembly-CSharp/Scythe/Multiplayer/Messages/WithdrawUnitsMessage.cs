using System;
using System.Collections.Generic;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002B9 RID: 697
	public class WithdrawUnitsMessage : Message, IExecutableMessage
	{
		// Token: 0x060015B5 RID: 5557 RVA: 0x0009E3B8 File Offset: 0x0009C5B8
		public WithdrawUnitsMessage(GameHex withdrawPosition, List<Unit> unitsToWithdraw)
		{
			this.withdrawPositionX = withdrawPosition.posX;
			this.withdrawPositionY = withdrawPosition.posY;
			this.unitsIndexes = new List<int>();
			this.unitsTypes = new List<int>();
			this.workersToLoad = new List<int>();
			foreach (Unit unit in unitsToWithdraw)
			{
				this.unitsIndexes.Add(unit.Id);
				this.unitsTypes.Add((int)unit.UnitType);
			}
			if (unitsToWithdraw[0] is Mech)
			{
				Mech mech = unitsToWithdraw[0] as Mech;
				for (int i = 0; i < mech.LoadedWorkers.Count; i++)
				{
					this.workersToLoad.Add(mech.LoadedWorkers[i].Id);
				}
			}
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x0009E4B0 File Offset: 0x0009C6B0
		public void Execute(GameManager gameManager)
		{
			GameHex selectedBattlefield = gameManager.combatManager.GetSelectedBattlefield();
			List<Unit> list = new List<Unit>();
			for (int i = 0; i < this.unitsIndexes.Count; i++)
			{
				UnitType unitType = (UnitType)this.unitsTypes[i];
				Unit unit = gameManager.moveManager.GetUnit(unitType, selectedBattlefield.posX, selectedBattlefield.posY, this.unitsIndexes[i], gameManager.combatManager.GetDefeated());
				if (unitType != UnitType.Worker || !this.workersToLoad.Contains(unit.Id))
				{
					list.Add(unit);
				}
			}
			GameHex gameHex = gameManager.gameBoard.hexMap[this.withdrawPositionX, this.withdrawPositionY];
			GainMove gainMove = new GainMove(gameManager);
			gainMove.SetPlayer(gameManager.combatManager.GetDefeated());
			if (this.workersToLoad != null && this.workersToLoad.Count > 0)
			{
				foreach (int num in this.workersToLoad)
				{
					Unit unit2 = gameManager.moveManager.GetUnit(UnitType.Worker, selectedBattlefield.posX, selectedBattlefield.posY, num, gameManager.combatManager.GetDefeated());
					gainMove.LoadWorkerToMech(unit2, list[0]);
				}
			}
			gainMove.WithdrawDefeatedPlayer(gameManager.combatManager.GetDefeated(), list, gameHex);
			gainMove.Execute();
			gainMove.Clear();
		}

		// Token: 0x04000FFB RID: 4091
		private int withdrawPositionX;

		// Token: 0x04000FFC RID: 4092
		private int withdrawPositionY;

		// Token: 0x04000FFD RID: 4093
		private List<int> unitsIndexes;

		// Token: 0x04000FFE RID: 4094
		private List<int> unitsTypes;

		// Token: 0x04000FFF RID: 4095
		private List<int> workersToLoad;
	}
}
