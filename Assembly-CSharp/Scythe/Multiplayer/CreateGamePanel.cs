using System;
using I2.Loc;
using Scythe.Analytics;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x0200021F RID: 543
	public class CreateGamePanel : MonoBehaviour
	{
		// Token: 0x06001012 RID: 4114 RVA: 0x0003256F File Offset: 0x0003076F
		private void Start()
		{
			this.synchronousDescription = ScriptLocalization.Get("Lobby/Play&StayInfo");
			this.asynchronousDescription = ScriptLocalization.Get("Lobby/Play&GoInfo");
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x00032591 File Offset: 0x00030791
		public virtual void Activate()
		{
			base.gameObject.SetActive(true);
			if (PlayerInfo.me.CurrentLobbyRoom != null)
			{
				this.ActivateBackToRoomPanel();
				return;
			}
			this.ActivateStepOne();
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x000325B8 File Offset: 0x000307B8
		private void ActivateBackToRoomPanel()
		{
			this.backToRoomPanel.SetActive(true);
			this.firstPart.SetActive(false);
			this.secondPart.SetActive(false);
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0008F164 File Offset: 0x0008D364
		private void ActivateStepOne()
		{
			this.backToRoomPanel.SetActive(false);
			this.firstPart.SetActive(true);
			this.privateGameCheckmark.isOn = false;
			this.matsChoicesCheckmark.isOn = false;
			this.secondPart.SetActive(false);
			this.CheckDLCs();
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x000325DE File Offset: 0x000307DE
		private void CheckDLCs()
		{
			this.ifaCheckmark.isOn = GameServiceController.Instance.InvadersFromAfarUnlocked();
			this.ifaSettings.gameObject.SetActive(GameServiceController.Instance.InvadersFromAfarUnlocked());
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0003260F File Offset: 0x0003080F
		public void ApplyProfanityFilter(string name)
		{
			this.gameName.text = Censor.Process(name);
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x0008F1B4 File Offset: 0x0008D3B4
		public void ChangeGameType(bool asynchronous)
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			if (asynchronous)
			{
				this.playGoText.font = this.activeFont;
				this.playStayText.font = this.deactiveFont;
				this.descriptionPart1.text = this.asynchronousDescription;
				this.settings.transform.SetParent(this.asynchronousContent);
				return;
			}
			this.playStayText.font = this.activeFont;
			this.playGoText.font = this.deactiveFont;
			this.descriptionPart1.text = this.synchronousDescription;
			this.settings.transform.SetParent(this.synchronousContent);
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0008F264 File Offset: 0x0008D464
		public void MoveToStepTwo()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.firstPart.gameObject.SetActive(false);
			this.secondPart.gameObject.SetActive(true);
			if (this.asynchronousToggle.isOn)
			{
				this.backgroundImage.sprite = this.asynchronousSprite;
				this.gameType.text = this.playGoText.text;
				this.descriptionPart2.text = this.asynchronousDescription;
			}
			else
			{
				this.backgroundImage.sprite = this.synchronousSprite;
				this.gameType.text = this.playStayText.text;
				this.descriptionPart2.text = this.synchronousDescription;
			}
			this.rankedObject.SetActive(this.rankedToggle.isOn);
			this.eloRange.text = string.Format("{0} - {1}", this.eloAmount.GetMin(), this.eloAmount.GetMax());
			this.ifaMarker.SetActive(this.ifaCheckmark.isOn);
			this.gameName.text = string.Format("{0} {1}", ScriptLocalization.Get("Common/Game"), PlayerInfo.me.PlayerStats.Name);
			this.playersAmount.Init(this.ifaCheckmark.isOn);
			this.playerTime.Init(this.asynchronousToggle.isOn);
			this.normalGameSettings.gameObject.SetActive(!this.rankedToggle.isOn);
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0008F3F8 File Offset: 0x0008D5F8
		public void CreateGame()
		{
			if (this.gameName.text == string.Empty)
			{
				return;
			}
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiChatButton);
			AnalyticsEventData.SetMatchLaunchMethod(MatchLaunchMethods.create);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_create_game_button);
			if (this.rankedToggle.isOn)
			{
				LobbyRoom lobbyRoom = new LobbyRoom(this.gameName.text, this.playersAmount.GetPlayersAmount(), this.playerTime.GetTimeInMinutes(), true, this.eloAmount.GetMin(), this.eloAmount.GetMax(), true, this.asynchronousToggle.isOn, false, this.ifaCheckmark.isOn, true);
				this.lobby.CreateRoom(lobbyRoom);
				return;
			}
			LobbyRoom lobbyRoom2 = new LobbyRoom(this.gameName.text, this.playersAmount.GetPlayersAmount(), this.playerTime.GetTimeInMinutes(), false, 0, 0, this.promoCardsCheckmark.isOn, this.asynchronousToggle.isOn, this.privateGameCheckmark.isOn, this.ifaCheckmark.isOn, !this.matsChoicesCheckmark.isOn);
			this.lobby.CreateRoom(lobbyRoom2);
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x00032622 File Offset: 0x00030822
		public void BackToStepOne()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.firstPart.SetActive(true);
			this.secondPart.SetActive(false);
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x00029172 File Offset: 0x00027372
		public virtual void Deactivate()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x04000C50 RID: 3152
		[SerializeField]
		protected Lobby lobby;

		// Token: 0x04000C51 RID: 3153
		[SerializeField]
		private GameObject firstPart;

		// Token: 0x04000C52 RID: 3154
		[SerializeField]
		private Toggle asynchronousToggle;

		// Token: 0x04000C53 RID: 3155
		[SerializeField]
		private TextMeshProUGUI playStayText;

		// Token: 0x04000C54 RID: 3156
		[SerializeField]
		private TextMeshProUGUI playGoText;

		// Token: 0x04000C55 RID: 3157
		[SerializeField]
		private TMP_FontAsset activeFont;

		// Token: 0x04000C56 RID: 3158
		[SerializeField]
		private TMP_FontAsset deactiveFont;

		// Token: 0x04000C57 RID: 3159
		[SerializeField]
		private TextMeshProUGUI descriptionPart1;

		// Token: 0x04000C58 RID: 3160
		private string synchronousDescription = string.Empty;

		// Token: 0x04000C59 RID: 3161
		private string asynchronousDescription = string.Empty;

		// Token: 0x04000C5A RID: 3162
		[SerializeField]
		private Toggle rankedToggle;

		// Token: 0x04000C5B RID: 3163
		[SerializeField]
		private ELOAmount eloAmount;

		// Token: 0x04000C5C RID: 3164
		[SerializeField]
		private Toggle ifaCheckmark;

		// Token: 0x04000C5D RID: 3165
		[SerializeField]
		private GameObject ifaSettings;

		// Token: 0x04000C5E RID: 3166
		[SerializeField]
		private Transform synchronousContent;

		// Token: 0x04000C5F RID: 3167
		[SerializeField]
		private Transform asynchronousContent;

		// Token: 0x04000C60 RID: 3168
		[SerializeField]
		private GameObject settings;

		// Token: 0x04000C61 RID: 3169
		[SerializeField]
		private GameObject secondPart;

		// Token: 0x04000C62 RID: 3170
		[SerializeField]
		private TextMeshProUGUI gameType;

		// Token: 0x04000C63 RID: 3171
		[SerializeField]
		private GameObject rankedObject;

		// Token: 0x04000C64 RID: 3172
		[SerializeField]
		private TextMeshProUGUI eloRange;

		// Token: 0x04000C65 RID: 3173
		[SerializeField]
		private TextMeshProUGUI descriptionPart2;

		// Token: 0x04000C66 RID: 3174
		[SerializeField]
		private GameObject ifaMarker;

		// Token: 0x04000C67 RID: 3175
		[SerializeField]
		private Image backgroundImage;

		// Token: 0x04000C68 RID: 3176
		[SerializeField]
		private Sprite synchronousSprite;

		// Token: 0x04000C69 RID: 3177
		[SerializeField]
		private Sprite asynchronousSprite;

		// Token: 0x04000C6A RID: 3178
		[SerializeField]
		private TMP_InputField gameName;

		// Token: 0x04000C6B RID: 3179
		[SerializeField]
		private PlayersAmount playersAmount;

		// Token: 0x04000C6C RID: 3180
		[SerializeField]
		private PlayerTime playerTime;

		// Token: 0x04000C6D RID: 3181
		[SerializeField]
		private Toggle promoCardsCheckmark;

		// Token: 0x04000C6E RID: 3182
		[SerializeField]
		private GameObject normalGameSettings;

		// Token: 0x04000C6F RID: 3183
		[SerializeField]
		private Toggle matsChoicesCheckmark;

		// Token: 0x04000C70 RID: 3184
		[SerializeField]
		private Toggle privateGameCheckmark;

		// Token: 0x04000C71 RID: 3185
		[SerializeField]
		private GameObject backToRoomPanel;
	}
}
