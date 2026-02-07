using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000362 RID: 866
	[DataContract]
	public class ErrorResponse : IEquatable<ErrorResponse>
	{
		// Token: 0x060018D7 RID: 6359 RVA: 0x00038BCD File Offset: 0x00036DCD
		[Preserve]
		public ErrorResponse(Result Result = Result.Success, Error Error = Error.None, List<string> ValidationErrors = null)
		{
			this.Result = Result;
			this.Error = Error;
			this.ValidationErrors = ValidationErrors;
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060018D8 RID: 6360 RVA: 0x00038BEA File Offset: 0x00036DEA
		// (set) Token: 0x060018D9 RID: 6361 RVA: 0x00038BF2 File Offset: 0x00036DF2
		[DataMember(Name = "result", EmitDefaultValue = true)]
		public Result Result { get; set; }

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060018DA RID: 6362 RVA: 0x00038BFB File Offset: 0x00036DFB
		// (set) Token: 0x060018DB RID: 6363 RVA: 0x00038C03 File Offset: 0x00036E03
		[DataMember(Name = "error", EmitDefaultValue = true)]
		public Error Error { get; set; }

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x060018DC RID: 6364 RVA: 0x00038C0C File Offset: 0x00036E0C
		// (set) Token: 0x060018DD RID: 6365 RVA: 0x00038C14 File Offset: 0x00036E14
		[DataMember(Name = "validationErrors", EmitDefaultValue = true)]
		public List<string> ValidationErrors { get; set; }

		// Token: 0x060018DE RID: 6366 RVA: 0x000A0D84 File Offset: 0x0009EF84
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class ErrorResponse {\n");
			stringBuilder.Append("  Result: ").Append(this.Result).Append("\n");
			stringBuilder.Append("  Error: ").Append(this.Error).Append("\n");
			stringBuilder.Append("  ValidationErrors: ").Append(this.ValidationErrors).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x060018DF RID: 6367 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x060018E0 RID: 6368 RVA: 0x00038C1D File Offset: 0x00036E1D
		public override bool Equals(object input)
		{
			return this.Equals(input as ErrorResponse);
		}

		// Token: 0x060018E1 RID: 6369 RVA: 0x000A0E20 File Offset: 0x0009F020
		public bool Equals(ErrorResponse input)
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
			if (this.Error != input.Error)
			{
				Error error = this.Error;
				if (!this.Error.Equals(input.Error))
				{
					return false;
				}
			}
			return this.ValidationErrors == input.ValidationErrors || (this.ValidationErrors != null && this.ValidationErrors.SequenceEqual(input.ValidationErrors));
		}

		// Token: 0x060018E2 RID: 6370 RVA: 0x000A0ECC File Offset: 0x0009F0CC
		public override int GetHashCode()
		{
			int num = 41;
			Result result = this.Result;
			num = num * 59 + this.Result.GetHashCode();
			Error error = this.Error;
			num = num * 59 + this.Error.GetHashCode();
			if (this.ValidationErrors != null)
			{
				num = num * 59 + this.ValidationErrors.GetHashCode();
			}
			return num;
		}
	}
}
