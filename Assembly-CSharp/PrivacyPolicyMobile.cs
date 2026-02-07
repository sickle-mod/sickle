using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000F9 RID: 249
public class PrivacyPolicyMobile : MonoBehaviour
{
	// Token: 0x0600081F RID: 2079 RVA: 0x0002D6D6 File Offset: 0x0002B8D6
	private void AcceptPrivacyPolicy()
	{
		this.SetPrivacyPolicyState(true);
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x0002D6DF File Offset: 0x0002B8DF
	private void DeclinePrivacePolicy()
	{
		this.SetPrivacyPolicyState(false);
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x0002D6DF File Offset: 0x0002B8DF
	private void WithdrawConsent()
	{
		this.SetPrivacyPolicyState(false);
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x0002D6E8 File Offset: 0x0002B8E8
	public static bool IsPrivacyPolicyAccepted()
	{
		return PlayerPrefs.HasKey("PlayerConsent") && PlayerPrefs.GetInt("PlayerConsent") == 1;
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x0002D705 File Offset: 0x0002B905
	private void SetPrivacyPolicyState(bool accepted)
	{
		PlayerPrefs.SetInt("PlayerConsent", accepted ? 1 : 0);
		if (this.privacyPolicyToggle != null)
		{
			this.privacyPolicyToggle.isOn = accepted;
		}
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x0002D732 File Offset: 0x0002B932
	private void Awake()
	{
		if (this.privacyPolicyToggle != null)
		{
			this.privacyPolicyToggle.isOn = PrivacyPolicyMobile.IsPrivacyPolicyAccepted();
		}
		if (!PrivacyPolicyMobile.IsPrivacyPolicyAccepted())
		{
			this.ShowPrivacyPolicy();
		}
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x00077ABC File Offset: 0x00075CBC
	public void ShowPrivacyPolicy()
	{
		this.menuPopup.Reset();
		this.menuPopup.Initialize(ScriptLocalization.Get("MainMenu/PrivacyPolicyHeader"), ScriptLocalization.Get("MainMenu/PrivacyPolicyContent"), ScriptLocalization.Get("MainMenu/PrivacyPolicyInfo"), 2);
		this.menuPopup.InitializeButton(0, ScriptLocalization.Get("MainMenu/Disagree"), MenuPopupUI.ButtonColor.Red);
		this.menuPopup.InitializeButton(1, ScriptLocalization.Get("MainMenu/Agree"), MenuPopupUI.ButtonColor.Green);
		this.menuPopup.OnClickButton0 += this.DeclinePrivacyPolicy_OnClick;
		this.menuPopup.OnClickButton1 += this.AcceptPrivacyPolicy_OnClick;
		this.menuPopup.Show();
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x0002D75F File Offset: 0x0002B95F
	private void AcceptPrivacyPolicy_OnClick()
	{
		this.menuPopup.Hide();
		this.AcceptPrivacyPolicy();
	}

	// Token: 0x06000827 RID: 2087 RVA: 0x0002D772 File Offset: 0x0002B972
	private void DeclinePrivacyPolicy_OnClick()
	{
		this.menuPopup.Hide();
		this.ShowPrivacyPolicyDeclineNotice();
	}

	// Token: 0x06000828 RID: 2088 RVA: 0x00077B68 File Offset: 0x00075D68
	private void ShowPrivacyPolicyDeclineNotice()
	{
		this.menuPopupWithoutDescription.Reset();
		this.menuPopupWithoutDescription.Initialize(ScriptLocalization.Get("MainMenu/Notice"), ScriptLocalization.Get("MainMenu/PrivacyPolicyNoticeContent"), "", 2);
		this.menuPopupWithoutDescription.InitializeButton(0, ScriptLocalization.Get("MainMenu/Close"), MenuPopupUI.ButtonColor.Red);
		this.menuPopupWithoutDescription.InitializeButton(1, ScriptLocalization.Get("MainMenu/Reconcider"), MenuPopupUI.ButtonColor.Green);
		this.menuPopupWithoutDescription.OnClickButton0 += this.DeclineNoticeClose_OnClick;
		this.menuPopupWithoutDescription.OnClickButton1 += this.DeclineNoticeReconcider_OnClick;
		this.menuPopupWithoutDescription.Show();
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x0002D785 File Offset: 0x0002B985
	private void DeclineNoticeClose_OnClick()
	{
		this.menuPopupWithoutDescription.Hide();
		this.DeclinePrivacePolicy();
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x0002D798 File Offset: 0x0002B998
	private void DeclineNoticeReconcider_OnClick()
	{
		this.menuPopupWithoutDescription.Hide();
		this.ShowPrivacyPolicy();
	}

	// Token: 0x0600082B RID: 2091 RVA: 0x00077C0C File Offset: 0x00075E0C
	public void ShowPrivacyPolicyWithdraw()
	{
		this.menuPopupWithoutDescription.Reset();
		this.menuPopupWithoutDescription.Initialize(ScriptLocalization.Get("MainMenu/PrivacyPolicyWithdrawHeader"), ScriptLocalization.Get("MainMenu/PrivacyPolicyWithdrawContent"), "", 2);
		this.menuPopupWithoutDescription.InitializeButton(0, ScriptLocalization.Get("Lobby/Cancel"), MenuPopupUI.ButtonColor.Green);
		this.menuPopupWithoutDescription.InitializeButton(1, ScriptLocalization.Get("MainMenu/Withdraw"), MenuPopupUI.ButtonColor.Red);
		this.menuPopupWithoutDescription.OnClickButton0 += this.PrivacyPolicyWithdrawCancel_OnClick;
		this.menuPopupWithoutDescription.OnClickButton1 += this.PrivacyPolicyWithdraw_OnClick;
		this.menuPopupWithoutDescription.Show();
	}

	// Token: 0x0600082C RID: 2092 RVA: 0x0002D7AB File Offset: 0x0002B9AB
	private void PrivacyPolicyWithdraw_OnClick()
	{
		this.menuPopupWithoutDescription.Hide();
		this.ShowPrivacyPolicyWithdrawNotice();
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x0002D7BE File Offset: 0x0002B9BE
	private void PrivacyPolicyWithdrawCancel_OnClick()
	{
		this.menuPopupWithoutDescription.Hide();
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x00077CB0 File Offset: 0x00075EB0
	private void ShowPrivacyPolicyWithdrawNotice()
	{
		this.menuPopupWithoutDescription.Reset();
		this.menuPopupWithoutDescription.Initialize(ScriptLocalization.Get("MainMenu/Notice"), ScriptLocalization.Get("MainMenu/PrivacyPolicyNoticeContent"), "", 2);
		this.menuPopupWithoutDescription.InitializeButton(0, ScriptLocalization.Get("MainMenu/Close"), MenuPopupUI.ButtonColor.Red);
		this.menuPopupWithoutDescription.InitializeButton(1, ScriptLocalization.Get("MainMenu/Reconcider"), MenuPopupUI.ButtonColor.Green);
		this.menuPopupWithoutDescription.OnClickButton0 += this.WithdrawNoticeClose_OnClick;
		this.menuPopupWithoutDescription.OnClickButton1 += this.WithdrawNoticeReconcider_OnClick;
		this.menuPopupWithoutDescription.Show();
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x0002D7CB File Offset: 0x0002B9CB
	private void WithdrawNoticeClose_OnClick()
	{
		this.menuPopupWithoutDescription.Hide();
		this.WithdrawConsent();
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x0002D7DE File Offset: 0x0002B9DE
	private void WithdrawNoticeReconcider_OnClick()
	{
		this.menuPopupWithoutDescription.Hide();
		this.ShowPrivacyPolicyWithdraw();
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x0002D7F1 File Offset: 0x0002B9F1
	public void PrivacyPolicyToggle_OnClick()
	{
		if (PrivacyPolicyMobile.IsPrivacyPolicyAccepted())
		{
			this.ShowPrivacyPolicyWithdraw();
		}
		else
		{
			this.ShowPrivacyPolicy();
		}
		this.privacyPolicyToggle.isOn = PrivacyPolicyMobile.IsPrivacyPolicyAccepted();
	}

	// Token: 0x040006E3 RID: 1763
	[SerializeField]
	private MenuPopupUI menuPopup;

	// Token: 0x040006E4 RID: 1764
	[SerializeField]
	private MenuPopupUI menuPopupWithoutDescription;

	// Token: 0x040006E5 RID: 1765
	[SerializeField]
	private Toggle privacyPolicyToggle;

	// Token: 0x040006E6 RID: 1766
	private const string PRIVACY_POLICY_HEADER = "MainMenu/PrivacyPolicyHeader";

	// Token: 0x040006E7 RID: 1767
	private const string PRIVACY_POLICY_CONTENT = "MainMenu/PrivacyPolicyContent";

	// Token: 0x040006E8 RID: 1768
	private const string PRIVACY_POLICY_INFO = "MainMenu/PrivacyPolicyInfo";

	// Token: 0x040006E9 RID: 1769
	private const string PRIVACY_POLICY_NOTICE_HEADER = "MainMenu/Notice";

	// Token: 0x040006EA RID: 1770
	private const string PRIVACY_POLICY_NOTICE_CONTENT = "MainMenu/PrivacyPolicyNoticeContent";

	// Token: 0x040006EB RID: 1771
	private const string PRIVACY_POLICY_WITHDRAW_HEADER = "MainMenu/PrivacyPolicyWithdrawHeader";

	// Token: 0x040006EC RID: 1772
	private const string PRIVACY_POLICY_WITHDRAW_CONTENT = "MainMenu/PrivacyPolicyWithdrawContent";

	// Token: 0x040006ED RID: 1773
	private const string DELETE_DATA_HEADER = "MainMenu/DeleteData";

	// Token: 0x040006EE RID: 1774
	private const string DELETE_DATA_CONTENT = "MainMenu/DeleteDataContent";

	// Token: 0x040006EF RID: 1775
	private const string LABEL_AGREE = "MainMenu/Agree";

	// Token: 0x040006F0 RID: 1776
	private const string LABEL_DISAGREE = "MainMenu/Disagree";

	// Token: 0x040006F1 RID: 1777
	private const string LABEL_CLOSE = "MainMenu/Close";

	// Token: 0x040006F2 RID: 1778
	private const string LABEL_RECONCIDER = "MainMenu/Reconcider";

	// Token: 0x040006F3 RID: 1779
	private const string LABEL_WITHDRAW = "MainMenu/Withdraw";

	// Token: 0x040006F4 RID: 1780
	private const string LABEL_CANEL = "Lobby/Cancel";

	// Token: 0x040006F5 RID: 1781
	private const string LABEL_DELETE_DATA = "MainMenu/DeleteData";

	// Token: 0x040006F6 RID: 1782
	private const string PREFFS_KEY_PRIVACY_AND_DATA_USAGE_CONSENT = "PlayerConsent";
}
