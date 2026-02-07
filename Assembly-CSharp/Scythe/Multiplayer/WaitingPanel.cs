using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x02000278 RID: 632
	public class WaitingPanel : MonoBehaviour
	{
		// Token: 0x06001366 RID: 4966 RVA: 0x00034F46 File Offset: 0x00033146
		private void Update()
		{
			this.waitingImage.transform.Rotate(Vector3.back, 30f * Time.deltaTime);
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x00034F68 File Offset: 0x00033168
		public void Activate()
		{
			this.Activate(string.Empty, true);
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x00034F76 File Offset: 0x00033176
		public void Activate(string text, bool blurBackground)
		{
			this.hidingPanel.SetActive(blurBackground);
			if (this.waitingText != null)
			{
				this.waitingText.text = text;
			}
			base.gameObject.SetActive(true);
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x00029172 File Offset: 0x00027372
		public void Deactivate()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x04000E8C RID: 3724
		[SerializeField]
		private GameObject hidingPanel;

		// Token: 0x04000E8D RID: 3725
		[SerializeField]
		private Image waitingImage;

		// Token: 0x04000E8E RID: 3726
		[SerializeField]
		private TextMeshProUGUI waitingText;

		// Token: 0x04000E8F RID: 3727
		[SerializeField]
		private const float rotationPerSecond = 30f;
	}
}
