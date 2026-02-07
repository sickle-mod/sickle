using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000369 RID: 873
	[DataContract]
	public class LoginParams : IEquatable<LoginParams>
	{
		// Token: 0x06001919 RID: 6425 RVA: 0x000A14F0 File Offset: 0x0009F6F0
		[Preserve]
		public LoginParams(string Login = null, string Password = null, string Ticket = null, string PublicKeyUrl = null, string Signature = null, string Salt = null, string PlayerId = null, long? Timestamp = null, string BundleId = null)
		{
			this.Login = Login;
			this.Password = Password;
			this.Ticket = Ticket;
			this.PublicKeyUrl = PublicKeyUrl;
			this.Signature = Signature;
			this.Salt = Salt;
			this.PlayerId = PlayerId;
			this.Timestamp = Timestamp;
			this.BundleId = BundleId;
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x0600191A RID: 6426 RVA: 0x00038DBA File Offset: 0x00036FBA
		// (set) Token: 0x0600191B RID: 6427 RVA: 0x00038DC2 File Offset: 0x00036FC2
		[DataMember(Name = "login", EmitDefaultValue = true)]
		public string Login { get; set; }

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x0600191C RID: 6428 RVA: 0x00038DCB File Offset: 0x00036FCB
		// (set) Token: 0x0600191D RID: 6429 RVA: 0x00038DD3 File Offset: 0x00036FD3
		[DataMember(Name = "password", EmitDefaultValue = true)]
		public string Password { get; set; }

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x0600191E RID: 6430 RVA: 0x00038DDC File Offset: 0x00036FDC
		// (set) Token: 0x0600191F RID: 6431 RVA: 0x00038DE4 File Offset: 0x00036FE4
		[DataMember(Name = "ticket", EmitDefaultValue = true)]
		public string Ticket { get; set; }

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06001920 RID: 6432 RVA: 0x00038DED File Offset: 0x00036FED
		// (set) Token: 0x06001921 RID: 6433 RVA: 0x00038DF5 File Offset: 0x00036FF5
		[DataMember(Name = "publicKeyUrl", EmitDefaultValue = true)]
		public string PublicKeyUrl { get; set; }

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06001922 RID: 6434 RVA: 0x00038DFE File Offset: 0x00036FFE
		// (set) Token: 0x06001923 RID: 6435 RVA: 0x00038E06 File Offset: 0x00037006
		[DataMember(Name = "signature", EmitDefaultValue = true)]
		public string Signature { get; set; }

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06001924 RID: 6436 RVA: 0x00038E0F File Offset: 0x0003700F
		// (set) Token: 0x06001925 RID: 6437 RVA: 0x00038E17 File Offset: 0x00037017
		[DataMember(Name = "salt", EmitDefaultValue = true)]
		public string Salt { get; set; }

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06001926 RID: 6438 RVA: 0x00038E20 File Offset: 0x00037020
		// (set) Token: 0x06001927 RID: 6439 RVA: 0x00038E28 File Offset: 0x00037028
		[DataMember(Name = "playerId", EmitDefaultValue = true)]
		public string PlayerId { get; set; }

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06001928 RID: 6440 RVA: 0x00038E31 File Offset: 0x00037031
		// (set) Token: 0x06001929 RID: 6441 RVA: 0x00038E39 File Offset: 0x00037039
		[DataMember(Name = "timestamp", EmitDefaultValue = true)]
		public long? Timestamp { get; set; }

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x0600192A RID: 6442 RVA: 0x00038E42 File Offset: 0x00037042
		// (set) Token: 0x0600192B RID: 6443 RVA: 0x00038E4A File Offset: 0x0003704A
		[DataMember(Name = "bundleId", EmitDefaultValue = true)]
		public string BundleId { get; set; }

		// Token: 0x0600192C RID: 6444 RVA: 0x000A1548 File Offset: 0x0009F748
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class LoginParams {\n");
			stringBuilder.Append("  Login: ").Append(this.Login).Append("\n");
			stringBuilder.Append("  Password: ").Append(this.Password).Append("\n");
			stringBuilder.Append("  Ticket: ").Append(this.Ticket).Append("\n");
			stringBuilder.Append("  PublicKeyUrl: ").Append(this.PublicKeyUrl).Append("\n");
			stringBuilder.Append("  Signature: ").Append(this.Signature).Append("\n");
			stringBuilder.Append("  Salt: ").Append(this.Salt).Append("\n");
			stringBuilder.Append("  PlayerId: ").Append(this.PlayerId).Append("\n");
			stringBuilder.Append("  Timestamp: ").Append(this.Timestamp).Append("\n");
			stringBuilder.Append("  BundleId: ").Append(this.BundleId).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x0600192E RID: 6446 RVA: 0x00038E53 File Offset: 0x00037053
		public override bool Equals(object input)
		{
			return this.Equals(input as LoginParams);
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x000A16A8 File Offset: 0x0009F8A8
		public bool Equals(LoginParams input)
		{
			if (input == null)
			{
				return false;
			}
			if ((this.Login == input.Login || (this.Login != null && this.Login.Equals(input.Login))) && (this.Password == input.Password || (this.Password != null && this.Password.Equals(input.Password))) && (this.Ticket == input.Ticket || (this.Ticket != null && this.Ticket.Equals(input.Ticket))) && (this.PublicKeyUrl == input.PublicKeyUrl || (this.PublicKeyUrl != null && this.PublicKeyUrl.Equals(input.PublicKeyUrl))) && (this.Signature == input.Signature || (this.Signature != null && this.Signature.Equals(input.Signature))) && (this.Salt == input.Salt || (this.Salt != null && this.Salt.Equals(input.Salt))) && (this.PlayerId == input.PlayerId || (this.PlayerId != null && this.PlayerId.Equals(input.PlayerId))))
			{
				long? timestamp = this.Timestamp;
				long? timestamp2 = input.Timestamp;
				if (((timestamp.GetValueOrDefault() == timestamp2.GetValueOrDefault()) & (timestamp != null == (timestamp2 != null))) || (this.Timestamp != null && this.Timestamp.Equals(input.Timestamp)))
				{
					return this.BundleId == input.BundleId || (this.BundleId != null && this.BundleId.Equals(input.BundleId));
				}
			}
			return false;
		}

		// Token: 0x06001930 RID: 6448 RVA: 0x000A18BC File Offset: 0x0009FABC
		public override int GetHashCode()
		{
			int num = 41;
			if (this.Login != null)
			{
				num = num * 59 + this.Login.GetHashCode();
			}
			if (this.Password != null)
			{
				num = num * 59 + this.Password.GetHashCode();
			}
			if (this.Ticket != null)
			{
				num = num * 59 + this.Ticket.GetHashCode();
			}
			if (this.PublicKeyUrl != null)
			{
				num = num * 59 + this.PublicKeyUrl.GetHashCode();
			}
			if (this.Signature != null)
			{
				num = num * 59 + this.Signature.GetHashCode();
			}
			if (this.Salt != null)
			{
				num = num * 59 + this.Salt.GetHashCode();
			}
			if (this.PlayerId != null)
			{
				num = num * 59 + this.PlayerId.GetHashCode();
			}
			if (this.Timestamp != null)
			{
				num = num * 59 + this.Timestamp.GetHashCode();
			}
			if (this.BundleId != null)
			{
				num = num * 59 + this.BundleId.GetHashCode();
			}
			return num;
		}
	}
}
