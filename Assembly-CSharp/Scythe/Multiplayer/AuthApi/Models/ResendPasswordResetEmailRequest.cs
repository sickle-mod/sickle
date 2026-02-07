using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000379 RID: 889
	[DataContract]
	public class ResendPasswordResetEmailRequest : IEquatable<ResendPasswordResetEmailRequest>
	{
		// Token: 0x060019C2 RID: 6594 RVA: 0x000392D7 File Offset: 0x000374D7
		[Preserve]
		public ResendPasswordResetEmailRequest(string LoginOrEmail = null)
		{
			if (LoginOrEmail == null)
			{
				throw new InvalidDataException("LoginOrEmail is a required property for ResendPasswordResetEmailRequest and cannot be null");
			}
			this.LoginOrEmail = LoginOrEmail;
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060019C3 RID: 6595 RVA: 0x000392F4 File Offset: 0x000374F4
		// (set) Token: 0x060019C4 RID: 6596 RVA: 0x000392FC File Offset: 0x000374FC
		[DataMember(Name = "loginOrEmail", EmitDefaultValue = true)]
		public string LoginOrEmail { get; set; }

		// Token: 0x060019C5 RID: 6597 RVA: 0x000A2C80 File Offset: 0x000A0E80
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class ResendPasswordResetEmailRequest {\n");
			stringBuilder.Append("  LoginOrEmail: ").Append(this.LoginOrEmail).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x00039305 File Offset: 0x00037505
		public override bool Equals(object input)
		{
			return this.Equals(input as ResendPasswordResetEmailRequest);
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x00039313 File Offset: 0x00037513
		public bool Equals(ResendPasswordResetEmailRequest input)
		{
			return input != null && (this.LoginOrEmail == input.LoginOrEmail || (this.LoginOrEmail != null && this.LoginOrEmail.Equals(input.LoginOrEmail)));
		}

		// Token: 0x060019C9 RID: 6601 RVA: 0x000A2CD0 File Offset: 0x000A0ED0
		public override int GetHashCode()
		{
			int num = 41;
			if (this.LoginOrEmail != null)
			{
				num = num * 59 + this.LoginOrEmail.GetHashCode();
			}
			return num;
		}
	}
}
