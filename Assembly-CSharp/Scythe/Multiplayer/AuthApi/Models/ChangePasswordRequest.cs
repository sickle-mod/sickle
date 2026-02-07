using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x0200035D RID: 861
	[DataContract]
	public class ChangePasswordRequest : IEquatable<ChangePasswordRequest>
	{
		// Token: 0x060018B5 RID: 6325 RVA: 0x00038A9C File Offset: 0x00036C9C
		[Preserve]
		public ChangePasswordRequest(string OldPassword = null, string NewPassword = null)
		{
			if (OldPassword == null)
			{
				throw new InvalidDataException("OldPassword is a required property for ChangePasswordRequest and cannot be null");
			}
			this.OldPassword = OldPassword;
			if (NewPassword == null)
			{
				throw new InvalidDataException("NewPassword is a required property for ChangePasswordRequest and cannot be null");
			}
			this.NewPassword = NewPassword;
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x060018B6 RID: 6326 RVA: 0x00038ACE File Offset: 0x00036CCE
		// (set) Token: 0x060018B7 RID: 6327 RVA: 0x00038AD6 File Offset: 0x00036CD6
		[DataMember(Name = "oldPassword", EmitDefaultValue = true)]
		public string OldPassword { get; set; }

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x060018B8 RID: 6328 RVA: 0x00038ADF File Offset: 0x00036CDF
		// (set) Token: 0x060018B9 RID: 6329 RVA: 0x00038AE7 File Offset: 0x00036CE7
		[DataMember(Name = "newPassword", EmitDefaultValue = true)]
		public string NewPassword { get; set; }

		// Token: 0x060018BA RID: 6330 RVA: 0x000A0A34 File Offset: 0x0009EC34
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class ChangePasswordRequest {\n");
			stringBuilder.Append("  OldPassword: ").Append(this.OldPassword).Append("\n");
			stringBuilder.Append("  NewPassword: ").Append(this.NewPassword).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x060018BC RID: 6332 RVA: 0x00038AF0 File Offset: 0x00036CF0
		public override bool Equals(object input)
		{
			return this.Equals(input as ChangePasswordRequest);
		}

		// Token: 0x060018BD RID: 6333 RVA: 0x000A0AA8 File Offset: 0x0009ECA8
		public bool Equals(ChangePasswordRequest input)
		{
			return input != null && (this.OldPassword == input.OldPassword || (this.OldPassword != null && this.OldPassword.Equals(input.OldPassword))) && (this.NewPassword == input.NewPassword || (this.NewPassword != null && this.NewPassword.Equals(input.NewPassword)));
		}

		// Token: 0x060018BE RID: 6334 RVA: 0x000A0B1C File Offset: 0x0009ED1C
		public override int GetHashCode()
		{
			int num = 41;
			if (this.OldPassword != null)
			{
				num = num * 59 + this.OldPassword.GetHashCode();
			}
			if (this.NewPassword != null)
			{
				num = num * 59 + this.NewPassword.GetHashCode();
			}
			return num;
		}
	}
}
