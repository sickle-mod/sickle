using System;
using System.Collections.Generic;
using System.Xml;
using Scythe.Multiplayer.Messages;

namespace Scythe.GameLogic.Actions
{
	// Token: 0x0200060E RID: 1550
	public class GainAnyResource : GainAction
	{
		// Token: 0x1700038B RID: 907
		// (get) Token: 0x060030BF RID: 12479 RVA: 0x0004681C File Offset: 0x00044A1C
		// (set) Token: 0x060030C0 RID: 12480 RVA: 0x00046824 File Offset: 0x00044A24
		public int ResourcesLeft { get; private set; }

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x060030C1 RID: 12481 RVA: 0x0004682D File Offset: 0x00044A2D
		// (set) Token: 0x060030C2 RID: 12482 RVA: 0x00046835 File Offset: 0x00044A35
		public List<Dictionary<ResourceType, int>> Trade { get; private set; }

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x060030C3 RID: 12483 RVA: 0x0004683E File Offset: 0x00044A3E
		// (set) Token: 0x060030C4 RID: 12484 RVA: 0x00046846 File Offset: 0x00044A46
		public List<GameHex> SelectedFields { get; private set; }

		// Token: 0x060030C5 RID: 12485 RVA: 0x0004684F File Offset: 0x00044A4F
		public GainAnyResource()
			: base(0, 0, false, false, false)
		{
			this.gainType = GainType.AnyResource;
			this.ResourcesLeft = -1;
			base.IsEncounter = false;
			this.Trade = new List<Dictionary<ResourceType, int>>();
			this.SelectedFields = new List<GameHex>();
		}

		// Token: 0x060030C6 RID: 12486 RVA: 0x00046887 File Offset: 0x00044A87
		public GainAnyResource(GameManager gameManager)
			: base(gameManager, 0, 0, false, false, false)
		{
			this.gainType = GainType.AnyResource;
			this.ResourcesLeft = -1;
			base.IsEncounter = false;
			this.Trade = new List<Dictionary<ResourceType, int>>();
			this.SelectedFields = new List<GameHex>();
		}

		// Token: 0x060030C7 RID: 12487 RVA: 0x000468C0 File Offset: 0x00044AC0
		public GainAnyResource(GameManager gameManager, short amount, short maxLevelUpgrade = 0, bool isEncounter = false)
			: base(gameManager, amount, maxLevelUpgrade, isEncounter, false, false)
		{
			this.gainType = GainType.AnyResource;
			this.ResourcesLeft = (int)amount;
			this.Trade = new List<Dictionary<ResourceType, int>>();
			this.SelectedFields = new List<GameHex>();
		}

		// Token: 0x060030C8 RID: 12488 RVA: 0x00127C0C File Offset: 0x00125E0C
		public bool AddResourceToField(ResourceType resource, GameHex field, int amount)
		{
			if (!this.CheckLogic(field, amount, base.IsEncounter))
			{
				return false;
			}
			if (this.SelectedFields == null)
			{
				this.SelectedFields = new List<GameHex>();
			}
			if (this.Trade == null)
			{
				this.Trade = new List<Dictionary<ResourceType, int>>();
			}
			if (!this.SelectedFields.Contains(field))
			{
				this.SelectedFields.Add(field);
				this.Trade.Add(new Dictionary<ResourceType, int>());
			}
			if (this.SelectedFields == null)
			{
				int num = this.SelectedFields.IndexOf(field);
				this.Trade[num].Add(resource, amount);
			}
			else
			{
				int num2 = this.SelectedFields.IndexOf(field);
				if (this.Trade[num2].ContainsKey(resource))
				{
					Dictionary<ResourceType, int> dictionary = this.Trade[num2];
					dictionary[resource] += amount;
				}
				else
				{
					this.Trade[num2].Add(resource, amount);
				}
			}
			if ((!this.gameManager.IsMultiplayer || this.gameManager.PlayerOwner == null || this.gameManager.IsMyTurn()) && !this.gameManager.IsMultiplayer)
			{
				bool isHuman = this.gameManager.PlayerCurrent.IsHuman;
			}
			this.ResourcesLeft -= amount;
			base.ActionSelected = true;
			return true;
		}

