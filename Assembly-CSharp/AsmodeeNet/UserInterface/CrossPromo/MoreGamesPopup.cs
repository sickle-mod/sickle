using System;
using System.Collections.Generic;
using AsmodeeNet.Analytics;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network.RestApi;
using AsmodeeNet.Utils;
using Scythe.Analytics;
using UnityEngine;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x0200081D RID: 2077
	public class MoreGamesPopup : BaseGroupOfProductPopup
	{
		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06003AF9 RID: 15097 RVA: 0x0004E193 File Offset: 0x0004C393
		// (set) Token: 0x06003AFA RID: 15098 RVA: 0x00152238 File Offset: 0x00150438
		public GameProductTag CurrentFilter
		{
			get
			{
				return this._currentFilter;
			}
			set
			{
				this._currentFilter = value;
				if (this._groupOfProducts[value] == null)
				{
					this.spinner.SetActive(true);
					CrossPromoCacheManager.CancelLoadMoreGame();
					CrossPromoCacheManager.LoadMoreGame(new GameProductTag?(this.CurrentFilter), delegate(ShowcaseProduct[] products)
					{
						this.spinner.SetActive(false);
						this._groupOfProducts[value] = products;
						this.ReloadProducts(products);
					}, delegate
					{
						AlertController.InstantiateAlertController(CoreApplication.Instance.LocalizationManager.GetLocalizedText("Standard.Error.Title"), CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.Error.UnableToConnect")).AddAction(CoreApplication.Instance.LocalizationManager.GetLocalizedText("Standard.Alert.Ok"), AlertController.ButtonStyle.Default, new Action(this.Dismiss));
					});
				}
				else
				{
					base.ReloadProducts(this._groupOfProducts[value]);
				}
				CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action screen_previous_nav_action;
				switch (value)
				{
				case GameProductTag.featured:
					screen_previous_nav_action = CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.click_filter_featured;
					break;
				case GameProductTag.gamer:
					screen_previous_nav_action = CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.click_filter_gamer;
					break;
				case GameProductTag.family:
					screen_previous_nav_action = CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.click_filter_family;
					break;
				case GameProductTag.board:
					screen_previous_nav_action = CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.click_filter_boardgame;
					break;
				default:
					screen_previous_nav_action = CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.automatic;
					break;
				}
				AnalyticsEvents.LogCrossPromoScreenDisplayEvent(CROSSPROMO_SCREEN_DISPLAY.screen_current.more_games, screen_previous_nav_action, null, null, null);
			}
		}

		// Token: 0x06003AFB RID: 15099 RVA: 0x00152310 File Offset: 0x00150510
		public static MoreGamesPopup InstantiateMoreGames()
		{
			MoreGamesPopup moreGamesPopup = (MoreGamesPopup)global::UnityEngine.Object.FindObjectOfType(typeof(MoreGamesPopup));
			if (moreGamesPopup == null)
			{
				moreGamesPopup = global::UnityEngine.Object.Instantiate<GameObject>(CoreApplication.Instance.InterfaceSkin.MoreGamesPopupPrefab).GetComponent<MoreGamesPopup>();
			}
			else
			{
				AsmoLogger.Error("InterstitialPopup", "Try to InstantiateMoreGames twice", null);
			}
			return moreGamesPopup;
		}

		// Token: 0x06003AFC RID: 15100 RVA: 0x0004E19B File Offset: 0x0004C39B
		private void Start()
		{
			AnalyticsEventData.NavigationButtonClicked(ScreenPreviousNavActions.click_more_games_button);
			AnalyticsEventLogger.Instance.LogScreenDisplay(Screens.cross_promo, Contexts.outgame);
			AnalyticsEvents.LogCrossPromoOpenedEvent(new CROSSPROMO_OPENED.crosspromo_type?(CROSSPROMO_OPENED.crosspromo_type.more_games), null);
			this.CurrentFilter = GameProductTag.featured;
		}

		// Token: 0x06003AFD RID: 15101 RVA: 0x0004E1C4 File Offset: 0x0004C3C4
		public void ShowFeatured(bool isOn)
		{
			if (!isOn)
			{
				return;
			}
			if (this.CurrentFilter != GameProductTag.featured)
			{
				this.CurrentFilter = GameProductTag.featured;
			}
		}

		// Token: 0x06003AFE RID: 15102 RVA: 0x0004E1D9 File Offset: 0x0004C3D9
		public void ShowGamer(bool isOn)
		{
			if (!isOn)
			{
				return;
			}
			if (this.CurrentFilter != GameProductTag.gamer)
			{
				this.CurrentFilter = GameProductTag.gamer;
			}
		}

		// Token: 0x06003AFF RID: 15103 RVA: 0x0004E1EF File Offset: 0x0004C3EF
		public void ShowFamily(bool isOn)
		{
			if (!isOn)
			{
				return;
			}
			if (this.CurrentFilter != GameProductTag.family)
			{
				this.CurrentFilter = GameProductTag.family;
			}
		}

		// Token: 0x06003B00 RID: 15104 RVA: 0x0004E205 File Offset: 0x0004C405
		public void ShowBoard(bool isOn)
		{
			if (!isOn)
			{
				return;
			}
			if (this.CurrentFilter != GameProductTag.board)
			{
				this.CurrentFilter = GameProductTag.board;
			}
		}

		// Token: 0x06003B01 RID: 15105 RVA: 0x0004E174 File Offset: 0x0004C374
		public override void Dismiss()
		{
			base.Dismiss();
		}

		// Token: 0x04002CB5 RID: 11445
		public TabToggleTMP TabToggleNew;

		// Token: 0x04002CB6 RID: 11446
		public GameObject spinner;

		// Token: 0x04002CB7 RID: 11447
		private const string _consoleModuleName = "InterstitialPopup";

		// Token: 0x04002CB8 RID: 11448
		private Dictionary<GameProductTag, ShowcaseProduct[]> _groupOfProducts = new Dictionary<GameProductTag, ShowcaseProduct[]>
		{
			{
				GameProductTag.board,
				null
			},
			{
				GameProductTag.family,
				null
			},
			{
				GameProductTag.featured,
				null
			},
			{
				GameProductTag.gamer,
				null
			}
		};

		// Token: 0x04002CB9 RID: 11449
		private GameProductTag _currentFilter;
	}
}
