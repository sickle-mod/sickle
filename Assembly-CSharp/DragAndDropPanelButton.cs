using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000079 RID: 121
public class DragAndDropPanelButton : MonoBehaviour
{
	// Token: 0x0600042D RID: 1069 RVA: 0x0002A9EF File Offset: 0x00028BEF
	public void SetActive(bool active)
	{
		if (base.gameObject.activeSelf != active)
		{
			if (active)
			{
				this.ShowButtonAnimation();
				return;
			}
			this.HideButtonAnimation();
		}
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x0002AA0F File Offset: 0x00028C0F
	public void SetInteractable(bool interactable)
	{
		this.button.interactable = interactable;
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x0002AA1D File Offset: 0x00028C1D
	public void SetNumberOfObjects(int objectCount)
	{
		this.OnNumberOfObjectsChanged(objectCount);
		this.amount.text = objectCount.ToString();
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x00063DBC File Offset: 0x00061FBC
	private void OnNumberOfObjectsChanged(int newObjectCount)
	{
		int num = 0;
		if (int.TryParse(this.amount.text, out num))
		{
			if (newObjectCount > num)
			{
				this.FlashIncreaseAnimation();
				return;
			}
			if (newObjectCount < num)
			{
				this.FlashDecreaseAnimation();
			}
		}
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x0002AA38 File Offset: 0x00028C38
	public string GetText()
	{
		return this.amount.text;
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x0002AA45 File Offset: 0x00028C45
	public Button GetButton()
	{
		return this.button;
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x00063DF4 File Offset: 0x00061FF4
	public void MoveByDistance(float distance)
	{
		Vector3 localPosition = base.GetComponent<RectTransform>().localPosition;
		localPosition.x += distance;
		base.GetComponent<RectTransform>().localPosition = localPosition;
	}

	// Token: 0x06000434 RID: 1076 RVA: 0x00063E28 File Offset: 0x00062028
	private void ShowButtonAnimation()
	{
		this.ForceToFinishAnimation();
		base.gameObject.SetActive(true);
		base.transform.localScale = Vector3.zero;
		base.transform.DOScale(1f, 0.5f).SetEase(Ease.InOutQuart);
	}

	// Token: 0x06000435 RID: 1077 RVA: 0x0002AA4D File Offset: 0x00028C4D
	private void HideButtonAnimation()
	{
		this.ForceToFinishAnimation();
		base.transform.DOScale(0f, 0.5f).SetEase(Ease.InOutQuart).OnComplete(delegate
		{
			base.gameObject.SetActive(false);
		});
	}

	// Token: 0x06000436 RID: 1078 RVA: 0x0002AA83 File Offset: 0x00028C83
	public void FlashIncreaseAnimation()
	{
		this.ForceToFinishAnimation();
		base.transform.DOScale(1.2f, 0.5f).SetEase(Ease.OutFlash, 4f, -1f);
	}

	// Token: 0x06000437 RID: 1079 RVA: 0x0002AAB2 File Offset: 0x00028CB2
	public void FlashDecreaseAnimation()
	{
		this.ForceToFinishAnimation();
		base.transform.DOScale(0.8f, 0.5f).SetEase(Ease.OutFlash, 4f, -1f);
	}

	// Token: 0x06000438 RID: 1080 RVA: 0x0002AAE1 File Offset: 0x00028CE1
	public void ForceToFinishAnimation()
	{
		base.transform.DOComplete(true);
	}

	// Token: 0x04000393 RID: 915
	[SerializeField]
	private TextMeshProUGUI amount;

	// Token: 0x04000394 RID: 916
	[SerializeField]
	private Button button;
}
