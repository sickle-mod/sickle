using System;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000825 RID: 2085
	public class ActivityIndicatorButton : MonoBehaviour
	{
		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06003B1D RID: 15133 RVA: 0x0004E40A File Offset: 0x0004C60A
		// (set) Token: 0x06003B1E RID: 15134 RVA: 0x0004E412 File Offset: 0x0004C612
		public bool Waiting
		{
			get
			{
				return this._waiting;
			}
			set
			{
				this._waiting = value;
				this._text.SetActive(!this._waiting);
				this._spinner.SetActive(this._waiting);
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06003B1F RID: 15135 RVA: 0x0004E440 File Offset: 0x0004C640
		// (set) Token: 0x06003B20 RID: 15136 RVA: 0x0004E44D File Offset: 0x0004C64D
		public bool Interactable
		{
			get
			{
				return this._button.interactable;
			}
			set
			{
				this._button.interactable = value;
			}
		}

		// Token: 0x04002CD3 RID: 11475
		[SerializeField]
		private GameObject _text;

		// Token: 0x04002CD4 RID: 11476
		[SerializeField]
		private GameObject _spinner;

		// Token: 0x04002CD5 RID: 11477
		[SerializeField]
		private Button _button;

		// Token: 0x04002CD6 RID: 11478
		private bool _waiting;
	}
}
