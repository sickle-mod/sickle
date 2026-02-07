using System;
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace Scythe.Multiplayer.AuthApi.Models
{
	// Token: 0x0200037F RID: 895
	public class SendSupportTicketResponse : IEquatable<SendSupportTicketResponse>
	{
		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x060019F0 RID: 6640 RVA: 0x0003947E File Offset: 0x0003767E
		// (set) Token: 0x060019F1 RID: 6641 RVA: 0x00039486 File Offset: 0x00037686
		[DataMember(Name = "result", EmitDefaultValue = true)]
		public Result Result { get; set; }

		// Token: 0x060019F2 RID: 6642 RVA: 0x0003948F File Offset: 0x0003768F
		[Preserve]
		public SendSupportTicketResponse(Result Result = Result.Success)
		{
			this.Result = Result;
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x0003949E File Offset: 0x0003769E
		public override bool Equals(object input)
		{
			return this.Equals(input as SendSupportTicketResponse);
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x000A3160 File Offset: 0x000A1360
		public bool Equals(SendSupportTicketResponse input)
		{
			if (input == null)
			{
				return false;
			}
			if (this.Result != input.Result)
			{
				Result result = this.Result;
				return this.Result.Equals(input.Result);
			}
			return true;
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x000A31A8 File Offset: 0x000A13A8
		public override int GetHashCode()
		{
			int num = 41;
			Result result = this.Result;
			return num * 59 + this.Result.GetHashCode();
		}
	}
}
