using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.Utilities
{
	// Token: 0x020001CE RID: 462
	public class ChangeLabelOnInputFieldValueChanged : MonoBehaviour
	{
		// Token: 0x06000D6D RID: 3437 RVA: 0x00030E83 File Offset: 0x0002F083
		private void Awake()
		{
			this._observedInputField.onValueChanged.AddListener(new UnityAction<string>(this.OnObservedInputFieldValueChanged));
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x00030EA1 File Offset: 0x0002F0A1
		private void OnObservedInputFieldValueChanged(string value)
		{
			this._labelText.text = value;
		}

		// Token: 0x04000AB9 RID: 2745
		[SerializeField]
		private InputField _observedInputField;

		// Token: 0x04000ABA RID: 2746
		[SerializeField]
		private TMP_Text _labelText;
	}
}
