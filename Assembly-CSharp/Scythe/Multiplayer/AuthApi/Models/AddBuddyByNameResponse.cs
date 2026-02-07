using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000356 RID: 854
	[DataContract]
	public class AddBuddyByNameResponse : IEquatable<AddBuddyByNameResponse>
	{
		// Token: 0x0600187B RID: 6267 RVA: 0x000388E0 File Offset: 0x00036AE0
		[Preserve]
		public AddBuddyByNameResponse(Result Result = Result.Success, Guid? BuddyId = null)
		{
			this.Result = Result;
			this.BuddyId = BuddyId;
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x0600187C RID: 6268 RVA: 0x000388F6 File Offset: 0x00036AF6
		// (set) Token: 0x0600187D RID: 6269 RVA: 0x000388FE File Offset: 0x00036AFE
		[DataMember(Name = "result", EmitDefaultValue = true)]
		public Result Result { get; set; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x0600187E RID: 6270 RVA: 0x00038907 File Offset: 0x00036B07
		// (set) Token: 0x0600187F RID: 6271 RVA: 0x0003890F File Offset: 0x00036B0F
		[DataMember(Name = "buddyId", EmitDefaultValue = true)]
		public Guid? BuddyId { get; set; }

		// Token: 0x06001880 RID: 6272 RVA: 0x000A0244 File Offset: 0x0009E444
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class AddBuddyByNameResponse {\n");
			stringBuilder.Append("  Result: ").Append(this.Result).Append("\n");
			stringBuilder.Append("  BuddyId: ").Append(this.BuddyId).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x00038918 File Offset: 0x00036B18
		public override bool Equals(object input)
		{
			return this.Equals(input as AddBuddyByNameResponse);
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x000A02C0 File Offset: 0x0009E4C0
		public bool Equals(AddBuddyByNameResponse input)
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

		// Token: 0x06001884 RID: 6276 RVA: 0x000A0380 File Offset: 0x0009E580
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
