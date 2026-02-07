using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000330 RID: 816
	public class SpectateGame : Data
	{
		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06001777 RID: 6007 RVA: 0x00037FBF File Offset: 0x000361BF
		// (set) Token: 0x06001778 RID: 6008 RVA: 0x00037FC7 File Offset: 0x000361C7
		public int FactionToSpectate { get; set; }

		// Token: 0x06001779 RID: 6009 RVA: 0x00037A7B File Offset: 0x00035C7B
		public SpectateGame()
		{
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x00037FD0 File Offset: 0x000361D0
		public SpectateGame(int factionToSpectate, string roomId)
			: base(roomId)
		{
			this.FactionToSpectate = factionToSpectate;
		}
	}
}
