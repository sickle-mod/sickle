using System;
using System.Collections.Generic;
using System.Xml;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x02000624 RID: 1572
	public class GainWorker : GainAction
	{
		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x060031DA RID: 12762 RVA: 0x00047674 File Offset: 0x00045874
		// (set) Token: 0x060031DB RID: 12763 RVA: 0x0004767C File Offset: 0x0004587C
		public GameHex DeployPosition { get; private set; }

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x060031DC RID: 12764 RVA: 0x00047685 File Offset: 0x00045885
		// (set) Token: 0x060031DD RID: 12765 RVA: 0x0004768D File Offset: 0x0004588D
		public int WorkersAmount { get; private set; }

		// Token: 0x060031DE RID: 12766 RVA: 0x00047696 File Offset: 0x00045896
		public GainWorker()
			: base(0, 0, false, false, false)
		{
			this.gainType = GainType.Worker;
		}

		// Token: 0x060031DF RID: 12767 RVA: 0x000476AB File Offset: 0x000458AB
		public GainWorker(GameManager gameManager)
			: base(gameManager, 0, 0, false, false, false)
		{
			this.gainType = GainType.Worker;
		}

		// Token: 0x060031E0 RID: 12768 RVA: 0x000476C1 File Offset: 0x000458C1
		public GainWorker(GameManager gameManager, short amount, short maxLevelUpgrade = 0, bool isEncounter = false)
			: base(gameManager, amount, maxLevelUpgrade, isEncounter, false, false)
		{
			this.gainType = GainType.Worker;
			this.DeployPosition = null;
			this.WorkersAmount = 1;
		}

		// Token: 0x060031E1 RID: 12769 RVA: 0x000476E6 File Offset: 0x000458E6
		public override void SetPlayer(Player player)
		{
			base.SetPlayer(player);
		}

		// Token: 0x060031E2 RID: 12770 RVA: 0x000476EF File Offset: 0x000458EF
		public bool SetLocationAndWorkersAmount(GameHex location, int workersAmount)
		{
			if (!this.CheckLogic(location, workersAmount))
			{
				return false;
			}
			this.DeployPosition = location;
			this.WorkersAmount = workersAmount;
			base.ActionSelected = true;
			return true;
		}

		// Token: 0x060031E3 RID: 12771 RVA: 0x00047713 File Offset: 0x00045913
		public override bool CanExecute()
		{
			return this.CheckLogic(this.DeployPosition, this.WorkersAmount);
		}

		// Token: 0x060031E4 RID: 12772 RVA: 0x0012D414 File Offset: 0x0012B614
		private bool CheckLogic(GameHex field, int workersAmount)
		{
			if (!this.GainAvaliable())
			{
				return false;
			}
			if (field == null)
			{
				return false;
			}
			if (workersAmount > (int)((short)(8 - this.player.matPlayer.workers.Count)))
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
			else if (this.player.currentMatSection == 4)
			{
				if (this.player.matPlayer.GetPlayerMatSection(4).CardId == 1)
				{
					if (field.GetOwnerWorkers().Count <= 0)
					{
						return false;
					}
				}
				else if (!this.AnyMechIsAvailableToSpawnWorker())
				{
					return false;
				}
			}
			else if (field.GetOwnerWorkers().Count <= 0 && (field.Building == null || field.Building.buildingType != BuildingType.Mill || field.Building.player != this.player))
			{
				return false;
			}
			return true;
		}

		// Token: 0x060031E5 RID: 12773 RVA: 0x0012D4E8 File Offset: 0x0012B6E8
		public override bool GainAvaliable()
		{
			if (this.player.matPlayer.GetPlayerMatSection(4) == null || this.player.matPlayer.GetPlayerMatSection(4).CardId != 14 || this.player.matPlayer.workers.Count >= 8 || this != this.player.matPlayer.GetPlayerMatSection(4).ActionTop.GetGainAction(0))
			{
				return 8 - this.player.matPlayer.workers.Count != 0;
			}
			if (this.player.matFaction.mechs.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < this.player.matFaction.mechs.Count; i++)
			{
				if (this.player.matFaction.mechs[i].position.hexType != HexType.capital)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060031E6 RID: 12774 RVA: 0x00047727 File Offset: 0x00045927
		public override bool IsMaxReached()
		{
			return this.player.matPlayer.workers.Count == 8;
		}

		// Token: 0x060031E7 RID: 12775 RVA: 0x0012D5E0 File Offset: 0x0012B7E0
		public override LogInfo GetLogInfo()
		{
			return new WorkerLogInfo(this.gameManager)
			{
				Type = LogInfoType.GainWorker,
				IsEncounter = base.IsEncounter,
				PlayerAssigned = this.player.matFaction.faction,
				Position = this.DeployPosition,
				WorkersAmount = this.WorkersAmount
			};
		}

		// Token: 0x060031E8 RID: 12776 RVA: 0x0012D63C File Offset: 0x0012B83C
		public override void Execute()
		{
			base.Gained = true;
			if ((this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null && !this.gameManager.IsMyTurn()) || (!this.gameManager.IsMultiplayer && !this.gameManager.PlayerCurrent.IsHuman && this.gameManager.showEnemyActions))
			{
				this.enemyActionInfo = new GainWorkerEnemyActionInfo();
				this.enemyActionInfo.actionOwner = this.player.matFaction.faction;
			}
			for (int i = 0; i < this.WorkersAmount; i++)
			{
				Worker worker = new Worker(this.gameManager, this.player, 1, -1);
				worker.spawnAnimation = true;
				worker.position = this.DeployPosition;
				this.player.matPlayer.workers.Add(worker);
				this.player.matPlayer.AddCostsToProduce();
				this.player.matPlayer.SetPlayer(this.player);
				if ((this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null && !this.gameManager.IsMyTurn()) || (!this.gameManager.IsMultiplayer && !this.gameManager.PlayerCurrent.IsHuman && this.gameManager.showEnemyActions))
				{
					worker.enemySpawnAnimation = true;
					this.enemyActionInfo.workers.Add(worker);
					this.gameManager.EnemyProduceWorker(this.enemyActionInfo);
				}
			}
			if (((this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null && !this.gameManager.IsMyTurn()) || (!this.gameManager.IsMultiplayer && !this.gameManager.PlayerCurrent.IsHuman && this.gameManager.showEnemyActions)) && this.WorkersAmount > 0)
			{
				GainWorkersEndEnemyActionInfo gainWorkersEndEnemyActionInfo = new GainWorkersEndEnemyActionInfo();
				gainWorkersEndEnemyActionInfo.actionOwner = this.player.matFaction.faction;
				gainWorkersEndEnemyActionInfo.deployPosition = this.DeployPosition;
				this.gameManager.EnemyProduceWorkersEnd(gainWorkersEndEnemyActionInfo);
			}
			if (this.gameManager.IsMultiplayer && ((this.gameManager.IsMyTurn() && this.player == this.gameManager.PlayerOwner) || (this.gameManager.PlayerOwner == null && !this.player.IsHuman)))
			{
				if (this.player.currentMatSection == 4 && !this.player.topActionFinished)
				{
					if ((this.player.matPlayer.GetPlayerMatSection(4) as FactoryCard).CardId == 14)
					{
						this.gameManager.OnActionSent(new GainWorkerMessage(this.DeployPosition, this.WorkersAmount, base.IsEncounter));
					}
				}
				else if (this.DeployPosition != null && this.DeployPosition.hasEncounter && this.player.character.position == this.DeployPosition && this.gameManager.LastEncounterCard != null)
				{
					this.gameManager.OnActionSent(new GainWorkerMessage(this.DeployPosition, this.WorkersAmount, base.IsEncounter));
				}
			}
			this.player.matPlayer.CheckWorkerStar();
		}

		// Token: 0x060031E9 RID: 12777 RVA: 0x00047741 File Offset: 0x00045941
		public override void Clear()
		{
			base.Gained = false;
			this.DeployPosition = null;
			this.WorkersAmount = 1;
			base.ActionSelected = false;
		}

		// Token: 0x060031EA RID: 12778 RVA: 0x0012D96C File Offset: 0x0012BB6C
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			if (reader.GetAttribute("X") != null)
			{
				int num = int.Parse(reader.GetAttribute("X"));
				int num2 = int.Parse(reader.GetAttribute("Y"));
				this.DeployPosition = this.gameManager.gameBoard.hexMap[num, num2];
			}
			this.WorkersAmount = int.Parse(reader.GetAttribute("Workers"));
			if (reader.GetAttribute("Enc") != null)
			{
				base.IsEncounter = true;
			}
		}

		// Token: 0x060031EB RID: 12779 RVA: 0x0012D9F8 File Offset: 0x0012BBF8
		public override void WriteXml(XmlWriter writer)
		{
			base.WriteXml(writer);
			if (this.DeployPosition != null)
			{
				writer.WriteAttributeString("X", this.DeployPosition.posX.ToString());
				writer.WriteAttributeString("Y", this.DeployPosition.posY.ToString());
			}
			writer.WriteAttributeString("Workers", this.WorkersAmount.ToString());
			if (base.IsEncounter)
			{
				writer.WriteAttributeString("Enc", "");
			}
		}

		// Token: 0x060031EC RID: 12780 RVA: 0x0012DA7C File Offset: 0x0012BC7C
		private bool AnyMechIsAvailableToSpawnWorker()
		{
			using (List<Mech>.Enumerator enumerator = this.player.matFaction.mechs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.position != this.player.GetCapital())
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0400218F RID: 8591
		private GainWorkerEnemyActionInfo enemyActionInfo;
	}
}
