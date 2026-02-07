using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000072 RID: 114
public class LoadingPlayerInfo : MonoBehaviour
{
	// Token: 0x060003E3 RID: 995 RVA: 0x0002A6CF File Offset: 0x000288CF
	public void FillPlayerInfo(string factionName, string playerName, Sprite icon)
	{
		this.factionName.text = factionName;
		this.playerName.text = playerName;
		this.factionIcon.sprite = icon;
	}

	// Token: 0x04000356 RID: 854
	[SerializeField]
	private TMP_Text factionName;

	// Token: 0x04000357 RID: 855
	[SerializeField]
	private TMP_Text playerName;

	// Token: 0x04000358 RID: 856
	[SerializeField]
	private Image factionIcon;
}
