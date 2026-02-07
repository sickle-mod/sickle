using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004D2 RID: 1234
	public class AlbumMobile : MonoBehaviour
	{
		// Token: 0x06002752 RID: 10066 RVA: 0x000E7DA8 File Offset: 0x000E5FA8
		private void Awake()
		{
			this.encountersToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnEncountersToggleValueChanged));
			this.factoryToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnFactoryToggleValueChanged));
			this.objectivesToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnObjectivesToggleValueChanged));
			this.selectorNextButton.onClick.AddListener(new UnityAction(this.OnSelectorNextButtonClicked));
			this.selectorPreviousButton.onClick.AddListener(new UnityAction(this.OnSelectorPreviousButtonClicked));
			this.backButton.onClick.AddListener(new UnityAction(this.OnBackButtonClicked));
			this.currentlySelectedTab = this.encountersTab;
			this.currentlySelectedTab.OpenTab();
		}

		// Token: 0x06002753 RID: 10067 RVA: 0x000E7E74 File Offset: 0x000E6074
		private void OnEnable()
		{
			this.encountersToggle.isOn = true;
			this.encountersToggle.OnSelect(new BaseEventData(EventSystem.current));
			this.objectivesToggle.isOn = false;
			this.factoryToggle.isOn = false;
			this.currentlySelectedTab = this.encountersTab;
			this.currentlySelectedTab.OpenTab();
		}

		// Token: 0x06002754 RID: 10068 RVA: 0x00041215 File Offset: 0x0003F415
		private void OnDisable()
		{
			this.currentSelectorIndex = 0;
		}

		// Token: 0x06002755 RID: 10069 RVA: 0x000E7ED4 File Offset: 0x000E60D4
		private void OnEncountersToggleValueChanged(bool isOn)
		{
			if (isOn && this.currentlySelectedTab != this.encountersTab)
			{
				this.currentSelectorIndex = 0;
				this.currentlySelectedTab.gameObject.SetActive(false);
				this.currentlySelectedTab = this.encountersTab;
				this.currentlySelectedTab.gameObject.SetActive(true);
				this.currentlySelectedTab.OpenTab();
				ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
			}
		}

		// Token: 0x06002756 RID: 10070 RVA: 0x000E7F44 File Offset: 0x000E6144
		private void OnFactoryToggleValueChanged(bool isOn)
		{
			if (isOn && this.currentlySelectedTab != this.factoryTab)
			{
				this.currentSelectorIndex = 0;
				this.currentlySelectedTab.gameObject.SetActive(false);
				this.currentlySelectedTab = this.factoryTab;
				this.currentlySelectedTab.gameObject.SetActive(true);
				this.currentlySelectedTab.OpenTab();
				ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
			}
		}

		// Token: 0x06002757 RID: 10071 RVA: 0x000E7FB4 File Offset: 0x000E61B4
		private void OnObjectivesToggleValueChanged(bool isOn)
		{
			if (isOn && this.currentlySelectedTab != this.objectivesTab)
			{
				this.currentSelectorIndex = 0;
				this.currentlySelectedTab.gameObject.SetActive(false);
				this.currentlySelectedTab = this.objectivesTab;
				this.currentlySelectedTab.gameObject.SetActive(true);
				this.currentlySelectedTab.OpenTab();
				ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
			}
		}

		// Token: 0x06002758 RID: 10072 RVA: 0x000E8024 File Offset: 0x000E6224
		private void OnSelectorNextButtonClicked()
		{
			this.currentSelectorIndex = ((this.currentSelectorIndex < this.currentlySelectedTab.CardCount - 1) ? (this.currentSelectorIndex + 1) : 0);
			this.currentlySelectedTab.ChooseNext(this.currentSelectorIndex);
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
		}

		// Token: 0x06002759 RID: 10073 RVA: 0x000E8074 File Offset: 0x000E6274
		private void OnSelectorPreviousButtonClicked()
		{
			this.currentSelectorIndex = ((this.currentSelectorIndex > 0) ? (this.currentSelectorIndex - 1) : (this.currentlySelectedTab.CardCount - 1));
			this.currentlySelectedTab.ChoosePrevious(this.currentSelectorIndex);
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x0004121E File Offset: 0x0003F41E
		public void ResetAchievements()
		{
			AchievementManager.RemoveGainedAchievements();
			Debug.Log("Reset all achievements!");
		}

		// Token: 0x0600275B RID: 10075 RVA: 0x0004122F File Offset: 0x0003F42F
		private void OnBackButtonClicked()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
			base.gameObject.SetActive(false);
		}

		// Token: 0x04001C24 RID: 7204
		[SerializeField]
		private Toggle encountersToggle;

		// Token: 0x04001C25 RID: 7205
		[SerializeField]
		private Toggle factoryToggle;

		// Token: 0x04001C26 RID: 7206
		[SerializeField]
		private Toggle objectivesToggle;

		// Token: 0x04001C27 RID: 7207
		[SerializeField]
		private Button selectorNextButton;

		// Token: 0x04001C28 RID: 7208
		[SerializeField]
		private Button selectorPreviousButton;

		// Token: 0x04001C29 RID: 7209
		[SerializeField]
		private Button backButton;

		// Token: 0x04001C2A RID: 7210
		[SerializeField]
		private AlbumMobileTab encountersTab;

		// Token: 0x04001C2B RID: 7211
		[SerializeField]
		private AlbumMobileTab factoryTab;

		// Token: 0x04001C2C RID: 7212
		[SerializeField]
		private AlbumMobileTab objectivesTab;

		// Token: 0x04001C2D RID: 7213
		private AlbumMobileTab currentlySelectedTab;

		// Token: 0x04001C2E RID: 7214
		private int currentSelectorIndex;
	}
}
