using System;
using System.Collections.Generic;
using System.Linq;
using AsmodeeNet.Foundation;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007C2 RID: 1986
	[RequireComponent(typeof(RectTransform))]
	public class RectTransformModifier : MonoBehaviour
	{
		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06003903 RID: 14595 RVA: 0x0004CAD7 File Offset: 0x0004ACD7
		public bool HasFrameAndAnchors
		{
			get
			{
				return (this.parameters & RectTransformModifier.Parameters.FrameAndAnchors) > (RectTransformModifier.Parameters)0;
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06003904 RID: 14596 RVA: 0x0004CAE4 File Offset: 0x0004ACE4
		public bool HasPivot
		{
			get
			{
				return (this.parameters & RectTransformModifier.Parameters.Pivot) > (RectTransformModifier.Parameters)0;
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06003905 RID: 14597 RVA: 0x0004CAF1 File Offset: 0x0004ACF1
		public bool HasRotation
		{
			get
			{
				return (this.parameters & RectTransformModifier.Parameters.Rotation) > (RectTransformModifier.Parameters)0;
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06003906 RID: 14598 RVA: 0x0004CAFE File Offset: 0x0004ACFE
		public bool HasScale
		{
			get
			{
				return (this.parameters & RectTransformModifier.Parameters.Scale) > (RectTransformModifier.Parameters)0;
			}
		}

		// Token: 0x06003907 RID: 14599 RVA: 0x0004CB0B File Offset: 0x0004AD0B
		private void Start()
		{
			this.aspectSpecifications.Sort((RectTransformModifier.AspectSpecification a, RectTransformModifier.AspectSpecification b) => a.aspect.CompareTo(b.aspect));
		}

		// Token: 0x06003908 RID: 14600 RVA: 0x0004CB37 File Offset: 0x0004AD37
		private void OnEnable()
		{
			CoreApplication.Instance.Preferences.AspectDidChange += this._SetNeedsUpdate;
			CoreApplication.Instance.Preferences.InterfaceDisplayModeDidChange += this._SetNeedsUpdate;
			this._SetNeedsUpdate();
		}

		// Token: 0x06003909 RID: 14601 RVA: 0x0004CB75 File Offset: 0x0004AD75
		private void OnDisable()
		{
			if (CoreApplication.IsQuitting)
			{
				return;
			}
			CoreApplication.Instance.Preferences.AspectDidChange -= this._SetNeedsUpdate;
			CoreApplication.Instance.Preferences.InterfaceDisplayModeDidChange -= this._SetNeedsUpdate;
		}

		// Token: 0x0600390A RID: 14602 RVA: 0x0004CBB5 File Offset: 0x0004ADB5
		private void _SetNeedsUpdate()
		{
			this._needsUpdate = true;
		}

		// Token: 0x0600390B RID: 14603 RVA: 0x0014C4C4 File Offset: 0x0014A6C4
		private void Update()
		{
			if (this.strategy == RectTransformModifier.Strategy.PerAspect && this.reference != null && !Mathf.Approximately(this.reference.rect.y, 0f))
			{
				float num = this.reference.rect.x / this.reference.rect.y;
				if (!Mathf.Approximately(num, this._referenceAspect))
				{
					this._referenceAspect = num;
					this._needsUpdate = true;
				}
			}
			if (this._needsUpdate)
			{
				this._needsUpdate = false;
				if (this.strategy == RectTransformModifier.Strategy.PerAspect)
				{
					this._ApplyPerAspectStrategy();
					return;
				}
				this._ApplyPerDisplayModeStrategy();
			}
		}

		// Token: 0x0600390C RID: 14604 RVA: 0x0014C570 File Offset: 0x0014A770
		private void _ApplyPerAspectStrategy()
		{
			Preferences preferences = CoreApplication.Instance.Preferences;
			float num = ((this.reference != null) ? this._referenceAspect : preferences.Aspect);
			Preferences.DisplayMode interfaceDisplayMode = preferences.InterfaceDisplayMode;
			RectTransform rectTransform = base.transform as RectTransform;
			RectTransformModifier.AspectSpecification aspectSpecification = this.aspectSpecifications.First<RectTransformModifier.AspectSpecification>();
			if (num <= aspectSpecification.aspect)
			{
				RectTransformModifier.RectTransformSpecification rectTransformSpecification = RectTransformModifier._FindRectTransformSpecificationForDisplayMode(aspectSpecification.displayModeSpecifications, interfaceDisplayMode);
				if (this.HasFrameAndAnchors)
				{
					rectTransform.anchorMin = rectTransformSpecification.anchorMin;
					rectTransform.anchorMax = rectTransformSpecification.anchorMax;
					rectTransform.sizeDelta = rectTransformSpecification.sizeDelta;
					rectTransform.localPosition = new Vector3(0f, 0f, rectTransformSpecification.zPosition);
					rectTransform.anchoredPosition = rectTransformSpecification.anchoredPosition;
				}
				if (this.HasPivot)
				{
					rectTransform.pivot = rectTransformSpecification.pivot;
				}
				if (this.HasRotation)
				{
					rectTransform.localEulerAngles = rectTransformSpecification.rotation;
				}
				if (this.HasScale)
				{
					rectTransform.localScale = rectTransformSpecification.scale;
					return;
				}
			}
			else
			{
				RectTransformModifier.AspectSpecification aspectSpecification2 = this.aspectSpecifications.Last<RectTransformModifier.AspectSpecification>();
				if (num >= aspectSpecification2.aspect)
				{
					RectTransformModifier.RectTransformSpecification rectTransformSpecification2 = RectTransformModifier._FindRectTransformSpecificationForDisplayMode(aspectSpecification2.displayModeSpecifications, interfaceDisplayMode);
					if (this.HasFrameAndAnchors)
					{
						rectTransform.anchorMin = rectTransformSpecification2.anchorMin;
						rectTransform.anchorMax = rectTransformSpecification2.anchorMax;
						rectTransform.sizeDelta = rectTransformSpecification2.sizeDelta;
						rectTransform.localPosition = new Vector3(0f, 0f, rectTransformSpecification2.zPosition);
						rectTransform.anchoredPosition = rectTransformSpecification2.anchoredPosition;
					}
					if (this.HasPivot)
					{
						rectTransform.pivot = rectTransformSpecification2.pivot;
					}
					if (this.HasRotation)
					{
						rectTransform.localEulerAngles = rectTransformSpecification2.rotation;
					}
					if (this.HasScale)
					{
						rectTransform.localScale = rectTransformSpecification2.scale;
						return;
					}
				}
				else
				{
					RectTransformModifier.AspectSpecification aspectSpecification4;
					RectTransformModifier.AspectSpecification aspectSpecification3 = (aspectSpecification4 = this.aspectSpecifications.First<RectTransformModifier.AspectSpecification>());
					foreach (RectTransformModifier.AspectSpecification aspectSpecification5 in this.aspectSpecifications)
					{
						if (aspectSpecification5.aspect >= num)
						{
							aspectSpecification3 = aspectSpecification5;
							break;
						}
						aspectSpecification3 = (aspectSpecification4 = aspectSpecification5);
					}
					RectTransformModifier.RectTransformSpecification rectTransformSpecification3 = RectTransformModifier._FindRectTransformSpecificationForDisplayMode(aspectSpecification4.displayModeSpecifications, interfaceDisplayMode);
					RectTransformModifier.RectTransformSpecification rectTransformSpecification4 = RectTransformModifier._FindRectTransformSpecificationForDisplayMode(aspectSpecification3.displayModeSpecifications, interfaceDisplayMode);
					float num2 = (num - aspectSpecification4.aspect) / (aspectSpecification3.aspect - aspectSpecification4.aspect);
					if (this.HasFrameAndAnchors)
					{
						rectTransform.anchorMin = Vector2.Lerp(rectTransformSpecification3.anchorMin, rectTransformSpecification4.anchorMin, num2);
						rectTransform.anchorMax = Vector2.Lerp(rectTransformSpecification3.anchorMax, rectTransformSpecification4.anchorMax, num2);
						rectTransform.sizeDelta = Vector2.Lerp(rectTransformSpecification3.sizeDelta, rectTransformSpecification4.sizeDelta, num2);
						rectTransform.localPosition = new Vector3(0f, 0f, Mathf.Lerp(rectTransformSpecification3.zPosition, rectTransformSpecification4.zPosition, num2));
						rectTransform.anchoredPosition = Vector2.Lerp(rectTransformSpecification3.anchoredPosition, rectTransformSpecification4.anchoredPosition, num2);
					}
					if (this.HasPivot)
					{
						rectTransform.pivot = Vector2.Lerp(rectTransformSpecification3.pivot, rectTransformSpecification4.pivot, num2);
					}
					if (this.HasRotation)
					{
						rectTransform.localEulerAngles = Vector3.Lerp(rectTransformSpecification3.rotation, rectTransformSpecification4.rotation, num2);
					}
					if (this.HasScale)
					{
						rectTransform.localScale = Vector3.Lerp(rectTransformSpecification3.scale, rectTransformSpecification4.scale, num2);
					}
				}
			}
		}

		// Token: 0x0600390D RID: 14605 RVA: 0x0014C8EC File Offset: 0x0014AAEC
		private void _ApplyPerDisplayModeStrategy()
		{
			Preferences.DisplayMode interfaceDisplayMode = CoreApplication.Instance.Preferences.InterfaceDisplayMode;
			RectTransform rectTransform = base.transform as RectTransform;
			RectTransformModifier.RectTransformSpecification rectTransformSpecification = RectTransformModifier._FindRectTransformSpecificationForDisplayMode(this.displayModeSpecifications, interfaceDisplayMode);
			if (this.HasFrameAndAnchors)
			{
				rectTransform.anchorMin = rectTransformSpecification.anchorMin;
				rectTransform.anchorMax = rectTransformSpecification.anchorMax;
				rectTransform.sizeDelta = rectTransformSpecification.sizeDelta;
				rectTransform.localPosition = new Vector3(0f, 0f, rectTransformSpecification.zPosition);
				rectTransform.anchoredPosition = rectTransformSpecification.anchoredPosition;
			}
			if (this.HasPivot)
			{
				rectTransform.pivot = rectTransformSpecification.pivot;
			}
			if (this.HasRotation)
			{
				rectTransform.localEulerAngles = rectTransformSpecification.rotation;
			}
			if (this.HasScale)
			{
				rectTransform.localScale = rectTransformSpecification.scale;
			}
		}

		// Token: 0x0600390E RID: 14606 RVA: 0x0014C9B4 File Offset: 0x0014ABB4
		private static RectTransformModifier.RectTransformSpecification _FindRectTransformSpecificationForDisplayMode(List<RectTransformModifier.DisplayModeSpecification> displayModeSpecifications, Preferences.DisplayMode displayMode)
		{
			RectTransformModifier.RectTransformSpecification rectTransformSpecification = null;
			foreach (RectTransformModifier.DisplayModeSpecification displayModeSpecification in displayModeSpecifications)
			{
				if (displayModeSpecification.displayMode == Preferences.DisplayMode.Unknown)
				{
					rectTransformSpecification = displayModeSpecification.specification;
				}
				else if (displayModeSpecification.displayMode == displayMode)
				{
					return displayModeSpecification.specification;
				}
			}
			return rectTransformSpecification;
		}

		// Token: 0x0600390F RID: 14607 RVA: 0x0004CBBE File Offset: 0x0004ADBE
		private void OnDrawGizmosSelected()
		{
			bool isPlaying = Application.isPlaying;
		}

		// Token: 0x06003910 RID: 14608 RVA: 0x0014CA24 File Offset: 0x0014AC24
		private Color _GizmosColorForDisplayMode(Preferences.DisplayMode? displayMode)
		{
			if (displayMode != null)
			{
				switch (displayMode.Value)
				{
				case Preferences.DisplayMode.Small:
					return Color.green;
				case Preferences.DisplayMode.Regular:
					return Color.cyan;
				case Preferences.DisplayMode.Big:
					return Color.blue;
				}
			}
			return Color.gray;
		}

		// Token: 0x04002AF7 RID: 10999
		private const string _documentation = "<b>RectTransformModifier</b> automatically updates a <b>RectTransform</b> according to the current aspect ratio and interface <b>DisplayMode</b>";

		// Token: 0x04002AF8 RID: 11000
		public RectTransformModifier.Parameters parameters;

		// Token: 0x04002AF9 RID: 11001
		public RectTransformModifier.Strategy strategy = RectTransformModifier.Strategy.PerDisplayMode;

		// Token: 0x04002AFA RID: 11002
		public RectTransform reference;

		// Token: 0x04002AFB RID: 11003
		private float _referenceAspect;

		// Token: 0x04002AFC RID: 11004
		public List<RectTransformModifier.AspectSpecification> aspectSpecifications = new List<RectTransformModifier.AspectSpecification>();

		// Token: 0x04002AFD RID: 11005
		public List<RectTransformModifier.DisplayModeSpecification> displayModeSpecifications = new List<RectTransformModifier.DisplayModeSpecification>();

		// Token: 0x04002AFE RID: 11006
		private bool _needsUpdate;

		// Token: 0x020007C3 RID: 1987
		[Flags]
		public enum Parameters
		{
			// Token: 0x04002B00 RID: 11008
			FrameAndAnchors = 1,
			// Token: 0x04002B01 RID: 11009
			Pivot = 2,
			// Token: 0x04002B02 RID: 11010
			Rotation = 4,
			// Token: 0x04002B03 RID: 11011
			Scale = 8
		}

		// Token: 0x020007C4 RID: 1988
		public enum Strategy
		{
			// Token: 0x04002B05 RID: 11013
			PerAspect,
			// Token: 0x04002B06 RID: 11014
			PerDisplayMode
		}

		// Token: 0x020007C5 RID: 1989
		[Serializable]
		public class AspectSpecification
		{
			// Token: 0x04002B07 RID: 11015
			public float aspect = 1f;

			// Token: 0x04002B08 RID: 11016
			public List<RectTransformModifier.DisplayModeSpecification> displayModeSpecifications = new List<RectTransformModifier.DisplayModeSpecification>();
		}

		// Token: 0x020007C6 RID: 1990
		[Serializable]
		public class DisplayModeSpecification
		{
			// Token: 0x04002B09 RID: 11017
			public Preferences.DisplayMode displayMode;

			// Token: 0x04002B0A RID: 11018
			public RectTransformModifier.RectTransformSpecification specification;
		}

		// Token: 0x020007C7 RID: 1991
		[Serializable]
		public class RectTransformSpecification
		{
			// Token: 0x04002B0B RID: 11019
			public Vector2 anchoredPosition;

			// Token: 0x04002B0C RID: 11020
			public Vector2 sizeDelta;

			// Token: 0x04002B0D RID: 11021
			public float zPosition;

			// Token: 0x04002B0E RID: 11022
			public Vector2 anchorMin;

			// Token: 0x04002B0F RID: 11023
			public Vector2 anchorMax;

			// Token: 0x04002B10 RID: 11024
			public Vector2 pivot;

			// Token: 0x04002B11 RID: 11025
			public Vector3 rotation;

			// Token: 0x04002B12 RID: 11026
			public Vector3 scale;
		}
	}
}
