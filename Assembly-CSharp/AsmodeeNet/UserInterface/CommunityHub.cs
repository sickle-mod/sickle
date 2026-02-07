using System;
using System.Collections.Generic;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network;
using AsmodeeNet.Network.RestApi;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x0200078E RID: 1934
	public class CommunityHub : MonoBehaviour
	{
		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x060037E8 RID: 14312 RVA: 0x0004BE1C File Offset: 0x0004A01C
		// (set) Token: 0x060037E9 RID: 14313 RVA: 0x0004BE24 File Offset: 0x0004A024
		public CommunityHubLayout CommunityHubLayout { get; private set; }

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x060037EA RID: 14314 RVA: 0x0004BE2D File Offset: 0x0004A02D
		public IList<RectTransform> TransformsToAutoLayout
		{
			get
			{
				return this._transformsToAutoLayout.AsReadOnly();
			}
		}

		// Token: 0x060037EB RID: 14315 RVA: 0x0004BE3A File Offset: 0x0004A03A
		public void AddTransformToAutoLayout(RectTransform transform)
		{
			if (!this._transformsToAutoLayout.Contains(transform))
			{
				this._transformsToAutoLayout.Add(transform);
			}
			if (this.CommunityHubLayout != null)
			{
				this.CommunityHubLayout.SetNeedsUpdateLayout();
			}
		}

		// Token: 0x060037EC RID: 14316 RVA: 0x0004BE6F File Offset: 0x0004A06F
		public void RemoveTransformToAutoLayout(RectTransform transform)
		{
			if (this._transformsToAutoLayout.Contains(transform))
			{
				this._transformsToAutoLayout.Remove(transform);
			}
			if (this.CommunityHubLayout != null)
			{
				this.CommunityHubLayout.SetNeedsUpdateLayout();
			}
		}

		// Token: 0x1400013C RID: 316
		// (add) Token: 0x060037ED RID: 14317 RVA: 0x00148968 File Offset: 0x00146B68
		// (remove) Token: 0x060037EE RID: 14318 RVA: 0x001489A0 File Offset: 0x00146BA0
		public event Action<Vector2, Vector2> layoutDidChange;

		// Token: 0x060037EF RID: 14319 RVA: 0x0004BEA5 File Offset: 0x0004A0A5
		public void CallLayoutDidChangeEvent()
		{
			if (this.layoutDidChange != null)
			{
				this.layoutDidChange(this.LayoutSafeAreaAnchorMin, this.LayoutSafeAreaAnchorMax);
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x060037F0 RID: 14320 RVA: 0x0004BEC6 File Offset: 0x0004A0C6
		public Vector2 LayoutSafeAreaAnchorMin
		{
			get
			{
				if (!(this.CommunityHubLayout != null))
				{
					return Vector2.zero;
				}
				return this.CommunityHubLayout.SafeAreaAnchorMin;
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x060037F1 RID: 14321 RVA: 0x0004BEE7 File Offset: 0x0004A0E7
		public Vector2 LayoutSafeAreaAnchorMax
		{
			get
			{
				if (!(this.CommunityHubLayout != null))
				{
					return Vector2.one;
				}
				return this.CommunityHubLayout.SafeAreaAnchorMin;
			}
		}

		// Token: 0x060037F2 RID: 14322 RVA: 0x0004BF08 File Offset: 0x0004A108
		private void Awake()
		{
			this._RegisterLiteSDKRequiredOnlineStatus();
		}

		// Token: 0x060037F3 RID: 14323 RVA: 0x0004BF10 File Offset: 0x0004A110
		private void OnEnable()
		{
			this._AttachInterface();
			if (this.CommunityHubLayout != null)
			{
				this.CommunityHubLayout.SetNeedsUpdateLayout();
			}
			this.Update();
		}

		// Token: 0x060037F4 RID: 14324 RVA: 0x001489D8 File Offset: 0x00146BD8
		private void Update()
		{
			bool isNetworkReachable = this._isNetworkReachable;
			this._isNetworkReachable = WebChecker.IsNetworkReachable;
			if (isNetworkReachable != this._isNetworkReachable && this.networkReachabilityDidChange != null)
			{
				this.networkReachabilityDidChange(this._isNetworkReachable);
			}
			OAuthGate oauthGate = CoreApplication.Instance.OAuthGate;
			bool hasPublicScopeToken = this._hasPublicScopeToken;
			this._hasPublicScopeToken = oauthGate.HasPublicToken;
			if (hasPublicScopeToken != this._hasPublicScopeToken && this.publicScopeTokenDidChange != null)
			{
				this.publicScopeTokenDidChange(this._hasPublicScopeToken);
			}
			bool hasPrivateScopeToken = this._hasPrivateScopeToken;
			this._hasPrivateScopeToken = oauthGate.HasPrivateToken;
			if (hasPrivateScopeToken != this._hasPrivateScopeToken && this.privateScopeTokenDidChange != null)
			{
				this.privateScopeTokenDidChange(this._hasPrivateScopeToken);
			}
			User userDetails = oauthGate.UserDetails;
			if (userDetails != this._userDetails)
			{
				this._userDetails = userDetails;
				if (this.UserDetailsDidChange != null)
				{
					this.UserDetailsDidChange(this._userDetails);
				}
				if (this._userDetails != null)
				{
					CoreApplication.Instance.AnalyticsManager.UserId = this._userDetails.UserId.ToString();
					return;
				}
				CoreApplication.Instance.AnalyticsManager.UserId = string.Empty;
			}
		}

		// Token: 0x060037F5 RID: 14325 RVA: 0x0004BF37 File Offset: 0x0004A137
		private void OnDisable()
		{
			this._DettachInterface();
		}

		// Token: 0x060037F6 RID: 14326 RVA: 0x00148AF8 File Offset: 0x00146CF8
		private void _AttachInterface()
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate(Resources.Load("CommunityHubLayout", typeof(GameObject))) as GameObject;
			this.CommunityHubLayout = gameObject.GetComponent<CommunityHubLayout>();
			gameObject.transform.SetParent(base.transform);
		}

		// Token: 0x060037F7 RID: 14327 RVA: 0x0004BF3F File Offset: 0x0004A13F
		private void _DettachInterface()
		{
			global::UnityEngine.Object.Destroy(this.CommunityHubLayout.gameObject);
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x060037F8 RID: 14328 RVA: 0x0004BF51 File Offset: 0x0004A151
		public bool IsNetworkReachable
		{
			get
			{
				return this._isNetworkReachable;
			}
		}

		// Token: 0x1400013D RID: 317
		// (add) Token: 0x060037F9 RID: 14329 RVA: 0x00148B44 File Offset: 0x00146D44
		// (remove) Token: 0x060037FA RID: 14330 RVA: 0x00148B7C File Offset: 0x00146D7C
		public event Action<bool> networkReachabilityDidChange;

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x060037FB RID: 14331 RVA: 0x0004BF59 File Offset: 0x0004A159
		public bool HasPublicScopeToken
		{
			get
			{
				return this._hasPublicScopeToken;
			}
		}

		// Token: 0x1400013E RID: 318
		// (add) Token: 0x060037FC RID: 14332 RVA: 0x00148BB4 File Offset: 0x00146DB4
		// (remove) Token: 0x060037FD RID: 14333 RVA: 0x00148BEC File Offset: 0x00146DEC
		public event Action<bool> publicScopeTokenDidChange;

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x060037FE RID: 14334 RVA: 0x0004BF61 File Offset: 0x0004A161
		public bool HasPrivateScopeToken
		{
			get
			{
				return this._hasPrivateScopeToken;
			}
		}

		// Token: 0x1400013F RID: 319
		// (add) Token: 0x060037FF RID: 14335 RVA: 0x00148C24 File Offset: 0x00146E24
		// (remove) Token: 0x06003800 RID: 14336 RVA: 0x00148C5C File Offset: 0x00146E5C
		public event Action<bool> privateScopeTokenDidChange;

		// Token: 0x14000140 RID: 320
		// (add) Token: 0x06003801 RID: 14337 RVA: 0x00148C94 File Offset: 0x00146E94
		// (remove) Token: 0x06003802 RID: 14338 RVA: 0x00148CCC File Offset: 0x00146ECC
		public event Action<User> UserDetailsDidChange;

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06003803 RID: 14339 RVA: 0x0004BF69 File Offset: 0x0004A169
		public List<Type> RequiredOnlineStatuses
		{
			get
			{
				return new List<Type>(this._requiredOnlineStatuses.Keys);
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06003804 RID: 14340 RVA: 0x0004BF7B File Offset: 0x0004A17B
		public int RequiredOnlineStatusesCount
		{
			get
			{
				return this._requiredOnlineStatuses.Count;
			}
		}

		// Token: 0x06003805 RID: 14341 RVA: 0x0004BF88 File Offset: 0x0004A188
		public RequiredOnlineStatus RequiredOnlineStatusForType(Type requiredOnlineStatusType)
		{
			return this._requiredOnlineStatuses[requiredOnlineStatusType];
		}

		// Token: 0x06003806 RID: 14342 RVA: 0x00148D04 File Offset: 0x00146F04
		private void _RegisterLiteSDKRequiredOnlineStatus()
		{
			this._RegisterRequiredOnlineStatus(typeof(NoOnlineConnectionStatus));
			this._RegisterRequiredOnlineStatus(typeof(InternetConnectionStatus));
			this._RegisterRequiredOnlineStatus(typeof(InternetConnectionWithPublicScopeTokenStatus));
			this._RegisterRequiredOnlineStatus(typeof(InternetConnectionWithPrivateScopeTokenOptionalStatus));
			this._RegisterRequiredOnlineStatus(typeof(InternetConnectionWithPrivateScopeTokenRequiredStatus));
		}

		// Token: 0x06003807 RID: 14343 RVA: 0x00148D64 File Offset: 0x00146F64
		private void _RegisterRequiredOnlineStatus(Type requiredOnlineStatusType)
		{
			if (!requiredOnlineStatusType.IsSubclassOf(typeof(RequiredOnlineStatus)))
			{
				throw new ArgumentException("Must be a subclass of RequiredOnlineStatus");
			}
			AsmoLogger.Debug("CommunityHub", "Register Online Status: " + requiredOnlineStatusType.Name, null);
			RequiredOnlineStatus requiredOnlineStatus = (RequiredOnlineStatus)Activator.CreateInstance(requiredOnlineStatusType);
			this._requiredOnlineStatuses.Add(requiredOnlineStatusType, requiredOnlineStatus);
		}

		// Token: 0x06003808 RID: 14344 RVA: 0x0004BF96 File Offset: 0x0004A196
		public void RequireOnlineStatus(Type requiredOnlineStatusType, Action onSuccess, Action onFailure)
		{
			AsmoLogger.Debug("CommunityHub", "Require Online Status: " + requiredOnlineStatusType.Name, null);
			this.RequiredOnlineStatusForType(requiredOnlineStatusType).MeetRequirements(onSuccess, onFailure);
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06003809 RID: 14345 RVA: 0x0004BFC1 File Offset: 0x0004A1C1
		// (set) Token: 0x0600380A RID: 14346 RVA: 0x0004BFCE File Offset: 0x0004A1CE
		public List<CommunityHubContent> CommunityHubContents
		{
			get
			{
				return new List<CommunityHubContent>(this._communityHubContents);
			}
			set
			{
				this._communityHubContents = value;
				this._UpdateCommunityHubContent();
			}
		}

		// Token: 0x0600380B RID: 14347 RVA: 0x00148DC4 File Offset: 0x00146FC4
		public void AddContent(CommunityHubContent content)
		{
			if (!this._communityHubContents.Contains(content))
			{
				this._communityHubContents.Add(content);
				content.Start();
				this._UpdateCommunityHubContent();
				return;
			}
			AsmoLogger.Warning("CommunityHub", () => "Content already displayed: " + content.ToString(), null);
		}

		// Token: 0x0600380C RID: 14348 RVA: 0x00148E2C File Offset: 0x0014702C
		public void RemoveContent(CommunityHubContent content)
		{
			if (this._communityHubContents.Contains(content))
			{
				content.Stop();
				this._communityHubContents.Remove(content);
				this._UpdateCommunityHubContent();
				return;
			}
			AsmoLogger.Warning("CommunityHub", () => "Can't remove displayed content: " + content.ToString(), null);
		}

		// Token: 0x0600380D RID: 14349 RVA: 0x0004BFDD File Offset: 0x0004A1DD
		public void RemoveAllContent()
		{
			this._communityHubContents.ForEach(delegate(CommunityHubContent content)
			{
				content.Stop();
			});
			this._communityHubContents.Clear();
			this._UpdateCommunityHubContent();
		}

		// Token: 0x0600380E RID: 14350 RVA: 0x0004C01A File Offset: 0x0004A21A
		private void _UpdateCommunityHubContent()
		{
			this.CommunityHubLayout.SetNeedsUpdateLayout();
		}

		// Token: 0x040029FE RID: 10750
		private const string _debugModuleName = "CommunityHub";

		// Token: 0x04002A00 RID: 10752
		private List<RectTransform> _transformsToAutoLayout = new List<RectTransform>();

		// Token: 0x04002A02 RID: 10754
		private bool _isNetworkReachable;

		// Token: 0x04002A04 RID: 10756
		private bool _hasPublicScopeToken;

		// Token: 0x04002A06 RID: 10758
		private bool _hasPrivateScopeToken;

		// Token: 0x04002A08 RID: 10760
		private User _userDetails;

		// Token: 0x04002A0A RID: 10762
		private Dictionary<Type, RequiredOnlineStatus> _requiredOnlineStatuses = new Dictionary<Type, RequiredOnlineStatus>();

		// Token: 0x04002A0B RID: 10763
		private List<CommunityHubContent> _communityHubContents = new List<CommunityHubContent>();
	}
}
