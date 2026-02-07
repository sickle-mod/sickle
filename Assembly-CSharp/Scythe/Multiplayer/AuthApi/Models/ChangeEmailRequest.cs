using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x0200035B RID: 859
	[DataContract]
	public class ChangeEmailRequest : IEquatable<ChangeEmailRequest>
	{
		// Token: 0x060018A1 RID: 6305 RVA: 0x000389F4 File Offset: 0x00036BF4
		[Preserve]
		public ChangeEmailRequest(string NewEmail = null, string Password = null)
		{
			if (NewEmail == null)
			{
				throw new InvalidDataException("NewEmail is a required property for ChangeEmailRequest and cannot be null");
			}
			this.NewEmail = NewEmail;
			if (Password == null)
			{
				throw new InvalidDataException("Password is a required property for ChangeEmailRequest and cannot be null");
			}
			this.Password = Password;
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060018A2 RID: 6306 RVA: 0x00038A26 File Offset: 0x00036C26
		// (set) Token: 0x060018A3 RID: 6307 RVA: 0x00038A2E File Offset: 0x00036C2E
		[DataMember(Name = "newEmail", EmitDefaultValue = true)]
		public string NewEmail { get; set; }

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x060018A4 RID: 6308 RVA: 0x00038A37 File Offset: 0x00036C37
		// (set) Token: 0x060018A5 RID: 6309 RVA: 0x00038A3F File Offset: 0x00036C3F
		[DataMember(Name = "password", EmitDefaultValue = true)]
		public string Password { get; set; }

		// Token: 0x060018A6 RID: 6310 RVA: 0x000A07C8 File Offset: 0x0009E9C8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class ChangeEmailRequest {\n");
			stringBuilder.Append("  NewEmail: ").Append(this.NewEmail).Append("\n");
			stringBuilder.Append("  Password: ").Append(this.Password).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x060018A8 RID: 6312 RVA: 0x00038A48 File Offset: 0x00036C48
		public override bool Equals(object input)
		{
			return this.Equals(input as ChangeEmailRequest);
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x000A083C File Offset: 0x0009EA3C
		public bool Equals(ChangeEmailRequest input)
		{
			return input != null && (this.NewEmail == input.NewEmail || (this.NewEmail != null && this.NewEmail.Equals(input.NewEmail))) && (this.Password == input.Password || (this.Password != null && this.Password.Equals(input.Password)));
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x000A08B0 File Offset: 0x0009EAB0
		public override int GetHashCode()
		{
			int num = 41;
			if (this.NewEmail != null)
			{
				num = num * 59 + this.NewEmail.GetHashCode();
			}
			if (this.Password != null)
			{
				num = num * 59 + this.Password.GetHashCode();
			}
			return num;
		}
	}
}
