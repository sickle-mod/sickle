using System;
using Scythe.Utils.Extensions;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000329 RID: 809
	public class PlayerData
	{
		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06001728 RID: 5928 RVA: 0x00037CB3 File Offset: 0x00035EB3
		// (set) Token: 0x06001729 RID: 5929 RVA: 0x00037CBB File Offset: 0x00035EBB
		public string Name { get; set; }

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600172A RID: 5930 RVA: 0x00037CC4 File Offset: 0x00035EC4
		// (set) Token: 0x0600172B RID: 5931 RVA: 0x00037CCC File Offset: 0x00035ECC
		public Guid Id { get; set; }

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600172C RID: 5932 RVA: 0x00037CD5 File Offset: 0x00035ED5
		// (set) Token: 0x0600172D RID: 5933 RVA: 0x00037CDD File Offset: 0x00035EDD
		public int Faction { get; set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x0600172E RID: 5934 RVA: 0x00037CE6 File Offset: 0x00035EE6
		// (set) Token: 0x0600172F RID: 5935 RVA: 0x00037CEE File Offset: 0x00035EEE
		public int PlayerClock { get; set; }

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06001730 RID: 5936 RVA: 0x00037CF7 File Offset: 0x00035EF7
		// (set) Token: 0x06001731 RID: 5937 RVA: 0x00037CFF File Offset: 0x00035EFF
		public bool HasTurn { get; set; }

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06001732 RID: 5938 RVA: 0x00037D08 File Offset: 0x00035F08
		// (set) Token: 0x06001733 RID: 5939 RVA: 0x00037D10 File Offset: 0x00035F10
		public bool MapLoaded { get; set; }

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06001734 RID: 5940 RVA: 0x00037D19 File Offset: 0x00035F19
		// (set) Token: 0x06001735 RID: 5941 RVA: 0x00037D21 File Offset: 0x00035F21
		public bool IsOnline { get; set; }

		// Token: 0x06001736 RID: 5942 RVA: 0x00027E56 File Offset: 0x00026056
		public PlayerData()
		{
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x0009FBC0 File Offset: 0x0009DDC0
		public PlayerData(string name, Guid id, int faction, int playerClock)
		{
			this.Name = name;
			this.Id = id;
			this.Faction = faction;
			this.HasTurn = false;
			this.IsOnline = true;
			if (this.IsBot())
			{
				this.MapLoaded = true;
				this.PlayerClock = 0;
				return;
			}
			this.MapLoaded = false;
			this.PlayerClock = playerClock * 60;
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x00037D2A File Offset: 0x00035F2A
		public bool IsBot()
		{
			return this.Id == PlayerData.BotId || this.Id == PlayerData.LeaverId;
		}

		// Token: 0x04001111 RID: 4369
		public static readonly Guid BotId = Guid.Empty.BotGuid();

		// Token: 0x04001112 RID: 4370
		public static readonly Guid LeaverId = Guid.Empty;
	}
}
