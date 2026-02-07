using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.Multiplayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x0200039A RID: 922
	public class PointsCountingPresenter : MonoBehaviour
	{
		// Token: 0x06001B82 RID: 7042 RVA: 0x000ACA0C File Offset: 0x000AAC0C
		public void AnimatePointsCounting(List<PlayerEndGameStats> stats, Action callback)
		{
			this.stats = stats;
			this.endCallback = callback;
			base.gameObject.SetActive(true);
			this.enabledLogos = new List<EndPointsCountingElement>();
			for (int i = 0; i < this.logos.Count; i++)
			{
				EndPointsCountingElement elem = this.logos[i].GetComponent<EndPointsCountingElement>();
				if (stats.Any((PlayerEndGameStats x) => x.player.matFaction.faction == elem.faction))
				{
					elem = global::UnityEngine.Object.Instantiate<GameObject>(this.logos[i], base.transform).GetComponent<EndPointsCountingElement>();
					elem.GetComponent<RectTransform>().anchoredPosition = this.logos[i].GetComponent<RectTransform>().anchoredPosition;
					elem.transform.localScale = this.logos[i].transform.localScale;
					if (!PlatformManager.IsStandalone)
					{
						Vector3 vector = new Vector3(elem.transform.localPosition.x, elem.transform.localPosition.y, 0f);
						elem.transform.localPosition = vector;
						elem.transform.localRotation = Quaternion.identity;
					}
					elem.gameObject.SetActive(true);
					elem.popularityText.text = "";
					elem.scoreText.text = "";
					elem.popularityGate.SetActive(false);
					this.enabledLogos.Add(elem);
				}
			}
			if (stats == null)
			{
				Debug.LogError("Stats are empty!!");
			}
			this.StarsAnimation();
		}

		// Token: 0x06001B83 RID: 7043 RVA: 0x000ACBD0 File Offset: 0x000AADD0
		private void StarsAnimation()
		{
			PlayerEndGameStats playerEndGameStats = this.stats.Find((PlayerEndGameStats s) => s.player.GetNumberOfStars() == 6);
			if (playerEndGameStats == null)
			{
				Debug.Log("Nobody has 6 stars! Probably all human players left the game or time run out. Taking the player with the most points.");
				playerEndGameStats = this.stats[0];
			}
			Faction sixthStarGainerPlayerFaction = playerEndGameStats.player.matFaction.faction;
			EndPointsCountingElement sixthStarGainerPlayerFactionElement = this.enabledLogos.FirstOrDefault((EndPointsCountingElement x) => x.faction.Equals(sixthStarGainerPlayerFaction));
			GameObject logo = sixthStarGainerPlayerFactionElement.logo;
			for (int i = 0; i < this.enabledLogos.Count; i++)
			{
				if (this.enabledLogos[i] != sixthStarGainerPlayerFactionElement)
				{
					this.enabledLogos[i].gameObject.SetActive(false);
				}
			}
			sixthStarGainerPlayerFactionElement.transform.SetAsLastSibling();
			int numberOfStars = playerEndGameStats.player.GetNumberOfStars();
			int num = numberOfStars;
			if (num == 6)
			{
				num = 5;
			}
			float num2 = this.starsSpreadAngle / (float)num;
			float num3 = -this.starsSpreadAngle * 0.5f + num2 * 0.5f;
			sixthStarGainerPlayerFactionElement.Stars = new List<GameObject>();
			for (int j = 0; j < num; j++)
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.star);
				sixthStarGainerPlayerFactionElement.Stars.Add(gameObject);
				gameObject.transform.SetParent(sixthStarGainerPlayerFactionElement.transform, false);
				gameObject.transform.SetSiblingIndex(sixthStarGainerPlayerFactionElement.transform.childCount - 2);
				float num4 = this.starDistanceFromLogoCenter.y * Mathf.Sin(-num3 * 0.017453292f) + 0.5f;
				float num5 = this.starDistanceFromLogoCenter.y * Mathf.Cos(-num3 * 0.017453292f) + 0.5f;
				gameObject.transform.Rotate(0f, 0f, num3);
				num3 += num2;
				gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(num4 + this.starsSizeOfLogo * 0.5f, num5 + this.starsSizeOfLogo * 0.5f);
				gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(num4 - this.starsSizeOfLogo * 0.5f, num5 - this.starsSizeOfLogo * 0.5f);
				gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f);
				gameObject.SetActive(true);
			}
			sixthStarGainerPlayerFactionElement.GetComponent<RectTransform>().sizeDelta = new Vector2(this.sixthStarLogoSize.x, this.sixthStarLogoSize.x);
			this.SetSameImageColorWith0Alpha(logo.GetComponent<Image>());
			foreach (GameObject gameObject2 in sixthStarGainerPlayerFactionElement.Stars)
			{
				this.SetSameImageColorWith0Alpha(gameObject2.GetComponent<Image>());
			}
			sixthStarGainerPlayerFactionElement.GetComponent<RectTransform>().DOSizeDelta(new Vector2(this.sixthStarLogoSize.y, this.sixthStarLogoSize.y), this.sixthStarLogoFadeInTime, false).SetEase(this.sixthStarLogoFadeInEase);
			if (numberOfStars == 6)
			{
				logo.GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 1f), this.sixthStarLogoFadeInTime).SetEase(this.sixthStarLogoFadeInEase)
					.OnComplete(delegate
					{
						this.PutSixthStar(sixthStarGainerPlayerFactionElement);
					});
			}
			else
			{
				logo.GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 1f), this.sixthStarLogoFadeInTime).SetEase(this.sixthStarLogoFadeInEase)
					.OnComplete(delegate
					{
						this.ScaleDownSixthStarLogo(sixthStarGainerPlayerFactionElement.gameObject);
					});
			}
			foreach (GameObject gameObject3 in sixthStarGainerPlayerFactionElement.Stars)
			{
				gameObject3.GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 1f), this.sixthStarLogoFadeInTime).SetEase(this.sixthStarLogoFadeInEase);
			}
		}

		// Token: 0x06001B84 RID: 7044 RVA: 0x000AD038 File Offset: 0x000AB238
		public void Cleanup()
		{
			if (this.enabledLogos != null)
			{
				foreach (EndPointsCountingElement endPointsCountingElement in this.enabledLogos)
				{
					global::UnityEngine.Object.Destroy(endPointsCountingElement.gameObject);
				}
				this.enabledLogos.Clear();
			}
			this.winnerShowcase.gameObject.SetActive(false);
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x000AD0B4 File Offset: 0x000AB2B4
		private void PutSixthStar(EndPointsCountingElement sixthStarGainerPlayerFactionElement)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.star);
			sixthStarGainerPlayerFactionElement.Stars.Add(gameObject);
			gameObject.transform.SetParent(sixthStarGainerPlayerFactionElement.transform, false);
			gameObject.transform.SetSiblingIndex(sixthStarGainerPlayerFactionElement.transform.childCount - 2);
			gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f + this.sixthStarDistanceFromLogoCenter.x + this.sixthStarSize.y * 0.5f, 0.5f + this.sixthStarDistanceFromLogoCenter.y + this.sixthStarSize.y * 0.5f);
			gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f + this.sixthStarDistanceFromLogoCenter.x - this.sixthStarSize.y * 0.5f, 0.5f + this.sixthStarDistanceFromLogoCenter.y - this.sixthStarSize.y * 0.5f);
			gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(this.sixthStarSize.x, this.sixthStarSize.x);
			this.SetSameImageColorWith0Alpha(gameObject.GetComponent<Image>());
			gameObject.SetActive(true);
			gameObject.GetComponent<RectTransform>().DOSizeDelta(new Vector2(0f, 0f), this.sixthStarFadeInTime, false).SetEase(this.sixthStarFadeInEase)
				.SetDelay(this.delayBetweenShowingLogoAndSixthStar);
			gameObject.GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 1f), this.sixthStarFadeInTime).SetEase(this.sixthStarFadeInEase)
				.SetDelay(this.delayBetweenShowingLogoAndSixthStar)
				.OnComplete(delegate
				{
					this.ScaleDownSixthStarLogo(sixthStarGainerPlayerFactionElement.gameObject);
				});
		}

		// Token: 0x06001B86 RID: 7046 RVA: 0x000AD298 File Offset: 0x000AB498
		private void ScaleDownSixthStarLogo(GameObject sixthStarGainerPlayerFactionElement)
		{
			sixthStarGainerPlayerFactionElement.GetComponent<RectTransform>().DOSizeDelta(new Vector2(this.sixthStarLogoFinalSize, this.sixthStarLogoFinalSize), this.sixthStarLogoScaleDownTime, false).SetEase(this.sixthStarLogoScaleDownEase)
				.SetDelay(this.delayBetweenSixthStarAndScalingDownLogo)
				.OnComplete(new TweenCallback(this.MoveLogosHorizontally));
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x000AD2F0 File Offset: 0x000AB4F0
		private void MoveLogosHorizontally()
		{
			List<Player> players = GameController.GameManager.GetPlayers();
			this.enabledLogos = this.enabledLogos.OrderBy((EndPointsCountingElement x) => players.FindIndex((Player p) => p.matFaction.faction == x.faction)).ToList<EndPointsCountingElement>();
			float width = this.enabledLogos[0].GetComponent<RectTransform>().rect.width;
			float num = -(width + this.logosSpacing) * (float)this.enabledLogos.Count * 0.5f + width * 0.5f;
			Sequence sequence = DOTween.Sequence();
			for (int i = 0; i < this.enabledLogos.Count; i++)
			{
				this.enabledLogos[i].gameObject.SetActive(true);
				sequence.Insert(0f, this.enabledLogos[i].GetComponent<RectTransform>().DOAnchorPos(new Vector2(num + (float)i * (width + this.logosSpacing), this.enabledLogos[i].GetComponent<RectTransform>().anchoredPosition.y), this.logosSlidingTime, false).SetEase(this.logosSlidingAnimationCurve));
			}
			sequence.OnComplete(new TweenCallback(this.ShowStars));
		}

		// Token: 0x06001B88 RID: 7048 RVA: 0x000AD430 File Offset: 0x000AB630
		private void ShowStars()
		{
			int u;
			int u2;
			for (u = 0; u < this.enabledLogos.Count; u = u2 + 1)
			{
				int numberOfStars = this.stats.First((PlayerEndGameStats x) => x.player.matFaction.faction == this.enabledLogos[u].faction).player.GetNumberOfStars();
				if (numberOfStars != 6)
				{
					this.enabledLogos[u].Stars = new List<GameObject>();
					float num = this.starsSpreadAngle / (float)numberOfStars;
					float num2 = -this.starsSpreadAngle * 0.5f + num * 0.5f;
					for (int i = 0; i < numberOfStars; i++)
					{
						GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.star);
						this.enabledLogos[u].Stars.Add(gameObject);
						gameObject.transform.SetParent(this.enabledLogos[u].transform, false);
						gameObject.transform.SetSiblingIndex(this.enabledLogos[u].transform.childCount - 2);
						float num3 = this.starDistanceFromLogoCenter.y * Mathf.Sin(-num2 * 0.017453292f) + 0.5f;
						float num4 = this.starDistanceFromLogoCenter.y * Mathf.Cos(-num2 * 0.017453292f) + 0.5f;
						gameObject.transform.Rotate(0f, 0f, num2);
						num2 += num;
						gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(num3 + this.starsSizeOfLogo * 0.5f, num4 + this.starsSizeOfLogo * 0.5f);
						gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(num3 - this.starsSizeOfLogo * 0.5f, num4 - this.starsSizeOfLogo * 0.5f);
						gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f);
						gameObject.SetActive(true);
						gameObject.GetComponent<RectTransform>().localScale = Vector3.zero;
						Sequence sequence = DOTween.Sequence();
						sequence.Append(gameObject.GetComponent<RectTransform>().DOScale(1.35f, this.showStarsTime * 0.85f));
						sequence.Append(gameObject.GetComponent<RectTransform>().DOScale(1f, this.showStarsTime * 0.15f));
					}
				}
				u2 = u;
			}
			DOTween.Sequence().AppendInterval(this.showStarsTime + 0.4f).AppendCallback(delegate
			{
				this.MoveLogosUpAndShowPopularityGate();
			});
		}

		// Token: 0x06001B89 RID: 7049 RVA: 0x000AD6D4 File Offset: 0x000AB8D4
		private void MoveLogosUpAndShowPopularityGate()
		{
			Sequence sequence = DOTween.Sequence();
			for (int i = 0; i < this.enabledLogos.Count; i++)
			{
				sequence.Insert(0f, this.enabledLogos[i].GetComponent<RectTransform>().DOAnchorPos(new Vector2(this.enabledLogos[i].GetComponent<RectTransform>().anchoredPosition.x, this.enabledLogos[i].GetComponent<RectTransform>().anchoredPosition.y + this.logosUpMovementEndHeight), this.logosUpMovementTime, false).SetEase(this.logosUpMovementAnimationCurve));
				this.enabledLogos[i].multiplierText.text = "I";
				RectTransform component = this.enabledLogos[i].popularityGate.GetComponent<RectTransform>();
				component.gameObject.SetActive(true);
				sequence.Insert(0f, component.DOAnchorPos(new Vector2(component.anchoredPosition.x, component.anchoredPosition.y + this.popularityGatesDownMovementEndHeight), this.logosUpMovementTime, false).SetEase(this.logosUpMovementAnimationCurve));
			}
			sequence.OnComplete(new TweenCallback(this.CountPopularity));
		}

		// Token: 0x06001B8A RID: 7050 RVA: 0x000AD810 File Offset: 0x000ABA10
		private void CountPopularity()
		{
			int i;
			Func<PlayerEndGameStats, bool> <>9__0;
			int j;
			for (i = 0; i < this.enabledLogos.Count; i = j + 1)
			{
				IEnumerable<PlayerEndGameStats> enumerable = this.stats;
				Func<PlayerEndGameStats, bool> func;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = (PlayerEndGameStats x) => x.player.matFaction.faction == this.enabledLogos[i].faction);
				}
				PlayerEndGameStats playerEndGameStats = enumerable.First(func);
				Sequence sequence = DOTween.Sequence();
				int i1 = i;
				sequence.Append(DOTween.To(() => 0, delegate(int x)
				{
					this.enabledLogos[i1].popularityText.text = x.ToString("N0");
					int num = PopularityTrack.PopularityTier(x) - 1;
					if (this.enabledLogos[i1].LitPopularityIconLevel != num)
					{
						this.enabledLogos[i1].LitPopularityIconLevel = num;
						if (num == 1)
						{
							this.enabledLogos[i1].multiplierText.text = "II";
						}
						else if (num == 2)
						{
							this.enabledLogos[i1].multiplierText.text = "III";
						}
						DOTween.Sequence().Append(this.enabledLogos[i1].popularityIcon.GetComponent<RectTransform>().DOScale(1.2f, 0.1f)).Append(this.enabledLogos[i1].popularityIcon.GetComponent<RectTransform>().DOScale(1f, 0.1f).SetEase(Ease.InOutBounce));
					}
				}, playerEndGameStats.player.Popularity, this.popularityCountingTime).SetEase(Ease.Linear));
				if (i == 0)
				{
					sequence.AppendInterval(this.starScoringDelay);
					sequence.AppendCallback(new TweenCallback(this.AnimateStarsScore));
				}
				j = i;
			}
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x000AD93C File Offset: 0x000ABB3C
		private void AnimateStarsScore()
		{
			PointsCountingPresenter.<>c__DisplayClass68_0 CS$<>8__locals1 = new PointsCountingPresenter.<>c__DisplayClass68_0();
			CS$<>8__locals1.<>4__this = this;
			this.isShowingScores = true;
			CS$<>8__locals1.i = 0;
			while (CS$<>8__locals1.i < this.enabledLogos.Count)
			{
				PointsCountingPresenter.<>c__DisplayClass68_1 CS$<>8__locals2 = new PointsCountingPresenter.<>c__DisplayClass68_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.i1 = CS$<>8__locals2.CS$<>8__locals1.i;
				PointsCountingPresenter.<>c__DisplayClass68_1 CS$<>8__locals3 = CS$<>8__locals2;
				IEnumerable<PlayerEndGameStats> enumerable = this.stats;
				Func<PlayerEndGameStats, bool> func;
				if ((func = CS$<>8__locals2.CS$<>8__locals1.<>9__0) == null)
				{
					func = (CS$<>8__locals2.CS$<>8__locals1.<>9__0 = (PlayerEndGameStats x) => x.player.matFaction.faction == CS$<>8__locals2.CS$<>8__locals1.<>4__this.enabledLogos[CS$<>8__locals2.CS$<>8__locals1.i].faction);
				}
				CS$<>8__locals3.statsPlayer = enumerable.First(func);
				this.ChangeMultiplier(this.enabledLogos[CS$<>8__locals2.CS$<>8__locals1.i].multiplierText, PopularityTrack.StarsMultiplier(CS$<>8__locals2.statsPlayer.player.Popularity));
				this.enabledLogos[CS$<>8__locals2.i1].popularityText.DOFade(0f, this.popularityValueDisappearTime);
				Sequence sequence = DOTween.Sequence();
				sequence.AppendInterval(this.popularityGateChangeMultiplierTime * 1.2f);
				for (int i = this.enabledLogos[CS$<>8__locals2.CS$<>8__locals1.i].Stars.Count - 1; i >= 0; i--)
				{
					GameObject gameObject = this.enabledLogos[CS$<>8__locals2.CS$<>8__locals1.i].Stars[i];
					Sequence sequence2 = sequence.Append(gameObject.transform.DOScale(1.1f, 0.2f * this.starDisappearTime));
					TweenCallback tweenCallback;
					if ((tweenCallback = CS$<>8__locals2.<>9__1) == null)
					{
						tweenCallback = (CS$<>8__locals2.<>9__1 = delegate
						{
							CS$<>8__locals2.CS$<>8__locals1.<>4__this.AddPointsToScore(CS$<>8__locals2.i1, PopularityTrack.StarsMultiplier(CS$<>8__locals2.statsPlayer.player.Popularity), true);
						});
					}
					sequence2.AppendCallback(tweenCallback).Append(gameObject.transform.DOScale(0f, 0.8f * this.starDisappearTime)).Join(gameObject.GetComponent<Image>().DOFade(0f, 0.8f * this.starDisappearTime))
						.AppendInterval(this.starPauseBetweenTime);
				}
				if (CS$<>8__locals2.statsPlayer.player.GetNumberOfStars() == 6 || ((GameController.Instance.GameFinishedTimeOut || MultiplayerController.Instance.SpectatorMode) && CS$<>8__locals2.CS$<>8__locals1.i == 0))
				{
					sequence.AppendCallback(new TweenCallback(this.AnimateHexScoring));
				}
				int i2 = CS$<>8__locals1.i;
				CS$<>8__locals1.i = i2 + 1;
			}
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x000ADBA4 File Offset: 0x000ABDA4
		private void AnimateHexScoring()
		{
			PointsCountingPresenter.<>c__DisplayClass69_0 CS$<>8__locals1 = new PointsCountingPresenter.<>c__DisplayClass69_0();
			CS$<>8__locals1.<>4__this = this;
			int num = this.stats.Max((PlayerEndGameStats x) => x.territoryPoints);
			bool flag = false;
			CS$<>8__locals1.i = 0;
			while (CS$<>8__locals1.i < this.enabledLogos.Count)
			{
				PointsCountingPresenter.<>c__DisplayClass69_1 CS$<>8__locals2 = new PointsCountingPresenter.<>c__DisplayClass69_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.i1 = CS$<>8__locals2.CS$<>8__locals1.i;
				EndPointsCountingElement endPointsCountingElement = this.enabledLogos[CS$<>8__locals2.CS$<>8__locals1.i];
				CS$<>8__locals2.hexText = endPointsCountingElement.hexCounter.GetComponentInChildren<Text>();
				CS$<>8__locals2.hexText.text = "0";
				PointsCountingPresenter.<>c__DisplayClass69_1 CS$<>8__locals3 = CS$<>8__locals2;
				IEnumerable<PlayerEndGameStats> enumerable = this.stats;
				Func<PlayerEndGameStats, bool> func;
				if ((func = CS$<>8__locals2.CS$<>8__locals1.<>9__1) == null)
				{
					func = (CS$<>8__locals2.CS$<>8__locals1.<>9__1 = (PlayerEndGameStats x) => x.player.matFaction.faction == CS$<>8__locals2.CS$<>8__locals1.<>4__this.enabledLogos[CS$<>8__locals2.CS$<>8__locals1.i].faction);
				}
				CS$<>8__locals3.statsPlayer = enumerable.First(func);
				endPointsCountingElement.hexCounter.SetActive(true);
				endPointsCountingElement.hexCounter.transform.localScale = new Vector3(1f, 0f, 1f);
				this.ChangeMultiplier(this.enabledLogos[CS$<>8__locals2.CS$<>8__locals1.i].multiplierText, PopularityTrack.TerritoryMultiplier(CS$<>8__locals2.statsPlayer.player.Popularity));
				Sequence sequence = DOTween.Sequence();
				sequence.Append(endPointsCountingElement.hexCounter.transform.DOScaleY(1f, this.restScoringAppearTime));
				int num2 = CS$<>8__locals2.statsPlayer.territoryPoints / PopularityTrack.TerritoryMultiplier(CS$<>8__locals2.statsPlayer.player.Popularity);
				CS$<>8__locals2.hexText.text = num2.ToString();
				for (int i = num2 - 1; i >= 0; i--)
				{
					sequence.AppendInterval((i == num2) ? this.restScoringFirstHold : this.restScoringDelay);
					int j1 = i;
					sequence.AppendCallback(delegate
					{
						CS$<>8__locals2.hexText.text = j1.ToString();
						CS$<>8__locals2.CS$<>8__locals1.<>4__this.AddPointsToScore(CS$<>8__locals2.i1, PopularityTrack.TerritoryMultiplier(CS$<>8__locals2.statsPlayer.player.Popularity), true);
					});
					if (i == 0)
					{
						sequence.AppendInterval(this.restScoringLastHold);
					}
				}
				if (CS$<>8__locals2.statsPlayer.territoryPoints == num && !flag)
				{
					flag = true;
					for (int j = 0; j < this.enabledLogos.Count; j++)
					{
						int num3 = j;
						sequence.Append(this.enabledLogos[num3].hexCounter.transform.DOScaleY(0f, this.restScoringDisappearTime));
					}
					sequence.AppendInterval(this.restScoringPauseBetweenTime);
					sequence.AppendCallback(new TweenCallback(this.AnimateResourcesScoring));
				}
				int i2 = CS$<>8__locals1.i;
				CS$<>8__locals1.i = i2 + 1;
			}
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x000ADE70 File Offset: 0x000AC070
		private void AnimateResourcesScoring()
		{
			PointsCountingPresenter.<>c__DisplayClass70_0 CS$<>8__locals1 = new PointsCountingPresenter.<>c__DisplayClass70_0();
			CS$<>8__locals1.<>4__this = this;
			int num = this.stats.Max((PlayerEndGameStats x) => x.resourcePoints);
			bool flag = false;
			CS$<>8__locals1.i = 0;
			while (CS$<>8__locals1.i < this.enabledLogos.Count)
			{
				PointsCountingPresenter.<>c__DisplayClass70_1 CS$<>8__locals2 = new PointsCountingPresenter.<>c__DisplayClass70_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.i1 = CS$<>8__locals2.CS$<>8__locals1.i;
				EndPointsCountingElement endPointsCountingElement = this.enabledLogos[CS$<>8__locals2.CS$<>8__locals1.i];
				CS$<>8__locals2.resourcesText = endPointsCountingElement.resourcesCounter.GetComponentInChildren<Text>();
				CS$<>8__locals2.resourcesText.text = "0";
				PointsCountingPresenter.<>c__DisplayClass70_1 CS$<>8__locals3 = CS$<>8__locals2;
				IEnumerable<PlayerEndGameStats> enumerable = this.stats;
				Func<PlayerEndGameStats, bool> func;
				if ((func = CS$<>8__locals2.CS$<>8__locals1.<>9__1) == null)
				{
					func = (CS$<>8__locals2.CS$<>8__locals1.<>9__1 = (PlayerEndGameStats x) => x.player.matFaction.faction == CS$<>8__locals2.CS$<>8__locals1.<>4__this.enabledLogos[CS$<>8__locals2.CS$<>8__locals1.i].faction);
				}
				CS$<>8__locals3.statsPlayer = enumerable.First(func);
				endPointsCountingElement.resourcesCounter.SetActive(true);
				endPointsCountingElement.resourcesCounter.transform.localScale = new Vector3(1f, 0f, 1f);
				this.ChangeMultiplier(this.enabledLogos[CS$<>8__locals2.CS$<>8__locals1.i].multiplierText, PopularityTrack.ResourceMultiplier(CS$<>8__locals2.statsPlayer.player.Popularity));
				Sequence sequence = DOTween.Sequence();
				sequence.Append(endPointsCountingElement.resourcesCounter.transform.DOScaleY(1f, this.restScoringAppearTime));
				int num2 = CS$<>8__locals2.statsPlayer.resourcePoints / PopularityTrack.ResourceMultiplier(CS$<>8__locals2.statsPlayer.player.Popularity);
				CS$<>8__locals2.resourcesText.text = num2.ToString();
				for (int i = num2 - 1; i >= 0; i--)
				{
					sequence.AppendInterval((i == num2) ? this.restScoringFirstHold : this.restScoringDelay);
					int j1 = i;
					sequence.AppendCallback(delegate
					{
						CS$<>8__locals2.resourcesText.text = j1.ToString();
						CS$<>8__locals2.CS$<>8__locals1.<>4__this.AddPointsToScore(CS$<>8__locals2.i1, PopularityTrack.ResourceMultiplier(CS$<>8__locals2.statsPlayer.player.Popularity), true);
					});
					if (i == 0)
					{
						sequence.AppendInterval(this.restScoringLastHold);
					}
				}
				if (CS$<>8__locals2.statsPlayer.resourcePoints == num && !flag)
				{
					flag = true;
					for (int j = 0; j < this.enabledLogos.Count; j++)
					{
						int num3 = j;
						sequence.Append(this.enabledLogos[num3].resourcesCounter.transform.DOScaleY(0f, this.restScoringDisappearTime));
						sequence.Join(this.enabledLogos[num3].popularityGate.transform.DOScaleY(0f, this.popularityGateDisappearTime));
						sequence.Join(this.enabledLogos[num3].popularityGate.GetComponent<CanvasGroup>().DOFade(0f, this.popularityGateDisappearTime));
					}
					sequence.AppendInterval(this.restScoringPauseBetweenTime);
					sequence.AppendCallback(new TweenCallback(this.AnimateStructuresScoring));
				}
				int i2 = CS$<>8__locals1.i;
				CS$<>8__locals1.i = i2 + 1;
			}
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x000AE1A8 File Offset: 0x000AC3A8
		private void AnimateStructuresScoring()
		{
			int num = this.stats.Max((PlayerEndGameStats x) => x.structurePoints);
			bool flag = false;
			int i;
			Func<PlayerEndGameStats, bool> <>9__1;
			int i2;
			for (i = 0; i < this.enabledLogos.Count; i = i2 + 1)
			{
				int i1 = i;
				EndPointsCountingElement endPointsCountingElement = this.enabledLogos[i];
				Text structureText = endPointsCountingElement.structureCounter.GetComponentInChildren<Text>();
				structureText.text = "0";
				IEnumerable<PlayerEndGameStats> enumerable = this.stats;
				Func<PlayerEndGameStats, bool> func;
				if ((func = <>9__1) == null)
				{
					func = (<>9__1 = (PlayerEndGameStats x) => x.player.matFaction.faction == this.enabledLogos[i].faction);
				}
				PlayerEndGameStats playerEndGameStats = enumerable.First(func);
				endPointsCountingElement.structureCounter.SetActive(true);
				endPointsCountingElement.structureCounter.transform.localScale = new Vector3(1f, 0f, 1f);
				Sequence sequence = DOTween.Sequence();
				sequence.Append(endPointsCountingElement.structureCounter.transform.DOScaleY(1f, this.restScoringAppearTime));
				structureText.text = playerEndGameStats.structurePoints.ToString();
				for (int k = playerEndGameStats.structurePoints - 1; k >= 0; k--)
				{
					sequence.AppendInterval((k == playerEndGameStats.structurePoints) ? this.restScoringFirstHold : this.restScoringDelay);
					int j1 = k;
					sequence.AppendCallback(delegate
					{
						structureText.text = j1.ToString();
						this.AddPointsToScore(i1, 1, false);
					});
					if (k == 0)
					{
						sequence.AppendInterval(this.restScoringLastHold);
					}
				}
				if (playerEndGameStats.structurePoints == num && !flag)
				{
					flag = true;
					for (int j = 0; j < this.enabledLogos.Count; j++)
					{
						int num2 = j;
						sequence.Append(this.enabledLogos[num2].structureCounter.transform.DOScaleY(0f, this.restScoringDisappearTime));
					}
					sequence.AppendInterval(this.restScoringPauseBetweenTime);
					sequence.AppendCallback(new TweenCallback(this.AnimateCoinsScoring));
				}
				i2 = i;
			}
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x000AE424 File Offset: 0x000AC624
		private void AnimateCoinsScoring()
		{
			int num = this.stats.Max((PlayerEndGameStats x) => x.coinPoints);
			bool flag = false;
			int i;
			Func<PlayerEndGameStats, bool> <>9__1;
			int i2;
			for (i = 0; i < this.enabledLogos.Count; i = i2 + 1)
			{
				int i1 = i;
				EndPointsCountingElement endPointsCountingElement = this.enabledLogos[i];
				Text coinText = endPointsCountingElement.coinCounter.GetComponentInChildren<Text>();
				coinText.text = "0";
				IEnumerable<PlayerEndGameStats> enumerable = this.stats;
				Func<PlayerEndGameStats, bool> func;
				if ((func = <>9__1) == null)
				{
					func = (<>9__1 = (PlayerEndGameStats x) => x.player.matFaction.faction == this.enabledLogos[i].faction);
				}
				PlayerEndGameStats playerEndGameStats = enumerable.First(func);
				endPointsCountingElement.coinCounter.SetActive(true);
				endPointsCountingElement.coinCounter.transform.localScale = new Vector3(1f, 0f, 1f);
				Sequence sequence = DOTween.Sequence();
				sequence.Append(endPointsCountingElement.coinCounter.transform.DOScaleY(1f, this.restScoringAppearTime));
				coinText.text = playerEndGameStats.coinPoints.ToString();
				for (int k = playerEndGameStats.coinPoints - 1; k >= 0; k--)
				{
					sequence.AppendInterval((k == playerEndGameStats.coinPoints) ? this.restScoringFirstHold : this.coinsScoringDelay);
					int j1 = k;
					sequence.AppendCallback(delegate
					{
						coinText.text = j1.ToString();
						this.AddPointsToScore(i1, 1, false);
					});
					if (k == 0)
					{
						sequence.AppendInterval(this.restScoringLastHold);
					}
				}
				if (playerEndGameStats.coinPoints == num && !flag)
				{
					flag = true;
					for (int j = 0; j < this.enabledLogos.Count; j++)
					{
						int num2 = j;
						sequence.Append(this.enabledLogos[num2].coinCounter.transform.DOScaleY(0f, this.restScoringDisappearTime));
					}
					sequence.AppendInterval(this.restScoringPauseBetweenTime);
					sequence.AppendCallback(new TweenCallback(this.AnimateWinnerShowcase));
				}
				i2 = i;
			}
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x000AE6A0 File Offset: 0x000AC8A0
		private void AnimateFlagsScoring()
		{
			PointsCountingPresenter.<>c__DisplayClass73_0 CS$<>8__locals1 = new PointsCountingPresenter.<>c__DisplayClass73_0();
			CS$<>8__locals1.<>4__this = this;
			int num = new FindWinner(GameController.GameManager).CalculateAlbionBonusPoints(this.stats.Find((PlayerEndGameStats x) => x.player.matFaction.faction == Faction.Albion).player);
			bool flag = false;
			CS$<>8__locals1.value = 0;
			int i;
			int l;
			for (i = 0; i < this.enabledLogos.Count; i = l + 1)
			{
				if (this.stats.First((PlayerEndGameStats x) => x.player.matFaction.faction == CS$<>8__locals1.<>4__this.enabledLogos[i].faction).player.matFaction.faction == Faction.Albion)
				{
					CS$<>8__locals1.value = i;
					break;
				}
				l = i;
			}
			CS$<>8__locals1.i1 = CS$<>8__locals1.value;
			EndPointsCountingElement endPointsCountingElement = this.enabledLogos[CS$<>8__locals1.value];
			CS$<>8__locals1.flagText = endPointsCountingElement.flagsCounter.GetComponentInChildren<Text>();
			CS$<>8__locals1.flagText.text = "0";
			PlayerEndGameStats playerEndGameStats = this.stats.First((PlayerEndGameStats x) => x.player.matFaction.faction == CS$<>8__locals1.<>4__this.enabledLogos[CS$<>8__locals1.value].faction);
			endPointsCountingElement.flagsCounter.SetActive(true);
			endPointsCountingElement.flagsCounter.transform.localScale = new Vector3(1f, 0f, 1f);
			Sequence sequence = DOTween.Sequence();
			sequence.Append(endPointsCountingElement.flagsCounter.transform.DOScaleY(1f, this.restScoringAppearTime));
			if (playerEndGameStats.player.matFaction.faction != Faction.Albion)
			{
				CS$<>8__locals1.flagText.text = "0";
			}
			else
			{
				CS$<>8__locals1.flagText.text = num.ToString();
				for (int j = num - 1; j >= 0; j--)
				{
					sequence.AppendInterval((j == num) ? this.restScoringFirstHold : this.restScoringDelay);
					int j1 = j;
					sequence.AppendCallback(delegate
					{
						CS$<>8__locals1.flagText.text = j1.ToString();
						CS$<>8__locals1.<>4__this.AddPointsToScore(CS$<>8__locals1.i1, 1, true);
					});
					if (j == 0)
					{
						sequence.AppendInterval(this.restScoringLastHold);
					}
				}
			}
			if (!flag)
			{
				for (int k = 0; k < this.enabledLogos.Count; k++)
				{
					int num2 = k;
					sequence.Append(this.enabledLogos[num2].flagsCounter.transform.DOScaleY(0f, this.restScoringDisappearTime));
				}
				sequence.AppendInterval(this.restScoringPauseBetweenTime);
				if (this.stats.Any((PlayerEndGameStats x) => x.player.matFaction.faction == Faction.Togawa))
				{
					sequence.AppendCallback(new TweenCallback(this.AnimateTrapsScoring));
					return;
				}
				if (this.stats.Any((PlayerEndGameStats x) => x.player.matFaction.faction == Faction.Polania) && GameController.GameManager.players.Count >= 6)
				{
					sequence.AppendCallback(new TweenCallback(this.AnimateEncountersScoring));
					return;
				}
				sequence.AppendCallback(new TweenCallback(this.AnimateWinnerShowcase));
			}
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x000AE9E0 File Offset: 0x000ACBE0
		private void AnimateTrapsScoring()
		{
			PointsCountingPresenter.<>c__DisplayClass74_0 CS$<>8__locals1 = new PointsCountingPresenter.<>c__DisplayClass74_0();
			CS$<>8__locals1.<>4__this = this;
			int num = new FindWinner(GameController.GameManager).CalculateTogawaBonusPoints(this.stats.Find((PlayerEndGameStats x) => x.player.matFaction.faction == Faction.Togawa).player);
			bool flag = false;
			CS$<>8__locals1.value = 0;
			int i;
			int l;
			for (i = 0; i < this.enabledLogos.Count; i = l + 1)
			{
				if (this.stats.First((PlayerEndGameStats x) => x.player.matFaction.faction == CS$<>8__locals1.<>4__this.enabledLogos[i].faction).player.matFaction.faction == Faction.Togawa)
				{
					CS$<>8__locals1.value = i;
					break;
				}
				l = i;
			}
			CS$<>8__locals1.i1 = CS$<>8__locals1.value;
			EndPointsCountingElement endPointsCountingElement = this.enabledLogos[CS$<>8__locals1.value];
			CS$<>8__locals1.trapsText = endPointsCountingElement.trapsCounter.GetComponentInChildren<Text>();
			CS$<>8__locals1.trapsText.text = "0";
			this.stats.First((PlayerEndGameStats x) => x.player.matFaction.faction == CS$<>8__locals1.<>4__this.enabledLogos[CS$<>8__locals1.value].faction);
			endPointsCountingElement.trapsCounter.SetActive(true);
			endPointsCountingElement.trapsCounter.transform.localScale = new Vector3(1f, 0f, 1f);
			Sequence sequence = DOTween.Sequence();
			sequence.Append(endPointsCountingElement.trapsCounter.transform.DOScaleY(1f, this.restScoringAppearTime));
			CS$<>8__locals1.trapsText.text = num.ToString();
			for (int j = num - 1; j >= 0; j--)
			{
				sequence.AppendInterval((j == num) ? this.restScoringFirstHold : this.restScoringDelay);
				int j1 = j;
				sequence.AppendCallback(delegate
				{
					CS$<>8__locals1.trapsText.text = j1.ToString();
					CS$<>8__locals1.<>4__this.AddPointsToScore(CS$<>8__locals1.i1, 1, true);
				});
				if (j == 0)
				{
					sequence.AppendInterval(this.restScoringLastHold);
				}
			}
			if (!flag)
			{
				for (int k = 0; k < this.enabledLogos.Count; k++)
				{
					int num2 = k;
					sequence.Append(this.enabledLogos[num2].trapsCounter.transform.DOScaleY(0f, this.restScoringDisappearTime));
				}
				sequence.AppendInterval(this.restScoringPauseBetweenTime);
				if (this.stats.Any((PlayerEndGameStats x) => x.player.matFaction.faction == Faction.Polania) && GameController.GameManager.players.Count >= 6)
				{
					sequence.AppendCallback(new TweenCallback(this.AnimateEncountersScoring));
					return;
				}
				sequence.AppendCallback(new TweenCallback(this.AnimateWinnerShowcase));
			}
		}

		// Token: 0x06001B92 RID: 7058 RVA: 0x000AECBC File Offset: 0x000ACEBC
		private void AnimateEncountersScoring()
		{
			PointsCountingPresenter.<>c__DisplayClass75_0 CS$<>8__locals1 = new PointsCountingPresenter.<>c__DisplayClass75_0();
			CS$<>8__locals1.<>4__this = this;
			int num = new FindWinner(GameController.GameManager).CalculatePolaniaBonusPoints(this.stats.Find((PlayerEndGameStats x) => x.player.matFaction.faction == Faction.Polania).player, GameController.GameManager.players.Count);
			bool flag = false;
			CS$<>8__locals1.value = 0;
			int i;
			int l;
			for (i = 0; i < this.enabledLogos.Count; i = l + 1)
			{
				if (this.stats.First((PlayerEndGameStats x) => x.player.matFaction.faction == CS$<>8__locals1.<>4__this.enabledLogos[i].faction).player.matFaction.faction == Faction.Polania)
				{
					CS$<>8__locals1.value = i;
					break;
				}
				l = i;
			}
			CS$<>8__locals1.i1 = CS$<>8__locals1.value;
			EndPointsCountingElement endPointsCountingElement = this.enabledLogos[CS$<>8__locals1.value];
			CS$<>8__locals1.encountersText = endPointsCountingElement.encountersCounter.GetComponentInChildren<Text>();
			CS$<>8__locals1.encountersText.text = "0";
			this.stats.First((PlayerEndGameStats x) => x.player.matFaction.faction == CS$<>8__locals1.<>4__this.enabledLogos[CS$<>8__locals1.value].faction);
			endPointsCountingElement.encountersCounter.SetActive(true);
			endPointsCountingElement.encountersCounter.transform.localScale = new Vector3(1f, 0f, 1f);
			Sequence sequence = DOTween.Sequence();
			sequence.Append(endPointsCountingElement.encountersCounter.transform.DOScaleY(1f, this.restScoringAppearTime));
			CS$<>8__locals1.encountersText.text = num.ToString();
			for (int j = num - 1; j >= 0; j--)
			{
				sequence.AppendInterval((j == num) ? this.restScoringFirstHold : this.restScoringDelay);
				int j1 = j;
				sequence.AppendCallback(delegate
				{
					CS$<>8__locals1.encountersText.text = j1.ToString();
					CS$<>8__locals1.<>4__this.AddPointsToScore(CS$<>8__locals1.i1, 1, true);
				});
				if (j == 0)
				{
					sequence.AppendInterval(this.restScoringLastHold);
				}
			}
			if (!flag)
			{
				for (int k = 0; k < this.enabledLogos.Count; k++)
				{
					int num2 = k;
					sequence.Append(this.enabledLogos[num2].encountersCounter.transform.DOScaleY(0f, this.restScoringDisappearTime));
				}
				sequence.AppendInterval(this.restScoringPauseBetweenTime);
				sequence.AppendCallback(new TweenCallback(this.AnimateWinnerShowcase));
			}
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x000AEF50 File Offset: 0x000AD150
		private void AnimateWinnerShowcase()
		{
			this.isShowingScores = false;
			DOTween.Sequence().AppendInterval(this.winnerSequenceCompletionDelay).AppendCallback(delegate
			{
				this.endCallback();
			});
			int num = this.enabledLogos.FindIndex((EndPointsCountingElement x) => x.faction == this.stats[0].player.matFaction.faction);
			for (int i = 0; i < this.enabledLogos.Count; i++)
			{
				if (i == num)
				{
					this.AnimateWinner(this.enabledLogos[i]);
				}
				else
				{
					this.AnimateLoser(this.enabledLogos[i]);
				}
			}
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x000AEFE0 File Offset: 0x000AD1E0
		private void AnimateWinner(EndPointsCountingElement winner)
		{
			TextMeshProUGUI winnerText = this.winnerShowcase.GetComponentInChildren<TextMeshProUGUI>();
			CanvasGroup component = this.winnerShowcase.GetComponent<CanvasGroup>();
			DOTween.Sequence().AppendInterval(this.winnerDelay).Append(winner.GetComponent<RectTransform>().DOAnchorPosX(0f, this.winnerMoveTime, false).SetEase(this.winnerMoveAnimationCurve))
				.Join(winner.GetComponent<RectTransform>().DOSizeDelta(new Vector2(this.winnerLogoSize, this.winnerLogoSize), this.winnerEnlargeTime, false).SetEase(this.winnerEnlargeAnimationCurve))
				.AppendInterval(this.winnerShowTime)
				.Append(winner.GetComponent<CanvasGroup>().DOFade(0f, this.winnerDisappearTime))
				.Join(component.DOFade(0f, this.winnerDisappearTime));
			Sequence sequence = DOTween.Sequence();
			component.alpha = 1f;
			this.winnerShowcase.transform.localScale = new Vector3(0f, 1f, 1f);
			this.winnerShowcase.gameObject.SetActive(true);
			sequence.AppendInterval(this.winnerTitleDelay).Append(winner.scoreText.transform.DOScale(0f, this.winnerTitleShowTime / 2f)).AppendCallback(delegate
			{
				winnerText.text = ScriptLocalization.Get(string.Format("GameScene/Victory{0}", winner.faction)).ToUpper();
			})
				.Append(this.winnerShowcase.transform.DOScaleX(1f, this.winnerTitleShowTime));
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x00039DD4 File Offset: 0x00037FD4
		private void AnimateLoser(EndPointsCountingElement loser)
		{
			loser.transform.DOScaleX(0f, this.winnerOthersDisappearTime);
			loser.GetComponent<CanvasGroup>().DOFade(0f, this.winnerOthersDisappearTime);
		}

		// Token: 0x06001B96 RID: 7062 RVA: 0x000AF17C File Offset: 0x000AD37C
		private void ChangeMultiplier(Text multiplierText, int multiplierValue)
		{
			DOTween.Sequence().Append(multiplierText.DOFade(0f, this.popularityGateChangeMultiplierTime / 2f)).AppendCallback(delegate
			{
				multiplierText.text = string.Format("<size=7>x</size>{0}", multiplierValue);
			})
				.Append(multiplierText.DOFade(1f, this.popularityGateChangeMultiplierTime / 2f));
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x000AF1F8 File Offset: 0x000AD3F8
		private void Update()
		{
			if (this.isShowingScores)
			{
				for (int i = 0; i < this.enabledLogos.Count; i++)
				{
					if ((float)this.enabledLogos[i].ScoreTarget > this.enabledLogos[i].ScoreCurrent)
					{
						this.enabledLogos[i].ScoreCurrent += this.scoreIncrementSpeed * Time.deltaTime;
						if ((float)this.enabledLogos[i].ScoreTarget < this.enabledLogos[i].ScoreCurrent)
						{
							this.enabledLogos[i].ScoreCurrent = (float)this.enabledLogos[i].ScoreTarget;
						}
						this.enabledLogos[i].scoreText.text = ((int)this.enabledLogos[i].ScoreCurrent).ToString();
					}
				}
			}
		}

		// Token: 0x06001B98 RID: 7064 RVA: 0x000AF2F0 File Offset: 0x000AD4F0
		private void AddPointsToScore(int logoIndex, int points, bool bounce = true)
		{
			this.enabledLogos[logoIndex].ScoreTarget += points;
			if (bounce)
			{
				DOTween.Sequence().Append(this.enabledLogos[logoIndex].popularityIcon.GetComponent<RectTransform>().DOScale(1.2f, 0.1f)).Append(this.enabledLogos[logoIndex].popularityIcon.GetComponent<RectTransform>().DOScale(1f, 0.1f).SetEase(Ease.InOutBounce));
			}
		}

		// Token: 0x06001B99 RID: 7065 RVA: 0x000AF37C File Offset: 0x000AD57C
		private void SetSameImageColorWith0Alpha(Image image)
		{
			Color color = image.color;
			color.a = 0f;
			image.color = color;
		}

		// Token: 0x0400136D RID: 4973
		[Header("References")]
		[SerializeField]
		private RectTransform UICanvas;

		// Token: 0x0400136E RID: 4974
		[SerializeField]
		private List<GameObject> logos;

		// Token: 0x0400136F RID: 4975
		[SerializeField]
		private GameObject star;

		// Token: 0x04001370 RID: 4976
		[SerializeField]
		private GameObject winnerShowcase;

		// Token: 0x04001371 RID: 4977
		[Header("Show 6th star stage")]
		[SerializeField]
		private float sixthStarLogoFadeInTime = 1f;

		// Token: 0x04001372 RID: 4978
		[SerializeField]
		private float starsSpreadAngle = 60f;

		// Token: 0x04001373 RID: 4979
		[SerializeField]
		[Range(0f, 1f)]
		private float starsSizeOfLogo = 0.5f;

		// Token: 0x04001374 RID: 4980
		[SerializeField]
		private Vector2 starDistanceFromLogoCenter = new Vector2(0f, 1f);

		// Token: 0x04001375 RID: 4981
		[SerializeField]
		private Vector2 sixthStarLogoSize = new Vector2(500f, 200f);

		// Token: 0x04001376 RID: 4982
		[SerializeField]
		private Ease sixthStarLogoFadeInEase = Ease.OutExpo;

		// Token: 0x04001377 RID: 4983
		[SerializeField]
		private float delayBetweenShowingLogoAndSixthStar = 1f;

		// Token: 0x04001378 RID: 4984
		[SerializeField]
		private float sixthStarFadeInTime = 1f;

		// Token: 0x04001379 RID: 4985
		[SerializeField]
		private Vector2 sixthStarSize = new Vector2(500f, 0.5f);

		// Token: 0x0400137A RID: 4986
		[SerializeField]
		private Vector2 sixthStarDistanceFromLogoCenter = new Vector2(0f, 0.2f);

		// Token: 0x0400137B RID: 4987
		[SerializeField]
		private Ease sixthStarFadeInEase = Ease.OutExpo;

		// Token: 0x0400137C RID: 4988
		[SerializeField]
		private float delayBetweenSixthStarAndScalingDownLogo = 0.2f;

		// Token: 0x0400137D RID: 4989
		[SerializeField]
		private float sixthStarLogoScaleDownTime = 1f;

		// Token: 0x0400137E RID: 4990
		[SerializeField]
		private float sixthStarLogoFinalSize = 100f;

		// Token: 0x0400137F RID: 4991
		[SerializeField]
		private Ease sixthStarLogoScaleDownEase = Ease.OutExpo;

		// Token: 0x04001380 RID: 4992
		[Header("Show all factions stage")]
		[SerializeField]
		private float logosSpacing = 12f;

		// Token: 0x04001381 RID: 4993
		[SerializeField]
		private float logosSlidingTime = 2f;

		// Token: 0x04001382 RID: 4994
		[SerializeField]
		private float showStarsTime = 0.3f;

		// Token: 0x04001383 RID: 4995
		[SerializeField]
		private AnimationCurve logosSlidingAnimationCurve;

		// Token: 0x04001384 RID: 4996
		[SerializeField]
		private float logosUpMovementEndHeight = 50f;

		// Token: 0x04001385 RID: 4997
		[SerializeField]
		private float popularityGatesDownMovementEndHeight = -150f;

		// Token: 0x04001386 RID: 4998
		[SerializeField]
		private float logosUpMovementTime = 1f;

		// Token: 0x04001387 RID: 4999
		[SerializeField]
		private AnimationCurve logosUpMovementAnimationCurve;

		// Token: 0x04001388 RID: 5000
		[Header("Popularity count stage")]
		[SerializeField]
		private float popularityCountingTime;

		// Token: 0x04001389 RID: 5001
		[SerializeField]
		private float popularityValueDisappearTime = 0.3f;

		// Token: 0x0400138A RID: 5002
		[Header("Star scoring")]
		[SerializeField]
		private float scoreIncrementSpeed;

		// Token: 0x0400138B RID: 5003
		[SerializeField]
		private float starScoringDelay;

		// Token: 0x0400138C RID: 5004
		[SerializeField]
		private float starDisappearTime = 0.3f;

		// Token: 0x0400138D RID: 5005
		[SerializeField]
		private float starPauseBetweenTime = 0.2f;

		// Token: 0x0400138E RID: 5006
		[Header("Rest scoring")]
		[SerializeField]
		private float restScoringDelay = 0.25f;

		// Token: 0x0400138F RID: 5007
		[SerializeField]
		private float restScoringAppearTime = 0.3f;

		// Token: 0x04001390 RID: 5008
		[SerializeField]
		private float restScoringDisappearTime = 0.2f;

		// Token: 0x04001391 RID: 5009
		[SerializeField]
		private float restScoringPauseBetweenTime = 0.3f;

		// Token: 0x04001392 RID: 5010
		[SerializeField]
		private float restScoringFirstHold = 0.5f;

		// Token: 0x04001393 RID: 5011
		[SerializeField]
		private float restScoringLastHold = 1f;

		// Token: 0x04001394 RID: 5012
		[SerializeField]
		private float popularityGateDisappearTime = 0.3f;

		// Token: 0x04001395 RID: 5013
		[SerializeField]
		private float popularityGateChangeMultiplierTime = 0.3f;

		// Token: 0x04001396 RID: 5014
		[Header("Coins scoring")]
		[SerializeField]
		private float coinsScoringDelay = 0.05f;

		// Token: 0x04001397 RID: 5015
		[Header("Winner showcase")]
		[SerializeField]
		private float winnerLogoSize = 100f;

		// Token: 0x04001398 RID: 5016
		[SerializeField]
		private float winnerMoveTime = 0.3f;

		// Token: 0x04001399 RID: 5017
		[SerializeField]
		private float winnerDelay = 0.2f;

		// Token: 0x0400139A RID: 5018
		[SerializeField]
		private AnimationCurve winnerMoveAnimationCurve;

		// Token: 0x0400139B RID: 5019
		[SerializeField]
		private float winnerEnlargeTime = 1.4f;

		// Token: 0x0400139C RID: 5020
		[SerializeField]
		private AnimationCurve winnerEnlargeAnimationCurve;

		// Token: 0x0400139D RID: 5021
		[SerializeField]
		private float winnerOthersDisappearTime = 0.3f;

		// Token: 0x0400139E RID: 5022
		[SerializeField]
		private float winnerShowTime = 2f;

		// Token: 0x0400139F RID: 5023
		[SerializeField]
		private float winnerDisappearTime = 0.3f;

		// Token: 0x040013A0 RID: 5024
		[SerializeField]
		private float winnerSequenceCompletionDelay = 1f;

		// Token: 0x040013A1 RID: 5025
		[SerializeField]
		private float winnerTitleShowTime;

		// Token: 0x040013A2 RID: 5026
		[SerializeField]
		private float winnerTitleDelay = 1f;

		// Token: 0x040013A3 RID: 5027
		[SerializeField]
		private List<EndPointsCountingElement> enabledLogos;

		// Token: 0x040013A4 RID: 5028
		private bool isShowingScores;

		// Token: 0x040013A5 RID: 5029
		private List<PlayerEndGameStats> stats;

		// Token: 0x040013A6 RID: 5030
		private Action endCallback;

		// Token: 0x040013A7 RID: 5031
		private const int maxStars = 6;
	}
}
