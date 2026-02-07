using System;
using I2.Loc;
using Newtonsoft.Json;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;

namespace Scythe.Multiplayer
{
	// Token: 0x0200021E RID: 542
	public class ConnectionProblemPanel : MonoBehaviour
	{
		// Token: 0x0600100B RID: 4107 RVA: 0x00032522 File Offset: 0x00030722
		public void Activate(Exception exception)
		{
			base.gameObject.SetActive(true);
			this.SetAppropriateErrorText(exception);
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x00029172 File Offset: 0x00027372
		public void Deactivate()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x00032537 File Offset: 0x00030737
		public bool IsActive()
		{
			return base.gameObject.activeInHierarchy;
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0008F104 File Offset: 0x0008D304
		private void SetAppropriateErrorText(Exception exception)
		{
			if (this.IsPlayerTimedOut(exception))
			{
				this.errorText.text = ScriptLocalization.Get("Lobby/ConnectionTimedOut");
				return;
			}
			if (this.IsAuthorizationError(exception))
			{
				this.errorText.text = ScriptLocalization.Get("Lobby/FailedToConnectEloMessage");
				return;
			}
			this.errorText.text = ScriptLocalization.Get("Lobby/FailedToConnectEloMessage");
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x00032544 File Offset: 0x00030744
		private bool IsAuthorizationError(Exception error)
		{
			return error is UnauthorizedAccessException;
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0003254F File Offset: 0x0003074F
		private bool IsPlayerTimedOut(Exception error)
		{
			return this.IsAuthorizationError(error) && JsonConvert.DeserializeObject<AuthorizationError>(error.Message).ErrorStatus == AuthorizationErrorStatus.PlayerNotLoggedIn;
		}

		// Token: 0x04000C4F RID: 3151
		[SerializeField]
		private TextMeshProUGUI errorText;
	}
}
