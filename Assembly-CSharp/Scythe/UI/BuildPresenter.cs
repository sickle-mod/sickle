using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using HoneyFramework;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020003D3 RID: 979
	public class BuildPresenter : ActionPresenter
	{
		// Token: 0x140000B7 RID: 183
		// (add) Token: 0x06001CBF RID: 7359 RVA: 0x000B2AC8 File Offset: 0x000B0CC8
		// (remove) Token: 0x06001CC0 RID: 7360 RVA: 0x000B2AFC File Offset: 0x000B0CFC
		public static event BuildPresenter.BuildStart BuildStarted;

		// Token: 0x140000B8 RID: 184
		// (add) Token: 0x06001CC1 RID: 7361 RVA: 0x000B2B30 File Offset: 0x000B0D30
		// (remove) Token: 0x06001CC2 RID: 7362 RVA: 0x000B2B64 File Offset: 0x000B0D64
		public static event BuildPresenter.BuildEnd BuildEnded;

		// Token: 0x140000B9 RID: 185
		// (add) Token: 0x06001CC3 RID: 7363 RVA: 0x000B2B98 File Offset: 0x000B0D98
		// (remove) Token: 0x06001CC4 RID: 7364 RVA: 0x000B2BCC File Offset: 0x000B0DCC
		public static event BuildPresenter.BuildSelect BuildingSelected;

		// Token: 0x06001CC5 RID: 7365 RVA: 0x000B2C00 File Offset: 0x000B0E00
		private void Awake()
		{
			if (!PlatformManager.IsStandalone)
			{
				this.originalBuildSelectionSizeDelta = this.buildWindowRect.sizeDelta;
				this.originalBuildWindowAnchoredPosition = this.buildWindowRect.anchoredPosition;
				for (int i = 0; i < this.closeInfoButtons.Length; i++)
				{
					this.closeInfoButtons[i].onClick.AddListener(new UnityAction(this.OnCloseInformationButtonClicked));
				}
				this.informationButton.onClick.AddListener(new UnityAction(this.OnInformationButtonClicked));
			}
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x0003AC83 File Offset: 0x00038E83
		private void Update()
		{
			if (PlatformManager.IsMobile && (Input.anyKeyDown || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)) && this.buildWindowAnimation != null)
			{
				this.buildWindowAnimation.Complete(true);
			}
		}

		// Token: 0x06001CC7 RID: 7367 RVA: 0x000B2C84 File Offset: 0x000B0E84
		public override void ChangeLayoutForAction(BaseAction action)
		{
			this.action = action as GainBuilding;
			this.sectionId = -1;
			this.hexesShown = false;
			this.availableHexes.Clear();
			GameController.HexSelectionMode = GameController.SelectionMode.Build;
			if (this.guideline != null)
			{
				this.guideline.SetActive(true);
			}
			GameController.Instance.matFaction.ClearHintStories();
			if (PlatformManager.IsStandalone)
			{
				this.hintAction.SetActive(true);
			}
			else
			{
				SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Hidden, 0.25f);
				if (OptionsManager.IsConfirmActions())
				{
					this.AnimateBuildSelectionPanel();
				}
				this.buildWindowMobile.SetActive(true);
			}
			this.EnableInput();
			if (BuildPresenter.BuildStarted != null)
			{
				BuildPresenter.BuildStarted();
			}
			this.AssistHighlight(1);
			if (PlatformManager.IsStandalone && OptionsManager.IsActionAssist())
			{
				GameController.Instance.MaximizePlayerMat();
			}
		}

		// Token: 0x06001CC8 RID: 7368 RVA: 0x000B2D5C File Offset: 0x000B0F5C
		public void AssistHighlight(int step)
		{
			for (int i = 0; i < this.assistIcon.Length; i++)
			{
				this.assistHighlight[i].SetActive(i == step);
				this.assistIcon[i].color = ((i == step) ? Color.white : Color.black);
			}
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x000B2DAC File Offset: 0x000B0FAC
		private void AnimateBuildSelectionPanel()
		{
			Vector2 vector = new Vector2(this.buildWindowRect.parent.GetComponent<RectTransform>().rect.width / 2f, this.buildWindowRect.parent.GetComponent<RectTransform>().rect.height / 2f);
			this.SetButtonsInteractable(false);
			this.buildWindowRect.anchoredPosition = vector;
			this.buildWindowRect.localScale = Vector3.zero;
			Canvas.ForceUpdateCanvases();
			this.buildWindowAnimation = DOTween.Sequence();
			this.buildWindowAnimation.Append(this.buildWindowRect.DOScale(1f, this.popupAnimationSpeed).SetEase(Ease.InOutCubic));
			this.buildWindowAnimation.Join(this.buildWindowRect.DOAnchorPos(this.originalBuildWindowAnchoredPosition, this.popupAnimationSpeed, false).SetEase(Ease.InOutCubic));
			this.buildWindowAnimation.OnComplete(delegate
			{
				this.OnAnimationComplete();
			});
			this.buildWindowAnimation.Play<Sequence>();
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x0003ACC1 File Offset: 0x00038EC1
		private void OnAnimationComplete()
		{
			this.SetButtonsInteractable(true);
			this.buildWindowAnimation = null;
		}

		// Token: 0x06001CCB RID: 7371 RVA: 0x000B2EB4 File Offset: 0x000B10B4
		private void SetButtonsInteractable(bool interactable)
		{
			for (int i = 0; i < this.buttonsMobile.Length; i++)
			{
				this.buttonsMobile[i].interactable = interactable;
			}
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x000B2EE4 File Offset: 0x000B10E4
		private void EnableInput()
		{
			for (int i = 0; i < 4; i++)
			{
				if (!GameController.GameManager.PlayerCurrent.matPlayer.buildings.Contains(GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(i).ActionTop.Structure))
				{
					MatPlayerSectionPresenter matPlayerSectionPresenter = GameController.Instance.matPlayer.matSection[i];
					matPlayerSectionPresenter.sectionGlass.enabled = false;
					if (PlatformManager.IsStandalone)
					{
						matPlayerSectionPresenter.SetSectionCooldown(false, false);
					}
					matPlayerSectionPresenter.topActionPresenter.buildingButton.image.color = this.glowDefault1;
					matPlayerSectionPresenter.topActionPresenter.buildingButton.gameObject.SetActive(true);
					matPlayerSectionPresenter.topActionPresenter.buildingButton.GetComponent<Animator>().StopPlayback();
					matPlayerSectionPresenter.topActionPresenter.buildingSpecific.material = null;
					if (PlatformManager.IsStandalone && matPlayerSectionPresenter.topActionPresenter.guideline != null)
					{
						matPlayerSectionPresenter.topActionPresenter.guideline.enabled = true;
					}
				}
			}
			if (PlatformManager.IsMobile)
			{
				base.EnableMapBlackout();
				this.RefreshButtonsMobile();
			}
			PlayerUnits.ChangeUnitsColliderState(false);
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x0003ACD1 File Offset: 0x00038ED1
		public void OnMillClick()
		{
			this.AfterBuildingClick(BuildingType.Mill);
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x0003ACDA File Offset: 0x00038EDA
		public void OnArmoryClick()
		{
			this.AfterBuildingClick(BuildingType.Armory);
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x0003ACE3 File Offset: 0x00038EE3
		public void OnMineClick()
		{
			this.AfterBuildingClick(BuildingType.Mine);
		}

		// Token: 0x06001CD0 RID: 7376 RVA: 0x0003ACEC File Offset: 0x00038EEC
		public void OnMonumentClick()
		{
			this.AfterBuildingClick(BuildingType.Monument);
		}

		// Token: 0x06001CD1 RID: 7377 RVA: 0x000B300C File Offset: 0x000B120C
		private void RefreshButtonsMobile()
		{
			for (int i = 0; i < this.buttonsMobile.Length; i++)
			{
				bool flag = GameController.GameManager.PlayerCurrent.matPlayer.GetBuilding((BuildingType)i) == null;
				bool flag2 = this.sectionId != -1 && this.lastBuildingType == (BuildingType)i;
				this.buttonsMobile[i].interactable = flag;
				this.buttonHighlightsMobile[i].enabled = flag2;
				this.buttonGlowsMobile[i].enabled = flag && !flag2;
			}
		}

		// Token: 0x06001CD2 RID: 7378 RVA: 0x000B3090 File Offset: 0x000B1290
		private void AfterBuildingClick(BuildingType buildingType)
		{
			if (!this.hexesShown)
			{
				this.FindAllHexes();
				GameController.HexGetFocused += this.OnHexSelected;
				if (BuildPresenter.BuildingSelected != null)
				{
					BuildPresenter.BuildingSelected();
				}
				this.hexesShown = true;
			}
			switch (buildingType)
			{
			case BuildingType.Mine:
				WorldSFXManager.PlaySound(SoundEnum.PlayersBoardChooseToBuildMine, AudioSourceType.WorldSfx);
				break;
			case BuildingType.Monument:
				WorldSFXManager.PlaySound(SoundEnum.PlayersBoardChooseToBuildMonument, AudioSourceType.WorldSfx);
				break;
			case BuildingType.Armory:
				WorldSFXManager.PlaySound(SoundEnum.PlayersBoardChooseToBuildArmory, AudioSourceType.WorldSfx);
				break;
			case BuildingType.Mill:
				WorldSFXManager.PlaySound(SoundEnum.PlayersBoardChooseToBuildMill, AudioSourceType.WorldSfx);
				break;
			}
			for (int i = 0; i < 4; i++)
			{
				MatPlayerSectionPresenter matPlayerSectionPresenter = GameController.Instance.matPlayer.matSection[i];
				if (GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(i).ActionTop.Structure.buildingType == buildingType)
				{
					this.sectionId = i;
					matPlayerSectionPresenter.topActionPresenter.buildingButton.image.color = this.glowSelected1;
					if (this.hintsVisible)
					{
						GameController.Instance.matFaction.ShowBuildingHint(buildingType, this.sectionId, true);
						if (!PlatformManager.IsStandalone)
						{
							this.informationButton.gameObject.SetActive(false);
							this.buildText.gameObject.SetActive(false);
						}
					}
					else if (!this.hintsVisible && !PlatformManager.IsStandalone)
					{
						this.informationButton.gameObject.SetActive(true);
						this.buildText.gameObject.SetActive(true);
					}
				}
				else
				{
					matPlayerSectionPresenter.topActionPresenter.buildingButton.image.color = this.glowDefault1;
				}
			}
			this.lastBuildingType = buildingType;
			this.AssistHighlight(2);
			if (OptionsManager.IsActionAssist())
			{
				if (PlatformManager.IsStandalone)
				{
					GameController.Instance.MinimizePlayerMat();
				}
				if (this.availableHexes.Count == 1)
				{
					if (OptionsManager.IsCameraAnimationsActive())
					{
						ShowEnemyMoves.Instance.AnimateCamToHex(this.availableHexes.ToList<Scythe.BoardPresenter.GameHexPresenter>()[0].GetWorldPosition());
					}
				}
				else if (this.availableHexes.Count > 1)
				{
					List<Vector3> list = new List<Vector3>();
					HashSet<GameHex> hashSet = new HashSet<GameHex>();
					foreach (Scythe.BoardPresenter.GameHexPresenter gameHexPresenter in this.availableHexes)
					{
						list.Add(gameHexPresenter.GetWorldPosition());
						hashSet.Add(gameHexPresenter.GetGameHexLogic());
					}
					Vector3 vector = AnimateCamera.Instance.CalculateCenterOfHexes(list);
					float num = AnimateCamera.Instance.CalculateZoomToShowGivenHexes(hashSet);
					if (OptionsManager.IsCameraAnimationsActive())
					{
						ShowEnemyMoves.Instance.AnimateCamToShowAllHexes(vector, num);
					}
				}
			}
			if (PlatformManager.IsMobile)
			{
				this.RefreshButtonsMobile();
			}
		}

		// Token: 0x06001CD3 RID: 7379 RVA: 0x000B3344 File Offset: 0x000B1544
		private void OnInformationButtonClicked()
		{
			this.hintsVisible = true;
			for (int i = 0; i < 4; i++)
			{
				MatPlayerSectionPresenter matPlayerSectionPresenter = GameController.Instance.matPlayer.matSection[i];
				if (GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(i).ActionTop.Structure.buildingType == this.lastBuildingType)
				{
					this.sectionId = i;
					matPlayerSectionPresenter.topActionPresenter.buildingButton.image.color = this.glowSelected1;
					GameController.Instance.matFaction.ShowBuildingHint(this.lastBuildingType, this.sectionId, true);
				}
				else
				{
					matPlayerSectionPresenter.topActionPresenter.buildingButton.image.color = this.glowDefault1;
				}
			}
			if (!PlatformManager.IsStandalone)
			{
				this.buildText.gameObject.SetActive(false);
				this.informationButton.gameObject.SetActive(false);
			}
			for (int j = 0; j < this.closeInfoButtons.Length; j++)
			{
				this.closeInfoButtons[j].gameObject.SetActive(true);
			}
		}

		// Token: 0x06001CD4 RID: 7380 RVA: 0x000B3458 File Offset: 0x000B1658
		private void OnCloseInformationButtonClicked()
		{
			this.hintsVisible = false;
			GameController.Instance.matFaction.BuildingTypeHideHints();
			if (!PlatformManager.IsStandalone)
			{
				this.buildText.gameObject.SetActive(true);
				this.informationButton.gameObject.SetActive(true);
			}
			for (int i = 0; i < this.closeInfoButtons.Length; i++)
			{
				this.closeInfoButtons[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x06001CD5 RID: 7381 RVA: 0x000B34CC File Offset: 0x000B16CC
		public void OnHexSelected(Scythe.BoardPresenter.GameHexPresenter hex)
		{
			if (this.availableHexes.Contains(hex))
			{
				Building structure = GameController.GameManager.PlayerCurrent.matPlayer.GetPlayerMatSection(this.sectionId).ActionTop.Structure;
				if (this.action.SetStructureAndLocation(structure, hex.GetGameHexLogic()))
				{
					hex.building.GetComponent<BuildingPresenter>().LaunchSpawnAnimation(this.action.Structure, hex, this.action.GetPlayer().matFaction.faction);
					AchievementManager.UpdateAchievementMine(this.action.Structure, hex.GetGameHexLogic());
					this.OnActionEnded();
				}
			}
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x000B3570 File Offset: 0x000B1770
		private void FindAllHexes()
		{
			GameController.ClearFocus();
			if (this.action.IsEncounter && GameController.GameManager.PlayerCurrent.character.position.Building == null)
			{
				Scythe.BoardPresenter.GameHexPresenter gameHexPresenter = GameController.Instance.GetGameHexPresenter(GameController.GameManager.PlayerCurrent.character.position);
				gameHexPresenter.SetFocus(true, HexMarkers.MarkerType.DeployTrade, 1f, false);
				this.availableHexes.Add(gameHexPresenter);
			}
			else
			{
				foreach (Worker worker in GameController.GameManager.PlayerCurrent.matPlayer.workers)
				{
					Scythe.BoardPresenter.GameHexPresenter gameHexPresenter2 = GameController.Instance.GetGameHexPresenter(worker.position.posX, worker.position.posY);
					if (!this.availableHexes.Contains(gameHexPresenter2) && worker.position.Building == null && worker.position.hexType != HexType.lake && worker.position.hexType != HexType.capital)
					{
						gameHexPresenter2.SetFocus(true, HexMarkers.MarkerType.DeployTrade, 1f, false);
						this.availableHexes.Add(gameHexPresenter2);
					}
				}
			}
			GameController.Instance.hexPointerController.SetHexesWithResource(this.availableHexes.ToList<Scythe.BoardPresenter.GameHexPresenter>());
		}

		// Token: 0x06001CD7 RID: 7383 RVA: 0x0003ACF5 File Offset: 0x00038EF5
		public HashSet<Scythe.BoardPresenter.GameHexPresenter> GetAvaliableHexes()
		{
			return this.availableHexes;
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x0003ACFD File Offset: 0x00038EFD
		public bool HaveAction()
		{
			return this.action != null;
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x0003AD08 File Offset: 0x00038F08
		public void EnableVisuals(bool enabled)
		{
			this.EnableMarkers(enabled);
		}

		// Token: 0x06001CDA RID: 7386 RVA: 0x000B36C8 File Offset: 0x000B18C8
		private void EnableMarkers(bool enabled)
		{
			foreach (Scythe.BoardPresenter.GameHexPresenter gameHexPresenter in this.availableHexes)
			{
				gameHexPresenter.SetFocus(enabled, HexMarkers.MarkerType.DeployTrade, 0f, false);
			}
		}

		// Token: 0x06001CDB RID: 7387 RVA: 0x0003AD11 File Offset: 0x00038F11
		public void OnEndBuildButtonClicked()
		{
			GameController.Instance.PopupWindowsBeforeNextTurn();
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x000B3720 File Offset: 0x000B1920
		public override void OnActionEnded()
		{
			PlayerUnits.ChangeUnitsColliderState(true);
			this.Clear();
			if (BuildPresenter.BuildEnded != null)
			{
				BuildPresenter.BuildEnded();
			}
			this.DisableInput();
			HumanInputHandler.Instance.OnInputEnded();
			this.action = null;
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x000B3770 File Offset: 0x000B1970
		private void DisableInput()
		{
			GameController.Instance.matFaction.ClearHintStories();
			foreach (Scythe.BoardPresenter.GameHexPresenter gameHexPresenter in this.availableHexes)
			{
				gameHexPresenter.SetFocus(false, HexMarkers.MarkerType.DeployTrade, 0.3f, false);
			}
			GameController.HexGetFocused -= this.OnHexSelected;
			GameController.HexSelectionMode = GameController.SelectionMode.Normal;
			base.DisableMapBlackout();
			for (int i = 0; i < 4; i++)
			{
				MatPlayerSectionPresenter matPlayerSectionPresenter = GameController.Instance.matPlayer.matSection[i];
				matPlayerSectionPresenter.topActionPresenter.buildingButton.image.color = this.glowDefault1;
				matPlayerSectionPresenter.topActionPresenter.buildingButton.gameObject.SetActive(false);
				if (PlatformManager.IsStandalone && matPlayerSectionPresenter.topActionPresenter.guideline != null)
				{
					matPlayerSectionPresenter.topActionPresenter.guideline.enabled = false;
				}
			}
			if (this.sectionId != -1)
			{
				GameController.Instance.matPlayer.matSection[this.sectionId].topActionPresenter.buildingButton.interactable = true;
			}
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x000B38A8 File Offset: 0x000B1AA8
		public override void Clear()
		{
			if (this.guideline != null)
			{
				this.guideline.SetActive(false);
			}
			GameObject[] array = this.assistHighlight;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
			if (!PlatformManager.IsStandalone)
			{
				this.buildWindowMobile.SetActive(false);
			}
			GameController.Instance.hexPointerController.Clear();
			GameController.ClearFocus();
			GameController.Instance.cameraControler.HooverReset();
		}

		// Token: 0x040014C1 RID: 5313
		public GameObject guideline;

		// Token: 0x040014C2 RID: 5314
		public GameObject hintAction;

		// Token: 0x040014C3 RID: 5315
		public GameObject[] assistHighlight;

		// Token: 0x040014C4 RID: 5316
		public Image[] assistIcon;

		// Token: 0x040014C5 RID: 5317
		public Color glowSelected1;

		// Token: 0x040014C6 RID: 5318
		public Color glowDefault1;

		// Token: 0x040014C7 RID: 5319
		public GameObject buildWindowMobile;

		// Token: 0x040014C8 RID: 5320
		public Button[] buttonsMobile;

		// Token: 0x040014C9 RID: 5321
		public Image[] buttonHighlightsMobile;

		// Token: 0x040014CA RID: 5322
		public Image[] buttonGlowsMobile;

		// Token: 0x040014CB RID: 5323
		public TMP_Text buildText;

		// Token: 0x040014CC RID: 5324
		public Button informationButton;

		// Token: 0x040014CD RID: 5325
		public Button[] closeInfoButtons;

		// Token: 0x040014CE RID: 5326
		private bool hintsVisible;

		// Token: 0x040014CF RID: 5327
		private bool hexesShown;

		// Token: 0x040014D0 RID: 5328
		private GainBuilding action;

		// Token: 0x040014D1 RID: 5329
		private int sectionId = -1;

		// Token: 0x040014D2 RID: 5330
		private HashSet<Scythe.BoardPresenter.GameHexPresenter> availableHexes = new HashSet<Scythe.BoardPresenter.GameHexPresenter>();

		// Token: 0x040014D6 RID: 5334
		public BuildingType lastBuildingType;

		// Token: 0x040014D7 RID: 5335
		[SerializeField]
		private RectTransform buildWindowRect;

		// Token: 0x040014D8 RID: 5336
		[SerializeField]
		private float popupAnimationSpeed = 0.5f;

		// Token: 0x040014D9 RID: 5337
		private Vector2 originalBuildSelectionSizeDelta;

		// Token: 0x040014DA RID: 5338
		private Vector2 originalBuildWindowAnchoredPosition;

		// Token: 0x040014DB RID: 5339
		private Sequence buildWindowAnimation;

		// Token: 0x020003D4 RID: 980
		// (Invoke) Token: 0x06001CE2 RID: 7394
		public delegate void BuildStart();

		// Token: 0x020003D5 RID: 981
		// (Invoke) Token: 0x06001CE6 RID: 7398
		public delegate void BuildEnd();

		// Token: 0x020003D6 RID: 982
		// (Invoke) Token: 0x06001CEA RID: 7402
		public delegate void BuildSelect();
	}
}
