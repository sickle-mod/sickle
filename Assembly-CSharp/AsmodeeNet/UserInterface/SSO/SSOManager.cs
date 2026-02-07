using System;
using System.Collections;
using AsmodeeNet.Analytics;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network;
using AsmodeeNet.Network.RestApi;
using AsmodeeNet.Utils;
using UnityEngine;
using Zxcvbn;

namespace AsmodeeNet.UserInterface.SSO
{
	// Token: 0x020007EE RID: 2030
	public class SSOManager : MonoBehaviour
	{
		// Token: 0x060039F6 RID: 14838 RVA: 0x0014FAE4 File Offset: 0x0014DCE4
		public static void InstantiateSSO(SSOAuthenticate authenticate, OnSSOSucceeded successCallback, AbortSSO abortCallback)
		{
			if ((SSOManager)global::UnityEngine.Object.FindObjectOfType(typeof(SSOManager)) == null)
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(CoreApplication.Instance.InterfaceSkin.ssoPrefab);
				global::UnityEngine.Object.DontDestroyOnLoad(gameObject);
				Action <>9__2;
				Action <>9__3;
				gameObject.GetComponent<SSOManager>().Init(authenticate, delegate
				{
					ResponsivePopUp component = gameObject.GetComponent<ResponsivePopUp>();
					Action action;
					if ((action = <>9__2) == null)
					{
						action = (<>9__2 = delegate
						{
							global::UnityEngine.Object.Destroy(gameObject);
							if (successCallback != null)
							{
								successCallback();
							}
						});
					}
					component.FadeOut(action);
				}, delegate
				{
					ResponsivePopUp component2 = gameObject.GetComponent<ResponsivePopUp>();
					Action action2;
					if ((action2 = <>9__3) == null)
					{
						action2 = (<>9__3 = delegate
						{
							global::UnityEngine.Object.Destroy(gameObject);
							if (abortCallback != null)
							{
								abortCallback();
							}
						});
					}
					component2.FadeOut(action2);
				});
				return;
			}
			AsmoLogger.Error("SSOManager", "Try to InstantiateSSO twice", null);
		}

