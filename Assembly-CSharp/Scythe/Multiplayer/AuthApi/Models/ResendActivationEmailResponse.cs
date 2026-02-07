using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000374 RID: 884
	[DataContract]
	public class ResendActivationEmailResponse : IEquatable<ResendActivationEmailResponse>
	{
		// Token: 0x06001996 RID: 6550 RVA: 0x00039160 File Offset: 0x00037360
		[Preserve]
		public ResendActivationEmailResponse(Result Result = Result.Success, string Token = null)
		{
			this.Result = Result;
			this.Token = Token;
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06001997 RID: 6551 RVA: 0x00039176 File Offset: 0x00037376
		// (set) Token: 0x06001998 RID: 6552 RVA: 0x0003917E File Offset: 0x0003737E
		[DataMember(Name = "result", EmitDefaultValue = true)]
		public Result Result { get; set; }

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06001999 RID: 6553 RVA: 0x00039187 File Offset: 0x00037387
		// (set) Token: 0x0600199A RID: 6554 RVA: 0x0003918F File Offset: 0x0003738F
		[DataMember(Name = "token", EmitDefaultValue = true)]
		public string Token { get; set; }

		// Token: 0x0600199B RID: 6555 RVA: 0x000A2844 File Offset: 0x000A0A44
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class ResendActivationEmailResponse {\n");
			stringBuilder.Append("  Result: ").Append(this.Result).Append("\n");
			stringBuilder.Append("  Token: ").Append(this.Token).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x00039198 File Offset: 0x00037398
		public override bool Equals(object input)
		{
			return this.Equals(input as ResendActivationEmailResponse);
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x000A28BC File Offset: 0x000A0ABC
		public bool Equals(ResendActivationEmailResponse input)
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

		// Token: 0x0600199F RID: 6559 RVA: 0x000A2938 File Offset: 0x000A0B38
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
