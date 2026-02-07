using System;
using HoneyFramework;
using Scythe.BoardPresenter;
using Scythe.GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000405 RID: 1029
	public class TradeButtonsPanel : MonoBehaviour
	{
		// Token: 0x06001F64 RID: 8036 RVA: 0x0003C30D File Offset: 0x0003A50D
		private void Update()
		{
			this.UpdatePosition();
			this.Scale();
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x000C0DDC File Offset: 0x000BEFDC
		public void AttachTradePanel(TradePresenter tradePresenter, Scythe.BoardPresenter.GameHexPresenter attachedGameHex, bool selectFieldMode)
		{
			this.Clear();
			this.tradePresenter = tradePresenter;
			this.attachedGameHex = attachedGameHex;
			this.UpdatePosition();
			this.FieldSelectButton.SetActive(selectFieldMode);
			this.ResourceButtons.SetActive(!selectFieldMode);
			if (!PlatformManager.IsStandalone)
			{
				GameController.HexGetFocused += this.OnHexFocused;
			}
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x000C0E38 File Offset: 0x000BF038
		private void UpdatePosition()
		{
			if (this.attachedGameHex == null)
			{
				return;
			}
			base.transform.position = HexCoordinates.HexToWorld3D(this.attachedGameHex.position);
			base.transform.rotation = Quaternion.Euler(90f, 90f, CameraControler.Instance.encounterRotationAdjustment.z);
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x000C0E94 File Offset: 0x000BF094
		private void Scale()
		{
			if (this.attachedGameHex == null)
			{
				return;
			}
			if (this.hidingAnimation)
			{
				this.UpdateHidingProgress();
			}
			float num = this.normalScale.x;
			if (this.hidingAnimation)
			{
				num = this.normalScale.x * this.visibility;
			}
			base.transform.localScale = new Vector3(num, num, num);
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x0003C31B File Offset: 0x0003A51B
		private void UpdateHidingProgress()
		{
			this.visibility -= Time.deltaTime * this.hidingSpeed;
			if (this.visibility <= 0f)
			{
				this.HidingComplete();
			}
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x0003C349 File Offset: 0x0003A549
		public void Hide()
		{
			this.SetButtonsInteractable(false);
			this.hidingAnimation = true;
			if (!PlatformManager.IsStandalone)
			{
				GameController.HexGetFocused -= this.OnHexFocused;
			}
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x0003C371 File Offset: 0x0003A571
		private void HidingComplete()
		{
			base.gameObject.SetActive(false);
			this.Clear();
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x0003C385 File Offset: 0x0003A585
		public void Clear()
		{
			this.hidingAnimation = false;
			this.visibility = 1f;
			this.SetButtonsInteractable(true);
			base.transform.localScale = this.normalScale;
			this.tradePresenter = null;
			this.attachedGameHex = null;
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x0003C3BF File Offset: 0x0003A5BF
		private void OnHexFocused(Scythe.BoardPresenter.GameHexPresenter gameHexPresenter)
		{
			if (this.attachedGameHex != null && gameHexPresenter == this.attachedGameHex)
			{
				this.ReportClickedMobile();
			}
		}

		// Token: 0x06001F6D RID: 8045 RVA: 0x0003C3D8 File Offset: 0x0003A5D8
		public void ReportClickedMobile()
		{
			this.tradePresenter.GameHexSelected(this.attachedGameHex);
			this.hexBackground.color = Color.green;
		}

		// Token: 0x06001F6E RID: 8046 RVA: 0x000C0EF4 File Offset: 0x000BF0F4
		public void ReportClickedResource(int resource)
		{
			this.tradePresenter.ResourceSelected((ResourceType)resource, this.attachedGameHex);
			switch (resource)
			{
			case 0:
				WorldSFXManager.PlaySound(SoundEnum.TradeOil, AudioSourceType.Buttons);
				return;
			case 1:
				WorldSFXManager.PlaySound(SoundEnum.TradeIron, AudioSourceType.Buttons);
				return;
			case 2:
				WorldSFXManager.PlaySound(SoundEnum.TradeGrain, AudioSourceType.Buttons);
				return;
			case 3:
				WorldSFXManager.PlaySound(SoundEnum.TradeWood, AudioSourceType.Buttons);
				return;
			default:
				return;
			}
		}

		// Token: 0x06001F6F RID: 8047 RVA: 0x000C0F54 File Offset: 0x000BF154
		public void SetButtonsInteractable(bool interactable)
		{
			foreach (object obj in this.ResourceButtons.transform)
			{
				Transform transform = (Transform)obj;
				transform.GetComponent<Button>().interactable = interactable;
				if (interactable)
				{
					transform.gameObject.GetComponent<PointerEventsController>().buttonHoover += this.HooverButtonSound;
				}
				else
				{
					transform.gameObject.GetComponent<PointerEventsController>().buttonHoover -= this.HooverButtonSound;
				}
			}
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x0003C3FB File Offset: 0x0003A5FB
		public void ButtonHooverSFXDisable(Transform button)
		{
			button.gameObject.GetComponent<PointerEventsController>().buttonHoover -= this.HooverButtonSound;
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x0003C419 File Offset: 0x0003A619
		private void HooverButtonSound()
		{
			WorldSFXManager.PlaySound(SoundEnum.PlayersBoardShowEnemysboardRelease, AudioSourceType.Buttons);
		}

		// Token: 0x06001F72 RID: 8050 RVA: 0x0003C423 File Offset: 0x0003A623
		public Scythe.BoardPresenter.GameHexPresenter GetAttachedGameHex()
		{
			return this.attachedGameHex;
		}

		// Token: 0x0400161D RID: 5661
		public GameObject FieldSelectButton;

		// Token: 0x0400161E RID: 5662
		public GameObject ResourceButtons;

		// Token: 0x0400161F RID: 5663
		public Image hexBackground;

		// Token: 0x04001620 RID: 5664
		public static float cameraZoomScalingBoundary = 0.3f;

		// Token: 0x04001621 RID: 5665
		private Vector3 normalScale = new Vector3(0.01f, 0.01f, 0.01f);

		// Token: 0x04001622 RID: 5666
		private TradePresenter tradePresenter;

		// Token: 0x04001623 RID: 5667
		private Scythe.BoardPresenter.GameHexPresenter attachedGameHex;

		// Token: 0x04001624 RID: 5668
		private bool hidingAnimation;

		// Token: 0x04001625 RID: 5669
		private float visibility = 1f;

		// Token: 0x04001626 RID: 5670
		private float hidingSpeed = 5f;
	}
}
