using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x0200035E RID: 862
	[DataContract]
	public class ChangePasswordResponse : IEquatable<ChangePasswordResponse>
	{
		// Token: 0x060018BF RID: 6335 RVA: 0x00038AFE File Offset: 0x00036CFE
		[Preserve]
		public ChangePasswordResponse(Result Result = Result.Success)
		{
			this.Result = Result;
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x060018C0 RID: 6336 RVA: 0x00038B0D File Offset: 0x00036D0D
		// (set) Token: 0x060018C1 RID: 6337 RVA: 0x00038B15 File Offset: 0x00036D15
		[DataMember(Name = "result", EmitDefaultValue = true)]
		public Result Result { get; set; }

		// Token: 0x060018C2 RID: 6338 RVA: 0x000A0B60 File Offset: 0x0009ED60
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class ChangePasswordResponse {\n");
			stringBuilder.Append("  Result: ").Append(this.Result).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x060018C3 RID: 6339 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x060018C4 RID: 6340 RVA: 0x00038B1E File Offset: 0x00036D1E
		public override bool Equals(object input)
		{
			return this.Equals(input as ChangePasswordResponse);
		}

		// Token: 0x060018C5 RID: 6341 RVA: 0x000A0BB8 File Offset: 0x0009EDB8
		public bool Equals(ChangePasswordResponse input)
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

		// Token: 0x060018C6 RID: 6342 RVA: 0x000A0C00 File Offset: 0x0009EE00
		public override int GetHashCode()
		{
			int num = 41;
			Result result = this.Result;
			return num * 59 + this.Result.GetHashCode();
		}
	}
}
