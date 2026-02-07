using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x02000609 RID: 1545
	public abstract class BaseAction : IXmlSerializable
	{
		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06003082 RID: 12418 RVA: 0x0004663C File Offset: 0x0004483C
		// (set) Token: 0x06003083 RID: 12419 RVA: 0x00046644 File Offset: 0x00044844
		public short Amount { get; protected set; }

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06003084 RID: 12420 RVA: 0x0004664D File Offset: 0x0004484D
		// (set) Token: 0x06003085 RID: 12421 RVA: 0x00046655 File Offset: 0x00044855
		public ActionType ActionType { get; protected set; }

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06003086 RID: 12422 RVA: 0x0004665E File Offset: 0x0004485E
		// (set) Token: 0x06003087 RID: 12423 RVA: 0x00046666 File Offset: 0x00044866
		public short upgradeLevel { get; protected set; }

		// Token: 0x06003088 RID: 12424 RVA: 0x0004666F File Offset: 0x0004486F
		public BaseAction(short amount, short maxUpgradeLevel = 0)
		{
			this.Amount = amount;
			this.upgradeLevel = 0;
			this.maxUpgradeLevel = maxUpgradeLevel;
		}

		// Token: 0x06003089 RID: 12425 RVA: 0x0004668C File Offset: 0x0004488C
		public BaseAction(GameManager gameManager, short amount, short maxUpgradeLevel = 0)
		{
			this.Amount = amount;
			this.gameManager = gameManager;
			this.upgradeLevel = 0;
			this.maxUpgradeLevel = maxUpgradeLevel;
		}

		// Token: 0x0600308A RID: 12426 RVA: 0x000466B0 File Offset: 0x000448B0
		public virtual void SetPlayer(Player player)
		{
			this.player = player;
		}

		// Token: 0x0600308B RID: 12427 RVA: 0x000466B9 File Offset: 0x000448B9
		public virtual Player GetPlayer()
		{
			return this.player;
		}

		// Token: 0x0600308C RID: 12428
		public abstract bool CanExecute();

		// Token: 0x0600308D RID: 12429
		public abstract void Execute();

		// Token: 0x0600308E RID: 12430
		public abstract void Upgrade();

		// Token: 0x0600308F RID: 12431 RVA: 0x000466C1 File Offset: 0x000448C1
		public short GetUpgradeLevel()
		{
			return this.upgradeLevel;
		}

		// Token: 0x06003090 RID: 12432 RVA: 0x000466C9 File Offset: 0x000448C9
		public short GetMaxUpgradeLevel()
		{
			return this.maxUpgradeLevel;
		}

		// Token: 0x06003091 RID: 12433 RVA: 0x000466D1 File Offset: 0x000448D1
		public bool CanUpgrade()
		{
			return this.upgradeLevel != this.maxUpgradeLevel;
		}

		// Token: 0x06003092 RID: 12434
		public abstract void Clear();

		// Token: 0x06003093 RID: 12435 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06003094 RID: 12436 RVA: 0x001272C4 File Offset: 0x001254C4
		public virtual void ReadXml(XmlReader reader)
		{
			reader.MoveToContent();
			this.Amount = short.Parse(reader.GetAttribute("Amount"));
			this.LogInfoType = (LogInfoType)int.Parse(reader.GetAttribute("LogInfoType"));
			if (reader.GetAttribute("Upgrade") != null)
			{
				this.upgradeLevel = short.Parse(reader.GetAttribute("Upgrade"));
			}
			if (reader.GetAttribute("MaxUpgrade") != null)
			{
				this.maxUpgradeLevel = short.Parse(reader.GetAttribute("MaxUpgrade"));
			}
			if (this.gameManager.GetPlayerByFaction((Faction)int.Parse(reader.GetAttribute("Faction"))) != null)
			{
				this.player = this.gameManager.GetPlayerByFaction((Faction)int.Parse(reader.GetAttribute("Faction")));
			}
		}

		// Token: 0x06003095 RID: 12437 RVA: 0x00127388 File Offset: 0x00125588
		public virtual void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("Amount", ((int)this.Amount).ToString());
			string text = "LogInfoType";
			int num = (int)this.LogInfoType;
			writer.WriteAttributeString(text, num.ToString());
			if (this.upgradeLevel != 0)
			{
				writer.WriteAttributeString("Upgrade", this.upgradeLevel.ToString());
			}
			if (this.maxUpgradeLevel != 0)
			{
				writer.WriteAttributeString("MaxUpgrade", this.maxUpgradeLevel.ToString());
			}
			if (this.player != null)
			{
				string text2 = "Faction";
				num = (int)this.player.matFaction.faction;
				writer.WriteAttributeString(text2, num.ToString());
			}
		}

		// Token: 0x04002131 RID: 8497
		public LogInfoType LogInfoType;

		// Token: 0x04002132 RID: 8498
		protected Player player;

		// Token: 0x04002133 RID: 8499
		protected GameManager gameManager;

		// Token: 0x04002135 RID: 8501
		protected short maxUpgradeLevel;
	}
}
