using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x0200037B RID: 891
	[DataContract]
	public class ResetPasswordRequest : IEquatable<ResetPasswordRequest>
	{
		// Token: 0x060019D4 RID: 6612 RVA: 0x00039390 File Offset: 0x00037590
		[Preserve]
		public ResetPasswordRequest(string LoginOrEmail = null, string NewPassword = null)
		{
			if (LoginOrEmail == null)
			{
				throw new InvalidDataException("LoginOrEmail is a required property for ResetPasswordRequest and cannot be null");
			}
			this.LoginOrEmail = LoginOrEmail;
			if (NewPassword == null)
			{
				throw new InvalidDataException("NewPassword is a required property for ResetPasswordRequest and cannot be null");
			}
			this.NewPassword = NewPassword;
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x060019D5 RID: 6613 RVA: 0x000393C2 File Offset: 0x000375C2
		// (set) Token: 0x060019D6 RID: 6614 RVA: 0x000393CA File Offset: 0x000375CA
		[DataMember(Name = "loginOrEmail", EmitDefaultValue = true)]
		public string LoginOrEmail { get; set; }

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x060019D7 RID: 6615 RVA: 0x000393D3 File Offset: 0x000375D3
		// (set) Token: 0x060019D8 RID: 6616 RVA: 0x000393DB File Offset: 0x000375DB
		[DataMember(Name = "newPassword", EmitDefaultValue = true)]
		public string NewPassword { get; set; }

		// Token: 0x060019D9 RID: 6617 RVA: 0x000A2E3C File Offset: 0x000A103C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class ResetPasswordRequest {\n");
			stringBuilder.Append("  LoginOrEmail: ").Append(this.LoginOrEmail).Append("\n");
			stringBuilder.Append("  NewPassword: ").Append(this.NewPassword).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x000393E4 File Offset: 0x000375E4
		public override bool Equals(object input)
		{
			return this.Equals(input as ResetPasswordRequest);
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x000A2EB0 File Offset: 0x000A10B0
		public bool Equals(ResetPasswordRequest input)
		{
			return input != null && (this.LoginOrEmail == input.LoginOrEmail || (this.LoginOrEmail != null && this.LoginOrEmail.Equals(input.LoginOrEmail))) && (this.NewPassword == input.NewPassword || (this.NewPassword != null && this.NewPassword.Equals(input.NewPassword)));
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x000A2F24 File Offset: 0x000A1124
		public override int GetHashCode()
		{
			int num = 41;
			if (this.LoginOrEmail != null)
			{
				num = num * 59 + this.LoginOrEmail.GetHashCode();
			}
			if (this.NewPassword != null)
			{
				num = num * 59 + this.NewPassword.GetHashCode();
			}
			return num;
		}
	}
}
