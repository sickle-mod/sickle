using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Assets.Scripts.LocalStore;
using Assets.Scripts.LocalStore.Model;
using Newtonsoft.Json;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using UnityEngine;

namespace AuthApi
{
	// Token: 0x020001C3 RID: 451
	public class AuthApiConnection
	{
		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000D44 RID: 3396 RVA: 0x000853AC File Offset: 0x000835AC
		public string Token
		{
			get
			{
				if (string.IsNullOrEmpty(this.token))
				{
					PlayerLoginResultDto playerLoginResultDto = this.UserToken(this.login, this.password);
					this.token = playerLoginResultDto.Token;
				}
				return this.token;
			}
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x000853EC File Offset: 0x000835EC
		public void TryToLogin(global::System.Action onSuccess, global::System.Action onFailure, global::System.Action onError = null)
		{
			try
			{
				this.SetLocalUserCredentials(onFailure, onError);
				this.TokenFullValidation(onSuccess, onFailure, onError);
			}
			catch (Exception ex)
			{
				if (onError == null)
				{
					throw;
				}
				Debug.LogError("TryToLogin error, " + ex.Message);
				onError();
			}
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x00030C4B File Offset: 0x0002EE4B
		public void SetCredentials(string login, string password)
		{
			this.login = login;
			this.password = password;
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x00030C5B File Offset: 0x0002EE5B
		public void TryToLinkPlatformAccount(global::System.Action onSuccess, global::System.Action onFailure)
		{
			Debug.LogError("[AuthApiConnection] I'm not implemented. Implement me!");
			if (onFailure != null)
			{
				onFailure();
			}
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x00085444 File Offset: 0x00083644
		private void SetLocalUserCredentials(global::System.Action onEmpty, global::System.Action onError)
		{
			LocalLoggedUserData value = this._prefsStorage.GetValue<LocalLoggedUserData>(PlayerPrefsStorageKeys.Auth_LocalLoggedUserDataObj, onError);
			if (value != null)
			{
				PlayerInfo.me.PlayerStats.Name = value.Login;
				PlayerInfo.me.PlayerStats.Id = value.Id;
				PlayerInfo.me.Token = value.Token;
				this.login = value.Login;
				this.token = value.Token;
				return;
			}
			this.login = (this.token = (this.password = null));
			onEmpty();
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x000854D8 File Offset: 0x000836D8
		private void TokenFullValidation(global::System.Action onValid, global::System.Action onInvalid, global::System.Action onError = null)
		{
			AuthApiConnection.<>c__DisplayClass12_0 CS$<>8__locals1 = new AuthApiConnection.<>c__DisplayClass12_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.onInvalid = onInvalid;
			CS$<>8__locals1.onValid = onValid;
			try
			{
				if (string.IsNullOrWhiteSpace(this.token))
				{
					CS$<>8__locals1.onInvalid();
				}
				else
				{
					this.ValidateToken(new Action<IsTokenValidResultDto>(CS$<>8__locals1.<TokenFullValidation>g__ValidateTokenSuccess|0), new Action<IsTokenValidResultDto>(CS$<>8__locals1.<TokenFullValidation>g__ValidateError|2));
				}
			}
			catch (Exception ex)
			{
				if (onError == null)
				{
					throw;
				}
				Debug.LogError("TokenFullValidation error, " + ex.Message);
				onError();
			}
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x00030C70 File Offset: 0x0002EE70
		private void ValidateToken(Action<IsTokenValidResultDto> onSuccess, Action<IsTokenValidResultDto> onError)
		{
			AuthRequestController.RequestGetCall<IsTokenValidResultDto, IsTokenValidResultDto>("/Account/TokenValidate?token=" + this.token, onSuccess, onError, null);
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x00085570 File Offset: 0x00083770
		private void ValidateForServerApiToken(Action<IsTokenValidResultDto> onSuccess, Action<IsTokenValidResultDto> onError)
		{
			RequestController.RequestGetCall("Login/TokenValidate?token=" + this.token, delegate(string response)
			{
				onSuccess(JsonConvert.DeserializeObject<IsTokenValidResultDto>(response));
			}, delegate(Exception error)
			{
				onError(new IsTokenValidResultDto
				{
					IsSuccesful = false,
					Message = error.Message
				});
			});
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x00030C8B File Offset: 0x0002EE8B
		private PlayerLoginResultDto UserToken(string login, string password)
		{
			return JsonConvert.DeserializeObject<PlayerLoginResultDto>(this.PostCall(AuthApiConnection.api.POST.Login, new { login, password }).Content.ReadAsStringAsync().Result);
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x000855C0 File Offset: 0x000837C0
		private HttpResponseMessage PostCall(string endpoint, object input)
		{
			string text = JsonConvert.SerializeObject(input);
			ByteArrayContent byteArrayContent = new ByteArrayContent(Encoding.UTF8.GetBytes(text))
			{
				Headers = 
				{
					ContentType = new MediaTypeHeaderValue("application/json")
				}
			};
			return AuthApiConnection.client.PostAsync(endpoint, byteArrayContent).Result;
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x00030CBD File Offset: 0x0002EEBD
		private HttpResponseMessage GetCall(string endpoint)
		{
			AuthApiConnection.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.token);
			return AuthApiConnection.client.GetAsync(endpoint).Result;
		}

		// Token: 0x04000AA9 RID: 2729
		private IKeyValueStorage _prefsStorage = new PlayerPrefsStorage();

		// Token: 0x04000AAA RID: 2730
		private string token;

		// Token: 0x04000AAB RID: 2731
		private string login;

		// Token: 0x04000AAC RID: 2732
		private string password;

		// Token: 0x04000AAD RID: 2733
		private static readonly HttpClient client = new HttpClient();

		// Token: 0x04000AAE RID: 2734
		private static readonly AuthApiEndpoints api = new AuthApiEndpoints();
	}
}
