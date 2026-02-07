using System;
using System.Collections;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200003F RID: 63
public class NameAndKarmaBar : MonoBehaviour
{
	// Token: 0x060001FC RID: 508 RVA: 0x000291A0 File Offset: 0x000273A0
	private void OnEnable()
	{
		if (PlayerInfo.me.IsLoaded)
		{
			this.Init();
			return;
		}
		base.StartCoroutine(this.DelayedInit());
	}

	// Token: 0x060001FD RID: 509 RVA: 0x000291C2 File Offset: 0x000273C2
	private void Init()
	{
		this.playerName.text = PlayerInfo.me.PlayerStats.Name;
		this.UpdateKarma();
	}

	// Token: 0x060001FE RID: 510 RVA: 0x0005A744 File Offset: 0x00058944
	private void UpdateKarma()
	{
		int num = ((PlayerInfo.me.PlayerStats.Karma < 100) ? (PlayerInfo.me.PlayerStats.Karma / 20 + 1) : this.karmaSigns.Length);
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

	// Token: 0x060001FF RID: 511 RVA: 0x000291E4 File Offset: 0x000273E4
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

	// Token: 0x06000200 RID: 512 RVA: 0x0005A7E0 File Offset: 0x000589E0
	public void ShowStats_OnClick()
	{
		Lobby componentInParent = base.GetComponentInParent<Lobby>();
		if (componentInParent != null)
		{
			componentInParent.ShowCurrentPlayerStats();
			return;
		}
		Debug.LogError("Trying to show player stats, but the lobby script cannot be found in any parent of NameAndKarmaBar");
	}

	// Token: 0x04000181 RID: 385
	private const int karmaPerSign = 20;

	// Token: 0x04000182 RID: 386
	[SerializeField]
	private TextMeshProUGUI playerName;

	// Token: 0x04000183 RID: 387
	[SerializeField]
	private Image[] karmaSigns;

	// Token: 0x04000184 RID: 388
	[SerializeField]
	private Sprite[] karmaLevels;
}
