using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000367 RID: 871
	[DataContract]
	public class LinkAccountResponse : IEquatable<LinkAccountResponse>
	{
		// Token: 0x06001907 RID: 6407 RVA: 0x00038D46 File Offset: 0x00036F46
		[Preserve]
		public LinkAccountResponse(Result Result = Result.Success)
		{
			this.Result = Result;
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06001908 RID: 6408 RVA: 0x00038D55 File Offset: 0x00036F55
		// (set) Token: 0x06001909 RID: 6409 RVA: 0x00038D5D File Offset: 0x00036F5D
		[DataMember(Name = "result", EmitDefaultValue = true)]
		public Result Result { get; set; }

		// Token: 0x0600190A RID: 6410 RVA: 0x000A12DC File Offset: 0x0009F4DC
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class LinkAccountResponse {\n");
			stringBuilder.Append("  Result: ").Append(this.Result).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x00038D66 File Offset: 0x00036F66
		public override bool Equals(object input)
		{
			return this.Equals(input as LinkAccountResponse);
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x000A1334 File Offset: 0x0009F534
		public bool Equals(LinkAccountResponse input)
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

		// Token: 0x0600190E RID: 6414 RVA: 0x000A137C File Offset: 0x0009F57C
		public override int GetHashCode()
		{
			int num = 41;
			Result result = this.Result;
			return num * 59 + this.Result.GetHashCode();
		}
	}
}
