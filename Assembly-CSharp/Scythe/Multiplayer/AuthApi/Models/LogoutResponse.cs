using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x0200036E RID: 878
	[DataContract]
	public class LogoutResponse : IEquatable<LogoutResponse>
	{
		// Token: 0x0600195E RID: 6494 RVA: 0x00038FB0 File Offset: 0x000371B0
		[Preserve]
		public LogoutResponse(Result Result = Result.Success)
		{
			this.Result = Result;
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x0600195F RID: 6495 RVA: 0x00038FBF File Offset: 0x000371BF
		// (set) Token: 0x06001960 RID: 6496 RVA: 0x00038FC7 File Offset: 0x000371C7
		[DataMember(Name = "result", EmitDefaultValue = true)]
		public Result Result { get; set; }

		// Token: 0x06001961 RID: 6497 RVA: 0x000A2168 File Offset: 0x000A0368
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class LogoutResponse {\n");
			stringBuilder.Append("  Result: ").Append(this.Result).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x00038FD0 File Offset: 0x000371D0
		public override bool Equals(object input)
		{
			return this.Equals(input as LogoutResponse);
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x000A21C0 File Offset: 0x000A03C0
		public bool Equals(LogoutResponse input)
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

		// Token: 0x06001965 RID: 6501 RVA: 0x000A2208 File Offset: 0x000A0408
		public override int GetHashCode()
		{
			int num = 41;
			Result result = this.Result;
			return num * 59 + this.Result.GetHashCode();
		}
	}
}
