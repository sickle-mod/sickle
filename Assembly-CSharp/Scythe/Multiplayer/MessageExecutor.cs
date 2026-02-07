using System;
using Scythe.GameLogic;
using Scythe.Multiplayer.Messages;
using Scythe.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x0200027D RID: 637
	public class MessageExecutor
	{
		// Token: 0x0600141A RID: 5146 RVA: 0x0003587D File Offset: 0x00033A7D
		public static int LastMessageCounter()
		{
			return MessageExecutor.lastMessageCounter;
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x00035884 File Offset: 0x00033A84
		public static void ResetLastMessageCounter()
		{
			MessageExecutor.lastMessageCounter = 0;
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x0003588C File Offset: 0x00033A8C
		public static bool IsMessageValid(Message message)
		{
			return message.GetCounter() == MessageExecutor.lastMessageCounter + 1;
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x0003589D File Offset: 0x00033A9D
		public static void IncreaseCounter()
		{
			MessageExecutor.lastMessageCounter++;
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x000358AB File Offset: 0x00033AAB
		public void ExecuteMessage(IExecutableMessage message, GameManager gameManager)
		{
			GameController.Instance.AddMessageToExecute(message);
		}

		// Token: 0x04000ECE RID: 3790
		private static int lastMessageCounter;
	}
}
