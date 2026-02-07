using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000378 RID: 888
	[DataContract]
	public class ResendLoginReminderEmailResponse : IEquatable<ResendLoginReminderEmailResponse>
	{
		// Token: 0x060019B8 RID: 6584 RVA: 0x00039291 File Offset: 0x00037491
		[Preserve]
		public ResendLoginReminderEmailResponse(Result Result = Result.Success, string Login = null)
		{
			this.Result = Result;
			this.Login = Login;
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060019B9 RID: 6585 RVA: 0x000392A7 File Offset: 0x000374A7
		// (set) Token: 0x060019BA RID: 6586 RVA: 0x000392AF File Offset: 0x000374AF
		[DataMember(Name = "result", EmitDefaultValue = true)]
		public Result Result { get; set; }

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060019BB RID: 6587 RVA: 0x000392B8 File Offset: 0x000374B8
		// (set) Token: 0x060019BC RID: 6588 RVA: 0x000392C0 File Offset: 0x000374C0
		[DataMember(Name = "login", EmitDefaultValue = true)]
		public string Login { get; set; }

		// Token: 0x060019BD RID: 6589 RVA: 0x000A2B40 File Offset: 0x000A0D40
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class ResendLoginReminderEmailResponse {\n");
			stringBuilder.Append("  Result: ").Append(this.Result).Append("\n");
			stringBuilder.Append("  Login: ").Append(this.Login).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x060019BF RID: 6591 RVA: 0x000392C9 File Offset: 0x000374C9
		public override bool Equals(object input)
		{
			return this.Equals(input as ResendLoginReminderEmailResponse);
		}

		// Token: 0x060019C0 RID: 6592 RVA: 0x000A2BB8 File Offset: 0x000A0DB8
		public bool Equals(ResendLoginReminderEmailResponse input)
		{
			if (input == null)
			{
				return false;
			}
			if (this.Result != input.Result)
			{
				Result result = this.Result;
				if (!this.Result.Equals(input.Result))
				{
					return false;
				}
			}
			return this.Login == input.Login || (this.Login != null && this.Login.Equals(input.Login));
		}

		// Token: 0x060019C1 RID: 6593 RVA: 0x000A2C34 File Offset: 0x000A0E34
		public override int GetHashCode()
		{
			int num = 41;
			Result result = this.Result;
			num = num * 59 + this.Result.GetHashCode();
			if (this.Login != null)
			{
				num = num * 59 + this.Login.GetHashCode();
			}
			return num;
		}
	}
}
