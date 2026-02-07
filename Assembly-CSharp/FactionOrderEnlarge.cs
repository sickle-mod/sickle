using System;
using System.Collections.Generic;
using Scythe.GameLogic;
using Scythe.UI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000DD RID: 221
public class FactionOrderEnlarge : MonoBehaviour
{
	// Token: 0x0600067E RID: 1662 RVA: 0x0002C210 File Offset: 0x0002A410
	private void OnEnable()
	{
		this.scaler = global::UnityEngine.Object.FindObjectOfType<PlatformCanvasScaler>();
		this.UpdateStatus();
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x0002C223 File Offset: 0x0002A423
	private void Update()
	{
		if (this.enlargeMoveEnabled)
		{
			this.MousePositionUpdate();
			if (!this.isMouseOnLogos())
			{
				EnemyInfoPreview.Instance.TurnOffEnemyPlayerInfo();
				this.MovePanelActivation(false);
			}
			this.EnlargePositionUpdate();
			this.SetupLogosBarPosition(this.currentPointerPosition);
		}
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x0007029C File Offset: 0x0006E49C
	public bool isMouseOnLogos()
	{
		return this.playerInputIsOnLogos.x < this.mouseRightXBorder && this.playerInputIsOnLogos.x > this.mouseLeftXBorder && this.playerInputIsOnLogos.y < this.mouseTopYBorder && this.playerInputIsOnLogos.y > this.mouseBottomYBorder;
	}

	// Token: 0x06000681 RID: 1665 RVA: 0x0002C25E File Offset: 0x0002A45E
	public void MovePanelActivation(bool activ)
	{
		this.enlargeMoveEnabled = activ;
	}

	// Token: 0x06000682 RID: 1666 RVA: 0x0002C267 File Offset: 0x0002A467
	public void SetupFactionLogosReference()
	{
		this.logosFactionOrder = GameController.Instance.playersFactions.GetBasicLogos();
	}

	// Token: 0x06000683 RID: 1667 RVA: 0x000702F8 File Offset: 0x0006E4F8
	private void EnlargePositionUpdate()
	{
		this.playerInputEnlargePosition = Input.GetTouch(0).position;
		base.GetComponent<RectTransform>();
		float width = this.parentCanvas.GetComponent<RectTransform>().rect.width;
		Vector2 anchoredPosition = base.transform.GetComponent<RectTransform>().anchoredPosition;
		this.mouseCanvasPoistion = this.playerInputEnlargePosition.x * width / (float)Screen.width;
		this.leftBorderCanvasPosition = this.leftBorder * width / (float)Screen.width;
		this.borderWidth = width - this.leftBorderCanvasPosition;
		this.xNewMousePosition = this.borderWidth - this.mouseCanvasPoistion + this.leftBorderCanvasPosition;
		this.scaledNewMousePoistion = this.xNewMousePosition / this.bottomBarScale;
		base.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-this.scaledNewMousePoistion + this.enlargeWidthOffset, this.enlargeYPosition);
	}

	// Token: 0x06000684 RID: 1668 RVA: 0x000703E4 File Offset: 0x0006E5E4
	private void MousePositionUpdate()
	{
		this.playerInputLogos = Input.GetTouch(0).position;
		this.playerInputIsOnLogos.x = this.playerInputLogos.x;
		this.playerInputIsOnLogos.y = this.playerInputLogos.y;
		this.currentPointerPosition = this.GetMouseLerpPosition(this.playerInputIsOnLogos.x);
	}

	// Token: 0x06000685 RID: 1669 RVA: 0x0002C27E File Offset: 0x0002A47E
	private float GetMouseLerpPosition(float mousePositionX)
	{
		return Mathf.InverseLerp(this.mouseLeftXBorder, this.mouseRightXBorder, mousePositionX);
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x00070450 File Offset: 0x0006E650
	private void SetupLogosBarPosition(float lerpMousePosition)
	{
		this.logosPositionToSetup.y = 0f;
		this.logosPositionToSetup.z = 0f;
		this.logosPositionToSetup.x = Mathf.Lerp(this.logosLeftXBorder, this.logosRightXBorder, lerpMousePosition);
		this.enlargeLogosParent.localPosition = this.logosPositionToSetup;
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x000704AC File Offset: 0x0006E6AC
	private void SaveFactionOrderBorderPositions()
	{
		for (int i = 1; i <= this.logosFactionOrder.Length - 1; i++)
		{
			if (this.logosFactionOrder[i].gameObject.activeInHierarchy)
			{
				this.mouseLeftXBorder = RectTransformUtility.WorldToScreenPoint(this.parentCanvas.worldCamera, this.logosFactionOrder[i].GetComponent<RectTransform>().position).x - this.logosFactionOrder[i].GetComponent<RectTransform>().rect.height;
			}
		}
		RectTransform component = this.logosFactionOrder[0].GetComponent<RectTransform>();
		Vector3 position = component.position;
		Vector2 vector = RectTransformUtility.WorldToScreenPoint(this.parentCanvas.worldCamera, position);
		float num = component.rect.height * this.scaler.HeightFactor * this.scaler.ScaleFactor;
		this.mouseRightXBorder = vector.x;
		this.mouseTopYBorder = vector.y;
		this.mouseBottomYBorder = vector.y - num;
		this.logosRightXBorder = this.logosLeftXBorder - this.logosPositionOffset * (float)GameController.GameManager.GetPlayers().Count;
	}

	// Token: 0x06000688 RID: 1672 RVA: 0x000705C4 File Offset: 0x0006E7C4
	private void UpdateStatus()
	{
		List<Player> players = GameController.GameManager.GetPlayers();
		int playerCurrentId = GameController.GameManager.PlayerCurrentId;
		for (int i = 0; i < this.factionEmblems.Length; i++)
		{
			if (i >= players.Count)
			{
				this.factionEmblems[i].gameObject.SetActive(false);
			}
			else
			{
				Player player = players[(playerCurrentId + i) % players.Count];
				this.factionEmblems[i].gameObject.SetActive(true);
				this.factionEmblems[i].sprite = GameController.factionInfo[player.matFaction.faction].logo;
			}
		}
		this.SaveFactionOrderBorderPositions();
	}

	// Token: 0x04000599 RID: 1433
	[SerializeField]
	private Image[] factionEmblems = new Image[5];

	// Token: 0x0400059A RID: 1434
	[SerializeField]
	private Transform enlargeLogosParent;

	// Token: 0x0400059B RID: 1435
	[SerializeField]
	private bool enlargeMoveEnabled;

	// Token: 0x0400059C RID: 1436
	[SerializeField]
	private Canvas parentCanvas;

	// Token: 0x0400059D RID: 1437
	[SerializeField]
	private Image[] logosFactionOrder;

	// Token: 0x0400059E RID: 1438
	[SerializeField]
	private float mouseLeftXBorder = 993f;

	// Token: 0x0400059F RID: 1439
	[SerializeField]
	private float mouseRightXBorder = 1059f;

	// Token: 0x040005A0 RID: 1440
	[SerializeField]
	private float mouseTopYBorder = 144f;

	// Token: 0x040005A1 RID: 1441
	[SerializeField]
	private float mouseBottomYBorder = 113f;

	// Token: 0x040005A2 RID: 1442
	[SerializeField]
	private float logosLeftXBorder = 120f;

	// Token: 0x040005A3 RID: 1443
	[SerializeField]
	private float logosRightXBorder = 54f;

	// Token: 0x040005A4 RID: 1444
	[SerializeField]
	private float logosPositionOffset = 51f;

	// Token: 0x040005A5 RID: 1445
	[SerializeField]
	private float currentPointerPosition;

	// Token: 0x040005A6 RID: 1446
	[SerializeField]
	private Vector3 logosPositionToSetup;

	// Token: 0x040005A7 RID: 1447
	[SerializeField]
	private Vector2 playerInputIsOnLogos;

	// Token: 0x040005A8 RID: 1448
	[SerializeField]
	private float enlargeYPosition = 120f;

	// Token: 0x040005A9 RID: 1449
	[SerializeField]
	private float leftBorder = 900f;

	// Token: 0x040005AA RID: 1450
	[SerializeField]
	private float enlargeWidthOffset;

	// Token: 0x040005AB RID: 1451
	[SerializeField]
	private float mouseCanvasPoistion;

	// Token: 0x040005AC RID: 1452
	[SerializeField]
	private float leftBorderCanvasPosition;

	// Token: 0x040005AD RID: 1453
	[SerializeField]
	private float borderWidth;

	// Token: 0x040005AE RID: 1454
	[SerializeField]
	private float xNewMousePosition;

	// Token: 0x040005AF RID: 1455
	[SerializeField]
	private float scaledNewMousePoistion;

	// Token: 0x040005B0 RID: 1456
	[SerializeField]
	private float bottomBarScale = 1.25f;

	// Token: 0x040005B1 RID: 1457
	private Vector3 playerInputEnlargePosition;

	// Token: 0x040005B2 RID: 1458
	private Vector3 playerInputLogos;

	// Token: 0x040005B3 RID: 1459
	private PlatformCanvasScaler scaler;
}
