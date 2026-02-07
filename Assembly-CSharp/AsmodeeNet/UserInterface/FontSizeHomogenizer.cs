using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AsmodeeNet.Foundation;
using AsmodeeNet.Foundation.Localization;
using AsmodeeNet.Utils;
using TMPro;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007B8 RID: 1976
	public class FontSizeHomogenizer : MonoBehaviour
	{
		// Token: 0x060038DF RID: 14559 RVA: 0x0014C118 File Offset: 0x0014A318
		private void OnEnable()
		{
			CoreApplication.Instance.Preferences.InterfaceDisplayModeDidChange += this.SetNeedsFontSizeUpdate;
			CoreApplication.Instance.Preferences.AspectDidChange += this.SetNeedsFontSizeUpdate;
			CoreApplication.Instance.LocalizationManager.OnLanguageChanged += this._OnLanguageChanged;
			this.SetNeedsFontSizeUpdate();
		}

		// Token: 0x060038E0 RID: 14560 RVA: 0x0014C17C File Offset: 0x0014A37C
		private void OnDisable()
		{
			if (CoreApplication.IsQuitting)
			{
				return;
			}
			CoreApplication.Instance.Preferences.InterfaceDisplayModeDidChange -= this.SetNeedsFontSizeUpdate;
			CoreApplication.Instance.Preferences.AspectDidChange -= this.SetNeedsFontSizeUpdate;
			CoreApplication.Instance.LocalizationManager.OnLanguageChanged -= this._OnLanguageChanged;
		}

		// Token: 0x060038E1 RID: 14561 RVA: 0x0004C980 File Offset: 0x0004AB80
		private void OnRectTransformDimensionsChange()
		{
			this.SetNeedsFontSizeUpdate();
		}

		// Token: 0x060038E2 RID: 14562 RVA: 0x0004C980 File Offset: 0x0004AB80
		private void _OnLanguageChanged(LocalizationManager localizationManager)
		{
			this.SetNeedsFontSizeUpdate();
		}

		// Token: 0x060038E3 RID: 14563 RVA: 0x0004C988 File Offset: 0x0004AB88
		public void SetNeedsFontSizeUpdate()
		{
			this._updateCount = 2;
		}

		// Token: 0x060038E4 RID: 14564 RVA: 0x0004C991 File Offset: 0x0004AB91
		private void LateUpdate()
		{
			if (this._updateCount > 0)
			{
				base.StopAllCoroutines();
				base.StartCoroutine(this._UpdateFontSizeAndWait());
			}
		}

		// Token: 0x060038E5 RID: 14565 RVA: 0x0004C9AF File Offset: 0x0004ABAF
		private IEnumerator _UpdateFontSizeAndWait()
		{
			while (this._updateCount > 0)
			{
				this._UpdateFontSize();
				yield return null;
			}
			yield break;
		}

		// Token: 0x060038E6 RID: 14566 RVA: 0x0014C1E4 File Offset: 0x0014A3E4
		private void _UpdateFontSize()
		{
			IEnumerable<TextMeshProUGUI> enumerable = this.labels.Where((TextMeshProUGUI x) => x.IsActive());
			if (!enumerable.Any<TextMeshProUGUI>())
			{
				return;
			}
			foreach (TextMeshProUGUI textMeshProUGUI in enumerable)
			{
				textMeshProUGUI.enableAutoSizing = true;
				textMeshProUGUI.ForceMeshUpdate(false, false);
			}
			float num = enumerable.Min((TextMeshProUGUI label) => label.fontSize);
			foreach (TextMeshProUGUI textMeshProUGUI2 in enumerable)
			{
				textMeshProUGUI2.fontSize = num;
				textMeshProUGUI2.enableAutoSizing = false;
			}
			this._updateCount--;
			if (this._updateCount == 0 && !MathUtils.Approximately(this._stabilizedFontSize, num, 1f))
			{
				this._updateCount = 1;
			}
			this._stabilizedFontSize = num;
		}

		// Token: 0x04002ADA RID: 10970
		private const string _documentation = "Based on <i>Auto Sizing</i> all the referenced TextMeshProUGUI texts will have the same font size.";

		// Token: 0x04002ADB RID: 10971
		public TextMeshProUGUI[] labels;

		// Token: 0x04002ADC RID: 10972
		private const int _minimalUpdateCount = 2;

		// Token: 0x04002ADD RID: 10973
		private int _updateCount;

		// Token: 0x04002ADE RID: 10974
		private float _stabilizedFontSize;
	}
}
