using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x0200035F RID: 863
	[DataContract]
	public class DeleteAccountRequest : IEquatable<DeleteAccountRequest>
	{
		// Token: 0x060018C7 RID: 6343 RVA: 0x00038B2C File Offset: 0x00036D2C
		[Preserve]
		public DeleteAccountRequest(string Password = null)
		{
			if (Password == null)
			{
				throw new InvalidDataException("Password is a required property for DeleteAccountRequest and cannot be null");
			}
			this.Password = Password;
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x060018C8 RID: 6344 RVA: 0x00038B49 File Offset: 0x00036D49
		// (set) Token: 0x060018C9 RID: 6345 RVA: 0x00038B51 File Offset: 0x00036D51
		[DataMember(Name = "password", EmitDefaultValue = true)]
		public string Password { get; set; }

		// Token: 0x060018CA RID: 6346 RVA: 0x000A0C34 File Offset: 0x0009EE34
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class DeleteAccountRequest {\n");
			stringBuilder.Append("  Password: ").Append(this.Password).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x060018CC RID: 6348 RVA: 0x00038B5A File Offset: 0x00036D5A
		public override bool Equals(object input)
		{
			return this.Equals(input as DeleteAccountRequest);
		}

		// Token: 0x060018CD RID: 6349 RVA: 0x00038B68 File Offset: 0x00036D68
		public bool Equals(DeleteAccountRequest input)
		{
			return input != null && (this.Password == input.Password || (this.Password != null && this.Password.Equals(input.Password)));
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x000A0C84 File Offset: 0x0009EE84
		public override int GetHashCode()
		{
			int num = 41;
			if (this.Password != null)
			{
				num = num * 59 + this.Password.GetHashCode();
			}
			return num;
		}
	}
}
