using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Scythe.GameLogic.Actions;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic
{
	// Token: 0x020005E0 RID: 1504
	public class MoveManager : IXmlSerializable
	{
		// Token: 0x1400013A RID: 314
		// (add) Token: 0x06002FAE RID: 12206 RVA: 0x0012326C File Offset: 0x0012146C
		// (remove) Token: 0x06002FAF RID: 12207 RVA: 0x001232A4 File Offset: 0x001214A4
		public event MoveManager.UnitMoveDelegate UnitMoved;

		// Token: 0x06002FB0 RID: 12208 RVA: 0x00045B3D File Offset: 0x00043D3D
		public MoveManager(GameManager gameManager)
		{
			this.gameManager = gameManager;
		}

		// Token: 0x06002FB1 RID: 12209 RVA: 0x00045B57 File Offset: 0x00043D57
		public void SetMoveAction(GainMove action)
		{
			this.gameManager.combatManager.OnCombatStageChanged += this.OnCombatResolved;
			this.action = action;
			action.SelectAction();
			this.gameManager.tokenManager.AttachListeners();
		}

		// Token: 0x06002FB2 RID: 12210 RVA: 0x001232DC File Offset: 0x001214DC
		public void Clear()
		{
			if (this.selectedUnit == this.movedUnit && this.movedUnit != null && this.movedUnit.MovesLeft != 0)
			{
				this.gameManager.actionLog.LogInfoReported(this.action.GetLogInfoForUnitMove(this.movedUnit));
			}
			this.action = null;
			this.selectedHex = null;
			this.selectedUnit = (this.movedUnit = null);
			this.possibleMoves.Clear();
			this.gameManager.combatManager.OnCombatStageChanged -= this.OnCombatResolved;
		}

		// Token: 0x06002FB3 RID: 12211 RVA: 0x00045B92 File Offset: 0x00043D92
		public bool IsPlayerMoving(Player player)
		{
			return this.action != null && this.action.GetPlayer() == player;
		}

		// Token: 0x06002FB4 RID: 12212 RVA: 0x00123374 File Offset: 0x00121574
		public List<Unit> GetUnitsWithMove()
		{
			List<Unit> list = new List<Unit>();
			Player player = this.action.GetPlayer();
			if (player.character.MovesLeft != 0)
			{
				list.Add(player.character);
			}
			foreach (Worker worker in player.matPlayer.workers)
			{
				if (worker.MovesLeft != 0)
				{
					list.Add(worker);
				}
			}
			foreach (Mech mech in player.matFaction.mechs)
			{
				if (mech.MovesLeft != 0)
				{
					list.Add(mech);
				}
			}
			return list;
		}

		// Token: 0x06002FB5 RID: 12213 RVA: 0x00045BAC File Offset: 0x00043DAC
		public Dictionary<GameHex, GameHex> GetPossibleMovesForSelectedUnit()
		{
			if (this.selectedUnit == null)
			{
				return new Dictionary<GameHex, GameHex>();
			}
			return this.gameManager.gameBoard.MoveRange(this.selectedUnit, (int)this.selectedUnit.MovesLeft);
		}

		// Token: 0x06002FB6 RID: 12214 RVA: 0x00045BDD File Offset: 0x00043DDD
		public Dictionary<GameHex, GameHex> GetPossibleMovesForSelectedUnit(Unit selectedUnit)
		{
			if (selectedUnit == null)
			{
				return new Dictionary<GameHex, GameHex>();
			}
			return this.gameManager.gameBoard.MoveRange(selectedUnit, (int)selectedUnit.MovesLeft);
		}

		// Token: 0x06002FB7 RID: 12215 RVA: 0x00045BFF File Offset: 0x00043DFF
		public bool CanSelectedUnitMove()
		{
			return this.action.MovesLeft > 1 || (this.action.MovesLeft == 1 && (this.movedUnit == null || this.movedUnit == this.selectedUnit));
		}

		// Token: 0x06002FB8 RID: 12216 RVA: 0x00045C39 File Offset: 0x00043E39
		public bool CanUnitMove(Unit unit)
		{
			return this.action.MovesLeft > 1 || (this.action.MovesLeft == 1 && (this.movedUnit == null || this.movedUnit == unit));
		}

		// Token: 0x06002FB9 RID: 12217 RVA: 0x00123454 File Offset: 0x00121654
		public Dictionary<GameHex, GameHex> SelectUnit(Unit unit)
		{
			if (this.movedUnit != null && this.movedUnit != unit)
			{
				this.UnloadEverything(this.movedUnit);
			}
			this.selectedUnit = unit;
			if (this.selectedUnit != null)
			{
				this.selectedHex = this.selectedUnit.position;
			}
			else
			{
				this.selectedHex = null;
			}
			this.possibleMoves = this.GetPossibleMovesForSelectedUnit();
			return this.possibleMoves;
		}

		// Token: 0x06002FBA RID: 12218 RVA: 0x00045C6E File Offset: 0x00043E6E
		public Unit GetSelectedUnit()
		{
			return this.selectedUnit;
		}

		// Token: 0x06002FBB RID: 12219 RVA: 0x00045C76 File Offset: 0x00043E76
		public Unit GetLastMovedUnit()
		{
			return this.movedUnit;
		}

		// Token: 0x06002FBC RID: 12220 RVA: 0x00045C7E File Offset: 0x00043E7E
		public GameHex GetLastSelectedGameHex()
		{
			return this.selectedHex;
		}

		// Token: 0x06002FBD RID: 12221 RVA: 0x001234BC File Offset: 0x001216BC
		public bool MoveSelectedUnit(GameHex moveToPosition, Dictionary<ResourceType, int> resources = null, List<Unit> loadedWorkers = null)
		{
			bool flag = false;
			this.selectedHex = moveToPosition;
			if (this.selectedUnit == null || this.possibleMoves == null || moveToPosition == null)
			{
				return false;
			}
			if (this.possibleMoves.ContainsKey(moveToPosition) && moveToPosition != this.selectedUnit.position && this.action.MovesLeft > 0)
			{
				int num = this.CalculateDistance(this.selectedUnit.position, moveToPosition);
				this.FinishPreviousMove(false);
				if (resources != null)
				{
					this.LoadResourcesToUnit(resources, this.selectedUnit);
				}
				if (loadedWorkers != null && this.selectedUnit is Mech)
				{
					this.LoadWorkersToMech(loadedWorkers, this.selectedUnit as Mech);
				}
				GameHex position = this.selectedUnit.position;
				flag = this.MoveSelectedUnitToPosition(moveToPosition, num);
				if (!flag)
				{
					this.UnloadEverything(this.selectedUnit);
				}
				else if (this.gameManager.IsMyTurn())
				{
					this.gameManager.OnActionSent(new MoveUnitMessage(moveToPosition, position, this.selectedUnit, num, resources, loadedWorkers));
				}
				this.possibleMoves = this.GetPossibleMovesForSelectedUnit();
			}
			return flag;
		}

		// Token: 0x06002FBE RID: 12222 RVA: 0x001235C4 File Offset: 0x001217C4
		public int CalculateDistance(GameHex from, GameHex to)
		{
			int num = 0;
			for (GameHex gameHex = to; gameHex != from; gameHex = this.possibleMoves[gameHex])
			{
				num++;
			}
			return num;
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x001235F0 File Offset: 0x001217F0
		public bool DoesUnitCannotMoveBecauseOfTheLakeCase()
		{
			return this.selectedUnit != null && this.possibleMoves != null && this.selectedHex != null && (this.possibleMoves.ContainsKey(this.selectedHex) && this.selectedHex != this.selectedUnit.position && this.selectedUnit.position.hexType == HexType.lake && this.selectedUnit.MovesLeft > 0 && (this.selectedUnit is Mech || this.selectedUnit is Worker));
		}

		// Token: 0x06002FC0 RID: 12224 RVA: 0x0012367C File Offset: 0x0012187C
		public bool DoesUnitCannotMoveBecauseOfTheLakeCaseClick()
		{
			return this.selectedUnit != null && (this.selectedUnit.position.hexType == HexType.lake && this.selectedUnit.MovesLeft > 0 && (this.selectedUnit is Mech || this.selectedUnit is Worker));
		}

		// Token: 0x06002FC1 RID: 12225 RVA: 0x001236D4 File Offset: 0x001218D4
		private void FinishPreviousMove(bool onTokenInteraction = false)
		{
			if (this.movedUnit != null && (this.movedUnit != this.selectedUnit || onTokenInteraction))
			{
				this.movedUnit.MovesLeft = 0;
				this.UnloadEverything(this.movedUnit);
				this.gameManager.actionLog.LogInfoReported(this.action.GetLogInfoForUnitMove(this.movedUnit));
				this.movedUnit = null;
				GainMove gainMove = this.action;
				gainMove.MovesLeft -= 1;
			}
		}

		// Token: 0x06002FC2 RID: 12226 RVA: 0x00123754 File Offset: 0x00121954
		private bool MoveSelectedUnitToPosition(GameHex moveToPosition, int distance)
		{
			Unit unit = this.selectedUnit;
			if (!this.action.MoveUnitToPosition(unit, moveToPosition, distance))
			{
				return false;
			}
			this.movedUnit = this.selectedUnit;
			this.selectedHex = moveToPosition;
			if (this.UnitMoved != null)
			{
				this.UnitMoved(this.movedUnit);
			}
			this.CheckMovesLeftCases(unit);
			return true;
		}

		// Token: 0x06002FC3 RID: 12227 RVA: 0x00045C86 File Offset: 0x00043E86
		private void CheckMovesLeftCases(Unit selectedUnitLogic)
		{
			if (selectedUnitLogic.MovesLeft == 0)
			{
				this.gameManager.actionLog.LogInfoReported(this.action.GetLogInfoForUnitMove(this.movedUnit));
				this.movedUnit = null;
				this.UnloadEverything(this.selectedUnit);
			}
		}

		// Token: 0x06002FC4 RID: 12228 RVA: 0x00045CC4 File Offset: 0x00043EC4
		private void CheckEndingTurnActivities()
		{
			if (this.gameManager.combatManager.GetBattlefields().Count == 0)
			{
				this.gameManager.CheckObjectiveCards();
			}
		}

		// Token: 0x06002FC5 RID: 12229 RVA: 0x00045CE8 File Offset: 0x00043EE8
		private void UnloadEverything(Unit unit)
		{
			this.UnloadAllResources(unit);
			if (unit is Mech)
			{
				this.UnloadAllWorkersFromMech(unit as Mech);
			}
		}

		// Token: 0x06002FC6 RID: 12230 RVA: 0x00045D06 File Offset: 0x00043F06
		public void UnitTokenInteraction()
		{
			if (this.movedUnit != null)
			{
				this.FinishPreviousMove(true);
			}
		}

		// Token: 0x06002FC7 RID: 12231 RVA: 0x00045D17 File Offset: 0x00043F17
		public void SelectedUnitArmedTrap()
		{
			if (this.movedUnit != null && this.movedUnit.MovesLeft > 0)
			{
				this.movedUnit.MovesLeft = 0;
				this.CheckMovesLeftCases(this.movedUnit);
			}
		}

		// Token: 0x06002FC8 RID: 12232 RVA: 0x00045D47 File Offset: 0x00043F47
		private void LoadResourcesToUnit(Dictionary<ResourceType, int> resources, Unit unit)
		{
			this.action.ExchangeResources(unit, resources[ResourceType.oil], resources[ResourceType.metal], resources[ResourceType.food], resources[ResourceType.wood]);
		}

		// Token: 0x06002FC9 RID: 12233 RVA: 0x00045D72 File Offset: 0x00043F72
		private void ExchangeResources(Unit unit, int oil, int metal, int food, int wood)
		{
			this.action.ExchangeResources(unit, oil, metal, food, wood);
		}

		// Token: 0x06002FCA RID: 12234 RVA: 0x00045D87 File Offset: 0x00043F87
		private bool UnloadAllResources(Unit unit)
		{
			if (unit.MovesLeft > 0 && this.gameManager.IsMyTurn())
			{
				this.gameManager.OnActionSent(new UnloadAllResourcesMessage());
			}
			return this.action.UnloadAllResources(unit);
		}

		// Token: 0x06002FCB RID: 12235 RVA: 0x001237B0 File Offset: 0x001219B0
		private void LoadWorkersToMech(List<Unit> workers, Mech mech)
		{
			foreach (Unit unit in workers)
			{
				Worker worker = (Worker)unit;
				this.action.LoadWorkerToMech(worker, mech);
			}
		}

		// Token: 0x06002FCC RID: 12236 RVA: 0x0012380C File Offset: 0x00121A0C
		public void UnloadAllWorkersFromMech(Mech mech)
		{
			while (mech.LoadedWorkers.Count > 0)
			{
				if (mech.MovesLeft > 0 && this.gameManager.IsMyTurn())
				{
					this.gameManager.OnActionSent(new UnloadWorkerMessage(mech.LoadedWorkers[0].position, mech.LoadedWorkers[0].Id));
				}
				this.action.UnloadWorkerFromMech(mech.LoadedWorkers[0], mech);
			}
		}

		// Token: 0x06002FCD RID: 12237 RVA: 0x0012388C File Offset: 0x00121A8C
		public bool UnloadWorkerFromSelectedMech(Unit worker)
		{
			if (!(this.selectedUnit is Mech))
			{
				return true;
			}
			Mech mech = this.selectedUnit as Mech;
			if (mech.LoadedWorkers.Contains(worker as Worker))
			{
				if (this.gameManager.IsMyTurn())
				{
					this.gameManager.OnActionSent(new UnloadWorkerMessage(worker.position, worker.Id));
				}
				return this.action.UnloadWorkerFromMech(worker, mech);
			}
			return true;
		}

		// Token: 0x06002FCE RID: 12238 RVA: 0x00045DBB File Offset: 0x00043FBB
		public bool UnloadResources()
		{
			return this.selectedUnit == null || this.UnloadAllResources(this.selectedUnit);
		}

		// Token: 0x06002FCF RID: 12239 RVA: 0x00123900 File Offset: 0x00121B00
		public bool DoesWorkersNeedToRetreat(GameHex hex)
		{
			if (this.gameManager.PlayerCurrent == hex.Enemy && hex.GetOwnerMechs().Count == 0 && hex.Owner.character.position != hex)
			{
				this.gameManager.combatManager.SelectBattlefield(hex);
				this.gameManager.combatManager.SwitchToNextStage();
				return true;
			}
			return false;
		}

		// Token: 0x06002FD0 RID: 12240 RVA: 0x00045DD3 File Offset: 0x00043FD3
		public bool CombatOnTheHex(GameHex hex)
		{
			return this.action.GetPlayer() == hex.Enemy && (hex.GetOwnerMechs().Count > 0 || hex.Owner.character.position == hex);
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x00045E0C File Offset: 0x0004400C
		public void OnCombatResolved(CombatStage stage)
		{
			if (stage == CombatStage.CombatResovled)
			{
				this.CheckEndingTurnActivities();
			}
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x00045E18 File Offset: 0x00044018
		public int GetMovesLeft()
		{
			if (this.action == null)
			{
				return 0;
			}
			return (int)this.action.MovesLeft;
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x00045E2F File Offset: 0x0004402F
		public GainMove GetActualAction()
		{
			return this.action;
		}

		// Token: 0x06002FD4 RID: 12244 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06002FD5 RID: 12245 RVA: 0x00123968 File Offset: 0x00121B68
		public void ReadXml(XmlReader reader)
		{
			if (reader.GetAttribute("NotUsed") != null)
			{
				return;
			}
			this.action = this.gameManager.actionManager.GetLastSelectedGainAction() as GainMove;
			if (this.action == null)
			{
				return;
			}
			this.SetMoveAction(this.action);
			if (reader.GetAttribute("X") != null)
			{
				int num = int.Parse(reader.GetAttribute("X"));
				int num2 = int.Parse(reader.GetAttribute("Y"));
				this.selectedHex = this.gameManager.gameBoard.hexMap[num, num2];
			}
			reader.ReadStartElement();
			if (reader.Name == "SelectedUnit")
			{
				int num3 = int.Parse(reader.GetAttribute("X"));
				int num4 = int.Parse(reader.GetAttribute("Y"));
				int num5 = int.Parse(reader.GetAttribute("Id"));
				UnitType unitType = (UnitType)int.Parse(reader.GetAttribute("Type"));
				this.selectedUnit = this.GetUnit(unitType, num3, num4, num5);
				reader.ReadStartElement();
			}
			if (reader.Name == "MovedUnit")
			{
				int num6 = int.Parse(reader.GetAttribute("X"));
				int num7 = int.Parse(reader.GetAttribute("Y"));
				int num8 = int.Parse(reader.GetAttribute("Id"));
				UnitType unitType2 = (UnitType)int.Parse(reader.GetAttribute("Type"));
				this.movedUnit = this.GetUnit(unitType2, num6, num7, num8);
				reader.ReadStartElement();
			}
			this.possibleMoves = this.GetPossibleMovesForSelectedUnit();
		}

		// Token: 0x06002FD6 RID: 12246 RVA: 0x00123AF8 File Offset: 0x00121CF8
		public void WriteXml(XmlWriter writer)
		{
			if (this.action == null)
			{
				writer.WriteAttributeString("NotUsed", "");
				return;
			}
			if (this.selectedHex != null)
			{
				writer.WriteAttributeString("X", this.selectedHex.posX.ToString());
				writer.WriteAttributeString("Y", this.selectedHex.posY.ToString());
			}
			if (this.selectedUnit != null)
			{
				writer.WriteStartElement("SelectedUnit");
				writer.WriteAttributeString("X", this.selectedUnit.position.posX.ToString());
				writer.WriteAttributeString("Y", this.selectedUnit.position.posY.ToString());
				writer.WriteAttributeString("Id", this.selectedUnit.Id.ToString());
				writer.WriteAttributeString("Type", ((int)this.selectedUnit.UnitType).ToString());
				writer.WriteEndElement();
			}
			if (this.movedUnit != null)
			{
				writer.WriteStartElement("MovedUnit");
				writer.WriteAttributeString("X", this.movedUnit.position.posX.ToString());
				writer.WriteAttributeString("Y", this.movedUnit.position.posY.ToString());
				writer.WriteAttributeString("Id", this.movedUnit.Id.ToString());
				writer.WriteAttributeString("Type", ((int)this.movedUnit.UnitType).ToString());
				writer.WriteEndElement();
			}
		}

		// Token: 0x06002FD7 RID: 12247 RVA: 0x00123C8C File Offset: 0x00121E8C
		public Unit GetUnit(UnitType unitType, int x, int y, int unitId)
		{
			switch (unitType)
			{
			case UnitType.Character:
				return this.gameManager.PlayerCurrent.character;
			case UnitType.Mech:
			{
				using (List<Mech>.Enumerator enumerator = this.gameManager.PlayerCurrent.matFaction.mechs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mech mech = enumerator.Current;
						if (mech.position.posX == x && mech.position.posY == y && mech.Id == unitId)
						{
							return mech;
						}
					}
					goto IL_0100;
				}
				break;
			}
			case UnitType.Worker:
				break;
			default:
				goto IL_0100;
			}
			foreach (Worker worker in this.gameManager.PlayerCurrent.matPlayer.workers)
			{
				if (worker.position.posX == x && worker.position.posY == y && worker.Id == unitId)
				{
					return worker;
				}
			}
			IL_0100:
			return null;
		}

		// Token: 0x06002FD8 RID: 12248 RVA: 0x00123DB8 File Offset: 0x00121FB8
		public Unit GetUnit(UnitType unitType, int x, int y, int unitId, Player player)
		{
			switch (unitType)
			{
			case UnitType.Character:
				return player.character;
			case UnitType.Mech:
			{
				using (List<Mech>.Enumerator enumerator = player.matFaction.mechs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mech mech = enumerator.Current;
						if (mech.position.posX == x && mech.position.posY == y && mech.Id == unitId)
						{
							return mech;
						}
					}
					goto IL_00E2;
				}
				break;
			}
			case UnitType.Worker:
				break;
			default:
				goto IL_00E2;
			}
			foreach (Worker worker in player.matPlayer.workers)
			{
				if (worker.position.posX == x && worker.position.posY == y && worker.Id == unitId)
				{
					return worker;
				}
			}
			IL_00E2:
			return null;
		}

		// Token: 0x04002085 RID: 8325
		private GainMove action;

		// Token: 0x04002086 RID: 8326
		private GameHex selectedHex;

		// Token: 0x04002087 RID: 8327
		private Unit selectedUnit;

		// Token: 0x04002088 RID: 8328
		private Unit movedUnit;

		// Token: 0x04002089 RID: 8329
		private Dictionary<GameHex, GameHex> possibleMoves = new Dictionary<GameHex, GameHex>();

		// Token: 0x0400208B RID: 8331
		private GameManager gameManager;

		// Token: 0x020005E1 RID: 1505
		// (Invoke) Token: 0x06002FDA RID: 12250
		public delegate void UnitMoveDelegate(Unit unit);
	}
}
