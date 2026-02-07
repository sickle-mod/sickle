using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000360 RID: 864
	[DataContract]
	public class DeleteAccountResponse : IEquatable<DeleteAccountResponse>
	{
		// Token: 0x060018CF RID: 6351 RVA: 0x00038B9F File Offset: 0x00036D9F
		[Preserve]
		public DeleteAccountResponse(Result Result = Result.Success)
		{
			this.Result = Result;
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060018D0 RID: 6352 RVA: 0x00038BAE File Offset: 0x00036DAE
		// (set) Token: 0x060018D1 RID: 6353 RVA: 0x00038BB6 File Offset: 0x00036DB6
		[DataMember(Name = "result", EmitDefaultValue = true)]
		public Result Result { get; set; }

		// Token: 0x060018D2 RID: 6354 RVA: 0x000A0CB0 File Offset: 0x0009EEB0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class DeleteAccountResponse {\n");
			stringBuilder.Append("  Result: ").Append(this.Result).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x00038BBF File Offset: 0x00036DBF
		public override bool Equals(object input)
		{
			return this.Equals(input as DeleteAccountResponse);
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x000A0D08 File Offset: 0x0009EF08
		public bool Equals(DeleteAccountResponse input)
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

		// Token: 0x060018D6 RID: 6358 RVA: 0x000A0D50 File Offset: 0x0009EF50
		public override int GetHashCode()
		{
			int num = 41;
			Result result = this.Result;
			return num * 59 + this.Result.GetHashCode();
		}
	}
}
