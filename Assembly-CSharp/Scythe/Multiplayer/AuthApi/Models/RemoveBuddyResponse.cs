using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000372 RID: 882
	[DataContract]
	public class RemoveBuddyResponse : IEquatable<RemoveBuddyResponse>
	{
		// Token: 0x06001984 RID: 6532 RVA: 0x000390A7 File Offset: 0x000372A7
		[Preserve]
		public RemoveBuddyResponse(Result Result = Result.Success, Guid BuddyId = default(Guid))
		{
			this.Result = Result;
			this.BuddyId = BuddyId;
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06001985 RID: 6533 RVA: 0x000390BD File Offset: 0x000372BD
		// (set) Token: 0x06001986 RID: 6534 RVA: 0x000390C5 File Offset: 0x000372C5
		[DataMember(Name = "result", EmitDefaultValue = true)]
		public Result Result { get; set; }

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06001987 RID: 6535 RVA: 0x000390CE File Offset: 0x000372CE
		// (set) Token: 0x06001988 RID: 6536 RVA: 0x000390D6 File Offset: 0x000372D6
		[DataMember(Name = "buddyId", EmitDefaultValue = true)]
		public Guid BuddyId { get; set; }

		// Token: 0x06001989 RID: 6537 RVA: 0x000A267C File Offset: 0x000A087C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class RemoveBuddyResponse {\n");
			stringBuilder.Append("  Result: ").Append(this.Result).Append("\n");
			stringBuilder.Append("  BuddyId: ").Append(this.BuddyId).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x000390DF File Offset: 0x000372DF
		public override bool Equals(object input)
		{
			return this.Equals(input as RemoveBuddyResponse);
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x000A26F8 File Offset: 0x000A08F8
		public bool Equals(RemoveBuddyResponse input)
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
			if (!(this.BuddyId == input.BuddyId))
			{
				Guid buddyId = this.BuddyId;
				return this.BuddyId.Equals(input.BuddyId);
			}
			return true;
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x000A2774 File Offset: 0x000A0974
		public override int GetHashCode()
		{
			int num = 41;
			Result result = this.Result;
			num = num * 59 + this.Result.GetHashCode();
			Guid buddyId = this.BuddyId;
			return num * 59 + this.BuddyId.GetHashCode();
		}
	}
}
