using System;
using System.Collections.Generic;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using Scythe.Utilities;
using UnityEngine;

namespace Scythe.UI.Tutorial
{
	// Token: 0x02000500 RID: 1280
	public class HighlightController : SingletonMono<HighlightController>
	{
		// Token: 0x060028F3 RID: 10483 RVA: 0x00042A08 File Offset: 0x00040C08
		public void HighlightUI(GameObject highlightedGameObject, HighlightController.HighlightStyle highlightStyle = HighlightController.HighlightStyle.Rectangle)
		{
			this.HighlightUI(highlightedGameObject.transform as RectTransform, highlightStyle);
		}

		// Token: 0x060028F4 RID: 10484 RVA: 0x00042A1C File Offset: 0x00040C1C
		public void HighlightUI(Component highlightedComponent, HighlightController.HighlightStyle highlightStyle = HighlightController.HighlightStyle.Rectangle)
		{
			this.HighlightUI(highlightedComponent.transform as RectTransform, highlightStyle);
		}

		// Token: 0x060028F5 RID: 10485 RVA: 0x000EBA04 File Offset: 0x000E9C04
		public void HighlightUnitsArrow(params Unit[] highlightedUnits)
		{
			foreach (Unit unit in highlightedUnits)
			{
				this.HighlightUnitsArrow(GameController.GetUnitPresenter(unit));
			}
		}

		// Token: 0x060028F6 RID: 10486 RVA: 0x000EBA34 File Offset: 0x000E9C34
		public void HighlightHexesArrow(params GameHex[] highlightedHexes)
		{
			foreach (GameHex gameHex in highlightedHexes)
			{
				this.HighlightHexArrow(GameController.Instance.GetGameHexPresenter(gameHex));
			}
		}

		// Token: 0x060028F7 RID: 10487 RVA: 0x00042A30 File Offset: 0x00040C30
		public void Reset()
		{
			this.ResetHighlightedUI();
			this.ResetHighlightedHexArrows();
		}

		// Token: 0x060028F8 RID: 10488 RVA: 0x00042A3E File Offset: 0x00040C3E
		private void HighlightUI(RectTransform highlightedRectTransform, HighlightController.HighlightStyle highlightStyle)
		{
			this.highlightedUI.Add(global::UnityEngine.Object.Instantiate<RectTransform>(this.highlightedUIPrefabs[(int)highlightStyle], highlightedRectTransform, false));
		}

		// Token: 0x060028F9 RID: 10489 RVA: 0x000EBA68 File Offset: 0x000E9C68
		private void HighlightUnitsArrow(UnitPresenter highlightedUnitPresenter)
		{
			RectTransform rectTransform = global::UnityEngine.Object.Instantiate<RectTransform>(this.highlightedArrowPrefab, highlightedUnitPresenter.transform, false);
			rectTransform.SetGlobalScale(Vector3.one);
			Vector3 position = rectTransform.position;
			position.y += 1f;
			rectTransform.position = position;
			rectTransform.rotation = Quaternion.identity;
			this.highlightedArrows.Add(rectTransform);
		}

		// Token: 0x060028FA RID: 10490 RVA: 0x000EBAC8 File Offset: 0x000E9CC8
		private void HighlightHexArrow(GameHexPresenter highlightedHexPresenter)
		{
			FlatGameHexPresenter flatGameHexPresenter = (FlatGameHexPresenter)highlightedHexPresenter;
			RectTransform rectTransform = global::UnityEngine.Object.Instantiate<RectTransform>(this.highlightedArrowPrefab, flatGameHexPresenter.hexObject.transform, false);
			rectTransform.SetGlobalScale(Vector3.one);
			rectTransform.rotation = Quaternion.identity;
			this.highlightedArrows.Add(rectTransform);
		}

		// Token: 0x060028FB RID: 10491 RVA: 0x000EBB18 File Offset: 0x000E9D18
		private void ResetHighlightedUI()
		{
			for (int i = 0; i < this.highlightedUI.Count; i++)
			{
				global::UnityEngine.Object.Destroy(this.highlightedUI[i].gameObject);
			}
			this.highlightedUI.Clear();
		}

		// Token: 0x060028FC RID: 10492 RVA: 0x000EBB5C File Offset: 0x000E9D5C
		private void ResetHighlightedHexArrows()
		{
			for (int i = 0; i < this.highlightedArrows.Count; i++)
			{
				global::UnityEngine.Object.Destroy(this.highlightedArrows[i].gameObject);
			}
			this.highlightedArrows.Clear();
		}

		// Token: 0x04001D5A RID: 7514
		[SerializeField]
		private RectTransform[] highlightedUIPrefabs;

		// Token: 0x04001D5B RID: 7515
		[SerializeField]
		private RectTransform highlightedArrowPrefab;

		// Token: 0x04001D5C RID: 7516
		private List<RectTransform> highlightedUI = new List<RectTransform>();

		// Token: 0x04001D5D RID: 7517
		private List<RectTransform> highlightedArrows = new List<RectTransform>();

		// Token: 0x02000501 RID: 1281
		public enum HighlightStyle
		{
			// Token: 0x04001D5F RID: 7519
			Rectangle,
			// Token: 0x04001D60 RID: 7520
			Circle,
			// Token: 0x04001D61 RID: 7521
			CircleBig
		}
	}
}
