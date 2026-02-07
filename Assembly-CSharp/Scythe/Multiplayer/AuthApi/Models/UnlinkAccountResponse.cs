using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000383 RID: 899
	[DataContract]
	public class UnlinkAccountResponse : IEquatable<UnlinkAccountResponse>
	{
		// Token: 0x06001A0E RID: 6670 RVA: 0x00039536 File Offset: 0x00037736
		[Preserve]
		public UnlinkAccountResponse(Result Result = Result.Success)
		{
			this.Result = Result;
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06001A0F RID: 6671 RVA: 0x00039545 File Offset: 0x00037745
		// (set) Token: 0x06001A10 RID: 6672 RVA: 0x0003954D File Offset: 0x0003774D
		[DataMember(Name = "result", EmitDefaultValue = true)]
		public Result Result { get; set; }

		// Token: 0x06001A11 RID: 6673 RVA: 0x000A3458 File Offset: 0x000A1658
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class UnlinkAccountResponse {\n");
			stringBuilder.Append("  Result: ").Append(this.Result).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x00039556 File Offset: 0x00037756
		public override bool Equals(object input)
		{
			return this.Equals(input as UnlinkAccountResponse);
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x000A34B0 File Offset: 0x000A16B0
		public bool Equals(UnlinkAccountResponse input)
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

		// Token: 0x06001A15 RID: 6677 RVA: 0x000A34F8 File Offset: 0x000A16F8
		public override int GetHashCode()
		{
			int num = 41;
			Result result = this.Result;
			return num * 59 + this.Result.GetHashCode();
		}
	}
}
