using System;
using AsmodeeNet.Foundation;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007A3 RID: 1955
	public class LoginBannerContent : CommunityHubContent
	{
		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x0600386A RID: 14442 RVA: 0x0002A1D9 File Offset: 0x000283D9
		public override CommunityHubContent.Layout DisplayedLayout
		{
			get
			{
				return CommunityHubContent.Layout.Header;
			}
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x0600386B RID: 14443 RVA: 0x0004C3AF File Offset: 0x0004A5AF
		public LoginBannerContentController Controller
		{
			get
			{
				return this._controller;
			}
		}

		// Token: 0x14000144 RID: 324
		// (add) Token: 0x0600386C RID: 14444 RVA: 0x0014A8B4 File Offset: 0x00148AB4
		// (remove) Token: 0x0600386D RID: 14445 RVA: 0x0014A8EC File Offset: 0x00148AEC
		public event Action LoginBannerDidSelectAccount;

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x0600386E RID: 14446 RVA: 0x0004C3B7 File Offset: 0x0004A5B7
		// (set) Token: 0x0600386F RID: 14447 RVA: 0x0004C3BF File Offset: 0x0004A5BF
		public bool AllowAutoCollapse
		{
			get
			{
				return this._allowAutoCollapse;
			}
			set
			{
				this._allowAutoCollapse = value;
				if (this._controller != null)
				{
					this._controller.AllowAutoCollapse = this._allowAutoCollapse;
				}
			}
		}

		// Token: 0x06003870 RID: 14448 RVA: 0x0014A924 File Offset: 0x00148B24
		public override void LoadDisplayedContent(Preferences.DisplayMode displayMode)
		{
			if (this._root != null || this._controller != null)
			{
				return;
			}
			CommunityHubSkin communityHubSkin = CoreApplication.Instance.CommunityHubLauncher.communityHubSkin;
			this._root = global::UnityEngine.Object.Instantiate<GameObject>(communityHubSkin.loginBannerContentPrefabs[displayMode]);
			this._controller = this._root.GetComponent<LoginBannerContentController>();
			if (this._root == null || this._controller == null)
			{
				AsmoLogger.Debug("LoginBannerContent", "Incomplete LoginBannerContent Prefab", null);
				this.DisposeDisplayedContent();
				return;
			}
			this._controller.AllowAutoCollapse = this._allowAutoCollapse;
			this._controller.LoginBannerDidSelectAccount += this._LoginBannerDidSelectAccount;
		}

		// Token: 0x06003871 RID: 14449 RVA: 0x0004C3E7 File Offset: 0x0004A5E7
		public override void DisposeDisplayedContent()
		{
			global::UnityEngine.Object.Destroy(this._root);
			this._root = null;
			this._controller = null;
		}

		// Token: 0x06003872 RID: 14450 RVA: 0x0004C402 File Offset: 0x0004A602
		private void _LoginBannerDidSelectAccount()
		{
			if (this.LoginBannerDidSelectAccount != null)
			{
				this.LoginBannerDidSelectAccount();
			}
		}

		// Token: 0x04002A59 RID: 10841
		private const string _debugModuleName = "LoginBannerContent";

		// Token: 0x04002A5A RID: 10842
		private LoginBannerContentController _controller;

		// Token: 0x04002A5C RID: 10844
		private bool _allowAutoCollapse = true;
	}
}
