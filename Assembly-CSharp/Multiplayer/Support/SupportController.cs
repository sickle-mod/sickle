using System;
using Multiplayer.AuthApi;
using Scythe.Multiplayer;
using Scythe.Multiplayer.AuthApi.Models;
using Scythe.Utilities;

namespace Multiplayer.Support
{
	// Token: 0x020001A1 RID: 417
	public class SupportController : Singleton<SupportController>
	{
		// Token: 0x06000C4F RID: 3151 RVA: 0x000303CD File Offset: 0x0002E5CD
		public void SendSupportTicket(string authorEmail, string messageBody, Action<SendSupportTicketResponse> onSuccess, Action<FailureResponse> onFailure, Action<Exception> onError)
		{
			AuthRestAPI.SendSupportTicket(authorEmail, messageBody, onSuccess, onFailure, onError);
		}
	}
}
