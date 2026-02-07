using System;
using AsmodeeNet.Foundation;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007A8 RID: 1960
	public class PlayersListsContent : CommunityHubContent
	{
		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x0600388D RID: 14477 RVA: 0x000283F8 File Offset: 0x000265F8
		public override CommunityHubContent.Layout DisplayedLayout
		{
			get
			{
				return CommunityHubContent.Layout.Tab;
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x0600388E RID: 14478 RVA: 0x0004C59C File Offset: 0x0004A79C
		public PlayersListsContentController Controller
		{
			get
			{
				return this._controller;
			}
		}

		// Token: 0x0600388F RID: 14479 RVA: 0x0014AEB4 File Offset: 0x001490B4
		public override void LoadDisplayedContent(Preferences.DisplayMode displayMode)
		{
			if (this._root != null || this._controller != null)
			{
				return;
			}
			CommunityHubSkin communityHubSkin = CoreApplication.Instance.CommunityHubLauncher.communityHubSkin;
			this._root = global::UnityEngine.Object.Instantiate<GameObject>(communityHubSkin.playersListsContentPrefabs[displayMode]);
			this._controller = this._root.GetComponent<PlayersListsContentController>();
			if (this._root == null || this._controller == null)
			{
				AsmoLogger.Debug("PlayersListsContent", "Incomplete PlayersListsContent Prefab", null);
				this.DisposeDisplayedContent();
			}
			base.TabIcons = communityHubSkin.playersListsContentIcons;
		}

		// Token: 0x06003890 RID: 14480 RVA: 0x0004C5A4 File Offset: 0x0004A7A4
		public override void DisposeDisplayedContent()
		{
			global::UnityEngine.Object.Destroy(this._root);
			this._root = null;
			this._controller = null;
		}

		// Token: 0x04002A80 RID: 10880
		private const string _debugModuleName = "PlayersListsContent";

		// Token: 0x04002A81 RID: 10881
		private PlayersListsContentController _controller;
	}
}
