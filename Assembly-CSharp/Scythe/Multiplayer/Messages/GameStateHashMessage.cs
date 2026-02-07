using System;
using System.Security.Cryptography;
using System.Text;
using Scythe.GameLogic;
using Scythe.Utilities;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002E7 RID: 743
	public class GameStateHashMessage : Message, IExecutableMessage
	{
		// Token: 0x06001624 RID: 5668 RVA: 0x00037367 File Offset: 0x00035567
		public GameStateHashMessage(string hash)
		{
			this.hash = hash;
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x0009F49C File Offset: 0x0009D69C
		public void Execute(GameManager gameManager)
		{
			string text;
			using (MD5 md = MD5.Create())
			{
				gameManager.SendingToPlayer = true;
				text = BitConverter.ToString(md.ComputeHash(Encoding.UTF8.GetBytes(GameSerializer.Serialize<GameManager>(gameManager)))).Replace("-", string.Empty);
				gameManager.SendingToPlayer = false;
			}
			if (!this.hash.Equals(text))
			{
				MultiplayerController.Instance.GetServerGameState();
			}
		}

		// Token: 0x04001060 RID: 4192
		private string hash;
	}
}
