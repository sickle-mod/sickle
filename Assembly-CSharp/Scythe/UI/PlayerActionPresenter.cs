using System;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000486 RID: 1158
	public abstract class PlayerActionPresenter : MonoBehaviour
	{
		// Token: 0x060024B9 RID: 9401 RVA: 0x000D9EF8 File Offset: 0x000D80F8
		public virtual void ChangeModeToNormal()
		{
			for (int i = 0; i < this.gainActionButton.Length; i++)
			{
				this.gainActionButton[i].onClick = this.actionListeners[i];
				if (!PlatformManager.IsStandalone && this.gainActionButton[i].transform.GetChild(0).childCount > 0)
				{
					this.gainActionButton[i].transform.GetChild(0).GetChild(0).GetComponent<Image>()
						.enabled = false;
				}
			}
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x0003F618 File Offset: 0x0003D818
		public virtual void ChangeModeToUpgrade(int index)
		{
			this.gainActionButton[index].onClick = this.upgradeListeners[index];
			this.gainActionButton[index].interactable = true;
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x0003F63D File Offset: 0x0003D83D
		public void DisableButton(int index)
		{
			this.gainActionButton[index].interactable = false;
			this.gainActionFrame[index].gameObject.SetActive(false);
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x000D9F74 File Offset: 0x000D8174
		private void EnableButton(int index)
		{
			this.gainActionButton[index].interactable = true;
			if (GameController.Instance != null)
			{
				Sprite sprite = GameController.Instance.matPlayer.defaultAction;
				if (this.overrideBackgroundMaterial != null)
				{
					sprite = this.overrideBackgroundMaterial;
				}
				else if (this.isFactoryAction)
				{
					sprite = GameController.Instance.matPlayer.factoryActiveAction;
				}
				if (this.gainActionButtonBackground.Length > index && this.gainActionButtonBackground[index] != null)
				{
					this.gainActionButtonBackground[index].sprite = sprite;
				}
			}
			this.gainActionFrame[index].gameObject.SetActive(true);
			if (this.gainActionFrame[index].GetComponent<Animator>().isInitialized)
			{
				this.gainActionFrame[index].GetComponent<Animator>().Play("AlphaPulseAction", 0, 0f);
			}
			this.actionEnabled[index] = true;
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x000DA054 File Offset: 0x000D8254
		public void DisableAllButtons(bool interactable = false)
		{
			for (int i = 0; i < this.gainActionButton.Length; i++)
			{
				this.gainActionFrame[i].gameObject.SetActive(false);
				this.gainActionButton[i].interactable = interactable;
				this.actionEnabled[i] = false;
			}
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x000DA0A0 File Offset: 0x000D82A0
		public void EnableAllButtons()
		{
			for (int i = 0; i < this.gainActionButton.Length; i++)
			{
				this.EnableButton(i);
			}
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x0003F660 File Offset: 0x0003D860
		public void ProduceMill()
		{
			this.actionCostButton[0].gameObject.SetActive(true);
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x0003F675 File Offset: 0x0003D875
		public void ProduceRegular(int index)
		{
			this.actionCostButton[index + 1].gameObject.SetActive(true);
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x000DA0C8 File Offset: 0x000D82C8
		public void OnProduceEnded()
		{
			for (int i = 0; i < this.actionCostButton.Length; i++)
			{
				if (this.actionCostButton[i].gameObject.activeInHierarchy)
				{
					this.actionCostButton[i].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x000DA110 File Offset: 0x000D8310
		protected virtual void Reset()
		{
			this.gainActionButtonBackground = new Image[this.gainActionButton.Length];
			if (PlatformManager.IsStandalone)
			{
				for (int i = 0; i < this.gainActionButton.Length; i++)
				{
					this.gainActionButtonBackground[i] = this.gainActionButton[i].GetComponent<Image>();
				}
			}
			else
			{
				for (int j = 0; j < this.gainActionButton.Length; j++)
				{
					this.gainActionButtonBackground[j] = this.gainActionButton[j].transform.GetChild(0).GetComponent<Image>();
				}
			}
			this.EnableAllButtons();
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x000DA19C File Offset: 0x000D839C
		public virtual void PayResource(int index)
		{
			this.actionCost[index].GetComponent<Image>().enabled = false;
			if (PlatformManager.IsStandalone)
			{
				this.actionCostButton[index].gameObject.SetActive(true);
			}
			if (this.payResourceAnimation != null && PlatformManager.IsStandalone)
			{
				this.payResourceAnimation.Play("PayResource", 0, 0f);
			}
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x0003F68C File Offset: 0x0003D88C
		public virtual void GetResource(int index)
		{
			this.actionCost[index].GetComponent<Image>().enabled = this.IsActionCostEnabled;
			this.actionCostButton[index].gameObject.SetActive(false);
		}

		// Token: 0x060024C5 RID: 9413 RVA: 0x000DA204 File Offset: 0x000D8404
		public virtual void OnPayResourceEnd()
		{
			for (int i = 0; i < this.actionCost.Length; i++)
			{
				if (PlatformManager.IsStandalone && this.actionCostButton[i].gameObject.activeInHierarchy)
				{
					this.actionCost[i].GetComponent<Image>().enabled = this.IsActionCostEnabled;
					this.actionCostButton[i].gameObject.SetActive(false);
					this.actionCostButton[i].image.raycastTarget = false;
				}
			}
		}

		// Token: 0x060024C6 RID: 9414 RVA: 0x000DA280 File Offset: 0x000D8480
		public void ResetButtonsBackgrounds()
		{
			foreach (MatPlayerSectionPresenter matPlayerSectionPresenter in GameController.Instance.matPlayer.matSection)
			{
				Sprite sprite = GameController.Instance.matPlayer.defaultAction;
				if (matPlayerSectionPresenter.topActionPresenter.overrideBackgroundMaterial != null)
				{
					sprite = matPlayerSectionPresenter.topActionPresenter.overrideBackgroundMaterial;
				}
				else if (matPlayerSectionPresenter.topActionPresenter.isFactoryAction)
				{
					sprite = GameController.Instance.matPlayer.factoryActiveAction;
				}
				Image[] array = matPlayerSectionPresenter.topActionPresenter.gainActionButtonBackground;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].sprite = sprite;
				}
				array = matPlayerSectionPresenter.downActionPresenter.gainActionButtonBackground;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].sprite = sprite;
				}
			}
		}

		// Token: 0x060024C7 RID: 9415 RVA: 0x0003F6B9 File Offset: 0x0003D8B9
		public void ChangeInteractiveActionColor(int id, Color color)
		{
			this.inactiveActionOverlays[id].color = color;
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x0003F6C9 File Offset: 0x0003D8C9
		public void ShowInactiveActionOverlay(int actionIndex)
		{
			this.ChangeInteractiveActionOverlayState(actionIndex, true);
			this.ChangeInteractiveActionColor(actionIndex, new Color(1f, 1f, 1f, 0.8f));
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x000DA37C File Offset: 0x000D857C
		public void HideInactiveActionOverlays()
		{
			for (int i = 0; i < this.inactiveActionOverlays.Length; i++)
			{
				this.ChangeInteractiveActionOverlayState(i, false);
			}
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x0003F6F3 File Offset: 0x0003D8F3
		public void ChangeInteractiveActionOverlayState(int id, bool enabled)
		{
			this.inactiveActionOverlays[id].enabled = enabled;
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x060024CB RID: 9419 RVA: 0x0003F703 File Offset: 0x0003D903
		protected bool IsActionCostEnabled
		{
			get
			{
				return PlatformManager.IsStandalone;
			}
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x000DA3A4 File Offset: 0x000D85A4
		protected void UpdateActionCostOpacity(SectionAction action)
		{
			for (int i = 0; i < this.actionCost.Length; i++)
			{
				this.GetActionCostIcon(i).color = Color.white;
			}
			Color color = new Color(1f, 1f, 1f, 1f);
			if (this is PlayerTopActionPresenter && (this as PlayerTopActionPresenter).actionType == TopActionType.Produce)
			{
				for (int j = 0; j < action.GetNumberOfPayActions(); j++)
				{
					this.GetActionCostIcon(2 * j + 1).color = (action.GetPayAction(j).CanPlayerPay() ? Color.white : color);
				}
				return;
			}
			int num = action.GetMissingResourceCount() * this.gainActionButton.Length;
			int num2 = this.actionCost.Length - 1;
			while (num2 >= 0 && num > 0)
			{
				if (num > 0 && this.actionCost[num2].gameObject.activeSelf && this.GetActionCostIcon(num2).enabled)
				{
					this.GetActionCostIcon(num2).color = color;
					num--;
				}
				num2--;
			}
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x000DA4A8 File Offset: 0x000D86A8
		public void UpdateActionCostOverlay(SectionAction action)
		{
			Color color = new Color(1f, 1f, 1f, 0.8f);
			for (int i = 0; i < this.inactiveActionOverlays.Length; i++)
			{
				if (this.inactiveActionOverlays.Length > i && this.inactiveActionOverlays[i] != null && !this.inactiveActionOverlays[i].enabled)
				{
					this.ChangeInteractiveActionOverlayState(i, !action.CanPlayerPayActions());
					this.ChangeInteractiveActionColor(i, color);
				}
			}
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x0003F70A File Offset: 0x0003D90A
		private Image GetActionCostIcon(int index)
		{
			return this.actionCost[index].transform.GetChild(1).GetComponent<Image>();
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x000DA528 File Offset: 0x000D8728
		public static string CreateFactoryCardDescription(TopAction factoryCard)
		{
			if (factoryCard.DifferentGain)
			{
				return string.Concat(new string[]
				{
					PlayerActionPresenter.GetPayString(factoryCard),
					PlayerActionPresenter.GetGainString(factoryCard, 0),
					"-----------------",
					Environment.NewLine,
					PlayerActionPresenter.GetPayString(factoryCard),
					PlayerActionPresenter.GetGainString(factoryCard, 1)
				});
			}
			return PlayerActionPresenter.GetPayString(factoryCard) + Environment.NewLine + PlayerActionPresenter.GetGainString(factoryCard, -1);
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x000DA598 File Offset: 0x000D8798
		protected static string CreateFactoryCardHintContent(TopAction factoryCard, int id = -1)
		{
			string text = PlayerActionPresenter.GetPayString(factoryCard);
			if (factoryCard.DifferentGain)
			{
				text += PlayerActionPresenter.GetGainString(factoryCard, id);
			}
			else
			{
				text += PlayerActionPresenter.GetAllGainString(factoryCard);
			}
			return text;
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x000DA5D4 File Offset: 0x000D87D4
		private static string GetPayString(TopAction factoryCard)
		{
			string text = ScriptLocalization.Get("PlayerMat/Pay").ToUpper() + " ";
			int numberOfPayActions = factoryCard.GetNumberOfPayActions();
			for (int i = 0; i < numberOfPayActions; i++)
			{
				PayAction payAction = factoryCard.GetPayAction(i);
				if (payAction is PayResource)
				{
					PayResource payResource = payAction as PayResource;
					if (payResource.AnyResource && !payResource.DifferentResources)
					{
						text += string.Format("{0} {1} {2}{3}", new object[]
						{
							payResource.GetAmount(),
							ScriptLocalization.Get("PlayerMat/AnyResources"),
							TMPHelper.GetTMPSprite("AnyResource"),
							(i == numberOfPayActions - 1) ? " " : ", "
						});
					}
					else if (payResource.AnyResource && payResource.DifferentResources)
					{
						text += string.Format("2 {0} {1}{2}{3}{4}", new object[]
						{
							ScriptLocalization.Get("PlayerMat/DifferentResources"),
							TMPHelper.GetTMPSprite("AnyResource"),
							TMPHelper.GetTMPSprite("Inequality"),
							TMPHelper.GetTMPSprite("AnyResource"),
							(i == numberOfPayActions - 1) ? " " : ", "
						});
					}
					else
					{
						text += string.Format("{0} {1}{2}", payResource.GetAmount(), TMPHelper.GetTMPSprite(payResource.ResourceToPay.ToString()), (i == numberOfPayActions - 1) ? " " : ", ");
					}
				}
				else
				{
					text += string.Format("{0} {1}{2}", payAction.GetAmount(), TMPHelper.GetTMPSprite(payAction.GetPayType().ToString()), (i == numberOfPayActions - 1) ? " " : ", ");
				}
			}
			return text;
		}

		// Token: 0x060024D2 RID: 9426 RVA: 0x0003F724 File Offset: 0x0003D924
		private static string GetGainString(TopAction factoryCard, int gainId)
		{
			if (gainId == -1)
			{
				return PlayerActionPresenter.GetAllGainString(factoryCard);
			}
			return PlayerActionPresenter.GetExplicitGainString(factoryCard, gainId);
		}

		// Token: 0x060024D3 RID: 9427 RVA: 0x000DA7A8 File Offset: 0x000D89A8
		private static string GetExplicitGainString(TopAction factoryCard, int gainId)
		{
			string text = Environment.NewLine + ScriptLocalization.Get("PlayerMat/Gain").ToUpper() + " ";
			int numberOfGainActions = factoryCard.GetNumberOfGainActions();
			for (int i = 0; i < numberOfGainActions; i++)
			{
				if (i == gainId)
				{
					GainAction gainAction = factoryCard.GetGainAction(i);
					GainType gainType = gainAction.GetGainType();
					string text2 = "";
					switch (gainType)
					{
					case GainType.Upgrade:
						text2 = ScriptLocalization.Get("PlayerMat/Upgrade").ToUpper() + " ";
						break;
					case GainType.Mech:
						text2 = ScriptLocalization.Get("PlayerMat/Mech").ToUpper() + " ";
						break;
					case GainType.Building:
						text2 = ScriptLocalization.Get("PlayerMat/Build").ToUpper() + " ";
						break;
					case GainType.Recruit:
						text2 = ScriptLocalization.Get("PlayerMat/Recruit").ToUpper() + " ";
						break;
					}
					text += string.Format("{0} {1}{2}   ", gainAction.Amount, text2, TMPHelper.GetTMPSprite(gainAction.GetGainType().ToString()));
				}
			}
			return text + Environment.NewLine;
		}

		// Token: 0x060024D4 RID: 9428 RVA: 0x000DA8E0 File Offset: 0x000D8AE0
		private static string GetAllGainString(TopAction factoryCard)
		{
			string text = ScriptLocalization.Get("PlayerMat/Gain").ToUpper() + " ";
			int numberOfGainActions = factoryCard.GetNumberOfGainActions();
			for (int i = 0; i < numberOfGainActions; i++)
			{
				GainAction gainAction = factoryCard.GetGainAction(i);
				text += string.Format("{0} {1}{2}", gainAction.Amount, TMPHelper.GetTMPSprite(gainAction.GetGainType().ToString()), (i == numberOfGainActions - 1) ? Environment.NewLine : ", ");
			}
			return text;
		}

		// Token: 0x040019BB RID: 6587
		public Button.ButtonClickedEvent[] actionListeners;

		// Token: 0x040019BC RID: 6588
		public Button.ButtonClickedEvent[] upgradeListeners;

		// Token: 0x040019BD RID: 6589
		public Button[] gainActionButton;

		// Token: 0x040019BE RID: 6590
		public Image[] gainActionFrame;

		// Token: 0x040019BF RID: 6591
		public Image[] actionCost;

		// Token: 0x040019C0 RID: 6592
		public Button[] actionCostButton;

		// Token: 0x040019C1 RID: 6593
		[SerializeField]
		protected Sprite goldBackgroundMaterial;

		// Token: 0x040019C2 RID: 6594
		[SerializeField]
		protected Sprite overrideBackgroundMaterial;

		// Token: 0x040019C3 RID: 6595
		[SerializeField]
		protected YesNoDialog notEnoughResourcesDialog;

		// Token: 0x040019C4 RID: 6596
		[SerializeField]
		private Animator payResourceAnimation;

		// Token: 0x040019C5 RID: 6597
		[HideInInspector]
		public bool[] actionEnabled = new bool[2];

		// Token: 0x040019C6 RID: 6598
		private const string TMP_SPRITE_START = "<sprite name=\"";

		// Token: 0x040019C7 RID: 6599
		private const string TMP_SPRITE_END = "\">";

		// Token: 0x040019C8 RID: 6600
		[HideInInspector]
		public Image[] gainActionButtonBackground;

		// Token: 0x040019C9 RID: 6601
		public bool isFactoryAction;

		// Token: 0x040019CA RID: 6602
		[SerializeField]
		protected Image[] upgradeAvailableIcons;

		// Token: 0x040019CB RID: 6603
		[SerializeField]
		protected Image[] inactiveActionOverlays;

		// Token: 0x040019CC RID: 6604
		protected Player player;
	}
}
