using System;
using System.Xml;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x0200060F RID: 1551
	public class GainBuilding : GainAction
	{
		// Token: 0x1700038E RID: 910
		// (get) Token: 0x060030D3 RID: 12499 RVA: 0x00046929 File Offset: 0x00044B29
		// (set) Token: 0x060030D4 RID: 12500 RVA: 0x00046931 File Offset: 0x00044B31
		public Building Structure { get; private set; }

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x060030D5 RID: 12501 RVA: 0x0004693A File Offset: 0x00044B3A
		// (set) Token: 0x060030D6 RID: 12502 RVA: 0x00046942 File Offset: 0x00044B42
		public GameHex DeployField { get; private set; }

		// Token: 0x060030D7 RID: 12503 RVA: 0x0004694B File Offset: 0x00044B4B
		public GainBuilding()
			: base(0, 0, false, false, false)
		{
			this.gainType = GainType.Building;
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x00046960 File Offset: 0x00044B60
		public GainBuilding(GameManager gameManager)
			: base(gameManager, 0, 0, false, false, false)
		{
			this.gainType = GainType.Building;
		}

		// Token: 0x060030D9 RID: 12505 RVA: 0x00046976 File Offset: 0x00044B76
		public GainBuilding(GameManager gameManager, short amount, short maxLevelUpgrade = 0, bool isEncounter = false)
			: base(gameManager, amount, maxLevelUpgrade, isEncounter, false, false)
		{
			this.gainType = GainType.Building;
			this.Structure = null;
			this.DeployField = null;
		}

		// Token: 0x060030DA RID: 12506 RVA: 0x0004699B File Offset: 0x00044B9B
		public bool SetStructureAndLocation(Building structure, GameHex deployField)
		{
			if (!this.CheckLogic(structure, deployField))
			{
				return false;
			}
			this.Structure = structure;
			this.DeployField = deployField;
			base.ActionSelected = true;
			return true;
		}

		// Token: 0x060030DB RID: 12507 RVA: 0x000469BF File Offset: 0x00044BBF
		public override bool CanExecute()
		{
			return this.CheckLogic(this.Structure, this.DeployField);
		}

		// Token: 0x060030DC RID: 12508 RVA: 0x0012851C File Offset: 0x0012671C
		private bool CheckLogic(Building building, GameHex field)
		{
			if (!this.GainAvaliable())
			{
				return false;
			}
			if (building == null || field == null)
			{
				return false;
			}
			if (building.IsOnMap())
			{
				return false;
			}
			if (!this.player.OwnedFields(false).Contains(field))
			{
				return false;
			}
			if (field.Building != null)
			{
				return false;
			}
			if (field.hexType == HexType.lake)
			{
				return false;
			}
			if (base.IsEncounter)
			{
				if (this.player.character.position != field)
				{
					return false;
				}
			}
			else if (field.GetOwnerWorkers().Count <= 0)
			{
				return false;
			}
			return true;
		}

		// Token: 0x060030DD RID: 12509 RVA: 0x001285A0 File Offset: 0x001267A0
		public override bool GainAvaliable()
		{
			if (this.player.matPlayer.buildings.Count != 4)
			{
				if (base.IsEncounter && this.player.character.position.Building == null)
				{
					return true;
				}
				if (!base.IsEncounter)
				{
					foreach (Worker worker in this.player.matPlayer.workers)
					{
						if (worker.position.Building == null && worker.position.hexType != HexType.capital && worker.position.hexType != HexType.lake)
						{
							return true;
						}
					}
					return false;
				}
			}
			return false;
		}

		// Token: 0x060030DE RID: 12510 RVA: 0x000469D3 File Offset: 0x00044BD3
		public override bool IsMaxReached()
		{
			return this.player.matPlayer.buildings.Count == 4;
		}

		// Token: 0x060030DF RID: 12511 RVA: 0x0012866C File Offset: 0x0012686C
		public override LogInfo GetLogInfo()
		{
			return new BuildLogInfo(this.gameManager)
			{
				Type = LogInfoType.Build,
				IsEncounter = base.IsEncounter,
				PlayerAssigned = this.player.matFaction.faction,
				PlacedBuilding = this.Structure,
				Position = this.DeployField
			};
		}

		// Token: 0x060030E0 RID: 12512 RVA: 0x001286C8 File Offset: 0x001268C8
		public override void Execute()
		{
			if (this.Structure.buildingType == BuildingType.Mill)
			{
				this.Structure.GetBonus(this.player);
			}
			this.Structure.position = this.DeployField;
			this.Structure.player = this.player;
			this.DeployField.Building = this.Structure;
			this.player.matPlayer.buildings.Add(this.Structure);
			base.Gained = true;
			if (this.gameManager.IsMultiplayer && ((this.gameManager.IsMyTurn() && this.player == this.gameManager.PlayerOwner) || (this.gameManager.PlayerOwner == null && !this.player.IsHuman)))
			{
				this.SendAction();
			}
			if ((this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null && !this.gameManager.IsMyTurn()) || (!this.gameManager.IsMultiplayer && !this.gameManager.PlayerCurrent.IsHuman))
			{
				this.Structure.enemySpawnAnimation = true;
				BuildEnemyActionInfo buildEnemyActionInfo = new BuildEnemyActionInfo();
				buildEnemyActionInfo.fromEncounter = base.IsEncounter;
				buildEnemyActionInfo.buildingHex = this.DeployField;
				buildEnemyActionInfo.building = this.Structure;
				buildEnemyActionInfo.actionOwner = this.player.matFaction.faction;
				buildEnemyActionInfo.actionType = LogInfoType.Build;
				this.gameManager.EnemyBuild(buildEnemyActionInfo);
			}
			this.player.matPlayer.CheckBuildingStar();
		}

		// Token: 0x060030E1 RID: 12513 RVA: 0x000469ED File Offset: 0x00044BED
		public override void Clear()
		{
			base.Gained = false;
			this.Structure = null;
			this.DeployField = null;
			base.ActionSelected = false;
		}

		// Token: 0x060030E2 RID: 12514 RVA: 0x00128850 File Offset: 0x00126A50
		private void SendAction()
		{
			int num = -1;
			for (int i = 0; i < 4; i++)
			{
				if (this.player.matPlayer.GetPlayerMatSection(i).ActionTop.Structure.buildingType == this.Structure.buildingType)
				{
					num = i;
					break;
				}
			}
			this.gameManager.OnActionSent(new GainBuildingMessage(this.DeployField, num, base.IsEncounter));
		}

		// Token: 0x060030E3 RID: 12515 RVA: 0x001288BC File Offset: 0x00126ABC
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			if (reader.GetAttribute("Index") == null)
			{
				return;
			}
			int num = int.Parse(reader.GetAttribute("Index"));
			this.Structure = this.player.matPlayer.GetPlayerMatSection(num).ActionTop.Structure;
			if (reader.GetAttribute("X") == null)
			{
				return;
			}
			int num2 = int.Parse(reader.GetAttribute("X"));
			int num3 = int.Parse(reader.GetAttribute("Y"));
			this.DeployField = this.gameManager.gameBoard.hexMap[num2, num3];
			if (reader.GetAttribute("Enc") != null)
			{
				base.IsEncounter = true;
			}
		}

		// Token: 0x060030E4 RID: 12516 RVA: 0x00128974 File Offset: 0x00126B74
		public override void WriteXml(XmlWriter writer)
		{
			base.WriteXml(writer);
			if (this.Structure == null)
			{
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				if (this.player.matPlayer.GetPlayerMatSection(i).ActionTop.Structure.buildingType == this.Structure.buildingType)
				{
					writer.WriteAttributeString("Index", i.ToString());
					break;
				}
			}
			if (this.DeployField == null)
			{
				return;
			}
			writer.WriteAttributeString("X", this.DeployField.posX.ToString());
			writer.WriteAttributeString("Y", this.DeployField.posY.ToString());
			if (base.IsEncounter)
			{
				writer.WriteAttributeString("Enc", "");
			}
		}
	}
}
