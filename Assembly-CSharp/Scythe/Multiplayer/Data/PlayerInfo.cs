using System;
using System.Xml.Serialization;
using Scythe.GameLogic;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x0200032A RID: 810
	public class PlayerInfo
	{
		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x0600173A RID: 5946 RVA: 0x00037D6B File Offset: 0x00035F6B
		// (set) Token: 0x0600173B RID: 5947 RVA: 0x00037D73 File Offset: 0x00035F73
		public string RoomId { get; set; }

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x0600173C RID: 5948 RVA: 0x00037D7C File Offset: 0x00035F7C
		// (set) Token: 0x0600173D RID: 5949 RVA: 0x00037D84 File Offset: 0x00035F84
		public bool IsReady { get; set; }

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x0600173E RID: 5950 RVA: 0x00037D8D File Offset: 0x00035F8D
		// (set) Token: 0x0600173F RID: 5951 RVA: 0x00037D95 File Offset: 0x00035F95
		public bool IsAdmin { get; set; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06001740 RID: 5952 RVA: 0x00037D9E File Offset: 0x00035F9E
		// (set) Token: 0x06001741 RID: 5953 RVA: 0x00037DA6 File Offset: 0x00035FA6
		public bool MapLoaded { get; set; }

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06001742 RID: 5954 RVA: 0x00037DAF File Offset: 0x00035FAF
		// (set) Token: 0x06001743 RID: 5955 RVA: 0x00037DB7 File Offset: 0x00035FB7
		public int Faction { get; set; }

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06001744 RID: 5956 RVA: 0x00037DC0 File Offset: 0x00035FC0
		// (set) Token: 0x06001745 RID: 5957 RVA: 0x00037DC8 File Offset: 0x00035FC8
		public int PlayerMat { get; set; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06001746 RID: 5958 RVA: 0x00037DD1 File Offset: 0x00035FD1
		// (set) Token: 0x06001747 RID: 5959 RVA: 0x00037DD9 File Offset: 0x00035FD9
		public int Slot { get; set; }

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06001748 RID: 5960 RVA: 0x00037DE2 File Offset: 0x00035FE2
		// (set) Token: 0x06001749 RID: 5961 RVA: 0x00037DEA File Offset: 0x00035FEA
		public bool DLC { get; set; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x0600174A RID: 5962 RVA: 0x00037DF3 File Offset: 0x00035FF3
		// (set) Token: 0x0600174B RID: 5963 RVA: 0x00037DFB File Offset: 0x00035FFB
		public RelationshipType RelationshipType { get; set; }

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x0600174C RID: 5964 RVA: 0x00037E04 File Offset: 0x00036004
		// (set) Token: 0x0600174D RID: 5965 RVA: 0x00037E0C File Offset: 0x0003600C
		public PlayerStatus PlayerStatus { get; set; }

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x0600174E RID: 5966 RVA: 0x00037E15 File Offset: 0x00036015
		// (set) Token: 0x0600174F RID: 5967 RVA: 0x00037E1D File Offset: 0x0003601D
		public Invitation Invitation { get; set; }

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06001750 RID: 5968 RVA: 0x00037E26 File Offset: 0x00036026
		// (set) Token: 0x06001751 RID: 5969 RVA: 0x00037E2E File Offset: 0x0003602E
		public PlayerStats PlayerStats { get; set; }

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06001752 RID: 5970 RVA: 0x00037E37 File Offset: 0x00036037
		// (set) Token: 0x06001753 RID: 5971 RVA: 0x00037E3F File Offset: 0x0003603F
		[XmlIgnore]
		public LobbyGame CurrentLobbyRoom { get; set; }

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06001754 RID: 5972 RVA: 0x00037E48 File Offset: 0x00036048
		// (set) Token: 0x06001755 RID: 5973 RVA: 0x00037E50 File Offset: 0x00036050
		[XmlIgnore]
		public string Token { get; set; }

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06001756 RID: 5974 RVA: 0x00037E59 File Offset: 0x00036059
		// (set) Token: 0x06001757 RID: 5975 RVA: 0x00037E61 File Offset: 0x00036061
		[XmlIgnore]
		public bool IsLoaded { get; set; }

		// Token: 0x06001758 RID: 5976 RVA: 0x00037E6A File Offset: 0x0003606A
		public PlayerInfo()
		{
			this.PlayerStats = new PlayerStats();
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x0009FC20 File Offset: 0x0009DE20
		public override string ToString()
		{
			string[] array = new string[16];
			array[0] = "ID: ";
			int num = 1;
			Guid id = this.PlayerStats.Id;
			array[num] = id.ToString();
			array[2] = " Name ";
			array[3] = this.PlayerStats.Name;
			array[4] = " IsReady: ";
			array[5] = this.IsReady.ToString();
			array[6] = " IsAdmin: ";
			array[7] = this.IsAdmin.ToString();
			array[8] = " SlotId: ";
			array[9] = this.Slot.ToString();
			array[10] = " Faction: ";
			array[11] = ((Faction)this.Faction).ToString();
			array[12] = " ELO: ";
			array[13] = this.PlayerStats.ELO.ToString();
			array[14] = " Karma: ";
			array[15] = this.PlayerStats.Karma.ToString();
			return string.Concat(array);
		}

		// Token: 0x04001129 RID: 4393
		public static PlayerInfo me = new PlayerInfo
		{
			RoomId = "00",
			IsReady = false,
			MapLoaded = false,
			IsAdmin = false,
			Faction = -1,
			PlayerMat = -1,
			Slot = -1,
			PlayerStats = new PlayerStats(),
			CurrentLobbyRoom = null,
			Token = string.Empty,
			DLC = GameServiceController.Instance.InvadersFromAfarUnlocked(),
			IsLoaded = false
		};
	}
}
