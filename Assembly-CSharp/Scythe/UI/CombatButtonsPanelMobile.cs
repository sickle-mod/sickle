using System;
using HoneyFramework;
using Scythe.BoardPresenter;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000440 RID: 1088
	public class CombatButtonsPanelMobile : MonoBehaviour
	{
		// Token: 0x140000DD RID: 221
		// (add) Token: 0x06002198 RID: 8600 RVA: 0x000C984C File Offset: 0x000C7A4C
		// (remove) Token: 0x06002199 RID: 8601 RVA: 0x000C9884 File Offset: 0x000C7A84
		public event Action<Scythe.BoardPresenter.GameHexPresenter> CombatButtonClicked;

		// Token: 0x0600219A RID: 8602 RVA: 0x0003D782 File Offset: 0x0003B982
		private void Awake()
		{
			this.FieldSelectButton.onClick.AddListener(new UnityAction(this.OnHexFocused));
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x0003D7A0 File Offset: 0x0003B9A0
		private void OnDestroy()
		{
			this.FieldSelectButton.onClick.RemoveListener(new UnityAction(this.OnHexFocused));
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x0003D7BE File Offset: 0x0003B9BE
		private void Update()
		{
			this.UpdatePosition();
			this.Scale();
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x000C98BC File Offset: 0x000C7ABC
		public void AttachCombatPanel(CombatPresenterMobile combatPresenter, Scythe.BoardPresenter.GameHexPresenter attachedGameHex, int heroCount = 0)
		{
			this.Clear();
			this.combatPresenter = combatPresenter;
			this.attachedGameHex = attachedGameHex;
			if (heroCount >= 2)
			{
				this.HeroesMarker.SetActive(true);
			}
			else if (heroCount == 1)
			{
				this.MechsHeroMarker.SetActive(true);
			}
			else
			{
				this.MechsMarker.SetActive(true);
			}
			this.UpdatePosition();
		}

		// Token: 0x0600219E RID: 8606 RVA: 0x000C9914 File Offset: 0x000C7B14
		private void UpdatePosition()
		{
			if (this.attachedGameHex == null)
			{
				return;
			}
			base.transform.position = HexCoordinates.HexToWorld3D(this.attachedGameHex.position);
			base.transform.rotation = Quaternion.Euler(this.rotation.x, this.rotation.y, CameraControler.Instance.encounterRotationAdjustment.z);
		}

		// Token: 0x0600219F RID: 8607 RVA: 0x000C997C File Offset: 0x000C7B7C
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

		// Token: 0x060021A0 RID: 8608 RVA: 0x0003D7CC File Offset: 0x0003B9CC
		private void UpdateHidingProgress()
		{
			this.visibility -= Time.deltaTime * this.hidingSpeed;
			if (this.visibility <= 0f)
			{
				this.HidingComplete();
			}
		}

		// Token: 0x060021A1 RID: 8609 RVA: 0x0003D7FA File Offset: 0x0003B9FA
		public void Hide()
		{
			this.hidingAnimation = true;
		}

		// Token: 0x060021A2 RID: 8610 RVA: 0x0003D803 File Offset: 0x0003BA03
		private void HidingComplete()
		{
			base.gameObject.SetActive(false);
			this.Clear();
		}

		// Token: 0x060021A3 RID: 8611 RVA: 0x000C99DC File Offset: 0x000C7BDC
		public void Clear()
		{
			this.BackToBattleText.SetActive(false);
			this.hidingAnimation = false;
			this.ResumeMode = false;
			this.visibility = 1f;
			base.transform.localScale = this.normalScale;
			this.CombatButtonClicked = null;
			this.combatPresenter = null;
			this.attachedGameHex = null;
			this.HeroesMarker.SetActive(false);
			this.MechsHeroMarker.SetActive(false);
			this.MechsMarker.SetActive(false);
			base.gameObject.SetActive(false);
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x0003D817 File Offset: 0x0003BA17
		private void OnHexFocused()
		{
			if (this.attachedGameHex != null)
			{
				this.ReportClickedMobile();
			}
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x0003D827 File Offset: 0x0003BA27
		public void ReportClickedMobile()
		{
			if (this.CombatButtonClicked != null && this.attachedGameHex != null)
			{
				this.CombatButtonClicked(this.attachedGameHex);
			}
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x0003D84A File Offset: 0x0003BA4A
		public Scythe.BoardPresenter.GameHexPresenter GetAttachedGameHex()
		{
			return this.attachedGameHex;
		}

		// Token: 0x0400176F RID: 5999
		public bool ResumeMode;

		// Token: 0x04001770 RID: 6000
		public Button FieldSelectButton;

		// Token: 0x04001771 RID: 6001
		public GameObject MechsMarker;

		// Token: 0x04001772 RID: 6002
		public GameObject MechsHeroMarker;

		// Token: 0x04001773 RID: 6003
		public GameObject HeroesMarker;

		// Token: 0x04001774 RID: 6004
		public Image hexBackground;

		// Token: 0x04001775 RID: 6005
		public Vector2 rotation;

		// Token: 0x04001776 RID: 6006
		public GameObject BackToBattleText;

		// Token: 0x04001777 RID: 6007
		public static float cameraZoomScalingBoundary = 0.3f;

		// Token: 0x04001778 RID: 6008
		private Vector3 normalScale = new Vector3(0.01f, 0.01f, 0.01f);

		// Token: 0x04001779 RID: 6009
		private CombatPresenterMobile combatPresenter;

		// Token: 0x0400177A RID: 6010
		private Scythe.BoardPresenter.GameHexPresenter attachedGameHex;

		// Token: 0x0400177B RID: 6011
		private bool hidingAnimation;

		// Token: 0x0400177C RID: 6012
		private float visibility = 1f;

		// Token: 0x0400177D RID: 6013
		private float hidingSpeed = 5f;
	}
}
