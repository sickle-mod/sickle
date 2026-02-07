using System;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using Scythe.Multiplayer.Messages;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020003F5 RID: 1013
	public class PassCoinsPresenter : MonoBehaviour
	{
		// Token: 0x06001EA1 RID: 7841 RVA: 0x000BC554 File Offset: 0x000BA754
		private void Awake()
		{
			this.sliderWrapper = new SliderWrapper(this.slider);
			this.sliderWrapper.ValueChanged += this.UpdateUITexts;
			if (this.playerSelectionPanel != null)
			{
				this.playerSelectionPanel.PlayerSelected += this.OnPlayerSelected;
			}
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x0003BDB8 File Offset: 0x00039FB8
		private void OnEnable()
		{
			GameController.OnEndTurnClick += this.Dismiss;
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x0003BDCB File Offset: 0x00039FCB
		private void OnDisable()
		{
			GameController.OnEndTurnClick -= this.Dismiss;
		}

		// Token: 0x06001EA4 RID: 7844 RVA: 0x0003BDDE File Offset: 0x00039FDE
		public void OnUndo()
		{
			this.SetActive(false);
		}

		// Token: 0x06001EA5 RID: 7845 RVA: 0x0002920A File Offset: 0x0002740A
		public void SetActive(bool active)
		{
			base.gameObject.SetActive(active);
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x000BC5B0 File Offset: 0x000BA7B0
		public void Show()
		{
			if (this.CanBeShown())
			{
				this.SetActive(true);
				if (this.playerSelectionPanel != null)
				{
					this.playerSelectionPanel.Show();
				}
				if (this.playerSelectionPanel != null)
				{
					this.amountSelectionPanel.SetActive(false);
				}
			}
		}

		// Token: 0x06001EA7 RID: 7847 RVA: 0x000BC600 File Offset: 0x000BA800
		public void Show(int playerID)
		{
			Player player = GameController.GameManager.players[playerID];
			this.Show(player);
		}

		// Token: 0x06001EA8 RID: 7848 RVA: 0x000BC628 File Offset: 0x000BA828
		public void Show(Player chosenPlayer)
		{
			if (this.CanBeShown() && chosenPlayer != GameController.GameManager.PlayerMaster)
			{
				this.chosenPlayer = chosenPlayer;
				this.SetActive(true);
				if (this.playerSelectionPanel != null)
				{
					this.playerSelectionPanel.Hide();
				}
				if (this.amountSelectionPanel != null)
				{
					this.amountSelectionPanel.SetActive(true);
				}
				this.UpdatePresenter();
			}
		}

		// Token: 0x06001EA9 RID: 7849 RVA: 0x0003BDE7 File Offset: 0x00039FE7
		private bool CanBeShown()
		{
			return GameController.GameManager.PlayerCurrent.IsHuman && (!GameController.GameManager.IsMultiplayer || GameController.GameManager.PlayerOwner == GameController.GameManager.PlayerCurrent);
		}

		// Token: 0x06001EAA RID: 7850 RVA: 0x000BC694 File Offset: 0x000BA894
		private void UpdatePresenter()
		{
			this.sliderWrapper.MaxValue = GameController.GameManager.PlayerCurrent.Coins;
			this.sliderWrapper.Value = 0;
			this.coinsSupply.text = GameController.GameManager.PlayerCurrent.Coins.ToString();
			this.coinsToSend.text = "0";
			this.senderFactionEmblem.sprite = GameController.factionInfo[GameController.GameManager.PlayerCurrent.matFaction.faction].logo;
			this.receiverFactionEmblem.sprite = GameController.factionInfo[this.chosenPlayer.matFaction.faction].logo;
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x000BC754 File Offset: 0x000BA954
		public void AddCoin()
		{
			int num = int.Parse(this.coinsToSend.text);
			Player playerCurrent = GameController.GameManager.PlayerCurrent;
			if (num >= playerCurrent.Coins)
			{
				return;
			}
			num++;
			this.sliderWrapper.Value = num;
			this.UpdateUITexts();
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x000BC7AC File Offset: 0x000BA9AC
		public void AddAll()
		{
			int num = int.Parse(this.coinsToSend.text);
			Player playerCurrent = GameController.GameManager.PlayerCurrent;
			if (num == playerCurrent.Coins)
			{
				return;
			}
			num = playerCurrent.Coins;
			this.sliderWrapper.Value = num;
			this.UpdateUITexts();
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x000BC804 File Offset: 0x000BAA04
		public void RemoveCoin()
		{
			int num = int.Parse(this.coinsToSend.text);
			if (num == 0)
			{
				return;
			}
			Player playerCurrent = GameController.GameManager.PlayerCurrent;
			num--;
			this.sliderWrapper.Value = num;
			this.UpdateUITexts();
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x000BC854 File Offset: 0x000BAA54
		public void RemoveAll()
		{
			if (int.Parse(this.coinsToSend.text) == 0)
			{
				return;
			}
			Player playerCurrent = GameController.GameManager.PlayerCurrent;
			int num = 0;
			this.sliderWrapper.Value = num;
			this.UpdateUITexts();
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x0003BE20 File Offset: 0x0003A020
		public void ChangeSliderValue()
		{
			this.sliderWrapper.Value = (int)this.slider.value;
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x000BC8A4 File Offset: 0x000BAAA4
		private void UpdateUITexts()
		{
			Player playerCurrent = GameController.GameManager.PlayerCurrent;
			int value = this.sliderWrapper.Value;
			this.coinsSupply.text = (playerCurrent.Coins - value).ToString();
			this.coinsToSend.text = value.ToString();
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x0003BDDE File Offset: 0x00039FDE
		public void Dismiss()
		{
			this.SetActive(false);
		}

		// Token: 0x06001EB2 RID: 7858 RVA: 0x000BC8F8 File Offset: 0x000BAAF8
		public void Accept()
		{
			int num = int.Parse(this.coinsToSend.text);
			if (num == 0)
			{
				return;
			}
			Player playerCurrent = GameController.GameManager.PlayerCurrent;
			PassCoins.PassCoinsBetweenPlayers(GameController.GameManager, playerCurrent, this.chosenPlayer, num);
			if (GameController.GameManager.IsMultiplayer)
			{
				GameController.GameManager.OnActionSent(new PassCoinsMessage((int)playerCurrent.matFaction.faction, (int)this.chosenPlayer.matFaction.faction, num));
			}
			GameController.Instance.UpdateStats(true, false);
			this.SetActive(false);
			this.chosenPlayer = null;
		}

		// Token: 0x06001EB3 RID: 7859 RVA: 0x0003BE39 File Offset: 0x0003A039
		private void OnPlayerSelected(Player player)
		{
			this.Show(player);
		}

		// Token: 0x040015B0 RID: 5552
		[SerializeField]
		private Text coinsSupply;

		// Token: 0x040015B1 RID: 5553
		[SerializeField]
		private Text coinsToSend;

		// Token: 0x040015B2 RID: 5554
		[SerializeField]
		private Slider slider;

		// Token: 0x040015B3 RID: 5555
		[SerializeField]
		private Image senderFactionEmblem;

		// Token: 0x040015B4 RID: 5556
		[SerializeField]
		private Image receiverFactionEmblem;

		// Token: 0x040015B5 RID: 5557
		[SerializeField]
		private PlayerSelectionPanel playerSelectionPanel;

		// Token: 0x040015B6 RID: 5558
		[SerializeField]
		private GameObject amountSelectionPanel;

		// Token: 0x040015B7 RID: 5559
		private Player chosenPlayer;

		// Token: 0x040015B8 RID: 5560
		private SliderWrapper sliderWrapper;
	}
}
