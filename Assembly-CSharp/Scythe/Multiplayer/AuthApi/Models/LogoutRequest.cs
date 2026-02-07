using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x0200036D RID: 877
	[DataContract]
	public class LogoutRequest : IEquatable<LogoutRequest>
	{
		// Token: 0x06001958 RID: 6488 RVA: 0x00027E56 File Offset: 0x00026056
		[Preserve]
		public LogoutRequest()
		{
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x00038F7E File Offset: 0x0003717E
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class LogoutRequest {\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x0600195A RID: 6490 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x00038FA2 File Offset: 0x000371A2
		public override bool Equals(object input)
		{
			return this.Equals(input as LogoutRequest);
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x00038C5D File Offset: 0x00036E5D
		public bool Equals(LogoutRequest input)
		{
			return false;
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x00038C62 File Offset: 0x00036E62
		public override int GetHashCode()
		{
			return 41;
		}
	}
}
