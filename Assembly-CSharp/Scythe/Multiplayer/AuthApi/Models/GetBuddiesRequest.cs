using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000363 RID: 867
	[DataContract]
	public class GetBuddiesRequest : IEquatable<GetBuddiesRequest>
	{
		// Token: 0x060018E3 RID: 6371 RVA: 0x00027E56 File Offset: 0x00026056
		[Preserve]
		public GetBuddiesRequest()
		{
		}

		// Token: 0x060018E4 RID: 6372 RVA: 0x00038C2B File Offset: 0x00036E2B
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class GetBuddiesRequest {\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x060018E5 RID: 6373 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x060018E6 RID: 6374 RVA: 0x00038C4F File Offset: 0x00036E4F
		public override bool Equals(object input)
		{
			return this.Equals(input as GetBuddiesRequest);
		}

		// Token: 0x060018E7 RID: 6375 RVA: 0x00038C5D File Offset: 0x00036E5D
		public bool Equals(GetBuddiesRequest input)
		{
			return false;
		}

		// Token: 0x060018E8 RID: 6376 RVA: 0x00038C62 File Offset: 0x00036E62
		public override int GetHashCode()
		{
			return 41;
		}
	}
}
