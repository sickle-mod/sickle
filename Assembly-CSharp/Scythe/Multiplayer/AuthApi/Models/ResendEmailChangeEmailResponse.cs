using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000376 RID: 886
	[DataContract]
	public class ResendEmailChangeEmailResponse : IEquatable<ResendEmailChangeEmailResponse>
	{
		// Token: 0x060019A6 RID: 6566 RVA: 0x000391D8 File Offset: 0x000373D8
		[Preserve]
		public ResendEmailChangeEmailResponse(Result Result = Result.Success, string Token = null)
		{
			this.Result = Result;
			this.Token = Token;
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x060019A7 RID: 6567 RVA: 0x000391EE File Offset: 0x000373EE
		// (set) Token: 0x060019A8 RID: 6568 RVA: 0x000391F6 File Offset: 0x000373F6
		[DataMember(Name = "result", EmitDefaultValue = true)]
		public Result Result { get; set; }

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060019A9 RID: 6569 RVA: 0x000391FF File Offset: 0x000373FF
		// (set) Token: 0x060019AA RID: 6570 RVA: 0x00039207 File Offset: 0x00037407
		[DataMember(Name = "token", EmitDefaultValue = true)]
		public string Token { get; set; }

		// Token: 0x060019AB RID: 6571 RVA: 0x000A2984 File Offset: 0x000A0B84
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class ResendEmailChangeEmailResponse {\n");
			stringBuilder.Append("  Result: ").Append(this.Result).Append("\n");
			stringBuilder.Append("  Token: ").Append(this.Token).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x00039210 File Offset: 0x00037410
		public override bool Equals(object input)
		{
			return this.Equals(input as ResendEmailChangeEmailResponse);
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x000A29FC File Offset: 0x000A0BFC
		public bool Equals(ResendEmailChangeEmailResponse input)
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
			return this.Token == input.Token || (this.Token != null && this.Token.Equals(input.Token));
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x000A2A78 File Offset: 0x000A0C78
		public override int GetHashCode()
		{
			int num = 41;
			Result result = this.Result;
			num = num * 59 + this.Result.GetHashCode();
			if (this.Token != null)
			{
				num = num * 59 + this.Token.GetHashCode();
			}
			return num;
		}
	}
}
