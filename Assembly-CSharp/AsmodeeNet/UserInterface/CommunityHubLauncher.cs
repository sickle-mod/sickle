using System;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000795 RID: 1941
	public class CommunityHubLauncher : MonoBehaviour
	{
		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06003821 RID: 14369 RVA: 0x0004C0C5 File Offset: 0x0004A2C5
		public CommunityHub CommunityHub
		{
			get
			{
				return this._communityHub;
			}
		}

		// Token: 0x14000141 RID: 321
		// (add) Token: 0x06003822 RID: 14370 RVA: 0x00148E94 File Offset: 0x00147094
		// (remove) Token: 0x06003823 RID: 14371 RVA: 0x00148ECC File Offset: 0x001470CC
		public event Action communityHubDidStart;

		// Token: 0x14000142 RID: 322
		// (add) Token: 0x06003824 RID: 14372 RVA: 0x00148F04 File Offset: 0x00147104
		// (remove) Token: 0x06003825 RID: 14373 RVA: 0x00148F3C File Offset: 0x0014713C
		public event Action communityHubDidStop;

		// Token: 0x06003826 RID: 14374 RVA: 0x0004C0CD File Offset: 0x0004A2CD
		private void Start()
		{
			if (this.autoLaunch)
			{
				this.LaunchCommunityHub();
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06003827 RID: 14375 RVA: 0x0004C0DD File Offset: 0x0004A2DD
		public bool IsCommunityHubLaunched
		{
			get
			{
				return this._communityHub != null;
			}
		}

		// Token: 0x06003828 RID: 14376 RVA: 0x00148F74 File Offset: 0x00147174
		public void LaunchCommunityHub()
		{
			if (this.IsCommunityHubLaunched)
			{
				return;
			}
			AsmoLogger.Debug("CommunityHubLauncher", "Launch Community Hub", null);
			if (this.communityHubSkin == null)
			{
				AsmoLogger.Warning("CommunityHubLauncher", "Skin not provided -> Fall back to default", null);
				this.communityHubSkin = global::UnityEngine.Object.Instantiate(Resources.Load("CommunityHubDefaultSkin", typeof(CommunityHubSkin))) as CommunityHubSkin;
			}
			this._communityHub = base.gameObject.AddComponent<CommunityHub>();
			if (this.communityHubDidStart != null)
			{
				this.communityHubDidStart();
			}
		}

		// Token: 0x06003829 RID: 14377 RVA: 0x0004C0EB File Offset: 0x0004A2EB
		public void StopCommunityHub()
		{
			if (!this.IsCommunityHubLaunched)
			{
				return;
			}
			AsmoLogger.Debug("CommunityHubLauncher", "Stop Community Hub", null);
			global::UnityEngine.Object.Destroy(this._communityHub);
			this._communityHub = null;
			if (this.communityHubDidStop != null)
			{
				this.communityHubDidStop();
			}
		}

		// Token: 0x04002A18 RID: 10776
		private const string _documentation = "<b>Community Hub</b> offers standard UI elements:\n- SSO\n- Players lists\n- Chat";

		// Token: 0x04002A19 RID: 10777
		private const string _consoleModuleName = "CommunityHubLauncher";

		// Token: 0x04002A1A RID: 10778
		private CommunityHub _communityHub;

		// Token: 0x04002A1D RID: 10781
		public CommunityHubSkin communityHubSkin;

		// Token: 0x04002A1E RID: 10782
		public bool autoLaunch;
	}
}
