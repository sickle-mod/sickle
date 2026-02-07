using System;
using Scythe.GameLogic;

namespace Scythe.UI
{
	// Token: 0x02000467 RID: 1127
	public class GameLogicHandler : PersistentSingleton<GameLogicHandler>
	{
		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06002390 RID: 9104 RVA: 0x0003EC1C File Offset: 0x0003CE1C
		public Game Game
		{
			get
			{
				return this._game;
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06002391 RID: 9105 RVA: 0x0003EC24 File Offset: 0x0003CE24
		public GameManager GameManager
		{
			get
			{
				return this._game.GameManager;
			}
		}

		// Token: 0x040018BB RID: 6331
		private Game _game = new Game();
	}
}
