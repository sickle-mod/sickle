using System;
using System.Collections.Generic;
using System.Xml;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x02000622 RID: 1570
	public class GainResource : GainAction
	{
		// Token: 0x1700039F RID: 927
		// (get) Token: 0x060031B9 RID: 12729 RVA: 0x000474A3 File Offset: 0x000456A3
		// (set) Token: 0x060031BA RID: 12730 RVA: 0x000474AB File Offset: 0x000456AB
		public ResourceType ResourceToGain { get; private set; }

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x060031BB RID: 12731 RVA: 0x000474B4 File Offset: 0x000456B4
		// (set) Token: 0x060031BC RID: 12732 RVA: 0x000474BC File Offset: 0x000456BC
		public GameHex Position { get; private set; }

		// Token: 0x060031BD RID: 12733 RVA: 0x000474C5 File Offset: 0x000456C5
		public GainResource()
			: base(0, 0, false, false, false)
		{
			this.gainType = GainType.Resource;
			this.Position = null;
		}

		// Token: 0x060031BE RID: 12734 RVA: 0x000474E0 File Offset: 0x000456E0
		public GainResource(GameManager gameManager)
			: base(gameManager, 0, 0, false, false, false)
		{
			this.gainType = GainType.Resource;
			this.Position = null;
		}

		// Token: 0x060031BF RID: 12735 RVA: 0x000474FC File Offset: 0x000456FC
		public GainResource(GameManager gameManager, ResourceType resource, short amount, short maxLevelUpgrade = 0, bool isEncounter = false)
			: base(gameManager, amount, maxLevelUpgrade, isEncounter, false, false)
		{
			this.gainType = GainType.Resource;
			this.ResourceToGain = resource;
			this.Position = null;
		}

		// Token: 0x060031C0 RID: 12736 RVA: 0x0012CD90 File Offset: 0x0012AF90
		public bool SetDestinationAmount(GameHex position, short amount)
		{
			if (!this.CheckLogic(position))
			{
				return false;
			}
			if ((!this.gameManager.IsMultiplayer || this.gameManager.PlayerOwner == null || this.gameManager.IsMyTurn()) && !this.gameManager.IsMultiplayer)
			{
				bool isHuman = this.gameManager.PlayerCurrent.IsHuman;
			}
			this.Position = position;
			base.Amount = amount;
			base.ActionSelected = true;
			return true;
		}

		// Token: 0x060031C1 RID: 12737 RVA: 0x00047521 File Offset: 0x00045721
		public override bool CanExecute()
		{
			return this.CheckLogic(this.Position);
		}

		// Token: 0x060031C2 RID: 12738 RVA: 0x0012CE04 File Offset: 0x0012B004
		public override LogInfo GetLogInfo()
		{
			HexUnitResourceLogInfo hexUnitResourceLogInfo = new HexUnitResourceLogInfo(this.gameManager);
			hexUnitResourceLogInfo.Type = LogInfoType.TradeResources;
			hexUnitResourceLogInfo.IsEncounter = base.IsEncounter;
			hexUnitResourceLogInfo.PlayerAssigned = this.player.matFaction.faction;
			Dictionary<ResourceType, int> dictionary = new Dictionary<ResourceType, int>();
			dictionary.Add(this.ResourceToGain, (int)base.Amount);
			hexUnitResourceLogInfo.Resources.Add(dictionary);
			hexUnitResourceLogInfo.Hexes.Add(this.Position);
			return hexUnitResourceLogInfo;
		}

		// Token: 0x060031C3 RID: 12739 RVA: 0x0012CE7C File Offset: 0x0012B07C
		public override void Execute()
		{
			base.Gained = true;
			Dictionary<ResourceType, int> resources = this.Position.resources;
			ResourceType resourceToGain = this.ResourceToGain;
			resources[resourceToGain] += (int)base.Amount;
			if (this.gameManager.IsMultiplayer && ((this.gameManager.IsMyTurn() && this.player == this.gameManager.PlayerOwner) || (this.gameManager.PlayerOwner == null && !this.player.IsHuman)))
			{
				this.gameManager.OnActionSent(new GainResourceMessage((int)this.ResourceToGain, this.Position, base.Amount, base.IsEncounter));
			}
		}

		// Token: 0x060031C4 RID: 12740 RVA: 0x0004752F File Offset: 0x0004572F
		private bool CheckLogic(GameHex position)
		{
			return position != null && this.player.OwnedFields(false).Contains(position);
		}

		// Token: 0x060031C5 RID: 12741 RVA: 0x0004754D File Offset: 0x0004574D
		public override void Clear()
		{
			base.Gained = false;
			this.Position = null;
			base.ActionSelected = false;
		}

		// Token: 0x060031C6 RID: 12742 RVA: 0x0012CF28 File Offset: 0x0012B128
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			if (reader.GetAttribute("X") != null)
			{
				int num = int.Parse(reader.GetAttribute("X"));
				int num2 = int.Parse(reader.GetAttribute("Y"));
				this.Position = this.gameManager.gameBoard.hexMap[num, num2];
			}
			this.ResourceToGain = (ResourceType)int.Parse(reader.GetAttribute("ResType"));
		}

		// Token: 0x060031C7 RID: 12743 RVA: 0x0012CFA0 File Offset: 0x0012B1A0
		public override void WriteXml(XmlWriter writer)
		{
			base.WriteXml(writer);
			if (this.Position != null)
			{
				writer.WriteAttributeString("X", this.Position.posX.ToString());
				writer.WriteAttributeString("Y", this.Position.posY.ToString());
			}
			writer.WriteAttributeString("ResType", ((int)this.ResourceToGain).ToString());
		}
	}
}
