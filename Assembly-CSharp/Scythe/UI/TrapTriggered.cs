using System;
using DG.Tweening;
using Scythe.GameLogic;
using TMPro;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x02000452 RID: 1106
	public class TrapTriggered : MonoBehaviour
	{
		// Token: 0x06002294 RID: 8852 RVA: 0x0002920A File Offset: 0x0002740A
		public void SetActive(bool active)
		{
			base.gameObject.SetActive(active);
		}

		// Token: 0x06002295 RID: 8853 RVA: 0x0003E250 File Offset: 0x0003C450
		public void ShowTrapPenalty(TrapToken trap, Transform lastUnitTranform)
		{
			this.SetPenaltyText(trap);
			this.TrapTriggeredAnimation(lastUnitTranform);
		}

		// Token: 0x06002296 RID: 8854 RVA: 0x000CD6F4 File Offset: 0x000CB8F4
		private void SetPenaltyText(TrapToken trap)
		{
			this.penaltyText.text = string.Concat(new object[]
			{
				"<voffset=0.1em><size=70%><sprite name=",
				trap.Penalty.ToString(),
				"></voffset><size=100%>-",
				trap.Amount
			});
		}

		// Token: 0x06002297 RID: 8855 RVA: 0x000CD74C File Offset: 0x000CB94C
		private void TrapTriggeredAnimation(Transform lastUnitTranform)
		{
			base.transform.position = lastUnitTranform.position + new Vector3(0f, 1.5f, 0f);
			base.transform.localScale = Vector3.zero;
			Color color = this.penaltyText.color;
			color.a = 0f;
			Color color2 = this.penaltyText.color;
			color2.a = 1f;
			this.penaltyText.color = color;
			this.animationSequence = DOTween.Sequence();
			this.animationSequence.Append(base.transform.DOScale(0.01f, 0.5f).SetEase(Ease.OutElastic));
			this.animationSequence.Join(this.penaltyText.DOColor(color2, 1f).SetEase(Ease.OutQuart));
			this.animationSequence.Append(base.transform.DOMoveY(base.transform.position.y + 3f, 1f, false));
			this.animationSequence.Join(this.penaltyText.DOColor(color, 1f).SetEase(Ease.OutQuart));
			this.SetActive(true);
			this.animationSequence.Play<Sequence>().OnComplete(delegate
			{
				this.OnAnimationComplete();
			});
		}

		// Token: 0x06002298 RID: 8856 RVA: 0x0003E260 File Offset: 0x0003C460
		private void OnAnimationComplete()
		{
			this.SetActive(false);
		}

		// Token: 0x040017FF RID: 6143
		[SerializeField]
		private TextMeshProUGUI penaltyText;

		// Token: 0x04001800 RID: 6144
		private Sequence animationSequence;
	}
}
