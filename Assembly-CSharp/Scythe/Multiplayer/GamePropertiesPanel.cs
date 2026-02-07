using System;
using System.Collections.Generic;
using I2.Loc;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;

namespace Scythe.Multiplayer
{
	// Token: 0x02000241 RID: 577
	public class GamePropertiesPanel : MonoBehaviour
	{
		// Token: 0x0600115B RID: 4443 RVA: 0x00092D0C File Offset: 0x00090F0C
		public void Init(LobbyGame roomData)
		{
			this.InitLocalization();
			this.ClearEntry();
			this.gameName.text = roomData.Name;
			this.SetGameType(roomData.IsAsynchronous);
			if (roomData.IsRanked)
			{
				this.AddNewSetting(this.eloImage, this.eloText, string.Format("{0}-{1}", roomData.MinELO, roomData.MaxELO));
				this.rankedObject.SetActive(true);
			}
			else
			{
				this.rankedObject.SetActive(false);
			}
			this.AddNewSetting(this.playersImage, this.playersText, string.Format("{0}/{1}", roomData.Players, roomData.MaxPlayers));
			if (roomData.IsAsynchronous)
			{
				if (roomData.PlayerClockTime / 1440 < 1)
				{
					this.AddNewSetting(this.playerTimeImage, this.playersTimeText, string.Format("{0} {1}", roomData.PlayerClockTime / 60, ScriptLocalization.Get("Lobby/Hours")));
				}
				else
				{
					this.AddNewSetting(this.playerTimeImage, this.playersTimeText, string.Format("{0} {1}", roomData.PlayerClockTime / 1440, this.asynchronousTime));
				}
			}
			else
			{
				this.AddNewSetting(this.playerTimeImage, this.playersTimeText, string.Format("{0} {1}", roomData.PlayerClockTime, this.synchronousTime));
			}
			this.AddNewSetting(this.promoCardsImage, this.promoCardsText, ScriptLocalization.Get(roomData.PromoCardsEnabled ? "Common/Yes" : "Common/No"));
			if (!roomData.AllRandom)
			{
				this.AddNewSetting(this.matChoicesImage, this.matChoicesText, ScriptLocalization.Get("Common/Yes"));
			}
			if (roomData.IsRanked)
			{
				this.AddNewSetting(this.possibleEloChangeImage, this.possibleEloChangeText, this.GetPossibleEloChange(roomData.PlayersList));
			}
			if (roomData.InvadersFromAfar)
			{
				this.expansionObject.SetActive(true);
			}
			if (roomData.IsPrivate && this.privateObject)
			{
				this.privateObject.SetActive(true);
			}
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x00092F24 File Offset: 0x00091124
		private void ClearEntry()
		{
			if (this.expansionObject)
			{
				this.expansionObject.SetActive(false);
			}
			if (this.privateObject)
			{
				this.privateObject.SetActive(false);
			}
			int num = 1;
			while (this.settingsContent.childCount > num)
			{
				Transform child = this.settingsContent.transform.GetChild(0);
				child.SetParent(null, false);
				global::UnityEngine.Object.Destroy(child.gameObject);
			}
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x00092F98 File Offset: 0x00091198
		private void SetGameType(bool asynchronous)
		{
			if (asynchronous)
			{
				this.gameType.text = "<color=#479BDA>" + this.playAndGoText + "</color>";
				return;
			}
			this.gameType.text = "<color=#36AF60>" + this.playAndStayText + "</color>";
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x00092FEC File Offset: 0x000911EC
		private void InitLocalization()
		{
			this.eloText = ScriptLocalization.Get("Lobby/ELOColon");
			this.playersText = ScriptLocalization.Get("Lobby/PlayersColon");
			this.playersTimeText = ScriptLocalization.Get("Lobby/PlayerTimeColon");
			this.promoCardsText = ScriptLocalization.Get("Lobby/PromoCardsColon");
			this.matChoicesText = ScriptLocalization.Get("Lobby/MatsChoicesColon");
			this.possibleEloChangeText = ScriptLocalization.Get("Lobby/EloChange");
			this.playAndStayText = ScriptLocalization.Get("Lobby/SynchronousGames");
			this.playAndGoText = ScriptLocalization.Get("Lobby/AsynchronousGames");
			this.synchronousTime = ScriptLocalization.Get("Lobby/MinutesAbbreviation");
			this.asynchronousTime = ScriptLocalization.Get("Lobby/Days");
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x0009309C File Offset: 0x0009129C
		private void AddNewSetting(Sprite sprite, string title, string value)
		{
			SettingEntry settingEntry = global::UnityEngine.Object.Instantiate<SettingEntry>(this.settingEntryPrefab, this.settingsContent);
			int num = 2;
			settingEntry.transform.SetSiblingIndex(settingEntry.transform.parent.childCount - num);
			settingEntry.Init(sprite, title, value);
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x000930E4 File Offset: 0x000912E4
		private string GetPossibleEloChange(List<PlayerInfo> players)
		{
			EloCalculator.PossibleEloChange possibleEloChange = EloCalculator.CalculatePossibleEloChange(players);
			return string.Format("{0}:{1}", possibleEloChange.min, possibleEloChange.max);
		}

		// Token: 0x04000D5A RID: 3418
		[SerializeField]
		private SettingEntry settingEntryPrefab;

		// Token: 0x04000D5B RID: 3419
		[SerializeField]
		private TextMeshProUGUI gameName;

		// Token: 0x04000D5C RID: 3420
		[SerializeField]
		private Transform settingsContent;

		// Token: 0x04000D5D RID: 3421
		[SerializeField]
		private TextMeshProUGUI gameType;

		// Token: 0x04000D5E RID: 3422
		[SerializeField]
		private Sprite playersImage;

		// Token: 0x04000D5F RID: 3423
		[SerializeField]
		private Sprite playerTimeImage;

		// Token: 0x04000D60 RID: 3424
		[SerializeField]
		private Sprite promoCardsImage;

		// Token: 0x04000D61 RID: 3425
		[SerializeField]
		private Sprite eloImage;

		// Token: 0x04000D62 RID: 3426
		[SerializeField]
		private Sprite matChoicesImage;

		// Token: 0x04000D63 RID: 3427
		[SerializeField]
		private Sprite possibleEloChangeImage;

		// Token: 0x04000D64 RID: 3428
		[SerializeField]
		private GameObject expansionObject;

		// Token: 0x04000D65 RID: 3429
		[SerializeField]
		private GameObject privateObject;

		// Token: 0x04000D66 RID: 3430
		[SerializeField]
		private GameObject rankedObject;

		// Token: 0x04000D67 RID: 3431
		private string eloText = string.Empty;

		// Token: 0x04000D68 RID: 3432
		private string playersText = string.Empty;

		// Token: 0x04000D69 RID: 3433
		private string playersTimeText = string.Empty;

		// Token: 0x04000D6A RID: 3434
		private string promoCardsText = string.Empty;

		// Token: 0x04000D6B RID: 3435
		private string matChoicesText = string.Empty;

		// Token: 0x04000D6C RID: 3436
		private string possibleEloChangeText = string.Empty;

		// Token: 0x04000D6D RID: 3437
		private string playAndStayText = string.Empty;

		// Token: 0x04000D6E RID: 3438
		private string playAndGoText = string.Empty;

		// Token: 0x04000D6F RID: 3439
		private string synchronousTime = string.Empty;

		// Token: 0x04000D70 RID: 3440
		private string asynchronousTime = string.Empty;
	}
}
