using System;
using System.Collections;
using System.Collections.Generic;
using AsmodeeNet.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007D0 RID: 2000
	[RequireComponent(typeof(ScrollRect))]
	public class TableView : MonoBehaviour
	{
		// Token: 0x1400014D RID: 333
		// (add) Token: 0x0600392E RID: 14638 RVA: 0x0014CFC4 File Offset: 0x0014B1C4
		// (remove) Token: 0x0600392F RID: 14639 RVA: 0x0014CFFC File Offset: 0x0014B1FC
		public event Action<int, bool> OnCellVisibilityChanged;

		// Token: 0x1400014E RID: 334
		// (add) Token: 0x06003930 RID: 14640 RVA: 0x0014D034 File Offset: 0x0014B234
		// (remove) Token: 0x06003931 RID: 14641 RVA: 0x0014D06C File Offset: 0x0014B26C
		public event Action OnScrollOver;

		// Token: 0x1400014F RID: 335
		// (add) Token: 0x06003932 RID: 14642 RVA: 0x0014D0A4 File Offset: 0x0014B2A4
		// (remove) Token: 0x06003933 RID: 14643 RVA: 0x0014D0DC File Offset: 0x0014B2DC
		public event Action<int> OnCellSelected;

		// Token: 0x14000150 RID: 336
		// (add) Token: 0x06003934 RID: 14644 RVA: 0x0014D114 File Offset: 0x0014B314
		// (remove) Token: 0x06003935 RID: 14645 RVA: 0x0014D14C File Offset: 0x0014B34C
		public event Action<int> OnCellDeselected;

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06003936 RID: 14646 RVA: 0x0004CCE7 File Offset: 0x0004AEE7
		// (set) Token: 0x06003937 RID: 14647 RVA: 0x0004CCEF File Offset: 0x0004AEEF
		public ITableViewDataSource DataSource
		{
			get
			{
				return this._dataSource;
			}
			set
			{
				this._dataSource = value;
				this._requiresReload = true;
			}
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06003938 RID: 14648 RVA: 0x0014D184 File Offset: 0x0014B384
		// (set) Token: 0x06003939 RID: 14649 RVA: 0x0014D1BC File Offset: 0x0014B3BC
		public int SelectedCellIndex
		{
			get
			{
				if (this._SelectedRow == null)
				{
					return -1;
				}
				return this._CellIndexFromRow(this._SelectedRow.Value, null);
			}
			set
			{
				this._SelectedRow = new int?(this._RowFromCellIndex(value, null));
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x0600393A RID: 14650 RVA: 0x0004CCFF File Offset: 0x0004AEFF
		public bool HasSelection
		{
			get
			{
				return this._SelectedRow != null;
			}
		}

		// Token: 0x0600393B RID: 14651 RVA: 0x0014D1E4 File Offset: 0x0014B3E4
		public TableViewCell GetReusableCell(string reuseIdentifier)
		{
			LinkedList<TableViewCell> linkedList;
			if (!this._reusableCells.TryGetValue(reuseIdentifier, out linkedList))
			{
				return null;
			}
			if (linkedList.Count == 0)
			{
				return null;
			}
			TableViewCell value = linkedList.First.Value;
			linkedList.RemoveFirst();
			return value;
		}

		// Token: 0x0600393C RID: 14652 RVA: 0x0014D220 File Offset: 0x0014B420
		private void _StoreCellForReuse(TableViewCell cell)
		{
			string reuseIdentifier = cell.ReuseIdentifier;
			if (string.IsNullOrEmpty(reuseIdentifier))
			{
				global::UnityEngine.Object.Destroy(cell.gameObject);
				return;
			}
			if (!this._reusableCells.ContainsKey(reuseIdentifier))
			{
				this._reusableCells.Add(reuseIdentifier, new LinkedList<TableViewCell>());
			}
			this._reusableCells[reuseIdentifier].AddLast(cell);
			cell.Clean();
			cell.transform.SetParent(this._reusableCellContainer, false);
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x0600393D RID: 14653 RVA: 0x0004CD0C File Offset: 0x0004AF0C
		public bool IsEmpty
		{
			get
			{
				return this.DataSource != null && this.DataSource.GetNumberOfCellsInTableView(this) == 0;
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x0600393E RID: 14654 RVA: 0x0004CD27 File Offset: 0x0004AF27
		// (set) Token: 0x0600393F RID: 14655 RVA: 0x0004CD2F File Offset: 0x0004AF2F
		public TableView.ScrollAnchor ScrollingStartingOrder
		{
			get
			{
				return this._scrollingStartingOrder;
			}
			set
			{
				this._scrollingStartingOrder = value;
				this._UpdateScrollAnchor();
				this._requiresReload = true;
			}
		}

		// Token: 0x06003940 RID: 14656 RVA: 0x0014D294 File Offset: 0x0014B494
		private void _UpdateScrollAnchor()
		{
			if (this.ScrollingStartingOrder == TableView.ScrollAnchor.Top)
			{
				this._verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
				this._scrollRect.content.anchorMin = new Vector2(0f, 1f);
				this._scrollRect.content.anchorMax = new Vector2(1f, 1f);
				this._scrollRect.content.pivot = new Vector2(0.5f, 1f);
				return;
			}
			this._verticalLayoutGroup.childAlignment = TextAnchor.LowerCenter;
			this._scrollRect.content.anchorMin = new Vector2(0f, 0f);
			this._scrollRect.content.anchorMax = new Vector2(1f, 0f);
			this._scrollRect.content.pivot = new Vector2(0.5f, 0f);
		}

		// Token: 0x06003941 RID: 14657 RVA: 0x0014D37C File Offset: 0x0014B57C
		public void ReloadData()
		{
			this._ClearAllRows();
			int numberOfCellsInTableView = this.DataSource.GetNumberOfCellsInTableView(this);
			this._rowHeights = new List<float>(numberOfCellsInTableView);
			if (this.IsEmpty)
			{
				return;
			}
			for (int i = 0; i < numberOfCellsInTableView; i++)
			{
				int num = this._CellIndexFromRow(i, null);
				float num2 = this.DataSource.GetHeightForCellIndexInTableView(this, num) + this._verticalLayoutGroup.spacing;
				this._rowHeights.Add(num2);
			}
			this._scrollRect.content.sizeDelta = new Vector2(this._scrollRect.content.sizeDelta[0], this._GetCumulativeRowHeight(this._rowHeights.Count - 1) + (float)this._verticalLayoutGroup.padding.vertical);
			this._RecalculateVisibleRows();
			this._requiresReload = false;
		}

		// Token: 0x06003942 RID: 14658 RVA: 0x0014D458 File Offset: 0x0014B658
		public bool InsertCellAtIndex(int index)
		{
			int num = this.DataSource.GetNumberOfCellsInTableView(this) - 1;
			if (index > num || index < 0)
			{
				AsmoLogger.Error("TableView", string.Format("Unable to add the target row: {0} is out of range [0..{1}]", index, num), null);
				return false;
			}
			int num2 = this._RowFromCellIndex(index, null);
			float num3 = this.DataSource.GetHeightForCellIndexInTableView(this, index) + this._verticalLayoutGroup.spacing;
			this._rowHeights.Insert(num2, num3);
			this._scrollRect.content.sizeDelta = new Vector2(this._scrollRect.content.sizeDelta[0], this._GetCumulativeRowHeight(this._rowHeights.Count - 1) + (float)this._verticalLayoutGroup.padding.vertical);
			if (this.HasSelection && index <= this.SelectedCellIndex)
			{
				if (this.OnCellDeselected != null)
				{
					this.OnCellDeselected(this.SelectedCellIndex);
				}
				int selectedCellIndex = this.SelectedCellIndex;
				this.SelectedCellIndex = selectedCellIndex + 1;
				if (this.OnCellSelected != null)
				{
					this.OnCellSelected(this.SelectedCellIndex);
				}
			}
			this._requiresRefresh = true;
			return true;
		}

		// Token: 0x06003943 RID: 14659 RVA: 0x0004CD45 File Offset: 0x0004AF45
		public void InsertCellAtIndexAndFocusOnIt(int index, TableView.CellAnchor cellAnchor, bool animated)
		{
			this.InsertCellAtIndex(index);
			this.FocusOnCellAtIndex(index, cellAnchor, animated);
		}

		// Token: 0x06003944 RID: 14660 RVA: 0x0014D588 File Offset: 0x0014B788
		public bool RemoveCellAtIndex(int index)
		{
			int num = this._rowHeights.Count - 1;
			if (index > num || index < 0)
			{
				AsmoLogger.Error("TableView", string.Format("Unable to remove the target row: {0} is out of range [0..{1}]", index, num), null);
				return false;
			}
			int num2 = this._RowFromCellIndex(index, new int?(this._rowHeights.Count));
			if (this.HasSelection)
			{
				int num3 = num2;
				int? selectedRow = this._SelectedRow;
				if ((num3 == selectedRow.GetValueOrDefault()) & (selectedRow != null))
				{
					if (this.OnCellDeselected != null)
					{
						this.OnCellDeselected(index);
					}
					this._SelectedRow = null;
				}
				else if (index < this.SelectedCellIndex)
				{
					if (this.OnCellDeselected != null)
					{
						this.OnCellDeselected(this.SelectedCellIndex);
					}
					int selectedCellIndex = this.SelectedCellIndex;
					this.SelectedCellIndex = selectedCellIndex - 1;
					if (this.OnCellSelected != null)
					{
						this.OnCellSelected(this.SelectedCellIndex);
					}
				}
			}
			this._rowHeights.RemoveAt(num2);
			this._scrollRect.content.sizeDelta = new Vector2(this._scrollRect.content.sizeDelta[0], this._GetCumulativeRowHeight(this._rowHeights.Count - 1) + (float)this._verticalLayoutGroup.padding.vertical);
			this._requiresRefresh = true;
			return true;
		}

		// Token: 0x06003945 RID: 14661 RVA: 0x0014D6E4 File Offset: 0x0014B8E4
		public TableViewCell GetCellAtIndex(int index)
		{
			return this._GetCellAtRow(this._RowFromCellIndex(index, null));
		}

		// Token: 0x06003946 RID: 14662 RVA: 0x0014D708 File Offset: 0x0014B908
		private TableViewCell _GetCellAtRow(int row)
		{
			TableViewCell tableViewCell = null;
			this._rowToVisibleCells.TryGetValue(row, out tableViewCell);
			return tableViewCell;
		}

		// Token: 0x06003947 RID: 14663 RVA: 0x0014D728 File Offset: 0x0014B928
		private int _GetRowOfCell(TableViewCell cell)
		{
			foreach (KeyValuePair<int, TableViewCell> keyValuePair in this._rowToVisibleCells)
			{
				if (keyValuePair.Value == cell)
				{
					return keyValuePair.Key;
				}
			}
			return -1;
		}

		// Token: 0x06003948 RID: 14664 RVA: 0x0014D790 File Offset: 0x0014B990
		public int GetIndexOfCell(TableViewCell cell)
		{
			int num = this._GetRowOfCell(cell);
			if (num > 0)
			{
				return this._CellIndexFromRow(num, null);
			}
			return -1;
		}

		// Token: 0x06003949 RID: 14665 RVA: 0x0014D7BC File Offset: 0x0014B9BC
		public bool IsCellVisibleAtIndex(int index)
		{
			int num = this._RowFromCellIndex(index, null);
			return this._rowToVisibleCells.ContainsKey(num);
		}

		// Token: 0x0600394A RID: 14666 RVA: 0x0014D7E8 File Offset: 0x0014B9E8
		public void NotifyCellDimensionsChangedAtIndex(int index)
		{
			if (index < 0 || index >= this.DataSource.GetNumberOfCellsInTableView(this))
			{
				AsmoLogger.Error("TableView", string.Format("A cell is notifying that its dimension changed, but the index {0} is out of bounds", index), null);
				return;
			}
			int num = this._RowFromCellIndex(index, null);
			float num2 = this._rowHeights[num];
			float heightForCellIndexInTableView = this.DataSource.GetHeightForCellIndexInTableView(this, index);
			float num3 = heightForCellIndexInTableView + this._verticalLayoutGroup.spacing;
			this._rowHeights[num] = num3;
			TableViewCell tableViewCell = this._GetCellAtRow(num);
			if (tableViewCell != null)
			{
				tableViewCell.GetComponent<LayoutElement>().preferredHeight = heightForCellIndexInTableView;
			}
			float num4 = num3 - num2;
			this._scrollRect.content.sizeDelta = new Vector2(this._scrollRect.content.sizeDelta.x, this._scrollRect.content.sizeDelta.y + num4);
			this._requiresRefresh = true;
		}

		// Token: 0x0600394B RID: 14667 RVA: 0x0014D8DC File Offset: 0x0014BADC
		public void FocusOnCellAtIndex(int index, TableView.CellAnchor cellAnchor, bool animated)
		{
			if (index < 0 || index >= this.DataSource.GetNumberOfCellsInTableView(this))
			{
				AsmoLogger.Error("TableView", string.Format("Unable to focus on the target index: {0} is out of bounds", index), null);
				return;
			}
			int num = this._RowFromCellIndex(index, null);
			float targetY = this._GetScrollYForRow(num, true) + this._verticalLayoutGroup.spacing;
			if (this._focusRoutine != null)
			{
				base.StopCoroutine(this._focusRoutine);
			}
			if (this._scrollPositionSetter != null)
			{
				this._scrollPositionSetter();
			}
			switch (cellAnchor)
			{
			case TableView.CellAnchor.Bottom:
				targetY -= (this._scrollRect.transform as RectTransform).rect.height - this.DataSource.GetHeightForCellIndexInTableView(this, index);
				break;
			case TableView.CellAnchor.Middle:
				targetY -= ((this._scrollRect.transform as RectTransform).rect.height - this.DataSource.GetHeightForCellIndexInTableView(this, index)) / 2f;
				break;
			}
			targetY = Mathf.Clamp(targetY, 0f, this._ScrollableHeight);
			this._scrollPositionSetter = delegate
			{
				this._ScrollY = targetY;
				this._scrollPositionSetter = null;
			};
			if (animated)
			{
				this._focusRoutine = base.StartCoroutine(this._FocusOnRowAnimation(targetY));
				return;
			}
			this._scrollPositionSetter();
		}

		// Token: 0x0600394C RID: 14668 RVA: 0x0004CD58 File Offset: 0x0004AF58
		private IEnumerator _FocusOnRowAnimation(float targetY)
		{
			float startTime = Time.time;
			float endTime = startTime + 1f;
			float StartY = this._ScrollY;
			while (Time.time < endTime)
			{
				float num = Mathf.InverseLerp(startTime, endTime, Time.time);
				this._ScrollY = Mathf.Lerp(StartY, targetY, num);
				yield return new WaitForEndOfFrame();
			}
			this._scrollPositionSetter();
			yield break;
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x0600394D RID: 14669 RVA: 0x0014DA58 File Offset: 0x0014BC58
		private float _ScrollableHeight
		{
			get
			{
				return this._scrollRect.content.rect.height - (base.transform as RectTransform).rect.height;
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x0600394E RID: 14670 RVA: 0x0004CD6E File Offset: 0x0004AF6E
		// (set) Token: 0x0600394F RID: 14671 RVA: 0x0014DA98 File Offset: 0x0014BC98
		private float _ScrollY
		{
			get
			{
				return this._scrollY;
			}
			set
			{
				if (this.IsEmpty)
				{
					return;
				}
				value = Mathf.Clamp(value, 0f, this._GetScrollYForRow(this._rowHeights.Count - 1, true));
				if (this._scrollY != value)
				{
					this._scrollY = value;
					this._requiresRefresh = true;
					float num = value / this._ScrollableHeight;
					this._scrollRect.verticalNormalizedPosition = 1f - num;
				}
			}
		}

		// Token: 0x06003950 RID: 14672 RVA: 0x0014DB04 File Offset: 0x0014BD04
		private float _GetScrollYForRow(int row, bool above = true)
		{
			float num = this._GetCumulativeRowHeight(row);
			num += (float)this._verticalLayoutGroup.padding.top;
			if (above)
			{
				num -= this._rowHeights[row];
			}
			return num;
		}

		// Token: 0x06003951 RID: 14673 RVA: 0x0014DB40 File Offset: 0x0014BD40
		private void _ScrollViewValueChanged(Vector2 newScrollValue)
		{
			float num = (1f - newScrollValue.y) * this._ScrollableHeight;
			if (Mathf.Approximately(num, this._ScrollY))
			{
				return;
			}
			this._ScrollY = num;
			this._requiresRefresh = true;
		}

		// Token: 0x06003952 RID: 14674 RVA: 0x0004CD76 File Offset: 0x0004AF76
		private void _RecalculateVisibleRows()
		{
			this._ClearAllRows();
			this._SetInitialVisibleRows();
		}

		// Token: 0x06003953 RID: 14675 RVA: 0x0004CD84 File Offset: 0x0004AF84
		private void _ClearAllRows()
		{
			while (this._rowToVisibleCells.Count > 0)
			{
				this._HideRow(false);
			}
			this._visibleRowRange = new global::UnityEngine.SocialPlatforms.Range(0, 0);
		}

		// Token: 0x06003954 RID: 14676 RVA: 0x0014DB80 File Offset: 0x0014BD80
		private void Awake()
		{
			this._scrollRect = base.GetComponent<ScrollRect>();
			if (this._scrollRect == null)
			{
				AsmoLogger.Error("TableView", "ScrollRect is null", null);
				return;
			}
			this._verticalLayoutGroup = base.GetComponentInChildren<VerticalLayoutGroup>(true);
			if (this._verticalLayoutGroup == null)
			{
				AsmoLogger.Error("TableView", "VerticalLayoutGroup is null", null);
				return;
			}
			this._UpdateScrollAnchor();
			this._topContentPlaceholder = this._CreateEmptyContentPlaceHolderElement("TopContentPlaceholder");
			this._topContentPlaceholder.transform.SetParent(this._scrollRect.content, false);
			this._bottomContentPlaceholder = this._CreateEmptyContentPlaceHolderElement("BottomContentPlaceholder");
			this._bottomContentPlaceholder.transform.SetParent(this._scrollRect.content, false);
			this._rowToVisibleCells = new Dictionary<int, TableViewCell>();
			this._reusableCellContainer = new GameObject("ReusableCells", new Type[] { typeof(RectTransform) }).GetComponent<RectTransform>();
			this._reusableCellContainer.SetParent(base.transform, false);
			this._reusableCellContainer.gameObject.SetActive(false);
			this._reusableCells = new Dictionary<string, LinkedList<TableViewCell>>();
		}

		// Token: 0x06003955 RID: 14677 RVA: 0x0014DCA8 File Offset: 0x0014BEA8
		private void Update()
		{
			if (this._requiresReload)
			{
				this.ReloadData();
			}
			if (!Mathf.Approximately(this._previousPos.y, this._scrollRect.normalizedPosition.y))
			{
				this._previousPos = this._scrollRect.normalizedPosition;
				if (this._stabilizationRoutine != null)
				{
					base.StopCoroutine(this._stabilizationRoutine);
				}
				this._stabilizationRoutine = base.StartCoroutine(this._WaitForScrollingStabilization());
			}
		}

		// Token: 0x06003956 RID: 14678 RVA: 0x0004CDAA File Offset: 0x0004AFAA
		private IEnumerator _WaitForScrollingStabilization()
		{
			yield return new WaitForSeconds(this._stabilizationDelay);
			if (this.OnScrollOver != null)
			{
				this.OnScrollOver();
			}
			yield break;
		}

		// Token: 0x06003957 RID: 14679 RVA: 0x0004CDB9 File Offset: 0x0004AFB9
		private void LateUpdate()
		{
			if (this._requiresRefresh)
			{
				this._RefreshVisibleRows();
			}
		}

		// Token: 0x06003958 RID: 14680 RVA: 0x0004CDC9 File Offset: 0x0004AFC9
		private void OnEnable()
		{
			this._scrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this._ScrollViewValueChanged));
		}

		// Token: 0x06003959 RID: 14681 RVA: 0x0004CDE7 File Offset: 0x0004AFE7
		private void OnDisable()
		{
			this._scrollRect.onValueChanged.RemoveListener(new UnityAction<Vector2>(this._ScrollViewValueChanged));
		}

		// Token: 0x0600395A RID: 14682 RVA: 0x0014DD1C File Offset: 0x0014BF1C
		private global::UnityEngine.SocialPlatforms.Range _CalculateCurrentVisibleRowRange()
		{
			float num = Math.Max(this._ScrollY - (float)this._verticalLayoutGroup.padding.top, 0f);
			float num2 = Math.Max((float)this._verticalLayoutGroup.padding.top - this._ScrollY, 0f);
			float num3 = num + (base.transform as RectTransform).rect.height - num2;
			int num4 = this._FindIndexOfRowAtY(num);
			int num5 = this._FindIndexOfRowAtY(num3);
			return new global::UnityEngine.SocialPlatforms.Range(num4, num5 - num4 + 1);
		}

		// Token: 0x0600395B RID: 14683 RVA: 0x0014DDAC File Offset: 0x0014BFAC
		private void _SetInitialVisibleRows()
		{
			global::UnityEngine.SocialPlatforms.Range range = this._CalculateCurrentVisibleRowRange();
			for (int i = 0; i < range.count; i++)
			{
				this._AddRow(range.from + i);
			}
			this._visibleRowRange = range;
			this._UpdatePaddingElements();
			this._UpdateCellsSiblingIndices();
		}

		// Token: 0x0600395C RID: 14684 RVA: 0x0014DDF4 File Offset: 0x0014BFF4
		private void _AddRow(int row)
		{
			TableViewCell cellForIndexInTableView = this.DataSource.GetCellForIndexInTableView(this, this._CellIndexFromRow(row, null));
			cellForIndexInTableView.transform.SetParent(this._scrollRect.content, false);
			cellForIndexInTableView.OnSelection = delegate(TableViewCell cell)
			{
				if (this._SelectedRow != null && this.OnCellDeselected != null)
				{
					this.OnCellDeselected(this._CellIndexFromRow(this._SelectedRow.Value, null));
				}
				this._SelectedRow = new int?(this._GetRowOfCell(cell));
				if (this.OnCellSelected != null)
				{
					this.OnCellSelected(this._CellIndexFromRow(this._SelectedRow.Value, null));
				}
			};
			LayoutElement layoutElement = cellForIndexInTableView.GetComponent<LayoutElement>();
			if (layoutElement == null)
			{
				layoutElement = cellForIndexInTableView.gameObject.AddComponent<LayoutElement>();
			}
			layoutElement.preferredHeight = this._rowHeights[row];
			if (row > 0)
			{
				layoutElement.preferredHeight -= this._verticalLayoutGroup.spacing;
			}
			this._rowToVisibleCells[row] = cellForIndexInTableView;
			if (this.OnCellVisibilityChanged != null)
			{
				this.OnCellVisibilityChanged(this._CellIndexFromRow(row, null), true);
			}
		}

		// Token: 0x0600395D RID: 14685 RVA: 0x0014DEC4 File Offset: 0x0014C0C4
		private void _RefreshVisibleRows()
		{
			this._requiresRefresh = false;
			if (this.IsEmpty)
			{
				return;
			}
			global::UnityEngine.SocialPlatforms.Range range = this._CalculateCurrentVisibleRowRange();
			int num = this._visibleRowRange.Last();
			int num2 = range.Last();
			if (range.from > num || num2 < this._visibleRowRange.from)
			{
				this._RecalculateVisibleRows();
				return;
			}
			for (int i = this._visibleRowRange.from; i < range.from; i++)
			{
				this._HideRow(false);
			}
			for (int j = num2; j < num; j++)
			{
				this._HideRow(true);
			}
			for (int k = this._visibleRowRange.from - 1; k >= range.from; k--)
			{
				this._AddRow(k);
			}
			for (int l = num + 1; l <= num2; l++)
			{
				this._AddRow(l);
			}
			this._visibleRowRange = range;
			this._UpdatePaddingElements();
			this._UpdateCellsSiblingIndices();
		}

		// Token: 0x0600395E RID: 14686 RVA: 0x0014DFA8 File Offset: 0x0014C1A8
		private void _UpdatePaddingElements()
		{
			float num = 0f;
			for (int i = 0; i < this._visibleRowRange.from; i++)
			{
				num += this._rowHeights[i];
			}
			this._topContentPlaceholder.preferredHeight = num;
			this._topContentPlaceholder.gameObject.SetActive(this._topContentPlaceholder.preferredHeight > 0f);
			for (int j = this._visibleRowRange.from; j <= this._visibleRowRange.Last(); j++)
			{
				num += this._rowHeights[j];
			}
			float num2 = this._scrollRect.content.rect.height - num;
			num2 -= (float)this._verticalLayoutGroup.padding.top;
			num2 -= (float)this._verticalLayoutGroup.padding.bottom;
			this._bottomContentPlaceholder.preferredHeight = num2 - this._verticalLayoutGroup.spacing;
			this._bottomContentPlaceholder.gameObject.SetActive(this._bottomContentPlaceholder.preferredHeight > 0f);
		}

		// Token: 0x0600395F RID: 14687 RVA: 0x0014E0BC File Offset: 0x0014C2BC
		private void _UpdateCellsSiblingIndices()
		{
			int num = 0;
			this._topContentPlaceholder.transform.SetSiblingIndex(num++);
			for (int i = this._visibleRowRange.from; i <= this._visibleRowRange.Last(); i++)
			{
				this._rowToVisibleCells[i].transform.SetSiblingIndex(num++);
			}
			this._bottomContentPlaceholder.transform.SetSiblingIndex(num++);
		}

		// Token: 0x06003960 RID: 14688 RVA: 0x0014E130 File Offset: 0x0014C330
		private void _HideRow(bool last)
		{
			int num = (last ? this._visibleRowRange.Last() : this._visibleRowRange.from);
			int num2 = this._CellIndexFromRow(num, null);
			TableViewCell tableViewCell = this._rowToVisibleCells[num];
			this._StoreCellForReuse(tableViewCell);
			this._rowToVisibleCells.Remove(num);
			this._visibleRowRange.count = this._visibleRowRange.count - 1;
			if (!last)
			{
				this._visibleRowRange.from = this._visibleRowRange.from + 1;
			}
			if (this.OnCellVisibilityChanged != null)
			{
				this.OnCellVisibilityChanged(num2, false);
			}
		}

		// Token: 0x06003961 RID: 14689 RVA: 0x0004CE05 File Offset: 0x0004B005
		private LayoutElement _CreateEmptyContentPlaceHolderElement(string name)
		{
			return new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(LayoutElement)
			}).GetComponent<LayoutElement>();
		}

		// Token: 0x06003962 RID: 14690 RVA: 0x0004CE32 File Offset: 0x0004B032
		private int _FindIndexOfRowAtY(float y)
		{
			return this._FindIndexOfRowAtY(y, 0, this._rowHeights.Count - 1);
		}

		// Token: 0x06003963 RID: 14691 RVA: 0x0014E1C4 File Offset: 0x0014C3C4
		private int _FindIndexOfRowAtY(float y, int startIndex, int endIndex)
		{
			if (startIndex >= endIndex)
			{
				return startIndex;
			}
			int num = (startIndex + endIndex) / 2;
			if (this._GetCumulativeRowHeight(num) >= y)
			{
				return this._FindIndexOfRowAtY(y, startIndex, num);
			}
			return this._FindIndexOfRowAtY(y, num + 1, endIndex);
		}

		// Token: 0x06003964 RID: 14692 RVA: 0x0014E1FC File Offset: 0x0014C3FC
		private int _CellIndexFromRow(int row, int? rowCount = null)
		{
			if (this.ScrollingStartingOrder == TableView.ScrollAnchor.Bottom)
			{
				return (rowCount ?? this.DataSource.GetNumberOfCellsInTableView(this)) - row - 1;
			}
			return row;
		}

		// Token: 0x06003965 RID: 14693 RVA: 0x0014E1FC File Offset: 0x0014C3FC
		private int _RowFromCellIndex(int index, int? rowCount = null)
		{
			if (this.ScrollingStartingOrder == TableView.ScrollAnchor.Bottom)
			{
				return (rowCount ?? this.DataSource.GetNumberOfCellsInTableView(this)) - index - 1;
			}
			return index;
		}

		// Token: 0x06003966 RID: 14694 RVA: 0x0014E238 File Offset: 0x0014C438
		private float _GetCumulativeRowHeight(int row)
		{
			float num = 0f;
			for (int i = 0; i <= row; i++)
			{
				num += this._rowHeights[i];
			}
			return num;
		}

		// Token: 0x04002B3B RID: 11067
		private const string _documentation = "A reusable table for for (vertical) tables. API inspired by Cocoa's UITableView.\nHierarchy structure should be:\nGameObject + TableView (this) + Mask + Scroll Rect (point to child)\n- Child GameObject + Vertical Layout Group";

		// Token: 0x04002B3C RID: 11068
		private const string _kModuleName = "TableView";

		// Token: 0x04002B41 RID: 11073
		private ITableViewDataSource _dataSource;

		// Token: 0x04002B42 RID: 11074
		private int? _SelectedRow;

		// Token: 0x04002B43 RID: 11075
		[SerializeField]
		private TableView.ScrollAnchor _scrollingStartingOrder;

		// Token: 0x04002B44 RID: 11076
		private Action _scrollPositionSetter;

		// Token: 0x04002B45 RID: 11077
		private float _scrollY;

		// Token: 0x04002B46 RID: 11078
		[SerializeField]
		private float _stabilizationDelay = 0.15f;

		// Token: 0x04002B47 RID: 11079
		private bool _requiresReload;

		// Token: 0x04002B48 RID: 11080
		private VerticalLayoutGroup _verticalLayoutGroup;

		// Token: 0x04002B49 RID: 11081
		private ScrollRect _scrollRect;

		// Token: 0x04002B4A RID: 11082
		private LayoutElement _topContentPlaceholder;

		// Token: 0x04002B4B RID: 11083
		private LayoutElement _bottomContentPlaceholder;

		// Token: 0x04002B4C RID: 11084
		private List<float> _rowHeights;

		// Token: 0x04002B4D RID: 11085
		private Dictionary<int, TableViewCell> _rowToVisibleCells;

		// Token: 0x04002B4E RID: 11086
		private global::UnityEngine.SocialPlatforms.Range _visibleRowRange;

		// Token: 0x04002B4F RID: 11087
		private RectTransform _reusableCellContainer;

		// Token: 0x04002B50 RID: 11088
		private Dictionary<string, LinkedList<TableViewCell>> _reusableCells;

		// Token: 0x04002B51 RID: 11089
		private bool _requiresRefresh;

		// Token: 0x04002B52 RID: 11090
		private Coroutine _focusRoutine;

		// Token: 0x04002B53 RID: 11091
		private Coroutine _stabilizationRoutine;

		// Token: 0x04002B54 RID: 11092
		private Vector2 _previousPos;

		// Token: 0x020007D1 RID: 2001
		public enum ScrollAnchor
		{
			// Token: 0x04002B56 RID: 11094
			Top,
			// Token: 0x04002B57 RID: 11095
			Bottom
		}

		// Token: 0x020007D2 RID: 2002
		public enum CellAnchor
		{
			// Token: 0x04002B59 RID: 11097
			Top,
			// Token: 0x04002B5A RID: 11098
			Bottom,
			// Token: 0x04002B5B RID: 11099
			Middle
		}
	}
}
