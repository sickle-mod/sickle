using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000380 RID: 896
	[DataContract]
	public class ServerErrorResponse : IEquatable<ServerErrorResponse>
	{
		// Token: 0x060019F6 RID: 6646 RVA: 0x000394AC File Offset: 0x000376AC
		[Preserve]
		public ServerErrorResponse(Result Result = Result.Success)
		{
			this.Result = Result;
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x060019F7 RID: 6647 RVA: 0x000394BB File Offset: 0x000376BB
		// (set) Token: 0x060019F8 RID: 6648 RVA: 0x000394C3 File Offset: 0x000376C3
		[DataMember(Name = "result", EmitDefaultValue = true)]
		public Result Result { get; set; }

		// Token: 0x060019F9 RID: 6649 RVA: 0x000A31DC File Offset: 0x000A13DC
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class ServerErrorResponse {\n");
			stringBuilder.Append("  Result: ").Append(this.Result).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x000394CC File Offset: 0x000376CC
		public override bool Equals(object input)
		{
			return this.Equals(input as ServerErrorResponse);
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x000A3234 File Offset: 0x000A1434
		public bool Equals(ServerErrorResponse input)
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

		// Token: 0x060019FD RID: 6653 RVA: 0x000A327C File Offset: 0x000A147C
		public override int GetHashCode()
		{
			int num = 41;
			Result result = this.Result;
			return num * 59 + this.Result.GetHashCode();
		}
	}
}
