using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000373 RID: 883
	[DataContract]
	public class ResendActivationEmailRequest : IEquatable<ResendActivationEmailRequest>
	{
		// Token: 0x0600198E RID: 6542 RVA: 0x000390ED File Offset: 0x000372ED
		[Preserve]
		public ResendActivationEmailRequest(string Login = null)
		{
			if (Login == null)
			{
				throw new InvalidDataException("Login is a required property for ResendActivationEmailRequest and cannot be null");
			}
			this.Login = Login;
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x0600198F RID: 6543 RVA: 0x0003910A File Offset: 0x0003730A
		// (set) Token: 0x06001990 RID: 6544 RVA: 0x00039112 File Offset: 0x00037312
		[DataMember(Name = "login", EmitDefaultValue = true)]
		public string Login { get; set; }

		// Token: 0x06001991 RID: 6545 RVA: 0x000A27C8 File Offset: 0x000A09C8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class ResendActivationEmailRequest {\n");
			stringBuilder.Append("  Login: ").Append(this.Login).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x0003911B File Offset: 0x0003731B
		public override bool Equals(object input)
		{
			return this.Equals(input as ResendActivationEmailRequest);
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x00039129 File Offset: 0x00037329
		public bool Equals(ResendActivationEmailRequest input)
		{
			return input != null && (this.Login == input.Login || (this.Login != null && this.Login.Equals(input.Login)));
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x000A2818 File Offset: 0x000A0A18
		public override int GetHashCode()
		{
			int num = 41;
			if (this.Login != null)
			{
				num = num * 59 + this.Login.GetHashCode();
			}
			return num;
		}
	}
}
