using System;
using UnityEngine;

namespace Reworked.Options
{
	// Token: 0x02000191 RID: 401
	public class SettingsLoader : MonoBehaviour
	{
		// Token: 0x06000BC8 RID: 3016 RVA: 0x0002FEEA File Offset: 0x0002E0EA
		private void Awake()
		{
			this.LoadSettings();
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x0002FEF2 File Offset: 0x0002E0F2
		private void LoadSettings()
		{
			if (!PlatformManager.IsStandalone)
			{
				this.AllowSleepMode(this.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_ALLOW_SLEEP_MODE, 1)));
			}
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x0002FF12 File Offset: 0x0002E112
		private void AllowSleepMode(bool allowSleepModeState)
		{
			Screen.sleepTimeout = (allowSleepModeState ? (-2) : (-1));
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x0002FF21 File Offset: 0x0002E121
		private bool IntToBool(int i)
		{
			return i != 0;
		}
	}
}
