using System;
using System.Collections;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x02000276 RID: 630
	public class TopBar : MonoBehaviour
	{
		// Token: 0x0600135B RID: 4955 RVA: 0x00034EDC File Offset: 0x000330DC
		private void OnEnable()
		{
			if (PlayerInfo.me.IsLoaded)
			{
				this.Init();
				return;
			}
			base.StartCoroutine(this.DelayedInit());
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x00034EFE File Offset: 0x000330FE
		private void Init()
		{
			this.playerName.text = PlayerInfo.me.PlayerStats.Name;
			this.UpdateKarma();
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x00034F20 File Offset: 0x00033120
		private IEnumerator DelayedInit()
		{
			yield return new WaitForSeconds(0.5f);
			if (PlayerInfo.me.IsLoaded)
			{
				this.Init();
			}
			else
			{
				base.StartCoroutine(this.DelayedInit());
			}
			yield break;
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x000987D0 File Offset: 0x000969D0
		public void UpdateKarma()
		{
			int num = ((PlayerInfo.me.PlayerStats.Karma < 100) ? (PlayerInfo.me.PlayerStats.Karma / 20 + 1) : 5);
			for (int i = 0; i < num; i++)
			{
				this.karmaSigns[i].sprite = this.karmaLevels[num - 1];
				this.karmaSigns[i].gameObject.SetActive(true);
			}
			for (int j = num; j < this.karmaSigns.Length; j++)
			{
				this.karmaSigns[j].gameObject.SetActive(false);
			}
		}

		// Token: 0x04000E85 RID: 3717
		private const int KARMA_PER_SIGN = 20;

		// Token: 0x04000E86 RID: 3718
		[SerializeField]
		private TextMeshProUGUI playerName;

		// Token: 0x04000E87 RID: 3719
		[SerializeField]
		private Image[] karmaSigns;

		// Token: 0x04000E88 RID: 3720
		[SerializeField]
		private Sprite[] karmaLevels;
	}
}
