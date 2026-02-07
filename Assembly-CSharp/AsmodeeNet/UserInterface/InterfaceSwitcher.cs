using System;
using System.Linq;
using AsmodeeNet.Foundation;
using AsmodeeNet.Foundation.Localization;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007C0 RID: 1984
	public class InterfaceSwitcher : MonoBehaviour
	{
		// Token: 0x060038FD RID: 14589 RVA: 0x0014C3C4 File Offset: 0x0014A5C4
		private void Awake()
		{
			this._languages = CoreApplication.Instance.LocalizationManager.xliffFiles.Select((TextAsset x) => XliffUtility.GetXliffTargetLangFromXml(x.text)).Distinct<LocalizationManager.Language>().ToArray<LocalizationManager.Language>();
		}

		// Token: 0x060038FE RID: 14590 RVA: 0x0014C414 File Offset: 0x0014A614
		private void Update()
		{
			if (KeyCombinationChecker.IsDebugKeyCombination())
			{
				if (Input.GetKeyDown(KeyCode.S))
				{
					CoreApplication.Instance.Preferences.InterfaceDisplayMode = Preferences.DisplayMode.Small;
					return;
				}
				if (Input.GetKeyDown(KeyCode.R))
				{
					CoreApplication.Instance.Preferences.InterfaceDisplayMode = Preferences.DisplayMode.Regular;
					return;
				}
				if (Input.GetKeyDown(KeyCode.B))
				{
					CoreApplication.Instance.Preferences.InterfaceDisplayMode = Preferences.DisplayMode.Big;
					return;
				}
				for (int i = 0; i < this._keyCodes.Length; i++)
				{
					if (Input.GetKeyDown(this._keyCodes[i]))
					{
						int num = Mathf.Min(i, this._languages.Length - 1);
						CoreApplication.Instance.LocalizationManager.CurrentLanguage = this._languages[num];
					}
				}
			}
		}

		// Token: 0x04002AF2 RID: 10994
		private const string _documentation = "[Ctrl] + [Alt] + S ➜ Small\n[Ctrl] + [Alt] + R ➜ Regular\n[Ctrl] + [Alt] + B ➜ Big\n\n[Ctrl] + [Alt] + [F1], [F2] ... ➜ Language ";

		// Token: 0x04002AF3 RID: 10995
		private LocalizationManager.Language[] _languages;

		// Token: 0x04002AF4 RID: 10996
		private KeyCode[] _keyCodes = new KeyCode[]
		{
			KeyCode.F1,
			KeyCode.F2,
			KeyCode.F3,
			KeyCode.F4,
			KeyCode.F5,
			KeyCode.F6,
			KeyCode.F7,
			KeyCode.F8,
			KeyCode.F9
		};
	}
}
