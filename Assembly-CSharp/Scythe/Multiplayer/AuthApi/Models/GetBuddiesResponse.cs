using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000364 RID: 868
	[DataContract]
	public class GetBuddiesResponse : IEquatable<GetBuddiesResponse>
	{
		// Token: 0x060018E9 RID: 6377 RVA: 0x00038C66 File Offset: 0x00036E66
		[Preserve]
		public GetBuddiesResponse(Result Result = Result.Success, List<BuddyDto> Buddies = null)
		{
			this.Result = Result;
			this.Buddies = Buddies;
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x060018EA RID: 6378 RVA: 0x00038C7C File Offset: 0x00036E7C
		// (set) Token: 0x060018EB RID: 6379 RVA: 0x00038C84 File Offset: 0x00036E84
		[DataMember(Name = "result", EmitDefaultValue = true)]
		public Result Result { get; set; }

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x060018EC RID: 6380 RVA: 0x00038C8D File Offset: 0x00036E8D
		// (set) Token: 0x060018ED RID: 6381 RVA: 0x00038C95 File Offset: 0x00036E95
		[DataMember(Name = "buddies", EmitDefaultValue = true)]
		public List<BuddyDto> Buddies { get; set; }

		// Token: 0x060018EE RID: 6382 RVA: 0x000A0F38 File Offset: 0x0009F138
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class GetBuddiesResponse {\n");
			stringBuilder.Append("  Result: ").Append(this.Result).Append("\n");
			stringBuilder.Append("  Buddies: ").Append(this.Buddies).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x060018EF RID: 6383 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x060018F0 RID: 6384 RVA: 0x00038C9E File Offset: 0x00036E9E
		public override bool Equals(object input)
		{
			return this.Equals(input as GetBuddiesResponse);
		}

		// Token: 0x060018F1 RID: 6385 RVA: 0x000A0FB0 File Offset: 0x0009F1B0
		public bool Equals(GetBuddiesResponse input)
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
			return this.Buddies == input.Buddies || (this.Buddies != null && this.Buddies.SequenceEqual(input.Buddies));
		}

		// Token: 0x060018F2 RID: 6386 RVA: 0x000A1028 File Offset: 0x0009F228
		public override int GetHashCode()
		{
			int num = 41;
			Result result = this.Result;
			num = num * 59 + this.Result.GetHashCode();
			if (this.Buddies != null)
			{
				num = num * 59 + this.Buddies.GetHashCode();
			}
			return num;
		}
	}
}
