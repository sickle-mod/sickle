using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x02000240 RID: 576
	public class SettingEntry : MonoBehaviour
	{
		// Token: 0x06001156 RID: 4438 RVA: 0x00033406 File Offset: 0x00031606
		public void Init(Sprite sprite, string title, string value)
		{
			this.image.sprite = sprite;
			this.title.text = title;
			this.value.text = value;
			base.gameObject.SetActive(true);
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x00033438 File Offset: 0x00031638
		public void InitUnavailable(Sprite sprite, string title, string value)
		{
			this.image.sprite = sprite;
			this.title.text = title;
			this.value.text = value;
			this.SetUnavailableColor(this.value);
			base.gameObject.SetActive(true);
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x00092C8C File Offset: 0x00090E8C
		public void InitYourTurnEntry(Sprite sprite, string title, string value)
		{
			this.image.sprite = sprite;
			this.title.text = title;
			this.value.text = value;
			this.value.font = this.yourTurnFont;
			base.gameObject.SetActive(true);
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x00092CDC File Offset: 0x00090EDC
		private void SetUnavailableColor(TextMeshProUGUI text)
		{
			Color color;
			ColorUtility.TryParseHtmlString("#EB5757", out color);
			color.a = 0.7f;
			text.color = color;
		}

		// Token: 0x04000D56 RID: 3414
		[SerializeField]
		private Image image;

		// Token: 0x04000D57 RID: 3415
		[SerializeField]
		private TextMeshProUGUI title;

		// Token: 0x04000D58 RID: 3416
		[SerializeField]
		private TextMeshProUGUI value;

		// Token: 0x04000D59 RID: 3417
		[SerializeField]
		private TMP_FontAsset yourTurnFont;
	}
}
