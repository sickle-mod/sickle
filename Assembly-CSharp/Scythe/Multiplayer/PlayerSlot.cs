using System;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.Multiplayer.Data;
using Scythe.Utils.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x0200024D RID: 589
	public class PlayerSlot : MonoBehaviour
	{
		// Token: 0x1400007D RID: 125
		// (add) Token: 0x060011CD RID: 4557 RVA: 0x000953F8 File Offset: 0x000935F8
		// (remove) Token: 0x060011CE RID: 4558 RVA: 0x00095430 File Offset: 0x00093630
		public event Action<Bot> AddBotSuccess;

		// Token: 0x1400007E RID: 126
		// (add) Token: 0x060011CF RID: 4559 RVA: 0x00095468 File Offset: 0x00093668
		// (remove) Token: 0x060011D0 RID: 4560 RVA: 0x000954A0 File Offset: 0x000936A0
		public event Action<Exception> AddBotError;

		// Token: 0x1400007F RID: 127
		// (add) Token: 0x060011D1 RID: 4561 RVA: 0x000954D8 File Offset: 0x000936D8
		// (remove) Token: 0x060011D2 RID: 4562 RVA: 0x00095510 File Offset: 0x00093710
		public event Action<Bot> RemoveBotSuccess;

		// Token: 0x14000080 RID: 128
		// (add) Token: 0x060011D3 RID: 4563 RVA: 0x00095548 File Offset: 0x00093748
		// (remove) Token: 0x060011D4 RID: 4564 RVA: 0x00095580 File Offset: 0x00093780
		public event global::System.Action ChoosingMatsSuccess;

		// Token: 0x060011D5 RID: 4565 RVA: 0x000955B8 File Offset: 0x000937B8
		public void DestroySlot()
		{
			this.AddBotSuccess = null;
			this.AddBotError = null;
			this.RemoveBotSuccess = null;
			this.ChoosingMatsSuccess = null;
			this.timer.OnTimePassed -= this.OnTimePassed;
			base.transform.SetParent(null, false);
			global::UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x000337C0 File Offset: 0x000319C0
		public Guid GetPlayerId()
		{
			if (this.empty)
			{
				return Guid.Empty;
			}
			if (this.takenByBot)
			{
				return Guid.Empty.BotGuid();
			}
			return this.playerData.PlayerStats.Id;
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x000337F3 File Offset: 0x000319F3
		public bool IsEmpty()
		{
			return this.empty;
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x000337FB File Offset: 0x000319FB
		public bool IsAvailable()
		{
			return this.IsEmpty() && !this.waitingForResponse;
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x00033810 File Offset: 0x00031A10
		public bool IsReady()
		{
			return this.empty || this.takenByBot || this.playerData.IsReady;
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x00033831 File Offset: 0x00031A31
		public int GetFaction()
		{
			return this.faction;
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x00033839 File Offset: 0x00031A39
		public int GetPlayerMat()
		{
			return this.playerMat;
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x00033841 File Offset: 0x00031A41
		public bool TakenByDLCPlayer()
		{
			if (this.empty)
			{
				return false;
			}
			if (this.takenByBot)
			{
				return this.ifa;
			}
			return this.playerData.DLC;
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x00033867 File Offset: 0x00031A67
		public bool TakenByBot()
		{
			return this.takenByBot;
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x0003386F File Offset: 0x00031A6F
		public void UpdateSlot(int slot)
		{
			this.slot = slot;
			if (!this.empty)
			{
				if (this.takenByBot)
				{
					this.botData.Slot = slot;
					return;
				}
				this.playerData.Slot = slot;
			}
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x00095610 File Offset: 0x00093810
		public void SetRemovePlayerButtonState(bool state)
		{
			if (state)
			{
				this.removePlayerButton.onClick.RemoveAllListeners();
				if (this.takenByBot)
				{
					this.removePlayerButton.onClick.AddListener(new UnityAction(this.OnRemoveBotClick));
				}
				else if (!this.empty)
				{
					this.removePlayerButton.onClick.AddListener(new UnityAction(this.RemovePlayer));
				}
			}
			if (this.takenByBot)
			{
				this.changeBotDifficultyLeft.gameObject.SetActive(state);
				this.changeBotDifficultyRight.gameObject.SetActive(state);
			}
			this.removePlayerButton.gameObject.SetActive(state);
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x000338A1 File Offset: 0x00031AA1
		public int GetSlotIndex()
		{
			return this.slot;
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x000956B8 File Offset: 0x000938B8
		public void SetMatAndFaction(int faction, int mat)
		{
			this.faction = faction;
			this.playerMat = mat;
			TMP_Text tmp_Text = this.factionName;
			string text = "FactionMat/";
			Faction faction2 = (Faction)faction;
			tmp_Text.text = ScriptLocalization.Get(text + faction2.ToString());
			TMP_Text tmp_Text2 = this.matName;
			string text2 = "PlayerMat/";
			PlayerMatType playerMatType = (PlayerMatType)mat;
			tmp_Text2.text = ScriptLocalization.Get(text2 + playerMatType.ToString());
			this.factionLogo.sprite = this.factionLogos[faction];
			this.background.sprite = this.factionBackgrounds[faction];
			this.UpdateSpecialAbilityAvailability(faction);
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x00095754 File Offset: 0x00093954
		public void UpdateSpecialAbilityAvailability(int faction)
		{
			this.polaniaAbilityDescription.SetActive(false);
			this.crimeaAbilityDescription.SetActive(false);
			if (this.gameRoom.NumberOfPlayersChoosing > 5)
			{
				if (faction == 0)
				{
					this.polaniaAbilityDescription.SetActive(true);
					return;
				}
				if (faction == 5)
				{
					this.crimeaAbilityDescription.SetActive(true);
				}
			}
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x000957A8 File Offset: 0x000939A8
		private void SetFactionImages(int faction)
		{
			if (faction == 7)
			{
				this.factionName.text = this.randomText;
				if (this.ifa && this.playerData.DLC)
				{
					this.background.sprite = this.factionBackgrounds[8];
				}
				else
				{
					this.background.sprite = this.factionBackgrounds[7];
				}
			}
			else
			{
				TMP_Text tmp_Text = this.factionName;
				string text = "FactionMat/";
				Faction faction2 = (Faction)faction;
				tmp_Text.text = ScriptLocalization.Get(text + faction2.ToString());
				this.background.sprite = this.factionBackgrounds[faction];
			}
			this.factionLogo.sprite = this.factionLogos[faction];
			this.UpdateSpecialAbilityAvailability(faction);
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x00095860 File Offset: 0x00093A60
		private void SetMatImages(int mat)
		{
			if (mat == 7)
			{
				this.matName.text = this.randomText;
				return;
			}
			TMP_Text tmp_Text = this.matName;
			string text = "PlayerMat/";
			PlayerMatType playerMatType = (PlayerMatType)mat;
			tmp_Text.text = ScriptLocalization.Get(text + playerMatType.ToString());
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x000958AC File Offset: 0x00093AAC
		public void ActivateAsEmpty(int slot, bool ifa, GameRoom gameRoom)
		{
			this.randomText = ScriptLocalization.Get("MainMenu/RandomizeMats");
			this.timer.OnTimePassed += this.OnTimePassed;
			this.ifa = ifa;
			this.slot = slot;
			this.gameRoom = gameRoom;
			this.elo.gameObject.SetActive(false);
			this.ChangeToEmpty();
			base.gameObject.SetActive(true);
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x00095918 File Offset: 0x00093B18
		public void ChangeToEmpty()
		{
			this.changeBotDifficultyLeft.SetActive(false);
			this.changeBotDifficultyRight.SetActive(false);
			this.playerType.gameObject.SetActive(false);
			this.playerName.text = string.Empty;
			this.removePlayerButton.gameObject.SetActive(false);
			this.addBotPanel.SetActive(false);
			this.readyPanel.SetActive(false);
			this.factionLogo.gameObject.SetActive(false);
			this.factionName.gameObject.SetActive(false);
			this.changeFactionButtons.SetActive(false);
			this.matName.gameObject.SetActive(false);
			this.changeMatButtons.SetActive(false);
			this.polaniaAbilityDescription.SetActive(false);
			this.crimeaAbilityDescription.SetActive(false);
			this.timer.Deactivate();
			this.background.sprite = this.emptySlotSprite;
			this.takenByBot = false;
			this.empty = true;
			this.faction = -1;
			this.playerMat = -1;
			this.elo.gameObject.SetActive(false);
			this.hidingPanel.SetActive(true);
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x00095A40 File Offset: 0x00093C40
		public void RevertMatsChoices()
		{
			this.faction = -1;
			this.playerMat = -1;
			this.polaniaAbilityDescription.SetActive(false);
			this.crimeaAbilityDescription.SetActive(false);
			if (this.takenByBot)
			{
				this.ChangeToBot(this.botData);
				return;
			}
			if (!this.empty)
			{
				this.ChangeToPlayer(this.playerData);
			}
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x000338A9 File Offset: 0x00031AA9
		public void ActivateAddBotPanel()
		{
			this.addBotPanel.SetActive(true);
			this.hidingPanel.SetActive(false);
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x000338C3 File Offset: 0x00031AC3
		public void DisableAddBotPanel()
		{
			this.addBotPanel.SetActive(false);
			if (this.IsEmpty())
			{
				this.hidingPanel.SetActive(true);
			}
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x00095A9C File Offset: 0x00093C9C
		public void OnAddBotClick(int difficulty)
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			string text = HotseatPanel.GenerateAIPlayerName(this.slot, difficulty);
			this.botData = new Bot(this.slot, difficulty, text);
			this.ChangeToBot(this.botData);
			this.SendAddBotRequest(this.botData);
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x00095AF0 File Offset: 0x00093CF0
		public void ChangeToBot(Bot botData)
		{
			this.botData = botData;
			this.takenByBot = true;
			this.empty = false;
			this.playerName.text = botData.Name;
			this.playerType.gameObject.SetActive(true);
			this.playerType.sprite = this.playerTypesImages[botData.Difficulty];
			this.factionName.gameObject.SetActive(true);
			this.factionName.text = this.randomText;
			this.matName.gameObject.SetActive(true);
			this.matName.text = this.randomText;
			this.factionLogo.gameObject.SetActive(true);
			this.factionLogo.sprite = this.factionLogos[7];
			if (this.ifa)
			{
				this.background.sprite = this.factionBackgrounds[8];
			}
			else
			{
				this.background.sprite = this.factionBackgrounds[7];
			}
			this.addBotPanel.SetActive(false);
			this.hidingPanel.SetActive(false);
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x00095BFC File Offset: 0x00093DFC
		private void SendAddBotRequest(Bot botData)
		{
			this.waitingForResponse = true;
			LobbyRestAPI.AddBot(botData, delegate(string response)
			{
				this.waitingForResponse = false;
				if (this.AddBotSuccess != null)
				{
					this.AddBotSuccess(botData);
				}
				this.removePlayerButton.gameObject.SetActive(true);
				this.removePlayerButton.onClick.RemoveAllListeners();
				this.removePlayerButton.onClick.AddListener(new UnityAction(this.OnRemoveBotClick));
				this.UpdateBotDifficultyButtons();
			}, delegate(Exception error)
			{
				this.waitingForResponse = false;
				if (this.takenByBot)
				{
					this.ChangeToEmpty();
				}
				if (this.AddBotError != null)
				{
					this.AddBotError(error);
				}
				Debug.LogError(error);
			});
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x000338E5 File Offset: 0x00031AE5
		public void OnRemoveBotClick()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.ChangeToEmpty();
			this.SendRemoveBotRequest();
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x000338FF File Offset: 0x00031AFF
		private void SendRemoveBotRequest()
		{
			this.waitingForResponse = true;
			LobbyRestAPI.RemoveBot(this.botData, delegate(string response)
			{
				this.waitingForResponse = false;
				if (this.RemoveBotSuccess != null)
				{
					this.RemoveBotSuccess(this.botData);
				}
				this.botData = null;
				this.changeBotDifficultyLeft.gameObject.SetActive(false);
				this.changeBotDifficultyRight.gameObject.SetActive(false);
			}, delegate(Exception error)
			{
				this.waitingForResponse = false;
				if (this.takenByBot)
				{
					this.ChangeToBot(this.botData);
					return;
				}
				this.botData = null;
			});
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x00095C48 File Offset: 0x00093E48
		public void OnChangeBotDifficulty(int change)
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.botData.Difficulty += change;
			string text = HotseatPanel.GenerateAIPlayerName(this.slot, this.botData.Difficulty);
			this.botData.Name = text;
			this.playerName.text = text;
			this.playerType.sprite = this.playerTypesImages[this.botData.Difficulty];
			this.changeBotDifficultyLeft.gameObject.SetActive(false);
			this.changeBotDifficultyRight.gameObject.SetActive(false);
			this.SendUpdateBotRequest(this.botData, change);
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x0003392B File Offset: 0x00031B2B
		private void UpdateBotDifficultyButtons()
		{
			this.changeBotDifficultyLeft.gameObject.SetActive(this.botData.Difficulty > 1);
			this.changeBotDifficultyRight.gameObject.SetActive(this.botData.Difficulty < 3);
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x00095CF0 File Offset: 0x00093EF0
		private void SendUpdateBotRequest(Bot newData, int difficultyChange)
		{
			this.waitingForResponse = true;
			LobbyRestAPI.UpdateBot(newData, delegate(string response)
			{
				this.waitingForResponse = false;
				this.UpdateBotDifficultyButtons();
			}, delegate(Exception error)
			{
				this.waitingForResponse = false;
				this.botData.Difficulty -= difficultyChange;
				string text = HotseatPanel.GenerateAIPlayerName(this.slot, this.botData.Difficulty);
				this.botData.Name = text;
				this.playerName.text = text;
				this.UpdateBotDifficultyButtons();
			});
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x00095D38 File Offset: 0x00093F38
		public void ChangeToPlayer(PlayerInfo player)
		{
			this.takenByBot = false;
			this.empty = false;
			this.playerData = player;
			this.playerName.text = player.PlayerStats.Name;
			this.playerType.gameObject.SetActive(true);
			this.playerType.sprite = this.playerTypesImages[0];
			this.factionName.gameObject.SetActive(true);
			this.factionName.text = this.randomText;
			this.matName.gameObject.SetActive(true);
			this.matName.text = this.randomText;
			this.factionLogo.gameObject.SetActive(true);
			this.factionLogo.sprite = this.factionLogos[7];
			this.changeFactionButtons.SetActive(false);
			this.changeMatButtons.SetActive(false);
			if (this.ifa && player.DLC)
			{
				this.background.sprite = this.factionBackgrounds[8];
			}
			else
			{
				this.background.sprite = this.factionBackgrounds[7];
			}
			if (PlayerInfo.me.IsAdmin && player != PlayerInfo.me)
			{
				this.removePlayerButton.gameObject.SetActive(true);
				this.removePlayerButton.onClick.RemoveAllListeners();
				this.removePlayerButton.onClick.AddListener(new UnityAction(this.RemovePlayer));
			}
			this.elo.gameObject.SetActive(true);
			this.elo.text = string.Format("{0}: {1}", ScriptLocalization.Get("Lobby/Elo"), player.PlayerStats.ELO);
			this.hidingPanel.gameObject.SetActive(false);
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x00033969 File Offset: 0x00031B69
		public void ChangeReadyState(bool newState)
		{
			this.readyPanel.SetActive(newState);
			if (!this.takenByBot)
			{
				this.playerData.IsReady = newState;
			}
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x0003398B File Offset: 0x00031B8B
		public void DisableReadyPanel()
		{
			this.readyPanel.SetActive(false);
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x00095EF0 File Offset: 0x000940F0
		private void RemovePlayer()
		{
			this.waitingForResponse = true;
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.removePlayerButton.gameObject.SetActive(false);
			LobbyRestAPI.RemovePlayer(this.playerData.PlayerStats.Id, delegate(string response)
			{
				this.waitingForResponse = false;
			}, delegate(Exception error)
			{
				this.waitingForResponse = false;
				this.removePlayerButton.gameObject.SetActive(true);
			});
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x00033999 File Offset: 0x00031B99
		public void EnableChoosingPanel(MatAndFactionChoose matAndFactionChoose)
		{
			this.matAndFactionChoose = matAndFactionChoose;
			this.changeFactionButtons.SetActive(true);
			this.changeMatButtons.SetActive(true);
			this.EnableTimer();
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x000339C0 File Offset: 0x00031BC0
		public void EnableTimer()
		{
			this.timer.Activate();
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x000339CD File Offset: 0x00031BCD
		public void DeactivateTimer()
		{
			this.timer.Deactivate();
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x000339DA File Offset: 0x00031BDA
		public void NextFaction()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.matAndFactionChoose.MoveToNextFaction();
			this.SetFactionImages(this.matAndFactionChoose.GetCurrentFaction());
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x00033A04 File Offset: 0x00031C04
		public void PreviousFaction()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.matAndFactionChoose.MoveToPreviousFaction();
			this.SetFactionImages(this.matAndFactionChoose.GetCurrentFaction());
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x00033A2E File Offset: 0x00031C2E
		public void NextPlayerMat()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.matAndFactionChoose.MoveToNextMat();
			this.SetMatImages(this.matAndFactionChoose.GetCurrentPlayerMat());
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x00033A58 File Offset: 0x00031C58
		public void PreviousPlayerMat()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.matAndFactionChoose.MoveToPreviousMat();
			this.SetMatImages(this.matAndFactionChoose.GetCurrentPlayerMat());
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x00095F50 File Offset: 0x00094150
		public void EndSetting()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.changeFactionButtons.SetActive(false);
			this.changeMatButtons.SetActive(false);
			this.DeactivateTimer();
			int num = this.matAndFactionChoose.GetCurrentFaction();
			if (num == 7)
			{
				this.matAndFactionChoose.SetRandomFaction();
				num = this.matAndFactionChoose.GetCurrentFaction();
			}
			int num2 = this.matAndFactionChoose.GetCurrentPlayerMat();
			if (num2 == 7)
			{
				this.matAndFactionChoose.SetRandomMat();
				num2 = this.matAndFactionChoose.GetCurrentPlayerMat();
			}
			this.SetMatAndFaction(num, num2);
			if (this.ChoosingMatsSuccess != null)
			{
				this.ChoosingMatsSuccess();
			}
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x00095FF0 File Offset: 0x000941F0
		private void OnTimePassed()
		{
			if (this.playerData.PlayerStats.Id == PlayerInfo.me.PlayerStats.Id)
			{
				this.EndSetting();
				return;
			}
			LobbyRestAPI.CheckForLeavers(delegate(string s)
			{
				Debug.Log("CheckForLeavers Success");
			}, delegate(Exception exception)
			{
				Debug.Log("CheckForLeavers exception " + ((exception != null) ? exception.ToString() : null));
			});
		}

		// Token: 0x04000DBB RID: 3515
		[SerializeField]
		private GameObject changeBotDifficultyLeft;

		// Token: 0x04000DBC RID: 3516
		[SerializeField]
		private GameObject changeBotDifficultyRight;

		// Token: 0x04000DBD RID: 3517
		[SerializeField]
		private Image playerType;

		// Token: 0x04000DBE RID: 3518
		[SerializeField]
		private TextMeshProUGUI playerName;

		// Token: 0x04000DBF RID: 3519
		[SerializeField]
		private Button removePlayerButton;

		// Token: 0x04000DC0 RID: 3520
		[SerializeField]
		private Sprite[] playerTypesImages;

		// Token: 0x04000DC1 RID: 3521
		[SerializeField]
		private GameObject addBotPanel;

		// Token: 0x04000DC2 RID: 3522
		[SerializeField]
		private GameObject readyPanel;

		// Token: 0x04000DC3 RID: 3523
		[SerializeField]
		private Image factionLogo;

		// Token: 0x04000DC4 RID: 3524
		[SerializeField]
		private TextMeshProUGUI factionName;

		// Token: 0x04000DC5 RID: 3525
		[SerializeField]
		private GameObject changeFactionButtons;

		// Token: 0x04000DC6 RID: 3526
		[SerializeField]
		private Sprite[] factionLogos;

		// Token: 0x04000DC7 RID: 3527
		[SerializeField]
		private TextMeshProUGUI matName;

		// Token: 0x04000DC8 RID: 3528
		[SerializeField]
		private GameObject changeMatButtons;

		// Token: 0x04000DC9 RID: 3529
		[SerializeField]
		private GameObject hidingPanel;

		// Token: 0x04000DCA RID: 3530
		[SerializeField]
		private Image background;

		// Token: 0x04000DCB RID: 3531
		[SerializeField]
		private Sprite[] factionBackgrounds;

		// Token: 0x04000DCC RID: 3532
		[SerializeField]
		private Sprite emptySlotSprite;

		// Token: 0x04000DCD RID: 3533
		[SerializeField]
		private GameObject polaniaAbilityDescription;

		// Token: 0x04000DCE RID: 3534
		[SerializeField]
		private GameObject crimeaAbilityDescription;

		// Token: 0x04000DCF RID: 3535
		[SerializeField]
		private MatChoiceTimer timer;

		// Token: 0x04000DD0 RID: 3536
		[SerializeField]
		private TextMeshProUGUI elo;

		// Token: 0x04000DD1 RID: 3537
		private string randomText = string.Empty;

		// Token: 0x04000DD2 RID: 3538
		private GameRoom gameRoom;

		// Token: 0x04000DD3 RID: 3539
		private bool ifa;

		// Token: 0x04000DD4 RID: 3540
		private int slot = -1;

		// Token: 0x04000DD5 RID: 3541
		private bool takenByBot;

		// Token: 0x04000DD6 RID: 3542
		private bool empty = true;

		// Token: 0x04000DD7 RID: 3543
		private bool waitingForResponse;

		// Token: 0x04000DD8 RID: 3544
		private Bot botData;

		// Token: 0x04000DD9 RID: 3545
		private PlayerInfo playerData;

		// Token: 0x04000DDA RID: 3546
		private int faction = -1;

		// Token: 0x04000DDB RID: 3547
		private int playerMat = -1;

		// Token: 0x04000DDC RID: 3548
		private MatAndFactionChoose matAndFactionChoose;
	}
}
