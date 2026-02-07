using System;
using System.Linq;
using Scythe.Utilities;
using UnityEngine;

namespace Reworked.Options
{
	// Token: 0x0200018D RID: 397
	public class GraphicSettingsManager : SingletonMono<GraphicSettingsManager>
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000BAC RID: 2988 RVA: 0x0002FDAE File Offset: 0x0002DFAE
		// (set) Token: 0x06000BAD RID: 2989 RVA: 0x0002FDB6 File Offset: 0x0002DFB6
		public GraphicSettings Settings { get; private set; }

		// Token: 0x06000BAE RID: 2990 RVA: 0x0002FDBF File Offset: 0x0002DFBF
		private void Awake()
		{
			if (PlatformManager.IsStandalone)
			{
				global::UnityEngine.Object.DontDestroyOnLoad(SingletonMono<GraphicSettingsManager>.Instance.gameObject);
				this.LoadSettings();
			}
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x0007F798 File Offset: 0x0007D998
		private void LoadSettings()
		{
			string @string = PlayerPrefs.GetString("GraphicSettings", string.Empty);
			if (!string.IsNullOrEmpty(@string))
			{
				this.Settings = JsonUtility.FromJson<GraphicSettings>(@string);
			}
			else
			{
				this.Settings = new GraphicSettings(Screen.resolutions.LastOrDefault<Resolution>(), QualitySettings.GetQualityLevel(), !Screen.fullScreen);
			}
			this.Settings.OnResolutionChange += this.GraphicSettings_OnResolutionChange;
			this.Settings.OnQualityChange += this.GraphicSettings_OnQualityChange;
			this.Settings.OnWindowedModeChange += this.GraphicSettings_OnWindowedModeChange;
			this.Settings.OnVSyncChange += this.GraphicSettings_OnVSyncChange;
			this.Settings.ApplySettings();
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x0007F854 File Offset: 0x0007DA54
		private void SaveSettings()
		{
			string text = JsonUtility.ToJson(this.Settings);
			PlayerPrefs.SetString("GraphicSettings", text);
			PlayerPrefs.Save();
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x0007F880 File Offset: 0x0007DA80
		private void GraphicSettings_OnResolutionChange()
		{
			Resolution resolution = this.Settings.Resolution;
			Screen.SetResolution(resolution.width, resolution.height, !this.Settings.WindowedMode, resolution.refreshRate);
			this.SaveSettings();
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x0002FDDD File Offset: 0x0002DFDD
		private void GraphicSettings_OnQualityChange()
		{
			QualitySettings.SetQualityLevel(this.Settings.Quality);
			this.SetupTargetFramrate();
			this.SaveSettings();
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x0007F8C8 File Offset: 0x0007DAC8
		private void GraphicSettings_OnWindowedModeChange()
		{
			bool flag = Screen.fullScreen != !this.Settings.WindowedMode;
			Screen.fullScreen = !this.Settings.WindowedMode;
			if (flag && this.Settings.WindowedMode)
			{
				Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
			}
			this.SaveSettings();
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x0002FDFB File Offset: 0x0002DFFB
		private void GraphicSettings_OnVSyncChange()
		{
			QualitySettings.vSyncCount = (this.Settings.VSync ? 1 : 0);
			this.SaveSettings();
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x0007F938 File Offset: 0x0007DB38
		private void SetupTargetFramrate()
		{
			if (PlatformManager.IsMobile)
			{
				Application.targetFrameRate = 30;
				return;
			}
			switch (QualitySettings.GetQualityLevel())
			{
			case 0:
				Application.targetFrameRate = 30;
				return;
			case 1:
				Application.targetFrameRate = 30;
				return;
			case 2:
				Application.targetFrameRate = 60;
				return;
			case 3:
				Application.targetFrameRate = 60;
				return;
			case 4:
				Application.targetFrameRate = 60;
				return;
			case 5:
				Application.targetFrameRate = 60;
				return;
			default:
				Application.targetFrameRate = 60;
				return;
			}
		}

		// Token: 0x04000980 RID: 2432
		private const string PREFS_GRAPHIC_SETTINGS = "GraphicSettings";
	}
}
