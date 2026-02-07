using System;
using System.Collections.Generic;
using System.Linq;
using I2.Loc;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Reworked.Options
{
	// Token: 0x0200018E RID: 398
	public class GraphicSettingsPanel : MonoBehaviour
	{
		// Token: 0x06000BB7 RID: 2999 RVA: 0x0007F9B4 File Offset: 0x0007DBB4
		public void Init()
		{
			this.InitResolutions();
			Resolution resolution = SingletonMono<GraphicSettingsManager>.Instance.Settings.Resolution;
			this.PrepareResolutionsLabels();
			this.PrepareQualityLabels();
			this.SetupResolutionDropdown(resolution);
			this.SetupQualityDropdown(SingletonMono<GraphicSettingsManager>.Instance.Settings.Quality);
			this.SetupWindowedModeToggle();
			this.SetupVSyncToggle();
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x0007FA0C File Offset: 0x0007DC0C
		private void InitResolutions()
		{
			this.resolutions = Screen.resolutions;
			List<Resolution> list = new List<Resolution>();
			Resolution[] array = this.resolutions;
			for (int i = 0; i < array.Length; i++)
			{
				Resolution r = array[i];
				if (!list.Any((Resolution reso) => reso.width == r.width && reso.height == r.height))
				{
					list.Add(r);
				}
			}
			this.resolutions = list.ToArray();
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x0007FA80 File Offset: 0x0007DC80
		private void SetupResolutionDropdown(Resolution savedResolution)
		{
			this.resolutionDropdown.ClearOptions();
			this.resolutionDropdown.AddOptions(this.resolutionsLabels);
			this.resolutionDropdown.onValueChanged.RemoveAllListeners();
			this.resolutionDropdown.onValueChanged.AddListener(new UnityAction<int>(this.OnResolutionChange));
			int num = this.resolutions.ToList<Resolution>().FindIndex((Resolution r) => r.width == savedResolution.width && r.height == savedResolution.height);
			this.resolutionDropdown.SetValueWithoutNotify(num);
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0007FB0C File Offset: 0x0007DD0C
		private void SetupQualityDropdown(int savedQuality)
		{
			this.qualityDropdown.ClearOptions();
			this.qualityDropdown.AddOptions(this.qualityLabels);
			this.qualityDropdown.onValueChanged.RemoveAllListeners();
			this.qualityDropdown.onValueChanged.AddListener(new UnityAction<int>(this.OnQualityChange));
			this.qualityDropdown.SetValueWithoutNotify(savedQuality);
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x0007FB70 File Offset: 0x0007DD70
		private void SetupWindowedModeToggle()
		{
			this.windowedToggle.isOn = SingletonMono<GraphicSettingsManager>.Instance.Settings.WindowedMode;
			this.windowedToggle.onValueChanged.RemoveAllListeners();
			this.windowedToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnWindowedModeChange));
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x0007FBC4 File Offset: 0x0007DDC4
		private void SetupVSyncToggle()
		{
			this.vSyncToggle.isOn = SingletonMono<GraphicSettingsManager>.Instance.Settings.VSync;
			this.vSyncToggle.onValueChanged.RemoveAllListeners();
			this.vSyncToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnVSyncChange));
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x0007FC18 File Offset: 0x0007DE18
		private void PrepareResolutionsLabels()
		{
			this.resolutionsLabels.Clear();
			foreach (Resolution resolution in this.resolutions)
			{
				this.resolutionsLabels.Add(string.Format("{0}x{1}", resolution.width, resolution.height));
			}
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x0007FC7C File Offset: 0x0007DE7C
		private void PrepareQualityLabels()
		{
			this.qualityLabels.Clear();
			foreach (string text in QualitySettings.names)
			{
				this.qualityLabels.Add(LocalizationManager.GetTranslation("Options/" + text, true, 0, true, false, null, null));
			}
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x0002FE21 File Offset: 0x0002E021
		private void OnResolutionChange(int value)
		{
			SingletonMono<GraphicSettingsManager>.Instance.Settings.Resolution = this.resolutions[value];
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x0002FE3E File Offset: 0x0002E03E
		private void OnQualityChange(int value)
		{
			SingletonMono<GraphicSettingsManager>.Instance.Settings.Quality = value;
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x0002FE50 File Offset: 0x0002E050
		private void OnWindowedModeChange(bool value)
		{
			SingletonMono<GraphicSettingsManager>.Instance.Settings.WindowedMode = value;
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x0002FE62 File Offset: 0x0002E062
		private void OnVSyncChange(bool value)
		{
			SingletonMono<GraphicSettingsManager>.Instance.Settings.VSync = value;
		}

		// Token: 0x04000982 RID: 2434
		[SerializeField]
		private Dropdown resolutionDropdown;

		// Token: 0x04000983 RID: 2435
		[SerializeField]
		private Dropdown qualityDropdown;

		// Token: 0x04000984 RID: 2436
		[SerializeField]
		private Toggle windowedToggle;

		// Token: 0x04000985 RID: 2437
		[SerializeField]
		private Toggle vSyncToggle;

		// Token: 0x04000986 RID: 2438
		private Resolution[] resolutions;

		// Token: 0x04000987 RID: 2439
		private readonly List<string> resolutionsLabels = new List<string>();

		// Token: 0x04000988 RID: 2440
		private readonly List<string> qualityLabels = new List<string>();
	}
}
