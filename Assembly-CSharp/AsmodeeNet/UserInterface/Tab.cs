using System;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007AB RID: 1963
	public class Tab : MonoBehaviour
	{
		// Token: 0x14000146 RID: 326
		// (add) Token: 0x06003898 RID: 14488 RVA: 0x0014AF54 File Offset: 0x00149154
		// (remove) Token: 0x06003899 RID: 14489 RVA: 0x0014AF8C File Offset: 0x0014918C
		public event Action<Tab> OnTabSelected;

		// Token: 0x0600389A RID: 14490 RVA: 0x0014AFC4 File Offset: 0x001491C4
		public void Setup(CommunityHubSkin.TabIcons icons, ToggleGroup toggleGroup, bool isHorizontal)
		{
			this._icons = icons;
			this._horizontal.root.gameObject.SetActive(isHorizontal);
			this._vertical.root.gameObject.SetActive(!isHorizontal);
			RectTransform rectTransform = (isHorizontal ? this._horizontal.root : this._vertical.root);
			(base.transform as RectTransform).sizeDelta = rectTransform.sizeDelta;
			(isHorizontal ? this._horizontal.toggle : this._vertical.toggle).group = toggleGroup;
			this._Update();
		}

		// Token: 0x0600389B RID: 14491 RVA: 0x0004C62D File Offset: 0x0004A82D
		public void OnToggleValueChanged(bool value)
		{
			this.IsOn = value;
			if (value && this.OnTabSelected != null)
			{
				this.OnTabSelected(this);
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x0600389C RID: 14492 RVA: 0x0004C64D File Offset: 0x0004A84D
		// (set) Token: 0x0600389D RID: 14493 RVA: 0x0004C655 File Offset: 0x0004A855
		public bool IsOn
		{
			get
			{
				return this._isOn;
			}
			set
			{
				this._isOn = value;
				this._Update();
			}
		}

		// Token: 0x0600389E RID: 14494 RVA: 0x0014B060 File Offset: 0x00149260
		private void _Update()
		{
			bool activeSelf = this._horizontal.root.gameObject.activeSelf;
			(activeSelf ? this._horizontal.icon : this._vertical.icon).sprite = (this.IsOn ? this._icons.spriteOn : this._icons.spriteOff);
			(activeSelf ? this._horizontal.toggle : this._vertical.toggle).isOn = this.IsOn;
		}

		// Token: 0x04002A84 RID: 10884
		[SerializeField]
		private Tab.TabOutlet _vertical;

		// Token: 0x04002A85 RID: 10885
		[SerializeField]
		private Tab.TabOutlet _horizontal;

		// Token: 0x04002A87 RID: 10887
		private CommunityHubSkin.TabIcons _icons;

		// Token: 0x04002A88 RID: 10888
		private bool _isOn;

		// Token: 0x020007AC RID: 1964
		[Serializable]
		private struct TabOutlet
		{
			// Token: 0x04002A89 RID: 10889
			public RectTransform root;

			// Token: 0x04002A8A RID: 10890
			public Image icon;

			// Token: 0x04002A8B RID: 10891
			public Toggle toggle;
		}
	}
}
