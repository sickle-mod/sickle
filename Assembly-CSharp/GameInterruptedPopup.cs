using System;
using I2.Loc;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;

// Token: 0x02000043 RID: 67
public class GameInterruptedPopup : MonoBehaviour
{
	// Token: 0x06000215 RID: 533 RVA: 0x0005A9E4 File Offset: 0x00058BE4
	public void Activate(GameType gameType, bool isRanked, int timeInMinutes)
	{
		base.gameObject.SetActive(true);
		if (this.popupContentLabel)
		{
			string text = string.Empty;
			int num;
			if (gameType == GameType.Synchronous)
			{
				num = timeInMinutes;
				if (isRanked)
				{
					text = "Lobby/PlayStayInterruptedRankedDescription";
				}
				else
				{
					text = "Lobby/PlayStayInterruptedDescription";
				}
			}
			else
			{
				num = timeInMinutes / 1440;
				if (isRanked)
				{
					text = "Lobby/PlayGoInterruptedRankedDescription";
				}
				else
				{
					text = "Lobby/PlayGoInterruptedDescription";
				}
			}
			string text2 = ScriptLocalization.Get(text);
			if (!string.IsNullOrEmpty(text2))
			{
				text2 = text2.Replace("{[TIME]}", num.ToString());
			}
			else
			{
				text2 = "[LOC KEY NOT FOUND: " + text + "]";
			}
			this.popupContentLabel.text = text2;
		}
	}

	// Token: 0x06000216 RID: 534 RVA: 0x00029172 File Offset: 0x00027372
	public void Deactivate()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000217 RID: 535 RVA: 0x00029261 File Offset: 0x00027461
	public void OkButton_OnClick()
	{
		this.Deactivate();
	}

	// Token: 0x0400018F RID: 399
	[SerializeField]
	private TextMeshProUGUI popupContentLabel;
}
