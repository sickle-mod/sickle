using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000357 RID: 855
	[DataContract]
	public class AddBuddyRequest : IEquatable<AddBuddyRequest>
	{
		// Token: 0x06001885 RID: 6277 RVA: 0x00038926 File Offset: 0x00036B26
		[Preserve]
		public AddBuddyRequest(Guid? BuddyId = null)
		{
			if (BuddyId == null)
			{
				throw new InvalidDataException("BuddyId is a required property for AddBuddyRequest and cannot be null");
			}
			this.BuddyId = BuddyId;
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06001886 RID: 6278 RVA: 0x00038949 File Offset: 0x00036B49
		// (set) Token: 0x06001887 RID: 6279 RVA: 0x00038951 File Offset: 0x00036B51
		[DataMember(Name = "buddyId", EmitDefaultValue = true)]
		public Guid? BuddyId { get; set; }

		// Token: 0x06001888 RID: 6280 RVA: 0x000A03DC File Offset: 0x0009E5DC
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class AddBuddyRequest {\n");
			stringBuilder.Append("  BuddyId: ").Append(this.BuddyId).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x0003895A File Offset: 0x00036B5A
		public override bool Equals(object input)
		{
			return this.Equals(input as AddBuddyRequest);
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x000A0434 File Offset: 0x0009E634
		public bool Equals(AddBuddyRequest input)
		{
			return input != null && (this.BuddyId == input.BuddyId || (this.BuddyId != null && this.BuddyId.Equals(input.BuddyId)));
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x000A04BC File Offset: 0x0009E6BC
		public override int GetHashCode()
		{
			int num = 41;
			if (this.BuddyId != null)
			{
				num = num * 59 + this.BuddyId.GetHashCode();
			}
			return num;
		}
	}
}