		// Token: 0x060030C9 RID: 12489 RVA: 0x00127D58 File Offset: 0x00125F58
		public override bool GainAvaliable()
		{
			bool flag = true;
			int num = 0;
			using (List<Worker>.Enumerator enumerator = this.gameManager.PlayerCurrent.matPlayer.workers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.position == this.gameManager.PlayerCurrent.GetCapital())
					{
						num++;
					}
				}
			}
			if (num == this.gameManager.PlayerCurrent.matPlayer.workers.Count)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060030CA RID: 12490 RVA: 0x000468F3 File Offset: 0x00044AF3
		public override bool CanExecute()
		{
			return this.CheckLogic();
		}

		// Token: 0x060030CB RID: 12491 RVA: 0x00127DF4 File Offset: 0x00125FF4
		private bool CheckLogic()
		{
			int num = 0;
			for (int i = 0; i < this.Trade.Count; i++)
			{
				foreach (ResourceType resourceType in this.Trade[i].Keys)
				{
					num += this.Trade[i][resourceType];
				}
			}
			return num <= (int)base.Amount;
		}

		// Token: 0x060030CC RID: 12492 RVA: 0x00127E84 File Offset: 0x00126084
		private bool CheckLogic(GameHex field, int amount, bool encounter)
		{
			if (amount > this.ResourcesLeft)
			{
				return false;
			}
			if (!encounter)
			{
				using (List<Worker>.Enumerator enumerator = this.player.matPlayer.workers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.position == field)
						{
							return true;
						}
					}
				}
				return false;
			}
			if (this.player.character.position == field)
			{
				return true;
			}
			return false;
		}

		// Token: 0x060030CD RID: 12493 RVA: 0x00127F0C File Offset: 0x0012610C
		public override LogInfo GetLogInfo()
		{
			HexUnitResourceLogInfo hexUnitResourceLogInfo = new HexUnitResourceLogInfo(this.gameManager);
			hexUnitResourceLogInfo.Type = LogInfoType.TradeResources;
			hexUnitResourceLogInfo.IsEncounter = base.IsEncounter;
			hexUnitResourceLogInfo.PlayerAssigned = this.player.matFaction.faction;
			if (this.SelectedFields != null)
			{
				hexUnitResourceLogInfo.Hexes.AddRange(this.SelectedFields);
			}
			hexUnitResourceLogInfo.Resources = new List<Dictionary<ResourceType, int>>(this.Trade);
			return hexUnitResourceLogInfo;
		}

		// Token: 0x060030CE RID: 12494 RVA: 0x00127F7C File Offset: 0x0012617C
		public override void Execute()
		{
			base.Gained = true;
			for (int i = 0; i < this.SelectedFields.Count; i++)
			{
				foreach (ResourceType resourceType in this.Trade[i].Keys)
				{
					Dictionary<ResourceType, int> resources = this.SelectedFields[i].resources;
					ResourceType resourceType2 = resourceType;
					resources[resourceType2] += this.Trade[i][resourceType];
				}
			}
			if (this.gameManager.IsMultiplayer && ((this.gameManager.IsMyTurn() && this.player == this.gameManager.PlayerOwner) || (this.gameManager.PlayerOwner == null && !this.player.IsHuman)))
			{
				this.gameManager.OnActionSent(new GainAnyResourceMessage(this.SelectedFields, this.Trade, base.IsEncounter));
			}
			if ((this.gameManager.IsMultiplayer && this.gameManager.PlayerOwner != null && !this.gameManager.IsMyTurn()) || (!this.gameManager.IsMultiplayer && !this.gameManager.PlayerCurrent.IsHuman))
			{
				TradeEnemyActionInfo tradeEnemyActionInfo = new TradeEnemyActionInfo();
				tradeEnemyActionInfo.fromEncounter = base.IsEncounter;
				tradeEnemyActionInfo.actionOwner = this.player.matFaction.faction;
				tradeEnemyActionInfo.actionType = LogInfoType.TradeResources;
				tradeEnemyActionInfo.hexes = new List<GameHex>(this.SelectedFields);
				for (int j = 0; j < this.SelectedFields.Count; j++)
				{
					this.SelectedFields[j].snapshotResourcesAfterTopAction = new Dictionary<ResourceType, int>(this.SelectedFields[j].resources);
					this.SelectedFields[j].skipTopActionPresentationUpdate = true;
				}
				tradeEnemyActionInfo.resourcesToTrade = new List<Dictionary<ResourceType, int>>(this.Trade);
				this.gameManager.EnemyTrade(tradeEnemyActionInfo);
			}
		}

		// Token: 0x060030CF RID: 12495 RVA: 0x000468FB File Offset: 0x00044AFB
		public override void Clear()
		{
			base.Gained = false;
			this.Trade.Clear();
			this.ResourcesLeft = (int)base.Amount;
			base.ActionSelected = false;
			this.SelectedFields = null;
		}

		// Token: 0x060030D0 RID: 12496 RVA: 0x0012819C File Offset: 0x0012639C
		public override void ReadXml(XmlReader reader)
		{
			base.ReadXml(reader);
			this.ResourcesLeft = (int)base.Amount;
			reader.MoveToContent();
			reader.ReadStartElement();
			this.SelectedFields = new List<GameHex>();
			this.Trade = new List<Dictionary<ResourceType, int>>();
			while (reader.Name == "Hex")
			{
				int num = int.Parse(reader.GetAttribute("X"));
				int num2 = int.Parse(reader.GetAttribute("Y"));
				GameHex gameHex = this.gameManager.gameBoard.hexMap[num, num2];
				if (!this.SelectedFields.Contains(gameHex))
				{
					this.SelectedFields.Add(gameHex);
					this.Trade.Add(new Dictionary<ResourceType, int>());
				}
				int num3 = this.SelectedFields.IndexOf(gameHex);
				if (reader.GetAttribute("Enc") != null)
				{
					base.IsEncounter = true;
				}
				if (reader.GetAttribute("R0") != null)
				{
					this.AddResourceOnRead(0, short.Parse(reader.GetAttribute("R0")), num3, gameHex);
				}
				if (reader.GetAttribute("R1") != null)
				{
					this.AddResourceOnRead(1, short.Parse(reader.GetAttribute("R1")), num3, gameHex);
				}
				if (reader.GetAttribute("R2") != null)
				{
					this.AddResourceOnRead(2, short.Parse(reader.GetAttribute("R2")), num3, gameHex);
				}
				if (reader.GetAttribute("R3") != null)
				{
					this.AddResourceOnRead(3, short.Parse(reader.GetAttribute("R3")), num3, gameHex);
				}
				reader.ReadStartElement();
				if (reader.Name != "Hex" && reader.NodeType == XmlNodeType.EndElement)
				{
					reader.ReadEndElement();
				}
			}
		}

		// Token: 0x060030D1 RID: 12497 RVA: 0x00128340 File Offset: 0x00126540
		private void AddResourceOnRead(int resource, short value, int hexListIndex, GameHex position)
		{
			if (this.SelectedFields == null || hexListIndex >= this.SelectedFields.Count || this.SelectedFields[hexListIndex] != position)
			{
				this.SelectedFields.Add(position);
				this.Trade.Add(new Dictionary<ResourceType, int>());
				this.Trade[this.SelectedFields.IndexOf(position)].Add((ResourceType)resource, (int)value);
				return;
			}
			if (this.Trade[hexListIndex].ContainsKey((ResourceType)resource))
			{
				Dictionary<ResourceType, int> dictionary = this.Trade[hexListIndex];
				dictionary[(ResourceType)resource] = dictionary[(ResourceType)resource] + (int)value;
				return;
			}
			this.Trade[hexListIndex].Add((ResourceType)resource, (int)value);
		}

		// Token: 0x060030D2 RID: 12498 RVA: 0x001283F8 File Offset: 0x001265F8
		public override void WriteXml(XmlWriter writer)
		{
			base.WriteXml(writer);
			if (this.SelectedFields != null)
			{
				List<Dictionary<ResourceType, int>> trade = this.Trade;
				for (int i = 0; i < this.SelectedFields.Count; i++)
				{
					writer.WriteStartElement("Hex");
					writer.WriteAttributeString("X", this.SelectedFields[i].posX.ToString());
					writer.WriteAttributeString("Y", this.SelectedFields[i].posY.ToString());
					if (base.IsEncounter)
					{
						writer.WriteAttributeString("Enc", "");
					}
					foreach (ResourceType resourceType in trade[i].Keys)
					{
						string text = "R";
						int num = (int)resourceType;
						writer.WriteAttributeString(text + num.ToString(), trade[i][resourceType].ToString());
					}
					writer.WriteEndElement();
				}
			}
		}
	}
}
