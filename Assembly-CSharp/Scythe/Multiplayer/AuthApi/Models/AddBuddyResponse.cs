using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000358 RID: 856
	[DataContract]
	public class AddBuddyResponse : IEquatable<AddBuddyResponse>
	{
		// Token: 0x0600188D RID: 6285 RVA: 0x00038968 File Offset: 0x00036B68
		[Preserve]
		public AddBuddyResponse(Result Result = Result.Success, Guid? BuddyId = null)
		{
			this.Result = Result;
			this.BuddyId = BuddyId;
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x0600188E RID: 6286 RVA: 0x0003897E File Offset: 0x00036B7E
		// (set) Token: 0x0600188F RID: 6287 RVA: 0x00038986 File Offset: 0x00036B86
		[DataMember(Name = "result", EmitDefaultValue = true)]
		public Result Result { get; set; }

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06001890 RID: 6288 RVA: 0x0003898F File Offset: 0x00036B8F
		// (set) Token: 0x06001891 RID: 6289 RVA: 0x00038997 File Offset: 0x00036B97
		[DataMember(Name = "buddyId", EmitDefaultValue = true)]
		public Guid? BuddyId { get; set; }

		// Token: 0x06001892 RID: 6290 RVA: 0x000A04F8 File Offset: 0x0009E6F8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class AddBuddyResponse {\n");
			stringBuilder.Append("  Result: ").Append(this.Result).Append("\n");
			stringBuilder.Append("  BuddyId: ").Append(this.BuddyId).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x000389A0 File Offset: 0x00036BA0
		public override bool Equals(object input)
		{
			return this.Equals(input as AddBuddyResponse);
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x000A0574 File Offset: 0x0009E774
		public bool Equals(AddBuddyResponse input)
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
			return this.BuddyId == input.BuddyId || (this.BuddyId != null && this.BuddyId.Equals(input.BuddyId));
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x000A0634 File Offset: 0x0009E834
		public override int GetHashCode()
		{
			int num = 41;
			Result result = this.Result;
			num = num * 59 + this.Result.GetHashCode();
			if (this.BuddyId != null)
			{
				num = num * 59 + this.BuddyId.GetHashCode();
			}
			return num;
		}
	}
}