		// Token: 0x060039F7 RID: 14839 RVA: 0x0014FB80 File Offset: 0x0014DD80
		public static void InstantiateUpdateEmailPopUp(Action<bool> onComplete)
		{
			if ((UpdateEmailPopUp)global::UnityEngine.Object.FindObjectOfType(typeof(UpdateEmailPopUp)) == null)
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(CoreApplication.Instance.InterfaceSkin.updateEmailPopUpPrefab);
				global::UnityEngine.Object.DontDestroyOnLoad(gameObject);
				gameObject.GetComponent<UpdateEmailPopUp>().Init(delegate(bool success)
				{
					if (success)
					{
						Action <>9__2;
						CoreApplication.Instance.OAuthGate.UpdateUserDetails(delegate
						{
							ResponsivePopUp component = gameObject.GetComponent<ResponsivePopUp>();
							Action action;
							if ((action = <>9__2) == null)
							{
								action = (<>9__2 = delegate
								{
									global::UnityEngine.Object.Destroy(gameObject);
									if (onComplete != null)
									{
										onComplete(success);
									}
								});
							}
							component.FadeOut(action);
						}, false);
						return;
					}
					gameObject.GetComponent<ResponsivePopUp>().FadeOut(delegate
					{
						global::UnityEngine.Object.Destroy(gameObject);
						if (onComplete != null)
						{
							onComplete(success);
						}
					});
				});
				return;
			}
			AsmoLogger.Error("SSOManager", "Try to InstantiateUpdateEmailPopUp twice", null);
			onComplete(false);
		}

		// Token: 0x060039F8 RID: 14840 RVA: 0x0004D496 File Offset: 0x0004B696
		public void Init(SSOAuthenticate ssoAuthenticate, OnSSOSucceeded onSSOSucceded, AbortSSO abortSSO)
		{
			this._SSOAuthenticate = ssoAuthenticate;
			this._onSSOSucceed = onSSOSucceded;
			this._abortSSO = abortSSO;
			AnalyticsEvents.LogConnectAsmodeeNetStartEvent("automatic", null);
			this._DisplayDoYouHaveAnAccountPanel();
		}

		// Token: 0x060039F9 RID: 14841 RVA: 0x0004D4BE File Offset: 0x0004B6BE
		public void Abort()
		{
			this._AbortLoginPanel();
			this._AbortPasswordPanel();
			this._AbortWelcomePanel();
			this._AbortChooseALoginNamePanel();
			this._HideAllPanels();
			AnalyticsEvents.LogConnectAsmodeeNetStopEvent(false, this._lastError, this._didResetPassword, null);
			this._abortSSO();
		}

		// Token: 0x060039FA RID: 14842 RVA: 0x0014FC14 File Offset: 0x0014DE14
		private void _HideAllPanels()
		{
			this._ui.DoYouHaveAnAccountPanel.Hide();
			this._ui.LoginPanel.Hide();
			this._ui.PasswordPanel.Hide();
			this._ui.ChooseALoginNamePanel.Hide();
			this._ui.WelcomePanel.Hide();
			this._ui.OkPanel.Hide();
		}

		// Token: 0x060039FB RID: 14843 RVA: 0x0004D4FC File Offset: 0x0004B6FC
		private void _DisplayDoYouHaveAnAccountPanel()
		{
			this._HideAllPanels();
			this._ui.DoYouHaveAnAccountPanel.Show();
		}

		// Token: 0x060039FC RID: 14844 RVA: 0x0004D514 File Offset: 0x0004B714
		public void DoYouHaveAnAccountPanel_No()
		{
			this._DisplayWelcomePanel(true);
		}

		// Token: 0x060039FD RID: 14845 RVA: 0x0004D51D File Offset: 0x0004B71D
		public void DoYouHaveAnAccountPanel_Yes()
		{
			this._DisplayLoginPanel(true);
		}

		// Token: 0x060039FE RID: 14846 RVA: 0x0004D526 File Offset: 0x0004B726
		private void _DisplayLoginPanel(bool resetLogin = true)
		{
			this._HideAllPanels();
			this._ui.LoginPanel.Show(resetLogin);
		}

		// Token: 0x060039FF RID: 14847 RVA: 0x0004D53F File Offset: 0x0004B73F
		private void _AbortLoginPanel()
		{
			if (this._searchByLoginOrEmailEndpoint != null)
			{
				this._searchByLoginOrEmailEndpoint.Abort();
				this._searchByLoginOrEmailEndpoint = null;
			}
			this._ui.LoginPanel.SwitchWaitingPanelMode(false, -1);
		}

		// Token: 0x06003A00 RID: 14848 RVA: 0x0004D56D File Offset: 0x0004B76D
		public void LoginPanel_No()
		{
			this._AbortLoginPanel();
			this._DisplayDoYouHaveAnAccountPanel();
		}

		// Token: 0x06003A01 RID: 14849 RVA: 0x0014FC84 File Offset: 0x0014DE84
		public void LoginPanel_Yes()
		{
			if (!this._ui.LoginPanel.AreRequirementsMet)
			{
				return;
			}
			this._ui.LoginPanel.SwitchWaitingPanelMode(true, 1);
			string text = this._ui.LoginPanel.Text;
			try
			{
				if (EmailFormatValidator.IsValidEmail(text))
				{
					this._searchByLoginOrEmailEndpoint = new SearchByEmailEndpoint(text, Extras.None, null);
				}
				else
				{
					this._searchByLoginOrEmailEndpoint = new SearchByLoginEndpoint(text, Extras.None, -1, -1, null);
				}
			}
			catch (Exception ex)
			{
				this._LoginOrMailNotFound(CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.LoginPanel.LoginEmailInvalidMessage"));
				AsmoLogger.Error("SSOManager", ex.Message, null);
				return;
			}
			this._searchByLoginOrEmailEndpoint.Execute(delegate(PaginatedResult<UserSearchResult> result, WebError webError)
			{
				this._ui.LoginPanel.SwitchWaitingPanelMode(false, -1);
				if (webError == null)
				{
					if (result.TotalElement > 0)
					{
						this._LoginOrMailFound(result.Elements[0]);
					}
					else
					{
						this._LoginOrMailNotFound(CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.Error.LoginOrEmailNotFound"));
					}
				}
				else
				{
					this._lastError = webError.ToString();
					string text2;
					if (webError.ToChildError<ApiResponseError>() != null)
					{
						text2 = string.Format("{0}\n{1}", CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.Error.LoginOrEmailNotFound"), webError);
					}
					else
					{
						text2 = CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.Error.UnableToConnect");
					}
					this._DisplayOkPanel(CoreApplication.Instance.LocalizationManager.GetLocalizedText("Standard.Error.Title"), text2, delegate
					{
						this._DisplayLoginPanel(false);
					}, SSOOKPanel.MessageType.Error);
				}
				this._searchByLoginOrEmailEndpoint = null;
			});
		}

		// Token: 0x06003A02 RID: 14850 RVA: 0x0004D57B File Offset: 0x0004B77B
		private void _LoginOrMailFound(UserSearchResult user)
		{
			this._user = user;
			this._DisplayPasswordPanel();
		}

		// Token: 0x06003A03 RID: 14851 RVA: 0x0004D58A File Offset: 0x0004B78A
		private void _LoginOrMailNotFound(string errorMessage)
		{
			this._ui.LoginPanel.DisplayErrorMessage(errorMessage);
			this._ui.LoginPanel.SelectInputFieldContent();
		}

		// Token: 0x06003A04 RID: 14852 RVA: 0x0004D5AD File Offset: 0x0004B7AD
		private void _DisplayPasswordPanel()
		{
			this._HideAllPanels();
			this._ui.PasswordPanel.Show(true);
		}

		// Token: 0x06003A05 RID: 14853 RVA: 0x0004D5C6 File Offset: 0x0004B7C6
		private void _AbortPasswordPanel()
		{
			if (this._resetPasswordEndpoint != null)
			{
				this._resetPasswordEndpoint.Abort();
				this._resetPasswordEndpoint = null;
			}
			this._ui.PasswordPanel.SwitchWaitingPanelMode(false, -1);
		}

		// Token: 0x06003A06 RID: 14854 RVA: 0x0004D5F4 File Offset: 0x0004B7F4
		public void PasswordPanel_No()
		{
			this._AbortPasswordPanel();
			this._user = null;
			this._DisplayLoginPanel(false);
		}

		// Token: 0x06003A07 RID: 14855 RVA: 0x0014FD48 File Offset: 0x0014DF48
		public void PasswordPanel_Yes()
		{
			if (!this._ui.PasswordPanel.AreRequirementsMet)
			{
				return;
			}
			this._ui.PasswordPanel.SwitchWaitingPanelMode(true, 1);
			string text = this._ui.PasswordPanel.Text;
			this._SSOAuthenticate(this._user.LoginName, text, delegate(OAuthError error)
			{
				this._ui.PasswordPanel.SwitchWaitingPanelMode(false, -1);
				if (error == null)
				{
					AnalyticsEvents.LogConnectAsmodeeNetStopEvent(true, this._lastError, this._didResetPassword, null);
					this._onSSOSucceed();
					return;
				}
				if (error.status / 100 == 4)
				{
					this._ui.PasswordPanel.DisplayErrorMessage(CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.Error.BadLoginPasswordCombination"));
					this._ui.PasswordPanel.SelectInputFieldContent();
					return;
				}
				this._DisplayOkPanel(CoreApplication.Instance.LocalizationManager.GetLocalizedText("Standard.Error.Title"), CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.Error.UnableToConnect"), delegate
				{
					this._DisplayPasswordPanel();
				}, SSOOKPanel.MessageType.Error);
			});
		}

		// Token: 0x06003A08 RID: 14856 RVA: 0x0014FDB0 File Offset: 0x0014DFB0
		public void PasswordPanel_ResetPassword()
		{
			this._ui.PasswordPanel.SwitchWaitingPanelMode(true, 2);
			this._resetPasswordEndpoint = new ResetPasswordEndpoint(this._user.UserId, null);
			this._resetPasswordEndpoint.Execute(delegate(WebError webError)
			{
				if (webError != null)
				{
					this._lastError = webError.ToString();
				}
				this._ui.PasswordPanel.SwitchWaitingPanelMode(false, -1);
				string text = ((webError == null) ? CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.PasswordReset.Title") : CoreApplication.Instance.LocalizationManager.GetLocalizedText("Standard.Error.Title"));
				string text2 = ((webError == null) ? CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.PasswordReset.Message") : CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.Error.UnableToConnect"));
				SSOOKPanel.MessageType messageType = ((webError == null) ? SSOOKPanel.MessageType.Standard : SSOOKPanel.MessageType.Error);
				this._DisplayOkPanel(text, text2, delegate
				{
					this._DisplayPasswordPanel();
				}, messageType);
				this._resetPasswordEndpoint = null;
				this._didResetPassword = true;
			});
		}

		// Token: 0x06003A09 RID: 14857 RVA: 0x0004D60A File Offset: 0x0004B80A
		private void _DisplayWelcomePanel(bool resetEmail = true)
		{
			this._HideAllPanels();
			this._ui.WelcomePanel.Show(resetEmail);
		}

		// Token: 0x06003A0A RID: 14858 RVA: 0x0004D623 File Offset: 0x0004B823
		private void _AbortWelcomePanel()
		{
			if (this._searchByEmailEndpoint != null)
			{
				this._searchByEmailEndpoint.Abort();
				this._searchByEmailEndpoint = null;
			}
			this._ui.WelcomePanel.SwitchWaitingPanelMode(false, -1);
		}

		// Token: 0x06003A0B RID: 14859 RVA: 0x0004D651 File Offset: 0x0004B851
		public void WelcomePanel_No()
		{
			this._AbortWelcomePanel();
			this._user = null;
			this._DisplayDoYouHaveAnAccountPanel();
		}

		// Token: 0x06003A0C RID: 14860 RVA: 0x0014FE00 File Offset: 0x0014E000
		public void WelcomePanel_Yes()
		{
			if (!this._ui.WelcomePanel.AreRequirementsMet)
			{
				return;
			}
			this._ui.WelcomePanel.SwitchWaitingPanelMode(true, 1);
			string email = this._ui.WelcomePanel.Email;
			this._searchByEmailEndpoint = new SearchByEmailEndpoint(email, Extras.None, null);
			this._searchByEmailEndpoint.Execute(delegate(PaginatedResult<UserSearchResult> result, WebError webError)
			{
				this._ui.WelcomePanel.SwitchWaitingPanelMode(false, -1);
				if (webError == null)
				{
					if (result.TotalElement > 0)
					{
						this._WelcomePanelEmailFound(result.Elements[0]);
					}
					else
					{
						this._WelcomePanelContinueAccountCreation();
					}
				}
				else
				{
					this._lastError = webError.ToString();
					this._DisplayOkPanel(CoreApplication.Instance.LocalizationManager.GetLocalizedText("Standard.Error.Title"), CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.Error.UnableToConnect"), delegate
					{
						this._DisplayWelcomePanel(false);
					}, SSOOKPanel.MessageType.Error);
				}
				this._searchByEmailEndpoint = null;
			});
		}

		// Token: 0x06003A0D RID: 14861 RVA: 0x0004D666 File Offset: 0x0004B866
		private void _WelcomePanelEmailFound(UserSearchResult user)
		{
			this._user = user;
			this._ui.LoginPanel.Text = this._ui.WelcomePanel.Email;
			this._DisplayPasswordPanel();
		}

		// Token: 0x06003A0E RID: 14862 RVA: 0x0004D695 File Offset: 0x0004B895
		private void _WelcomePanelContinueAccountCreation()
		{
			this._DisplayChooseALoginNamePanel();
		}

		// Token: 0x06003A0F RID: 14863 RVA: 0x0004D69D File Offset: 0x0004B89D
		private void _DisplayChooseALoginNamePanel()
		{
			this._HideAllPanels();
			this._ui.ChooseALoginNamePanel.Show();
		}

		// Token: 0x06003A10 RID: 14864 RVA: 0x0014FE68 File Offset: 0x0014E068
		private void _AbortChooseALoginNamePanel()
		{
			if (this._userSignUpEndpoint != null)
			{
				this._userSignUpEndpoint.Abort();
				this._userSignUpEndpoint = null;
			}
			if (this._searchByLoginEndpoint != null)
			{
				this._searchByLoginEndpoint.Abort();
				this._searchByLoginEndpoint = null;
			}
			this._ui.ChooseALoginNamePanel.SwitchWaitingPanelMode(false, -1);
		}

		// Token: 0x06003A11 RID: 14865 RVA: 0x0004D6B5 File Offset: 0x0004B8B5
		public void ChooseALoginNamePanel_No()
		{
			this._AbortChooseALoginNamePanel();
			this._DisplayWelcomePanel(true);
		}

		// Token: 0x06003A12 RID: 14866 RVA: 0x0014FEBC File Offset: 0x0014E0BC
		public void ChooseALoginNamePanel_Yes()
		{
			if (!this._ui.ChooseALoginNamePanel.AreRequirementsMet)
			{
				return;
			}
			try
			{
				this._userSignUpEndpoint = new UserSignUpEndpoint(this._ui.ChooseALoginNamePanel.LoginName, this._ui.ChooseALoginNamePanel.Password, this._ui.WelcomePanel.Email, this._ui.WelcomePanel.SubscribeToNewsletter, null);
			}
			catch (Exception ex)
			{
				this._userSignUpEndpoint = null;
				this._DisplayOkPanel(CoreApplication.Instance.LocalizationManager.GetLocalizedText("Standard.Error.Title"), ex.Message, delegate
				{
					this._DisplayChooseALoginNamePanel();
				}, SSOOKPanel.MessageType.Error);
				return;
			}
			this._userSignUpEndpoint.Execute(delegate(ApiSignUpResponse endpoint, WebError webError)
			{
				if (webError == null)
				{
					this._SSOAuthenticate(this._ui.ChooseALoginNamePanel.LoginName, this._ui.ChooseALoginNamePanel.Password, delegate(OAuthError error)
					{
						this._ui.ChooseALoginNamePanel.SwitchWaitingPanelMode(false, -1);
						if (error == null)
						{
							this._DisplayOkPanel(CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.AccountCreationSuccess.Title"), CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.AccountCreationSuccess.Message"), delegate
							{
								AnalyticsEvents.LogCreateAsmodeeNetAccountEvent(this._ui.WelcomePanel.SubscribeToNewsletter, null);
								AnalyticsEvents.LogConnectAsmodeeNetStopEvent(true, this._lastError, this._didResetPassword, null);
								this._onSSOSucceed();
							}, SSOOKPanel.MessageType.Standard);
							return;
						}
						this._DisplayOkPanel(CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.AccountCreationSuccess.Title"), CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.AccountCreationSuccess.LoginErrorMessage"), delegate
						{
							this._DisplayDoYouHaveAnAccountPanel();
						}, SSOOKPanel.MessageType.Standard);
					});
				}
				else
				{
					this._lastError = webError.ToString();
					this._ui.ChooseALoginNamePanel.SwitchWaitingPanelMode(false, -1);
					if (webError.ToChildError<ApiResponseError>() != null)
					{
						this._DisplayOkPanel(CoreApplication.Instance.LocalizationManager.GetLocalizedText("Standard.Error.Title"), CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.Error.UnexpectedSignUpError"), delegate
						{
							this._DisplayWelcomePanel(true);
						}, SSOOKPanel.MessageType.Error);
					}
					else
					{
						this._DisplayOkPanel(CoreApplication.Instance.LocalizationManager.GetLocalizedText("Standard.Error.Title"), CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.Error.UnableToConnect"), delegate
						{
							this._DisplayChooseALoginNamePanel();
						}, SSOOKPanel.MessageType.Error);
					}
				}
				this._userSignUpEndpoint = null;
			});
			this._ui.ChooseALoginNamePanel.SwitchWaitingPanelMode(true, 1);
		}

		// Token: 0x06003A13 RID: 14867 RVA: 0x0014FF9C File Offset: 0x0014E19C
		public void ChooseALoginNamePanel_OnLoginNameInputChange(string value)
		{
			if (value.Length < 4)
			{
				this._ui.ChooseALoginNamePanel.ShowLoginNameState(SSOChooseALoginNamePanel.LoginState.Unavailable);
				return;
			}
			this._ui.ChooseALoginNamePanel.ShowLoginNameState(SSOChooseALoginNamePanel.LoginState.Searching);
			base.StopAllCoroutines();
			base.StartCoroutine(this._ChooseALoginNamePanel_OnLoginNameInputChange(value, this._delayBetweenChecks));
		}

		// Token: 0x06003A14 RID: 14868 RVA: 0x0004D6C4 File Offset: 0x0004B8C4
		private IEnumerator _ChooseALoginNamePanel_OnLoginNameInputChange(string value, float delay)
		{
			yield return new WaitForSeconds(delay);
			try
			{
				this._searchByLoginEndpoint = new SearchByLoginEndpoint(value, Extras.None, -1, -1, null);
			}
			catch (Exception ex)
			{
				this._searchByLoginEndpoint = null;
				this._DisplayOkPanel(CoreApplication.Instance.LocalizationManager.GetLocalizedText("Standard.Error.Title"), ex.Message, delegate
				{
					this._DisplayChooseALoginNamePanel();
				}, SSOOKPanel.MessageType.Error);
				yield break;
			}
			this._searchByLoginEndpoint.Execute(delegate(PaginatedResult<UserSearchResult> result, WebError webError)
			{
				if (webError == null)
				{
					if (result.TotalElement > 0)
					{
						this._ui.ChooseALoginNamePanel.ShowLoginNameState(SSOChooseALoginNamePanel.LoginState.Unavailable);
					}
					else
					{
						this._ui.ChooseALoginNamePanel.ShowLoginNameState(SSOChooseALoginNamePanel.LoginState.Available);
					}
				}
				else
				{
					this._lastError = webError.ToString();
					this._DisplayOkPanel(CoreApplication.Instance.LocalizationManager.GetLocalizedText("Standard.Error.Title"), CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.Error.UnableToConnect"), delegate
					{
						this._DisplayChooseALoginNamePanel();
					}, SSOOKPanel.MessageType.Error);
				}
				this._searchByLoginEndpoint = null;
			});
			yield break;
		}

		// Token: 0x06003A15 RID: 14869 RVA: 0x0004D6E1 File Offset: 0x0004B8E1
		public void ChooseALoginNamePanel_OnPasswordInputChange(string value)
		{
			this._ui.ChooseALoginNamePanel.ShowPasswordStrength(global::Zxcvbn.Zxcvbn.MatchPassword(value, null).Score);
		}

		// Token: 0x06003A16 RID: 14870 RVA: 0x0004D6FF File Offset: 0x0004B8FF
		private void _DisplayOkPanel(string title, string message, Action onHideAction, SSOOKPanel.MessageType messageType = SSOOKPanel.MessageType.Standard)
		{
			this._HideAllPanels();
			this._ui.OkPanel.Show(title, message, onHideAction, messageType);
		}

		// Token: 0x04002BCC RID: 11212
		private const string _kModuleName = "SSOManager";

		// Token: 0x04002BCD RID: 11213
		[SerializeField]
		private SSOManager.UI _ui;

		// Token: 0x04002BCE RID: 11214
		private UserSearchResult _user;

		// Token: 0x04002BCF RID: 11215
		private bool _didResetPassword;

		// Token: 0x04002BD0 RID: 11216
		private string _lastError;

		// Token: 0x04002BD1 RID: 11217
		private SSOAuthenticate _SSOAuthenticate;

		// Token: 0x04002BD2 RID: 11218
		private OnSSOSucceeded _onSSOSucceed;

		// Token: 0x04002BD3 RID: 11219
		private AbortSSO _abortSSO;

		// Token: 0x04002BD4 RID: 11220
		private EndpointWithPaginatedResponse<UserSearchResult> _searchByLoginOrEmailEndpoint;

		// Token: 0x04002BD5 RID: 11221
		private ResetPasswordEndpoint _resetPasswordEndpoint;

		// Token: 0x04002BD6 RID: 11222
		private SearchByEmailEndpoint _searchByEmailEndpoint;

		// Token: 0x04002BD7 RID: 11223
		private UserSignUpEndpoint _userSignUpEndpoint;

		// Token: 0x04002BD8 RID: 11224
		private SearchByLoginEndpoint _searchByLoginEndpoint;

		// Token: 0x04002BD9 RID: 11225
		private float _delayBetweenChecks = 0.25f;

		// Token: 0x020007EF RID: 2031
		[Serializable]
		public class UI
		{
			// Token: 0x04002BDA RID: 11226
			public SSOBasePanel DoYouHaveAnAccountPanel;

			// Token: 0x04002BDB RID: 11227
			public SSOLoginAndPasswordPanel LoginPanel;

			// Token: 0x04002BDC RID: 11228
			public SSOLoginAndPasswordPanel PasswordPanel;

			// Token: 0x04002BDD RID: 11229
			public SSOChooseALoginNamePanel ChooseALoginNamePanel;

			// Token: 0x04002BDE RID: 11230
			public SSOWelcomePanel WelcomePanel;

			// Token: 0x04002BDF RID: 11231
			public SSOOKPanel OkPanel;
		}
	}
}
