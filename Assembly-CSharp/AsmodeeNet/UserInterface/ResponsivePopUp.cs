using System;
using System.Collections;
using AsmodeeNet.Foundation;
using AsmodeeNet.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007C9 RID: 1993
	[RequireComponent(typeof(Canvas))]
	[RequireComponent(typeof(CanvasScaler))]
	public class ResponsivePopUp : MonoBehaviour
	{
		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06003918 RID: 14616 RVA: 0x0014CA70 File Offset: 0x0014AC70
		public float Ratio
		{
			get
			{
				switch (this._prefs.InterfaceDisplayMode)
				{
				case Preferences.DisplayMode.Small:
					return this.smallRatio;
				case Preferences.DisplayMode.Big:
					return this.bigRatio;
				}
				return this.regularRatio;
			}
		}

		// Token: 0x06003919 RID: 14617 RVA: 0x0014CAB4 File Offset: 0x0014ACB4
		private void Awake()
		{
			global::UnityEngine.Object component = base.GetComponent<Canvas>();
			this._canvasScaler = base.GetComponent<CanvasScaler>();
			if (component == null || this._canvasScaler == null)
			{
				AsmoLogger.Error("ResponsivePopUp", "ResponsivePopUp component should be added to the root element of your popup (containing the Canvas and <b>CanvasScaler</b>)", null);
				base.gameObject.SetActive(false);
				return;
			}
			this._root = base.transform as RectTransform;
			this._canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			this._canvasScalerOriginalReferenceResolution = this._canvasScaler.referenceResolution;
			this.container.anchorMin = new Vector2(0.5f, 0.5f);
			this.container.anchorMax = new Vector2(0.5f, 0.5f);
			this.container.localPosition = new Vector3(0f, 0f, 0f);
			this._prefs = CoreApplication.Instance.Preferences;
			if (this.fadeDisplay)
			{
				this._canvasGroup = base.GetComponent<CanvasGroup>();
			}
		}

		// Token: 0x0600391A RID: 14618 RVA: 0x0014CBAC File Offset: 0x0014ADAC
		private void OnEnable()
		{
			this._prefs.AspectDidChange += this.SetNeedsUpdate;
			this._prefs.InterfaceDisplayModeDidChange += this.SetNeedsUpdate;
			this._needsUpdate = true;
			this.Update();
			if (this.autoFadeOnEnable)
			{
				this.FadeIn(null);
			}
		}

		// Token: 0x0600391B RID: 14619 RVA: 0x0004CC28 File Offset: 0x0004AE28
		private void OnDisable()
		{
			if (!CoreApplication.IsQuitting)
			{
				this._prefs.AspectDidChange -= this.SetNeedsUpdate;
				this._prefs.InterfaceDisplayModeDidChange -= this.SetNeedsUpdate;
			}
		}

		// Token: 0x0600391C RID: 14620 RVA: 0x0004CC5F File Offset: 0x0004AE5F
		private void SetNeedsUpdate()
		{
			this._needsUpdate = true;
		}

		// Token: 0x1400014C RID: 332
		// (add) Token: 0x0600391D RID: 14621 RVA: 0x0014CC04 File Offset: 0x0014AE04
		// (remove) Token: 0x0600391E RID: 14622 RVA: 0x0014CC3C File Offset: 0x0014AE3C
		public event Action OnUpdateFinished;

		// Token: 0x0600391F RID: 14623 RVA: 0x0014CC74 File Offset: 0x0014AE74
		private void Update()
		{
			if (!this._needsUpdate)
			{
				return;
			}
			this._needsUpdate = false;
			ResponsivePopUp.ResponsiveSettings responsiveSettings;
			float num;
			switch (this._prefs.InterfaceDisplayMode)
			{
			case Preferences.DisplayMode.Small:
				responsiveSettings = this.smallSettings;
				num = this.smallRatio;
				goto IL_0060;
			case Preferences.DisplayMode.Big:
				responsiveSettings = this.bigSettings;
				num = this.bigRatio;
				goto IL_0060;
			}
			responsiveSettings = this.regularSettings;
			num = this.regularRatio;
			IL_0060:
			if (this.responsiveScope == ResponsivePopUp.ResponsiveScope.Global)
			{
				responsiveSettings = this.globalSettings;
			}
			if (responsiveSettings.strategy == ResponsivePopUp.ResponsiveStrategy.FixedSize)
			{
				this._canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
				Vector2 vector = responsiveSettings.size;
				float num2 = vector.x / vector.y;
				float num3 = Mathf.Max(this._canvasScalerOriginalReferenceResolution.x, this._canvasScalerOriginalReferenceResolution.y);
				vector = ((responsiveSettings.size.x >= responsiveSettings.size.y) ? new Vector2(num3, num3 / num2) : new Vector2(num3 * num2, num3));
				this._canvasScaler.referenceResolution = vector;
				this.container.sizeDelta = responsiveSettings.size;
			}
			else
			{
				this._canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Shrink;
				float num4 = Mathf.Max(this._canvasScalerOriginalReferenceResolution.x, this._canvasScalerOriginalReferenceResolution.y);
				this._canvasScaler.referenceResolution = new Vector2(num4, num4);
				float num5 = ((this._root.sizeDelta.x >= this._root.sizeDelta.y) ? (num4 / this._root.sizeDelta.x) : (num4 / this._root.sizeDelta.y));
				this.container.sizeDelta = this._root.sizeDelta * num5;
			}
			this.container.localScale = new Vector3(num, num, num);
			if (this.OnUpdateFinished != null)
			{
				this.OnUpdateFinished();
			}
		}

		// Token: 0x06003920 RID: 14624 RVA: 0x0004CC68 File Offset: 0x0004AE68
		public void FadeIn(Action completion = null)
		{
			this._Fade(ResponsivePopUp.FadeType.In, completion);
		}

		// Token: 0x06003921 RID: 14625 RVA: 0x0004CC72 File Offset: 0x0004AE72
		public void FadeOut(Action completion = null)
		{
			this._Fade(ResponsivePopUp.FadeType.Out, completion);
		}

		// Token: 0x06003922 RID: 14626 RVA: 0x0014CE5C File Offset: 0x0014B05C
		private void _Fade(ResponsivePopUp.FadeType type, Action completion)
		{
			if (!this.fadeDisplay)
			{
				if (completion != null)
				{
					completion();
				}
				return;
			}
			if (this._canvasGroup == null)
			{
				AsmoLogger.Error("ResponsivePopUp", "Fade In/Out requires a CanvasGroup", null);
			}
			base.StartCoroutine(this._FadeAnimation(type, completion));
		}

		// Token: 0x06003923 RID: 14627 RVA: 0x0004CC7C File Offset: 0x0004AE7C
		private IEnumerator _FadeAnimation(ResponsivePopUp.FadeType type, Action completion)
		{
			if (type == ResponsivePopUp.FadeType.In)
			{
				this._canvasGroup.alpha = 0f;
				float invDuration = 3.3333333f;
				while (this._canvasGroup.alpha < 1f)
				{
					this._canvasGroup.alpha += Time.deltaTime * invDuration;
					yield return null;
				}
			}
			else
			{
				this._canvasGroup.alpha = 1f;
				float invDuration = 3.3333333f;
				while (this._canvasGroup.alpha > 0f)
				{
					this._canvasGroup.alpha -= Time.deltaTime * invDuration;
					yield return null;
				}
			}
			if (completion != null)
			{
				completion();
			}
			yield break;
		}

		// Token: 0x04002B15 RID: 11029
		private const string _documentation = "<b>ResponsivePopUp</b> will handle the basic layout of a popup window according to the <b>DisplayMode</b> [<b>Small</b>|<b>Regular</b>|<b>Big</b>]\nIt should be added to the root element of your popup (containing the <b>Canvas</b> and <b>CanvasScaler</b>)";

		// Token: 0x04002B16 RID: 11030
		public RectTransform background;

		// Token: 0x04002B17 RID: 11031
		public RectTransform container;

		// Token: 0x04002B18 RID: 11032
		private RectTransform _root;

		// Token: 0x04002B19 RID: 11033
		private CanvasGroup _canvasGroup;

		// Token: 0x04002B1A RID: 11034
		private CanvasScaler _canvasScaler;

		// Token: 0x04002B1B RID: 11035
		private Vector2 _canvasScalerOriginalReferenceResolution;

		// Token: 0x04002B1C RID: 11036
		public ResponsivePopUp.ResponsiveScope responsiveScope;

		// Token: 0x04002B1D RID: 11037
		public ResponsivePopUp.ResponsiveSettings globalSettings;

		// Token: 0x04002B1E RID: 11038
		public ResponsivePopUp.ResponsiveSettings smallSettings;

		// Token: 0x04002B1F RID: 11039
		public ResponsivePopUp.ResponsiveSettings regularSettings;

		// Token: 0x04002B20 RID: 11040
		public ResponsivePopUp.ResponsiveSettings bigSettings;

		// Token: 0x04002B21 RID: 11041
		public float smallRatio = 0.9f;

		// Token: 0x04002B22 RID: 11042
		public float regularRatio = 0.6f;

		// Token: 0x04002B23 RID: 11043
		public float bigRatio = 0.4f;

		// Token: 0x04002B24 RID: 11044
		public bool fadeDisplay = true;

		// Token: 0x04002B25 RID: 11045
		public const float fadeDuration = 0.3f;

		// Token: 0x04002B26 RID: 11046
		public bool autoFadeOnEnable = true;

		// Token: 0x04002B27 RID: 11047
		private Preferences _prefs;

		// Token: 0x04002B28 RID: 11048
		private bool _needsUpdate;

		// Token: 0x020007CA RID: 1994
		[Serializable]
		public enum ResponsiveScope
		{
			// Token: 0x04002B2B RID: 11051
			Global,
			// Token: 0x04002B2C RID: 11052
			PerDisplayMode
		}

		// Token: 0x020007CB RID: 1995
		[Serializable]
		public enum ResponsiveStrategy
		{
			// Token: 0x04002B2E RID: 11054
			FixedSize,
			// Token: 0x04002B2F RID: 11055
			FillSpace
		}

		// Token: 0x020007CC RID: 1996
		[Serializable]
		public struct ResponsiveSettings
		{
			// Token: 0x04002B30 RID: 11056
			public ResponsivePopUp.ResponsiveStrategy strategy;

			// Token: 0x04002B31 RID: 11057
			public Vector2 size;
		}

		// Token: 0x020007CD RID: 1997
		private enum FadeType
		{
			// Token: 0x04002B33 RID: 11059
			In,
			// Token: 0x04002B34 RID: 11060
			Out
		}
	}
}
