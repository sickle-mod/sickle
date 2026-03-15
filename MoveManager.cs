using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Scythe.GameLogic.Actions;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic
{
	// Token: 0x020005EE RID: 1518
	public class MoveManager : IXmlSerializable
	{
		// Token: 0x1400013A RID: 314
		// (add) Token: 0x06003001 RID: 12289 RVA: 0x0012B394 File Offset: 0x00129594
		// (remove) Token: 0x06003002 RID: 12290 RVA: 0x0012B3CC File Offset: 0x001295CC
		public event MoveManager.UnitMoveDelegate UnitMoved;

		// Token: 0x06003003 RID: 12291 RVA: 0x00045B97 File Offset: 0x00043D97
		public MoveManager(GameManager gameManager)
		{
			this.gameManager = gameManager;
		}

		// Token: 0x06003004 RID: 12292 RVA: 0x00045BB1 File Offset: 0x00043DB1
		public void SetMoveAction(GainMove action)
		{
			this.gameManager.combatManager.OnCombatStageChanged += this.OnCombatResolved;
			this.action = action;
			action.SelectAction();
			this.gameManager.tokenManager.AttachListeners();
		}

		// Token: 0x06003005 RID: 12293 RVA: 0x0012B404 File Offset: 0x00129604
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

		// Token: 0x06003006 RID: 12294 RVA: 0x00045BEC File Offset: 0x00043DEC
		public bool IsPlayerMoving(Player player)
		{
			return this.action != null && this.action.GetPlayer() == player;
		}

		// Token: 0x06003007 RID: 12295 RVA: 0x0012B49C File Offset: 0x0012969C
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

		// Token: 0x06003008 RID: 12296 RVA: 0x00045C06 File Offset: 0x00043E06
		public Dictionary<GameHex, GameHex> GetPossibleMovesForSelectedUnit()
		{
			if (this.selectedUnit == null)
			{
				return new Dictionary<GameHex, GameHex>();
			}
			return this.gameManager.gameBoard.MoveRange(this.selectedUnit, (int)this.selectedUnit.MovesLeft);
		}

		// Token: 0x06003009 RID: 12297 RVA: 0x00045C37 File Offset: 0x00043E37
		public Dictionary<GameHex, GameHex> GetPossibleMovesForSelectedUnit(Unit selectedUnit)
		{
			if (selectedUnit == null)
			{
				return new Dictionary<GameHex, GameHex>();
			}
			return this.gameManager.gameBoard.MoveRange(selectedUnit, (int)selectedUnit.MovesLeft);
		}

		// Token: 0x0600300A RID: 12298 RVA: 0x00045C59 File Offset: 0x00043E59
		public bool CanSelectedUnitMove()
		{
			return this.action.MovesLeft > 1 || (this.action.MovesLeft == 1 && (this.movedUnit == null || this.movedUnit == this.selectedUnit));
		}

		// Token: 0x0600300B RID: 12299 RVA: 0x00045C93 File Offset: 0x00043E93
		public bool CanUnitMove(Unit unit)
		{
			return this.action.MovesLeft > 1 || (this.action.MovesLeft == 1 && (this.movedUnit == null || this.movedUnit == unit));
		}

		// Token: 0x0600300C RID: 12300 RVA: 0x0012B57C File Offset: 0x0012977C
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

		// Token: 0x0600300D RID: 12301 RVA: 0x00045CC8 File Offset: 0x00043EC8
		public Unit GetSelectedUnit()
		{
			return this.selectedUnit;
		}

		// Token: 0x0600300E RID: 12302 RVA: 0x00045CD0 File Offset: 0x00043ED0
		public Unit GetLastMovedUnit()
		{
			return this.movedUnit;
		}

		// Token: 0x0600300F RID: 12303 RVA: 0x00045CD8 File Offset: 0x00043ED8
		public GameHex GetLastSelectedGameHex()
		{
			return this.selectedHex;
		}

		// Token: 0x06003010 RID: 12304 RVA: 0x0012B5E4 File Offset: 0x001297E4
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

		// Token: 0x06003011 RID: 12305 RVA: 0x0012B6EC File Offset: 0x001298EC
		public int CalculateDistance(GameHex from, GameHex to)
		{
			int num = 0;
			GameHex gameHex = to;
			int maxDistance = 100; // Safety against cycles
			while (gameHex != from && num < maxDistance)
			{
				if (!this.possibleMoves.ContainsKey(gameHex))
				{
					return 999;
				}
				gameHex = this.possibleMoves[gameHex];
				num++;
			}
			return (gameHex == from) ? num : 999;
		}

		// Token: 0x06003012 RID: 12306 RVA: 0x0012B718 File Offset: 0x00129918
		public bool DoesUnitCannotMoveBecauseOfTheLakeCase()
		{
			return this.selectedUnit != null && this.possibleMoves != null && this.selectedHex != null && (this.possibleMoves.ContainsKey(this.selectedHex) && this.selectedHex != this.selectedUnit.position && this.selectedUnit.position.hexType == HexType.lake && this.selectedUnit.MovesLeft > 0 && (this.selectedUnit is Mech || this.selectedUnit is Worker));
		}

		// Token: 0x06003013 RID: 12307 RVA: 0x0012B7A4 File Offset: 0x001299A4
		public bool DoesUnitCannotMoveBecauseOfTheLakeCaseClick()
		{
			return this.selectedUnit != null && (this.selectedUnit.position.hexType == HexType.lake && this.selectedUnit.MovesLeft > 0 && (this.selectedUnit is Mech || this.selectedUnit is Worker));
		}

		// Token: 0x06003014 RID: 12308 RVA: 0x0012B7FC File Offset: 0x001299FC
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

		// Token: 0x06003015 RID: 12309 RVA: 0x0012B87C File Offset: 0x00129A7C
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

		// Token: 0x06003016 RID: 12310 RVA: 0x00045CE0 File Offset: 0x00043EE0
		private void CheckMovesLeftCases(Unit selectedUnitLogic)
		{
			if (selectedUnitLogic.MovesLeft == 0)
			{
				this.gameManager.actionLog.LogInfoReported(this.action.GetLogInfoForUnitMove(this.movedUnit));
				this.movedUnit = null;
				this.UnloadEverything(this.selectedUnit);
			}
		}

		// Token: 0x06003017 RID: 12311 RVA: 0x00045D1E File Offset: 0x00043F1E
		private void CheckEndingTurnActivities()
		{
			if (this.gameManager.combatManager.GetBattlefields().Count == 0)
			{
				this.gameManager.CheckObjectiveCards();
			}
		}

		// Token: 0x06003018 RID: 12312 RVA: 0x00045D42 File Offset: 0x00043F42
		private void UnloadEverything(Unit unit)
		{
			this.UnloadAllResources(unit);
			if (unit is Mech)
			{
				this.UnloadAllWorkersFromMech(unit as Mech);
			}
		}

		// Token: 0x06003019 RID: 12313 RVA: 0x00045D60 File Offset: 0x00043F60
		public void UnitTokenInteraction()
		{
			if (this.movedUnit != null)
			{
				this.FinishPreviousMove(true);
			}
		}

		// Token: 0x0600301A RID: 12314 RVA: 0x00045D71 File Offset: 0x00043F71
		public void SelectedUnitArmedTrap()
		{
			if (this.movedUnit != null && this.movedUnit.MovesLeft > 0)
			{
				this.movedUnit.MovesLeft = 0;
				this.CheckMovesLeftCases(this.movedUnit);
			}
		}

		// Token: 0x0600301B RID: 12315 RVA: 0x00045DA1 File Offset: 0x00043FA1
		private void LoadResourcesToUnit(Dictionary<ResourceType, int> resources, Unit unit)
		{
			this.action.ExchangeResources(unit, resources[ResourceType.oil], resources[ResourceType.metal], resources[ResourceType.food], resources[ResourceType.wood]);
		}

		// Token: 0x0600301C RID: 12316 RVA: 0x00045DCC File Offset: 0x00043FCC
		private void ExchangeResources(Unit unit, int oil, int metal, int food, int wood)
		{
			this.action.ExchangeResources(unit, oil, metal, food, wood);
		}

		// Token: 0x0600301D RID: 12317 RVA: 0x00045DE1 File Offset: 0x00043FE1
		private bool UnloadAllResources(Unit unit)
		{
			if (unit.MovesLeft > 0 && this.gameManager.IsMyTurn())
			{
				this.gameManager.OnActionSent(new UnloadAllResourcesMessage());
			}
			return this.action.UnloadAllResources(unit);
		}

		// Token: 0x0600301E RID: 12318 RVA: 0x0012B8D8 File Offset: 0x00129AD8
		private void LoadWorkersToMech(List<Unit> workers, Mech mech)
		{
			foreach (Unit unit in workers)
			{
				Worker worker = (Worker)unit;
				this.action.LoadWorkerToMech(worker, mech);
			}
		}

		// Token: 0x0600301F RID: 12319 RVA: 0x0012B934 File Offset: 0x00129B34
		public void UnloadAllWorkersFromMech(Mech mech)
		{
			if (mech == null || mech.LoadedWorkers == null) return;
			
			// Use a separate list to avoid modification during iteration issues, 
			// even if UnloadWorkerFromMech removes them synchronously.
			List<Worker> workersToUnload = new List<Worker>(mech.LoadedWorkers);
			foreach (Worker worker in workersToUnload)
			{
				if (mech.MovesLeft > 0 && this.gameManager.IsMyTurn())
				{
					this.gameManager.OnActionSent(new UnloadWorkerMessage(worker.position, worker.Id));
				}
				this.action.UnloadWorkerFromMech(worker, mech);
			}
		}

		// Token: 0x06003020 RID: 12320 RVA: 0x0012B9B4 File Offset: 0x00129BB4
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

		// Token: 0x06003021 RID: 12321 RVA: 0x00045E15 File Offset: 0x00044015
		public bool UnloadResources()
		{
			return this.selectedUnit == null || this.UnloadAllResources(this.selectedUnit);
		}

		// Token: 0x06003022 RID: 12322 RVA: 0x0012BA28 File Offset: 0x00129C28
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

		// Token: 0x06003023 RID: 12323 RVA: 0x00045E2D File Offset: 0x0004402D
		public bool CombatOnTheHex(GameHex hex)
		{
			return this.action.GetPlayer() == hex.Enemy && (hex.GetOwnerMechs().Count > 0 || hex.Owner.character.position == hex);
		}

		// Token: 0x06003024 RID: 12324 RVA: 0x00045E66 File Offset: 0x00044066
		public void OnCombatResolved(CombatStage stage)
		{
			if (stage == CombatStage.CombatResovled)
			{
				this.CheckEndingTurnActivities();
			}
		}

		// Token: 0x06003025 RID: 12325 RVA: 0x00045E72 File Offset: 0x00044072
		public int GetMovesLeft()
		{
			if (this.action == null)
			{
				return 0;
			}
			return (int)this.action.MovesLeft;
		}

		// Token: 0x06003026 RID: 12326 RVA: 0x00045E89 File Offset: 0x00044089
		public GainMove GetActualAction()
		{
			return this.action;
		}

		// Token: 0x06003027 RID: 12327 RVA: 0x0002F60E File Offset: 0x0002D80E
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06003028 RID: 12328 RVA: 0x0012BA90 File Offset: 0x00129C90
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

		// Token: 0x06003029 RID: 12329 RVA: 0x0012BC20 File Offset: 0x00129E20
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

		// Token: 0x0600302A RID: 12330 RVA: 0x0012BDB4 File Offset: 0x00129FB4
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

		// Token: 0x0600302B RID: 12331 RVA: 0x0012BEE0 File Offset: 0x0012A0E0
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

		// Token: 0x040020A7 RID: 8359
		private GainMove action;

		// Token: 0x040020A8 RID: 8360
		private GameHex selectedHex;

		// Token: 0x040020A9 RID: 8361
		private Unit selectedUnit;

		// Token: 0x040020AA RID: 8362
		private Unit movedUnit;

		// Token: 0x040020AB RID: 8363
		private Dictionary<GameHex, GameHex> possibleMoves = new Dictionary<GameHex, GameHex>();

		// Token: 0x040020AD RID: 8365
		private GameManager gameManager;

		// Token: 0x020005EF RID: 1519
		// (Invoke) Token: 0x0600302D RID: 12333
		public delegate void UnitMoveDelegate(Unit unit);
	}
}
