using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000334 RID: 820
	public class GameCenterVerificationSignatureError
	{
		// Token: 0x0600178C RID: 6028 RVA: 0x000380F9 File Offset: 0x000362F9
		public string GetErrorMessage()
		{
			return this.m_message;
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x00038101 File Offset: 0x00036301
		public int GetErrorCode()
		{
			return this.m_code;
		}

		// Token: 0x04001148 RID: 4424
		private string m_message;

		// Token: 0x04001149 RID: 4425
		private int m_code;
	}
}
