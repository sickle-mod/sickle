using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000381 RID: 897
	[DataContract]
	public class UnauthorizedResponse : IEquatable<UnauthorizedResponse>
	{
		// Token: 0x060019FE RID: 6654 RVA: 0x000394DA File Offset: 0x000376DA
		[Preserve]
		public UnauthorizedResponse(Result Result = Result.Success)
		{
			this.Result = Result;
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x060019FF RID: 6655 RVA: 0x000394E9 File Offset: 0x000376E9
		// (set) Token: 0x06001A00 RID: 6656 RVA: 0x000394F1 File Offset: 0x000376F1
		[DataMember(Name = "result", EmitDefaultValue = true)]
		public Result Result { get; set; }

		// Token: 0x06001A01 RID: 6657 RVA: 0x000A32B0 File Offset: 0x000A14B0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class UnauthorizedResponse {\n");
			stringBuilder.Append("  Result: ").Append(this.Result).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x000394FA File Offset: 0x000376FA
		public override bool Equals(object input)
		{
			return this.Equals(input as UnauthorizedResponse);
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x000A3308 File Offset: 0x000A1508
		public bool Equals(UnauthorizedResponse input)
		{
			if (input == null)
			{
				return false;
			}
			if (this.Result != input.Result)
			{
				Result result = this.Result;
				return this.Result.Equals(input.Result);
			}
			return true;
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x000A3350 File Offset: 0x000A1550
		public override int GetHashCode()
		{
			int num = 41;
			Result result = this.Result;
			return num * 59 + this.Result.GetHashCode();
		}
	}
}
