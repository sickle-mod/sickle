using System;
using Scythe.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x0200047B RID: 1147
	public class LastActionInfoPresenter : MonoBehaviour
	{
		// Token: 0x06002458 RID: 9304 RVA: 0x0003F23F File Offset: 0x0003D43F
		private void OnEnable()
		{
			GameController.GameManager.ObtainActionInfo += this.ActionMessageReceived;
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x0003F257 File Offset: 0x0003D457
		private void OnDisable()
		{
			GameController.GameManager.ObtainActionInfo -= this.ActionMessageReceived;
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x0003F26F File Offset: 0x0003D46F
		private void Update()
		{
			if (this.updateNeeded)
			{
				this.SetNewMessage();
			}
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x0003F27F File Offset: 0x0003D47F
		private bool MultiplayerAndEnemyTurn()
		{
			return GameController.GameManager.IsMultiplayer && GameController.GameManager.PlayerOwner != GameController.GameManager.PlayerCurrent;
		}

		// Token: 0x0600245C RID: 9308 RVA: 0x0003F2A8 File Offset: 0x0003D4A8
		private void ActionMessageReceived(string actionInfo)
		{
			if (this.MultiplayerAndEnemyTurn())
			{
				this.lastMessage = actionInfo.Substring(0, 2) + actionInfo.Remove(0, 2);
				this.updateNeeded = true;
			}
		}

		// Token: 0x0600245D RID: 9309 RVA: 0x0003F2D4 File Offset: 0x0003D4D4
		private void SetNewMessage()
		{
			if (this.MultiplayerAndEnemyTurn())
			{
				this.updateNeeded = false;
				this.ActionInfoText.text = this.ParseMessage(this.lastMessage);
			}
		}

		// Token: 0x0600245E RID: 9310 RVA: 0x000D73BC File Offset: 0x000D55BC
		public string ParseMessage(string message)
		{
			if (message.StartsWith("^"))
			{
				char c = message[1];
				if (c != 'A')
				{
					if (c != 'C')
					{
						switch (c)
						{
						case 'N':
							this.FactionImage.GetComponent<Image>().sprite = GameController.factionInfo[Faction.Nordic].logo;
							break;
						case 'P':
							this.FactionImage.GetComponent<Image>().sprite = GameController.factionInfo[Faction.Polania].logo;
							break;
						case 'R':
							this.FactionImage.GetComponent<Image>().sprite = GameController.factionInfo[Faction.Rusviet].logo;
							break;
						case 'S':
							this.FactionImage.GetComponent<Image>().sprite = GameController.factionInfo[Faction.Saxony].logo;
							break;
						case 'T':
							this.FactionImage.GetComponent<Image>().sprite = GameController.factionInfo[Faction.Togawa].logo;
							break;
						}
					}
					else
					{
						this.FactionImage.GetComponent<Image>().sprite = GameController.factionInfo[Faction.Crimea].logo;
					}
				}
				else
				{
					this.FactionImage.GetComponent<Image>().sprite = GameController.factionInfo[Faction.Albion].logo;
				}
				message = message.Remove(0, 2);
				if (!this.FactionImage.enabled)
				{
					this.FactionImage.enabled = true;
				}
			}
			return message;
		}

		// Token: 0x0400195E RID: 6494
		public TextMeshProUGUI ActionInfoText;

		// Token: 0x0400195F RID: 6495
		public Image FactionImage;

		// Token: 0x04001960 RID: 6496
		private string lastMessage = "";

		// Token: 0x04001961 RID: 6497
		private bool updateNeeded;
	}
}
