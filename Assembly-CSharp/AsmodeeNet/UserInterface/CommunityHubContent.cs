using System;
using AsmodeeNet.Foundation;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000792 RID: 1938
	public abstract class CommunityHubContent
	{
		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06003817 RID: 14359
		public abstract CommunityHubContent.Layout DisplayedLayout { get; }

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06003818 RID: 14360 RVA: 0x000283F8 File Offset: 0x000265F8
		public virtual CommunityHubContent.ActivityType BackgroundActivity
		{
			get
			{
				return CommunityHubContent.ActivityType.Inactive;
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06003819 RID: 14361 RVA: 0x0004C092 File Offset: 0x0004A292
		public RectTransform DisplayedContent
		{
			get
			{
				if (!(this._root != null))
				{
					return null;
				}
				return this._root.transform as RectTransform;
			}
		}

		// Token: 0x0600381A RID: 14362 RVA: 0x00027EF0 File Offset: 0x000260F0
		public virtual void Start()
		{
		}

		// Token: 0x0600381B RID: 14363 RVA: 0x00027EF0 File Offset: 0x000260F0
		public virtual void Stop()
		{
		}

		// Token: 0x0600381C RID: 14364 RVA: 0x00027EF0 File Offset: 0x000260F0
		public virtual void LoadDisplayedContent(Preferences.DisplayMode displayMode)
		{
		}

		// Token: 0x0600381D RID: 14365 RVA: 0x00027EF0 File Offset: 0x000260F0
		public virtual void DisposeDisplayedContent()
		{
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x0600381E RID: 14366 RVA: 0x0004C0B4 File Offset: 0x0004A2B4
		// (set) Token: 0x0600381F RID: 14367 RVA: 0x0004C0BC File Offset: 0x0004A2BC
		public CommunityHubSkin.TabIcons TabIcons { get; protected set; }

		// Token: 0x04002A10 RID: 10768
		protected GameObject _root;

		// Token: 0x02000793 RID: 1939
		public enum Layout
		{
			// Token: 0x04002A13 RID: 10771
			Header,
			// Token: 0x04002A14 RID: 10772
			Tab
		}

		// Token: 0x02000794 RID: 1940
		public enum ActivityType
		{
			// Token: 0x04002A16 RID: 10774
			Active,
			// Token: 0x04002A17 RID: 10775
			Inactive
		}
	}
}
