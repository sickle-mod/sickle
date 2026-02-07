using System;
using System.Collections.Generic;
using System.Linq;
using AsmodeeNet.Foundation;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000796 RID: 1942
	public class CommunityHubLayout : MonoBehaviour
	{
		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x0600382B RID: 14379 RVA: 0x0004C12B File Offset: 0x0004A32B
		public Vector2 SafeAreaAnchorMin
		{
			get
			{
				if (this._layoutContext == null)
				{
					return Vector2.zero;
				}
				return this._layoutContext.safeAreaAnchorMin;
			}
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x0600382C RID: 14380 RVA: 0x0004C146 File Offset: 0x0004A346
		public Vector2 SafeAreaAnchorMax
		{
			get
			{
				if (this._layoutContext == null)
				{
					return Vector2.one;
				}
				return this._layoutContext.safeAreaAnchorMax;
			}
		}

		// Token: 0x0600382D RID: 14381 RVA: 0x00149000 File Offset: 0x00147200
		private void OnEnable()
		{
			this._needsUpdate = true;
			this.IsCollapsed = this._IsCollapsible;
			CoreApplication.Instance.Preferences.InterfaceOrientationDidChange += this._InterfaceOrientationDidChange;
			CoreApplication.Instance.Preferences.InterfaceDisplayModeDidChange += this._InterfaceDisplayModeDidChange;
		}

		// Token: 0x0600382E RID: 14382 RVA: 0x0004C161 File Offset: 0x0004A361
		private void Update()
		{
			this.UpdateLayout(false);
			if (!base.enabled)
			{
				return;
			}
			this._tabBar.UpdateTabs();
		}

		// Token: 0x0600382F RID: 14383 RVA: 0x00149058 File Offset: 0x00147258
		private void OnDisable()
		{
			if (!CoreApplication.IsQuitting)
			{
				CoreApplication.Instance.Preferences.InterfaceOrientationDidChange -= this._InterfaceOrientationDidChange;
				CoreApplication.Instance.Preferences.InterfaceDisplayModeDidChange -= this._InterfaceDisplayModeDidChange;
				this.UpdateLayout(true);
			}
		}

		// Token: 0x06003830 RID: 14384 RVA: 0x0004C17E File Offset: 0x0004A37E
		public void SetNeedsUpdateLayout()
		{
			this._needsUpdate = true;
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06003831 RID: 14385 RVA: 0x0004C187 File Offset: 0x0004A387
		// (set) Token: 0x06003832 RID: 14386 RVA: 0x0004C18F File Offset: 0x0004A38F
		public bool IsCollapsed
		{
			get
			{
				return this._collapsed;
			}
			set
			{
				this._collapsed = value;
				if (this._expandCollapseButton != null)
				{
					this._expandCollapseButton.IsCollapsed = this._collapsed;
				}
				this._needsUpdate = true;
			}
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06003833 RID: 14387 RVA: 0x001490AC File Offset: 0x001472AC
		private bool _IsCollapsible
		{
			get
			{
				return CoreApplication.Instance.Preferences.InterfaceDisplayMode == Preferences.DisplayMode.Small || (!MathUtils.Approximately(this._aspect, this.regularAspectRatioLimitVertical, 0.01f) && !MathUtils.Approximately(this._aspect, this.regularAspectRatioLimitHorizontal, 0.01f) && this.regularAspectRatioLimitVertical + 0.01f < this._aspect && this._aspect < this.regularAspectRatioLimitHorizontal - 0.01f);
			}
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06003834 RID: 14388 RVA: 0x00149128 File Offset: 0x00147328
		private bool _DisplayTabBarWhileCollapsed
		{
			get
			{
				if (CoreApplication.Instance.Preferences.InterfaceDisplayMode == Preferences.DisplayMode.Small)
				{
					if (CoreApplication.Instance.Preferences.InterfaceOrientation == Preferences.Orientation.Horizontal)
					{
						float num = this._tabBar.Thickness / this.container.rect.size.x;
						if (this._aspect * (1f - num) > this.compactAspectRatioLimitHorizontal)
						{
							return true;
						}
					}
					else
					{
						float num2 = this._tabBar.Thickness / this.container.rect.size.y;
						if (this._aspect / (1f - num2) < this.compactAspectRatioLimitVertical)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06003835 RID: 14389 RVA: 0x001491D8 File Offset: 0x001473D8
		public void UpdateLayout(bool force = false)
		{
			Preferences.DisplayMode interfaceDisplayMode = CoreApplication.Instance.Preferences.InterfaceDisplayMode;
			bool flag = this._displayMode != interfaceDisplayMode;
			this._displayMode = interfaceDisplayMode;
			if (flag)
			{
				this._DestroyExpandCollapseButton();
				this._DestroyTabBar();
				this._CreateExpandCollapseButton();
				this._CreateTabBar();
			}
			float aspect = CoreApplication.Instance.Preferences.Aspect;
			if (!MathUtils.Approximately(aspect, this._aspect, 0.01f))
			{
				if (CoreApplication.Instance.Preferences.InterfaceDisplayMode != Preferences.DisplayMode.Small)
				{
					float num = this.regularAspectRatioLimitHorizontal - 0.01f;
					float num2 = this.regularAspectRatioLimitVertical + 0.01f;
					if ((this._aspect >= num && aspect < num) || (aspect >= num && this._aspect < num) || (this._aspect <= num2 && aspect > num2) || (aspect <= num2 && this._aspect > num2))
					{
						this._needsUpdate = true;
					}
				}
				else if (CoreApplication.Instance.Preferences.InterfaceOrientation == Preferences.Orientation.Horizontal)
				{
					float num3 = this._tabBar.Thickness / this.container.rect.size.x;
					float num4 = this._aspect * (1f - num3);
					float num5 = aspect * (1f - num3);
					if ((num4 >= this.compactAspectRatioLimitHorizontal && num5 < this.compactAspectRatioLimitHorizontal) || (num4 < this.compactAspectRatioLimitHorizontal && num5 >= this.compactAspectRatioLimitHorizontal))
					{
						this._needsUpdate = true;
					}
				}
				else
				{
					float num6 = this._tabBar.Thickness / this.container.rect.size.y;
					float num7 = this._aspect / (1f - num6);
					float num8 = aspect / (1f - num6);
					if ((num7 >= this.compactAspectRatioLimitVertical && num8 < this.compactAspectRatioLimitVertical) || (num7 < this.compactAspectRatioLimitVertical && num8 >= this.compactAspectRatioLimitVertical))
					{
						this._needsUpdate = true;
					}
				}
				this._aspect = aspect;
			}
			if (this._needsUpdate || force)
			{
				if (!this._IsCollapsible)
				{
					this.IsCollapsed = false;
				}
				if (MathUtils.Approximately(this.container.rect.size, this._containerSize, 1f))
				{
					this._needsUpdate = false;
				}
				this._containerSize = this.container.rect.size;
				this._layoutContext = new CommunityHubLayout.LayoutContext();
				this._UpdateContentParenthood(flag);
				this._UpdateContainerLayout();
				if (CoreApplication.Instance.Preferences.InterfaceOrientation == Preferences.Orientation.Horizontal)
				{
					this._UpdateHeadersLayout();
					this._UpdateTabsLayout();
					this._UpdateExpandCollapseButtonLayout();
				}
				else
				{
					this._UpdateTabsLayout();
					this._UpdateExpandCollapseButtonLayout();
					this._UpdateHeadersLayout();
				}
				this._UpdateAutoLayoutTransforms();
				CoreApplication.Instance.CommunityHub.CallLayoutDidChangeEvent();
			}
		}

		// Token: 0x06003836 RID: 14390 RVA: 0x00149490 File Offset: 0x00147690
		private void _UpdateContentParenthood(bool forceReload)
		{
			CommunityHub communityHub = CoreApplication.Instance.CommunityHub;
			foreach (object obj in this.container)
			{
				RectTransform rectTransform = (RectTransform)obj;
				bool flag = true;
				if (!forceReload)
				{
					foreach (CommunityHubContent communityHubContent in communityHub.CommunityHubContents)
					{
						if (rectTransform == communityHubContent.DisplayedContent)
						{
							flag = false;
						}
					}
				}
				if (rectTransform.transform == this._expandCollapseButtonRoot.transform || rectTransform.transform == this._tabBarRoot.transform)
				{
					flag = false;
				}
				if (flag)
				{
					rectTransform.SetParent(null);
					this._transformToContent[rectTransform].DisposeDisplayedContent();
					this._transformToContent.Remove(rectTransform);
				}
			}
			foreach (CommunityHubContent communityHubContent2 in communityHub.CommunityHubContents)
			{
				communityHubContent2.LoadDisplayedContent(this._displayMode);
				RectTransform displayedContent = communityHubContent2.DisplayedContent;
				if (displayedContent != null && displayedContent.parent != this.container)
				{
					displayedContent.SetParent(this.container, false);
					this._transformToContent.Add(displayedContent, communityHubContent2);
				}
			}
		}

		// Token: 0x06003837 RID: 14391 RVA: 0x00149634 File Offset: 0x00147834
		private void _UpdateContainerLayout()
		{
			if (!base.enabled)
			{
				return;
			}
			List<CommunityHubContent> list = this._ContentsWithLayout(CommunityHubContent.Layout.Header);
			List<CommunityHubContent> list2 = this._ContentsWithLayout(CommunityHubContent.Layout.Tab);
			foreach (CommunityHubContent communityHubContent in list)
			{
				communityHubContent.DisplayedContent.SetAsFirstSibling();
			}
			if (list2.Count > 0)
			{
				Preferences.Orientation interfaceOrientation = CoreApplication.Instance.Preferences.InterfaceOrientation;
				if (CoreApplication.Instance.Preferences.InterfaceDisplayMode == Preferences.DisplayMode.Small)
				{
					if (interfaceOrientation == Preferences.Orientation.Horizontal)
					{
						float num = this._tabBar.Thickness / this.container.rect.size.x;
						if (this._aspect * (1f - num) > this.compactAspectRatioLimitHorizontal)
						{
							this._layoutContext.safeAreaAnchorMax = new Vector2(1f - num, 1f);
							return;
						}
					}
					else
					{
						float num2 = this._tabBar.Thickness / this.container.rect.size.y;
						if (this._aspect / (1f - num2) < this.compactAspectRatioLimitVertical)
						{
							this._layoutContext.safeAreaAnchorMax = new Vector2(1f, 1f - num2);
							return;
						}
					}
				}
				else if (!this._IsCollapsible)
				{
					if (interfaceOrientation == Preferences.Orientation.Horizontal)
					{
						this._layoutContext.safeAreaAnchorMax = new Vector2(0.75f, 1f);
						return;
					}
					this._layoutContext.safeAreaAnchorMax = new Vector2(1f, 0.75f);
				}
			}
		}

		// Token: 0x06003838 RID: 14392 RVA: 0x001497CC File Offset: 0x001479CC
		private void _UpdateHeadersLayout()
		{
			if (!base.enabled)
			{
				return;
			}
			List<CommunityHubContent> list = this._ContentsWithLayout(CommunityHubContent.Layout.Header);
			List<CommunityHubContent> list2 = this._ContentsWithLayout(CommunityHubContent.Layout.Tab);
			Vector2 one = Vector2.one;
			if (list2.Count > 0 && CoreApplication.Instance.Preferences.InterfaceOrientation == Preferences.Orientation.Vertical)
			{
				if (CoreApplication.Instance.Preferences.InterfaceDisplayMode == Preferences.DisplayMode.Small)
				{
					one = new Vector2(1f, this._layoutContext.tabsAnchorMin.y);
				}
				else if (!this._IsCollapsible || !this.IsCollapsed)
				{
					one = new Vector2(1f, this._layoutContext.tabsAnchorMin.y);
				}
			}
			Vector3 vector = new Vector3(-this._layoutContext.expandCollapseButtonWidth, 0f, 0f);
			foreach (CommunityHubContent communityHubContent in list)
			{
				RectTransform displayedContent = communityHubContent.DisplayedContent;
				displayedContent.anchorMin = one;
				displayedContent.anchorMax = one;
				displayedContent.anchoredPosition = vector;
				vector.y -= displayedContent.rect.size.y;
			}
			this._layoutContext.headersHeight = -vector.y;
		}

		// Token: 0x06003839 RID: 14393 RVA: 0x00149914 File Offset: 0x00147B14
		private void _UpdateTabsLayout()
		{
			if (!base.enabled)
			{
				return;
			}
			List<CommunityHubContent> list = this._ContentsWithLayout(CommunityHubContent.Layout.Tab);
			if (list.Count > 0)
			{
				Preferences.Orientation interfaceOrientation = CoreApplication.Instance.Preferences.InterfaceOrientation;
				int num = ((CoreApplication.Instance.Preferences.InterfaceDisplayMode == Preferences.DisplayMode.Small) ? 1 : 2);
				bool flag = list.Count >= num && (!this.IsCollapsed || (this.IsCollapsed && this._DisplayTabBarWhileCollapsed));
				this._tabBarRoot.SetActive(flag);
				CommunityHubSkin skin = CoreApplication.Instance.CommunityHubLauncher.communityHubSkin;
				if (flag)
				{
					if (CoreApplication.Instance.Preferences.InterfaceDisplayMode == Preferences.DisplayMode.Small)
					{
						if (interfaceOrientation == Preferences.Orientation.Horizontal)
						{
							this._tabBar.Anchor = (this.IsCollapsed ? TabBar.TabBarAnchor.Left : TabBar.TabBarAnchor.Right);
						}
						else
						{
							this._tabBar.Anchor = (this.IsCollapsed ? TabBar.TabBarAnchor.Bottom : TabBar.TabBarAnchor.Top);
						}
					}
					else
					{
						this._tabBar.Anchor = ((interfaceOrientation == Preferences.Orientation.Horizontal) ? TabBar.TabBarAnchor.Top : TabBar.TabBarAnchor.Right);
					}
				}
				List<TabBarItem> list2 = new List<TabBarItem>();
				Func<CommunityHubSkin.TabIcons, int, TabBarItem> func = delegate(CommunityHubSkin.TabIcons tabIcons, int tag)
				{
					GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(skin.tabPrefabs[this._displayMode]);
					Tab component = gameObject.GetComponent<Tab>();
					RectTransform rectTransform2 = gameObject.transform as RectTransform;
					component.Setup(tabIcons, this._tabBar.toggleGroup, this._tabBar.IsHorizontal);
					return new TabBarItem(tag, component, rectTransform2);
				};
				int i;
				for (i = 0; i < list.Count; i++)
				{
					CommunityHubContent communityHubContent = list[i];
					list2.Add(func(communityHubContent.TabIcons, i));
				}
				if (CoreApplication.Instance.Preferences.InterfaceDisplayMode == Preferences.DisplayMode.Small && !this.IsCollapsed)
				{
					Sprite sprite = ((interfaceOrientation == Preferences.Orientation.Horizontal) ? skin.collapseButtonInVerticalTabBarIcon : skin.collapseButtonInHorizontalTabBarIcon);
					CommunityHubSkin.TabIcons tabIcons2 = new CommunityHubSkin.TabIcons
					{
						spriteOff = sprite
					};
					list2.Insert(0, func(tabIcons2, i + 1));
				}
				this._tabBar.TabBarItems = list2;
				if (this._SelectedContentInTabs == null || !list.Contains(this._SelectedContentInTabs))
				{
					this._SelectedContentInTabs = list[0];
					this._tabBar.SelectedItemTag = 0;
				}
				else
				{
					this._SelectedContentInTabs = this._SelectedContentInTabs;
					this._tabBar.SelectedItemTag = list.IndexOf(this._SelectedContentInTabs);
				}
				if (CoreApplication.Instance.Preferences.InterfaceDisplayMode == Preferences.DisplayMode.Small)
				{
					if (interfaceOrientation == Preferences.Orientation.Horizontal)
					{
						if (this.IsCollapsed)
						{
							if (this._DisplayTabBarWhileCollapsed)
							{
								float num2 = this._tabBar.Thickness / this.container.rect.size.x;
								this._layoutContext.tabsAnchorMin = new Vector2(1f - num2, 0f);
								this._layoutContext.tabsAnchorMax = new Vector2(2f - num2, 1f);
							}
							else
							{
								this._layoutContext.tabsAnchorMin = new Vector2(1f, 0f);
								this._layoutContext.tabsAnchorMax = new Vector2(2f, 1f);
							}
						}
					}
					else if (this.IsCollapsed)
					{
						if (this._DisplayTabBarWhileCollapsed)
						{
							float num3 = this._tabBar.Thickness / this.container.rect.size.y;
							this._layoutContext.tabsAnchorMin = new Vector2(0f, 1f - num3);
							this._layoutContext.tabsAnchorMax = new Vector2(1f, 2f - num3);
						}
						else
						{
							this._layoutContext.tabsAnchorMin = new Vector2(0f, 1f);
							this._layoutContext.tabsAnchorMax = new Vector2(1f, 2f);
						}
					}
				}
				else if (interfaceOrientation == Preferences.Orientation.Horizontal)
				{
					if (this.IsCollapsed && this._IsCollapsible)
					{
						this._layoutContext.tabsAnchorMin = new Vector2(1f, 0f);
						this._layoutContext.tabsAnchorMax = new Vector2(1.25f, 1f);
					}
					else
					{
						this._layoutContext.tabsAnchorMin = new Vector2(0.75f, 0f);
						this._layoutContext.tabsAnchorMax = new Vector2(1f, 1f);
					}
				}
				else if (this.IsCollapsed && this._IsCollapsible)
				{
					this._layoutContext.tabsAnchorMin = new Vector2(0f, 1f);
					this._layoutContext.tabsAnchorMax = new Vector2(1f, 1.25f);
				}
				else
				{
					this._layoutContext.tabsAnchorMin = new Vector2(0f, 0.75f);
					this._layoutContext.tabsAnchorMax = new Vector2(1f, 1f);
				}
				Vector2 zero = Vector2.zero;
				Vector2 zero2 = Vector2.zero;
				if (CoreApplication.Instance.Preferences.InterfaceDisplayMode != Preferences.DisplayMode.Small || this.IsCollapsed)
				{
					zero2 = new Vector2(0f, -this._layoutContext.headersHeight);
				}
				if (flag)
				{
					RectTransform rectTransform = (RectTransform)this._tabBarRoot.transform;
					float thickness = this._tabBar.Thickness;
					switch (this._tabBar.Anchor)
					{
					case TabBar.TabBarAnchor.Top:
						rectTransform.anchorMin = new Vector2(this._layoutContext.tabsAnchorMin.x, this._layoutContext.tabsAnchorMax.y);
						rectTransform.anchorMax = this._layoutContext.tabsAnchorMax;
						rectTransform.offsetMax = new Vector2(0f, zero2.y);
						zero2.y -= thickness;
						rectTransform.offsetMin = new Vector2(0f, zero2.y);
						break;
					case TabBar.TabBarAnchor.Bottom:
						rectTransform.anchorMin = this._layoutContext.tabsAnchorMin;
						rectTransform.anchorMax = new Vector2(this._layoutContext.tabsAnchorMax.x, this._layoutContext.tabsAnchorMin.y);
						rectTransform.offsetMin = Vector2.zero;
						rectTransform.offsetMax = new Vector2(0f, thickness);
						zero.y += thickness;
						break;
					case TabBar.TabBarAnchor.Left:
						rectTransform.anchorMin = this._layoutContext.tabsAnchorMin;
						rectTransform.anchorMax = new Vector2(this._layoutContext.tabsAnchorMin.x, this._layoutContext.tabsAnchorMax.y);
						rectTransform.offsetMin = Vector2.zero;
						rectTransform.offsetMax = new Vector2(thickness, zero2.y);
						zero.x += thickness;
						break;
					case TabBar.TabBarAnchor.Right:
						rectTransform.anchorMin = new Vector2(this._layoutContext.tabsAnchorMax.x, this._layoutContext.tabsAnchorMin.y);
						rectTransform.anchorMax = this._layoutContext.tabsAnchorMax;
						rectTransform.offsetMin = new Vector2(-thickness, 0f);
						rectTransform.offsetMax = new Vector2(0f, -zero2.y);
						zero2.x -= thickness;
						break;
					}
				}
				using (List<CommunityHubContent>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CommunityHubContent communityHubContent2 = enumerator.Current;
						RectTransform displayedContent = communityHubContent2.DisplayedContent;
						displayedContent.anchorMin = this._layoutContext.tabsAnchorMin;
						displayedContent.anchorMax = this._layoutContext.tabsAnchorMax;
						displayedContent.offsetMin = zero;
						displayedContent.offsetMax = zero2;
					}
					return;
				}
			}
			this._SelectedContentInTabs = null;
			this._tabBar.TabBarItems = new List<TabBarItem>();
			this._tabBarRoot.SetActive(false);
		}

		// Token: 0x0600383A RID: 14394 RVA: 0x0014A0A0 File Offset: 0x001482A0
		private void _UpdateExpandCollapseButtonLayout()
		{
			if (this._ContentsWithLayout(CommunityHubContent.Layout.Tab).Count <= 0 || !this._IsCollapsible)
			{
				this._expandCollapseButtonRoot.SetActive(false);
				return;
			}
			if (CoreApplication.Instance.Preferences.InterfaceDisplayMode != Preferences.DisplayMode.Small || (this.IsCollapsed && !this._DisplayTabBarWhileCollapsed))
			{
				this._expandCollapseButtonRoot.SetActive(true);
				Vector2 vector;
				if (CoreApplication.Instance.Preferences.InterfaceOrientation == Preferences.Orientation.Horizontal)
				{
					vector.x = this._layoutContext.tabsAnchorMin.x;
					vector.y = this._layoutContext.tabsAnchorMax.y;
				}
				else
				{
					vector.x = this._layoutContext.tabsAnchorMax.x;
					vector.y = this._layoutContext.tabsAnchorMin.y;
				}
				RectTransform rectTransform = (RectTransform)this._expandCollapseButtonRoot.transform;
				rectTransform.anchorMin = vector;
				rectTransform.anchorMax = vector;
				rectTransform.pivot = Vector2.one;
				rectTransform.anchoredPosition = new Vector2(0f, -this._layoutContext.headersHeight);
				this._layoutContext.expandCollapseButtonWidth = rectTransform.rect.size.x;
				return;
			}
			this._expandCollapseButtonRoot.SetActive(false);
		}

		// Token: 0x0600383B RID: 14395 RVA: 0x0014A1EC File Offset: 0x001483EC
		private void _UpdateAutoLayoutTransforms()
		{
			foreach (RectTransform rectTransform in CoreApplication.Instance.CommunityHub.TransformsToAutoLayout)
			{
				rectTransform.anchorMin = this._layoutContext.safeAreaAnchorMin;
				rectTransform.anchorMax = this._layoutContext.safeAreaAnchorMax;
			}
		}

		// Token: 0x0600383C RID: 14396 RVA: 0x0014A25C File Offset: 0x0014845C
		private List<CommunityHubContent> _ContentsWithLayout(CommunityHubContent.Layout layout)
		{
			return CoreApplication.Instance.CommunityHub.CommunityHubContents.Where((CommunityHubContent content) => content.DisplayedLayout == layout).ToList<CommunityHubContent>();
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x0600383D RID: 14397 RVA: 0x0004C1BE File Offset: 0x0004A3BE
		// (set) Token: 0x0600383E RID: 14398 RVA: 0x0014A29C File Offset: 0x0014849C
		public CommunityHubContent _SelectedContentInTabs
		{
			get
			{
				return this._selectedContentInTabs;
			}
			set
			{
				this._selectedContentInTabs = value;
				if (this._SelectedContentInTabs != null)
				{
					this._SelectedContentInTabs.DisplayedContent.SetAsLastSibling();
					this._tabBarRoot.transform.SetAsLastSibling();
				}
				foreach (CommunityHubContent communityHubContent in this._ContentsWithLayout(CommunityHubContent.Layout.Tab))
				{
					if (communityHubContent == this._SelectedContentInTabs)
					{
						communityHubContent.DisplayedContent.gameObject.SetActive(true);
					}
					else if (communityHubContent.BackgroundActivity == CommunityHubContent.ActivityType.Inactive)
					{
						communityHubContent.DisplayedContent.gameObject.SetActive(false);
					}
				}
			}
		}

		// Token: 0x0600383F RID: 14399 RVA: 0x0004C17E File Offset: 0x0004A37E
		private void _InterfaceOrientationDidChange()
		{
			this._needsUpdate = true;
		}

		// Token: 0x06003840 RID: 14400 RVA: 0x0004C17E File Offset: 0x0004A37E
		private void _InterfaceDisplayModeDidChange()
		{
			this._needsUpdate = true;
		}

		// Token: 0x06003841 RID: 14401 RVA: 0x0014A350 File Offset: 0x00148550
		private void _CreateExpandCollapseButton()
		{
			if (this._expandCollapseButtonRoot != null || this._expandCollapseButton != null)
			{
				return;
			}
			CommunityHubSkin communityHubSkin = CoreApplication.Instance.CommunityHubLauncher.communityHubSkin;
			this._expandCollapseButtonRoot = global::UnityEngine.Object.Instantiate<GameObject>(communityHubSkin.expandCollapseButtonPrefabs[this._displayMode]);
			this._expandCollapseButton = this._expandCollapseButtonRoot.GetComponent<ExpandCollapseButton>();
			if (this._expandCollapseButtonRoot == null || this._expandCollapseButton == null)
			{
				AsmoLogger.Debug("CommunityHubLayout", "Incomplete ExpandCollapseButton Prefab", null);
				this._DestroyExpandCollapseButton();
			}
			this._expandCollapseButton.IsCollapsed = this._collapsed;
			this._expandCollapseButton.onExpandCollapseButtonClicked += this._OnExpandCollapseButtonClicked;
			this._expandCollapseButtonRoot.transform.SetParent(this.container, false);
		}

		// Token: 0x06003842 RID: 14402 RVA: 0x0004C1C6 File Offset: 0x0004A3C6
		private void _DestroyExpandCollapseButton()
		{
			if (this._expandCollapseButtonRoot != null)
			{
				this._expandCollapseButtonRoot.transform.SetParent(null);
				global::UnityEngine.Object.Destroy(this._expandCollapseButtonRoot);
				this._expandCollapseButtonRoot = null;
				this._expandCollapseButton = null;
			}
		}

		// Token: 0x06003843 RID: 14403 RVA: 0x0004C200 File Offset: 0x0004A400
		private void _OnExpandCollapseButtonClicked()
		{
			this.IsCollapsed = !this.IsCollapsed;
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06003844 RID: 14404 RVA: 0x0004C211 File Offset: 0x0004A411
		// (set) Token: 0x06003845 RID: 14405 RVA: 0x0004C219 File Offset: 0x0004A419
		public TabBar.TabAlignment TabBarAlignment
		{
			get
			{
				return this._tabBarAlignment;
			}
			set
			{
				this._tabBarAlignment = value;
				if (this._tabBar != null)
				{
					this._tabBar.Alignment = this._tabBarAlignment;
				}
			}
		}

		// Token: 0x06003846 RID: 14406 RVA: 0x0014A428 File Offset: 0x00148628
		private void _CreateTabBar()
		{
			if (this._tabBarRoot != null || this._tabBar != null)
			{
				return;
			}
			CommunityHubSkin communityHubSkin = CoreApplication.Instance.CommunityHubLauncher.communityHubSkin;
			this._tabBarRoot = global::UnityEngine.Object.Instantiate<GameObject>(communityHubSkin.tabBarPrefabs[this._displayMode]);
			this._tabBar = this._tabBarRoot.GetComponent<TabBar>();
			if (this._tabBarRoot == null || this._tabBar == null)
			{
				AsmoLogger.Debug("CommunityHubLayout", "Incomplete TabBar Prefab", null);
				this._DestroyTabBar();
			}
			if (this.TabBarAlignment == TabBar.TabAlignment.Unknown)
			{
				this.TabBarAlignment = this._tabBar.Alignment;
			}
			else
			{
				this._tabBar.Alignment = this.TabBarAlignment;
			}
			this._tabBar.OnTabBarDidSelectItem += this._OnTabBarDidSelectItem;
			this._tabBarRoot.transform.SetParent(this.container, false);
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x0004C241 File Offset: 0x0004A441
		private void _DestroyTabBar()
		{
			if (this._tabBarRoot != null)
			{
				this._tabBarRoot.transform.SetParent(null);
				global::UnityEngine.Object.Destroy(this._tabBarRoot);
				this._tabBarRoot = null;
				this._tabBar = null;
			}
		}

		// Token: 0x06003848 RID: 14408 RVA: 0x0014A51C File Offset: 0x0014871C
		private void _OnTabBarDidSelectItem(int tag)
		{
			List<CommunityHubContent> list = this._ContentsWithLayout(CommunityHubContent.Layout.Tab);
			if (tag < list.Count)
			{
				this._SelectedContentInTabs = list[tag];
				if (this.IsCollapsed)
				{
					this._OnExpandCollapseButtonClicked();
					return;
				}
			}
			else
			{
				this._OnExpandCollapseButtonClicked();
			}
		}

		// Token: 0x04002A1F RID: 10783
		private const string _debugModuleName = "CommunityHubLayout";

		// Token: 0x04002A20 RID: 10784
		public RectTransform container;

		// Token: 0x04002A21 RID: 10785
		private CommunityHubLayout.LayoutContext _layoutContext;

		// Token: 0x04002A22 RID: 10786
		private Dictionary<RectTransform, CommunityHubContent> _transformToContent = new Dictionary<RectTransform, CommunityHubContent>();

		// Token: 0x04002A23 RID: 10787
		private bool _needsUpdate;

		// Token: 0x04002A24 RID: 10788
		private bool _collapsed;

		// Token: 0x04002A25 RID: 10789
		private float _aspect;

		// Token: 0x04002A26 RID: 10790
		private Preferences.DisplayMode _displayMode;

		// Token: 0x04002A27 RID: 10791
		private const float kEpsilon = 0.01f;

		// Token: 0x04002A28 RID: 10792
		public float regularAspectRatioLimitHorizontal = 1.4545455f;

		// Token: 0x04002A29 RID: 10793
		public float regularAspectRatioLimitVertical = 0.6875f;

		// Token: 0x04002A2A RID: 10794
		public float compactAspectRatioLimitHorizontal = 1.3333334f;

		// Token: 0x04002A2B RID: 10795
		public float compactAspectRatioLimitVertical = 0.75f;

		// Token: 0x04002A2C RID: 10796
		private Vector2 _containerSize;

		// Token: 0x04002A2D RID: 10797
		private CommunityHubContent _selectedContentInTabs;

		// Token: 0x04002A2E RID: 10798
		private GameObject _expandCollapseButtonRoot;

		// Token: 0x04002A2F RID: 10799
		private ExpandCollapseButton _expandCollapseButton;

		// Token: 0x04002A30 RID: 10800
		private GameObject _tabBarRoot;

		// Token: 0x04002A31 RID: 10801
		private TabBar _tabBar;

		// Token: 0x04002A32 RID: 10802
		private TabBar.TabAlignment _tabBarAlignment;

		// Token: 0x02000797 RID: 1943
		private class LayoutContext
		{
			// Token: 0x04002A33 RID: 10803
			public Vector2 safeAreaAnchorMin = Vector2.zero;

			// Token: 0x04002A34 RID: 10804
			public Vector2 safeAreaAnchorMax = Vector2.one;

			// Token: 0x04002A35 RID: 10805
			public float headersHeight;

			// Token: 0x04002A36 RID: 10806
			public float expandCollapseButtonWidth;

			// Token: 0x04002A37 RID: 10807
			public Vector2 tabsAnchorMin = Vector2.zero;

			// Token: 0x04002A38 RID: 10808
			public Vector2 tabsAnchorMax = Vector2.one;
		}
	}
}
