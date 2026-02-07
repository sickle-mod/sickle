using System;
using UnityEngine;

namespace HoneyFramework
{
	// Token: 0x020001BD RID: 445
	public class HexMarkers : MonoBehaviour
	{
		// Token: 0x06000D1D RID: 3357 RVA: 0x00084DFC File Offset: 0x00082FFC
		private void Awake()
		{
			HexMarkers.instance = this;
			this.hexData = new Texture2D(HexMarkers.textureMapSize * HexMarkers.dataSize, HexMarkers.textureMapSize * HexMarkers.dataSize, TextureFormat.ARGB32, false, false);
			this.hexData.filterMode = FilterMode.Point;
			this.colorData = new Color32[HexMarkers.textureMapSize * HexMarkers.textureMapSize * HexMarkers.dataSize * HexMarkers.dataSize];
			this.hexData.SetPixels32(this.colorData);
			this.hexData.Apply();
			this.dirty = false;
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x00030963 File Offset: 0x0002EB63
		public static Texture2D GetMarkersTexture()
		{
			return HexMarkers.instance.markersTexture;
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0003096F File Offset: 0x0002EB6F
		public static Texture2D GetHexDataTexture()
		{
			return HexMarkers.instance.hexData;
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0003097B File Offset: 0x0002EB7B
		public static Vector4 GetMarkersSettings()
		{
			return new Vector4(8f, 8f, (float)HexMarkers.textureMapSize, (float)HexMarkers.dataSize);
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x00030998 File Offset: 0x0002EB98
		public static void MarkAsDirtyDataTexture()
		{
			if (HexMarkers.instance != null)
			{
				HexMarkers.instance.dirty = true;
			}
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x00084E84 File Offset: 0x00083084
		public static void ClearAllMarkers()
		{
			if (HexMarkers.instance != null)
			{
				for (int i = 0; i < HexMarkers.instance.colorData.Length; i++)
				{
					HexMarkers.instance.colorData[i] = Color.black;
				}
				HexMarkers.instance.hexData.SetPixels32(HexMarkers.instance.colorData);
				HexMarkers.instance.dirty = true;
			}
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x00084EF4 File Offset: 0x000830F4
		public static void SetMarkerType(Vector3i position, HexMarkers.MarkerType type)
		{
			if (HexMarkers.instance != null)
			{
				switch (type)
				{
				case HexMarkers.MarkerType.Move:
				case HexMarkers.MarkerType.DistanceRange:
				case HexMarkers.MarkerType.RetreatWithoutWorkers:
				case HexMarkers.MarkerType.HexBorder:
				case HexMarkers.MarkerType.MoveToEncounter:
					HexMarkers.instance.SetMarkerType(position, type, HexMarkers.Layer.Action, 0f);
					return;
				case HexMarkers.MarkerType.MoveToEnemy:
				case HexMarkers.MarkerType.Battle:
					HexMarkers.instance.SetMarkerType(position, type, HexMarkers.Layer.Conflict, 0f);
					return;
				case HexMarkers.MarkerType.PayResource:
				case HexMarkers.MarkerType.DeployTrade:
					if (PlatformManager.IsMobile)
					{
						HexMarkers.instance.SetMarkerType(position, HexMarkers.MarkerType.DeployTrade, HexMarkers.Layer.Action, 0f);
						return;
					}
					HexMarkers.instance.SetMarkerType(position, type, HexMarkers.Layer.Action, 0f);
					return;
				case HexMarkers.MarkerType.OwnerPolania:
				case HexMarkers.MarkerType.OwnerCrimea:
				case HexMarkers.MarkerType.OwnerNords:
				case HexMarkers.MarkerType.OwnerSaxony:
				case HexMarkers.MarkerType.OwnerRusviet:
				case HexMarkers.MarkerType.OwnerAlbion:
				case HexMarkers.MarkerType.OwnerTogawa:
				case HexMarkers.MarkerType.OwnerNone:
					HexMarkers.instance.SetMarkerType(position, type, HexMarkers.Layer.Owner, 0f);
					break;
				case HexMarkers.MarkerType.Hoover:
				case HexMarkers.MarkerType.FieldSelected:
				case HexMarkers.MarkerType.Capital:
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x000309B2 File Offset: 0x0002EBB2
		public static void SetMarkerType(Vector3i position, int type, HexMarkers.Layer layer, float rotation)
		{
			if (HexMarkers.instance != null)
			{
				HexMarkers.instance.SetMarkerType(position, layer, rotation, type);
			}
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x000309CF File Offset: 0x0002EBCF
		public static void ClearMarkerLayer(Vector3i position, HexMarkers.Layer layer)
		{
			if (HexMarkers.instance != null)
			{
				HexMarkers.instance.SetMarkerType(position, HexMarkers.MarkerType.None, layer, 0f);
			}
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x00084FD0 File Offset: 0x000831D0
		public static void ClearMarkerType(Vector3i position, HexMarkers.MarkerType type)
		{
			if (HexMarkers.instance != null)
			{
				switch (type)
				{
				case HexMarkers.MarkerType.Move:
				case HexMarkers.MarkerType.DistanceRange:
				case HexMarkers.MarkerType.RetreatWithoutWorkers:
				case HexMarkers.MarkerType.PayResource:
				case HexMarkers.MarkerType.DeployTrade:
				case HexMarkers.MarkerType.HexBorder:
				case HexMarkers.MarkerType.MoveToEncounter:
					HexMarkers.instance.SetMarkerType(position, HexMarkers.MarkerType.None, HexMarkers.Layer.Action, 0f);
					return;
				case HexMarkers.MarkerType.MoveToEnemy:
				case HexMarkers.MarkerType.Battle:
					HexMarkers.instance.SetMarkerType(position, HexMarkers.MarkerType.None, HexMarkers.Layer.Conflict, 0f);
					return;
				case HexMarkers.MarkerType.OwnerPolania:
				case HexMarkers.MarkerType.OwnerCrimea:
				case HexMarkers.MarkerType.OwnerNords:
				case HexMarkers.MarkerType.OwnerSaxony:
				case HexMarkers.MarkerType.OwnerRusviet:
				case HexMarkers.MarkerType.OwnerAlbion:
				case HexMarkers.MarkerType.OwnerTogawa:
				case HexMarkers.MarkerType.OwnerNone:
					HexMarkers.instance.SetMarkerType(position, HexMarkers.MarkerType.None, HexMarkers.Layer.Owner, 0f);
					break;
				case HexMarkers.MarkerType.Hoover:
				case HexMarkers.MarkerType.FieldSelected:
				case HexMarkers.MarkerType.Capital:
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x000309F0 File Offset: 0x0002EBF0
		public void SetMarkerType(Vector3i position, HexMarkers.MarkerType type, HexMarkers.Layer layer, float markerRotation)
		{
			this.SetMarkerType(position, layer, markerRotation, (int)type);
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x00085080 File Offset: 0x00083280
		public void SetMarkerType(Vector3i position, HexMarkers.Layer layer, float markerRotation, int iType)
		{
			int num = position.x * HexMarkers.dataSize;
			int num2 = position.y * HexMarkers.dataSize;
			if (num < 0)
			{
				num = HexMarkers.textureMapSize * HexMarkers.dataSize + num;
			}
			if (num2 < 0)
			{
				num2 = HexMarkers.textureMapSize * HexMarkers.dataSize + num2;
			}
			Color pixel = this.hexData.GetPixel(num, num2);
			Color pixel2 = this.hexData.GetPixel(num + 1, num2);
			switch (layer)
			{
			case HexMarkers.Layer.Owner:
				pixel.b = (float)iType / this.markerTypeCount;
				pixel2.b = markerRotation;
				break;
			case HexMarkers.Layer.Hoover:
				pixel.g = (float)iType / this.markerTypeCount;
				pixel2.g = markerRotation;
				break;
			case HexMarkers.Layer.Conflict:
				pixel.a = (float)iType / this.markerTypeCount;
				pixel2.a = markerRotation;
				break;
			case HexMarkers.Layer.Action:
				pixel.r = (float)iType / this.markerTypeCount;
				pixel2.r = markerRotation;
				break;
			}
			this.hexData.SetPixel(num, num2, pixel);
			this.hexData.SetPixel(num + 1, num2, pixel2);
			this.dirty = true;
		}

		// Token: 0x04000A76 RID: 2678
		public static float[] directionZeroOneScale = new float[] { 0f, 0.16666667f, 0.33333334f, 0.5f, 0.6666667f, 0.8333333f };

		// Token: 0x04000A77 RID: 2679
		public static HexMarkers instance;

		// Token: 0x04000A78 RID: 2680
		private float markerTypeCount = 64f;

		// Token: 0x04000A79 RID: 2681
		private static int textureMapSize = 64;

		// Token: 0x04000A7A RID: 2682
		private static int dataSize = 2;

		// Token: 0x04000A7B RID: 2683
		public Texture2D markersTexture;

		// Token: 0x04000A7C RID: 2684
		private Texture2D hexData;

		// Token: 0x04000A7D RID: 2685
		private Color32[] colorData;

		// Token: 0x04000A7E RID: 2686
		private bool dirty;

		// Token: 0x020001BE RID: 446
		public enum Layer
		{
			// Token: 0x04000A80 RID: 2688
			Owner,
			// Token: 0x04000A81 RID: 2689
			Hoover,
			// Token: 0x04000A82 RID: 2690
			Conflict,
			// Token: 0x04000A83 RID: 2691
			Action
		}

		// Token: 0x020001BF RID: 447
		public enum MarkerType
		{
			// Token: 0x04000A85 RID: 2693
			None,
			// Token: 0x04000A86 RID: 2694
			Move,
			// Token: 0x04000A87 RID: 2695
			DistanceRange,
			// Token: 0x04000A88 RID: 2696
			MoveToEnemy,
			// Token: 0x04000A89 RID: 2697
			RetreatWithoutWorkers,
			// Token: 0x04000A8A RID: 2698
			PayResource,
			// Token: 0x04000A8B RID: 2699
			DeployTrade,
			// Token: 0x04000A8C RID: 2700
			Battle,
			// Token: 0x04000A8D RID: 2701
			OwnerPolania,
			// Token: 0x04000A8E RID: 2702
			OwnerCrimea,
			// Token: 0x04000A8F RID: 2703
			OwnerNords,
			// Token: 0x04000A90 RID: 2704
			OwnerSaxony,
			// Token: 0x04000A91 RID: 2705
			OwnerRusviet,
			// Token: 0x04000A92 RID: 2706
			HexBorder,
			// Token: 0x04000A93 RID: 2707
			Hoover,
			// Token: 0x04000A94 RID: 2708
			FieldSelected,
			// Token: 0x04000A95 RID: 2709
			OwnerAlbion,
			// Token: 0x04000A96 RID: 2710
			OwnerTogawa,
			// Token: 0x04000A97 RID: 2711
			OwnerNone,
			// Token: 0x04000A98 RID: 2712
			MoveToEncounter,
			// Token: 0x04000A99 RID: 2713
			Capital,
			// Token: 0x04000A9A RID: 2714
			MAX = 64
		}
	}
}
