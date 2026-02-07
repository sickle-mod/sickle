using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000328 RID: 808
	public class LobbyRoom : Data
	{
		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600170C RID: 5900 RVA: 0x00037BD6 File Offset: 0x00035DD6
		// (set) Token: 0x0600170D RID: 5901 RVA: 0x00037BDE File Offset: 0x00035DDE
		public int MaxPlayers { get; set; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600170E RID: 5902 RVA: 0x00037BE7 File Offset: 0x00035DE7
		// (set) Token: 0x0600170F RID: 5903 RVA: 0x00037BEF File Offset: 0x00035DEF
		public string Name { get; set; }

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06001710 RID: 5904 RVA: 0x00037BF8 File Offset: 0x00035DF8
		// (set) Token: 0x06001711 RID: 5905 RVA: 0x00037C00 File Offset: 0x00035E00
		public int Players { get; set; }

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06001712 RID: 5906 RVA: 0x00037C09 File Offset: 0x00035E09
		// (set) Token: 0x06001713 RID: 5907 RVA: 0x00037C11 File Offset: 0x00035E11
		public int PlayerClockTime { get; set; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06001714 RID: 5908 RVA: 0x00037C1A File Offset: 0x00035E1A
		// (set) Token: 0x06001715 RID: 5909 RVA: 0x00037C22 File Offset: 0x00035E22
		public bool IsRanked { get; set; }

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06001716 RID: 5910 RVA: 0x00037C2B File Offset: 0x00035E2B
		// (set) Token: 0x06001717 RID: 5911 RVA: 0x00037C33 File Offset: 0x00035E33
		public int MinELO { get; set; }

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06001718 RID: 5912 RVA: 0x00037C3C File Offset: 0x00035E3C
		// (set) Token: 0x06001719 RID: 5913 RVA: 0x00037C44 File Offset: 0x00035E44
		public int MaxELO { get; set; }

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x0600171A RID: 5914 RVA: 0x00037C4D File Offset: 0x00035E4D
		// (set) Token: 0x0600171B RID: 5915 RVA: 0x00037C55 File Offset: 0x00035E55
		public bool PromoCardsEnabled { get; set; }

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600171C RID: 5916 RVA: 0x00037C5E File Offset: 0x00035E5E
		// (set) Token: 0x0600171D RID: 5917 RVA: 0x00037C66 File Offset: 0x00035E66
		public bool IsAsynchronous { get; set; }

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600171E RID: 5918 RVA: 0x00037C6F File Offset: 0x00035E6F
		// (set) Token: 0x0600171F RID: 5919 RVA: 0x00037C77 File Offset: 0x00035E77
		public bool IsPrivate { get; set; }

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06001720 RID: 5920 RVA: 0x00037C80 File Offset: 0x00035E80
		// (set) Token: 0x06001721 RID: 5921 RVA: 0x00037C88 File Offset: 0x00035E88
		public bool InvadersFromAfar { get; set; }

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06001722 RID: 5922 RVA: 0x00037C91 File Offset: 0x00035E91
		// (set) Token: 0x06001723 RID: 5923 RVA: 0x00037C99 File Offset: 0x00035E99
		public bool AllRandom { get; set; }

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06001724 RID: 5924 RVA: 0x00037CA2 File Offset: 0x00035EA2
		// (set) Token: 0x06001725 RID: 5925 RVA: 0x00037CAA File Offset: 0x00035EAA
		public bool ChoosingMats { get; set; }

		// Token: 0x06001726 RID: 5926 RVA: 0x00037A7B File Offset: 0x00035C7B
		public LobbyRoom()
		{
		}

		// Token: 0x06001727 RID: 5927 RVA: 0x0009FB44 File Offset: 0x0009DD44
		public LobbyRoom(string name, int maxPlayers, int playerClockTime, bool isRanked, int minELO, int maxELO, bool promoCards, bool isAsynchronous, bool isPrivate, bool invadersFromAfar, bool allRandom)
		{
			this.Name = name;
			this.MaxPlayers = maxPlayers;
			this.Players = 0;
			base.RoomId = "00";
			this.PlayerClockTime = playerClockTime;
			this.IsRanked = isRanked;
			this.MinELO = minELO;
			this.MaxELO = maxELO;
			this.PromoCardsEnabled = promoCards;
			this.IsAsynchronous = isAsynchronous;
			this.IsPrivate = isPrivate;
			this.InvadersFromAfar = invadersFromAfar;
			this.AllRandom = allRandom;
		}
	}
}
