using System;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Tls;

namespace AsmodeeNet.Network
{
	// Token: 0x02000868 RID: 2152
	public class CertificateVerifier : ICertificateVerifyer
	{
		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06003C79 RID: 15481 RVA: 0x0004F23F File Offset: 0x0004D43F
		// (set) Token: 0x06003C7A RID: 15482 RVA: 0x0004F247 File Offset: 0x0004D447
		public bool isValid { get; protected set; }

		// Token: 0x06003C7B RID: 15483 RVA: 0x0004F250 File Offset: 0x0004D450
		public CertificateVerifier(string[] publicKeys)
		{
			this._publicKeys = publicKeys;
		}

		// Token: 0x06003C7C RID: 15484 RVA: 0x00155870 File Offset: 0x00153A70
		public bool IsValid(Uri serverUri, X509CertificateStructure[] certs)
		{
			this.isValid = false;
			int num = 0;
			while (num < certs.Length && !this.isValid)
			{
				string @string = certs[num].SubjectPublicKeyInfo.PublicKeyData.GetString();
				foreach (string text in this._publicKeys)
				{
					if (@string == text)
					{
						this.isValid = true;
						break;
					}
				}
				num++;
			}
			return this.isValid;
		}

		// Token: 0x04002DD0 RID: 11728
		private string[] _publicKeys;
	}
}
