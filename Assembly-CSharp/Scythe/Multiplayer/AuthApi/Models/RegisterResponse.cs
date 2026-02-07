using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000370 RID: 880
	[DataContract]
	public class RegisterResponse : IEquatable<RegisterResponse>
	{
		// Token: 0x06001972 RID: 6514 RVA: 0x0003901F File Offset: 0x0003721F
		[Preserve]
		public RegisterResponse(Result Result = Result.Success, string ActivationToken = null)
		{
			this.Result = Result;
			this.ActivationToken = ActivationToken;
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06001973 RID: 6515 RVA: 0x00039035 File Offset: 0x00037235
		// (set) Token: 0x06001974 RID: 6516 RVA: 0x0003903D File Offset: 0x0003723D
		[DataMember(Name = "result", EmitDefaultValue = true)]
		public Result Result { get; set; }

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06001975 RID: 6517 RVA: 0x00039046 File Offset: 0x00037246
		// (set) Token: 0x06001976 RID: 6518 RVA: 0x0003904E File Offset: 0x0003724E
		[DataMember(Name = "activationToken", EmitDefaultValue = true)]
		public string ActivationToken { get; set; }

		// Token: 0x06001977 RID: 6519 RVA: 0x000A2420 File Offset: 0x000A0620
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class RegisterResponse {\n");
			stringBuilder.Append("  Result: ").Append(this.Result).Append("\n");
			stringBuilder.Append("  ActivationToken: ").Append(this.ActivationToken).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x00039057 File Offset: 0x00037257
		public override bool Equals(object input)
		{
			return this.Equals(input as RegisterResponse);
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x000A2498 File Offset: 0x000A0698
		public bool Equals(RegisterResponse input)
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
			return this.ActivationToken == input.ActivationToken || (this.ActivationToken != null && this.ActivationToken.Equals(input.ActivationToken));
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x000A2514 File Offset: 0x000A0714
		public override int GetHashCode()
		{
			int num = 41;
			Result result = this.Result;
			num = num * 59 + this.Result.GetHashCode();
			if (this.ActivationToken != null)
			{
				num = num * 59 + this.ActivationToken.GetHashCode();
			}
			return num;
		}
	}
}
