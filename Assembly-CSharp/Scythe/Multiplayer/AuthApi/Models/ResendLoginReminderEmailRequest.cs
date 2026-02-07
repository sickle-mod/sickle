using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000377 RID: 887
	[DataContract]
	public class ResendLoginReminderEmailRequest : IEquatable<ResendLoginReminderEmailRequest>
	{
		// Token: 0x060019B0 RID: 6576 RVA: 0x0003921E File Offset: 0x0003741E
		[Preserve]
		public ResendLoginReminderEmailRequest(string Email = null)
		{
			if (Email == null)
			{
				throw new InvalidDataException("Email is a required property for ResendLoginReminderEmailRequest and cannot be null");
			}
			this.Email = Email;
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060019B1 RID: 6577 RVA: 0x0003923B File Offset: 0x0003743B
		// (set) Token: 0x060019B2 RID: 6578 RVA: 0x00039243 File Offset: 0x00037443
		[DataMember(Name = "email", EmitDefaultValue = true)]
		public string Email { get; set; }

		// Token: 0x060019B3 RID: 6579 RVA: 0x000A2AC4 File Offset: 0x000A0CC4
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class ResendLoginReminderEmailRequest {\n");
			stringBuilder.Append("  Email: ").Append(this.Email).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x0003924C File Offset: 0x0003744C
		public override bool Equals(object input)
		{
			return this.Equals(input as ResendLoginReminderEmailRequest);
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x0003925A File Offset: 0x0003745A
		public bool Equals(ResendLoginReminderEmailRequest input)
		{
			return input != null && (this.Email == input.Email || (this.Email != null && this.Email.Equals(input.Email)));
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x000A2B14 File Offset: 0x000A0D14
		public override int GetHashCode()
		{
			int num = 41;
			if (this.Email != null)
			{
				num = num * 59 + this.Email.GetHashCode();
			}
			return num;
		}
	}
}
