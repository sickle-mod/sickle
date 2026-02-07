using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x0200062E RID: 1582
	public class PayResource : PayAction
	{
		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06003225 RID: 12837 RVA: 0x00047A8D File Offset: 0x00045C8D
		// (set) Token: 0x06003226 RID: 12838 RVA: 0x00047A95 File Offset: 0x00045C95
		public List<ResourceBundle> ResourceBundles { get; private set; }

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06003227 RID: 12839 RVA: 0x00047A9E File Offset: 0x00045C9E
		// (set) Token: 0x06003228 RID: 12840 RVA: 0x00047AA6 File Offset: 0x00045CA6
		public ResourceType ResourceToPay { get; private set; }

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06003229 RID: 12841 RVA: 0x00047AAF File Offset: 0x00045CAF
		// (set) Token: 0x0600322A RID: 12842 RVA: 0x00047AB7 File Offset: 0x00045CB7
		public bool AnyResource { get; private set; }

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x0600322B RID: 12843 RVA: 0x00047AC0 File Offset: 0x00045CC0
		// (set) Token: 0x0600322C RID: 12844 RVA: 0x00047AC8 File Offset: 0x00045CC8
		public bool DifferentResources { get; private set; }

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x0600322D RID: 12845 RVA: 0x00047AD1 File Offset: 0x00045CD1
		// (set) Token: 0x0600322E RID: 12846 RVA: 0x00047AD9 File Offset: 0x00045CD9
		public CombatCard CardToPay { get; private set; }

		// Token: 0x0600322F RID: 12847 RVA: 0x00047AE2 File Offset: 0x00045CE2
		public PayResource()
			: base(0, 0, false, false)
		{
			this.payType = PayType.Resource;
			this.ResourceBundles = new List<ResourceBundle>();
		}

		// Token: 0x06003230 RID: 12848 RVA: 0x00047B00 File Offset: 0x00045D00
		public PayResource(GameManager gameManager)
			: base(gameManager, 0, 0, false, false)
		{
			this.payType = PayType.Resource;
			this.ResourceBundles = new List<ResourceBundle>();
		}

		// Token: 0x06003231 RID: 12849 RVA: 0x00047B1F File Offset: 0x00045D1F
		public PayResource(GameManager gameManager, ResourceType resource, short amount, short maxUpgradeLevel = 0, bool payed = false, bool isEncounter = false)
			: base(gameManager, amount, maxUpgradeLevel, payed, isEncounter)
		{
			this.payType = PayType.Resource;
			this.ResourceToPay = resource;
			this.AnyResource = false;
			this.ResourceBundles = new List<ResourceBundle>();
		}

		// Token: 0x06003232 RID: 12850 RVA: 0x00047B4F File Offset: 0x00045D4F
		public PayResource(GameManager gameManager, bool differentResources, short amount, short maxUpgradeLevel = 0, bool payed = false, bool isEncounter = false)
			: base(gameManager, amount, maxUpgradeLevel, payed, isEncounter)
		{
			this.payType = PayType.Resource;
			this.AnyResource = true;
			this.DifferentResources = differentResources;
			this.ResourceBundles = new List<ResourceBundle>();
		}

		// Token: 0x06003233 RID: 12851 RVA: 0x0012E0EC File Offset: 0x0012C2EC
		public override int GetMissingResourceCount()
		{
			int num = ((this.player.matFaction.factionPerk == AbilityPerk.Coercion && this.player.combatCards.Count > 0) ? 1 : 0);
			int num2;
			if (!this.DifferentResources)
			{
				if (!this.AnyResource)
				{
					num2 = this.player.Resources(false)[this.ResourceToPay];
				}
				else
				{
					num2 = this.player.Resources(false).Sum((KeyValuePair<ResourceType, int> r) => r.Value);
				}
			}
			else
			{
				num2 = this.player.Resources(false).Count((KeyValuePair<ResourceType, int> r) => r.Value > 0);
			}
			int num3 = num2;
			return Math.Max(0, (int)base.Amount - (num3 + num));
		}

		// Token: 0x06003234 RID: 12852 RVA: 0x00047B7F File Offset: 0x00045D7F
		public bool SetResources(List<ResourceBundle> resourceBundles, CombatCard card = null)
		{
			if (!this.CheckLogic(resourceBundles, card))
			{
				return false;
			}
			this.ResourceBundles = resourceBundles;
			this.CardToPay = card;
			return true;
		}

		// Token: 0x06003235 RID: 12853 RVA: 0x00047B9C File Offset: 0x00045D9C
		public override bool CanExecute()
		{
			return this.CheckLogic(this.ResourceBundles, this.CardToPay);
		}

		// Token: 0x06003236 RID: 12854 RVA: 0x00047BB0 File Offset: 0x00045DB0
		private bool CheckLogic(List<ResourceBundle> resourceBundles, CombatCard card = null)
		{
			if (this.AnyResource)
			{
				return this.CanExecuteWithAnyResources(resourceBundles, card);
			}
			return this.CanExecuteWithSameResources(resourceBundles, card);
		}

		// Token: 0x06003237 RID: 12855 RVA: 0x0012E1C4 File Offset: 0x0012C3C4
		private bool CanExecuteWithAnyResources(List<ResourceBundle> resourceBundles, CombatCard card)
		{
			HashSet<ResourceType> hashSet = new HashSet<ResourceType>();
			int num = 0;
			foreach (ResourceBundle resourceBundle in resourceBundles)
			{
				if (this.DifferentResources)
				{
					if (resourceBundle.amount != 1)
					{
						return false;
					}
					if (hashSet.Contains(resourceBundle.resourceType))
					{
						return false;
					}
					hashSet.Add(resourceBundle.resourceType);
				}
				if (resourceBundle.resourceType != ResourceType.combatCard)
				{
					if (!this.player.OwnedFields(false).Contains(resourceBundle.gameHex))
					{
						return false;
					}
					if (resourceBundle.gameHex.resources[resourceBundle.resourceType] < resourceBundle.amount)
					{
						return false;
					}
					num += resourceBundle.amount;
				}
				else
				{
					if (!this.gameManager.IsMultiplayer || this.gameManager.PlayerOwner == this.player)
					{
						if (card == null)
						{
							return false;
						}
						if (!this.player.combatCards.Contains(card))
						{
							return false;
						}
					}
					if (this.player.matFaction.factionPerk != AbilityPerk.Coercion)
					{
						return false;
					}
					num++;
				}
			}
			return num == (int)base.Amount;
		}

		// Token: 0x06003238 RID: 12856 RVA: 0x0012E328 File Offset: 0x0012C528
		private bool CanExecuteWithSameResources(List<ResourceBundle> resourceBundles, CombatCard card = null)
		{
			int num = 0;
			foreach (ResourceBundle resourceBundle in resourceBundles)
			{
				if (resourceBundle.resourceType != ResourceType.combatCard)
				{
					if (resourceBundle.resourceType != this.ResourceToPay)
					{
						return false;
					}
					if (!this.player.OwnedFields(false).Contains(resourceBundle.gameHex))
					{
						return false;
					}
					if (resourceBundle.gameHex.resources[this.ResourceToPay] < resourceBundle.amount)
					{
						return false;
					}
					num += resourceBundle.amount;
				}
				else
				{
					if (!this.gameManager.IsMultiplayer || this.gameManager.PlayerOwner == this.player || (!this.player.IsHuman && this.gameManager.PlayerOwner == null))
					{
						if (card == null)
						{
							return false;
						}
						if (!this.player.combatCards.Contains(card))
						{
							return false;
						}
					}
					if (this.player.matFaction.factionPerk != AbilityPerk.Coercion)
					{
						return false;
					}
					num++;
				}
			}
			return num == (int)base.Amount;
		}

		// Token: 0x06003239 RID: 12857 RVA: 0x0012E468 File Offset: 0x0012C668
		public override void Execute()
		{
			base.Payed = true;
			if (this.gameManager.IsMultiplayer && ((this.gameManager.IsMyTurn() && this.player == this.gameManager.PlayerOwner) || (this.gameManager.PlayerOwner == null && !this.player.IsHuman)))
			{
				this.gameManager.OnActionSent(new PayResourceMessage((this.CardToPay != null) ? this.CardToPay.CombatBonus : (-1), this.ResourceBundles));
			}
			foreach (ResourceBundle resourceBundle in this.ResourceBundles)
			{
				if (resourceBundle.resourceType == ResourceType.combatCard)
				{
					PayCombatCard payCombatCard = new PayCombatCard(this.gameManager, 1, 0, false, true);
					payCombatCard.SetPlayer(this.player);
					payCombatCard.SetCombatCards(new List<CombatCard> { this.CardToPay });
					payCombatCard.Execute();
					this.gameManager.SetAmountOfCombatCardsLeft();
				}
				else
				{
					Dictionary<ResourceType, int> resources = resourceBundle.gameHex.resources;
					ResourceType resourceType = resourceBundle.resourceType;
					resources[resourceType] -= resourceBundle.amount;
				}
				if ((this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null && !this.gameManager.IsMyTurn()) || (!this.gameManager.IsMultiplayer && !this.gameManager.PlayerCurrent.IsHuman))
				{
					EnemyPayResourceFromHexInfo enemyPayResourceFromHexInfo = new EnemyPayResourceFromHexInfo();
					enemyPayResourceFromHexInfo.actionType = this.LogInfoType;
					enemyPayResourceFromHexInfo.actionOwner = this.player.matFaction.faction;
					enemyPayResourceFromHexInfo.fromEncounter = base.IsEncounter;
					enemyPayResourceFromHexInfo.gameHex = resourceBundle.gameHex;
					resourceBundle.gameHex.skipDownActionPresentationUpdate = true;
					enemyPayResourceFromHexInfo.resourceType = resourceBundle.resourceType;
					enemyPayResourceFromHexInfo.amount = resourceBundle.amount;
					enemyPayResourceFromHexInfo.allHexes = this.gameManager.PlayerCurrent.OwnedFields(false);
					this.gameManager.EnemyPayResouces(enemyPayResourceFromHexInfo);
				}
			}
		}

		// Token: 0x0600323A RID: 12858 RVA: 0x00047BD5 File Offset: 0x00045DD5
		public override void Clear()
		{
			this.ResourceBundles.Clear();
			this.CardToPay = null;
			base.Clear();
		}

		// Token: 0x0600323B RID: 12859 RVA: 0x0012E698 File Offset: 0x0012C898
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			this.ResourceToPay = (ResourceType)int.Parse(reader.GetAttribute("ResToPay"));
			if (reader.GetAttribute("Diff") != null)
			{
				this.DifferentResources = true;
			}
			if (reader.GetAttribute("Any") != null)
			{
				this.AnyResource = true;
			}
			if (reader.GetAttribute("Card") != null)
			{
				this.CardToPay = this.player.combatCards.Find((CombatCard c) => c.CardId == int.Parse(reader.GetAttribute("Card")));
			}
			reader.ReadStartElement();
			while (reader.Name == "Bundle")
			{
				ResourceBundle resourceBundle = default(ResourceBundle);
				int num = int.Parse(reader.GetAttribute("X"));
				int num2 = int.Parse(reader.GetAttribute("Y"));
				resourceBundle.gameHex = this.gameManager.gameBoard.hexMap[num, num2];
				resourceBundle.resourceType = (ResourceType)int.Parse(reader.GetAttribute("Type"));
				resourceBundle.amount = (int)short.Parse(reader.GetAttribute("Amount"));
				this.ResourceBundles.Add(resourceBundle);
				reader.ReadStartElement();
				if (reader.Name != "Bundle" && reader.NodeType == XmlNodeType.EndElement)
				{
					reader.ReadEndElement();
				}
			}
		}

		// Token: 0x0600323C RID: 12860 RVA: 0x0012E83C File Offset: 0x0012CA3C
		public override void WriteXml(XmlWriter writer)
		{
			base.WriteXml(writer);
			writer.WriteAttributeString("ResToPay", ((int)this.ResourceToPay).ToString());
			if (this.DifferentResources)
			{
				writer.WriteAttributeString("Diff", "");
			}
			if (this.AnyResource)
			{
				writer.WriteAttributeString("Any", "");
			}
			if (this.CardToPay != null)
			{
				writer.WriteAttributeString("Card", this.CardToPay.CardId.ToString());
			}
			foreach (ResourceBundle resourceBundle in this.ResourceBundles)
			{
				writer.WriteStartElement("Bundle");
				writer.WriteAttributeString("X", resourceBundle.gameHex.posX.ToString());
				writer.WriteAttributeString("Y", resourceBundle.gameHex.posY.ToString());
				string text = "Type";
				int num = (int)resourceBundle.resourceType;
				writer.WriteAttributeString(text, num.ToString());
				string text2 = "Amount";
				num = resourceBundle.amount;
				writer.WriteAttributeString(text2, num.ToString());
				writer.WriteEndElement();
			}
		}

		// Token: 0x0600323D RID: 12861 RVA: 0x0012E980 File Offset: 0x0012CB80
		public override LogInfo GetLogInfo()
		{
			PayResourceLogInfo payResourceLogInfo = new PayResourceLogInfo(this.gameManager);
			payResourceLogInfo.Type = LogInfoType.PayResource;
			payResourceLogInfo.PlayerAssigned = this.player.matFaction.faction;
			payResourceLogInfo.IsEncounter = base.IsEncounter;
			foreach (ResourceBundle resourceBundle in this.ResourceBundles)
			{
				if (!payResourceLogInfo.Resources.ContainsKey(resourceBundle.resourceType))
				{
					payResourceLogInfo.Resources[resourceBundle.resourceType] = resourceBundle.amount;
				}
				else
				{
					Dictionary<ResourceType, int> resources = payResourceLogInfo.Resources;
					ResourceType resourceType = resourceBundle.resourceType;
					resources[resourceType] += resourceBundle.amount;
				}
			}
			return payResourceLogInfo;
		}
	}
}
