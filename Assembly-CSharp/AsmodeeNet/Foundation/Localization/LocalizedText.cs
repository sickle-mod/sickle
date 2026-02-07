using System;
using AsmodeeNet.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.Foundation.Localization
{
	// Token: 0x0200096C RID: 2412
	public class LocalizedText : MonoBehaviour
	{
		// Token: 0x060040E2 RID: 16610 RVA: 0x0015F7F4 File Offset: 0x0015D9F4
		private void Awake()
		{
			this._txt = base.GetComponent<Text>();
			if (this._txt == null)
			{
				this._tmpTxt = base.GetComponent<TextMeshProUGUI>();
				if (this._tmpTxt == null)
				{
					AsmoLogger.Error("LocalizedText", string.Format("Missing Text or TextMeshProUGUI component in {0}", base.gameObject.name), null);
				}
			}
		}

		// Token: 0x060040E3 RID: 16611 RVA: 0x0015F858 File Offset: 0x0015DA58
		private void OnEnable()
		{
			LocalizationManager localizationManager = CoreApplication.Instance.LocalizationManager;
			localizationManager.OnLanguageChanged += this._OnLanguageChanged;
			this._UpdateDisplayedText(localizationManager.GetLocalizedText(this.key));
		}

		// Token: 0x060040E4 RID: 16612 RVA: 0x00051ADE File Offset: 0x0004FCDE
		private void OnDisable()
		{
			if (CoreApplication.IsQuitting)
			{
				return;
			}
			CoreApplication.Instance.LocalizationManager.OnLanguageChanged -= this._OnLanguageChanged;
		}

		// Token: 0x060040E5 RID: 16613 RVA: 0x00051B03 File Offset: 0x0004FD03
		private void _OnLanguageChanged(LocalizationManager localizationManager)
		{
			this._UpdateDisplayedText(localizationManager.GetLocalizedText(this.key));
		}

		// Token: 0x060040E6 RID: 16614 RVA: 0x00051B17 File Offset: 0x0004FD17
		private void _UpdateDisplayedText(string msg)
		{
			if (this._txt != null)
			{
				this._txt.text = msg;
				return;
			}
			this._tmpTxt.text = msg;
		}

		// Token: 0x04003133 RID: 12595
		private const string _documentation = "<b>LocalizedText</b> automatically retrieves the <b>key</b> to localize. Check the <b>Asmodee.net/Localization</b> menu.";

		// Token: 0x04003134 RID: 12596
		private const string _kModuleName = "LocalizedText";

		// Token: 0x04003135 RID: 12597
		public string key;

		// Token: 0x04003136 RID: 12598
		private Text _txt;

		// Token: 0x04003137 RID: 12599
		private TextMeshProUGUI _tmpTxt;
	}
}
