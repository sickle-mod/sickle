using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x0200037A RID: 890
	[DataContract]
	public class ResendPasswordResetEmailResponse : IEquatable<ResendPasswordResetEmailResponse>
	{
		// Token: 0x060019CA RID: 6602 RVA: 0x0003934A File Offset: 0x0003754A
		[Preserve]
		public ResendPasswordResetEmailResponse(Result Result = Result.Success, string Token = null)
		{
			this.Result = Result;
			this.Token = Token;
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x060019CB RID: 6603 RVA: 0x00039360 File Offset: 0x00037560
		// (set) Token: 0x060019CC RID: 6604 RVA: 0x00039368 File Offset: 0x00037568
		[DataMember(Name = "result", EmitDefaultValue = true)]
		public Result Result { get; set; }

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x060019CD RID: 6605 RVA: 0x00039371 File Offset: 0x00037571
		// (set) Token: 0x060019CE RID: 6606 RVA: 0x00039379 File Offset: 0x00037579
		[DataMember(Name = "token", EmitDefaultValue = true)]
		public string Token { get; set; }

		// Token: 0x060019CF RID: 6607 RVA: 0x000A2CFC File Offset: 0x000A0EFC
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class ResendPasswordResetEmailResponse {\n");
			stringBuilder.Append("  Result: ").Append(this.Result).Append("\n");
			stringBuilder.Append("  Token: ").Append(this.Token).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x060019D0 RID: 6608 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x060019D1 RID: 6609 RVA: 0x00039382 File Offset: 0x00037582
		public override bool Equals(object input)
		{
			return this.Equals(input as ResendPasswordResetEmailResponse);
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x000A2D74 File Offset: 0x000A0F74
		public bool Equals(ResendPasswordResetEmailResponse input)
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

		// Token: 0x060019D3 RID: 6611 RVA: 0x000A2DF0 File Offset: 0x000A0FF0
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
