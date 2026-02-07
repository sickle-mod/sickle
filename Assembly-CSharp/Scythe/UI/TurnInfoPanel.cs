using System;
using HoneyFramework;
using I2.Loc;
using Scythe.GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004C1 RID: 1217
	public class TurnInfoPanel : MonoBehaviour
	{
		// Token: 0x140000F2 RID: 242
		// (add) Token: 0x060026BF RID: 9919 RVA: 0x000E5F70 File Offset: 0x000E4170
		// (remove) Token: 0x060026C0 RID: 9920 RVA: 0x000E5FA8 File Offset: 0x000E41A8
		public event TurnInfoPanel.OnEndTurn OnEndTurnClick;

		// Token: 0x140000F3 RID: 243
		// (add) Token: 0x060026C1 RID: 9921 RVA: 0x000E5FE0 File Offset: 0x000E41E0
		// (remove) Token: 0x060026C2 RID: 9922 RVA: 0x000E6018 File Offset: 0x000E4218
		public event TurnInfoPanel.AnyKeyClicked OnAnyKeyClicked;

		// Token: 0x060026C3 RID: 9923 RVA: 0x00027EF0 File Offset: 0x000260F0
		private void OnEnable()
		{
		}

		// Token: 0x060026C4 RID: 9924 RVA: 0x00027EF0 File Offset: 0x000260F0
		private void OnDisable()
		{
		}

		// Token: 0x060026C5 RID: 9925 RVA: 0x000E6050 File Offset: 0x000E4250
		private void LateUpdate()
		{
			if (GameController.GameManager.PlayerCurrent != null && this.awaitingInputFromNextPlayer && this.timeStart != 0f && ((Input.anyKeyDown && (Time.time > this.timeStart + 0.5f || !GameController.GameManager.IsMultiplayer)) || (this.showTime != 0f && Time.time > this.timeStart + this.showTime)))
			{
				this.DeactivateTurnInfoPanel();
				GameController.Instance.combatPresenter.ShowInfoAboutUsedCombatAbility();
				if (this.OnAnyKeyClicked != null)
				{
					this.OnAnyKeyClicked();
				}
				if (GameController.GameManager.GetPlayersWithoutAICount() > 1 && !GameController.GameManager.IsMultiplayer)
				{
					if (PlatformManager.IsStandalone && this.aiInfoText != null)
					{
						this.aiInfoText.text = GameController.GameManager.PlayerCurrent.Name;
						return;
					}
					if (this.aiInfoTextMobile != null)
					{
						this.aiInfoTextMobile.text = GameController.GameManager.PlayerCurrent.Name;
					}
				}
			}
		}

		// Token: 0x060026C6 RID: 9926 RVA: 0x000E6170 File Offset: 0x000E4370
		public void ActivateTurnInfoPanel()
		{
			if (PlatformManager.IsMobile && GameController.GameManager.IsHotSeat && !GameController.GameManager.IsAIHotSeat && GameController.GameManager.PlayerCurrent.IsHuman)
			{
				this.SetupActualPlayerInfo(GameController.GameManager.PlayerCurrent, false);
				this.SetMobileTurnPanelForHumans(GameController.GameManager.PlayerCurrent.matFaction.faction);
			}
			else if (GameController.GameManager.IsMultiplayer || !GameController.GameManager.PlayerCurrent.IsHuman || GameController.GameManager.IsAIHotSeat)
			{
				this.SetActualPlayerInfo();
				this.EnablePressAnyKey(true);
				this.EnableAiInfo(true);
			}
			if ((GameController.GameManager.IsMultiplayer && GameController.GameManager.PlayerOwner == GameController.GameManager.PlayerCurrent) || (!GameController.GameManager.IsMultiplayer && GameController.GameManager.IsAIHotSeat))
			{
				this.awaitingInputFromNextPlayer = true;
			}
			this.timeStart = Time.time;
			base.gameObject.SetActive(true);
			this.DeactivateCombatEnemyActionInfo();
		}

		// Token: 0x060026C7 RID: 9927 RVA: 0x000E6278 File Offset: 0x000E4478
		public void ActivateTurnInfoPanelCombat(Player player)
		{
			if (GameController.GameManager.combatManager.GetAttacker() != null && GameController.GameManager.combatManager.GetAttacker().IsHuman && GameController.GameManager.combatManager.GetDefender() != null && GameController.GameManager.combatManager.GetDefender().IsHuman)
			{
				this.SetupActualPlayerInfo(player, false);
				if (PlatformManager.IsMobile && GameController.GameManager.IsHotSeat)
				{
					this.SetMobileTurnPanelForHumans(player.matFaction.faction);
				}
				this.awaitingInputFromNextPlayer = true;
				this.timeStart = Time.time;
				base.gameObject.SetActive(true);
			}
			this.DeactivateCombatEnemyActionInfo();
		}

		// Token: 0x060026C8 RID: 9928 RVA: 0x00040B36 File Offset: 0x0003ED36
		public void ActivateTurnInfoPanelNoInput()
		{
			this.SetActualPlayerInfo();
			if (this.pressAnyKey != null)
			{
				this.pressAnyKey.gameObject.SetActive(false);
			}
			this.awaitingInputFromNextPlayer = false;
			base.gameObject.SetActive(true);
			this.DeactivateCombatEnemyActionInfo();
		}

		// Token: 0x060026C9 RID: 9929 RVA: 0x00040B76 File Offset: 0x0003ED76
		public void ActivateTurnInfoPanelNoInput(Player player)
		{
			this.SetupActualPlayerInfo(player, true);
			this.EnablePressAnyKey(false);
			this.EnableAiInfo(true);
			this.awaitingInputFromNextPlayer = false;
			base.gameObject.SetActive(true);
			this.DeactivateCombatEnemyActionInfo();
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x000E6328 File Offset: 0x000E4528
		public void EnableEnemyActionInfo(Faction faction)
		{
			this.EnablePressAnyKey(false);
			this.EnableAiInfo(true);
			this.awaitingInputFromNextPlayer = false;
			base.gameObject.SetActive(true);
			if (this.aiInfoLogo != null)
			{
				this.aiInfoLogo.sprite = GameController.factionInfo[faction].logo;
			}
			this.SetInfoText(ScriptLocalization.Get("FactionMat/" + faction.ToString()));
			this.DeactivateCombatEnemyActionInfo();
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x00040BA7 File Offset: 0x0003EDA7
		public void DeactivateCombatEnemyActionInfo()
		{
			if (PlatformManager.IsMobile && GameController.Instance.combatPresenterMobile.combatEnemyActionInfo.IsActive())
			{
				GameController.Instance.combatPresenterMobile.combatEnemyActionInfo.Hide();
			}
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x00040BDA File Offset: 0x0003EDDA
		public void ConfirmButton_OnClick()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonBgGreenButton);
			this.DeactivateTurnInfoPanel();
		}

		// Token: 0x060026CD RID: 9933 RVA: 0x000E63A8 File Offset: 0x000E45A8
		public void DeactivateTurnInfoPanel()
		{
			CameraControler.CameraMovementBlocked = false;
			this.awaitingInputFromNextPlayer = false;
			this.timeStart = 0f;
			if (this.OnEndTurnClick != null)
			{
				this.OnEndTurnClick();
			}
			if (GameController.GameManager.combatManager.IsPlayerInCombat())
			{
				Player actualPlayer = GameController.GameManager.combatManager.GetActualPlayer();
				if (actualPlayer != null && actualPlayer.IsHuman)
				{
					GameController.Instance.playerStats.UpdateAllStats(actualPlayer, GameController.factionInfo[actualPlayer.matFaction.faction].logo);
				}
			}
			if (PlatformManager.IsMobile)
			{
				this.EnableBackground(false);
				this.EnableTurnInfoPlate(false);
				this.EnableAiInfo(false);
				this.EnableBigLogo(false);
				this.EnablePressAnyKey(false);
				this.EnableMobileConfirmButton(false);
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x060026CE RID: 9934 RVA: 0x000E6474 File Offset: 0x000E4674
		public void DisableEnemyActionInfo()
		{
			if (PlatformManager.IsMobile)
			{
				this.EnableBackground(false);
				this.EnableTurnInfoPlate(false);
				this.EnableAiInfo(false);
				this.EnableBigLogo(false);
				this.EnablePressAnyKey(false);
				this.EnableMobileConfirmButton(false);
			}
			this.EnableAiInfo(false);
			base.gameObject.SetActive(false);
		}

		// Token: 0x060026CF RID: 9935 RVA: 0x00040BEE File Offset: 0x0003EDEE
		private void EnableBackground(bool enable)
		{
			if (this.backgroundBlur != null)
			{
				this.backgroundBlur.gameObject.SetActive(enable);
			}
		}

		// Token: 0x060026D0 RID: 9936 RVA: 0x00040C0F File Offset: 0x0003EE0F
		private void EnablePressAnyKey(bool enable)
		{
			if (this.pressAnyKey != null)
			{
				this.pressAnyKey.gameObject.SetActive(enable);
			}
		}

		// Token: 0x060026D1 RID: 9937 RVA: 0x00040C30 File Offset: 0x0003EE30
		private void EnableAiInfo(bool enable)
		{
			if (this.aiInfo != null)
			{
				this.aiInfo.gameObject.SetActive(enable);
			}
		}

		// Token: 0x060026D2 RID: 9938 RVA: 0x00040C51 File Offset: 0x0003EE51
		private void EnableTurnInfoPlate(bool enable)
		{
			if (this.turnInfoPlate != null)
			{
				this.turnInfoPlate.gameObject.SetActive(enable);
			}
		}

		// Token: 0x060026D3 RID: 9939 RVA: 0x00040C72 File Offset: 0x0003EE72
		private void EnableBigLogo(bool enable)
		{
			if (this.bigLogo != null)
			{
				this.bigLogo.gameObject.SetActive(enable);
			}
		}

		// Token: 0x060026D4 RID: 9940 RVA: 0x00040C93 File Offset: 0x0003EE93
		private void EnableMobileConfirmButton(bool enable)
		{
			if (this.mobileConfirmButton != null)
			{
				this.mobileConfirmButton.gameObject.SetActive(enable);
			}
		}

		// Token: 0x060026D5 RID: 9941 RVA: 0x00040CB4 File Offset: 0x0003EEB4
		public void SetupActualActionAnimationName(string actionName)
		{
			this.aiInfoText.text = actionName;
		}

		// Token: 0x060026D6 RID: 9942 RVA: 0x000E64C8 File Offset: 0x000E46C8
		private void SetupActualPlayerInfo(Player playerAI, bool smallVersion)
		{
			if (smallVersion)
			{
				this.SetInfoText(ScriptLocalization.Get("FactionMat/" + playerAI.matFaction.faction.ToString()));
				if (this.aiInfoLogo != null)
				{
					this.aiInfoLogo.sprite = GameController.factionInfo[playerAI.matFaction.faction].logo;
					return;
				}
			}
			else
			{
				this.turnInfoBackground.color = GameController.factionInfo[playerAI.matFaction.faction].color;
				this.turnInfo.color = GameController.factionInfo[playerAI.matFaction.faction].colorAccent;
				this.turnInfo.text = ScriptLocalization.Get("FactionMat/" + playerAI.matFaction.faction.ToString());
				if (this.pressAnyKey != null)
				{
					this.pressAnyKey.color = GameController.factionInfo[playerAI.matFaction.faction].colorAccent;
				}
				if (this.aiTurn != null)
				{
					this.aiTurn.color = GameController.factionInfo[playerAI.matFaction.faction].colorAccent;
				}
				this.turnInfoLogo1.sprite = GameController.factionInfo[playerAI.matFaction.faction].logo;
				if (this.turnInfoLogo2 != null)
				{
					this.turnInfoLogo2.sprite = GameController.factionInfo[playerAI.matFaction.faction].logo;
				}
			}
		}

		// Token: 0x060026D7 RID: 9943 RVA: 0x000E6674 File Offset: 0x000E4874
		public void SetActualPlayerInfo()
		{
			if ((GameController.GameManager.IsMultiplayer && GameController.GameManager.PlayerCurrent == GameController.GameManager.PlayerOwner && !GameController.GameManager.PlayerCurrent.topActionFinished) || (!GameController.GameManager.IsMultiplayer && GameController.GameManager.PlayerCurrent.IsHuman))
			{
				ShowEnemyMoves.Instance.DisableAnimationVignette();
				if (!OptionsManager.IsFastForward())
				{
					ShowEnemyMoves.Instance.SetupNormalSpeed();
				}
				else if (!GameController.GameManager.IsCampaign)
				{
					ShowEnemyMoves.Instance.SetupFastSpeed();
				}
				if (!GameController.GameManager.IsMultiplayer && GameController.GameManager.PlayerCurrent.IsHuman && !GameController.GameManager.IsAIHotSeat && PlatformManager.IsMobile)
				{
					this.SetMobileTurnPanelForHumans(GameController.GameManager.PlayerCurrent.matFaction.faction);
				}
				else if (GameController.GameManager.GetPlayersWithoutAICount() > 1 && !GameController.GameManager.IsMultiplayer)
				{
					this.SetInfoText(GameController.GameManager.PlayerCurrent.Name);
				}
				else
				{
					string text = ScriptLocalization.Get("GameScene/YourTurn");
					this.SetInfoText(text);
				}
			}
			else
			{
				ShowEnemyMoves.Instance.EnableAnimationVignette();
				string text2 = ScriptLocalization.Get("FactionMat/" + GameController.GameManager.PlayerCurrent.matFaction.faction.ToString());
				this.SetInfoText(text2);
			}
			if (this.aiInfoLogo != null)
			{
				this.aiInfoLogo.sprite = GameController.factionInfo[GameController.GameManager.PlayerCurrent.matFaction.faction].logo;
			}
		}

		// Token: 0x060026D8 RID: 9944 RVA: 0x000E681C File Offset: 0x000E4A1C
		public void SetMobileTurnPanelForHumans(Faction faction)
		{
			AssetBundle assetBundle = AssetBundleManager.LoadAssetBundle("graphic_backgrounds");
			string text = "Background_Mat_Faction_" + faction.ToString();
			this.backgroundBlur.GetComponent<Image>().sprite = assetBundle.LoadAsset<Sprite>(text);
			this.EnableAiInfo(false);
			this.EnableBackground(true);
			this.EnableTurnInfoPlate(true);
			this.EnableBigLogo(true);
			this.EnablePressAnyKey(false);
			this.EnableMobileConfirmButton(true);
		}

		// Token: 0x060026D9 RID: 9945 RVA: 0x00040CC2 File Offset: 0x0003EEC2
		public void SetInfoText(string actionName)
		{
			if (PlatformManager.IsStandalone)
			{
				this.aiInfoText.text = actionName;
				return;
			}
			this.aiInfoTextMobile.text = actionName;
		}

		// Token: 0x060026DA RID: 9946 RVA: 0x000E6890 File Offset: 0x000E4A90
		private void LocalizeTexts()
		{
			if (GameController.GameManager.GetPlayersWithoutAICount() > 1 && !GameController.GameManager.IsMultiplayer)
			{
				if (PlatformManager.IsStandalone && this.aiInfoText != null)
				{
					this.aiInfoText.text = GameController.GameManager.PlayerCurrent.Name;
					return;
				}
				if (this.aiInfoTextMobile != null)
				{
					this.aiInfoTextMobile.text = GameController.GameManager.PlayerCurrent.Name;
				}
			}
		}

		// Token: 0x04001BBB RID: 7099
		public Text turnInfo;

		// Token: 0x04001BBC RID: 7100
		public Text pressAnyKey;

		// Token: 0x04001BBD RID: 7101
		public Text aiTurn;

		// Token: 0x04001BBE RID: 7102
		public Image turnInfoBackground;

		// Token: 0x04001BBF RID: 7103
		public Image turnInfoLogo1;

		// Token: 0x04001BC0 RID: 7104
		public Image turnInfoLogo2;

		// Token: 0x04001BC1 RID: 7105
		public Image aiInfoLogo;

		// Token: 0x04001BC2 RID: 7106
		public Text aiInfoText;

		// Token: 0x04001BC3 RID: 7107
		public TMP_Text aiInfoTextMobile;

		// Token: 0x04001BC4 RID: 7108
		public GameObject aiInfo;

		// Token: 0x04001BC5 RID: 7109
		public GameObject backgroundBlur;

		// Token: 0x04001BC6 RID: 7110
		public GameObject turnInfoPlate;

		// Token: 0x04001BC7 RID: 7111
		public GameObject bigLogo;

		// Token: 0x04001BC8 RID: 7112
		public Button mobileConfirmButton;

		// Token: 0x04001BC9 RID: 7113
		public float showTime;

		// Token: 0x04001BCC RID: 7116
		public bool awaitingInputFromNextPlayer;

		// Token: 0x04001BCD RID: 7117
		private float timeStart;

		// Token: 0x04001BCE RID: 7118
		private TurnInfoPanel.TurnInfoTextType currentTurnInfoType;

		// Token: 0x020004C2 RID: 1218
		private enum TurnInfoTextType
		{
			// Token: 0x04001BD0 RID: 7120
			ActionName,
			// Token: 0x04001BD1 RID: 7121
			PlayerName,
			// Token: 0x04001BD2 RID: 7122
			Nothing
		}

		// Token: 0x020004C3 RID: 1219
		// (Invoke) Token: 0x060026DD RID: 9949
		public delegate void OnEndTurn();

		// Token: 0x020004C4 RID: 1220
		// (Invoke) Token: 0x060026E1 RID: 9953
		public delegate void AnyKeyClicked();
	}
}
