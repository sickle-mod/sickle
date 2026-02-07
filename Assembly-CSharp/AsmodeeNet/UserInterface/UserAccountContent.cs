using System;
using AsmodeeNet.Foundation;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007B3 RID: 1971
	public class UserAccountContent : CommunityHubContent
	{
		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x060038B5 RID: 14517 RVA: 0x000283F8 File Offset: 0x000265F8
		public override CommunityHubContent.Layout DisplayedLayout
		{
			get
			{
				return CommunityHubContent.Layout.Tab;
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x060038B6 RID: 14518 RVA: 0x0004C776 File Offset: 0x0004A976
		public UserAccountContentController Controller
		{
			get
			{
				return this._controller;
			}
		}

		// Token: 0x14000148 RID: 328
		// (add) Token: 0x060038B7 RID: 14519 RVA: 0x0014B844 File Offset: 0x00149A44
		// (remove) Token: 0x060038B8 RID: 14520 RVA: 0x0014B87C File Offset: 0x00149A7C
		public event Action UserAccountDidClose;

		// Token: 0x14000149 RID: 329
		// (add) Token: 0x060038B9 RID: 14521 RVA: 0x0014B8B4 File Offset: 0x00149AB4
		// (remove) Token: 0x060038BA RID: 14522 RVA: 0x0014B8EC File Offset: 0x00149AEC
		public event Action UserDidLogOut;

		// Token: 0x060038BB RID: 14523 RVA: 0x0014B924 File Offset: 0x00149B24
		public override void LoadDisplayedContent(Preferences.DisplayMode displayMode)
		{
			if (this._root != null || this._controller != null)
			{
				return;
			}
			CommunityHubSkin communityHubSkin = CoreApplication.Instance.CommunityHubLauncher.communityHubSkin;
			this._root = global::UnityEngine.Object.Instantiate<GameObject>(communityHubSkin.userAccountContentPrefabs[displayMode]);
			this._controller = this._root.GetComponent<UserAccountContentController>();
			if (this._root == null || this._controller == null)
			{
				AsmoLogger.Debug("UserAccountContent", "Incomplete UserAccountContent Prefab", null);
				this.DisposeDisplayedContent();
			}
			else
			{
				this._controller.shouldDisplayCloseButton = this.shouldDisplayCloseButton;
				this._controller.UserAccountDidClose += this._UserAccountDidClose;
				this._controller.UserDidLogOut += this._UserDidLogOut;
			}
			base.TabIcons = communityHubSkin.userAccountContentIcons;
		}

		// Token: 0x060038BC RID: 14524 RVA: 0x0004C77E File Offset: 0x0004A97E
		public override void DisposeDisplayedContent()
		{
			global::UnityEngine.Object.Destroy(this._root);
			this._root = null;
			this._controller = null;
		}

		// Token: 0x060038BD RID: 14525 RVA: 0x0004C799 File Offset: 0x0004A999
		private void _UserAccountDidClose()
		{
			if (this.UserAccountDidClose != null)
			{
				this.UserAccountDidClose();
			}
		}

		// Token: 0x060038BE RID: 14526 RVA: 0x0004C7AE File Offset: 0x0004A9AE
		private void _UserDidLogOut()
		{
			if (this.UserDidLogOut != null)
			{
				this.UserDidLogOut();
			}
		}

		// Token: 0x04002AA9 RID: 10921
		private const string _debugModuleName = "UserAccountContent";

		// Token: 0x04002AAA RID: 10922
		private UserAccountContentController _controller;

		// Token: 0x04002AAD RID: 10925
		public bool shouldDisplayCloseButton = true;
	}
}
