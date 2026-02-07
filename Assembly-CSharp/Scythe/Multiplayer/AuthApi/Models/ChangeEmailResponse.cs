using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x0200035C RID: 860
	[DataContract]
	public class ChangeEmailResponse : IEquatable<ChangeEmailResponse>
	{
		// Token: 0x060018AB RID: 6315 RVA: 0x00038A56 File Offset: 0x00036C56
		[Preserve]
		public ChangeEmailResponse(Result Result = Result.Success, string Token = null)
		{
			this.Result = Result;
			this.Token = Token;
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x060018AC RID: 6316 RVA: 0x00038A6C File Offset: 0x00036C6C
		// (set) Token: 0x060018AD RID: 6317 RVA: 0x00038A74 File Offset: 0x00036C74
		[DataMember(Name = "result", EmitDefaultValue = true)]
		public Result Result { get; set; }

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x060018AE RID: 6318 RVA: 0x00038A7D File Offset: 0x00036C7D
		// (set) Token: 0x060018AF RID: 6319 RVA: 0x00038A85 File Offset: 0x00036C85
		[DataMember(Name = "token", EmitDefaultValue = true)]
		public string Token { get; set; }

		// Token: 0x060018B0 RID: 6320 RVA: 0x000A08F4 File Offset: 0x0009EAF4
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class ChangeEmailResponse {\n");
			stringBuilder.Append("  Result: ").Append(this.Result).Append("\n");
			stringBuilder.Append("  Token: ").Append(this.Token).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x060018B1 RID: 6321 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x00038A8E File Offset: 0x00036C8E
		public override bool Equals(object input)
		{
			return this.Equals(input as ChangeEmailResponse);
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x000A096C File Offset: 0x0009EB6C
		public bool Equals(ChangeEmailResponse input)
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

		// Token: 0x060018B4 RID: 6324 RVA: 0x000A09E8 File Offset: 0x0009EBE8
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
