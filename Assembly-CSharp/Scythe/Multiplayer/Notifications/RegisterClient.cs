using System;
using I2.Loc;
using Newtonsoft.Json;
using UnityEngine;

namespace Scythe.Multiplayer.Notifications
{
	// Token: 0x020002A7 RID: 679
	public class RegisterClient
	{
		// Token: 0x0600157B RID: 5499 RVA: 0x0009DBC8 File Offset: 0x0009BDC8
		public void Register(string handle)
		{
			if (this.registrationInProcess)
			{
				Debug.Log("[RegisterClient] RegistrationInProcess");
				return;
			}
			this.handle = handle;
			this.registrationInProcess = true;
			this.RetrieveRegistrationId(this.handle, new Action<string>(this.OnRegistrationIdRetrieved), new Action<Exception>(this.OnRegistrationIdRetrieveError));
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x000368C5 File Offset: 0x00034AC5
		private void OnRegistrationIdRetrieved(string registrationId)
		{
			Debug.Log("[RegisterClient] OnRegistrationIdRetrieved");
			this.UpdateRegistration(registrationId, new Action<string>(this.OnRegistrationUpdated), new Action<Exception>(this.OnRegistartionUpdateError));
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x000368F0 File Offset: 0x00034AF0
		private void OnRegistrationIdRetrieveError(Exception exception)
		{
			this.registrationInProcess = false;
			Debug.LogWarning("[RegisterClient] OnRegistrationIdRetrieveError " + exception.Message);
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x0009DC1C File Offset: 0x0009BE1C
		private void RetrieveRegistrationId(string handle, Action<string> onSuccess, Action<Exception> onError)
		{
			string text = JsonConvert.SerializeObject(new { handle });
			RequestController.RequestPostCall(this.GetUrl(), text, true, onSuccess, onError);
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x0003690E File Offset: 0x00034B0E
		private void OnRegistrationUpdated(string response)
		{
			this.registrationInProcess = false;
			Debug.Log("[RegisterClient] OnRegistrationUpdated");
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x00036921 File Offset: 0x00034B21
		private void OnRegistartionUpdateError(Exception exception)
		{
			this.registrationInProcess = false;
			Debug.LogWarning("[RegisterClient] OnRegistartionUpdateError " + exception.Message);
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x0009DC48 File Offset: 0x0009BE48
		private void UpdateRegistration(string registrationId, Action<string> onSuccess, Action<Exception> onError)
		{
			string text = JsonConvert.SerializeObject(new
			{
				Handle = this.handle,
				Language = LocalizationManager.CurrentLanguageCode
			});
			string url = this.GetUrl();
			RequestController.RequestPutCall(string.Format("{0}?registrationId={1}", url, registrationId), text, true, onSuccess, onError);
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x0003693F File Offset: 0x00034B3F
		private string GetUrl()
		{
			return "Notifications/Register/Device";
		}

		// Token: 0x04000FB9 RID: 4025
		private const string SERVICE_ENDPOINT = "Notifications/Register/";

		// Token: 0x04000FBA RID: 4026
		private const string METHOD_ENDPOINT = "Device";

		// Token: 0x04000FBB RID: 4027
		private string handle = string.Empty;

		// Token: 0x04000FBC RID: 4028
		private bool registrationInProcess;
	}
}
