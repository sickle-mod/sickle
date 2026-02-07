using System;
using System.Collections.Generic;
using System.Linq;
using I2.Loc;
using Newtonsoft.Json;
using Scythe.Multiplayer.Data;

namespace Scythe.Multiplayer.Messages
{
	// Token: 0x020002C0 RID: 704
	public class AdminMessages : Message, IExecutableLobbyMessage
	{
		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060015C4 RID: 5572 RVA: 0x00036D78 File Offset: 0x00034F78
		// (set) Token: 0x060015C5 RID: 5573 RVA: 0x00036D80 File Offset: 0x00034F80
		public List<string> Messages { get; set; }

		// Token: 0x060015C6 RID: 5574 RVA: 0x00036D89 File Offset: 0x00034F89
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoom gameRoom)
		{
			this.ExecuteLogic();
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x00036D89 File Offset: 0x00034F89
		public void Execute(Lobby lobby, PlayerListPanel playerListPanel, GameListPanel gameListPanel, GameRoomMobile gameRoomMobile)
		{
			this.ExecuteLogic();
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x0009E934 File Offset: 0x0009CB34
		private void ExecuteLogic()
		{
			List<AdminMessage> list = new List<AdminMessage>(this.Messages.Count);
			list.AddRange(this.Messages.Select((string message) => JsonConvert.DeserializeObject<AdminMessage>(message)));
			list.Sort((AdminMessage m1, AdminMessage m2) => m1.Counter.CompareTo(m2.Counter));
			string text = LocalizationManager.CurrentLanguage.ToLower();
			foreach (AdminMessage adminMessage in list)
			{
				ServerUpdate serverUpdate = JsonConvert.DeserializeObject<ServerUpdate>(adminMessage.Message);
				string text2 = string.Empty;
				uint num = global::<PrivateImplementationDetails>.ComputeStringHash(text);
				if (num <= 2471602315U)
				{
					if (num <= 599131013U)
					{
						if (num != 380651494U)
						{
							if (num == 599131013U)
							{
								if (text == "french")
								{
									text2 = serverUpdate.French;
								}
							}
						}
						else if (text == "russian")
						{
							text2 = serverUpdate.Russian;
						}
					}
					else if (num != 854220921U)
					{
						if (num != 1901528810U)
						{
							if (num == 2471602315U)
							{
								if (text == "italian")
								{
									text2 = serverUpdate.Italian;
								}
							}
						}
						else if (text == "japanese")
						{
							text2 = serverUpdate.Japanese;
						}
					}
					else if (text == "traditional chinese")
					{
						text2 = serverUpdate.TraditionalChinese;
					}
				}
				else if (num <= 3405445907U)
				{
					if (num != 2499415067U)
					{
						if (num != 3180870988U)
						{
							if (num == 3405445907U)
							{
								if (text == "german")
								{
									text2 = serverUpdate.German;
								}
							}
						}
						else if (text == "polish")
						{
							text2 = serverUpdate.Polish;
						}
					}
					else if (text == "english")
					{
						text2 = serverUpdate.English;
					}
				}
				else if (num != 3719199419U)
				{
					if (num != 3929117860U)
					{
						if (num == 3973263950U)
						{
							if (text == "brazilian portuguese")
							{
								text2 = serverUpdate.Brazilian;
							}
						}
					}
					else if (text == "simplified chinese")
					{
						text2 = serverUpdate.SimplifiedChinese;
					}
				}
				else if (text == "spanish")
				{
					text2 = serverUpdate.Spanish;
				}
				RequestController.serverUpdates.Enqueue(new ChatMessage(true, false, string.Empty, string.Empty, text2));
			}
		}
	}
}
