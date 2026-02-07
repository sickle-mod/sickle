using System;
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x0200037E RID: 894
	public class SendSupportTicketRequest : IEquatable<SendSupportTicketRequest>
	{
		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x060019E8 RID: 6632 RVA: 0x00039438 File Offset: 0x00037638
		// (set) Token: 0x060019E9 RID: 6633 RVA: 0x00039440 File Offset: 0x00037640
		[DataMember(Name = "authoremail", EmitDefaultValue = true)]
		public string AuthorEmail { get; set; }

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x060019EA RID: 6634 RVA: 0x00039449 File Offset: 0x00037649
		// (set) Token: 0x060019EB RID: 6635 RVA: 0x00039451 File Offset: 0x00037651
		[DataMember(Name = "messagebody", EmitDefaultValue = true)]
		public string MessageBody { get; set; }

		// Token: 0x060019EC RID: 6636 RVA: 0x0003945A File Offset: 0x0003765A
		[Preserve]
		public SendSupportTicketRequest(string authorEmail, string messageBody)
		{
			this.AuthorEmail = authorEmail;
			this.MessageBody = messageBody;
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x00039470 File Offset: 0x00037670
		public override bool Equals(object input)
		{
			return this.Equals(input as SendSupportTicketRequest);
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x000A30A8 File Offset: 0x000A12A8
		public bool Equals(SendSupportTicketRequest input)
		{
			return input != null && (this.AuthorEmail == input.AuthorEmail || (this.AuthorEmail != null && this.AuthorEmail.Equals(input.AuthorEmail))) && (this.MessageBody == input.MessageBody || (this.MessageBody != null && this.MessageBody.Equals(input.MessageBody)));
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x000A311C File Offset: 0x000A131C
		public override int GetHashCode()
		{
			int num = 41;
			if (this.AuthorEmail != null)
			{
				num = num * 59 + this.AuthorEmail.GetHashCode();
			}
			if (this.MessageBody != null)
			{
				num = num * 59 + this.MessageBody.GetHashCode();
			}
			return num;
		}
	}
}
