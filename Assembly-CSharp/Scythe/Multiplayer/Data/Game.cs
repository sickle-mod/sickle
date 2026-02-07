using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000311 RID: 785
	public class Game : Data
	{
		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060016C8 RID: 5832 RVA: 0x00037970 File Offset: 0x00035B70
		// (set) Token: 0x060016C9 RID: 5833 RVA: 0x00037978 File Offset: 0x00035B78
		public bool Asynchronous { get; set; }

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060016CA RID: 5834 RVA: 0x00037981 File Offset: 0x00035B81
		// (set) Token: 0x060016CB RID: 5835 RVA: 0x00037989 File Offset: 0x00035B89
		public bool Ranked { get; set; }

		// Token: 0x060016CC RID: 5836 RVA: 0x00037992 File Offset: 0x00035B92
		public Game()
		{
			this.Asynchronous = MultiplayerController.Instance.Asynchronous;
			this.Ranked = MultiplayerController.Instance.Ranked;
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x000379BA File Offset: 0x00035BBA
		public Game(string gameId, bool ranked, bool asynchronous)
			: base(gameId)
		{
			this.Asynchronous = asynchronous;
			this.Ranked = ranked;
		}
	}
}
