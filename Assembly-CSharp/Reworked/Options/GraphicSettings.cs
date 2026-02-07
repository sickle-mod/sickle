using System;
using UnityEngine;

namespace Reworked.Options
{
	// Token: 0x0200018C RID: 396
	[Serializable]
	public class GraphicSettings
	{
		// Token: 0x1400003C RID: 60
		// (add) Token: 0x06000B9A RID: 2970 RVA: 0x0007F548 File Offset: 0x0007D748
		// (remove) Token: 0x06000B9B RID: 2971 RVA: 0x0007F580 File Offset: 0x0007D780
		public event Action OnResolutionChange;

		// Token: 0x1400003D RID: 61
		// (add) Token: 0x06000B9C RID: 2972 RVA: 0x0007F5B8 File Offset: 0x0007D7B8
		// (remove) Token: 0x06000B9D RID: 2973 RVA: 0x0007F5F0 File Offset: 0x0007D7F0
		public event Action OnQualityChange;

		// Token: 0x1400003E RID: 62
		// (add) Token: 0x06000B9E RID: 2974 RVA: 0x0007F628 File Offset: 0x0007D828
		// (remove) Token: 0x06000B9F RID: 2975 RVA: 0x0007F660 File Offset: 0x0007D860
		public event Action OnWindowedModeChange;

		// Token: 0x1400003F RID: 63
		// (add) Token: 0x06000BA0 RID: 2976 RVA: 0x0007F698 File Offset: 0x0007D898
		// (remove) Token: 0x06000BA1 RID: 2977 RVA: 0x0007F6D0 File Offset: 0x0007D8D0
		public event Action OnVSyncChange;

		// Token: 0x06000BA2 RID: 2978 RVA: 0x0002FCD5 File Offset: 0x0002DED5
		public GraphicSettings(Resolution resolution, int quality, bool windowed)
		{
			this.screenWidth = resolution.width;
			this.screenHeight = resolution.height;
			this.refreshRate = resolution.refreshRate;
			this.qualityLevel = quality;
			this.windowed = windowed;
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000BA3 RID: 2979 RVA: 0x0007F708 File Offset: 0x0007D908
		// (set) Token: 0x06000BA4 RID: 2980 RVA: 0x0002FD12 File Offset: 0x0002DF12
		public Resolution Resolution
		{
			get
			{
				return new Resolution
				{
					height = this.screenHeight,
					width = this.screenWidth,
					refreshRate = this.refreshRate
				};
			}
			set
			{
				this.screenWidth = value.width;
				this.screenHeight = value.height;
				this.refreshRate = value.refreshRate;
				Action onResolutionChange = this.OnResolutionChange;
				if (onResolutionChange == null)
				{
					return;
				}
				onResolutionChange();
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000BA5 RID: 2981 RVA: 0x0002FD4B File Offset: 0x0002DF4B
		// (set) Token: 0x06000BA6 RID: 2982 RVA: 0x0002FD53 File Offset: 0x0002DF53
		public int Quality
		{
			get
			{
				return this.qualityLevel;
			}
			set
			{
				this.qualityLevel = value;
				Action onQualityChange = this.OnQualityChange;
				if (onQualityChange == null)
				{
					return;
				}
				onQualityChange();
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000BA7 RID: 2983 RVA: 0x0002FD6C File Offset: 0x0002DF6C
		// (set) Token: 0x06000BA8 RID: 2984 RVA: 0x0002FD74 File Offset: 0x0002DF74
		public bool WindowedMode
		{
			get
			{
				return this.windowed;
			}
			set
			{
				this.windowed = value;
				Action onWindowedModeChange = this.OnWindowedModeChange;
				if (onWindowedModeChange == null)
				{
					return;
				}
				onWindowedModeChange();
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000BA9 RID: 2985 RVA: 0x0002FD8D File Offset: 0x0002DF8D
		// (set) Token: 0x06000BAA RID: 2986 RVA: 0x0002FD95 File Offset: 0x0002DF95
		public bool VSync
		{
			get
			{
				return this.isVSyncOn;
			}
			set
			{
				this.isVSyncOn = value;
				Action onVSyncChange = this.OnVSyncChange;
				if (onVSyncChange == null)
				{
					return;
				}
				onVSyncChange();
			}
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x0007F748 File Offset: 0x0007D948
		public void ApplySettings()
		{
			Action onResolutionChange = this.OnResolutionChange;
			if (onResolutionChange != null)
			{
				onResolutionChange();
			}
			Action onQualityChange = this.OnQualityChange;
			if (onQualityChange != null)
			{
				onQualityChange();
			}
			Action onWindowedModeChange = this.OnWindowedModeChange;
			if (onWindowedModeChange != null)
			{
				onWindowedModeChange();
			}
			Action onVSyncChange = this.OnVSyncChange;
			if (onVSyncChange == null)
			{
				return;
			}
			onVSyncChange();
		}

		// Token: 0x04000976 RID: 2422
		[SerializeField]
		private int screenWidth;

		// Token: 0x04000977 RID: 2423
		[SerializeField]
		private int screenHeight;

		// Token: 0x04000978 RID: 2424
		[SerializeField]
		private int refreshRate;

		// Token: 0x04000979 RID: 2425
		[SerializeField]
		private int qualityLevel;

		// Token: 0x0400097A RID: 2426
		[SerializeField]
		private bool windowed;

		// Token: 0x0400097B RID: 2427
		[SerializeField]
		private bool isVSyncOn;
	}
}
