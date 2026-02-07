using System;
using System.Collections.Generic;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x0200046B RID: 1131
	public class AutomaticGainPresenter : MonoBehaviour
	{
		// Token: 0x060023AA RID: 9130 RVA: 0x0003ECFA File Offset: 0x0003CEFA
		private void OnEnable()
		{
			this.UpdateToggles();
		}

		// Token: 0x060023AB RID: 9131 RVA: 0x00029172 File Offset: 0x00027372
		public void Close()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x060023AC RID: 9132 RVA: 0x000D34AC File Offset: 0x000D16AC
		public void UpdateToggles()
		{
			Dictionary<GainType, bool> automaticGain = this.GetPlayer().automaticGain;
			this.GainPowerToggle.isOn = !automaticGain[GainType.Power];
			this.GainCoinsToggle.isOn = !automaticGain[GainType.Coin];
			this.GainPopularityToggle.isOn = !automaticGain[GainType.Popularity];
			this.GainCombatCardsToggle.isOn = !automaticGain[GainType.CombatCard];
			if (!PlatformManager.IsStandalone && this.AutomaticGainsToggle != null)
			{
				this.AutomaticGainsToggle.isOn = automaticGain[GainType.Power];
				this.AutomaticGainsToggle.isOn = automaticGain[GainType.Coin];
				this.AutomaticGainsToggle.isOn = automaticGain[GainType.Popularity];
				this.AutomaticGainsToggle.isOn = automaticGain[GainType.CombatCard];
			}
		}

		// Token: 0x060023AD RID: 9133 RVA: 0x000D3578 File Offset: 0x000D1778
		public void LoadPreferences()
		{
			if (GameController.GameManager.IsMultiplayer)
			{
				Player player = this.GetPlayer();
				this.LoadPreferences(player);
				return;
			}
			if (!GameController.GameManager.IsCampaign)
			{
				foreach (Player player2 in GameController.GameManager.GetPlayersWithoutAI())
				{
					this.LoadPreferences(player2);
				}
			}
		}

		// Token: 0x060023AE RID: 9134 RVA: 0x000D35F0 File Offset: 0x000D17F0
		public void LoadPreferences(Player player)
		{
			player.automaticGain[GainType.Power] = PlayerPrefs.GetInt("AutoGainPower", 1) == 1;
			player.automaticGain[GainType.Coin] = PlayerPrefs.GetInt("AutoGainCoin", 1) == 1;
			player.automaticGain[GainType.Popularity] = PlayerPrefs.GetInt("AutoGainPopularity", 1) == 1;
			player.automaticGain[GainType.CombatCard] = PlayerPrefs.GetInt("AutoGainAmmo", 1) == 1;
		}

		// Token: 0x060023AF RID: 9135 RVA: 0x000D3668 File Offset: 0x000D1868
		public void SavePreferences(Player player)
		{
			if (this.isWelcomePage || GameController.GameManager.IsMultiplayer || GameController.GameManager.GetPlayersWithoutAICount() == 1)
			{
				PlayerPrefs.SetInt("AutoGainPower", player.automaticGain[GainType.Power] ? 1 : 0);
				PlayerPrefs.SetInt("AutoGainCoin", player.automaticGain[GainType.Coin] ? 1 : 0);
				PlayerPrefs.SetInt("AutoGainPopularity", player.automaticGain[GainType.Popularity] ? 1 : 0);
				PlayerPrefs.SetInt("AutoGainAmmo", player.automaticGain[GainType.CombatCard] ? 1 : 0);
			}
		}

		// Token: 0x060023B0 RID: 9136 RVA: 0x000D3708 File Offset: 0x000D1908
		private Player GetPlayer()
		{
			Player player = null;
			if (GameController.GameManager != null)
			{
				if (GameController.GameManager.IsMultiplayer)
				{
					player = GameController.GameManager.PlayerOwner;
				}
				else
				{
					player = GameController.GameManager.PlayerCurrent;
				}
			}
			if (player == null)
			{
				player = new Player(null);
				this.LoadPreferences(player);
			}
			return player;
		}

		// Token: 0x060023B1 RID: 9137 RVA: 0x000D3754 File Offset: 0x000D1954
		public void OnPowerClicked()
		{
			Player player = this.GetPlayer();
			player.automaticGain[GainType.Power] = !this.GainPowerToggle.isOn;
			this.SavePreferences(player);
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
		}

		// Token: 0x060023B2 RID: 9138 RVA: 0x000D3798 File Offset: 0x000D1998
		public void OnCoinsClicked()
		{
			Player player = this.GetPlayer();
			player.automaticGain[GainType.Coin] = !this.GainCoinsToggle.isOn;
			this.SavePreferences(player);
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x000D37DC File Offset: 0x000D19DC
		public void OnPopularityClicked()
		{
			Player player = this.GetPlayer();
			player.automaticGain[GainType.Popularity] = !this.GainPopularityToggle.isOn;
			this.SavePreferences(player);
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x000D3820 File Offset: 0x000D1A20
		public void OnCombatCardsClicked()
		{
			Player player = this.GetPlayer();
			player.automaticGain[GainType.CombatCard] = !this.GainCombatCardsToggle.isOn;
			this.SavePreferences(player);
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x000D3864 File Offset: 0x000D1A64
		public void OnAcceptGainsClicked()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV2);
			Player player = this.GetPlayer();
			player.automaticGain[GainType.Power] = this.AutomaticGainsToggle.isOn;
			player.automaticGain[GainType.Coin] = this.AutomaticGainsToggle.isOn;
			player.automaticGain[GainType.Popularity] = this.AutomaticGainsToggle.isOn;
			player.automaticGain[GainType.CombatCard] = this.AutomaticGainsToggle.isOn;
			this.SavePreferences(player);
		}

		// Token: 0x040018CD RID: 6349
		public Toggle GainPowerToggle;

		// Token: 0x040018CE RID: 6350
		public Toggle GainCoinsToggle;

		// Token: 0x040018CF RID: 6351
		public Toggle GainPopularityToggle;

		// Token: 0x040018D0 RID: 6352
		public Toggle GainCombatCardsToggle;

		// Token: 0x040018D1 RID: 6353
		public Toggle AutomaticGainsToggle;

		// Token: 0x040018D2 RID: 6354
		public bool isWelcomePage;
	}
}
