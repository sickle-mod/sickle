using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002A9 RID: 681
	public class GainAnyResourceMessage : Message, IExecutableMessage
	{
		// Token: 0x1700018F RID: 399
		// (get) Token: 0x0600158E RID: 5518 RVA: 0x000369EB File Offset: 0x00034BEB
		public List<int> getX
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x0600158F RID: 5519 RVA: 0x000369F3 File Offset: 0x00034BF3
		public List<int> getY
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06001590 RID: 5520 RVA: 0x000369FB File Offset: 0x00034BFB
		public List<Dictionary<ResourceType, int>> getResources
		{
			get
			{
				return this.resources;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06001591 RID: 5521 RVA: 0x00036A03 File Offset: 0x00034C03
		public bool getEncounter
		{
			get
			{
				return this.encounter;
			}
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x0009DC88 File Offset: 0x0009BE88
		public GainAnyResourceMessage(List<GameHex> hexes, List<Dictionary<ResourceType, int>> resources, bool encounter)
		{
			this.x = new List<int>();
			this.y = new List<int>();
			for (int i = 0; i < hexes.Count; i++)
			{
				this.x.Add(hexes[i].posX);
				this.y.Add(hexes[i].posY);
			}
			this.resources = resources;
			this.encounter = encounter;
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x0009DD00 File Offset: 0x0009BF00
		public void Execute(GameManager gameManager)
		{
			GainAnyResource gainAnyResource = gameManager.actionManager.GetLastSelectedGainAction() as GainAnyResource;
			for (int i = 0; i < this.x.Count; i++)
			{
				GameHex gameHex = gameManager.gameBoard.hexMap[this.x[i], this.y[i]];
				foreach (KeyValuePair<ResourceType, int> keyValuePair in this.resources[i])
				{
					gainAnyResource.AddResourceToField(keyValuePair.Key, gameHex, keyValuePair.Value);
				}
			}
			gameManager.actionManager.PrepareNextAction();
		}

		// Token: 0x04000FC2 RID: 4034
		private List<int> x;

		// Token: 0x04000FC3 RID: 4035
		private List<int> y;

		// Token: 0x04000FC4 RID: 4036
		private List<Dictionary<ResourceType, int>> resources;

		// Token: 0x04000FC5 RID: 4037
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		private bool encounter;
	}
}
