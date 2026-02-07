using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x0200037C RID: 892
	[DataContract]
	public class ResetPasswordResponse : IEquatable<ResetPasswordResponse>
	{
		// Token: 0x060019DE RID: 6622 RVA: 0x000393F2 File Offset: 0x000375F2
		[Preserve]
		public ResetPasswordResponse(Result Result = Result.Success, string Token = null)
		{
			this.Result = Result;
			this.Token = Token;
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x060019DF RID: 6623 RVA: 0x00039408 File Offset: 0x00037608
		// (set) Token: 0x060019E0 RID: 6624 RVA: 0x00039410 File Offset: 0x00037610
		[DataMember(Name = "result", EmitDefaultValue = true)]
		public Result Result { get; set; }

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x060019E1 RID: 6625 RVA: 0x00039419 File Offset: 0x00037619
		// (set) Token: 0x060019E2 RID: 6626 RVA: 0x00039421 File Offset: 0x00037621
		[DataMember(Name = "token", EmitDefaultValue = true)]
		public string Token { get; set; }

		// Token: 0x060019E3 RID: 6627 RVA: 0x000A2F68 File Offset: 0x000A1168
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class ResetPasswordResponse {\n");
			stringBuilder.Append("  Result: ").Append(this.Result).Append("\n");
			stringBuilder.Append("  Token: ").Append(this.Token).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x0003942A File Offset: 0x0003762A
		public override bool Equals(object input)
		{
			return this.Equals(input as ResetPasswordResponse);
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x000A2FE0 File Offset: 0x000A11E0
		public bool Equals(ResetPasswordResponse input)
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

		// Token: 0x060019E7 RID: 6631 RVA: 0x000A305C File Offset: 0x000A125C
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
