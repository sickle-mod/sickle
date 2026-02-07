using System;
using AsmodeeNet.Foundation;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x0200079A RID: 1946
	[CreateAssetMenu]
	public class CommunityHubSkin : ScriptableObject
	{
		// Token: 0x04002A3C RID: 10812
		[Header("Community Hub")]
		[SerializeField]
		public CommunityHubSkin.PerDisplayModeGameObject expandCollapseButtonPrefabs;

		// Token: 0x04002A3D RID: 10813
		public Sprite collapseButtonInHorizontalTabBarIcon;

		// Token: 0x04002A3E RID: 10814
		public Sprite collapseButtonInVerticalTabBarIcon;

		// Token: 0x04002A3F RID: 10815
		public CommunityHubSkin.PerDisplayModeGameObject tabBarPrefabs;

		// Token: 0x04002A40 RID: 10816
		public CommunityHubSkin.PerDisplayModeGameObject tabPrefabs;

		// Token: 0x04002A41 RID: 10817
		[Header("Login Banner Content")]
		public CommunityHubSkin.PerDisplayModeGameObject loginBannerContentPrefabs;

		// Token: 0x04002A42 RID: 10818
		[Header("User Account Content")]
		public CommunityHubSkin.PerDisplayModeGameObject userAccountContentPrefabs;

		// Token: 0x04002A43 RID: 10819
		public CommunityHubSkin.TabIcons userAccountContentIcons;

		// Token: 0x04002A44 RID: 10820
		[Header("Players Lists Content")]
		public CommunityHubSkin.PerDisplayModeGameObject playersListsContentPrefabs;

		// Token: 0x04002A45 RID: 10821
		public CommunityHubSkin.TabIcons playersListsContentIcons;

		// Token: 0x0200079B RID: 1947
		[Serializable]
		public class TabIcons
		{
			// Token: 0x04002A46 RID: 10822
			public Sprite spriteOff;

			// Token: 0x04002A47 RID: 10823
			public Sprite spriteOn;
		}

		// Token: 0x0200079C RID: 1948
		[Serializable]
		public class PerDisplayModeGameObject
		{
			// Token: 0x1700046E RID: 1134
			public GameObject this[Preferences.DisplayMode displayMode]
			{
				get
				{
					GameObject gameObject = null;
					switch (displayMode)
					{
					case Preferences.DisplayMode.Small:
						gameObject = this.small;
						break;
					case Preferences.DisplayMode.Regular:
						gameObject = this.regular;
						break;
					case Preferences.DisplayMode.Big:
						gameObject = this.big;
						break;
					}
					if (!(gameObject != null))
					{
						return this.generic;
					}
					return gameObject;
				}
			}

			// Token: 0x04002A48 RID: 10824
			public GameObject generic;

			// Token: 0x04002A49 RID: 10825
			public GameObject small;

			// Token: 0x04002A4A RID: 10826
			public GameObject regular;

			// Token: 0x04002A4B RID: 10827
			public GameObject big;
		}
	}
}
