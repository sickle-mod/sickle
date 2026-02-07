using System;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network;
using AsmodeeNet.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007A4 RID: 1956
	public class LoginBannerContentController : MonoBehaviour
	{
		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06003874 RID: 14452 RVA: 0x0004C426 File Offset: 0x0004A626
		// (set) Token: 0x06003875 RID: 14453 RVA: 0x0004C42E File Offset: 0x0004A62E
		public bool AllowAutoCollapse
		{
			get
			{
				return this._allowAutoCollapse;
			}
			set
			{
				this._allowAutoCollapse = value;
				this._StopCollapseTimer();
				this._UpdateTargetTransform();
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06003876 RID: 14454 RVA: 0x0004C443 File Offset: 0x0004A643
		// (set) Token: 0x06003877 RID: 14455 RVA: 0x0014A9E4 File Offset: 0x00148BE4
		private LoginBannerContentController.State _State
		{
			get
			{
				return this._state;
			}
			set
			{
				if (this._state == value)
				{
					return;
				}
				this._state = value;
				switch (this._state)
				{
				default:
					this._DisplayState = LoginBannerContentController.DisplayState.Partial;
					return;
				case LoginBannerContentController.State.NotAuthenticated:
					this._DisplayState = LoginBannerContentController.DisplayState.Full;
					return;
				case LoginBannerContentController.State.Authenticating:
					if (this._DisplayState == LoginBannerContentController.DisplayState.Full || this._DisplayState == LoginBannerContentController.DisplayState.TemporaryFull)
					{
						this._DisplayState = LoginBannerContentController.DisplayState.TemporaryFull;
						return;
					}
					this._DisplayState = LoginBannerContentController.DisplayState.Partial;
					return;
				case LoginBannerContentController.State.Authenticated:
					this._DisplayState = LoginBannerContentController.DisplayState.TemporaryFull;
					return;
				}
			}
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06003878 RID: 14456 RVA: 0x0004C44B File Offset: 0x0004A64B
		// (set) Token: 0x06003879 RID: 14457 RVA: 0x0004C453 File Offset: 0x0004A653
		private LoginBannerContentController.DisplayState _DisplayState
		{
			get
			{
				return this._displayState;
			}
			set
			{
				if (this._displayState == value)
				{
					return;
				}
				this._displayState = value;
				this._StopCollapseTimer();
				this._UpdateTargetTransform();
			}
		}

		// Token: 0x14000145 RID: 325
		// (add) Token: 0x0600387A RID: 14458 RVA: 0x0014AA5C File Offset: 0x00148C5C
		// (remove) Token: 0x0600387B RID: 14459 RVA: 0x0014AA94 File Offset: 0x00148C94
		public event Action LoginBannerDidSelectAccount;

		// Token: 0x0600387C RID: 14460 RVA: 0x0004C472 File Offset: 0x0004A672
		public void OnLoginButtonClicked()
		{
			this._getTokenCallbackId = CoreApplication.Instance.OAuthGate.GetPrivateAccessToken(false, delegate(OAuthError error)
			{
				this._getTokenCallbackId = null;
				if (error != null)
				{
					this._State = LoginBannerContentController.State.Error;
				}
			});
		}

		// Token: 0x0600387D RID: 14461 RVA: 0x0004C496 File Offset: 0x0004A696
		public void OnAccountButtonClicked()
		{
			if (this.LoginBannerDidSelectAccount != null)
			{
				this.LoginBannerDidSelectAccount();
			}
		}

		// Token: 0x0600387E RID: 14462 RVA: 0x0004C4AB File Offset: 0x0004A6AB
		public void OnBannerClicked()
		{
			if (this._DisplayState == LoginBannerContentController.DisplayState.Partial)
			{
				this._DisplayState = LoginBannerContentController.DisplayState.TemporaryFull;
				return;
			}
			if (this._State == LoginBannerContentController.State.NotAuthenticated)
			{
				this.OnLoginButtonClicked();
				return;
			}
			if (this._State == LoginBannerContentController.State.Authenticated)
			{
				this.OnAccountButtonClicked();
			}
		}

		// Token: 0x0600387F RID: 14463 RVA: 0x0004C4DD File Offset: 0x0004A6DD
		private void OnEnable()
		{
			this._StopCollapseTimer();
			this._UpdateTargetTransform();
			this.contentTransform.anchoredPosition = this._targetTransform.anchoredPosition;
		}

		// Token: 0x06003880 RID: 14464 RVA: 0x0014AACC File Offset: 0x00148CCC
		private void OnDisable()
		{
			if (this._avatarRetrievalHandle != null)
			{
				this._avatarRetrievalHandle.Abort();
				this._avatarRetrievalHandle = null;
			}
			if (this._getTokenCallbackId != null)
			{
				CoreApplication.Instance.OAuthGate.CancelAccessTokenRequest(this._getTokenCallbackId.Value);
				this._getTokenCallbackId = null;
			}
		}

		// Token: 0x06003881 RID: 14465 RVA: 0x0014AB28 File Offset: 0x00148D28
		private void Update()
		{
			if (this._collapseTimeRunning)
			{
				this._collapseTimer -= Time.deltaTime;
				if (this._collapseTimer <= 0f)
				{
					this._StopCollapseTimer();
					this._DisplayState = LoginBannerContentController.DisplayState.Partial;
				}
			}
			LoginBannerContentController.State state;
			if (!CoreApplication.Instance.CommunityHub.IsNetworkReachable)
			{
				state = LoginBannerContentController.State.Error;
			}
			else if (CoreApplication.Instance.OAuthGate.HasPrivateToken)
			{
				if (CoreApplication.Instance.OAuthGate.UserDetails == null)
				{
					state = LoginBannerContentController.State.Authenticating;
				}
				else
				{
					state = LoginBannerContentController.State.Authenticated;
				}
			}
			else if (CoreApplication.Instance.OAuthGate.IsRetrievingPrivateToken)
			{
				state = LoginBannerContentController.State.Authenticating;
			}
			else
			{
				state = LoginBannerContentController.State.NotAuthenticated;
			}
			bool flag = this._State != state;
			this._State = state;
			this._UpdateIndicator();
			this._UpdateButton();
			this._UpdatePosition();
			if (flag && this._State == LoginBannerContentController.State.Authenticated)
			{
				int userId = CoreApplication.Instance.OAuthGate.UserDetails.UserId;
				this._avatarRetrievalHandle = CoreApplication.Instance.AvatarManager.LoadPlayerAvatar(userId, this.avatarImage, delegate(bool succeeded)
				{
					this._avatarRetrievalHandle = null;
				});
			}
		}

		// Token: 0x06003882 RID: 14466 RVA: 0x0014AC30 File Offset: 0x00148E30
		private void _UpdateIndicator()
		{
			switch (this._State)
			{
			default:
				this.networkErrorIndicator.SetActive(true);
				this.notAuthenticatedAvatarGroup.SetActive(false);
				this.authenticatingAvatarGroup.SetActive(false);
				this.authenticatedAvatarGroup.SetActive(false);
				return;
			case LoginBannerContentController.State.NotAuthenticated:
				this.networkErrorIndicator.SetActive(false);
				this.notAuthenticatedAvatarGroup.SetActive(true);
				this.authenticatingAvatarGroup.SetActive(false);
				this.authenticatedAvatarGroup.SetActive(false);
				return;
			case LoginBannerContentController.State.Authenticating:
				this.networkErrorIndicator.SetActive(false);
				this.notAuthenticatedAvatarGroup.SetActive(false);
				this.authenticatingAvatarGroup.SetActive(true);
				this.authenticatedAvatarGroup.SetActive(false);
				return;
			case LoginBannerContentController.State.Authenticated:
				if (this._avatarRetrievalHandle != null)
				{
					this.networkErrorIndicator.SetActive(false);
					this.notAuthenticatedAvatarGroup.SetActive(false);
					this.authenticatingAvatarGroup.SetActive(true);
					this.authenticatedAvatarGroup.SetActive(false);
					return;
				}
				this.networkErrorIndicator.SetActive(false);
				this.notAuthenticatedAvatarGroup.SetActive(false);
				this.authenticatingAvatarGroup.SetActive(false);
				this.authenticatedAvatarGroup.SetActive(true);
				return;
			}
		}

		// Token: 0x06003883 RID: 14467 RVA: 0x0014AD58 File Offset: 0x00148F58
		private void _UpdateButton()
		{
			switch (this._State)
			{
			default:
				this.loginButton.gameObject.SetActive(false);
				this.accountButton.gameObject.SetActive(false);
				return;
			case LoginBannerContentController.State.NotAuthenticated:
				this.loginButton.gameObject.SetActive(true);
				this.loginButton.interactable = true;
				this.accountButton.gameObject.SetActive(false);
				return;
			case LoginBannerContentController.State.Authenticating:
				this.loginButton.gameObject.SetActive(true);
				this.loginButton.interactable = false;
				this.accountButton.gameObject.SetActive(false);
				return;
			case LoginBannerContentController.State.Authenticated:
				this.accountButtonText.text = CoreApplication.Instance.OAuthGate.UserDetails.LoginName;
				this.loginButton.gameObject.SetActive(false);
				this.accountButton.gameObject.SetActive(true);
				return;
			}
		}

		// Token: 0x06003884 RID: 14468 RVA: 0x0004C501 File Offset: 0x0004A701
		private void _UpdatePosition()
		{
			this.contentTransform.anchoredPosition = Vector2.Lerp(this.contentTransform.anchoredPosition, this._targetTransform.anchoredPosition, Time.deltaTime * 10f);
		}

		// Token: 0x06003885 RID: 14469 RVA: 0x0014AE48 File Offset: 0x00149048
		private void _UpdateTargetTransform()
		{
			switch (this._DisplayState)
			{
			default:
				this._targetTransform = this.hiddenTransformRef;
				return;
			case LoginBannerContentController.DisplayState.Partial:
				this._targetTransform = this.collapsedTransformRef;
				return;
			case LoginBannerContentController.DisplayState.Full:
				this._targetTransform = this.expandedTransformRef;
				return;
			case LoginBannerContentController.DisplayState.TemporaryFull:
				this._targetTransform = this.expandedTransformRef;
				if (this._allowAutoCollapse)
				{
					this._StartCollapseTimer();
				}
				return;
			}
		}

		// Token: 0x06003886 RID: 14470 RVA: 0x0004C534 File Offset: 0x0004A734
		private void _StartCollapseTimer()
		{
			this._collapseTimeRunning = true;
			this._collapseTimer = 4f;
		}

		// Token: 0x06003887 RID: 14471 RVA: 0x0004C548 File Offset: 0x0004A748
		private void _StopCollapseTimer()
		{
			this._collapseTimeRunning = false;
			this._collapseTimer = 0f;
		}

		// Token: 0x04002A5D RID: 10845
		private const string _documentation = "<b>LoginBannerContentController</b> Allows a user to check it's authentication status to the <b>RestAPI</b>.";

		// Token: 0x04002A5E RID: 10846
		[Header("Position References")]
		public RectTransform contentTransform;

		// Token: 0x04002A5F RID: 10847
		private RectTransform _targetTransform;

		// Token: 0x04002A60 RID: 10848
		[Tooltip("Fully visible")]
		public RectTransform expandedTransformRef;

		// Token: 0x04002A61 RID: 10849
		[Tooltip("Only indicators")]
		public RectTransform collapsedTransformRef;

		// Token: 0x04002A62 RID: 10850
		[Tooltip("Nothing visible")]
		public RectTransform hiddenTransformRef;

		// Token: 0x04002A63 RID: 10851
		private bool _allowAutoCollapse = true;

		// Token: 0x04002A64 RID: 10852
		[Header("Indicators")]
		[Tooltip("Displayed when the network is not reachable")]
		public GameObject networkErrorIndicator;

		// Token: 0x04002A65 RID: 10853
		[Tooltip("Displayed when the user is not authenticated")]
		public GameObject notAuthenticatedAvatarGroup;

		// Token: 0x04002A66 RID: 10854
		[Tooltip("Displayed when the user is authenticating")]
		public GameObject authenticatingAvatarGroup;

		// Token: 0x04002A67 RID: 10855
		[Tooltip("Displayed when the user is authenticated and the avatar has been retrieved")]
		public GameObject authenticatedAvatarGroup;

		// Token: 0x04002A68 RID: 10856
		public Image avatarImage;

		// Token: 0x04002A69 RID: 10857
		[Header("Buttons")]
		[Tooltip("Displayed when the user is not authenticated")]
		public Button loginButton;

		// Token: 0x04002A6A RID: 10858
		[Tooltip("Displayed when the user is authenticated")]
		public Button accountButton;

		// Token: 0x04002A6B RID: 10859
		public TextMeshProUGUI accountButtonText;

		// Token: 0x04002A6C RID: 10860
		private LoginBannerContentController.State _state;

		// Token: 0x04002A6D RID: 10861
		private LoginBannerContentController.DisplayState _displayState;

		// Token: 0x04002A6F RID: 10863
		private const float _collapseAnimationSmoothing = 10f;

		// Token: 0x04002A70 RID: 10864
		private const float _collapseTimerDuration = 4f;

		// Token: 0x04002A71 RID: 10865
		private float _collapseTimer;

		// Token: 0x04002A72 RID: 10866
		private bool _collapseTimeRunning;

		// Token: 0x04002A73 RID: 10867
		private AvatarManager.RetrievalHandle _avatarRetrievalHandle;

		// Token: 0x04002A74 RID: 10868
		protected int? _getTokenCallbackId;

		// Token: 0x020007A5 RID: 1957
		private enum State
		{
			// Token: 0x04002A76 RID: 10870
			Unknown,
			// Token: 0x04002A77 RID: 10871
			Error,
			// Token: 0x04002A78 RID: 10872
			NotAuthenticated,
			// Token: 0x04002A79 RID: 10873
			Authenticating,
			// Token: 0x04002A7A RID: 10874
			Authenticated
		}

		// Token: 0x020007A6 RID: 1958
		private enum DisplayState
		{
			// Token: 0x04002A7C RID: 10876
			Hidden,
			// Token: 0x04002A7D RID: 10877
			Partial,
			// Token: 0x04002A7E RID: 10878
			Full,
			// Token: 0x04002A7F RID: 10879
			TemporaryFull
		}
	}
}
