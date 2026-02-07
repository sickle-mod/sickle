using System;
using System.Collections;
using System.Linq;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network;
using AsmodeeNet.Network.RestApi;
using AsmodeeNet.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007B4 RID: 1972
	public class UserAccountContentController : MonoBehaviour
	{
		// Token: 0x1400014A RID: 330
		// (add) Token: 0x060038C0 RID: 14528 RVA: 0x0014BA08 File Offset: 0x00149C08
		// (remove) Token: 0x060038C1 RID: 14529 RVA: 0x0014BA40 File Offset: 0x00149C40
		public event Action UserAccountDidClose;

		// Token: 0x1400014B RID: 331
		// (add) Token: 0x060038C2 RID: 14530 RVA: 0x0014BA78 File Offset: 0x00149C78
		// (remove) Token: 0x060038C3 RID: 14531 RVA: 0x0014BAB0 File Offset: 0x00149CB0
		public event Action UserDidLogOut;

		// Token: 0x060038C4 RID: 14532 RVA: 0x0004C7D2 File Offset: 0x0004A9D2
		private void Start()
		{
			this._updateTitle();
		}

		// Token: 0x060038C5 RID: 14533 RVA: 0x0004C7DA File Offset: 0x0004A9DA
		private void OnEnable()
		{
			this._needsLayout = true;
			this.Update();
		}

		// Token: 0x060038C6 RID: 14534 RVA: 0x0004C7E9 File Offset: 0x0004A9E9
		private void OnDisable()
		{
			if (this._resetPasswordEndpoint != null)
			{
				this._resetPasswordEndpoint.Abort();
				this._resetPasswordEndpoint = null;
			}
			if (this._avatarRetrievalHandle != null)
			{
				this._avatarRetrievalHandle.Abort();
				this._avatarRetrievalHandle = null;
			}
			this._state = UserAccountContentController.State.Unknown;
		}

		// Token: 0x060038C7 RID: 14535 RVA: 0x0004C826 File Offset: 0x0004AA26
		private void Update()
		{
			this._UpdateCloseButton();
			this._UpdateAspect();
			this._UpdateState();
			this._UpdateDisplay();
		}

		// Token: 0x060038C8 RID: 14536 RVA: 0x0004C840 File Offset: 0x0004AA40
		private void _UpdateCloseButton()
		{
			if (this.closeButton != null)
			{
				this.closeButton.gameObject.SetActive(this.shouldDisplayCloseButton);
			}
		}

		// Token: 0x060038C9 RID: 14537 RVA: 0x0014BAE8 File Offset: 0x00149CE8
		private void _UpdateAspect()
		{
			float num = this.container.rect.size.x / this.container.rect.size.y;
			if (!Mathf.Approximately(num, this._containerAspect))
			{
				if ((this._containerAspect >= 1f && num < 1f) || (this._containerAspect < 1f && num >= 1f))
				{
					this._needsLayout = true;
				}
				this._containerAspect = num;
			}
		}

		// Token: 0x060038CA RID: 14538 RVA: 0x0014BB70 File Offset: 0x00149D70
		private void _UpdateState()
		{
			UserAccountContentController.State state;
			if (CoreApplication.Instance.OAuthGate.HasPrivateToken)
			{
				if (CoreApplication.Instance.OAuthGate.UserDetails == null)
				{
					state = UserAccountContentController.State.FetchingUserData;
				}
				else
				{
					state = UserAccountContentController.State.ValidUserData;
				}
			}
			else
			{
				state = UserAccountContentController.State.NotAuthenticated;
			}
			bool flag = this._state != state;
			this._state = state;
			if (flag && this._state == UserAccountContentController.State.ValidUserData)
			{
				OAuthGate oauthGate = CoreApplication.Instance.OAuthGate;
				User userDetails = oauthGate.UserDetails;
				bool valueOrDefault = userDetails.EmailValid.GetValueOrDefault();
				if (this.nameLabel != null)
				{
					this.nameLabel.text = userDetails.Name;
				}
				if (this.loginNameLabel != null)
				{
					this.loginNameLabel.text = userDetails.LoginName;
				}
				if (this.userIdentifierLabel != null)
				{
					this.userIdentifierLabel.text = "# " + userDetails.UserId.ToString();
				}
				if (this.emailLabel != null)
				{
					this.emailLabel.text = userDetails.Email;
				}
				if (this.coppaIndicator != null)
				{
					this.coppaIndicator.SetActive(userDetails.Coppa.GetValueOrDefault());
				}
				if (this.registeredLabel != null)
				{
					this.registeredLabel.text = ((userDetails.JoinDate != null) ? userDetails.JoinDate.Value.ToShortDateString() : "?");
				}
				ISteamManager steamManager = oauthGate.SteamManager;
				if (steamManager == null || !steamManager.HasClient)
				{
					if (this.steamContainer != null)
					{
						this.steamContainer.SetActive(false);
					}
				}
				else
				{
					if (this.steamContainer != null)
					{
						this.steamContainer.SetActive(true);
					}
					PartnerAccount steamMe = steamManager.Me;
					if (this.steamIdentifierLabel != null)
					{
						this.steamIdentifierLabel.text = "Steam #" + steamMe.PartnerUser;
					}
					if (this.steamWarningStatus != null)
					{
						bool flag2 = userDetails.Partners.FirstOrDefault((PartnerAccount p) => p.Equals(steamMe)) == null;
						this.steamWarningStatus.SetActive(flag2);
					}
				}
				if (this.getMyPasswordButton != null)
				{
					this.getMyPasswordButton.gameObject.SetActive(valueOrDefault);
				}
				if (this.getMyPasswordResultLabel != null)
				{
					this.getMyPasswordResultLabel.gameObject.SetActive(false);
				}
				if (this.updateMyEmailButton != null)
				{
					this.updateMyEmailButton.gameObject.SetActive(!valueOrDefault);
				}
				int userId = CoreApplication.Instance.OAuthGate.UserDetails.UserId;
				this._avatarRetrievalHandle = CoreApplication.Instance.AvatarManager.LoadPlayerAvatar(userId, this.avatarImage, delegate(bool succeeded)
				{
					this._avatarRetrievalHandle = null;
				});
				FontSizeHomogenizer[] array = this.fontSizeHomogenizers;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetNeedsFontSizeUpdate();
				}
			}
		}

		// Token: 0x060038CB RID: 14539 RVA: 0x0014BE7C File Offset: 0x0014A07C
		private void _UpdateDisplay()
		{
			switch (this._state)
			{
			default:
				this.content.SetActive(false);
				this.activityIndicator.SetActive(false);
				this.errorIndicator.SetActive(false);
				this.notAuthenticatedIndicator.SetActive(true);
				return;
			case UserAccountContentController.State.FetchingUserData:
				this.content.SetActive(false);
				this.activityIndicator.SetActive(true);
				this.errorIndicator.SetActive(false);
				this.notAuthenticatedIndicator.SetActive(false);
				return;
			case UserAccountContentController.State.ValidUserData:
				this.content.SetActive(true);
				this.activityIndicator.SetActive(false);
				this.errorIndicator.SetActive(false);
				this.notAuthenticatedIndicator.SetActive(false);
				if (this._needsLayout)
				{
					this._needsLayout = false;
					base.StartCoroutine(this._UpdateLayout());
					return;
				}
				break;
			case UserAccountContentController.State.Error:
				this.content.SetActive(false);
				this.activityIndicator.SetActive(false);
				this.errorIndicator.SetActive(true);
				this.notAuthenticatedIndicator.SetActive(false);
				break;
			}
		}

		// Token: 0x060038CC RID: 14540 RVA: 0x0004C866 File Offset: 0x0004AA66
		private IEnumerator _UpdateLayout()
		{
			this.container.gameObject.SetActive(false);
			VerticalLayoutGroup[] components = this.container.gameObject.GetComponents<VerticalLayoutGroup>();
			for (int i = 0; i < components.Length; i++)
			{
				global::UnityEngine.Object.Destroy(components[i]);
			}
			HorizontalLayoutGroup[] components2 = this.container.gameObject.GetComponents<HorizontalLayoutGroup>();
			for (int i = 0; i < components2.Length; i++)
			{
				global::UnityEngine.Object.Destroy(components2[i]);
			}
			yield return new WaitForEndOfFrame();
			if (this._containerAspect >= 1f)
			{
				HorizontalLayoutGroup horizontalLayoutGroup = this.container.gameObject.AddComponent<HorizontalLayoutGroup>();
				horizontalLayoutGroup.childForceExpandWidth = true;
				horizontalLayoutGroup.childForceExpandHeight = false;
				horizontalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
			}
			else
			{
				VerticalLayoutGroup verticalLayoutGroup = this.container.gameObject.AddComponent<VerticalLayoutGroup>();
				verticalLayoutGroup.childForceExpandWidth = true;
				verticalLayoutGroup.childForceExpandHeight = false;
				verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
			}
			this.container.gameObject.SetActive(true);
			yield break;
		}

		// Token: 0x060038CD RID: 14541 RVA: 0x0004C875 File Offset: 0x0004AA75
		public void OnCloseButtonClicked()
		{
			if (this.UserAccountDidClose != null)
			{
				this.UserAccountDidClose();
			}
		}

		// Token: 0x060038CE RID: 14542 RVA: 0x0004C88A File Offset: 0x0004AA8A
		public void OnToggleSDKVersion()
		{
			this._displaySDKVersion = !this._displaySDKVersion;
			this._updateTitle();
		}

		// Token: 0x060038CF RID: 14543 RVA: 0x0004C8A1 File Offset: 0x0004AAA1
		private void _updateTitle()
		{
			if (this.title != null)
			{
				this.title.text = (this._displaySDKVersion ? ("Version " + SDKVersionManager.Version()) : "Asmodee.net");
			}
		}

		// Token: 0x060038D0 RID: 14544 RVA: 0x0004C8DA File Offset: 0x0004AADA
		public void OnLogoutButtonClicked()
		{
			CoreApplication.Instance.OAuthGate.LogOut();
			if (this.UserDidLogOut != null)
			{
				this.UserDidLogOut();
			}
		}

		// Token: 0x060038D1 RID: 14545 RVA: 0x0004C8FE File Offset: 0x0004AAFE
		public void OnEditMyAccountButtonClicked()
		{
			Application.OpenURL("https://account.asmodee.net/profile");
		}

		// Token: 0x060038D2 RID: 14546 RVA: 0x0004C90A File Offset: 0x0004AB0A
		public void OnGetMyPasswordButtonClicked()
		{
			if (this._resetPasswordEndpoint == null)
			{
				this.getMyPasswordButton.interactable = false;
				this._resetPasswordEndpoint = new ResetPasswordEndpoint(null);
				this._resetPasswordEndpoint.Execute(delegate(WebError webError)
				{
					this.getMyPasswordButton.interactable = true;
					if (this.getMyPasswordResultLabel != null)
					{
						if (webError == null)
						{
							this.getMyPasswordResultLabel.text = "We sent you an e-mail. Please follow the instructions";
						}
						else
						{
							this.getMyPasswordResultLabel.text = "An error occured, please try again later";
						}
						this.getMyPasswordResultLabel.gameObject.SetActive(true);
						this.getMyPasswordButton.gameObject.SetActive(false);
					}
					this._resetPasswordEndpoint = null;
				});
			}
		}

		// Token: 0x060038D3 RID: 14547 RVA: 0x00027EF0 File Offset: 0x000260F0
		public void OnUpdateMyEmailButtonClicked()
		{
		}

		// Token: 0x04002AAE RID: 10926
		private const string _documentation = "<b>UserAccountContentController</b> displays account informations to an authenticated user. It also allows basic actions: logout, password rest, email update...";

		// Token: 0x04002AAF RID: 10927
		public RectTransform container;

		// Token: 0x04002AB0 RID: 10928
		public bool shouldDisplayCloseButton = true;

		// Token: 0x04002AB1 RID: 10929
		public GameObject content;

		// Token: 0x04002AB2 RID: 10930
		public GameObject activityIndicator;

		// Token: 0x04002AB3 RID: 10931
		public GameObject errorIndicator;

		// Token: 0x04002AB4 RID: 10932
		public GameObject notAuthenticatedIndicator;

		// Token: 0x04002AB5 RID: 10933
		[Header("User Informations")]
		public TextMeshProUGUI title;

		// Token: 0x04002AB6 RID: 10934
		public Image avatarImage;

		// Token: 0x04002AB7 RID: 10935
		public TextMeshProUGUI nameLabel;

		// Token: 0x04002AB8 RID: 10936
		public TextMeshProUGUI loginNameLabel;

		// Token: 0x04002AB9 RID: 10937
		public TextMeshProUGUI userIdentifierLabel;

		// Token: 0x04002ABA RID: 10938
		public TextMeshProUGUI emailLabel;

		// Token: 0x04002ABB RID: 10939
		public TextMeshProUGUI registeredLabel;

		// Token: 0x04002ABC RID: 10940
		public GameObject steamContainer;

		// Token: 0x04002ABD RID: 10941
		public TextMeshProUGUI steamIdentifierLabel;

		// Token: 0x04002ABE RID: 10942
		public GameObject steamWarningStatus;

		// Token: 0x04002ABF RID: 10943
		public GameObject coppaIndicator;

		// Token: 0x04002AC2 RID: 10946
		public FontSizeHomogenizer[] fontSizeHomogenizers;

		// Token: 0x04002AC3 RID: 10947
		[Header("Actions")]
		public Button closeButton;

		// Token: 0x04002AC4 RID: 10948
		public Button logoutButton;

		// Token: 0x04002AC5 RID: 10949
		public Button getMyPasswordButton;

		// Token: 0x04002AC6 RID: 10950
		public TextMeshProUGUI getMyPasswordResultLabel;

		// Token: 0x04002AC7 RID: 10951
		public Button editMyProfileButton;

		// Token: 0x04002AC8 RID: 10952
		public Button updateMyEmailButton;

		// Token: 0x04002AC9 RID: 10953
		private float _containerAspect;

		// Token: 0x04002ACA RID: 10954
		private bool _needsLayout;

		// Token: 0x04002ACB RID: 10955
		private ResetPasswordEndpoint _resetPasswordEndpoint;

		// Token: 0x04002ACC RID: 10956
		private AvatarManager.RetrievalHandle _avatarRetrievalHandle;

		// Token: 0x04002ACD RID: 10957
		private UserAccountContentController.State _state;

		// Token: 0x04002ACE RID: 10958
		private bool _displaySDKVersion;

		// Token: 0x020007B5 RID: 1973
		private enum State
		{
			// Token: 0x04002AD0 RID: 10960
			Unknown,
			// Token: 0x04002AD1 RID: 10961
			NotAuthenticated,
			// Token: 0x04002AD2 RID: 10962
			Authenticating,
			// Token: 0x04002AD3 RID: 10963
			FetchingUserData,
			// Token: 0x04002AD4 RID: 10964
			ValidUserData,
			// Token: 0x04002AD5 RID: 10965
			Error
		}
	}
}
