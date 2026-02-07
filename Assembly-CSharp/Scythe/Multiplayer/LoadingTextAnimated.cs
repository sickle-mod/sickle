using System;
using System.Collections;
using I2.Loc;
using TMPro;
using UnityEngine;

namespace Scythe.Multiplayer
{
	// Token: 0x0200023E RID: 574
	public class LoadingTextAnimated : MonoBehaviour
	{
		// Token: 0x0600114C RID: 4428 RVA: 0x0003337F File Offset: 0x0003157F
		public void StartAnimating()
		{
			if (this.animating)
			{
				return;
			}
			base.gameObject.SetActive(true);
			this.text.text = ScriptLocalization.Get("Lobby/Loading");
			this.animating = true;
			base.StartCoroutine(this.AnimateDotText());
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x000333BF File Offset: 0x000315BF
		public void StopAnimating()
		{
			this.animating = false;
			base.StopCoroutine(this.AnimateDotText());
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x000333E0 File Offset: 0x000315E0
		private IEnumerator AnimateDotText()
		{
			int currentDotsAmount = 0;
			while (this.animating && base.gameObject.activeInHierarchy)
			{
				if (currentDotsAmount < 3)
				{
					int num = currentDotsAmount;
					currentDotsAmount = num + 1;
					TextMeshProUGUI textMeshProUGUI = this.text;
					textMeshProUGUI.text += ".";
				}
				else
				{
					currentDotsAmount = 0;
					this.text.text = ScriptLocalization.Get("Lobby/Loading");
				}
				yield return new WaitForSeconds(0.4f);
			}
			yield break;
		}

		// Token: 0x04000D50 RID: 3408
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04000D51 RID: 3409
		private bool animating;
	}
}
