using System;
using System.Collections.Generic;
using HoneyFramework;
using Scythe.GameLogic;
using Scythe.UI;
using UnityEngine;

// Token: 0x0200009E RID: 158
public class AnimateCamera : MonoBehaviour
{
	// Token: 0x0600051C RID: 1308 RVA: 0x0002B2C7 File Offset: 0x000294C7
	private void Awake()
	{
		AnimateCamera.Instance = this;
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x000665C8 File Offset: 0x000647C8
	private void Update()
	{
		if (this.camAnim)
		{
			float num = (Time.time - this.camAnimStartTime) / this.camAnimLength;
			float num2 = this.camAnimationCurve.Evaluate(num);
			if (num > 1f)
			{
				num2 = 1f;
				this.camAnim = false;
				if (GameController.GameManager.showEnemyActions)
				{
					ShowEnemyMoves.Instance.SetAnimationInProgress(false);
				}
				if (GameController.GameManager.showEnemyActions && !ShowEnemyMoves.Instance.MoreAnimations())
				{
					ShowEnemyMoves.Instance.GetNextAnimation();
				}
			}
			GameController.Instance.cameraControler.swivel.transform.position = new Vector3(Mathf.Lerp(this.camXfrom, this.camXto, num2), 0f, Mathf.Lerp(this.camYfrom, this.camYto, num2));
			GameController.Instance.cameraControler.zoom = Mathf.Lerp(this.camZfrom, this.camZto, num2);
			GameController.Instance.cameraControler.stickMinZoom = Mathf.Lerp(this.camParamsFrom.stickMinZoom, this.camParamsTo.stickMinZoom, num2);
			GameController.Instance.cameraControler.stickMaxZoom = Mathf.Lerp(this.camParamsFrom.stickMaxZoom, this.camParamsTo.stickMaxZoom, num2);
			GameController.Instance.cameraControler.swivelMinZoom = Mathf.Lerp(this.camParamsFrom.swivelMinZoom, this.camParamsTo.swivelMinZoom, num2);
			GameController.Instance.cameraControler.swivelMaxZoom = Mathf.Lerp(this.camParamsFrom.swivelMaxZoom, this.camParamsTo.swivelMaxZoom, num2);
			GameController.Instance.cameraControler.posXMin = Mathf.Lerp(this.camParamsFrom.posXMin, this.camParamsTo.posXMin, num2);
			GameController.Instance.cameraControler.posXMax = Mathf.Lerp(this.camParamsFrom.posXMax, this.camParamsTo.posXMax, num2);
			GameController.Instance.cameraControler.posYMin = Mathf.Lerp(this.camParamsFrom.posYMin, this.camParamsTo.posYMin, num2);
			GameController.Instance.cameraControler.posYMax = Mathf.Lerp(this.camParamsFrom.posYMax, this.camParamsTo.posYMax, num2);
		}
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x00066810 File Offset: 0x00064A10
	public void AnimateCam(float camXto, float camYto, float camZto, CameraParams camParamsTo, float camAnimLength = 1f)
	{
		this.camXfrom = GameController.Instance.cameraControler.swivel.transform.position.x;
		this.camYfrom = GameController.Instance.cameraControler.swivel.transform.position.z;
		this.camZfrom = GameController.Instance.cameraControler.zoom;
		this.camParamsFrom = new CameraParams
		{
			stickMinZoom = GameController.Instance.cameraControler.stickMinZoom,
			stickMaxZoom = GameController.Instance.cameraControler.stickMaxZoom,
			swivelMinZoom = GameController.Instance.cameraControler.swivelMinZoom,
			swivelMaxZoom = GameController.Instance.cameraControler.swivelMaxZoom,
			posXMin = GameController.Instance.cameraControler.posXMin,
			posXMax = GameController.Instance.cameraControler.posXMax,
			posYMin = GameController.Instance.cameraControler.posYMin,
			posYMax = GameController.Instance.cameraControler.posYMax
		};
		this.camXto = camXto;
		this.camYto = camYto;
		this.camZto = camZto;
		this.camParamsTo = camParamsTo;
		this.camAnimLength = camAnimLength;
		this.camAnimStartTime = Time.time;
		this.camAnim = true;
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x00066964 File Offset: 0x00064B64
	private void SetCamera(Vector3 position, float posXMin, float posXMax, float posYMin, float posYMax, float zoom, float stickMinZoom, float stickMaxZoom, float swivelMinZoom, float swivelMaxZoom, float focalSizeFar)
	{
		GameController.Instance.cameraControler.swivel.transform.position = new Vector3(position.x, 0f, position.z);
		GameController.Instance.cameraControler.posXMin = posXMin;
		GameController.Instance.cameraControler.posXMax = posXMax;
		GameController.Instance.cameraControler.posYMin = posYMin;
		GameController.Instance.cameraControler.posYMax = posYMax;
		GameController.Instance.cameraControler.zoom = zoom;
		GameController.Instance.cameraControler.stickMinZoom = stickMinZoom;
		GameController.Instance.cameraControler.stickMaxZoom = stickMaxZoom;
		GameController.Instance.cameraControler.swivelMinZoom = swivelMinZoom;
		GameController.Instance.cameraControler.swivelMaxZoom = swivelMaxZoom;
		GameController.Instance.cameraControler.focalSizeFar = focalSizeFar;
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x00066A48 File Offset: 0x00064C48
	public Vector3 CalculateCenterOfHexes(ICollection<Vector3> hexes)
	{
		Vector3 vector = Vector3.zero;
		foreach (Vector3 vector2 in hexes)
		{
			vector += vector2;
		}
		return vector / (float)hexes.Count;
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x00066AA4 File Offset: 0x00064CA4
	public float CalculateZoomToShowGivenHexes(IEnumerable<GameHex> playerHexes)
	{
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		foreach (GameHex gameHex in playerHexes)
		{
			list.Add(gameHex.posX);
			list2.Add(gameHex.posY);
		}
		int num = Mathf.Max(list.ToArray());
		int num2 = Mathf.Max(list2.ToArray());
		int num3 = Mathf.Min(list.ToArray());
		int num4 = Mathf.Min(list2.ToArray());
		int num5 = num2 - num4;
		int num6 = num - num3;
		if (num5 >= num6)
		{
			return this.hexesZooms.zoomDependentOnHexAmountHeight[num5].zoom;
		}
		return this.hexesZooms.zoomDependentOnHexAmountWidth[num6].zoom;
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x0002B2CF File Offset: 0x000294CF
	public void StopAnimating()
	{
		this.camAnim = false;
	}

	// Token: 0x04000432 RID: 1074
	public static AnimateCamera Instance;

	// Token: 0x04000433 RID: 1075
	public HexesAmountZoom hexesZooms;

	// Token: 0x04000434 RID: 1076
	public HexesAmountZoom basesZooms;

	// Token: 0x04000435 RID: 1077
	public AnimationCurve camAnimationCurve;

	// Token: 0x04000436 RID: 1078
	public float zoomHexHeight = 0.23f;

	// Token: 0x04000437 RID: 1079
	public float cameraAnimationTime = 1f;

	// Token: 0x04000438 RID: 1080
	private float camXfrom;

	// Token: 0x04000439 RID: 1081
	private float camXto;

	// Token: 0x0400043A RID: 1082
	private float camYfrom;

	// Token: 0x0400043B RID: 1083
	private float camYto;

	// Token: 0x0400043C RID: 1084
	private float camZfrom;

	// Token: 0x0400043D RID: 1085
	private float camZto;

	// Token: 0x0400043E RID: 1086
	private float camAnimStartTime;

	// Token: 0x0400043F RID: 1087
	private CameraParams camParamsFrom;

	// Token: 0x04000440 RID: 1088
	private CameraParams camParamsTo;

	// Token: 0x04000441 RID: 1089
	private bool camAnim;

	// Token: 0x04000442 RID: 1090
	private float camAnimLength;

	// Token: 0x0200009F RID: 159
	[Serializable]
	public struct ZoomThatShowHexes
	{
		// Token: 0x04000443 RID: 1091
		public int amountOfHexes;

		// Token: 0x04000444 RID: 1092
		public float zoom;
	}
}
