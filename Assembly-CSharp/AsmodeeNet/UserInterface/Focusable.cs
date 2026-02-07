using System;
using AsmodeeNet.Foundation;
using AsmodeeNet.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007DA RID: 2010
	[RequireComponent(typeof(Selectable))]
	public class Focusable : MonoBehaviour
	{
		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06003986 RID: 14726 RVA: 0x0004CF41 File Offset: 0x0004B141
		// (set) Token: 0x06003987 RID: 14727 RVA: 0x0004CF49 File Offset: 0x0004B149
		public Selectable Selectable { get; private set; }

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06003988 RID: 14728 RVA: 0x0004CF52 File Offset: 0x0004B152
		// (set) Token: 0x06003989 RID: 14729 RVA: 0x0004CF5A File Offset: 0x0004B15A
		public InputField InputField { get; private set; }

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x0600398A RID: 14730 RVA: 0x0004CF63 File Offset: 0x0004B163
		// (set) Token: 0x0600398B RID: 14731 RVA: 0x0004CF6B File Offset: 0x0004B16B
		public TMP_InputField TMP_InputField { get; private set; }

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x0600398C RID: 14732 RVA: 0x0004CF74 File Offset: 0x0004B174
		public bool IsInputField
		{
			get
			{
				return this.InputField != null || this.TMP_InputField != null;
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x0600398D RID: 14733 RVA: 0x0014E470 File Offset: 0x0014C670
		public FocusableLayer FocusableLayer
		{
			get
			{
				Transform transform = base.gameObject.transform;
				while (transform.parent != null)
				{
					FocusableLayer component = transform.parent.GetComponent<FocusableLayer>();
					if (component != null)
					{
						return component;
					}
					transform = transform.parent;
				}
				return CoreApplication.Instance.UINavigationManager.RootFocusableLayer;
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x0600398E RID: 14734 RVA: 0x0014E4C8 File Offset: 0x0014C6C8
		public Vector2 ViewportPosition
		{
			get
			{
				if (this._canvas == null)
				{
					return Camera.main.WorldToViewportPoint(base.transform.position);
				}
				if (this._canvas.renderMode == RenderMode.ScreenSpaceOverlay)
				{
					RectTransform rectTransform = this._canvas.transform as RectTransform;
					Vector2 size = rectTransform.rect.size;
					Vector2 vector = RectTransformUtility.CalculateRelativeRectTransformBounds(rectTransform, base.transform as RectTransform).center;
					return new Vector2(vector.x / size.x + 0.5f, vector.y / size.y + 0.5f);
				}
				return (this._canvas.worldCamera ?? Camera.main).WorldToViewportPoint(base.transform.position);
			}
		}

		// Token: 0x0600398F RID: 14735 RVA: 0x0014E5A0 File Offset: 0x0014C7A0
		private void Start()
		{
			this.Selectable = base.GetComponent<Selectable>();
			if (this.Selectable == null)
			{
				AsmoLogger.Error("Focusable", "Missing Selectable component", null);
			}
			this._canvas = base.GetComponentInParent<Canvas>();
			if (this.next != null)
			{
				this.next.previous = this;
			}
			this.InputField = base.GetComponent<InputField>();
			this.TMP_InputField = base.GetComponent<TMP_InputField>();
		}

		// Token: 0x06003990 RID: 14736 RVA: 0x0004CF92 File Offset: 0x0004B192
		private void OnEnable()
		{
			this.FocusableLayer.RegisterFocusable(this);
		}

		// Token: 0x06003991 RID: 14737 RVA: 0x0004CFA0 File Offset: 0x0004B1A0
		private void OnDisable()
		{
			if (CoreApplication.IsQuitting)
			{
				return;
			}
			this.FocusableLayer.UnRegisterFocusable(this);
		}

		// Token: 0x04002B6E RID: 11118
		private const string _documentation = "<b>Focusable</b> registers a <b>UI.Selectable</b> control with the <b>FocusManager</b>. It will be part of the UI cross-platform navigation system.";

		// Token: 0x04002B6F RID: 11119
		private const string _kModuleName = "Focusable";

		// Token: 0x04002B71 RID: 11121
		[Tooltip("Flag this Focusable as the first element to take focus")]
		public bool firstFocusable;

		// Token: 0x04002B72 RID: 11122
		public Focusable next;

		// Token: 0x04002B73 RID: 11123
		[HideInInspector]
		public Focusable previous;

		// Token: 0x04002B76 RID: 11126
		private Canvas _canvas;
	}
}
