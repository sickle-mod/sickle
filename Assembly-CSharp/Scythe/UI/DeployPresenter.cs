using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using HoneyFramework;
using I2.Loc;
using Scythe.Analytics;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020003DA RID: 986
	public class DeployPresenter : ActionPresenter
	{
		// Token: 0x140000BC RID: 188
		// (add) Token: 0x06001D3C RID: 7484 RVA: 0x000B5B00 File Offset: 0x000B3D00
		// (remove) Token: 0x06001D3D RID: 7485 RVA: 0x000B5B34 File Offset: 0x000B3D34
		public static event DeployPresenter.DeployEnd DeployEnded;

		// Token: 0x06001D3E RID: 7486 RVA: 0x0003B028 File Offset: 0x00039228
		public void Awake()
		{
			if (!PlatformManager.IsStandalone)
			{
				this.originalDeploySelectionSizeDelta = this.deployWindowRect.sizeDelta;
				this.originalDeployWindowAnchoredPosition = this.deployWindowRect.anchoredPosition;
			}
		}

		// Token: 0x06001D3F RID: 7487 RVA: 0x0003B053 File Offset: 0x00039253
		private void Update()
		{
			if (PlatformManager.IsMobile && (Input.anyKeyDown || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)) && this.deployWindowAnimation != null)
			{
				this.deployWindowAnimation.Complete(true);
			}
		}

		// Token: 0x06001D40 RID: 7488 RVA: 0x000B5B68 File Offset: 0x000B3D68
		public override void ChangeLayoutForAction(BaseAction action)
		{
			this.action = action as GainMech;
			this.skillIndex = -1;
			this.availableHexes.Clear();
			GameController.HexSelectionMode = GameController.SelectionMode.Deploy;
			GameController.Instance.matFaction.ClearHintStories();
			if (PlatformManager.IsStandalone)
			{
				this.hintAction.SetActive(true);
			}
			else
			{
				SingletonMono<BottomBar>.Instance.AnimateToState(BottomBar.BottomBarState.Hidden, 0.25f);
			}
			this.EnableInput();
			base.EnableMapBlackout();
			this.AssistHighlight(1);
		}

		// Token: 0x06001D41 RID: 7489 RVA: 0x000B5BE0 File Offset: 0x000B3DE0
		public void AssistHighlight(int step)
		{
			for (int i = 0; i < this.assistIcon.Length; i++)
			{
				this.assistHighlight[i].SetActive(i == step);
				this.assistIcon[i].color = ((i == step) ? Color.white : Color.black);
			}
		}

		// Token: 0x06001D42 RID: 7490 RVA: 0x000B5C30 File Offset: 0x000B3E30
		private void AnimateDeploySelectionPanel()
		{
			Vector2 vector = new Vector2(this.deployWindowRect.parent.GetComponent<RectTransform>().rect.width / 2f, this.deployWindowRect.parent.GetComponent<RectTransform>().rect.height / 2f);
			this.SetButtonsInteractable(false);
			this.deployWindowRect.anchoredPosition = vector;
			this.deployWindowRect.localScale = Vector3.zero;
			Canvas.ForceUpdateCanvases();
			this.deployWindowAnimation = DOTween.Sequence();
			this.deployWindowAnimation.Append(this.deployWindowRect.DOScale(1f, this.popupAnimationSpeed).SetEase(Ease.InOutCubic));
			this.deployWindowAnimation.Join(this.deployWindowRect.DOAnchorPos(this.originalDeployWindowAnchoredPosition, this.popupAnimationSpeed, false).SetEase(Ease.InOutCubic));
			this.deployWindowAnimation.OnComplete(delegate
			{
				this.OnAnimationComplete();
			});
			this.deployWindowAnimation.Play<Sequence>();
		}

		// Token: 0x06001D43 RID: 7491 RVA: 0x000B5D38 File Offset: 0x000B3F38
		private void EnableInput()
		{
			this.deployStep = DeployPresenter.DeployStep.selectMech;
			if (!this.openedEarlier && PlatformManager.IsStandalone)
			{
				AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
				AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.tab_mech, Screens.in_game, Contexts.ingame);
			}
			for (int i = 0; i < 4; i++)
			{
				GameController.Instance.matFaction.mechGlowMobileShort[i].color = (GameController.Instance.matFaction.mechGlow[i].color = this.HighlightDefault);
				GameController.Instance.matFaction.mechButtonMobileShort[i].interactable = (GameController.Instance.matFaction.mechGlowMobileShort[i].enabled = (GameController.Instance.matFaction.mechButton[i].interactable = (GameController.Instance.matFaction.mechGlow[i].enabled = !GameController.GameManager.PlayerCurrent.matFaction.SkillUnlocked[i])));
				GameController.Instance.matFaction.mechGlow[i].GetComponent<Animator>().StopPlayback();
				Graphic graphic = GameController.Instance.matFaction.mechAbilityTitlesMobileShort[i];
				Color color = (GameController.Instance.matFaction.mechAbilityTitles[i].color = GameController.Instance.matFaction.MechTitleColorActive);
				graphic.color = color;
				if (GameController.GameManager.PlayerCurrent.matFaction.SkillUnlocked[i])
				{
					Graphic graphic2 = GameController.Instance.matFaction.mechAbilityIconsMobileShort[i];
					Graphic graphic3 = GameController.Instance.matFaction.mechAbilityIcons[i];
					color = new Color(1f, 1f, 1f, 0.8f);
					graphic3.color = color;
					graphic2.color = color;
					GameController.Instance.matFaction.mechAbilityIconsMobileShort[i].material = (GameController.Instance.matFaction.mechAbilityIcons[i].material = null);
				}
				else
				{
					Graphic graphic4 = GameController.Instance.matFaction.mechAbilityIconsMobileShort[i];
					Graphic graphic5 = GameController.Instance.matFaction.mechAbilityIcons[i];
					color = new Color(0.4375f, 0.4375f, 0.4375f, 1f);
					graphic5.color = color;
					graphic4.color = color;
					GameController.Instance.matFaction.mechAbilityIconsMobileShort[i].material = (GameController.Instance.matFaction.mechAbilityIcons[i].material = GameController.Instance.matFaction.sepiaUI);
				}
				GameController.Instance.matFaction.mechAbilityDescriptions[i].color = GameController.Instance.matFaction.MechDescriptionColorActive;
				if (!PlatformManager.IsStandalone)
				{
					GameController.Instance.matFaction.mechButton[i].gameObject.SetActive(false);
					GameController.Instance.matFaction.mechButtonMobileShort[i].gameObject.SetActive(true);
				}
			}
			if (PlatformManager.IsStandalone)
			{
				this.openedEarlier = this.mechToggle.isOn;
				this.mechToggle.isOn = true;
			}
			else
			{
				this.mechSelectionMobile.SetActive(true);
				if (OptionsManager.IsConfirmActions())
				{
					this.AnimateDeploySelectionPanel();
				}
			}
			PlayerUnits.ChangeUnitsColliderState(false);
		}

		// Token: 0x06001D44 RID: 7492 RVA: 0x0003B091 File Offset: 0x00039291
		private void OnAnimationComplete()
		{
			this.SetButtonsInteractable(true);
			this.deployWindowAnimation = null;
		}

		// Token: 0x06001D45 RID: 7493 RVA: 0x000B605C File Offset: 0x000B425C
		private void SetButtonsInteractable(bool interactable)
		{
			for (int i = 0; i < GameController.Instance.matFaction.mechButton.Length; i++)
			{
				bool flag = interactable && !GameController.GameManager.PlayerCurrent.matFaction.SkillUnlocked[i];
				GameController.Instance.matFaction.mechButton[i].interactable = flag;
				GameController.Instance.matFaction.mechButtonMobileShort[i].interactable = flag;
			}
		}

		// Token: 0x06001D46 RID: 7494 RVA: 0x000B60D4 File Offset: 0x000B42D4
		public bool OnMechSelected(int index)
		{
			if (this.deployStep == DeployPresenter.DeployStep.selectMech || this.deployStep == DeployPresenter.DeployStep.selectField)
			{
				this.skillIndex = index;
				for (int i = 0; i < 4; i++)
				{
					if (i != index)
					{
						GameController.Instance.matFaction.mechGlowMobileShort[i].color = (GameController.Instance.matFaction.mechGlow[i].color = this.HighlightDefault);
						GameController.Instance.matFaction.mechGlowMobileShort[i].enabled = (GameController.Instance.matFaction.mechGlow[i].enabled = !GameController.GameManager.PlayerCurrent.matFaction.SkillUnlocked[i]);
					}
					else
					{
						GameController.Instance.matFaction.mechGlowMobileShort[i].color = (GameController.Instance.matFaction.mechGlow[i].color = this.HighlightSelected);
					}
				}
				if (this.deployStep == DeployPresenter.DeployStep.selectMech)
				{
					this.FindAllHexes();
					GameController.HexGetFocused += this.OnHexSelected;
					this.AssistHighlight(2);
					if (OptionsManager.IsActionAssist())
					{
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
				}
				if (this.deployWindowAnimation != null)
				{
					this.deployWindowAnimation.Complete(true);
					this.deployWindowAnimation = null;
				}
				this.deployStep = DeployPresenter.DeployStep.selectField;
				return true;
			}
			return false;
		}

		// Token: 0x06001D47 RID: 7495 RVA: 0x000B6314 File Offset: 0x000B4514
		public void OnHexSelected(Scythe.BoardPresenter.GameHexPresenter hex)
		{
			if (this.deployStep == DeployPresenter.DeployStep.selectField && this.availableHexes.Contains(hex))
			{
				this.action.SetMechAndLocation(new Mech(GameController.GameManager, GameController.GameManager.PlayerCurrent, 1), hex.GetGameHexLogic(), this.skillIndex);
				this.OnActionEnded();
			}
		}

		// Token: 0x06001D48 RID: 7496 RVA: 0x0002BA43 File Offset: 0x00029C43
		private Scythe.BoardPresenter.GameHexPresenter GetGameHexPresenter(int x, int y)
		{
			return GameController.Instance.gameBoardPresenter.GetGameHexPresenter(x, y);
		}

		// Token: 0x06001D49 RID: 7497 RVA: 0x000B636C File Offset: 0x000B456C
		private void FindAllHexes()
		{
			GameController.ClearFocus();
			if (this.action.IsEncounter)
			{
				FlatGameHexPresenter flatGameHexPresenter = (FlatGameHexPresenter)GameController.Instance.GetGameHexPresenter(GameController.GameManager.PlayerCurrent.character.position);
				this.HighlightHex(flatGameHexPresenter);
			}
			else
			{
				foreach (Worker worker in GameController.GameManager.PlayerCurrent.matPlayer.workers)
				{
					FlatGameHexPresenter flatGameHexPresenter2 = (FlatGameHexPresenter)GameController.Instance.GetGameHexPresenter(worker.position.posX, worker.position.posY);
					if (!this.availableHexes.Contains(flatGameHexPresenter2) && flatGameHexPresenter2.hexType != HexType.lake && flatGameHexPresenter2.hexType != HexType.capital)
					{
						this.HighlightHex(flatGameHexPresenter2);
					}
				}
			}
			GameController.Instance.hexPointerController.SetHexesWithResource(this.availableHexes.ToList<Scythe.BoardPresenter.GameHexPresenter>());
			if (this.lastActionContainer != null)
			{
				this.lastActionContainer.SetActive(true);
				this.lastActionText.text = ScriptLocalization.Get("GameScene/PlaceYourMech");
			}
		}

		// Token: 0x06001D4A RID: 7498 RVA: 0x000B64A0 File Offset: 0x000B46A0
		private void HighlightHex(FlatGameHexPresenter hex)
		{
			hex.SetFocus(true, HexMarkers.MarkerType.DeployTrade, 1f, false);
			this.availableHexes.Add(hex);
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.mechMarkerPrefab[(int)this.action.GetPlayer().matFaction.faction], GameController.Instance.gameBoardPresenter.transform);
			gameObject.transform.position = hex.worldPosition;
			gameObject.gameObject.SetActive(true);
			this.mechMarkers.Add(gameObject);
		}

		// Token: 0x06001D4B RID: 7499 RVA: 0x0003B0A1 File Offset: 0x000392A1
		public HashSet<Scythe.BoardPresenter.GameHexPresenter> GetAvaliableHexes()
		{
			return this.availableHexes;
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x0003B0A9 File Offset: 0x000392A9
		public bool HaveAction()
		{
			return this.action != null;
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x0003B0B4 File Offset: 0x000392B4
		public void EnableVisuals(bool enabled)
		{
			this.EnableMarkers(enabled);
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x000B6524 File Offset: 0x000B4724
		private void EnableMarkers(bool enabled)
		{
			foreach (Scythe.BoardPresenter.GameHexPresenter gameHexPresenter in this.availableHexes)
			{
				gameHexPresenter.SetFocus(enabled, HexMarkers.MarkerType.DeployTrade, 0f, false);
			}
		}

		// Token: 0x06001D4F RID: 7503 RVA: 0x0003AD11 File Offset: 0x00038F11
		public void OnEndDeployButtonClicked()
		{
			GameController.Instance.PopupWindowsBeforeNextTurn();
		}

		// Token: 0x06001D50 RID: 7504 RVA: 0x000B657C File Offset: 0x000B477C
		public override void OnActionEnded()
		{
			this.deployStep = DeployPresenter.DeployStep.none;
			if (this.lastActionContainer != null)
			{
				this.lastActionContainer.SetActive(false);
			}
			if (!this.openedEarlier)
			{
				if (PlatformManager.IsStandalone)
				{
					this.mechToggle.isOn = false;
					AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.ingame_event);
					AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.in_game, Screens.tab_mech, Contexts.ingame);
				}
				this.openedEarlier = false;
			}
			if (!PlatformManager.IsStandalone)
			{
				this.mechSelectionMobile.SetActive(false);
			}
			PlayerUnits.ChangeUnitsColliderState(true);
			this.Clear();
			GameController.Instance.UpdateStats(true, true);
			foreach (Scythe.BoardPresenter.GameHexPresenter gameHexPresenter in this.availableHexes)
			{
				gameHexPresenter.SetFocus(false, HexMarkers.MarkerType.DeployTrade, 0.3f, false);
			}
			foreach (GameObject gameObject in this.mechMarkers)
			{
				global::UnityEngine.Object.Destroy(gameObject);
			}
			this.mechMarkers.Clear();
			GameController.HexGetFocused -= this.OnHexSelected;
			GameController.HexSelectionMode = GameController.SelectionMode.Normal;
			HumanInputHandler.Instance.OnInputEnded();
			base.gameObject.SetActive(false);
			this.action = null;
			if (DeployPresenter.DeployEnded != null)
			{
				DeployPresenter.DeployEnded();
			}
		}

		// Token: 0x06001D51 RID: 7505 RVA: 0x000B66E8 File Offset: 0x000B48E8
		public override void Clear()
		{
			base.Clear();
			GameObject[] array = this.assistHighlight;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
			base.DisableMapBlackout();
			for (int j = 0; j < 4; j++)
			{
				GameController.Instance.matFaction.mechButtonMobileShort[j].interactable = (GameController.Instance.matFaction.mechButton[j].interactable = false);
				GameController.Instance.matFaction.mechGlowMobileShort[j].enabled = (GameController.Instance.matFaction.mechGlow[j].enabled = false);
			}
			GameController.Instance.hexPointerController.Clear();
			GameController.ClearFocus();
			GameController.Instance.cameraControler.HooverReset();
			GameController.Instance.matFaction.ClearHintStories();
		}

		// Token: 0x040014FC RID: 5372
		public Toggle2 mechToggle;

		// Token: 0x040014FD RID: 5373
		public GameObject hintAction;

		// Token: 0x040014FE RID: 5374
		public GameObject mechSelectionMobile;

		// Token: 0x040014FF RID: 5375
		public GameObject[] assistHighlight;

		// Token: 0x04001500 RID: 5376
		public Image[] assistIcon;

		// Token: 0x04001501 RID: 5377
		public Color HighlightDefault = Color.white;

		// Token: 0x04001502 RID: 5378
		public Color HighlightSelected = Color.green;

		// Token: 0x04001503 RID: 5379
		public GameObject[] mechMarkerPrefab;

		// Token: 0x04001504 RID: 5380
		private List<GameObject> mechMarkers = new List<GameObject>();

		// Token: 0x04001505 RID: 5381
		public GameObject lastActionContainer;

		// Token: 0x04001506 RID: 5382
		public TextMeshProUGUI lastActionText;

		// Token: 0x04001507 RID: 5383
		[HideInInspector]
		public DeployPresenter.DeployStep deployStep;

		// Token: 0x04001508 RID: 5384
		private int skillIndex = -1;

		// Token: 0x04001509 RID: 5385
		private GainMech action;

		// Token: 0x0400150A RID: 5386
		private HashSet<Scythe.BoardPresenter.GameHexPresenter> availableHexes = new HashSet<Scythe.BoardPresenter.GameHexPresenter>();

		// Token: 0x0400150B RID: 5387
		private bool openedEarlier;

		// Token: 0x0400150D RID: 5389
		private Sequence deployWindowAnimation;

		// Token: 0x0400150E RID: 5390
		[SerializeField]
		private RectTransform deployWindowRect;

		// Token: 0x0400150F RID: 5391
		[SerializeField]
		private float popupAnimationSpeed = 0.5f;

		// Token: 0x04001510 RID: 5392
		private Vector2 originalDeploySelectionSizeDelta;

		// Token: 0x04001511 RID: 5393
		private Vector2 originalDeployWindowAnchoredPosition;

		// Token: 0x020003DB RID: 987
		public enum DeployStep
		{
			// Token: 0x04001513 RID: 5395
			none,
			// Token: 0x04001514 RID: 5396
			selectMech,
			// Token: 0x04001515 RID: 5397
			selectField
		}

		// Token: 0x020003DC RID: 988
		// (Invoke) Token: 0x06001D55 RID: 7509
		public delegate void DeployEnd();
	}
}
