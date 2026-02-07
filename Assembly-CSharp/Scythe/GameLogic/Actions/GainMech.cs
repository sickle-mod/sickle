using System;
using System.Xml;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x02000613 RID: 1555
	public class GainMech : GainAction
	{
		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06003100 RID: 12544 RVA: 0x00046BB7 File Offset: 0x00044DB7
		// (set) Token: 0x06003101 RID: 12545 RVA: 0x00046BBF File Offset: 0x00044DBF
		public Mech MechToDeploy { get; private set; }

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06003102 RID: 12546 RVA: 0x00046BC8 File Offset: 0x00044DC8
		// (set) Token: 0x06003103 RID: 12547 RVA: 0x00046BD0 File Offset: 0x00044DD0
		public GameHex DeployField { get; private set; }

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06003104 RID: 12548 RVA: 0x00046BD9 File Offset: 0x00044DD9
		// (set) Token: 0x06003105 RID: 12549 RVA: 0x00046BE1 File Offset: 0x00044DE1
		public int SkillIndex { get; private set; }

		// Token: 0x06003106 RID: 12550 RVA: 0x00046BEA File Offset: 0x00044DEA
		public GainMech()
			: base(0, 0, false, false, false)
		{
			this.gainType = GainType.Mech;
		}

		// Token: 0x06003107 RID: 12551 RVA: 0x00046BFF File Offset: 0x00044DFF
		public GainMech(GameManager gameManager)
			: base(gameManager, 0, 0, false, false, false)
		{
			this.gainType = GainType.Mech;
		}

		// Token: 0x06003108 RID: 12552 RVA: 0x00046C15 File Offset: 0x00044E15
		public GainMech(GameManager gameManager, short amount, short maxLevelUpgrade = 0, bool isEncounter = false)
			: base(gameManager, amount, maxLevelUpgrade, isEncounter, false, false)
		{
			this.gainType = GainType.Mech;
		}

		// Token: 0x06003109 RID: 12553 RVA: 0x00046C2C File Offset: 0x00044E2C
		public bool SetMechAndLocation(Mech mech, GameHex deployField, int skillIndex)
		{
			if (!this.CheckLogic(mech, deployField, skillIndex))
			{
				return false;
			}
			this.MechToDeploy = mech;
			this.DeployField = deployField;
			this.SkillIndex = skillIndex;
			base.ActionSelected = true;
			return true;
		}

		// Token: 0x0600310A RID: 12554 RVA: 0x00046C58 File Offset: 0x00044E58
		public override bool CanExecute()
		{
			return this.CheckLogic(this.MechToDeploy, this.DeployField, this.SkillIndex);
		}

		// Token: 0x0600310B RID: 12555 RVA: 0x00128E40 File Offset: 0x00127040
		private bool CheckLogic(Mech mech, GameHex deployField, int skillIndex)
		{
			if (!this.GainAvaliable())
			{
				return false;
			}
			if (mech == null || deployField == null)
			{
				return false;
			}
			if (deployField.hexType == HexType.lake)
			{
				return false;
			}
			if (mech.IsOnMap())
			{
				return false;
			}
			if (this.player.matFaction.SkillUnlocked[skillIndex])
			{
				return false;
			}
			if (base.IsEncounter)
			{
				if (this.player.character.position != deployField)
				{
					return false;
				}
			}
			else if (deployField.GetOwnerWorkers().Count <= 0)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600310C RID: 12556 RVA: 0x00128EBC File Offset: 0x001270BC
		public override bool GainAvaliable()
		{
			bool flag = false;
			foreach (Worker worker in this.player.matPlayer.workers)
			{
				if (worker.position.hexType != HexType.capital && worker.position.hexType != HexType.lake)
				{
					flag = true;
					break;
				}
			}
			return this.player.matFaction.mechs.Count != 4 && (flag || base.IsEncounter);
		}

		// Token: 0x0600310D RID: 12557 RVA: 0x00046C72 File Offset: 0x00044E72
		public override bool IsMaxReached()
		{
			return this.player.matFaction.mechs.Count == 4;
		}

		// Token: 0x0600310E RID: 12558 RVA: 0x00128F5C File Offset: 0x0012715C
		public override LogInfo GetLogInfo()
		{
			return new DeployLogInfo(this.gameManager)
			{
				Type = LogInfoType.Deploy,
				DeployedMech = this.MechToDeploy,
				PlayerAssigned = this.player.matFaction.faction,
				Position = this.DeployField,
				MechBonus = this.SkillIndex,
				IsEncounter = base.IsEncounter
			};
		}

		// Token: 0x0600310F RID: 12559 RVA: 0x00128FC4 File Offset: 0x001271C4
		public override void Execute()
		{
			base.Gained = true;
			this.MechToDeploy.position = this.DeployField;
			this.MechToDeploy.spawnAnimation = true;
			this.player.matFaction.mechs.Add(this.MechToDeploy);
			this.player.matFaction.SkillUnlocked[this.SkillIndex] = true;
			this.player.matFaction.CheckMechStar();
			if (this.SkillIndex == 3 && this.player.matFaction.faction != Faction.Albion && this.player.matFaction.faction != Faction.Togawa)
			{
				this.player.UpgradeMaxMoveCount();
			}
			if (this.gameManager.IsMultiplayer && ((this.gameManager.IsMyTurn() && this.player == this.gameManager.PlayerOwner) || (this.gameManager.PlayerOwner == null && !this.player.IsHuman)))
			{
				this.gameManager.OnActionSent(new GainMechMessage(this.DeployField, this.SkillIndex, base.IsEncounter));
			}
			if ((this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null && !this.gameManager.IsMyTurn()) || (!this.gameManager.IsMultiplayer && !this.gameManager.PlayerCurrent.IsHuman && this.gameManager.showEnemyActions))
			{
				this.MechToDeploy.enemySpawnAnimation = true;
				DeployEnemyActionInfo deployEnemyActionInfo = new DeployEnemyActionInfo();
				deployEnemyActionInfo.actionOwner = this.player.matFaction.faction;
				deployEnemyActionInfo.actionType = LogInfoType.Deploy;
				deployEnemyActionInfo.fromEncounter = base.IsEncounter;
				deployEnemyActionInfo.mechHex = this.DeployField;
				deployEnemyActionInfo.mechToDeploy = this.MechToDeploy;
				deployEnemyActionInfo.skillIndex = this.SkillIndex;
				deployEnemyActionInfo.allHexes = this.gameManager.PlayerCurrent.OwnedFields(false);
				foreach (Unit unit in this.gameManager.PlayerCurrent.GetAllUnits())
				{
					if (unit.UnitType != UnitType.Worker)
					{
						deployEnemyActionInfo.allPlayerBattleUnits.Add(unit);
					}
				}
				this.gameManager.EnemyDeploy(deployEnemyActionInfo);
			}
		}

		// Token: 0x06003110 RID: 12560 RVA: 0x00046C8C File Offset: 0x00044E8C
		public override void Clear()
		{
			base.Gained = false;
			this.MechToDeploy = null;
			this.DeployField = null;
			base.ActionSelected = false;
		}

		// Token: 0x06003111 RID: 12561 RVA: 0x00129218 File Offset: 0x00127418
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			if (reader.GetAttribute("X") == null)
			{
				return;
			}
			int num = int.Parse(reader.GetAttribute("X"));
			int num2 = int.Parse(reader.GetAttribute("Y"));
			this.DeployField = this.gameManager.gameBoard.hexMap[num, num2];
			this.MechToDeploy = new Mech(this.gameManager, this.player, 1);
			this.SkillIndex = int.Parse(reader.GetAttribute("Skill"));
			if (reader.GetAttribute("Enc") != null)
			{
				base.IsEncounter = true;
			}
		}

		// Token: 0x06003112 RID: 12562 RVA: 0x001292BC File Offset: 0x001274BC
		public override void WriteXml(XmlWriter writer)
		{
			base.WriteXml(writer);
			if (this.DeployField == null)
			{
				return;
			}
			writer.WriteAttributeString("X", this.DeployField.posX.ToString());
			writer.WriteAttributeString("Y", this.DeployField.posY.ToString());
			writer.WriteAttributeString("Skill", this.SkillIndex.ToString());
			if (base.IsEncounter)
			{
				writer.WriteAttributeString("Enc", "");
			}
		}
	}
}
