using System;
using System.Collections;
using System.Collections.Generic;
using AsmodeeNet.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007AE RID: 1966
	public class TabBar : MonoBehaviour
	{
		// Token: 0x14000147 RID: 327
		// (add) Token: 0x060038A1 RID: 14497 RVA: 0x0014B0E8 File Offset: 0x001492E8
		// (remove) Token: 0x060038A2 RID: 14498 RVA: 0x0014B120 File Offset: 0x00149320
		public event Action<int> OnTabBarDidSelectItem;

		// Token: 0x060038A3 RID: 14499 RVA: 0x0014B158 File Offset: 0x00149358
		public void OnTabSelected(Tab tab)
		{
			this._selectedItemTag = this._tabBarItems.Find((TabBarItem item) => item.tab == tab).tag;
			if (this.OnTabBarDidSelectItem != null)
			{
				this.OnTabBarDidSelectItem(this._selectedItemTag);
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x060038A4 RID: 14500 RVA: 0x0004C67B File Offset: 0x0004A87B
		// (set) Token: 0x060038A5 RID: 14501 RVA: 0x0014B1B0 File Offset: 0x001493B0
		public int SelectedItemTag
		{
			get
			{
				return this._selectedItemTag;
			}
			set
			{
				this._selectedItemTag = value;
				foreach (TabBarItem tabBarItem in this._tabBarItems)
				{
					tabBarItem.tab.IsOn = tabBarItem.tag == this._selectedItemTag;
				}
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x060038A6 RID: 14502 RVA: 0x0004C683 File Offset: 0x0004A883
		public RectTransform Background
		{
			get
			{
				if (!this.IsHorizontal)
				{
					return this._verticalBackground;
				}
				return this._horizontalBackground;
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x060038A7 RID: 14503 RVA: 0x0004C69A File Offset: 0x0004A89A
		public RectTransform TabContainer
		{
			get
			{
				if (!this.IsHorizontal)
				{
					return this._verticalTabContainer;
				}
				return this._horizontalTabContainer;
			}
		}

		// Token: 0x1700047C RID: 1148
		// (set) Token: 0x060038A8 RID: 14504 RVA: 0x0004C6B1 File Offset: 0x0004A8B1
		public List<TabBarItem> TabBarItems
		{
			set
			{
				if (!this._tabBarItems.Equals(value))
				{
					this._tabBarItems = value;
					this._selectedItemTag = -1;
					this._needsUpdate = true;
				}
			}
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x060038A9 RID: 14505 RVA: 0x0004C6D6 File Offset: 0x0004A8D6
		// (set) Token: 0x060038AA RID: 14506 RVA: 0x0004C6DE File Offset: 0x0004A8DE
		public TabBar.TabBarAnchor Anchor
		{
			get
			{
				return this._anchor;
			}
			set
			{
				if (this._anchor != value)
				{
					this._anchor = value;
					this._needsUpdate = true;
				}
			}
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x060038AB RID: 14507 RVA: 0x0004C6F7 File Offset: 0x0004A8F7
		// (set) Token: 0x060038AC RID: 14508 RVA: 0x0004C6FF File Offset: 0x0004A8FF
		public TabBar.TabAlignment Alignment
		{
			get
			{
				return this._alignment;
			}
			set
			{
				if (this._alignment != value)
				{
					this._alignment = value;
					this._needsUpdate = true;
				}
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x060038AD RID: 14509 RVA: 0x0014B21C File Offset: 0x0014941C
		public float Thickness
		{
			get
			{
				if (!this.IsHorizontal)
				{
					return this._verticalBackground.rect.size.x;
				}
				return this._horizontalBackground.rect.size.y;
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x060038AE RID: 14510 RVA: 0x0004C718 File Offset: 0x0004A918
		public bool IsHorizontal
		{
			get
			{
				return this._anchor == TabBar.TabBarAnchor.Top || this._anchor == TabBar.TabBarAnchor.Bottom;
			}
		}

		// Token: 0x060038AF RID: 14511 RVA: 0x0014B264 File Offset: 0x00149464
		public void UpdateTabs()
		{
			if (!this._needsUpdate)
			{
				return;
			}
			List<Transform> list = new List<Transform>(this._horizontalTabContainer.childCount + this._verticalTabContainer.childCount);
			foreach (Transform transform in new List<Transform> { this._horizontalTabContainer, this._verticalTabContainer })
			{
				using (IEnumerator enumerator2 = ((RectTransform)transform).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						RectTransform tabTransform = (RectTransform)enumerator2.Current;
						if (this._tabBarItems.Find((TabBarItem item) => item.transform == tabTransform).tab == null)
						{
							list.Add(tabTransform);
						}
					}
				}
			}
			foreach (Transform transform2 in list)
			{
				RectTransform rectTransform = (RectTransform)transform2;
				rectTransform.SetParent(null);
				global::UnityEngine.Object.Destroy(rectTransform.gameObject);
			}
			foreach (TabBarItem tabBarItem in this._tabBarItems)
			{
				if (tabBarItem.transform.parent != this.TabContainer)
				{
					tabBarItem.tab.OnTabSelected += this.OnTabSelected;
					tabBarItem.transform.SetParent(this.TabContainer, false);
				}
			}
			if (base.enabled)
			{
				this._needsUpdate = false;
				if (this.IsHorizontal)
				{
					this._horizontalBackground.gameObject.SetActive(true);
					this._verticalBackground.gameObject.SetActive(false);
				}
				else
				{
					this._horizontalBackground.gameObject.SetActive(false);
					this._verticalBackground.gameObject.SetActive(true);
				}
				float num = 0f;
				int num2 = 0;
				foreach (ref TabBarItem ptr in this._tabBarItems)
				{
					num2++;
					RectTransform transform3 = ptr.transform;
					float num3 = (this.IsHorizontal ? (this.Thickness / transform3.rect.size.y) : (this.Thickness / transform3.rect.size.x));
					transform3.localScale = new Vector3(num3, num3, num3);
					if (this.IsHorizontal)
					{
						if ((this.Alignment & TabBar.TabAlignment.Left) != TabBar.TabAlignment.Unknown && (this.Alignment & TabBar.TabAlignment.Right) == TabBar.TabAlignment.Unknown)
						{
							transform3.anchorMin = (transform3.anchorMax = Vector2.zero);
							transform3.pivot = Vector2.zero;
							transform3.anchoredPosition = new Vector2(num, 0f);
							num += transform3.rect.size.x * num3;
						}
						else if ((this.Alignment & TabBar.TabAlignment.Right) != TabBar.TabAlignment.Unknown && (this.Alignment & TabBar.TabAlignment.Left) == TabBar.TabAlignment.Unknown)
						{
							transform3.anchorMin = (transform3.anchorMax = new Vector2(1f, 0f));
							transform3.pivot = new Vector2(1f, 0f);
							transform3.anchoredPosition = new Vector2(num, 0f);
							num -= transform3.rect.size.x * num3;
						}
						else
						{
							float num4 = (float)num2 / ((float)this._tabBarItems.Count + 1f);
							transform3.anchorMin = new Vector2(num4, 0.5f);
							transform3.anchorMax = new Vector2(num4, 0.5f);
							transform3.pivot = new Vector2(0.5f, 0.5f);
							transform3.anchoredPosition = Vector2.zero;
						}
					}
					else
					{
						transform3.anchorMin = (transform3.anchorMax = Vector2.one);
						if ((this.Alignment & TabBar.TabAlignment.Top) != TabBar.TabAlignment.Unknown && (this.Alignment & TabBar.TabAlignment.Bottom) == TabBar.TabAlignment.Unknown)
						{
							transform3.anchorMin = (transform3.anchorMax = Vector2.one);
							transform3.pivot = Vector2.one;
							transform3.anchoredPosition = new Vector2(0f, num);
							num -= transform3.rect.size.y * num3;
						}
						else if ((this.Alignment & TabBar.TabAlignment.Bottom) != TabBar.TabAlignment.Unknown && (this.Alignment & TabBar.TabAlignment.Top) == TabBar.TabAlignment.Unknown)
						{
							transform3.anchorMin = (transform3.anchorMax = new Vector2(1f, 0f));
							transform3.pivot = new Vector2(1f, 0f);
							transform3.anchoredPosition = new Vector2(0f, num);
							num += transform3.rect.size.y * num3;
						}
						else
						{
							float num5 = (float)num2 / ((float)this._tabBarItems.Count + 1f);
							transform3.anchorMin = new Vector2(0.5f, num5);
							transform3.anchorMax = new Vector2(0.5f, num5);
							transform3.pivot = new Vector2(0.5f, 0.5f);
							transform3.anchoredPosition = Vector2.zero;
						}
					}
				}
			}
		}

		// Token: 0x04002A90 RID: 10896
		private int _selectedItemTag = -1;

		// Token: 0x04002A91 RID: 10897
		public ToggleGroup toggleGroup;

		// Token: 0x04002A92 RID: 10898
		[SerializeField]
		private RectTransform _horizontalBackground;

		// Token: 0x04002A93 RID: 10899
		[SerializeField]
		private RectTransform _verticalBackground;

		// Token: 0x04002A94 RID: 10900
		[SerializeField]
		private RectTransform _horizontalTabContainer;

		// Token: 0x04002A95 RID: 10901
		[SerializeField]
		private RectTransform _verticalTabContainer;

		// Token: 0x04002A96 RID: 10902
		private List<TabBarItem> _tabBarItems = new List<TabBarItem>();

		// Token: 0x04002A97 RID: 10903
		private TabBar.TabBarAnchor _anchor;

		// Token: 0x04002A98 RID: 10904
		[SerializeField]
		[EnumFlag]
		private TabBar.TabAlignment _alignment = TabBar.TabAlignment.Distributed;

		// Token: 0x04002A99 RID: 10905
		private bool _needsUpdate;

		// Token: 0x020007AF RID: 1967
		public enum TabBarAnchor
		{
			// Token: 0x04002A9B RID: 10907
			Unknown,
			// Token: 0x04002A9C RID: 10908
			Top,
			// Token: 0x04002A9D RID: 10909
			Bottom,
			// Token: 0x04002A9E RID: 10910
			Left,
			// Token: 0x04002A9F RID: 10911
			Right
		}

		// Token: 0x020007B0 RID: 1968
		[Flags]
		public enum TabAlignment
		{
			// Token: 0x04002AA1 RID: 10913
			Unknown = 0,
			// Token: 0x04002AA2 RID: 10914
			Left = 1,
			// Token: 0x04002AA3 RID: 10915
			Right = 2,
			// Token: 0x04002AA4 RID: 10916
			Top = 4,
			// Token: 0x04002AA5 RID: 10917
			Bottom = 8,
			// Token: 0x04002AA6 RID: 10918
			Distributed = 15
		}
	}
}
