using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x02000365 RID: 869
	[DataContract]
	public class LinkAccountParams : IEquatable<LinkAccountParams>
	{
		// Token: 0x060018F3 RID: 6387 RVA: 0x00038CAC File Offset: 0x00036EAC
		[Preserve]
		public LinkAccountParams(string Ticket = null, string PlatformUserId = null)
		{
			this.Ticket = Ticket;
			this.PlatformUserId = PlatformUserId;
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x060018F4 RID: 6388 RVA: 0x00038CC2 File Offset: 0x00036EC2
		// (set) Token: 0x060018F5 RID: 6389 RVA: 0x00038CCA File Offset: 0x00036ECA
		[DataMember(Name = "ticket", EmitDefaultValue = true)]
		public string Ticket { get; set; }

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x060018F6 RID: 6390 RVA: 0x00038CD3 File Offset: 0x00036ED3
		// (set) Token: 0x060018F7 RID: 6391 RVA: 0x00038CDB File Offset: 0x00036EDB
		[DataMember(Name = "platformUserId", EmitDefaultValue = true)]
		public string PlatformUserId { get; set; }

		// Token: 0x060018F8 RID: 6392 RVA: 0x000A1074 File Offset: 0x0009F274
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class LinkAccountParams {\n");
			stringBuilder.Append("  Ticket: ").Append(this.Ticket).Append("\n");
			stringBuilder.Append("  PlatformUserId: ").Append(this.PlatformUserId).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		// Token: 0x060018F9 RID: 6393 RVA: 0x00038892 File Offset: 0x00036A92
		public virtual string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		// Token: 0x060018FA RID: 6394 RVA: 0x00038CE4 File Offset: 0x00036EE4
		public override bool Equals(object input)
		{
			return this.Equals(input as LinkAccountParams);
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x000A10E8 File Offset: 0x0009F2E8
		public bool Equals(LinkAccountParams input)
		{
			return input != null && (this.Ticket == input.Ticket || (this.Ticket != null && this.Ticket.Equals(input.Ticket))) && (this.PlatformUserId == input.PlatformUserId || (this.PlatformUserId != null && this.PlatformUserId.Equals(input.PlatformUserId)));
		}

		// Token: 0x060018FC RID: 6396 RVA: 0x000A115C File Offset: 0x0009F35C
		public override int GetHashCode()
		{
			int num = 41;
			if (this.Ticket != null)
			{
				num = num * 59 + this.Ticket.GetHashCode();
			}
			if (this.PlatformUserId != null)
			{
				num = num * 59 + this.PlatformUserId.GetHashCode();
			}
			return num;
		}
	}
}
