using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000371 RID: 881
	[DataContract]
	public class RemoveBuddyRequest : IEquatable<RemoveBuddyRequest>
	{
		// Token: 0x0600197C RID: 6524 RVA: 0x00039065 File Offset: 0x00037265
		[Preserve]
		public RemoveBuddyRequest(Guid? BuddyId = null)
		{
			if (BuddyId == null)
			{
				throw new InvalidDataException("BuddyId is a required property for RemoveBuddyRequest and cannot be null");
			}
			this.BuddyId = BuddyId;
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x0600197D RID: 6525 RVA: 0x00039088 File Offset: 0x00037288
		// (set) Token: 0x0600197E RID: 6526 RVA: 0x00039090 File Offset: 0x00037290
		[DataMember(Name = "buddyId", EmitDefaultValue = true)]
		public Guid? BuddyId { get; set; }

		// Token: 0x0600197F RID: 6527 RVA: 0x000A2560 File Offset: 0x000A0760
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class RemoveBuddyRequest {\n");
			stringBuilder.Append("  BuddyId: ").Append(this.BuddyId).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x00039099 File Offset: 0x00037299
		public override bool Equals(object input)
		{
			return this.Equals(input as RemoveBuddyRequest);
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x000A25B8 File Offset: 0x000A07B8
		public bool Equals(RemoveBuddyRequest input)
		{
			return input != null && (this.BuddyId == input.BuddyId || (this.BuddyId != null && this.BuddyId.Equals(input.BuddyId)));
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x000A2640 File Offset: 0x000A0840
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
