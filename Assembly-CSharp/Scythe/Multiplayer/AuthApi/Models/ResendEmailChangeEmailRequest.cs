using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000375 RID: 885
	[DataContract]
	public class ResendEmailChangeEmailRequest : IEquatable<ResendEmailChangeEmailRequest>
	{
		// Token: 0x060019A0 RID: 6560 RVA: 0x00027E56 File Offset: 0x00026056
		[Preserve]
		public ResendEmailChangeEmailRequest()
		{
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x000391A6 File Offset: 0x000373A6
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class ResendEmailChangeEmailRequest {\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x000391CA File Offset: 0x000373CA
		public override bool Equals(object input)
		{
			return this.Equals(input as ResendEmailChangeEmailRequest);
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x00038C5D File Offset: 0x00036E5D
		public bool Equals(ResendEmailChangeEmailRequest input)
		{
			return false;
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x00038C62 File Offset: 0x00036E62
		public override int GetHashCode()
		{
			return 41;
		}
	}
}
