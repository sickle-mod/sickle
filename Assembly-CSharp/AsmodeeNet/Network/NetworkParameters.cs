using System;
using UnityEngine;

namespace AsmodeeNet.Network
{
	// Token: 0x02000869 RID: 2153
	[CreateAssetMenu]
	public class NetworkParameters : ScriptableObject
	{
		// Token: 0x06003C7D RID: 15485 RVA: 0x0004F25F File Offset: 0x0004D45F
		public string GetApiBaseUrl()
		{
			if (this.RestAPIHostName.StartsWith("http"))
			{
				return this.RestAPIHostName;
			}
			return "https://" + this.RestAPIHostName;
		}

		// Token: 0x04002DD1 RID: 11729
		[Header("Scalable Server")]
		public string HostName = string.Empty;

		// Token: 0x04002DD2 RID: 11730
		public int HostPort;

		// Token: 0x04002DD3 RID: 11731
		[HideInInspector]
		public bool UseSSL = true;

		// Token: 0x04002DD4 RID: 11732
		public string[] PinPublicKeys = new string[1];

		// Token: 0x04002DD5 RID: 11733
		public float PingDelay = 10f;

		// Token: 0x04002DD6 RID: 11734
		public float AutoReconnectDelay = 6f;

		// Token: 0x04002DD7 RID: 11735
		public string GameType = string.Empty;

		// Token: 0x04002DD8 RID: 11736
		[Header("REST API")]
		public string ClientId = string.Empty;

		// Token: 0x04002DD9 RID: 11737
		public string ClientSecret = string.Empty;

		// Token: 0x04002DDA RID: 11738
		public string RestAPIHostName = string.Empty;

		// Token: 0x04002DDB RID: 11739
		public string[] RestAPIPinPublicKeys = new string[1];
	}
}
