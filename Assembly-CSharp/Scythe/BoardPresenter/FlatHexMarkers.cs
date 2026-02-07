using System;
using System.Collections;
using DG.Tweening;
using HoneyFramework;
using UnityEngine;

namespace Scythe.BoardPresenter
{
	// Token: 0x020001D7 RID: 471
	public class FlatHexMarkers : MonoBehaviour
	{
		// Token: 0x06000DB5 RID: 3509 RVA: 0x00031099 File Offset: 0x0002F299
		private void Awake()
		{
			if (FlatHexMarkers.Instance == null)
			{
				FlatHexMarkers.Instance = this;
			}
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x000310AE File Offset: 0x0002F2AE
		private void Update()
		{
			if (this.flashing)
			{
				this.ChangeAlphaOfTheMarkers();
			}
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x000870EC File Offset: 0x000852EC
		public static void SetMarkerType(GameObject layerGroup, HexMarkers.MarkerType type)
		{
			if (FlatHexMarkers.Instance != null)
			{
				switch (type)
				{
				case HexMarkers.MarkerType.Move:
				case HexMarkers.MarkerType.MoveToEnemy:
				case HexMarkers.MarkerType.HexBorder:
				case HexMarkers.MarkerType.MoveToEncounter:
					FlatHexMarkers.Instance.GetSpriteForLayer(layerGroup, HexMarkers.Layer.Action).color = FlatHexMarkers.Instance.whiteDistanceRange;
					FlatHexMarkers.Instance.SetMarkerType(layerGroup, type, HexMarkers.Layer.Action, 0f);
					return;
				case HexMarkers.MarkerType.DistanceRange:
					FlatHexMarkers.Instance.GetSpriteForLayer(layerGroup, HexMarkers.Layer.Hoover).color = FlatHexMarkers.Instance.whiteDistanceRangeHalfAlpha;
					FlatHexMarkers.Instance.SetMarkerType(layerGroup, type, HexMarkers.Layer.Hoover, 0f);
					return;
				case HexMarkers.MarkerType.RetreatWithoutWorkers:
					if (PlatformManager.IsMobile)
					{
						FlatHexMarkers.Instance.SetMarkerType(layerGroup, type, HexMarkers.Layer.Action, 0f);
						return;
					}
					FlatHexMarkers.Instance.SetMarkerType(layerGroup, type, HexMarkers.Layer.Action, 0f);
					return;
				case HexMarkers.MarkerType.PayResource:
				case HexMarkers.MarkerType.DeployTrade:
					if (PlatformManager.IsMobile)
					{
						FlatHexMarkers.Instance.SetMarkerType(layerGroup, HexMarkers.MarkerType.DeployTrade, HexMarkers.Layer.Action, 0f);
						return;
					}
					FlatHexMarkers.Instance.SetMarkerType(layerGroup, type, HexMarkers.Layer.Action, 0f);
					return;
				case HexMarkers.MarkerType.Battle:
					FlatHexMarkers.Instance.SetMarkerType(layerGroup, type, HexMarkers.Layer.Conflict, 0f);
					return;
				case HexMarkers.MarkerType.OwnerPolania:
				case HexMarkers.MarkerType.OwnerCrimea:
				case HexMarkers.MarkerType.OwnerNords:
				case HexMarkers.MarkerType.OwnerSaxony:
				case HexMarkers.MarkerType.OwnerRusviet:
				case HexMarkers.MarkerType.OwnerAlbion:
				case HexMarkers.MarkerType.OwnerTogawa:
				case HexMarkers.MarkerType.OwnerNone:
					FlatHexMarkers.Instance.SetMarkerType(layerGroup, type, HexMarkers.Layer.Owner, 0f);
					break;
				case HexMarkers.MarkerType.Hoover:
					FlatHexMarkers.Instance.SetMarkerType(layerGroup, type, HexMarkers.Layer.Hoover, 0f);
					return;
				case HexMarkers.MarkerType.FieldSelected:
					FlatHexMarkers.Instance.SetMarkerType(layerGroup, type, HexMarkers.Layer.Hoover, 0f);
					return;
				case HexMarkers.MarkerType.Capital:
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x000310BE File Offset: 0x0002F2BE
		public static void SetMarkerType(GameObject layerGroup, int type, HexMarkers.Layer layer, float rotation)
		{
			if (FlatHexMarkers.Instance != null)
			{
				FlatHexMarkers.Instance.SetMarkerType(layerGroup, layer, rotation, type);
			}
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x000310DB File Offset: 0x0002F2DB
		public static void ClearMarkerLayer(GameObject layerGroup, HexMarkers.Layer layer)
		{
			if (FlatHexMarkers.Instance != null)
			{
				FlatHexMarkers.Instance.SetMarkerType(layerGroup, HexMarkers.MarkerType.None, layer, 0f);
			}
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x00087264 File Offset: 0x00085464
		public static void ClearMarkerType(GameObject layerGroup, HexMarkers.MarkerType type)
		{
			if (FlatHexMarkers.Instance != null)
			{
				switch (type)
				{
				case HexMarkers.MarkerType.Move:
				case HexMarkers.MarkerType.MoveToEnemy:
				case HexMarkers.MarkerType.HexBorder:
				case HexMarkers.MarkerType.MoveToEncounter:
					FlatHexMarkers.Instance.GetSpriteForLayer(layerGroup, HexMarkers.Layer.Action).color = FlatHexMarkers.Instance.whiteDistanceRange;
					FlatHexMarkers.Instance.SetMarkerType(layerGroup, HexMarkers.MarkerType.None, HexMarkers.Layer.Action, 0f);
					return;
				case HexMarkers.MarkerType.DistanceRange:
					FlatHexMarkers.Instance.GetSpriteForLayer(layerGroup, HexMarkers.Layer.Hoover).color = FlatHexMarkers.Instance.whiteDistanceRange;
					FlatHexMarkers.Instance.SetMarkerType(layerGroup, HexMarkers.MarkerType.None, HexMarkers.Layer.Hoover, 0f);
					return;
				case HexMarkers.MarkerType.RetreatWithoutWorkers:
				case HexMarkers.MarkerType.PayResource:
				case HexMarkers.MarkerType.DeployTrade:
					FlatHexMarkers.Instance.SetMarkerType(layerGroup, HexMarkers.MarkerType.None, HexMarkers.Layer.Action, 0f);
					return;
				case HexMarkers.MarkerType.Battle:
					FlatHexMarkers.Instance.SetMarkerType(layerGroup, HexMarkers.MarkerType.None, HexMarkers.Layer.Conflict, 0f);
					return;
				case HexMarkers.MarkerType.OwnerPolania:
				case HexMarkers.MarkerType.OwnerCrimea:
				case HexMarkers.MarkerType.OwnerNords:
				case HexMarkers.MarkerType.OwnerSaxony:
				case HexMarkers.MarkerType.OwnerRusviet:
				case HexMarkers.MarkerType.OwnerAlbion:
				case HexMarkers.MarkerType.OwnerTogawa:
				case HexMarkers.MarkerType.OwnerNone:
					FlatHexMarkers.Instance.SetMarkerType(layerGroup, HexMarkers.MarkerType.None, HexMarkers.Layer.Owner, 0f);
					break;
				case HexMarkers.MarkerType.Hoover:
					FlatHexMarkers.Instance.SetMarkerType(layerGroup, HexMarkers.MarkerType.None, HexMarkers.Layer.Hoover, 0f);
					return;
				case HexMarkers.MarkerType.FieldSelected:
					FlatHexMarkers.Instance.SetMarkerType(layerGroup, HexMarkers.MarkerType.None, HexMarkers.Layer.Hoover, 0f);
					return;
				case HexMarkers.MarkerType.Capital:
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x000310FC File Offset: 0x0002F2FC
		public void SetMarkerType(GameObject layerGroup, HexMarkers.MarkerType type, HexMarkers.Layer layer, float markerRotation)
		{
			this.SetMarkerType(layerGroup, layer, markerRotation, (int)type);
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x00087394 File Offset: 0x00085594
		public void SetMarkerType(GameObject layerGroup, HexMarkers.Layer layer, float markerRotation, int iType)
		{
			int num = iType;
			if (layer == HexMarkers.Layer.Owner && iType == 18)
			{
				num = 13;
			}
			this.GetSpriteForLayer(layerGroup, layer).sprite = this.markers[num];
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x00031109 File Offset: 0x0002F309
		private SpriteRenderer GetSpriteForLayer(GameObject layerGroup, HexMarkers.Layer layer)
		{
			return layerGroup.transform.Find(layer.ToString()).gameObject.GetComponent<SpriteRenderer>();
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x0003112D File Offset: 0x0002F32D
		public void FadeOutAnimation(GameObject layerGroup, HexMarkers.MarkerType type, float animationTime)
		{
			if (type == HexMarkers.MarkerType.DeployTrade || type == HexMarkers.MarkerType.PayResource || type == HexMarkers.MarkerType.RetreatWithoutWorkers)
			{
				this.flashing = false;
				FlatHexMarkers.ClearMarkerType(layerGroup, type);
				return;
			}
			this.FadeOut(layerGroup, type, animationTime);
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x00031153 File Offset: 0x0002F353
		public void FadeInAnimation(GameObject layerGroup, HexMarkers.MarkerType type, float animationTime)
		{
			if (type == HexMarkers.MarkerType.DeployTrade || type == HexMarkers.MarkerType.PayResource || type == HexMarkers.MarkerType.RetreatWithoutWorkers)
			{
				this.flashing = true;
				FlatHexMarkers.SetMarkerType(layerGroup, type);
				base.StartCoroutine(this.FlashingAnimation(layerGroup, type, animationTime));
				return;
			}
			this.FadeIn(layerGroup, type, animationTime);
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x000873C8 File Offset: 0x000855C8
		private void FadeOut(GameObject layerGroup, HexMarkers.MarkerType type, float animationTime)
		{
			string layerName = this.GetLayerForFade(type);
			if (PlatformManager.IsMobile)
			{
				layerGroup.transform.Find(layerName).GetComponent<SpriteRenderer>().DOComplete(true);
			}
			layerGroup.transform.Find(layerName).GetComponent<SpriteRenderer>().DOFade(0f, animationTime)
				.SetEase(this.fadeOutAnimationCurve)
				.OnComplete(delegate
				{
					this.ResetOpacity(layerGroup, type, layerGroup.transform.Find(layerName).GetComponent<SpriteRenderer>().color.a);
				});
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x00087470 File Offset: 0x00085670
		private void FadeIn(GameObject layerGroup, HexMarkers.MarkerType type, float animationTime)
		{
			string layerForFade = this.GetLayerForFade(type);
			if (PlatformManager.IsMobile)
			{
				layerGroup.transform.Find(layerForFade).GetComponent<SpriteRenderer>().DOComplete(true);
			}
			else
			{
				layerGroup.transform.Find(layerForFade).GetComponent<SpriteRenderer>().DOKill(true);
			}
			layerGroup.transform.Find(layerForFade).transform.localScale = Vector3.zero;
			FlatHexMarkers.SetMarkerType(layerGroup, type);
			layerGroup.transform.Find(layerForFade).transform.DOScale(1f, animationTime).SetEase(this.fadeInAnimationCurve);
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x00087508 File Offset: 0x00085708
		private void ResetScale(GameObject layerGroup, HexMarkers.MarkerType type)
		{
			string layerForFade = this.GetLayerForFade(type);
			FlatHexMarkers.ClearMarkerType(layerGroup, type);
			layerGroup.transform.Find(layerForFade).transform.localScale = Vector3.one;
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x00087540 File Offset: 0x00085740
		private void ResetOpacity(GameObject layerGroup, HexMarkers.MarkerType type, float startingAlpha)
		{
			string layerForFade = this.GetLayerForFade(type);
			FlatHexMarkers.ClearMarkerType(layerGroup, type);
			Color color = layerGroup.transform.Find(layerForFade).GetComponent<SpriteRenderer>().color;
			color.a = startingAlpha;
			layerGroup.transform.Find(layerForFade).GetComponent<SpriteRenderer>().color = color;
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x00031189 File Offset: 0x0002F389
		private string GetLayerForFade(HexMarkers.MarkerType type)
		{
			if (type == HexMarkers.MarkerType.Move || type == HexMarkers.MarkerType.MoveToEnemy || type == HexMarkers.MarkerType.MoveToEncounter)
			{
				return "Action";
			}
			return "Hoover";
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x000311A3 File Offset: 0x0002F3A3
		private IEnumerator FlashingAnimation(GameObject layerGroup, HexMarkers.MarkerType type, float animationTime)
		{
			string text = "Action";
			SpriteRenderer spriteRenderer = layerGroup.transform.Find(text).GetComponent<SpriteRenderer>();
			if (PlatformManager.IsMobile)
			{
				spriteRenderer.DOComplete(true);
			}
			Color color = spriteRenderer.color;
			while (this.flashing)
			{
				color = spriteRenderer.color;
				color.a = this.alphaCurrent;
				spriteRenderer.color = color;
				yield return null;
			}
			color = spriteRenderer.color;
			color.a = this.alphaValueMax;
			spriteRenderer.color = color;
			yield break;
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x00087594 File Offset: 0x00085794
		private void ChangeAlphaOfTheMarkers()
		{
			if (this.disappearing)
			{
				this.alphaCurrent -= this.flashingTime * Time.deltaTime;
				if (this.alphaCurrent <= this.alphaValueMin)
				{
					this.alphaCurrent = this.alphaValueMin;
					this.disappearing = false;
					return;
				}
			}
			else
			{
				this.alphaCurrent += this.flashingTime * Time.deltaTime;
				if (this.alphaCurrent >= this.alphaValueMax)
				{
					this.alphaCurrent = this.alphaValueMax;
					this.disappearing = true;
				}
			}
		}

		// Token: 0x04000AD3 RID: 2771
		public static FlatHexMarkers Instance;

		// Token: 0x04000AD4 RID: 2772
		public Sprite[] markers;

		// Token: 0x04000AD5 RID: 2773
		[Range(0.1f, 1f)]
		public float flashingTime = 0.3f;

		// Token: 0x04000AD6 RID: 2774
		private float alphaValueMax = 1f;

		// Token: 0x04000AD7 RID: 2775
		private float alphaCurrent = 1f;

		// Token: 0x04000AD8 RID: 2776
		[Range(0f, 1f)]
		public float alphaValueMin = 0.5f;

		// Token: 0x04000AD9 RID: 2777
		private bool disappearing = true;

		// Token: 0x04000ADA RID: 2778
		private bool flashing;

		// Token: 0x04000ADB RID: 2779
		[SerializeField]
		private AnimationCurve fadeInAnimationCurve;

		// Token: 0x04000ADC RID: 2780
		[SerializeField]
		private AnimationCurve fadeOutAnimationCurve;

		// Token: 0x04000ADD RID: 2781
		[SerializeField]
		private Color colorDistanceRange = new Color32(136, 136, 136, 136);

		// Token: 0x04000ADE RID: 2782
		private Color whiteDistanceRange = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x04000ADF RID: 2783
		private Color whiteDistanceRangeHalfAlpha = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 136);
	}
}
