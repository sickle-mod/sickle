using System;

namespace Scythe.Multiplayer.Data
{
	// Token: 0x02000333 RID: 819
	public class GameCenterVerificationSignature
	{
		// Token: 0x06001785 RID: 6021 RVA: 0x000380BC File Offset: 0x000362BC
		public string GetTimestamp()
		{
			return this.m_timestamp;
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x000380C4 File Offset: 0x000362C4
		public string GetSalt()
		{
			return this.m_salt;
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x000380CC File Offset: 0x000362CC
		public string GetPublicKeyUrl()
		{
			return this.m_publicKeyUrl;
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x000380D4 File Offset: 0x000362D4
		public string GetSignature()
		{
			return this.m_signature;
		}

		// Token: 0x06001789 RID: 6025 RVA: 0x000380DC File Offset: 0x000362DC
		public bool VerificationSuccess()
		{
			return this.m_error.GetErrorCode() == 0;
		}

		// Token: 0x0600178A RID: 6026 RVA: 0x000380EC File Offset: 0x000362EC
		public string GetErrorMessage()
		{
			return this.m_error.GetErrorMessage();
		}

		// Token: 0x04001143 RID: 4419
		private string m_timestamp;

		// Token: 0x04001144 RID: 4420
		private string m_salt;

		// Token: 0x04001145 RID: 4421
		private string m_publicKeyUrl;

		// Token: 0x04001146 RID: 4422
		private string m_signature;

		// Token: 0x04001147 RID: 4423
		private GameCenterVerificationSignatureError m_error;
	}
}
