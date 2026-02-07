using System;
using AsmodeeNet.Foundation;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x0200079D RID: 1949
	public class ExpandCollapseButton : MonoBehaviour
	{
		// Token: 0x14000143 RID: 323
		// (add) Token: 0x06003853 RID: 14419 RVA: 0x0014A61C File Offset: 0x0014881C
		// (remove) Token: 0x06003854 RID: 14420 RVA: 0x0014A654 File Offset: 0x00148854
		public event Action onExpandCollapseButtonClicked;

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06003855 RID: 14421 RVA: 0x0004C2FE File Offset: 0x0004A4FE
		// (set) Token: 0x06003856 RID: 14422 RVA: 0x0004C306 File Offset: 0x0004A506
		public bool IsCollapsed
		{
			get
			{
				return this._collapsed;
			}
			set
			{
				if (this._collapsed != value)
				{
					this._collapsed = value;
					this._needsUpdate = true;
				}
			}
		}

		// Token: 0x06003857 RID: 14423 RVA: 0x0004C31F File Offset: 0x0004A51F
		public void OnButtonClicked()
		{
			if (this.onExpandCollapseButtonClicked != null)
			{
				this.onExpandCollapseButtonClicked();
			}
		}

		// Token: 0x06003858 RID: 14424 RVA: 0x0004C334 File Offset: 0x0004A534
		private void OnEnable()
		{
			this._needsUpdate = true;
			CoreApplication.Instance.Preferences.InterfaceOrientationDidChange += this._InterfaceOrientationDidChange;
		}

		// Token: 0x06003859 RID: 14425 RVA: 0x0014A68C File Offset: 0x0014888C
		private void LateUpdate()
		{
			if (this._needsUpdate)
			{
				this._needsUpdate = false;
				bool flag = CoreApplication.Instance.Preferences.InterfaceOrientation == Preferences.Orientation.Horizontal;
				this.expandHorizontal.gameObject.SetActive(this._collapsed && flag);
				this.collapseHorizontal.gameObject.SetActive(!this._collapsed && flag);
				this.expandVertical.gameObject.SetActive(this._collapsed && !flag);
				this.collpaseVertical.gameObject.SetActive(!this._collapsed && !flag);
			}
		}

		// Token: 0x0600385A RID: 14426 RVA: 0x0004C358 File Offset: 0x0004A558
		private void OnDisable()
		{
			if (!CoreApplication.IsQuitting)
			{
				CoreApplication.Instance.Preferences.InterfaceOrientationDidChange -= this._InterfaceOrientationDidChange;
			}
		}

		// Token: 0x0600385B RID: 14427 RVA: 0x0004C37C File Offset: 0x0004A57C
		private void _InterfaceOrientationDidChange()
		{
			this._needsUpdate = true;
		}

		// Token: 0x04002A4C RID: 10828
		public Button button;

		// Token: 0x04002A4D RID: 10829
		public RectTransform expandHorizontal;

		// Token: 0x04002A4E RID: 10830
		public RectTransform collapseHorizontal;

		// Token: 0x04002A4F RID: 10831
		public RectTransform expandVertical;

		// Token: 0x04002A50 RID: 10832
		public RectTransform collpaseVertical;

		// Token: 0x04002A52 RID: 10834
		private bool _collapsed;

		// Token: 0x04002A53 RID: 10835
		private bool _needsUpdate;
	}
}
