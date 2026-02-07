using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Scythe.GameLogic
{
	// Token: 0x020005B4 RID: 1460
	public class TokenManager : IXmlSerializable
	{
		// Token: 0x06002E81 RID: 11905 RVA: 0x00044FC6 File Offset: 0x000431C6
		public TokenManager(GameManager gameManager)
		{
			this.gameManager = gameManager;
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x0011B278 File Offset: 0x00119478
		public void AttachListeners()
		{
			this.gameManager.moveManager.UnitMoved += this.LogicUnitMoved;
			this.gameManager.combatManager.OnCombatStageChanged += this.CombatStageFinished;
			this.gameManager.EncounterClosed += this.EncounterClosed;
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x0011B2D4 File Offset: 0x001194D4
		public void RemoveListeners()
		{
			this.gameManager.moveManager.UnitMoved -= this.LogicUnitMoved;
			this.gameManager.combatManager.OnCombatStageChanged -= this.CombatStageFinished;
			this.gameManager.EncounterClosed -= this.EncounterClosed;
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x00044FD5 File Offset: 0x000431D5
		public void OnUndo()
		{
			this.RemoveListeners();
			if (this.lastMovedUnit != null)
			{
				this.AttachListeners();
			}
		}

		// Token: 0x06002E85 RID: 11909 RVA: 0x0011B330 File Offset: 0x00119530
		private void LogicUnitMoved(Unit unit)
		{
			this.characterMoved = (this.mechMoved = false);
			this.lastMovedUnit = unit;
			if (unit.UnitType == UnitType.Character)
			{
				this.characterMoved = true;
			}
			else if (unit.UnitType == UnitType.Mech)
			{
				this.mechMoved = true;
			}
			this.matFaction = unit.Owner.matFaction;
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x00044FEB File Offset: 0x000431EB
		private void CombatStageFinished(CombatStage combatStage)
		{
			if (!this.PlayerCurrentHasTokens())
			{
				return;
			}
			if (combatStage == CombatStage.DeterminatingTheWinner)
			{
				this.lastMovedUnit = this.gameManager.combatManager.GetSelectedBattlefield().GetCombatUnit(this.gameManager.combatManager.GetAttacker());
			}
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x00045025 File Offset: 0x00043225
		private void EncounterClosed()
		{
			if (!this.PlayerCurrentHasTokens())
			{
				return;
			}
			this.lastMovedUnit = this.matFaction.MatOwner.character;
			this.PlacingConditionDetected();
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x0004504C File Offset: 0x0004324C
		public bool CanPlaceTrap(GameHex gameHex, Unit unit)
		{
			return this.matFaction != null && this.matFaction.faction == Faction.Togawa && this.CanPlaceToken(gameHex, unit);
		}

		// Token: 0x06002E89 RID: 11913 RVA: 0x0004506E File Offset: 0x0004326E
		public bool CanPlaceFlag(GameHex gameHex, Unit unit)
		{
			return this.matFaction != null && this.matFaction.faction == Faction.Albion && this.CanPlaceToken(gameHex, unit);
		}

		// Token: 0x06002E8A RID: 11914 RVA: 0x0011B388 File Offset: 0x00119588
		private bool CanPlaceToken(GameHex gameHex, Unit unit)
		{
			return gameHex.Token == null && this.matFaction.FactionTokens.GetPlacedTokensCount() != 4 && unit.UnitType == UnitType.Character && (!unit.position.hasEncounter || (unit.position.hasEncounter && unit.position.encounterTaken));
		}

		// Token: 0x06002E8B RID: 11915 RVA: 0x0011B3E4 File Offset: 0x001195E4
		public bool CanRearmTrap(GameHex gameHex, Faction faction)
		{
			return gameHex.Token != null && gameHex.Token.Owner.matFaction.faction == faction && !(gameHex.Token as TrapToken).Armed && gameHex.Token.Owner.matFaction.SkillUnlocked[3] && gameHex.GetCombatUnit(gameHex.Token.Owner) != null;
		}

		// Token: 0x06002E8C RID: 11916 RVA: 0x0011B454 File Offset: 0x00119654
		public bool HexCondition(Unit unit)
		{
			GameHex position = unit.position;
			return !position.Conflict(unit) && (!position.hasEncounter || position.encounterUsed) && position.hexType != HexType.capital;
		}

		// Token: 0x06002E8D RID: 11917 RVA: 0x0011B490 File Offset: 0x00119690
		public bool MoveCondition(Unit unit)
		{
			return (this.characterMoved || this.mechMoved) && this.lastMovedUnit != null && this.lastMovedUnit.UnitType != UnitType.Worker && unit.UnitType != UnitType.Worker && this.HexCondition(this.lastMovedUnit) && (this.MoveToRearmCondition(this.lastMovedUnit) || this.MoveToPlaceToken(this.lastMovedUnit));
		}

		// Token: 0x06002E8E RID: 11918 RVA: 0x00045090 File Offset: 0x00043290
		private bool MoveToRearmCondition(Unit unit)
		{
			return unit != null && unit.UnitType != UnitType.Worker && unit.Owner.matFaction.faction == Faction.Togawa && unit.Owner.matFaction.SkillUnlocked[3];
		}

		// Token: 0x06002E8F RID: 11919 RVA: 0x000450C5 File Offset: 0x000432C5
		private bool MoveToPlaceToken(Unit unit)
		{
			return unit != null && unit.UnitType == UnitType.Character && (unit.Owner.matFaction.faction == Faction.Togawa || unit.Owner.matFaction.faction == Faction.Albion);
		}

		// Token: 0x06002E90 RID: 11920 RVA: 0x000450FC File Offset: 0x000432FC
		public bool SameAsLastMovedUnit(Unit unit)
		{
			return this.lastMovedUnit == unit;
		}

		// Token: 0x06002E91 RID: 11921 RVA: 0x00045107 File Offset: 0x00043307
		private void PlacingConditionDetected()
		{
			if (this.CanPlaceTokenInfo != null && this.gameManager.IsMyTurn())
			{
				this.CanPlaceTokenInfo(this.lastMovedUnit, true);
			}
		}

		// Token: 0x06002E92 RID: 11922 RVA: 0x0011B4F8 File Offset: 0x001196F8
		private bool PlayerCurrentHasTokens()
		{
			return this.gameManager.PlayerCurrent.matFaction.faction.Equals(Faction.Albion) || this.gameManager.PlayerCurrent.matFaction.faction.Equals(Faction.Togawa);
		}

		// Token: 0x06002E93 RID: 11923 RVA: 0x00045130 File Offset: 0x00043330
		public Unit GetLastMovedUnit()
		{
			return this.lastMovedUnit;
		}

		// Token: 0x06002E94 RID: 11924 RVA: 0x00045138 File Offset: 0x00043338
		public bool TokenAlreadyPlaced()
		{
			return this.tokenPlaced;
		}

		// Token: 0x06002E95 RID: 11925 RVA: 0x0011B558 File Offset: 0x00119758
		public FactionAbilityToken PlaceToken(GameHex gameHex, int tokenId)
		{
			FactionAbilityToken factionAbilityToken = null;
			if (this.PlayerCurrentHasTokens() && !this.TokenAlreadyPlaced())
			{
				factionAbilityToken = this.gameManager.PlayerCurrent.matFaction.FactionTokens.GetToken(tokenId);
				factionAbilityToken.PlaceToken(gameHex, false);
				this.CreateLogInfo(factionAbilityToken, this.lastMovedUnit, TokenActionType.Placed);
				this.TokenPlaced(factionAbilityToken);
			}
			return factionAbilityToken;
		}

		// Token: 0x06002E96 RID: 11926 RVA: 0x0011B5B4 File Offset: 0x001197B4
		public FactionAbilityToken PlaceToken(GameHex gameHex)
		{
			FactionAbilityToken factionAbilityToken = null;
			if (this.PlayerCurrentHasTokens() && !this.TokenAlreadyPlaced())
			{
				factionAbilityToken = this.gameManager.PlayerCurrent.matFaction.FactionTokens.GetFirstUnplacedToken();
				factionAbilityToken.PlaceToken(gameHex, false);
				this.CreateLogInfo(factionAbilityToken, this.lastMovedUnit, TokenActionType.Placed);
				this.TokenPlaced(factionAbilityToken);
			}
			return factionAbilityToken;
		}

		// Token: 0x06002E97 RID: 11927 RVA: 0x00045140 File Offset: 0x00043340
		public void TrapTriggered(TrapToken trapToken, Unit initiator)
		{
			this.CreateLogInfo(trapToken, initiator, TokenActionType.Triggered);
		}

		// Token: 0x06002E98 RID: 11928 RVA: 0x0011B60C File Offset: 0x0011980C
		private void CreateLogInfo(FactionAbilityToken placedToken, Unit lastMovedUnit, TokenActionType action)
		{
			TokenActionLogInfo tokenActionLogInfo = placedToken.CreateLogInfo(action, lastMovedUnit) as TokenActionLogInfo;
			this.gameManager.actionLog.LogInfoReported(tokenActionLogInfo);
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x0011B638 File Offset: 0x00119838
		public FactionAbilityToken ArmTrap(GameHex gameHex)
		{
			(gameHex.Token as TrapToken).OnTokenArmed();
			if (this.TrapWasArmed != null)
			{
				this.TrapWasArmed(gameHex.Token);
			}
			if (this.gameManager.moveManager.GetActualAction() != null)
			{
				this.gameManager.moveManager.UnitTokenInteraction();
			}
			this.CreateLogInfo(gameHex.Token, null, TokenActionType.Armed);
			return gameHex.Token;
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x0004514B File Offset: 0x0004334B
		private void TokenPlaced(FactionAbilityToken placedToken)
		{
			this.tokenPlaced = true;
			if (this.TokenWasPlaced != null)
			{
				this.TokenWasPlaced(placedToken);
			}
			if (this.gameManager.moveManager.GetActualAction() != null)
			{
				this.gameManager.moveManager.UnitTokenInteraction();
			}
		}

		// Token: 0x06002E9B RID: 11931 RVA: 0x0004518A File Offset: 0x0004338A
		public void Clear()
		{
			if (this.CanPlaceTokenInfo != null)
			{
				this.CanPlaceTokenInfo(this.lastMovedUnit, false);
			}
			this.lastMovedUnit = null;
			this.tokenPlaced = false;
			this.matFaction = null;
			this.characterMoved = false;
			this.mechMoved = false;
		}

		// Token: 0x06002E9C RID: 11932 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06002E9D RID: 11933 RVA: 0x0011B6A4 File Offset: 0x001198A4
		public void ReadXml(XmlReader reader)
		{
			if (reader.GetAttribute("TokenPlaced") != null)
			{
				this.tokenPlaced = true;
			}
			reader.ReadStartElement();
			if (reader.Name == "MovedUnit")
			{
				int num = int.Parse(reader.GetAttribute("X"));
				int num2 = int.Parse(reader.GetAttribute("Y"));
				int num3 = int.Parse(reader.GetAttribute("Id"));
				UnitType unitType = (UnitType)int.Parse(reader.GetAttribute("Type"));
				this.lastMovedUnit = this.gameManager.moveManager.GetUnit(unitType, num, num2, num3);
				this.matFaction = this.lastMovedUnit.Owner.matFaction;
				reader.ReadStartElement();
			}
		}

		// Token: 0x06002E9E RID: 11934 RVA: 0x0011B758 File Offset: 0x00119958
		public void WriteXml(XmlWriter writer)
		{
			if (this.tokenPlaced)
			{
				writer.WriteAttributeString("TokenPlaced", "");
			}
			if (this.lastMovedUnit != null)
			{
				writer.WriteStartElement("MovedUnit");
				writer.WriteAttributeString("X", this.lastMovedUnit.position.posX.ToString());
				writer.WriteAttributeString("Y", this.lastMovedUnit.position.posY.ToString());
				writer.WriteAttributeString("Id", this.lastMovedUnit.Id.ToString());
				writer.WriteAttributeString("Type", ((int)this.lastMovedUnit.UnitType).ToString());
				writer.WriteEndElement();
			}
		}

		// Token: 0x04001F5F RID: 8031
		private bool tokenPlaced;

		// Token: 0x04001F60 RID: 8032
		private MatFaction matFaction;

		// Token: 0x04001F61 RID: 8033
		private bool characterMoved;

		// Token: 0x04001F62 RID: 8034
		private bool mechMoved;

		// Token: 0x04001F63 RID: 8035
		private Unit lastMovedUnit;

		// Token: 0x04001F64 RID: 8036
		private GameManager gameManager;

		// Token: 0x04001F65 RID: 8037
		public TokenManager.CanUnitPlaceToken CanPlaceTokenInfo;

		// Token: 0x04001F66 RID: 8038
		public TokenManager.OnTokenPlaced TokenWasPlaced;

		// Token: 0x04001F67 RID: 8039
		public TokenManager.OnTrapTriggered TrapWasArmed;

		// Token: 0x020005B5 RID: 1461
		// (Invoke) Token: 0x06002EA0 RID: 11936
		public delegate void CanUnitPlaceToken(Unit unit, bool canPlace);

		// Token: 0x020005B6 RID: 1462
		// (Invoke) Token: 0x06002EA4 RID: 11940
		public delegate void OnTokenPlaced(FactionAbilityToken placedToken);

		// Token: 0x020005B7 RID: 1463
		// (Invoke) Token: 0x06002EA8 RID: 11944
		public delegate void OnTrapTriggered(FactionAbilityToken triggeredTrap);
	}
}
