using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Scythe.GameLogic
{
	// Token: 0x020005F4 RID: 1524
	public class Unit : IXmlSerializable
	{
		// Token: 0x17000379 RID: 889
		// (get) Token: 0x0600304E RID: 12366 RVA: 0x000463A4 File Offset: 0x000445A4
		// (set) Token: 0x0600304F RID: 12367 RVA: 0x000463AC File Offset: 0x000445AC
		public Player Owner { get; set; }

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06003050 RID: 12368 RVA: 0x000463B5 File Offset: 0x000445B5
		// (set) Token: 0x06003051 RID: 12369 RVA: 0x000463BD File Offset: 0x000445BD
		public UnitType UnitType { get; protected set; }

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06003052 RID: 12370 RVA: 0x000463C6 File Offset: 0x000445C6
		// (set) Token: 0x06003053 RID: 12371 RVA: 0x000463CE File Offset: 0x000445CE
		public int IndexOnHex { get; set; }

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06003054 RID: 12372 RVA: 0x000463D7 File Offset: 0x000445D7
		// (set) Token: 0x06003055 RID: 12373 RVA: 0x000463DF File Offset: 0x000445DF
		public short MovesLeft { get; set; }

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06003056 RID: 12374 RVA: 0x000463E8 File Offset: 0x000445E8
		// (set) Token: 0x06003057 RID: 12375 RVA: 0x000463F0 File Offset: 0x000445F0
		public short MaxMoveCount { get; private set; }

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06003058 RID: 12376 RVA: 0x000463F9 File Offset: 0x000445F9
		// (set) Token: 0x06003059 RID: 12377 RVA: 0x00046401 File Offset: 0x00044601
		public int Id { get; set; }

		// Token: 0x0600305A RID: 12378 RVA: 0x00126ECC File Offset: 0x001250CC
		public Unit(GameManager gameManager, Player owner, short maxMoveCount = 1)
		{
			this.gameManager = gameManager;
			this.Owner = owner;
			this.MaxMoveCount = maxMoveCount;
			this.MovesLeft = maxMoveCount;
			this.IndexOnHex = -1;
			this.InitBackpack();
		}

		// Token: 0x0600305B RID: 12379 RVA: 0x00126F2C File Offset: 0x0012512C
		private void InitBackpack()
		{
			this.resources.Add(ResourceType.oil, 0);
			this.resources.Add(ResourceType.metal, 0);
			this.resources.Add(ResourceType.food, 0);
			this.resources.Add(ResourceType.wood, 0);
			this.savedResources.Add(ResourceType.oil, 0);
			this.savedResources.Add(ResourceType.metal, 0);
			this.savedResources.Add(ResourceType.food, 0);
			this.savedResources.Add(ResourceType.wood, 0);
		}

		// Token: 0x0600305C RID: 12380 RVA: 0x00126FA4 File Offset: 0x001251A4
		public void AddToBackpack(int oil, int metal, int food, int wood)
		{
			Dictionary<ResourceType, int> dictionary = this.resources;
			dictionary[ResourceType.oil] = dictionary[ResourceType.oil] + oil;
			dictionary = this.resources;
			dictionary[ResourceType.metal] = dictionary[ResourceType.metal] + metal;
			dictionary = this.resources;
			dictionary[ResourceType.food] = dictionary[ResourceType.food] + food;
			dictionary = this.resources;
			dictionary[ResourceType.wood] = dictionary[ResourceType.wood] + wood;
		}

		// Token: 0x0600305D RID: 12381 RVA: 0x0004640A File Offset: 0x0004460A
		public virtual void MoveTo(GameHex position)
		{
			this.position = position;
		}

		// Token: 0x0600305E RID: 12382 RVA: 0x00046413 File Offset: 0x00044613
		public bool IsOnMap()
		{
			return this.position != null;
		}

		// Token: 0x0600305F RID: 12383 RVA: 0x00127010 File Offset: 0x00125210
		public void UpgradeMaxMoveCount()
		{
			short maxMoveCount = this.MaxMoveCount;
			this.MaxMoveCount = maxMoveCount + 1;
			if (!this.gameManager.GameLoading)
			{
				this.MovesLeft = this.MaxMoveCount;
			}
		}

		// Token: 0x06003060 RID: 12384 RVA: 0x0004641E File Offset: 0x0004461E
		public bool NotMoved()
		{
			return this.MaxMoveCount == this.MovesLeft;
		}

		// Token: 0x06003061 RID: 12385 RVA: 0x0004642E File Offset: 0x0004462E
		public void SavePosition()
		{
			this.lastX = this.position.posX;
			this.lastY = this.position.posY;
		}

		// Token: 0x06003062 RID: 12386 RVA: 0x00046452 File Offset: 0x00044652
		public void SaveResources()
		{
			this.savedResources = new Dictionary<ResourceType, int>(this.resources);
		}

		// Token: 0x06003063 RID: 12387 RVA: 0x00046465 File Offset: 0x00044665
		public bool IsMoving()
		{
			return this.moveAnimationLeft != 0;
		}

		// Token: 0x06003064 RID: 12388 RVA: 0x00046470 File Offset: 0x00044670
		public void IncreaseMoveAnimationAmount()
		{
			this.moveAnimationLeft++;
		}

		// Token: 0x06003065 RID: 12389 RVA: 0x00046480 File Offset: 0x00044680
		public void DecreaseMoveAnimationAmount()
		{
			if (this.moveAnimationLeft > 0)
			{
				this.moveAnimationLeft--;
			}
		}

		// Token: 0x06003066 RID: 12390 RVA: 0x00046499 File Offset: 0x00044699
		public void SetRotation(float rotation)
		{
			this.rotation = rotation;
		}

		// Token: 0x06003067 RID: 12391 RVA: 0x000464A2 File Offset: 0x000446A2
		public float GetRotation()
		{
			return this.rotation;
		}

		// Token: 0x06003068 RID: 12392 RVA: 0x0002F5F7 File Offset: 0x0002D7F7
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06003069 RID: 12393 RVA: 0x00127048 File Offset: 0x00125248
		public virtual void ReadXml(XmlReader reader)
		{
			reader.MoveToContent();
			if (reader.GetAttribute("X") != null)
			{
				int num = int.Parse(reader.GetAttribute("X"));
				int num2 = int.Parse(reader.GetAttribute("Y"));
				this.position = this.gameManager.gameBoard.hexMap[num, num2];
			}
			if (reader.GetAttribute("LastX") != null)
			{
				this.lastX = int.Parse(reader.GetAttribute("LastX"));
				this.lastY = int.Parse(reader.GetAttribute("LastY"));
			}
			if (reader.GetAttribute("UnitRotation") != null && !this.gameManager.IsMultiplayer)
			{
				this.SetRotation(float.Parse(reader.GetAttribute("UnitRotation"), NumberStyles.Any, CultureInfo.InvariantCulture));
			}
			this.MovesLeft = short.Parse(reader.GetAttribute("Moves"));
			for (int i = 0; i < 4; i++)
			{
				if (reader.GetAttribute("R" + i.ToString()) != null)
				{
					Dictionary<ResourceType, int> dictionary = this.resources;
					ResourceType resourceType = (ResourceType)i;
					dictionary[resourceType] += int.Parse(reader.GetAttribute("R" + i.ToString()));
				}
			}
		}

		// Token: 0x0600306A RID: 12394 RVA: 0x00127190 File Offset: 0x00125390
		public virtual void WriteXml(XmlWriter writer)
		{
			if (this.position != null)
			{
				writer.WriteAttributeString("X", this.position.posX.ToString());
				writer.WriteAttributeString("Y", this.position.posY.ToString());
			}
			if (this.lastX != -1)
			{
				writer.WriteAttributeString("LastX", this.lastX.ToString());
				writer.WriteAttributeString("LastY", this.lastY.ToString());
			}
			writer.WriteAttributeString("UnitRotation", this.GetRotation().ToString());
			writer.WriteAttributeString("Moves", this.MovesLeft.ToString());
			foreach (ResourceType resourceType in this.resources.Keys)
			{
				if (this.resources[resourceType] > 0)
				{
					string text = "R";
					int num = (int)resourceType;
					writer.WriteAttributeString(text + num.ToString(), this.resources[resourceType].ToString());
				}
			}
		}

		// Token: 0x040020E3 RID: 8419
		public GameHex position;

		// Token: 0x040020E4 RID: 8420
		public Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();

		// Token: 0x040020E5 RID: 8421
		private int moveAnimationLeft;

		// Token: 0x040020E6 RID: 8422
		public bool spawnAnimation;

		// Token: 0x040020E7 RID: 8423
		public bool enemySpawnAnimation;

		// Token: 0x040020E8 RID: 8424
		public int lastX = -1;

		// Token: 0x040020E9 RID: 8425
		public int lastY = -1;

		// Token: 0x040020EA RID: 8426
		public Dictionary<ResourceType, int> savedResources = new Dictionary<ResourceType, int>();

		// Token: 0x040020EB RID: 8427
		private float rotation;

		// Token: 0x040020ED RID: 8429
		private GameManager gameManager;
	}
}
