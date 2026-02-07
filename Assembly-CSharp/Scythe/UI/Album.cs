using System;
using System.Collections;
using System.Collections.Generic;
using Scythe.Analytics;
using Scythe.Multiplayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004CB RID: 1227
	public class Album : MonoBehaviour
	{
		// Token: 0x06002715 RID: 10005 RVA: 0x00040EFD File Offset: 0x0003F0FD
		private void Awake()
		{
			this.InitializeAlbumObjects();
			this.InitializeToggles();
			this.InitializeButtons();
		}

		// Token: 0x06002716 RID: 10006 RVA: 0x00040F11 File Offset: 0x0003F111
		private void OnEnable()
		{
			this.ChangeView(this.currentTab, TabType.None);
		}

		// Token: 0x06002717 RID: 10007 RVA: 0x00040F20 File Offset: 0x0003F120
		private void OnDisable()
		{
			this.MinimizeCard(false);
			this.bundlesLoader.Clear();
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x00040F34 File Offset: 0x0003F134
		public void Open()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.MenuHotSeatButton);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_album_button);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.album, Contexts.outgame);
			base.gameObject.SetActive(true);
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x000E71BC File Offset: 0x000E53BC
		private void InitializeToggles()
		{
			if (this.toggles == null)
			{
				this.toggles = new List<Toggle>();
			}
			foreach (object obj in this.toggleGroup.transform)
			{
				Transform transform = (Transform)obj;
				this.toggles.Add(transform.GetComponent<Toggle>());
			}
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x000E7238 File Offset: 0x000E5438
		private void InitializeAlbumObjects()
		{
			int num = this.maxElementCount;
			for (int i = 0; i < num; i++)
			{
				global::UnityEngine.Object.Instantiate<GameObject>(this.albumObjectPrefab, this.albumObjectsContainer);
				int id = i;
				this.albumObjectsContainer.GetChild(i).GetComponent<AlbumObject>().GetButtonClickedEvent()
					.AddListener(delegate
					{
						this.MaximizeCard(id);
					});
				this.albumObjectsContainer.GetChild(i).GetComponent<AlbumObject>().SetFactoryCardBuilder(this.builder);
			}
			this.presentationCard.GetButtonClickedEvent().AddListener(delegate
			{
				this.MinimizeCard(true);
			});
			this.presentationCard.SetFactoryCardBuilder(this.builder);
			this.scrollBarResetFlag = true;
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x00040F62 File Offset: 0x0003F162
		private void InitializeButtons()
		{
			if (this.removeAchievementsButton != null)
			{
				this.removeAchievementsButton.gameObject.SetActive(PlatformManager.IsUnityEditor);
			}
		}

		// Token: 0x0600271C RID: 10012 RVA: 0x00040F87 File Offset: 0x0003F187
		private List<Sprite> GetSpritesForTabType(TabType type)
		{
			return this.sprites;
		}

		// Token: 0x0600271D RID: 10013 RVA: 0x00040F8F File Offset: 0x0003F18F
		private Sprite GetSprite(int id)
		{
			if (id >= 0 && id < this.GetSpritesForTabType(this.currentTab).Count)
			{
				return this.GetSpritesForTabType(this.currentTab)[id];
			}
			return null;
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x00040FBD File Offset: 0x0003F1BD
		private Sprite GetMaskSprite(int id)
		{
			return this.masks[id];
		}

		// Token: 0x0600271F RID: 10015 RVA: 0x00040FCB File Offset: 0x0003F1CB
		private bool GetUnlockedState(TabType type, int id)
		{
			switch (type)
			{
			case TabType.EncounterCards:
				return AchievementManager.EncounterCardUnlocked(id);
			case TabType.FactoryCards:
				return AchievementManager.FactoryCardUnlocked(id);
			case TabType.ObjectiveCards:
				return AchievementManager.ObjectiveCardUnlocked(id);
			case TabType.Mats:
				return this.UnlockedByDLC(id);
			default:
				return true;
			}
		}

		// Token: 0x06002720 RID: 10016 RVA: 0x00041003 File Offset: 0x0003F203
		private bool UnlockedByDLC(int id)
		{
			return id < 11 || GameServiceController.Instance.InvadersFromAfarUnlocked();
		}

		// Token: 0x06002721 RID: 10017 RVA: 0x00041016 File Offset: 0x0003F216
		public bool CardIsMaximized()
		{
			return this.presentationCard.gameObject.activeInHierarchy;
		}

		// Token: 0x06002722 RID: 10018 RVA: 0x000E72F8 File Offset: 0x000E54F8
		public void MaximizeCard(int id)
		{
			if (id == 13)
			{
				this.card14Info.gameObject.SetActive(true);
			}
			else
			{
				this.card14Info.gameObject.SetActive(false);
			}
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.EncounterShow);
			this.blur.SetActive(true);
			this.UpdateAlbumObject(this.presentationCard, id);
			this.albumScrollRect.StopMovement();
			this.albumScrollRect.verticalScrollbar.interactable = false;
			this.albumScrollRect.enabled = false;
		}

		// Token: 0x06002723 RID: 10019 RVA: 0x00041028 File Offset: 0x0003F228
		public void MinimizeCard(bool sound = true)
		{
			if (sound)
			{
				ButtonsSFXManager.Instance.PlaySound(SoundEnum.EncounterShow);
			}
			this.albumScrollRect.verticalScrollbar.interactable = true;
			this.albumScrollRect.enabled = true;
			this.blur.SetActive(false);
		}

		// Token: 0x06002724 RID: 10020 RVA: 0x000E737C File Offset: 0x000E557C
		public void TabChanged(int id)
		{
			if (this.toggles[id].isOn && id != (int)this.currentTab)
			{
				TabType tabType = this.currentTab;
				this.currentTab = (TabType)id;
				this.gridLayoutGroup.cellSize = this.cellSize[id];
				this.ChangeView(this.currentTab, tabType);
				ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiShowHideMapMarkers);
			}
		}

		// Token: 0x06002725 RID: 10021 RVA: 0x00041062 File Offset: 0x0003F262
		private void ChangeView(TabType type, TabType previousType)
		{
			if (previousType != TabType.None)
			{
				base.StopCoroutine(this.ChangeViewAsync(previousType));
			}
			if (this.albumObjectsContainer.childCount == 0)
			{
				this.InitializeAlbumObjects();
			}
			base.StartCoroutine(this.ChangeViewAsync(type));
		}

		// Token: 0x06002726 RID: 10022 RVA: 0x00041096 File Offset: 0x0003F296
		private IEnumerator ChangeViewAsync(TabType type)
		{
			int num = this.maxElementCount;
			for (int i = 0; i < num; i++)
			{
				this.albumObjectsContainer.GetChild(i).gameObject.SetActive(false);
			}
			if (!this.bundlesLoader.SpritesLoaded(type))
			{
				this.waitingPanel.Activate();
				yield return base.StartCoroutine(this.bundlesLoader.LoadBundle(type));
				if (type == TabType.Mats)
				{
					yield return base.StartCoroutine(this.bundlesLoader.LoadSprites(type, this.matSpriteNames));
				}
				else
				{
					yield return base.StartCoroutine(this.bundlesLoader.LoadSprites(type));
				}
				if (this.currentTab != type)
				{
					yield break;
				}
			}
			this.sprites = this.bundlesLoader.GetSprites(type);
			this.UpdateUnlockedCards();
			for (int j = 0; j < this.sprites.Count; j++)
			{
				this.albumObjectsContainer.GetChild(j).gameObject.SetActive(true);
				this.UpdateAlbumObject(this.albumObjectsContainer.GetChild(j).GetComponent<AlbumObject>(), j);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.albumObjectsContainer.GetComponent<RectTransform>());
			this.scrollBarResetFlag = true;
			this.waitingPanel.Deactivate();
			yield break;
		}

		// Token: 0x06002727 RID: 10023 RVA: 0x000E73E4 File Offset: 0x000E55E4
		private void LateUpdate()
		{
			if (this.scrollBarResetFlag)
			{
				this.scrollBarResetFlag = false;
				this.albumScrollRect.verticalNormalizedPosition = 1f;
				this.albumScrollRect.verticalScrollbar.value = 1f;
				return;
			}
			this.albumScrollRect.verticalScrollbar.size = 0.2f;
		}

		// Token: 0x06002728 RID: 10024 RVA: 0x000E743C File Offset: 0x000E563C
		private void UpdateUnlockedCards()
		{
			int unlockedCardsForDeck = this.GetUnlockedCardsForDeck(this.currentTab);
			int count = this.GetSpritesForTabType(this.currentTab).Count;
			this.unlockedCardsValue.text = unlockedCardsForDeck.ToString() + "/" + count.ToString();
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x000E748C File Offset: 0x000E568C
		private int GetUnlockedCardsForDeck(TabType type)
		{
			switch (type)
			{
			case TabType.EncounterCards:
				return 28 + AchievementManager.NumberOfUnlockedEncounterCards();
			case TabType.FactoryCards:
				return 12 + AchievementManager.NumberOfUnlockedFactoryCards();
			case TabType.ObjectiveCards:
				return 23 + AchievementManager.NumberOfUnlockedObjectiveCards();
			default:
				return this.GetSpritesForTabType(this.currentTab).Count;
			}
		}

		// Token: 0x0600272A RID: 10026 RVA: 0x000E74DC File Offset: 0x000E56DC
		private void UpdateAlbumObject(AlbumObject albumObject, int id)
		{
			Vector2 vector = this.cellSize[(int)this.currentTab];
			albumObject.UpdateObject(this.currentTab, this.GetSprite(id), this.GetMaskSprite((int)this.currentTab), vector, id, this.GetUnlockedState(this.currentTab, id + 1));
			if (albumObject.transform.parent == this.blur.transform)
			{
				if (this.currentTab == TabType.FactoryCards)
				{
					float num = this.blur.GetComponent<RectTransform>().rect.height / 150f;
					albumObject.GetComponent<RectTransform>().localScale = new Vector3(num, num, num);
					return;
				}
				float height = this.blur.GetComponent<RectTransform>().rect.height;
				float width = this.blur.GetComponent<RectTransform>().rect.width;
				float num2 = height / 100f;
				float num3 = width / 150f;
				if (num3 < num2)
				{
					num2 = num3;
				}
				albumObject.GetComponent<RectTransform>().localScale = new Vector3(num2, num2, num2);
			}
		}

		// Token: 0x0600272B RID: 10027 RVA: 0x000410AC File Offset: 0x0003F2AC
		public void ResetAchievements()
		{
			AchievementManager.RemoveGainedAchievements();
			this.ChangeView(this.currentTab, TabType.None);
		}

		// Token: 0x0600272C RID: 10028 RVA: 0x000410C0 File Offset: 0x0003F2C0
		public void Close()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV1);
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_back_button);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.title, Contexts.outgame);
			base.gameObject.SetActive(false);
		}

		// Token: 0x04001BF5 RID: 7157
		public ScrollRect albumScrollRect;

		// Token: 0x04001BF6 RID: 7158
		public GameObject albumObjectPrefab;

		// Token: 0x04001BF7 RID: 7159
		public Transform albumObjectsContainer;

		// Token: 0x04001BF8 RID: 7160
		public Transform card14Info;

		// Token: 0x04001BF9 RID: 7161
		public ToggleGroup toggleGroup;

		// Token: 0x04001BFA RID: 7162
		public GridLayoutGroup gridLayoutGroup;

		// Token: 0x04001BFB RID: 7163
		public TextMeshProUGUI unlockedCardsValue;

		// Token: 0x04001BFC RID: 7164
		public FactoryCardBuilder builder;

		// Token: 0x04001BFD RID: 7165
		public GameObject blur;

		// Token: 0x04001BFE RID: 7166
		public AlbumObject presentationCard;

		// Token: 0x04001BFF RID: 7167
		public List<Sprite> masks = new List<Sprite>();

		// Token: 0x04001C00 RID: 7168
		public List<Sprite> sprites = new List<Sprite>();

		// Token: 0x04001C01 RID: 7169
		public List<string> matSpriteNames = new List<string>();

		// Token: 0x04001C02 RID: 7170
		public int maxElementCount = 50;

		// Token: 0x04001C03 RID: 7171
		private TabType currentTab;

		// Token: 0x04001C04 RID: 7172
		[SerializeField]
		private List<Toggle> toggles = new List<Toggle>();

		// Token: 0x04001C05 RID: 7173
		[SerializeField]
		private WaitingPanel waitingPanel;

		// Token: 0x04001C06 RID: 7174
		[SerializeField]
		private Button removeAchievementsButton;

		// Token: 0x04001C07 RID: 7175
		public Vector2[] cellSize;

		// Token: 0x04001C08 RID: 7176
		private bool scrollBarResetFlag;

		// Token: 0x04001C09 RID: 7177
		private AlbumBundlesLoader bundlesLoader = new AlbumBundlesLoader();
	}
}
