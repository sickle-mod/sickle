using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using HoneyFramework;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.GameLogic.Actions;
using Scythe.UI;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

// Token: 0x020000A0 RID: 160
public class ShowEnemyMoves : MonoBehaviour
{
	// Token: 0x06000524 RID: 1316 RVA: 0x00066B84 File Offset: 0x00064D84
	public void AnimateCamToHex(Vector2 hex)
	{
		this.SetAnimationInProgress(true);
		AnimateCamera.Instance.AnimateCam(hex.x, hex.y, this.zoomBaseHeight, new CameraParams
		{
			stickMinZoom = 9f,
			stickMaxZoom = 50f,
			swivelMinZoom = 33.73f,
			swivelMaxZoom = 90f
		}, this.cameraAnimationTime);
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x00066BEC File Offset: 0x00064DEC
	public void AnimateCamToShowAllHexes(Vector2 camPosition, float camHeight)
	{
		this.SetAnimationInProgress(true);
		AnimateCamera.Instance.AnimateCam(camPosition.x, camPosition.y, camHeight, new CameraParams
		{
			stickMinZoom = 9f,
			stickMaxZoom = 50f,
			swivelMinZoom = 33.73f,
			swivelMaxZoom = 90f
		}, this.cameraAnimationTime);
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x00066C50 File Offset: 0x00064E50
	public void AnimateCamToBase(Faction actualFaction)
	{
		this.SetAnimationInProgress(true);
		switch (actualFaction)
		{
		case Faction.Polania:
			AnimateCamera.Instance.AnimateCam(this.basePositions[0].baseHex.x, this.basePositions[0].baseHex.y, this.zoomBaseHeight, new CameraParams
			{
				stickMinZoom = 9f,
				stickMaxZoom = 50f,
				swivelMinZoom = 33.73f,
				swivelMaxZoom = 90f
			}, this.cameraAnimationTime);
			this.actualFactionBasePosition = new Vector3(this.basePositions[0].resourceImagePosition.x, this.coinMinHeight, this.basePositions[0].resourceImagePosition.y);
			return;
		case Faction.Albion:
			AnimateCamera.Instance.AnimateCam(this.basePositions[1].baseHex.x, this.basePositions[1].baseHex.y, this.zoomBaseHeight, new CameraParams
			{
				stickMinZoom = 9f,
				stickMaxZoom = 50f,
				swivelMinZoom = 33.73f,
				swivelMaxZoom = 90f
			}, this.cameraAnimationTime);
			this.actualFactionBasePosition = new Vector3(this.basePositions[1].resourceImagePosition.x, this.coinMinHeight, this.basePositions[1].resourceImagePosition.y);
			return;
		case Faction.Nordic:
			AnimateCamera.Instance.AnimateCam(this.basePositions[2].baseHex.x, this.basePositions[2].baseHex.y, this.zoomBaseTopFactionsHeight, new CameraParams
			{
				stickMinZoom = 9f,
				stickMaxZoom = 50f,
				swivelMinZoom = 33.73f,
				swivelMaxZoom = 90f
			}, this.cameraAnimationTime);
			this.actualFactionBasePosition = new Vector3(this.basePositions[2].resourceImagePosition.x, this.coinMinHeight, this.basePositions[2].resourceImagePosition.y);
			return;
		case Faction.Rusviet:
			AnimateCamera.Instance.AnimateCam(this.basePositions[3].baseHex.x, this.basePositions[3].baseHex.y, this.zoomBaseHeight, new CameraParams
			{
				stickMinZoom = 9f,
				stickMaxZoom = 50f,
				swivelMinZoom = 33.73f,
				swivelMaxZoom = 90f
			}, this.cameraAnimationTime);
			this.actualFactionBasePosition = new Vector3(this.basePositions[3].resourceImagePosition.x, this.coinMinHeight, this.basePositions[3].resourceImagePosition.y);
			return;
		case Faction.Togawa:
			AnimateCamera.Instance.AnimateCam(this.basePositions[4].baseHex.x, this.basePositions[4].baseHex.y, this.zoomBaseTopFactionsHeight, new CameraParams
			{
				stickMinZoom = 9f,
				stickMaxZoom = 50f,
				swivelMinZoom = 33.73f,
				swivelMaxZoom = 90f
			}, this.cameraAnimationTime);
			this.actualFactionBasePosition = new Vector3(this.basePositions[4].resourceImagePosition.x, this.coinMinHeight, this.basePositions[4].resourceImagePosition.y);
			return;
		case Faction.Crimea:
			AnimateCamera.Instance.AnimateCam(this.basePositions[5].baseHex.x, this.basePositions[5].baseHex.y, this.zoomBaseHeight, new CameraParams
			{
				stickMinZoom = 9f,
				stickMaxZoom = 50f,
				swivelMinZoom = 33.73f,
				swivelMaxZoom = 90f
			}, this.cameraAnimationTime);
			this.actualFactionBasePosition = new Vector3(this.basePositions[5].resourceImagePosition.x, this.coinMinHeight, this.basePositions[5].resourceImagePosition.y);
			return;
		case Faction.Saxony:
			AnimateCamera.Instance.AnimateCam(this.basePositions[6].baseHex.x, this.basePositions[6].baseHex.y, this.zoomBaseHeight, new CameraParams
			{
				stickMinZoom = 9f,
				stickMaxZoom = 50f,
				swivelMinZoom = 33.73f,
				swivelMaxZoom = 90f
			}, this.cameraAnimationTime);
			this.actualFactionBasePosition = new Vector3(this.basePositions[6].resourceImagePosition.x, this.coinMinHeight, this.basePositions[6].resourceImagePosition.y);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x00067160 File Offset: 0x00065360
	public Vector3 GetBaseResourcePosition(Faction actualFaction, float yValue)
	{
		Vector3 zero = Vector3.zero;
		switch (actualFaction)
		{
		case Faction.Polania:
			zero = new Vector3(this.basePositions[0].resourceImagePosition.x, yValue, this.basePositions[0].resourceImagePosition.y);
			break;
		case Faction.Albion:
			zero = new Vector3(this.basePositions[1].resourceImagePosition.x, yValue, this.basePositions[1].resourceImagePosition.y);
			break;
		case Faction.Nordic:
			zero = new Vector3(this.basePositions[2].resourceImagePosition.x, yValue, this.basePositions[2].resourceImagePosition.y);
			break;
		case Faction.Rusviet:
			zero = new Vector3(this.basePositions[3].resourceImagePosition.x, yValue, this.basePositions[3].resourceImagePosition.y);
			break;
		case Faction.Togawa:
			zero = new Vector3(this.basePositions[4].resourceImagePosition.x, yValue, this.basePositions[4].resourceImagePosition.y);
			break;
		case Faction.Crimea:
			zero = new Vector3(this.basePositions[5].resourceImagePosition.x, yValue, this.basePositions[5].resourceImagePosition.y);
			break;
		case Faction.Saxony:
			zero = new Vector3(this.basePositions[6].resourceImagePosition.x, yValue, this.basePositions[6].resourceImagePosition.y);
			break;
		}
		return zero;
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x0002B2F6 File Offset: 0x000294F6
	public void SetupButtonOnStart()
	{
		if (!ShowEnemyMoves.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_FAST_FORWARD, 0)) || GameController.GameManager.IsCampaign)
		{
			this.SetupNormalSpeed();
			return;
		}
		this.SetupFastSpeed();
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x00029B9D File Offset: 0x00027D9D
	private static bool IntToBool(int val)
	{
		return val != 0;
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x00029B95 File Offset: 0x00027D95
	private static int BoolToInt(bool val)
	{
		if (!val)
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x0002B323 File Offset: 0x00029523
	public void OnOptionChanged(bool fastForwardAnimationsActive)
	{
		this.fastForwardAnimationsActive = fastForwardAnimationsActive;
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x0002B32C File Offset: 0x0002952C
	public void OnPauseEnd()
	{
		if (this.fastForwardAnimationsActive)
		{
			this.SetupFastSpeed();
			return;
		}
		this.SetupNormalSpeed();
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x00067320 File Offset: 0x00065520
	public void FastForwardAnimations()
	{
		if (Time.timeScale == 1f && !GameController.GameManager.IsCampaign)
		{
			this.SetupFastSpeed();
			PlayerPrefs.SetInt(OptionsManager.PREFS_KEY_FAST_FORWARD, ShowEnemyMoves.BoolToInt(true));
			return;
		}
		this.SetupNormalSpeed();
		PlayerPrefs.SetInt(OptionsManager.PREFS_KEY_FAST_FORWARD, ShowEnemyMoves.BoolToInt(false));
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x0002B343 File Offset: 0x00029543
	public void ResumeAnimationsSpeed()
	{
		if (ShowEnemyMoves.IntToBool(PlayerPrefs.GetInt(OptionsManager.PREFS_KEY_FAST_FORWARD, 0)))
		{
			this.SetupFastSpeed();
			return;
		}
		this.SetupNormalSpeed();
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x0002B364 File Offset: 0x00029564
	public void SetupNormalSpeed()
	{
		if (this.fastForwardButton != null)
		{
			this.fastForwardButton.GetComponent<Image>().sprite = this.speedUpImage;
		}
		Time.timeScale = 1f;
		this.fastForwardAnimationsActive = false;
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x0002B39B File Offset: 0x0002959B
	public void SetupFastSpeed()
	{
		if (this.fastForwardButton != null)
		{
			this.fastForwardButton.GetComponent<Image>().sprite = this.speedDownImage;
		}
		Time.timeScale = this.fastForwardSpeed;
		this.fastForwardAnimationsActive = true;
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x00067374 File Offset: 0x00065574
	private void MechSkillAnimation(Vector3 origin, List<Unit> anotherUnits, int skillIndex, Faction actionOwner)
	{
		this.SetAnimationInProgress(true);
		Unit unit2 = anotherUnits.Last<Unit>();
		using (List<Unit>.Enumerator enumerator = anotherUnits.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Unit unit = enumerator.Current;
				if (unit.UnitType != UnitType.Worker)
				{
					GameObject mechSkillMarker = ResourcesObjectPoolScript.Instance.GetPooledObject(9);
					mechSkillMarker.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = GameController.factionInfo[GameController.GameManager.GetPlayerByFaction(actionOwner).matFaction.faction].mechAbilityIcons[skillIndex];
					mechSkillMarker.transform.position = origin;
					mechSkillMarker.transform.position = new Vector3(mechSkillMarker.transform.position.x, mechSkillMarker.transform.position.y + 2f, mechSkillMarker.transform.position.z);
					mechSkillMarker.transform.GetChild(0).transform.localScale = new Vector3(0f, 0f, 0f);
					mechSkillMarker.gameObject.SetActive(true);
					if (unit.Equals(unit2))
					{
						mechSkillMarker.transform.GetChild(0).transform.DOScale(0.25f, 1f).SetEase(Ease.InOutExpo).OnComplete(delegate
						{
							this.FirstMechSkillAnimation(mechSkillMarker, unit, true);
						});
					}
					else
					{
						mechSkillMarker.transform.GetChild(0).transform.DOScale(0.25f, 1f).SetEase(Ease.InOutExpo).OnComplete(delegate
						{
							this.FirstMechSkillAnimation(mechSkillMarker, unit, false);
						});
					}
				}
			}
		}
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x00067590 File Offset: 0x00065790
	private void FirstMechSkillAnimation(GameObject mechSkillMarker, Unit unit, bool lastAnimation = false)
	{
		mechSkillMarker.transform.DOMoveX(GameController.GetUnitPresenter(unit).transform.position.x, 1f, false).SetEase(Ease.InOutExpo);
		mechSkillMarker.transform.DOMoveZ(GameController.GetUnitPresenter(unit).transform.position.z, 1f, false).SetEase(Ease.InOutExpo).OnComplete(delegate
		{
			this.SecondMechSkillAnimation(mechSkillMarker, lastAnimation);
		});
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x00067630 File Offset: 0x00065830
	private void SecondMechSkillAnimation(GameObject mechSkillMarker, bool lastAnimation = false)
	{
		mechSkillMarker.transform.GetChild(0).transform.DOScale(0f, 0.5f).SetEase(Ease.InOutExpo);
		mechSkillMarker.transform.DOMoveY(0f, 0.5f, false).SetEase(Ease.InOutExpo).OnComplete(delegate
		{
			this.HideSkillMarker(mechSkillMarker, lastAnimation);
		});
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x0002B3D3 File Offset: 0x000295D3
	private void HideSkillMarker(GameObject mechSkillMarker, bool lastAnimation = false)
	{
		mechSkillMarker.SetActive(false);
		if (lastAnimation)
		{
			this.SetAnimationInProgress(false);
			this.GetNextAnimation();
		}
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x000676BC File Offset: 0x000658BC
	private void MatAnimation(Player player, DownActionType actionDown, TopActionType actionTop, GainType gainType)
	{
		int num = 0;
		int num2 = 0;
		this.SetAnimationInProgress(true);
		List<MatPlayerSectionPresenter> list = new List<MatPlayerSectionPresenter>();
		this.enemyMat.SetActive(true);
		for (int i = 0; i < 4; i++)
		{
			MatPlayerSectionPresenter component = this.enemyPlayerMats[i].GetComponent<MatPlayerSectionPresenter>();
			list.Add(component);
		}
		Func<MatPlayerSectionPresenter, bool> <>9__1;
		Func<MatPlayerSectionPresenter, bool> <>9__2;
		for (int j = 0; j < 4; j++)
		{
			MatPlayerSection s = player.matPlayer.GetPlayerMatSection(j);
			int sectionID = list.SingleOrDefault((MatPlayerSectionPresenter section) => section.topActionPresenter.actionType == s.ActionTop.Type).sectionID;
			if (sectionID != j && sectionID < list.Count)
			{
				list[j].SwapTopAction(list[sectionID]);
			}
			if (PlatformManager.IsStandalone)
			{
				list[j].UpdateSection(s.ActionTop, s.ActionDown, player.matPlayer.workers.Count - 2, false);
			}
			IEnumerable<MatPlayerSectionPresenter> enumerable = list;
			Func<MatPlayerSectionPresenter, bool> func;
			if ((func = <>9__1) == null)
			{
				func = (<>9__1 = (MatPlayerSectionPresenter section) => section.topActionPresenter.actionType == actionTop);
			}
			num = enumerable.SingleOrDefault(func).sectionID;
			IEnumerable<MatPlayerSectionPresenter> enumerable2 = list;
			Func<MatPlayerSectionPresenter, bool> func2;
			if ((func2 = <>9__2) == null)
			{
				func2 = (<>9__2 = (MatPlayerSectionPresenter section) => section.downActionPresenter.actionType == actionDown);
			}
			num2 = enumerable2.SingleOrDefault(func2).sectionID;
		}
		if (PlatformManager.IsMobile)
		{
			int num3 = player.matPlayer.workers.Count - 2;
			if (num == num2)
			{
				MatPlayerSection playerMatSection = player.matPlayer.GetPlayerMatSection(num2);
				list[num2].UpdateSectionForEnemyAnimation(playerMatSection.ActionTop, playerMatSection.ActionDown, num3, true, true, gainType);
			}
			else
			{
				MatPlayerSection playerMatSection2 = player.matPlayer.GetPlayerMatSection(num);
				list[num].UpdateSectionForEnemyAnimation(playerMatSection2.ActionTop, playerMatSection2.ActionDown, num3, true, false, gainType);
				MatPlayerSection playerMatSection3 = player.matPlayer.GetPlayerMatSection(num2);
				list[num2].UpdateSectionForEnemyAnimation(playerMatSection3.ActionTop, playerMatSection3.ActionDown, num3, false, true, gainType);
			}
		}
		bool isMobile = PlatformManager.IsMobile;
		int num4 = (PlatformManager.IsMobile ? 0 : 1);
		switch (gainType)
		{
		case GainType.Coin:
			this.topAnimationPosition = this.enemyPlayerMats[num].GetComponentInChildren<PlayerTopActionPresenter>().actionGain2[1].gameObject.transform;
			this.enemyPlayerMats[num].GetComponentInChildren<PlayerTopActionPresenter>().actionGain2[1].gameObject.transform.GetChild(num4).gameObject.SetActive(isMobile);
			break;
		case GainType.Popularity:
			this.topAnimationPosition = this.enemyPlayerMats[num].GetComponentInChildren<PlayerTopActionPresenter>().actionGain2[1].gameObject.transform;
			this.enemyPlayerMats[num].GetComponentInChildren<PlayerTopActionPresenter>().actionGain2[1].gameObject.transform.GetChild(num4).gameObject.SetActive(isMobile);
			break;
		case GainType.Power:
			this.topAnimationPosition = this.enemyPlayerMats[num].GetComponentInChildren<PlayerTopActionPresenter>().actionGain1[2].gameObject.transform;
			this.enemyPlayerMats[num].GetComponentInChildren<PlayerTopActionPresenter>().actionGain1[2].gameObject.transform.GetChild(num4).gameObject.SetActive(isMobile);
			break;
		case GainType.CombatCard:
			this.topAnimationPosition = this.enemyPlayerMats[num].GetComponentInChildren<PlayerTopActionPresenter>().actionGain2[1].gameObject.transform;
			this.enemyPlayerMats[num].GetComponentInChildren<PlayerTopActionPresenter>().actionGain2[1].gameObject.transform.GetChild(num4).gameObject.SetActive(isMobile);
			break;
		case GainType.Produce:
			if (PlatformManager.IsMobile)
			{
				this.topAnimationPosition = this.enemyPlayerMats[num].GetComponentInChildren<PlayerTopActionPresenter>().actionGain1[1].gameObject.transform;
				this.enemyPlayerMats[num].GetComponentInChildren<PlayerTopActionPresenter>().actionGain1[1].gameObject.transform.GetChild(num4).gameObject.SetActive(isMobile);
			}
			else
			{
				this.topAnimationPosition = this.enemyPlayerMats[num].GetComponentInChildren<PlayerTopActionPresenter>().actionGain2[1].gameObject.transform;
				this.enemyPlayerMats[num].GetComponentInChildren<PlayerTopActionPresenter>().actionGain2[1].gameObject.transform.GetChild(num4).gameObject.SetActive(isMobile);
			}
			break;
		case GainType.Move:
			this.topAnimationPosition = this.enemyPlayerMats[num].GetComponentInChildren<PlayerTopActionPresenter>().actionGain1[2].gameObject.transform;
			this.enemyPlayerMats[num].GetComponentInChildren<PlayerTopActionPresenter>().actionGain1[2].gameObject.transform.GetChild(num4).gameObject.SetActive(isMobile);
			break;
		}
		switch (actionDown)
		{
		case DownActionType.Upgrade:
			this.downAnimationPosition = this.enemyPlayerMats[num2].GetComponentInChildren<PlayerDownActionPresenter>().GetUprgradePlaceForAnimation();
			break;
		case DownActionType.Deploy:
			this.downAnimationPosition = this.enemyPlayerMats[num2].GetComponentInChildren<PlayerDownActionPresenter>().GetUprgradePlaceForAnimation();
			break;
		case DownActionType.Build:
			this.downAnimationPosition = this.enemyPlayerMats[num2].GetComponentInChildren<PlayerDownActionPresenter>().GetUprgradePlaceForAnimation();
			break;
		case DownActionType.Enlist:
			this.downAnimationPosition = this.enemyPlayerMats[num2].GetComponentInChildren<PlayerDownActionPresenter>().GetUprgradePlaceForAnimation();
			break;
		}
		base.StartCoroutine(this.ShowMatAnimation(player, num, num2, gainType));
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x00067C28 File Offset: 0x00065E28
	private void MatAnimation(Player player, DownActionType actionDown)
	{
		this.SetAnimationInProgress(true);
		List<MatPlayerSectionPresenter> list = new List<MatPlayerSectionPresenter>();
		this.enemyMat.SetActive(true);
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			MatPlayerSectionPresenter component = this.enemyPlayerMats[i].GetComponent<MatPlayerSectionPresenter>();
			list.Add(component);
		}
		Func<MatPlayerSectionPresenter, bool> <>9__1;
		for (int j = 0; j < 4; j++)
		{
			MatPlayerSection s = player.matPlayer.GetPlayerMatSection(j);
			int sectionID = list.SingleOrDefault((MatPlayerSectionPresenter section) => section.topActionPresenter.actionType == s.ActionTop.Type).sectionID;
			if (sectionID != j && sectionID < list.Count)
			{
				list[j].SwapTopAction(list[sectionID]);
			}
			if (PlatformManager.IsMobile)
			{
				list[j].gameObject.SetActive(true);
				list[j].UpdateSection(s.ActionTop, s.ActionDown, player.matPlayer.workers.Count - 2, false);
				list[j].gameObject.SetActive(false);
			}
			else
			{
				list[j].UpdateSection(s.ActionTop, s.ActionDown, player.matPlayer.workers.Count - 2, false);
			}
			IEnumerable<MatPlayerSectionPresenter> enumerable = list;
			Func<MatPlayerSectionPresenter, bool> func;
			if ((func = <>9__1) == null)
			{
				func = (<>9__1 = (MatPlayerSectionPresenter section) => section.downActionPresenter.actionType == actionDown);
			}
			num = enumerable.SingleOrDefault(func).sectionID;
		}
		switch (actionDown)
		{
		case DownActionType.Upgrade:
			this.enlistAnimationPosition = this.enemyPlayerMats[num].GetComponentInChildren<PlayerDownActionPresenter>().enlistObject.transform;
			break;
		case DownActionType.Deploy:
			this.enlistAnimationPosition = this.enemyPlayerMats[num].GetComponentInChildren<PlayerDownActionPresenter>().enlistObject.transform;
			break;
		case DownActionType.Build:
			this.enlistAnimationPosition = this.enemyPlayerMats[num].GetComponentInChildren<PlayerDownActionPresenter>().enlistObject.transform;
			break;
		case DownActionType.Enlist:
			this.enlistAnimationPosition = this.enemyPlayerMats[num].GetComponentInChildren<PlayerDownActionPresenter>().enlistObject.transform;
			break;
		}
		base.StartCoroutine(this.ShowMatAnimation(num));
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x0002B3EC File Offset: 0x000295EC
	private IEnumerator ShowMatAnimation(int downID)
	{
		this.enemyPlayerMats[downID].SetActive(true);
		this.enlistFlashAnimation.transform.SetParent(this.enlistAnimationPosition);
		RectTransform component = this.enlistFlashAnimation.GetComponent<RectTransform>();
		if (PlatformManager.IsStandalone)
		{
			component.anchoredPosition = Vector2.zero;
		}
		else
		{
			component.offsetMin = Vector2.zero;
			component.offsetMax = Vector2.zero;
		}
		this.enlistFlashAnimation.SetActive(true);
		yield return new WaitForSeconds(2.1f);
		this.enlistFlashAnimation.SetActive(false);
		this.enlistFlashAnimation.transform.SetParent(this.enemyMat.transform);
		this.enemyPlayerMats[downID].SetActive(false);
		this.SetAnimationInProgress(false);
		this.GetNextAnimation();
		yield break;
	}

	// Token: 0x06000538 RID: 1336 RVA: 0x0002B402 File Offset: 0x00029602
	private IEnumerator ShowMatAnimation(Player player, int matSectionIdContainingTopAction, int matSectionIdContainingDownAction, GainType topActionGainType)
	{
		int numberOfWorkers = player.matPlayer.workers.Count - 2;
		float upgradeAnimationDuration = this.upgradeGreenFlashAnimation.GetComponent<MatFlashAnimation>().GetAnimationDuration();
		this.enemyPlayerMats[matSectionIdContainingTopAction].SetActive(true);
		this.enemyPlayerMats[matSectionIdContainingDownAction].SetActive(true);
		this.upgradeGreenFlashAnimation.transform.SetParent(this.topAnimationPosition);
		RectTransform rectTransform = this.upgradeGreenFlashAnimation.GetComponent<RectTransform>();
		if (PlatformManager.IsStandalone)
		{
			rectTransform.anchoredPosition = Vector2.zero;
		}
		else
		{
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
		}
		this.upgradeGreenFlashAnimation.SetActive(true);
		yield return new WaitForSeconds(upgradeAnimationDuration);
		if (PlatformManager.IsMobile)
		{
			if (matSectionIdContainingTopAction == matSectionIdContainingDownAction)
			{
				MatPlayerSection playerMatSection = player.matPlayer.GetPlayerMatSection(matSectionIdContainingTopAction);
				this.enemyPlayerMats[matSectionIdContainingTopAction].GetComponent<MatPlayerSectionPresenter>().UpdateSectionForEnemyAnimation(playerMatSection.ActionTop, playerMatSection.ActionDown, numberOfWorkers, false, true, topActionGainType);
			}
			else
			{
				MatPlayerSection playerMatSection2 = player.matPlayer.GetPlayerMatSection(matSectionIdContainingTopAction);
				this.enemyPlayerMats[matSectionIdContainingTopAction].GetComponent<MatPlayerSectionPresenter>().UpdateSectionForEnemyAnimation(playerMatSection2.ActionTop, playerMatSection2.ActionDown, numberOfWorkers, false, false, topActionGainType);
			}
		}
		this.upgradeRedFlashAnimation.transform.SetParent(this.downAnimationPosition);
		rectTransform = this.upgradeRedFlashAnimation.GetComponent<RectTransform>();
		if (PlatformManager.IsStandalone)
		{
			rectTransform.anchoredPosition = Vector2.zero;
		}
		else
		{
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
		}
		this.upgradeRedFlashAnimation.SetActive(true);
		yield return new WaitForSeconds(upgradeAnimationDuration);
		if (PlatformManager.IsMobile)
		{
			MatPlayerSection playerMatSection3 = player.matPlayer.GetPlayerMatSection(matSectionIdContainingDownAction);
			this.enemyPlayerMats[matSectionIdContainingDownAction].GetComponent<MatPlayerSectionPresenter>().UpdateSectionForEnemyAnimation(playerMatSection3.ActionTop, playerMatSection3.ActionDown, numberOfWorkers, false, false, topActionGainType);
			yield return new WaitForSeconds(1.25f);
		}
		this.upgradeGreenFlashAnimation.SetActive(false);
		this.upgradeGreenFlashAnimation.transform.SetParent(this.enemyMat.transform);
		this.upgradeRedFlashAnimation.SetActive(false);
		this.upgradeRedFlashAnimation.transform.SetParent(this.enemyMat.transform);
		this.enemyPlayerMats[matSectionIdContainingTopAction].SetActive(false);
		this.enemyPlayerMats[matSectionIdContainingDownAction].SetActive(false);
		this.SetAnimationInProgress(false);
		this.GetNextAnimation();
		yield break;
	}

	// Token: 0x06000539 RID: 1337 RVA: 0x0002B42E File Offset: 0x0002962E
	private bool IsNextAnimationTypeOf(Type type)
	{
		return this.enemyActionAnimations.Count != 0 && this.enemyActionAnimations.Peek().GetType() == type;
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x00067E68 File Offset: 0x00066068
	private void TakePositionUnitAnimation(Unit unitToAnimation, GameHex positionToTake)
	{
		if (this.IsNextAnimationTypeOf(typeof(MoveUnitAnimation)))
		{
			this.SetAnimationInProgress(!(this.enemyActionAnimations.Peek() as MoveUnitAnimation).takingOwnerPosition);
		}
		else
		{
			this.SetAnimationInProgress(true);
		}
		this.unitsToChangePositions++;
		GameController.GetUnitPresenter(unitToAnimation).RunMoveToOwnerPosition(positionToTake, true);
		this.GetNextAnimation();
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x0002B455 File Offset: 0x00029655
	private void MoveUnitAnimation(Unit unitToAnimation, Dictionary<GameHex, GameHex> possibleMoves, GameHex start, GameHex end)
	{
		this.SetAnimationInProgress(true);
		GameController.GetUnitPresenter(unitToAnimation).RunTheMoveAnimation(possibleMoves, true, false, false, start, end);
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x00067ED0 File Offset: 0x000660D0
	private void LoadResourcesAnimation(Dictionary<ResourceType, int> resources, Unit unit, GameHex hex)
	{
		this.SetAnimationInProgress(true);
		UnitPresenter unitPresenter = GameController.GetUnitPresenter(unit);
		int num = 0;
		foreach (ResourceType resourceType in resources.Keys)
		{
			if (resources[resourceType] > 0)
			{
				num++;
			}
			if (resources[resourceType] < 0)
			{
				num++;
			}
		}
		foreach (ResourceType resourceType2 in resources.Keys)
		{
			if (resources[resourceType2] > 0)
			{
				num--;
				if (num == 0)
				{
					unitPresenter.hex.ExchangeShowResourceMove(resourceType2, unitPresenter.gameObject.transform.position, false, resources[resourceType2], true);
				}
				else
				{
					unitPresenter.hex.ExchangeShowResourceMove(resourceType2, unitPresenter.gameObject.transform.position, false, resources[resourceType2], false);
				}
			}
			if (resources[resourceType2] < 0)
			{
				num--;
				if (num == 0)
				{
					GameController.Instance.GetGameHexPresenter(hex).ExchangeShowResourceMove(resourceType2, unitPresenter.gameObject.transform.position, true, resources[resourceType2], true);
				}
				else
				{
					GameController.Instance.GetGameHexPresenter(hex).ExchangeShowResourceMove(resourceType2, unitPresenter.gameObject.transform.position, true, resources[resourceType2], false);
				}
			}
		}
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x00068058 File Offset: 0x00066258
	private void LoadWorkerAnimation(List<Unit> workers, Unit mech)
	{
		this.SetAnimationInProgress(true);
		for (int i = 0; i < workers.Count; i++)
		{
			this.workersToLoad++;
			GameController.GetUnitPresenter(mech).LoadWorker(workers[i], true);
		}
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x000680A0 File Offset: 0x000662A0
	private void UnloadWorkerAnimation(UnloadWorkerAnimation unloadWorker)
	{
		this.SetAnimationInProgress(!this.IsNextAnimationTypeOf(typeof(UnloadWorkerAnimation)));
		this.GameController.GetGameHexPresenter(unloadWorker.positionToUnload);
		this.workersToUnload++;
		UnitPresenter unitPresenter = GameController.GetUnitPresenter(unloadWorker.mech);
		if (unitPresenter != null)
		{
			unitPresenter.UnloadWorkerByUnitId(unloadWorker.worker.Id, true);
		}
	}

	// Token: 0x0600053F RID: 1343 RVA: 0x00068110 File Offset: 0x00066310
	private void MoveToBattlefieldAnimation(Unit unit, GameHex moveFromHex, GameHex start, GameHex end)
	{
		this.SetAnimationInProgress(true);
		Dictionary<GameHex, GameHex> dictionary = new Dictionary<GameHex, GameHex>();
		dictionary.Add(start, null);
		dictionary.Add(end, start);
		GameController.GetUnitPresenter(unit).RunTheMoveAnimation(dictionary, true, false, true, start, end);
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x00068150 File Offset: 0x00066350
	private void MoveRetreatAnimation(List<Unit> units, GameHex positionBase)
	{
		this.SetAnimationInProgress(true);
		for (int i = 0; i < units.Count; i++)
		{
			Dictionary<GameHex, GameHex> dictionary = new Dictionary<GameHex, GameHex>();
			dictionary.Add(GameController.GetUnitPresenter(units[i]).hex.GetGameHexLogic(), null);
			if (!dictionary.ContainsKey(positionBase))
			{
				dictionary.Add(positionBase, GameController.GetUnitPresenter(units[i]).hex.GetGameHexLogic());
			}
			if (i != units.Count - 1)
			{
				GameController.GetUnitPresenter(units[i]).RunTheMoveAnimation(dictionary, true, true, false, null, null);
			}
			else
			{
				GameController.GetUnitPresenter(units[i]).RunTheMoveAnimation(dictionary, true, false, false, null, null);
			}
		}
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x00068200 File Offset: 0x00066400
	private void PayTopStats(GainType gainType, Vector3 startPosition)
	{
		this.SetAnimationInProgress(true);
		GameObject resourceToAnimation = this.GetStatMarkerModel(gainType);
		resourceToAnimation.transform.position = startPosition;
		resourceToAnimation.transform.localScale = new Vector3(0f, 0f, 0f);
		resourceToAnimation.SetActive(true);
		this.PayStatsParticle(startPosition);
		resourceToAnimation.transform.DOMoveY(this.payEndPosition, this.payAnimationTime, false).SetEase(Ease.InOutExpo).OnComplete(delegate
		{
			this.PayStatsDissolveAnimation(resourceToAnimation);
		});
		resourceToAnimation.transform.DOScale(0.5f, this.payAnimationTime).SetEase(Ease.Linear);
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x000682D0 File Offset: 0x000664D0
	private void PayStatsParticle(Vector3 position)
	{
		GameObject circle = ResourcesObjectPoolScript.Instance.GetPooledObject(11);
		circle.transform.position = new Vector3(position.x, 0.15f, position.z);
		circle.SetActive(true);
		circle.GetComponent<ParticleSystem>().Play();
		DOVirtual.DelayedCall(0.3f / Time.timeScale, delegate
		{
			this.HideParticle(circle);
		}, true);
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x0006835C File Offset: 0x0006655C
	private void PayStatsDissolveAnimation(GameObject resourceToAnimation)
	{
		DOTween.To(delegate(float x)
		{
			resourceToAnimation.transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_Cutoff", x);
		}, 0f, 1f, 0.75f).OnComplete(delegate
		{
			this.PayAnimationEnd(resourceToAnimation);
		});
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x0002B470 File Offset: 0x00029670
	private void PayAnimationEnd(GameObject resourceToHide)
	{
		resourceToHide.transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_Cutoff", 0f);
		resourceToHide.SetActive(false);
		this.SetAnimationInProgress(false);
		this.GetNextAnimation();
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x000683B0 File Offset: 0x000665B0
	private void SpawnBuilding(BuildSpawnAnimation spawnAnim)
	{
		this.SetAnimationInProgress(true);
		spawnAnim.hex.building.GetComponent<BuildingPresenter>().gameObject.SetActive(true);
		spawnAnim.hex.building.GetComponent<BuildingPresenter>().SetActive(true);
		spawnAnim.hex.building.GetComponent<BuildingPresenter>().SpawnAnimation(spawnAnim.buildingToSpawn.buildingType, spawnAnim.owner, true);
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x0006841C File Offset: 0x0006661C
	private void GainTopStats(GainType gainType, Vector3 startPosition)
	{
		this.SetAnimationInProgress(true);
		GameObject resourceToAnimation = this.GetStatMarkerModel(gainType);
		resourceToAnimation.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		resourceToAnimation.transform.position = startPosition;
		resourceToAnimation.SetActive(true);
		resourceToAnimation.transform.DOMoveY(this.coinMinHeight, this.gainAnimationTime, false).SetEase(Ease.Linear).OnComplete(delegate
		{
			this.GainAnimationEnd(resourceToAnimation);
		});
		DOVirtual.DelayedCall(this.normalGainTopStatsDelay / Time.timeScale, delegate
		{
			this.BaseParticleActivation(startPosition);
		}, true);
		resourceToAnimation.transform.DOScale(0f, this.payAnimationTime).SetEase(Ease.Linear);
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x00068510 File Offset: 0x00066710
	private void GainTopStats(GainType gainType, Vector3 startPosition, Vector3 particlePosition)
	{
		if (PlatformManager.IsMobile && gainType == GainType.CombatCard && GameController.GameManager.CombatCardsLeft() <= 0)
		{
			this.enemyActionAnimations.Clear();
			this.SetLastAnimation(null);
			this.SetAnimationInProgress(false);
			this.GameController.turnInfoPanel.DisableEnemyActionInfo();
			if (!GameController.GameManager.IsCampaign)
			{
				this.GameController.turnInfoPanel.ActivateTurnInfoPanel();
			}
			this.cameraMovementEffects.presenationFinished = true;
			CameraControler.CameraMovementBlocked = false;
			this.ReattachDelegates();
			if (!GameController.GameManager.IsMultiplayer && !GameController.GameManager.PlayerCurrent.IsHuman && (!GameController.GameManager.combatManager.IsPlayerInCombat() || !GameController.GameManager.combatManager.GetDefender().IsHuman))
			{
				this.GameController.GetNextPlayerAfterEnemyAnimation();
			}
			return;
		}
		this.bonusMarkerPositionRotationAdjustment();
		this.SetAnimationInProgress(true);
		Vector3 vector = new Vector3(startPosition.x + this.rotationAdjustment.x, startPosition.y - this.recruitModelYOffset, startPosition.z + this.rotationAdjustment.z);
		GameObject recruitMarker = this.GetStatMarkerModel(GainType.Recruit);
		GameObject resourceToAnimation = this.GetStatMarkerModel(gainType);
		resourceToAnimation.transform.localScale = new Vector3(0f, 0f, 0f);
		resourceToAnimation.transform.position = vector;
		recruitMarker.transform.position = vector;
		recruitMarker.SetActive(true);
		resourceToAnimation.SetActive(true);
		resourceToAnimation.transform.DOScale(0.5f, 0.5f).SetEase(Ease.OutQuint).OnComplete(delegate
		{
			this.BonusAnimationSecondAnimation(resourceToAnimation, recruitMarker, particlePosition);
		});
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x000686F0 File Offset: 0x000668F0
	public void bonusMarkerPositionRotationAdjustment()
	{
		string text = CameraControler.Instance.fixedRotationPos.ToString();
		uint num = global::<PrivateImplementationDetails>.ComputeStringHash(text);
		if (num <= 421209969U)
		{
			if (num <= 353113755U)
			{
				if (num != 201234636U)
				{
					if (num != 353113755U)
					{
						goto IL_02CA;
					}
					if (!(text == "-180"))
					{
						goto IL_02CA;
					}
					this.rotationAdjustment = new Vector3(-3.93f, 0f, 0f);
					this.bonusAnimationTargetAdjustment = new Vector3(2.4f, 0f, 0f);
					return;
				}
				else if (!(text == "90"))
				{
					goto IL_02CA;
				}
			}
			else if (num != 367583803U)
			{
				if (num != 421209969U)
				{
					goto IL_02CA;
				}
				if (!(text == "-120"))
				{
					goto IL_02CA;
				}
				this.rotationAdjustment = new Vector3(-2.9f, 0f, 2.2f);
				this.bonusAnimationTargetAdjustment = new Vector3(1.34f, 0f, -2.04f);
				return;
			}
			else if (!(text == "60"))
			{
				goto IL_02CA;
			}
			this.rotationAdjustment = new Vector3(-0.2f, 0f, -1.8f);
			this.bonusAnimationTargetAdjustment = new Vector3(-1.34f, 0f, 2.04f);
			return;
		}
		if (num > 1933075630U)
		{
			if (num != 3383969689U)
			{
				if (num != 3620974926U)
				{
					if (num != 4146044052U)
					{
						goto IL_02CA;
					}
					if (!(text == "180"))
					{
						goto IL_02CA;
					}
					this.rotationAdjustment = new Vector3(-3.93f, 0f, 0f);
					this.bonusAnimationTargetAdjustment = new Vector3(2.4f, 0f, 0f);
					return;
				}
				else if (!(text == "-60"))
				{
					goto IL_02CA;
				}
			}
			else if (!(text == "-90"))
			{
				goto IL_02CA;
			}
			this.rotationAdjustment = new Vector3(-0.2f, 0f, 2.2f);
			this.bonusAnimationTargetAdjustment = new Vector3(-1.34f, 1f, -2.04f);
			return;
		}
		if (num != 890022063U)
		{
			if (num == 1933075630U)
			{
				if (text == "120")
				{
					this.rotationAdjustment = new Vector3(-2.9f, 0f, -1.8f);
					this.bonusAnimationTargetAdjustment = new Vector3(1.34f, 0f, 2.04f);
					return;
				}
			}
		}
		else if (text == "0")
		{
			this.rotationAdjustment = new Vector3(0.85f, 0f, 0f);
			this.bonusAnimationTargetAdjustment = new Vector3(-2.4f, 0f, 0f);
			return;
		}
		IL_02CA:
		this.rotationAdjustment = Vector3.zero;
		this.bonusAnimationTargetAdjustment = Vector3.zero;
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x000689E0 File Offset: 0x00066BE0
	private void BaseParticleActivation(Vector3 position)
	{
		GameObject circle = ResourcesObjectPoolScript.Instance.GetPooledObject(10);
		circle.transform.position = new Vector3(position.x, 0.15f, position.z);
		circle.SetActive(true);
		circle.GetComponent<ParticleSystem>().Play();
		DOVirtual.DelayedCall(0.3f, delegate
		{
			this.HideParticle(circle);
		}, true);
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x0002B4AB File Offset: 0x000296AB
	private void HideParticle(GameObject circle)
	{
		circle.SetActive(false);
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x00068A68 File Offset: 0x00066C68
	private void BonusAnimationSecondAnimation(GameObject resourceToAnimation, GameObject recruitMarker, Vector3 startPosition)
	{
		Vector3 vector = new Vector3(resourceToAnimation.transform.position.x + this.bonusAnimationTargetAdjustment.x, resourceToAnimation.transform.position.y + this.bonusAnimationTargetAdjustment.y, resourceToAnimation.transform.position.z + this.bonusAnimationTargetAdjustment.z);
		resourceToAnimation.transform.DOMoveX(vector.x, this.gainAnimationTime, false).SetEase(Ease.Linear);
		resourceToAnimation.transform.DOMoveZ(vector.z, this.gainAnimationTime, false).SetEase(Ease.Linear);
		resourceToAnimation.transform.DOMoveY(vector.y, this.gainAnimationTime, false).SetEase(Ease.Linear).OnComplete(delegate
		{
			this.BonusAnimationThirdAnimation(resourceToAnimation, recruitMarker);
		});
		DOVirtual.DelayedCall(this.bonusGainTopStatsDelay, delegate
		{
			this.BaseParticleActivation(startPosition);
		}, true);
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x00068B98 File Offset: 0x00066D98
	private void BonusAnimationThirdAnimation(GameObject resourceToAnimation, GameObject recruitMarker)
	{
		resourceToAnimation.transform.DOMoveY(this.coinMinHeight, this.gainAnimationTime, false).SetEase(Ease.Linear).OnComplete(delegate
		{
			this.GainAnimationEnd(resourceToAnimation, recruitMarker);
		});
		resourceToAnimation.transform.DOScale(0f, this.payAnimationTime).SetEase(Ease.Linear);
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x0002B4B4 File Offset: 0x000296B4
	private void GainAnimationEnd(GameObject resourceToHide)
	{
		resourceToHide.transform.DOScale(0.5f, 0.0001f);
		resourceToHide.SetActive(false);
		this.SetAnimationInProgress(false);
		this.GetNextAnimation();
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x0002B4E0 File Offset: 0x000296E0
	private void GainAnimationEnd(GameObject resourceToHide, GameObject recruitMarker)
	{
		recruitMarker.SetActive(false);
		resourceToHide.transform.DOScale(0.5f, 0.0001f);
		resourceToHide.SetActive(false);
		this.SetAnimationInProgress(false);
		this.GetNextAnimation();
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x0002B513 File Offset: 0x00029713
	private void EnemyProduceAnimation(ProduceResourceAnimation enemyAction)
	{
		this.SetAnimationInProgress(true);
		this.GameController.GetGameHexPresenter(enemyAction.hex).ProduceAnimation(enemyAction.amountOfResources + 1, enemyAction.resourceType, true);
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x00068C18 File Offset: 0x00066E18
	private void EnemyProduceWorkerAnimation(ProduceWorkerAnimation enemyAction)
	{
		this.GameController.gameBoardPresenter.UpdateUnits(true);
		this.SetAnimationInProgress(true);
		for (int i = 0; i < enemyAction.workersToAnimation.Count; i++)
		{
			if (i != enemyAction.workersToAnimation.Count - 1)
			{
				GameController.GetUnitPresenter(enemyAction.workersToAnimation[i]).gameObject.SetActive(true);
				GameController.GetUnitPresenter(enemyAction.workersToAnimation[i]).UnitSpawnAnimation(true, false);
			}
			else
			{
				GameController.GetUnitPresenter(enemyAction.workersToAnimation[i]).gameObject.SetActive(true);
				GameController.GetUnitPresenter(enemyAction.workersToAnimation[i]).UnitSpawnAnimation(true, true);
			}
		}
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x00068CD0 File Offset: 0x00066ED0
	private void EnemySpawnMechAnimation(SpawnMechAnimation enemyAction)
	{
		this.GameController.gameBoardPresenter.UpdateUnits(true);
		this.SetAnimationInProgress(true);
		GameController.GetUnitPresenter(enemyAction.mechToSpawn).gameObject.SetActive(true);
		GameController.GetUnitPresenter(enemyAction.mechToSpawn).UnitSpawnAnimation(true, true);
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x00068D20 File Offset: 0x00066F20
	private void EnemyPayResourceAnimation(PayResourceAnimation enemyAction)
	{
		this.SetAnimationInProgress(true);
		if (PlatformManager.IsMobile)
		{
			this.GameController.GetGameHexPresenter(enemyAction.hex).PayResourcesAnimation(enemyAction.amountOfResources, enemyAction.resourceType, true);
			return;
		}
		this.GameController.GetGameHexPresenter(enemyAction.hex).PayResourcesAnimation(enemyAction.amountOfResources + 1, enemyAction.resourceType, true);
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x0002B541 File Offset: 0x00029741
	private void EnemyEndTurnAnimation(EndTurnAnimation endTurnAnimation)
	{
		this.SetAnimationInProgress(false);
		this.GetNextAnimation();
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x06000554 RID: 1364 RVA: 0x0002B550 File Offset: 0x00029750
	public float FastForwardSpeed
	{
		get
		{
			return this.fastForwardSpeed;
		}
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x0002B558 File Offset: 0x00029758
	private void Awake()
	{
		ShowEnemyMoves.Instance = this;
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x00068D84 File Offset: 0x00066F84
	private void Start()
	{
		this.cameraMovementEffects = CameraMovementEffects.Instance;
		this.GameController = GameController.Instance;
		this.postProcessingProfile = CameraControler.Instance.gameObject.GetComponent<PostProcessingBehaviour>().profile;
		this.animationFailsafeTimestamp = Time.time;
		this.animationFailsafeQueueSize = this.enemyActionAnimations.Count;
		if (PlatformManager.IsMobile)
		{
			this.UpdateFastForwardButton();
		}
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x00068DEC File Offset: 0x00066FEC
	private void Update()
	{
		if (this.animationInProgress)
		{
			if (this.animationFailsafeQueueSize != this.enemyActionAnimations.Count)
			{
				this.animationFailsafeQueueSize = this.enemyActionAnimations.Count;
				this.animationFailsafeTimestamp = Time.time;
				return;
			}
			if (Time.time - this.animationFailsafeTimestamp > 8f)
			{
				this.animationFailsafeQueueSize = this.enemyActionAnimations.Count;
				this.animationFailsafeTimestamp = Time.time;
				Debug.LogWarning("Animation freeze failsafe activated ");
				if (this.animationInProgress)
				{
					this.SetAnimationInProgress(false);
				}
				this.GetNextAnimation();
				return;
			}
		}
		else
		{
			this.animationFailsafeQueueSize = 0;
			this.animationFailsafeTimestamp = Time.time;
		}
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x0002B560 File Offset: 0x00029760
	public void Init()
	{
		GameController.OnEndTurnClick += this.ResetMergeActionsFlags;
		if (PlatformManager.IsMobile)
		{
			GameController.AfterEndTurnAIAndPlayer += this.OnTurnEnd;
			return;
		}
		GameController.AfterEndTurnAIAndPlayer += this.CleanSkippedHexesUpdateBool;
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x0002B59D File Offset: 0x0002979D
	public void OnUndo()
	{
		this.OnTurnEnd();
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x0002B5A5 File Offset: 0x000297A5
	private void OnDisable()
	{
		this.ClearDelegates();
		this.ClearAnimationReportCounters();
		this.SetupNormalSpeed();
		this.SetLastAnimation(null);
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x00068E94 File Offset: 0x00067094
	private void ClearDelegates()
	{
		GameController.OnEndTurnClick -= this.ResetMergeActionsFlags;
		if (PlatformManager.IsMobile)
		{
			GameController.AfterEndTurnAIAndPlayer -= this.OnTurnEnd;
		}
		else
		{
			GameController.AfterEndTurnAIAndPlayer -= this.CleanSkippedHexesUpdateBool;
		}
		this.payGainHexes.Clear();
		this.bonusAnimationList.Clear();
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x0002B5C0 File Offset: 0x000297C0
	public bool MoreAnimations()
	{
		return this.enemyActionAnimations.Count > 0;
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x00068EF4 File Offset: 0x000670F4
	public void ClearOnLoad()
	{
		this.ClearAnimationReportCounters();
		GameController.OnEndTurnClick -= this.ResetMergeActionsFlags;
		if (PlatformManager.IsMobile)
		{
			GameController.AfterEndTurnAIAndPlayer -= this.OnTurnEnd;
		}
		else
		{
			GameController.AfterEndTurnAIAndPlayer -= this.CleanSkippedHexesUpdateBool;
		}
		this.SetAnimationInProgress(false);
		this.enemyActionAnimations.Clear();
		this.workersToAnimation.Clear();
		this.firstProduceReceived = false;
		this.SetLastAnimation(null);
		GameObject[] array = this.enemyPlayerMats;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		this.enlistFlashAnimation.SetActive(false);
		this.upgradeGreenFlashAnimation.SetActive(false);
		this.upgradeRedFlashAnimation.SetActive(false);
		this.cameraMovementEffects.presenationFinished = true;
		CameraControler.CameraMovementBlocked = false;
		this.payGainHexes.Clear();
		this.bonusAnimationList.Clear();
		ResourcesObjectPoolScript.Instance.HideAllObjects();
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x00068FE4 File Offset: 0x000671E4
	private void ClearAnimationReportCounters()
	{
		this.workersToLoad = (this.workersToLoadReported = (this.workersToUnload = (this.workersToUnloadReported = (this.unitsToChangePositions = (this.unitsToChangePositionsReported = 0)))));
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x00069028 File Offset: 0x00067228
	private void ActionAnimationInterpreter(EnemyActionAnimation enemyAction)
	{
		switch (enemyAction.animationType)
		{
		case EnemyActionAnimationType.CameraToBaseAnimation:
		{
			CameraToBaseAnimation cameraToBaseAnimation = enemyAction as CameraToBaseAnimation;
			this.AnimateCamToBase(cameraToBaseAnimation.faction);
			return;
		}
		case EnemyActionAnimationType.PayTopStatsAnimation:
		{
			PayTopStatsAnimation payTopStatsAnimation = enemyAction as PayTopStatsAnimation;
			this.PayTopStats(payTopStatsAnimation.gainType, payTopStatsAnimation.resourceStartPosition);
			return;
		}
		case EnemyActionAnimationType.PayResourcesAnimation:
		{
			PayResourceAnimation payResourceAnimation = enemyAction as PayResourceAnimation;
			this.EnemyPayResourceAnimation(payResourceAnimation);
			return;
		}
		case EnemyActionAnimationType.GainTopStatsAnimation:
		{
			GainTopStatsAnimation gainTopStatsAnimation = enemyAction as GainTopStatsAnimation;
			if (gainTopStatsAnimation.isRecruitBonusAnimation)
			{
				this.GainTopStats(gainTopStatsAnimation.gainType, this.GetBonusMarkerPosition(gainTopStatsAnimation.actionOwner), gainTopStatsAnimation.resourceStartPosition);
				return;
			}
			this.GainTopStats(gainTopStatsAnimation.gainType, gainTopStatsAnimation.resourceStartPosition);
			return;
		}
		case EnemyActionAnimationType.CameraToHexAnimation:
		{
			CameraToHexAnimation cameraToHexAnimation = enemyAction as CameraToHexAnimation;
			this.AnimateCamToHex(cameraToHexAnimation.hex);
			return;
		}
		case EnemyActionAnimationType.MechSkillAnimation:
		{
			MechSkillAnimation mechSkillAnimation = enemyAction as MechSkillAnimation;
			this.MechSkillAnimation(GameController.GetUnitPresenter(mechSkillAnimation.mechToDeploy).gameObject.transform.position, mechSkillAnimation.allPlayerBattleUnits, mechSkillAnimation.skillIndex, mechSkillAnimation.actionOwner);
			return;
		}
		case EnemyActionAnimationType.UpgradeAnimation:
		{
			UpgradeAnimation upgradeAnimation = enemyAction as UpgradeAnimation;
			this.MatAnimation(upgradeAnimation.owner, upgradeAnimation.downAction, upgradeAnimation.topAction, upgradeAnimation.gainType);
			return;
		}
		case EnemyActionAnimationType.ProduceResourceAnimation:
		{
			ProduceResourceAnimation produceResourceAnimation = enemyAction as ProduceResourceAnimation;
			this.EnemyProduceAnimation(produceResourceAnimation);
			return;
		}
		case EnemyActionAnimationType.ProduceWorkerAnimation:
		{
			ProduceWorkerAnimation produceWorkerAnimation = enemyAction as ProduceWorkerAnimation;
			this.EnemyProduceWorkerAnimation(produceWorkerAnimation);
			return;
		}
		case EnemyActionAnimationType.TradeResourceAnimation:
		case EnemyActionAnimationType.TakingPositionAfterFight:
			break;
		case EnemyActionAnimationType.CameraToAllHexes:
		{
			CameraToAllHexesAnimation cameraToAllHexesAnimation = enemyAction as CameraToAllHexesAnimation;
			this.AnimateCamToShowAllHexes(new Vector2
			{
				x = cameraToAllHexesAnimation.cameraPosition.x,
				y = cameraToAllHexesAnimation.cameraPosition.y
			}, cameraToAllHexesAnimation.cameraZoom);
			return;
		}
		case EnemyActionAnimationType.SpawnBuildingAnimation:
		{
			BuildSpawnAnimation buildSpawnAnimation = enemyAction as BuildSpawnAnimation;
			this.SpawnBuilding(buildSpawnAnimation);
			return;
		}
		case EnemyActionAnimationType.SpawnMechAnimation:
		{
			SpawnMechAnimation spawnMechAnimation = enemyAction as SpawnMechAnimation;
			this.EnemySpawnMechAnimation(spawnMechAnimation);
			return;
		}
		case EnemyActionAnimationType.EnlistMatAnimation:
		{
			EnlistMatAnimation enlistMatAnimation = enemyAction as EnlistMatAnimation;
			this.MatAnimation(enlistMatAnimation.owner, enlistMatAnimation.DownAction);
			return;
		}
		case EnemyActionAnimationType.MoveUnitAnimation:
		{
			MoveUnitAnimation moveUnitAnimation = enemyAction as MoveUnitAnimation;
			if (!moveUnitAnimation.destinationIsBattlefield && !moveUnitAnimation.takingOwnerPosition)
			{
				this.MoveUnitAnimation(moveUnitAnimation.unitToAnimate, moveUnitAnimation.possibleMoves, moveUnitAnimation.from, moveUnitAnimation.to);
				return;
			}
			if (moveUnitAnimation.destinationIsBattlefield)
			{
				this.MoveToBattlefieldAnimation(moveUnitAnimation.unitToAnimate, moveUnitAnimation.moveFromPosition, moveUnitAnimation.from, moveUnitAnimation.to);
				return;
			}
			if (moveUnitAnimation.takingOwnerPosition)
			{
				this.TakePositionUnitAnimation(moveUnitAnimation.unitToAnimate, moveUnitAnimation.positionAfterFight);
				return;
			}
			break;
		}
		case EnemyActionAnimationType.MoveRetreatUnitsAnimation:
		{
			MoveRetreatUnitAnimation moveRetreatUnitAnimation = enemyAction as MoveRetreatUnitAnimation;
			this.MoveRetreatAnimation(moveRetreatUnitAnimation.allUnitsToRetreat, moveRetreatUnitAnimation.positionBase);
			return;
		}
		case EnemyActionAnimationType.LoadWorkerAnimation:
		{
			LoadWorkerAnimation loadWorkerAnimation = enemyAction as LoadWorkerAnimation;
			this.LoadWorkerAnimation(loadWorkerAnimation.allWorkersToLoad, loadWorkerAnimation.mech);
			return;
		}
		case EnemyActionAnimationType.UnloadWorkerAnimation:
		{
			UnloadWorkerAnimation unloadWorkerAnimation = enemyAction as UnloadWorkerAnimation;
			this.UnloadWorkerAnimation(unloadWorkerAnimation);
			return;
		}
		case EnemyActionAnimationType.LoadResourcesAnimation:
		{
			LoadResourcesAnimation loadResourcesAnimation = enemyAction as LoadResourcesAnimation;
			this.LoadResourcesAnimation(loadResourcesAnimation.resources, loadResourcesAnimation.unit, loadResourcesAnimation.hex);
			return;
		}
		case EnemyActionAnimationType.EndTurnAnimation:
			this.EnemyEndTurnAnimation(enemyAction as EndTurnAnimation);
			break;
		default:
			return;
		}
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x00069344 File Offset: 0x00067544
	public void GetNextAnimation()
	{
		if (!this.animationInProgress)
		{
			if (this.enemyActionAnimations.Count != 0)
			{
				if (OptionsManager.IsCameraAnimationsActive())
				{
					CameraControler.CameraMovementBlocked = true;
				}
				this.cameraMovementEffects.presenationFinished = false;
				EnemyActionAnimation enemyActionAnimation = this.enemyActionAnimations.Dequeue();
				if (!OptionsManager.IsCameraAnimationsActive() && (enemyActionAnimation.animationType == EnemyActionAnimationType.CameraToHexAnimation || enemyActionAnimation.animationType == EnemyActionAnimationType.CameraToAllHexes) && this.enemyActionAnimations.Count != 0)
				{
					enemyActionAnimation = this.enemyActionAnimations.Dequeue();
				}
				if (PlatformManager.IsMobile)
				{
					if (enemyActionAnimation.actionType != LogInfoType.TurnStarts)
					{
						this.GameController.turnInfoPanel.EnableEnemyActionInfo(enemyActionAnimation.actionOwner);
						this.GameController.turnInfoPanel.SetInfoText(this.GetActionNameText(enemyActionAnimation.actionType));
					}
				}
				else if (!GameController.GameManager.GetPlayerByFaction(enemyActionAnimation.actionOwner).IsHuman)
				{
					this.GameController.turnInfoPanel.EnableEnemyActionInfo(enemyActionAnimation.actionOwner);
					this.GameController.turnInfoPanel.SetupActualActionAnimationName(this.GetActionNameText(enemyActionAnimation.actionType));
				}
				this.ActionAnimationInterpreter(enemyActionAnimation);
				return;
			}
			this.enemyActionAnimations.Clear();
			if (PlatformManager.IsMobile)
			{
				Faction faction = GameController.GameManager.PlayerCurrent.matFaction.faction;
				if (this.lastAnimation != null)
				{
					faction = this.lastAnimation.actionOwner;
					if (this.lastAnimation.actionType != LogInfoType.TurnStarts)
					{
						this.GameController.turnInfoPanel.DisableEnemyActionInfo();
					}
				}
				else
				{
					this.GameController.turnInfoPanel.DisableEnemyActionInfo();
				}
				if (GameController.GameManager.IsMultiplayer && faction != GameController.GameManager.PlayerCurrent.matFaction.faction)
				{
					this.GameController.turnInfoPanel.ActivateTurnInfoPanel();
				}
			}
			else
			{
				this.GameController.turnInfoPanel.DisableEnemyActionInfo();
				if (!GameController.GameManager.IsCampaign)
				{
					this.GameController.turnInfoPanel.ActivateTurnInfoPanel();
				}
			}
			this.SetLastAnimation(null);
			this.SetAnimationInProgress(false);
			this.cameraMovementEffects.presenationFinished = true;
			CameraControler.CameraMovementBlocked = false;
			this.ReattachDelegates();
			if (!GameController.GameManager.IsMultiplayer && !GameController.GameManager.PlayerCurrent.IsHuman && (!GameController.GameManager.combatManager.IsPlayerInCombat() || !GameController.GameManager.combatManager.GetDefender().IsHuman))
			{
				this.GameController.GetNextPlayerAfterEnemyAnimation();
			}
		}
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x000695A0 File Offset: 0x000677A0
	public void EnableAnimationVignette()
	{
		VignetteModel.Settings settings = this.postProcessingProfile.vignette.settings;
		settings.intensity = 0.467f;
		this.postProcessingProfile.vignette.settings = settings;
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x000695DC File Offset: 0x000677DC
	public void DisableAnimationVignette()
	{
		VignetteModel.Settings settings = this.postProcessingProfile.vignette.settings;
		settings.intensity = this.vinietteIntensityDefault;
		this.postProcessingProfile.vignette.settings = settings;
	}

	// Token: 0x06000563 RID: 1379 RVA: 0x00069618 File Offset: 0x00067818
	private void ReattachDelegates()
	{
		if (GameController.GameManager.IsMultiplayer)
		{
			return;
		}
		if (PlatformManager.IsStandalone)
		{
			GameController.Instance.combatPresenter.AddDelegate();
			if (GameController.GameManager.combatManager.CombatAlreadyStarted())
			{
				GameController.Instance.combatPresenter.ChangeLayout(GameController.GameManager.combatManager.GetActualStage());
			}
		}
		else
		{
			GameController.Instance.combatPresenterMobile.AddDelegate();
			if (GameController.GameManager.combatManager.CombatAlreadyStarted())
			{
				GameController.Instance.combatPresenterMobile.ChangeLayout(GameController.GameManager.combatManager.GetActualStage());
			}
		}
		if (!GameController.Instance.endGameSequencePresenter.SequencePlayed())
		{
			GameController.Instance.AddGameEndedListerner();
			if (GameController.GameManager.GameFinished)
			{
				GameController.Instance.GameEnded();
			}
		}
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x0002B5D0 File Offset: 0x000297D0
	private void SetLastAnimation(EnemyActionAnimation lastAnimation)
	{
		this.lastAnimation = lastAnimation;
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x0002B5D9 File Offset: 0x000297D9
	public void SetAnimationInProgress(bool animationInProgress)
	{
		this.animationInProgress = animationInProgress;
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x000696E8 File Offset: 0x000678E8
	public void WorkerUnloadedReport()
	{
		this.workersToUnloadReported++;
		if (this.workersToUnload == this.workersToUnloadReported)
		{
			this.SetAnimationInProgress(false);
			this.workersToUnload = (this.workersToUnloadReported = 0);
			this.GetNextAnimation();
		}
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x00069730 File Offset: 0x00067930
	public void WorkerLoadedReport()
	{
		this.workersToLoadReported++;
		this.GetNextAnimation();
		if (this.workersToLoad == this.workersToLoadReported)
		{
			this.SetAnimationInProgress(false);
			this.workersToLoad = (this.workersToLoadReported = 0);
		}
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x00069778 File Offset: 0x00067978
	public void MoveEndedReport(bool takeOwnerPosition)
	{
		if (takeOwnerPosition)
		{
			this.unitsToChangePositionsReported++;
		}
		if (!takeOwnerPosition || (takeOwnerPosition && this.unitsToChangePositions == this.unitsToChangePositionsReported))
		{
			if (takeOwnerPosition)
			{
				this.unitsToChangePositions = (this.unitsToChangePositionsReported = 0);
			}
			this.SetAnimationInProgress(false);
			this.GetNextAnimation();
		}
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x000697CC File Offset: 0x000679CC
	private bool LastMoveToTakeOwnerPositionAnimation()
	{
		this.unitsToChangePositionsReported++;
		return this.enemyActionAnimations.Count == 0 || this.enemyActionAnimations.Count == 0 || !(this.enemyActionAnimations.Peek() is MoveUnitAnimation) || !(this.enemyActionAnimations.Peek() as MoveUnitAnimation).takingOwnerPosition;
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x00069830 File Offset: 0x00067A30
	public void OnEnemyPayResourcesFromHex(EnemyPayResourceFromHexInfo enemyActionInfo)
	{
		this.payGainHexes.Add(enemyActionInfo.gameHex);
		if (OptionsManager.IsCameraAnimationsActive())
		{
			CameraToAllHexesAnimation cameraToAllHexesAnimation = new CameraToAllHexesAnimation();
			cameraToAllHexesAnimation.actionOwner = enemyActionInfo.actionOwner;
			cameraToAllHexesAnimation.actionType = enemyActionInfo.actionType;
			cameraToAllHexesAnimation.animationType = EnemyActionAnimationType.CameraToAllHexes;
			List<Vector3> list = new List<Vector3>();
			foreach (GameHex gameHex in enemyActionInfo.allHexes)
			{
				list.Add(this.GameController.GetGameHexPresenter(gameHex).GetWorldPosition());
			}
			cameraToAllHexesAnimation.cameraPosition = AnimateCamera.Instance.CalculateCenterOfHexes(list);
			cameraToAllHexesAnimation.cameraZoom = AnimateCamera.Instance.CalculateZoomToShowGivenHexes(enemyActionInfo.allHexes);
			this.enemyActionAnimations.Enqueue(cameraToAllHexesAnimation);
			this.SetLastAnimation(cameraToAllHexesAnimation);
		}
		PayResourceAnimation payResourceAnimation = new PayResourceAnimation();
		payResourceAnimation.actionOwner = enemyActionInfo.actionOwner;
		payResourceAnimation.actionType = enemyActionInfo.actionType;
		payResourceAnimation.animationType = EnemyActionAnimationType.PayResourcesAnimation;
		payResourceAnimation.hex = enemyActionInfo.gameHex;
		payResourceAnimation.resourceType = enemyActionInfo.resourceType;
		payResourceAnimation.amountOfResources = enemyActionInfo.amount;
		this.enemyActionAnimations.Enqueue(payResourceAnimation);
		this.SetLastAnimation(payResourceAnimation);
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x00069978 File Offset: 0x00067B78
	public void OnEnemyBuildActionAnim(BuildEnemyActionInfo enemyActionInfo)
	{
		if (OptionsManager.IsCameraAnimationsActive())
		{
			CameraToHexAnimation cameraToHexAnimation = new CameraToHexAnimation();
			cameraToHexAnimation.actionOwner = enemyActionInfo.actionOwner;
			cameraToHexAnimation.actionType = enemyActionInfo.actionType;
			cameraToHexAnimation.animationType = EnemyActionAnimationType.CameraToHexAnimation;
			cameraToHexAnimation.hex = this.GameController.GetGameHexPresenter(enemyActionInfo.buildingHex).GetWorldPosition();
			this.enemyActionAnimations.Enqueue(cameraToHexAnimation);
			this.SetLastAnimation(cameraToHexAnimation);
		}
		BuildSpawnAnimation buildSpawnAnimation = new BuildSpawnAnimation();
		buildSpawnAnimation.actionOwner = enemyActionInfo.actionOwner;
		buildSpawnAnimation.actionType = enemyActionInfo.actionType;
		buildSpawnAnimation.animationType = EnemyActionAnimationType.SpawnBuildingAnimation;
		buildSpawnAnimation.hex = this.GameController.GetGameHexPresenter(enemyActionInfo.buildingHex);
		buildSpawnAnimation.owner = enemyActionInfo.actionOwner;
		buildSpawnAnimation.buildingToSpawn = enemyActionInfo.building;
		this.enemyActionAnimations.Enqueue(buildSpawnAnimation);
		this.SetLastAnimation(buildSpawnAnimation);
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x00069A48 File Offset: 0x00067C48
	public void OnEnemyDeployActionAnim(DeployEnemyActionInfo enemyActionInfo)
	{
		if (OptionsManager.IsCameraAnimationsActive())
		{
			CameraToHexAnimation cameraToHexAnimation = new CameraToHexAnimation();
			cameraToHexAnimation.actionOwner = enemyActionInfo.actionOwner;
			cameraToHexAnimation.actionType = enemyActionInfo.actionType;
			cameraToHexAnimation.animationType = EnemyActionAnimationType.CameraToHexAnimation;
			cameraToHexAnimation.hex = this.GameController.GetGameHexPresenter(enemyActionInfo.mechHex).GetWorldPosition();
			this.enemyActionAnimations.Enqueue(cameraToHexAnimation);
			this.SetLastAnimation(cameraToHexAnimation);
		}
		SpawnMechAnimation spawnMechAnimation = new SpawnMechAnimation();
		spawnMechAnimation.deployPosition = enemyActionInfo.mechHex;
		spawnMechAnimation.actionOwner = enemyActionInfo.actionOwner;
		spawnMechAnimation.actionType = enemyActionInfo.actionType;
		spawnMechAnimation.animationType = EnemyActionAnimationType.SpawnMechAnimation;
		spawnMechAnimation.mechToSpawn = enemyActionInfo.mechToDeploy;
		this.enemyActionAnimations.Enqueue(spawnMechAnimation);
		this.SetLastAnimation(spawnMechAnimation);
		if (OptionsManager.IsCameraAnimationsActive())
		{
			CameraToAllHexesAnimation cameraToAllHexesAnimation = new CameraToAllHexesAnimation();
			cameraToAllHexesAnimation.actionOwner = enemyActionInfo.actionOwner;
			cameraToAllHexesAnimation.actionType = enemyActionInfo.actionType;
			cameraToAllHexesAnimation.animationType = EnemyActionAnimationType.CameraToAllHexes;
			List<Vector3> list = new List<Vector3>();
			foreach (GameHex gameHex in enemyActionInfo.allHexes)
			{
				list.Add(this.GameController.GetGameHexPresenter(gameHex).GetWorldPosition());
			}
			cameraToAllHexesAnimation.cameraPosition = AnimateCamera.Instance.CalculateCenterOfHexes(list);
			cameraToAllHexesAnimation.cameraZoom = AnimateCamera.Instance.CalculateZoomToShowGivenHexes(enemyActionInfo.allHexes);
			this.enemyActionAnimations.Enqueue(cameraToAllHexesAnimation);
			this.SetLastAnimation(cameraToAllHexesAnimation);
		}
		MechSkillAnimation mechSkillAnimation = new MechSkillAnimation();
		mechSkillAnimation.actionOwner = enemyActionInfo.actionOwner;
		mechSkillAnimation.actionType = enemyActionInfo.actionType;
		mechSkillAnimation.animationType = EnemyActionAnimationType.MechSkillAnimation;
		mechSkillAnimation.mechHex = enemyActionInfo.mechHex;
		mechSkillAnimation.allPlayerBattleUnits = enemyActionInfo.allPlayerBattleUnits;
		mechSkillAnimation.skillIndex = enemyActionInfo.skillIndex;
		mechSkillAnimation.mechToDeploy = enemyActionInfo.mechToDeploy;
		this.enemyActionAnimations.Enqueue(mechSkillAnimation);
		this.SetLastAnimation(mechSkillAnimation);
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x00069C3C File Offset: 0x00067E3C
	public void OnEnemyGenerateRecruitBonusActionAnim(EnlistBonusEnemyActionInfo enemyActionInfo)
	{
		GainTopStatsAnimation gainTopStatsAnimation = new GainTopStatsAnimation();
		gainTopStatsAnimation.actionOwner = enemyActionInfo.actionOwner;
		gainTopStatsAnimation.actionType = LogInfoType.RecruitBonus;
		gainTopStatsAnimation.animationType = EnemyActionAnimationType.GainTopStatsAnimation;
		gainTopStatsAnimation.gainType = enemyActionInfo.oneTimeBonus;
		gainTopStatsAnimation.isRecruitBonusAnimation = true;
		gainTopStatsAnimation.resourceStartPosition = this.GetBaseResourcePosition(enemyActionInfo.actionOwner, this.coinMaxHeight);
		this.bonusAnimationList.Add(gainTopStatsAnimation);
		this.SetLastAnimation(gainTopStatsAnimation);
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x00069CA8 File Offset: 0x00067EA8
	public void OnEnemysBonusesEnd()
	{
		if (this.bonusAnimationList.Count == 3)
		{
			if (OptionsManager.IsCameraAnimationsActive())
			{
				CameraToAllHexesAnimation cameraToAllHexesAnimation = new CameraToAllHexesAnimation();
				cameraToAllHexesAnimation.actionOwner = GameController.GameManager.PlayerCurrent.matFaction.faction;
				cameraToAllHexesAnimation.actionType = LogInfoType.RecruitBonus;
				cameraToAllHexesAnimation.animationType = EnemyActionAnimationType.CameraToAllHexes;
				List<Vector3> list = new List<Vector3>();
				HashSet<GameHex> hashSet = new HashSet<GameHex>();
				list.Add(this.GameController.GetGameHexPresenter(GameController.GameManager.GetPlayerByFaction(this.bonusAnimationList[0].actionOwner).GetCapital()).GetWorldPosition());
				list.Add(this.GameController.GetGameHexPresenter(GameController.GameManager.GetPlayerByFaction(this.bonusAnimationList[1].actionOwner).GetCapital()).GetWorldPosition());
				list.Add(this.GameController.GetGameHexPresenter(GameController.GameManager.GetPlayerByFaction(this.bonusAnimationList[2].actionOwner).GetCapital()).GetWorldPosition());
				hashSet.Add(GameController.GameManager.GetPlayerByFaction(this.bonusAnimationList[0].actionOwner).GetCapital());
				hashSet.Add(GameController.GameManager.GetPlayerByFaction(this.bonusAnimationList[1].actionOwner).GetCapital());
				hashSet.Add(GameController.GameManager.GetPlayerByFaction(this.bonusAnimationList[2].actionOwner).GetCapital());
				cameraToAllHexesAnimation.cameraPosition = AnimateCamera.Instance.CalculateCenterOfHexes(list);
				cameraToAllHexesAnimation.cameraZoom = AnimateCamera.Instance.CalculateZoomToShowGivenHexes(hashSet);
				this.enemyActionAnimations.Enqueue(cameraToAllHexesAnimation);
				this.SetLastAnimation(cameraToAllHexesAnimation);
			}
			this.enemyActionAnimations.Enqueue(this.bonusAnimationList[0]);
			this.SetLastAnimation(this.bonusAnimationList[0]);
			this.enemyActionAnimations.Enqueue(this.bonusAnimationList[1]);
			this.SetLastAnimation(this.bonusAnimationList[1]);
			this.enemyActionAnimations.Enqueue(this.bonusAnimationList[2]);
			this.SetLastAnimation(this.bonusAnimationList[2]);
		}
		else if (this.bonusAnimationList.Count == 2)
		{
			if (OptionsManager.IsCameraAnimationsActive())
			{
				CameraToAllHexesAnimation cameraToAllHexesAnimation2 = new CameraToAllHexesAnimation();
				cameraToAllHexesAnimation2.actionOwner = GameController.GameManager.PlayerCurrent.matFaction.faction;
				cameraToAllHexesAnimation2.actionType = LogInfoType.RecruitBonus;
				cameraToAllHexesAnimation2.animationType = EnemyActionAnimationType.CameraToAllHexes;
				List<Vector3> list2 = new List<Vector3>();
				HashSet<GameHex> hashSet2 = new HashSet<GameHex>();
				list2.Add(this.GameController.GetGameHexPresenter(GameController.GameManager.GetPlayerByFaction(this.bonusAnimationList[0].actionOwner).GetCapital()).GetWorldPosition());
				list2.Add(this.GameController.GetGameHexPresenter(GameController.GameManager.GetPlayerByFaction(this.bonusAnimationList[1].actionOwner).GetCapital()).GetWorldPosition());
				hashSet2.Add(GameController.GameManager.GetPlayerByFaction(this.bonusAnimationList[0].actionOwner).GetCapital());
				hashSet2.Add(GameController.GameManager.GetPlayerByFaction(this.bonusAnimationList[1].actionOwner).GetCapital());
				cameraToAllHexesAnimation2.cameraPosition = AnimateCamera.Instance.CalculateCenterOfHexes(list2);
				cameraToAllHexesAnimation2.cameraZoom = AnimateCamera.Instance.CalculateZoomToShowGivenHexes(hashSet2);
				this.enemyActionAnimations.Enqueue(cameraToAllHexesAnimation2);
				this.SetLastAnimation(cameraToAllHexesAnimation2);
			}
			this.enemyActionAnimations.Enqueue(this.bonusAnimationList[0]);
			this.SetLastAnimation(this.bonusAnimationList[0]);
			this.enemyActionAnimations.Enqueue(this.bonusAnimationList[1]);
			this.SetLastAnimation(this.bonusAnimationList[1]);
		}
		else if (this.bonusAnimationList.Count == 1)
		{
			if (OptionsManager.IsCameraAnimationsActive())
			{
				CameraToBaseAnimation cameraToBaseAnimation = new CameraToBaseAnimation();
				cameraToBaseAnimation.actionOwner = this.bonusAnimationList[0].actionOwner;
				cameraToBaseAnimation.actionType = LogInfoType.RecruitBonus;
				cameraToBaseAnimation.animationType = EnemyActionAnimationType.CameraToBaseAnimation;
				cameraToBaseAnimation.faction = this.bonusAnimationList[0].actionOwner;
				this.enemyActionAnimations.Enqueue(cameraToBaseAnimation);
				this.SetLastAnimation(cameraToBaseAnimation);
			}
			this.enemyActionAnimations.Enqueue(this.bonusAnimationList[0]);
			this.SetLastAnimation(this.bonusAnimationList[0]);
		}
		this.bonusAnimationList.Clear();
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x0006A13C File Offset: 0x0006833C
	public void OnEnemyRecruitActionAnim(EnlistEnemyActionInfo enemyActionInfo)
	{
		if (OptionsManager.IsCameraAnimationsActive())
		{
			CameraToAllHexesAnimation cameraToAllHexesAnimation = new CameraToAllHexesAnimation();
			cameraToAllHexesAnimation.actionOwner = enemyActionInfo.actionOwner;
			cameraToAllHexesAnimation.actionType = enemyActionInfo.actionType;
			cameraToAllHexesAnimation.animationType = EnemyActionAnimationType.CameraToAllHexes;
			List<Vector3> list = new List<Vector3>();
			foreach (GameHex gameHex in enemyActionInfo.allHexes)
			{
				if (gameHex != null)
				{
					list.Add(this.GameController.GetGameHexPresenter(gameHex).GetWorldPosition());
				}
			}
			cameraToAllHexesAnimation.cameraPosition = AnimateCamera.Instance.CalculateCenterOfHexes(list);
			cameraToAllHexesAnimation.cameraZoom = AnimateCamera.Instance.CalculateZoomToShowGivenHexes(enemyActionInfo.allHexes);
			this.enemyActionAnimations.Enqueue(cameraToAllHexesAnimation);
			this.SetLastAnimation(cameraToAllHexesAnimation);
		}
		EnlistMatAnimation enlistMatAnimation = new EnlistMatAnimation();
		enlistMatAnimation.actionOwner = enemyActionInfo.actionOwner;
		enlistMatAnimation.actionType = enemyActionInfo.actionType;
		enlistMatAnimation.animationType = EnemyActionAnimationType.EnlistMatAnimation;
		enlistMatAnimation.DownAction = enemyActionInfo.typeOfDownAction;
		enlistMatAnimation.owner = GameController.GameManager.GetPlayerByFaction(enemyActionInfo.actionOwner);
		this.enemyActionAnimations.Enqueue(enlistMatAnimation);
		this.SetLastAnimation(enlistMatAnimation);
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x0006A278 File Offset: 0x00068478
	public void OnEnemyTradeActionAnim(TradeEnemyActionInfo enemyActionInfo)
	{
		this.payGainHexes.AddRange(enemyActionInfo.hexes);
		if (OptionsManager.IsCameraAnimationsActive())
		{
			CameraToBaseAnimation cameraToBaseAnimation = new CameraToBaseAnimation();
			cameraToBaseAnimation.actionOwner = enemyActionInfo.actionOwner;
			cameraToBaseAnimation.actionType = enemyActionInfo.actionType;
			cameraToBaseAnimation.animationType = EnemyActionAnimationType.CameraToBaseAnimation;
			cameraToBaseAnimation.faction = enemyActionInfo.actionOwner;
			this.enemyActionAnimations.Enqueue(cameraToBaseAnimation);
			this.SetLastAnimation(cameraToBaseAnimation);
		}
		PayTopStatsAnimation payTopStatsAnimation = new PayTopStatsAnimation();
		payTopStatsAnimation.actionOwner = enemyActionInfo.actionOwner;
		payTopStatsAnimation.actionType = enemyActionInfo.actionType;
		payTopStatsAnimation.animationType = EnemyActionAnimationType.PayTopStatsAnimation;
		payTopStatsAnimation.gainType = GainType.Coin;
		payTopStatsAnimation.resourceStartPosition = this.GetBaseResourcePosition(enemyActionInfo.actionOwner, this.coinMinHeight);
		this.enemyActionAnimations.Enqueue(payTopStatsAnimation);
		this.SetLastAnimation(payTopStatsAnimation);
		if (OptionsManager.IsCameraAnimationsActive())
		{
			CameraToAllHexesAnimation cameraToAllHexesAnimation = new CameraToAllHexesAnimation();
			cameraToAllHexesAnimation.actionOwner = enemyActionInfo.actionOwner;
			cameraToAllHexesAnimation.actionType = enemyActionInfo.actionType;
			cameraToAllHexesAnimation.animationType = EnemyActionAnimationType.CameraToAllHexes;
			List<Vector3> list = new List<Vector3>();
			HashSet<GameHex> hashSet = new HashSet<GameHex>();
			for (int i = 0; i < enemyActionInfo.hexes.Count; i++)
			{
				list.Add(this.GameController.GetGameHexPresenter(enemyActionInfo.hexes[i]).GetWorldPosition());
				hashSet.Add(enemyActionInfo.hexes[i]);
			}
			cameraToAllHexesAnimation.cameraPosition = AnimateCamera.Instance.CalculateCenterOfHexes(list);
			cameraToAllHexesAnimation.cameraZoom = AnimateCamera.Instance.CalculateZoomToShowGivenHexes(hashSet);
			this.enemyActionAnimations.Enqueue(cameraToAllHexesAnimation);
			this.SetLastAnimation(cameraToAllHexesAnimation);
		}
		for (int j = 0; j < enemyActionInfo.hexes.Count; j++)
		{
			foreach (ResourceType resourceType in enemyActionInfo.resourcesToTrade[j].Keys)
			{
				ProduceResourceAnimation produceResourceAnimation = new ProduceResourceAnimation();
				produceResourceAnimation.actionOwner = enemyActionInfo.actionOwner;
				produceResourceAnimation.actionType = enemyActionInfo.actionType;
				produceResourceAnimation.animationType = EnemyActionAnimationType.ProduceResourceAnimation;
				produceResourceAnimation.hex = enemyActionInfo.hexes[j];
				produceResourceAnimation.resourceType = resourceType;
				produceResourceAnimation.amountOfResources = 1;
				this.enemyActionAnimations.Enqueue(produceResourceAnimation);
				this.SetLastAnimation(produceResourceAnimation);
			}
		}
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x0006A4D4 File Offset: 0x000686D4
	public void OnEnemyUpgradeActionAnim(UpgradeEnemyActionInfo enemyActionInfo)
	{
		UpgradeAnimation upgradeAnimation = new UpgradeAnimation();
		upgradeAnimation.actionOwner = enemyActionInfo.actionOwner;
		upgradeAnimation.actionType = enemyActionInfo.actionType;
		upgradeAnimation.animationType = EnemyActionAnimationType.UpgradeAnimation;
		upgradeAnimation.owner = GameController.GameManager.GetPlayerByFaction(enemyActionInfo.actionOwner);
		upgradeAnimation.downAction = this.GetDownActionType(enemyActionInfo.DownResource);
		upgradeAnimation.topAction = this.GetTopActionType(enemyActionInfo.TopAction);
		upgradeAnimation.gainType = enemyActionInfo.TopAction;
		this.enemyActionAnimations.Enqueue(upgradeAnimation);
		this.SetLastAnimation(upgradeAnimation);
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x0006A560 File Offset: 0x00068760
	public void OnEnemyUnitMoveAnim(MoveEnemyActionInfo enemyActionInfo)
	{
		this.PrepareLoadWorkersBeforeMove(enemyActionInfo);
		bool flag = this.lastAnimation != null && this.lastAnimation is MoveUnitAnimation && (this.lastAnimation as MoveUnitAnimation).takingOwnerPosition;
		if (OptionsManager.IsCameraAnimationsActive() && (!enemyActionInfo.takingOwnerPosition || (enemyActionInfo.takingOwnerPosition && !flag)))
		{
			CameraToAllHexesAnimation cameraToAllHexesAnimation = new CameraToAllHexesAnimation();
			cameraToAllHexesAnimation.actionOwner = enemyActionInfo.actionOwner;
			cameraToAllHexesAnimation.actionType = enemyActionInfo.actionType;
			cameraToAllHexesAnimation.animationType = EnemyActionAnimationType.CameraToAllHexes;
			List<Vector3> list = new List<Vector3>();
			HashSet<GameHex> hashSet = new HashSet<GameHex>();
			list.Add(this.GameController.GetGameHexPresenter(GameController.GetUnitPresenter(enemyActionInfo.unit).hex.GetGameHexLogic()).GetWorldPosition());
			list.Add(this.GameController.GetGameHexPresenter(enemyActionInfo.unit.position).GetWorldPosition());
			hashSet.Add(GameController.GetUnitPresenter(enemyActionInfo.unit).hex.GetGameHexLogic());
			hashSet.Add(this.GameController.GetGameHexPresenter(enemyActionInfo.unit.position).GetGameHexLogic());
			cameraToAllHexesAnimation.cameraPosition = AnimateCamera.Instance.CalculateCenterOfHexes(list);
			cameraToAllHexesAnimation.cameraZoom = AnimateCamera.Instance.CalculateZoomToShowGivenHexes(hashSet);
			this.enemyActionAnimations.Enqueue(cameraToAllHexesAnimation);
			this.SetLastAnimation(cameraToAllHexesAnimation);
		}
		MoveUnitAnimation moveUnitAnimation = new MoveUnitAnimation();
		moveUnitAnimation.actionOwner = enemyActionInfo.actionOwner;
		moveUnitAnimation.actionType = enemyActionInfo.actionType;
		moveUnitAnimation.animationType = EnemyActionAnimationType.MoveUnitAnimation;
		moveUnitAnimation.unitToAnimate = enemyActionInfo.unit;
		moveUnitAnimation.possibleMoves = enemyActionInfo.possibleMoves;
		moveUnitAnimation.destinationIsBattlefield = enemyActionInfo.destinationIsBattlefield;
		moveUnitAnimation.takingOwnerPosition = enemyActionInfo.takingOwnerPosition;
		moveUnitAnimation.moveFromPosition = enemyActionInfo.moveFromPosition;
		moveUnitAnimation.from = enemyActionInfo.fromHex;
		moveUnitAnimation.to = enemyActionInfo.toHex;
		moveUnitAnimation.positionAfterFight = enemyActionInfo.positionAfterFight;
		this.enemyActionAnimations.Enqueue(moveUnitAnimation);
		this.SetLastAnimation(moveUnitAnimation);
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x0006A758 File Offset: 0x00068958
	private void PrepareLoadWorkersBeforeMove(MoveEnemyActionInfo enemyActionInfo)
	{
		if (this.loadToMechAnimations.Count > 0)
		{
			LoadWorkerAnimation loadWorkerAnimation = new LoadWorkerAnimation();
			int count = this.loadToMechAnimations.Count;
			loadWorkerAnimation.animationType = EnemyActionAnimationType.LoadWorkerAnimation;
			for (int i = 0; i < count; i++)
			{
				if (this.loadToMechAnimations.Peek().mech != enemyActionInfo.unit)
				{
					Debug.Log("Load worker break " + this.loadToMechAnimations.Peek().mech.Owner.matFaction.faction.ToString() + " " + enemyActionInfo.unit.Owner.matFaction.faction.ToString());
				}
				else
				{
					OneLoadWorkerAnimation oneLoadWorkerAnimation = this.loadToMechAnimations.Dequeue();
					loadWorkerAnimation.mech = oneLoadWorkerAnimation.mech;
					loadWorkerAnimation.allWorkersToLoad.Add(oneLoadWorkerAnimation.worker);
				}
			}
			loadWorkerAnimation.actionType = enemyActionInfo.actionType;
			loadWorkerAnimation.actionOwner = enemyActionInfo.actionOwner;
			this.enemyActionAnimations.Enqueue(loadWorkerAnimation);
			this.SetLastAnimation(loadWorkerAnimation);
		}
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x0006A870 File Offset: 0x00068A70
	public void OnEnemyLoadResourcesAnim(LoadResourcesEnemyActionInfo enemyActionInfo)
	{
		if (!enemyActionInfo.isUnload)
		{
			CameraToHexAnimation cameraToHexAnimation = new CameraToHexAnimation();
			cameraToHexAnimation.actionOwner = enemyActionInfo.actionOwner;
			cameraToHexAnimation.actionType = enemyActionInfo.actionType;
			cameraToHexAnimation.animationType = EnemyActionAnimationType.CameraToHexAnimation;
			cameraToHexAnimation.hex = this.GameController.GetGameHexPresenter(GameController.GetUnitPresenter(enemyActionInfo.unit).hex.GetGameHexLogic()).GetWorldPosition();
			this.enemyActionAnimations.Enqueue(cameraToHexAnimation);
			this.SetLastAnimation(cameraToHexAnimation);
		}
		LoadResourcesAnimation loadResourcesAnimation = new LoadResourcesAnimation();
		loadResourcesAnimation.actionOwner = enemyActionInfo.actionOwner;
		loadResourcesAnimation.actionType = enemyActionInfo.actionType;
		loadResourcesAnimation.animationType = EnemyActionAnimationType.LoadResourcesAnimation;
		loadResourcesAnimation.resources = enemyActionInfo.resources;
		loadResourcesAnimation.unit = enemyActionInfo.unit;
		loadResourcesAnimation.hex = enemyActionInfo.hex;
		this.enemyActionAnimations.Enqueue(loadResourcesAnimation);
		this.SetLastAnimation(loadResourcesAnimation);
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x0006A948 File Offset: 0x00068B48
	public void OnEnemyLoadWorkerAnim(LoadWorkerActionInfo enemyActionInfo)
	{
		if (OptionsManager.IsCameraAnimationsActive() && !(this.lastAnimation is OneLoadWorkerAnimation) && !(this.lastAnimation is CameraToHexAnimation))
		{
			CameraToHexAnimation cameraToHexAnimation = new CameraToHexAnimation();
			cameraToHexAnimation.actionOwner = enemyActionInfo.actionOwner;
			cameraToHexAnimation.actionType = enemyActionInfo.actionType;
			cameraToHexAnimation.animationType = EnemyActionAnimationType.CameraToHexAnimation;
			cameraToHexAnimation.hex = this.GameController.GetGameHexPresenter(GameController.GetUnitPresenter(enemyActionInfo.mech).hex.GetGameHexLogic()).GetWorldPosition();
			this.enemyActionAnimations.Enqueue(cameraToHexAnimation);
		}
		OneLoadWorkerAnimation oneLoadWorkerAnimation = new OneLoadWorkerAnimation();
		oneLoadWorkerAnimation.mech = enemyActionInfo.mech;
		oneLoadWorkerAnimation.worker = enemyActionInfo.worker;
		this.loadToMechAnimations.Enqueue(oneLoadWorkerAnimation);
		this.SetLastAnimation(oneLoadWorkerAnimation);
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x0006AA04 File Offset: 0x00068C04
	public void OnEnemyUnloadWorkerAnim(UnloadWorkerActionInfo enemyActionInfo)
	{
		UnloadWorkerAnimation unloadWorkerAnimation = new UnloadWorkerAnimation();
		unloadWorkerAnimation.animationType = EnemyActionAnimationType.UnloadWorkerAnimation;
		unloadWorkerAnimation.mech = enemyActionInfo.mech;
		unloadWorkerAnimation.worker = enemyActionInfo.worker;
		unloadWorkerAnimation.positionToUnload = enemyActionInfo.positionToUnload;
		unloadWorkerAnimation.actionType = enemyActionInfo.actionType;
		unloadWorkerAnimation.actionOwner = enemyActionInfo.actionOwner;
		unloadWorkerAnimation.unloadOnBattlefield = enemyActionInfo.unloadOnBattlefield;
		this.enemyActionAnimations.Enqueue(unloadWorkerAnimation);
		this.SetLastAnimation(unloadWorkerAnimation);
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x0006AA7C File Offset: 0x00068C7C
	public void OnEnemyRetreatUnitsMoveAnim(MoveRetreatEnemyActionInfo enemyActionInfo)
	{
		this.PrepareLoadWorkersBeforeRetreat(enemyActionInfo);
		if (this.lastAnimation != null && this.lastAnimation is MoveUnitAnimation)
		{
			bool takingOwnerPosition = (this.lastAnimation as MoveUnitAnimation).takingOwnerPosition;
		}
		if (OptionsManager.IsCameraAnimationsActive())
		{
			CameraToAllHexesAnimation cameraToAllHexesAnimation = new CameraToAllHexesAnimation();
			cameraToAllHexesAnimation.actionOwner = enemyActionInfo.actionOwner;
			cameraToAllHexesAnimation.actionType = enemyActionInfo.actionType;
			cameraToAllHexesAnimation.animationType = EnemyActionAnimationType.CameraToAllHexes;
			List<Vector3> list = new List<Vector3>();
			HashSet<GameHex> hashSet = new HashSet<GameHex>();
			list.Add(this.GameController.GetGameHexPresenter(GameController.GetUnitPresenter(enemyActionInfo.units[0]).hex.GetGameHexLogic()).GetWorldPosition());
			list.Add(this.GameController.GetGameHexPresenter(enemyActionInfo.units[0].position).GetWorldPosition());
			hashSet.Add(GameController.GetUnitPresenter(enemyActionInfo.units[0]).hex.GetGameHexLogic());
			hashSet.Add(this.GameController.GetGameHexPresenter(enemyActionInfo.units[0].position).GetGameHexLogic());
			cameraToAllHexesAnimation.cameraPosition = AnimateCamera.Instance.CalculateCenterOfHexes(list);
			cameraToAllHexesAnimation.cameraZoom = AnimateCamera.Instance.CalculateZoomToShowGivenHexes(hashSet);
			this.enemyActionAnimations.Enqueue(cameraToAllHexesAnimation);
			this.SetLastAnimation(cameraToAllHexesAnimation);
		}
		MoveRetreatUnitAnimation moveRetreatUnitAnimation = new MoveRetreatUnitAnimation();
		moveRetreatUnitAnimation.actionOwner = enemyActionInfo.actionOwner;
		moveRetreatUnitAnimation.actionType = enemyActionInfo.actionType;
		moveRetreatUnitAnimation.animationType = EnemyActionAnimationType.MoveRetreatUnitsAnimation;
		moveRetreatUnitAnimation.allUnitsToRetreat = enemyActionInfo.units;
		moveRetreatUnitAnimation.positionBase = enemyActionInfo.withdrawPositionHex;
		this.enemyActionAnimations.Enqueue(moveRetreatUnitAnimation);
		this.SetLastAnimation(moveRetreatUnitAnimation);
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x0006AC24 File Offset: 0x00068E24
	private void PrepareLoadWorkersBeforeRetreat(MoveRetreatEnemyActionInfo enemyActionInfo)
	{
		if (this.loadToMechAnimations.Count > 0)
		{
			LoadWorkerAnimation loadWorkerAnimation = new LoadWorkerAnimation();
			int count = this.loadToMechAnimations.Count;
			loadWorkerAnimation.animationType = EnemyActionAnimationType.LoadWorkerAnimation;
			for (int i = 0; i < count; i++)
			{
				if (this.loadToMechAnimations.Peek().mech == enemyActionInfo.units[0])
				{
					OneLoadWorkerAnimation oneLoadWorkerAnimation = this.loadToMechAnimations.Dequeue();
					loadWorkerAnimation.mech = oneLoadWorkerAnimation.mech;
					loadWorkerAnimation.allWorkersToLoad.Add(oneLoadWorkerAnimation.worker);
				}
			}
			loadWorkerAnimation.actionType = enemyActionInfo.actionType;
			loadWorkerAnimation.actionOwner = enemyActionInfo.actionOwner;
			this.enemyActionAnimations.Enqueue(loadWorkerAnimation);
			this.SetLastAnimation(loadWorkerAnimation);
		}
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x0006ACDC File Offset: 0x00068EDC
	public void OnEnemyProduceActionAnim(ProduceEnemyActionInfo enemyActionInfo)
	{
		this.payGainHexes.Add(enemyActionInfo.hex);
		if (enemyActionInfo.payPower && !this.firstProduceReceived)
		{
			if (OptionsManager.IsCameraAnimationsActive())
			{
				CameraToBaseAnimation cameraToBaseAnimation = new CameraToBaseAnimation();
				cameraToBaseAnimation.actionOwner = enemyActionInfo.actionOwner;
				cameraToBaseAnimation.actionType = LogInfoType.Produce;
				cameraToBaseAnimation.animationType = EnemyActionAnimationType.CameraToBaseAnimation;
				cameraToBaseAnimation.faction = enemyActionInfo.actionOwner;
				this.enemyActionAnimations.Enqueue(cameraToBaseAnimation);
				this.SetLastAnimation(cameraToBaseAnimation);
			}
			PayTopStatsAnimation payTopStatsAnimation = new PayTopStatsAnimation();
			payTopStatsAnimation.actionOwner = enemyActionInfo.actionOwner;
			payTopStatsAnimation.actionType = LogInfoType.Produce;
			payTopStatsAnimation.animationType = EnemyActionAnimationType.PayTopStatsAnimation;
			payTopStatsAnimation.gainType = GainType.Power;
			payTopStatsAnimation.resourceStartPosition = this.GetBaseResourcePosition(enemyActionInfo.actionOwner, this.coinMinHeight);
			if (enemyActionInfo.payPopularity)
			{
				payTopStatsAnimation.delayAfterAnimation = this.payAnimationTimeOffset;
			}
			this.enemyActionAnimations.Enqueue(payTopStatsAnimation);
			this.SetLastAnimation(payTopStatsAnimation);
			if (enemyActionInfo.payPopularity)
			{
				PayTopStatsAnimation payTopStatsAnimation2 = new PayTopStatsAnimation();
				payTopStatsAnimation2.actionOwner = enemyActionInfo.actionOwner;
				payTopStatsAnimation2.actionType = LogInfoType.Produce;
				payTopStatsAnimation2.animationType = EnemyActionAnimationType.PayTopStatsAnimation;
				payTopStatsAnimation.gainType = GainType.Popularity;
				payTopStatsAnimation.resourceStartPosition = this.GetBaseResourcePosition(enemyActionInfo.actionOwner, this.coinMinHeight);
				if (enemyActionInfo.payCoin)
				{
					payTopStatsAnimation.delayAfterAnimation = this.payAnimationTimeOffset;
				}
				this.enemyActionAnimations.Enqueue(payTopStatsAnimation);
				this.SetLastAnimation(payTopStatsAnimation);
				if (enemyActionInfo.payPopularity)
				{
					PayTopStatsAnimation payTopStatsAnimation3 = new PayTopStatsAnimation();
					payTopStatsAnimation3.actionOwner = enemyActionInfo.actionOwner;
					payTopStatsAnimation3.actionType = LogInfoType.Produce;
					payTopStatsAnimation3.animationType = EnemyActionAnimationType.PayTopStatsAnimation;
					payTopStatsAnimation.gainType = GainType.Coin;
					payTopStatsAnimation.resourceStartPosition = this.GetBaseResourcePosition(enemyActionInfo.actionOwner, this.coinMinHeight);
					payTopStatsAnimation.delayAfterAnimation = this.payAnimationTimeOffset;
					this.enemyActionAnimations.Enqueue(payTopStatsAnimation);
					this.SetLastAnimation(payTopStatsAnimation);
				}
			}
		}
		this.firstProduceReceived = true;
		if (OptionsManager.IsCameraAnimationsActive())
		{
			CameraToHexAnimation cameraToHexAnimation = new CameraToHexAnimation();
			cameraToHexAnimation.actionOwner = enemyActionInfo.actionOwner;
			cameraToHexAnimation.actionType = LogInfoType.Produce;
			cameraToHexAnimation.animationType = EnemyActionAnimationType.CameraToHexAnimation;
			cameraToHexAnimation.hex = this.GameController.GetGameHexPresenter(enemyActionInfo.hex).GetWorldPosition();
			this.enemyActionAnimations.Enqueue(cameraToHexAnimation);
			this.SetLastAnimation(cameraToHexAnimation);
		}
		ProduceResourceAnimation produceResourceAnimation = new ProduceResourceAnimation();
		produceResourceAnimation.actionOwner = enemyActionInfo.actionOwner;
		produceResourceAnimation.actionType = LogInfoType.Produce;
		produceResourceAnimation.animationType = EnemyActionAnimationType.ProduceResourceAnimation;
		produceResourceAnimation.hex = enemyActionInfo.hex;
		produceResourceAnimation.amountOfResources = enemyActionInfo.amount;
		produceResourceAnimation.resourceType = enemyActionInfo.resourceType;
		this.enemyActionAnimations.Enqueue(produceResourceAnimation);
		this.SetLastAnimation(produceResourceAnimation);
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x0006AF4C File Offset: 0x0006914C
	public void OnEnemyProduceWorkerSave(GainWorkerEnemyActionInfo enemyActionInfo)
	{
		foreach (Worker worker in enemyActionInfo.workers)
		{
			this.workersToAnimation.Enqueue(worker);
		}
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x0006AFA4 File Offset: 0x000691A4
	public void OnEnemyProduceWorkerActionAnim(GainWorkersEndEnemyActionInfo enemyActionInfo)
	{
		if (OptionsManager.IsCameraAnimationsActive())
		{
			CameraToHexAnimation cameraToHexAnimation = new CameraToHexAnimation();
			cameraToHexAnimation.actionOwner = enemyActionInfo.actionOwner;
			if (!enemyActionInfo.fromEncounter)
			{
				cameraToHexAnimation.actionType = LogInfoType.Produce;
			}
			cameraToHexAnimation.animationType = EnemyActionAnimationType.CameraToHexAnimation;
			cameraToHexAnimation.hex = this.GameController.GetGameHexPresenter(enemyActionInfo.deployPosition).GetWorldPosition();
			this.enemyActionAnimations.Enqueue(cameraToHexAnimation);
			this.SetLastAnimation(cameraToHexAnimation);
		}
		ProduceWorkerAnimation produceWorkerAnimation = new ProduceWorkerAnimation();
		produceWorkerAnimation.actionOwner = enemyActionInfo.actionOwner;
		produceWorkerAnimation.hex = enemyActionInfo.deployPosition;
		if (!enemyActionInfo.fromEncounter)
		{
			produceWorkerAnimation.actionType = LogInfoType.Produce;
		}
		produceWorkerAnimation.animationType = EnemyActionAnimationType.ProduceWorkerAnimation;
		int count = this.workersToAnimation.Count;
		for (int i = 0; i < count; i++)
		{
			Worker worker = this.workersToAnimation.Dequeue();
			produceWorkerAnimation.workersToAnimation.Add(worker);
		}
		this.workersToAnimation.Clear();
		this.enemyActionAnimations.Enqueue(produceWorkerAnimation);
		this.SetLastAnimation(produceWorkerAnimation);
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x0006B098 File Offset: 0x00069298
	public void OnEnemyGainTopStatsAnim(GainTopStatsEnemyActionInfo enemyActionInfo)
	{
		if (OptionsManager.IsCameraAnimationsActive())
		{
			CameraToBaseAnimation cameraToBaseAnimation = new CameraToBaseAnimation();
			cameraToBaseAnimation.actionOwner = enemyActionInfo.actionOwner;
			cameraToBaseAnimation.actionType = enemyActionInfo.actionType;
			cameraToBaseAnimation.animationType = EnemyActionAnimationType.CameraToBaseAnimation;
			cameraToBaseAnimation.faction = enemyActionInfo.actionOwner;
			this.enemyActionAnimations.Enqueue(cameraToBaseAnimation);
			this.SetLastAnimation(cameraToBaseAnimation);
		}
		for (int i = 0; i < enemyActionInfo.resourcesToGainAmount; i++)
		{
			GainTopStatsAnimation gainTopStatsAnimation = new GainTopStatsAnimation();
			gainTopStatsAnimation.actionOwner = enemyActionInfo.actionOwner;
			gainTopStatsAnimation.actionType = enemyActionInfo.actionType;
			gainTopStatsAnimation.animationType = EnemyActionAnimationType.GainTopStatsAnimation;
			gainTopStatsAnimation.gainType = enemyActionInfo.gainType;
			gainTopStatsAnimation.resourceStartPosition = this.GetBaseResourcePosition(enemyActionInfo.actionOwner, this.coinMaxHeight);
			this.enemyActionAnimations.Enqueue(gainTopStatsAnimation);
			this.SetLastAnimation(gainTopStatsAnimation);
		}
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x0006B160 File Offset: 0x00069360
	public void OnBotEndTurn(EndTurnActionInfo enemyActionInfo)
	{
		EndTurnAnimation endTurnAnimation = new EndTurnAnimation();
		endTurnAnimation.actionOwner = enemyActionInfo.actionOwner;
		endTurnAnimation.animationType = EnemyActionAnimationType.EndTurnAnimation;
		endTurnAnimation.owner = enemyActionInfo.owner;
		this.enemyActionAnimations.Enqueue(endTurnAnimation);
		this.SetLastAnimation(endTurnAnimation);
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x0006B1A8 File Offset: 0x000693A8
	public void CamToBaseOnTurnStart(Faction faction)
	{
		if (OptionsManager.IsCameraAnimationsActive())
		{
			HashSet<GameHex> hashSet = new HashSet<GameHex>();
			hashSet = GameController.GameManager.GetPlayerByFaction(faction).OwnedFields(false);
			CameraToAllHexesAnimation cameraToAllHexesAnimation = new CameraToAllHexesAnimation();
			cameraToAllHexesAnimation.actionOwner = faction;
			cameraToAllHexesAnimation.animationType = EnemyActionAnimationType.CameraToAllHexes;
			List<Vector3> list = new List<Vector3>();
			foreach (GameHex gameHex in hashSet)
			{
				list.Add(this.GameController.GetGameHexPresenter(gameHex).GetWorldPosition());
			}
			cameraToAllHexesAnimation.cameraPosition = AnimateCamera.Instance.CalculateCenterOfHexes(list);
			cameraToAllHexesAnimation.cameraZoom = AnimateCamera.Instance.CalculateZoomToShowGivenHexes(hashSet);
			this.enemyActionAnimations.Enqueue(cameraToAllHexesAnimation);
		}
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x0006B278 File Offset: 0x00069478
	public void CamToHexOnTurnStart(Vector2 hex, Faction faction)
	{
		CameraToHexAnimation cameraToHexAnimation = new CameraToHexAnimation();
		cameraToHexAnimation.animationType = EnemyActionAnimationType.CameraToHexAnimation;
		cameraToHexAnimation.actionOwner = faction;
		cameraToHexAnimation.actionType = LogInfoType.TurnStarts;
		cameraToHexAnimation.hex = hex;
		this.enemyActionAnimations.Enqueue(cameraToHexAnimation);
		this.SetLastAnimation(cameraToHexAnimation);
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x0006B2BC File Offset: 0x000694BC
	private string GetActionNameText(LogInfoType actionType)
	{
		switch (actionType)
		{
		case LogInfoType.GainCoin:
			return ScriptLocalization.Get("PlayerMat/GainCoin");
		case LogInfoType.GainPower:
			return ScriptLocalization.Get("PlayerMat/GainPower");
		case LogInfoType.GainCombatCard:
			return ScriptLocalization.Get("Tooltip/AmmoBasic");
		case LogInfoType.RecruitBonus:
			return ScriptLocalization.Get("PlayerMat/RecruitBonus");
		case LogInfoType.Move:
			return ScriptLocalization.Get("PlayerMat/Move");
		case LogInfoType.MoveCoins:
			return ScriptLocalization.Get("PlayerMat/MoveCoins");
		case LogInfoType.TradeResources:
			return ScriptLocalization.Get("PlayerMat/TradeResources");
		case LogInfoType.TradePopularity:
			return ScriptLocalization.Get("PlayerMat/TradePopularity");
		case LogInfoType.BolsterPower:
			return ScriptLocalization.Get("PlayerMat/BolsterPower");
		case LogInfoType.BolsterCombatCard:
			return ScriptLocalization.Get("PlayerMat/BolsterAmmo");
		case LogInfoType.Produce:
			return ScriptLocalization.Get("PlayerMat/Produce");
		case LogInfoType.Upgrade:
			return ScriptLocalization.Get("PlayerMat/Upgrade");
		case LogInfoType.Deploy:
			return ScriptLocalization.Get("PlayerMat/Deploy");
		case LogInfoType.Build:
			return ScriptLocalization.Get("PlayerMat/Build");
		case LogInfoType.Enlist:
			return ScriptLocalization.Get("PlayerMat/Enlist");
		case LogInfoType.RetreatMove:
			return ScriptLocalization.Get("PlayerMat/Retreat");
		case LogInfoType.FactoryTopAction:
			return ScriptLocalization.Get("PlayerMat/FactoryAction");
		case LogInfoType.GainWorker:
			return ScriptLocalization.Get("PlayerMat/GainWorker");
		case LogInfoType.Bolster:
			return ScriptLocalization.Get("PlayerMat/Bolster");
		case LogInfoType.Trade:
			return ScriptLocalization.Get("PlayerMat/Trade");
		case LogInfoType.Encounter:
			return ScriptLocalization.Get("Encounters/Encounter");
		}
		return actionType.ToString();
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x0006B448 File Offset: 0x00069648
	private void OnTurnEnd()
	{
		if (PlatformManager.IsMobile && ((!GameController.GameManager.IsMultiplayer && GameController.GameManager.PlayerCurrent.IsHuman) || (GameController.GameManager.IsMultiplayer && GameController.GameManager.IsMyTurn())))
		{
			this.SetupNormalSpeed();
		}
		if (PlatformManager.IsMobile)
		{
			this.UpdateFastForwardButton();
		}
		this.CleanSkippedHexesUpdateBool();
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x0006B4AC File Offset: 0x000696AC
	private void UpdateFastForwardButton()
	{
		if ((!GameController.GameManager.IsMultiplayer && !GameController.GameManager.PlayerCurrent.IsHuman) || (GameController.GameManager.IsMultiplayer && GameController.GameManager.PlayerCurrent != GameController.GameManager.PlayerOwner))
		{
			this.fastForwardButton.gameObject.SetActive(true);
			return;
		}
		this.fastForwardButton.gameObject.SetActive(false);
	}

	// Token: 0x06000583 RID: 1411 RVA: 0x0006B51C File Offset: 0x0006971C
	private void CleanSkippedHexesUpdateBool()
	{
		foreach (GameHex gameHex in this.payGainHexes)
		{
			gameHex.skipDownActionPresentationUpdate = false;
			gameHex.skipTopActionPresentationUpdate = false;
		}
		this.payGainHexes.Clear();
	}

	// Token: 0x06000584 RID: 1412 RVA: 0x0002B5E2 File Offset: 0x000297E2
	private void ResetMergeActionsFlags()
	{
		if (GameController.GameManager.IsMultiplayer)
		{
			this.firstProduceReceived = false;
			CameraControler.CameraMovementBlocked = false;
		}
	}

	// Token: 0x06000585 RID: 1413 RVA: 0x0002B5FD File Offset: 0x000297FD
	private TopActionType GetTopActionType(GainType type)
	{
		switch (type)
		{
		case GainType.Coin:
		case GainType.Move:
			return TopActionType.MoveGain;
		case GainType.Popularity:
		case GainType.AnyResource:
			return TopActionType.Trade;
		case GainType.Power:
			return TopActionType.Bolster;
		case GainType.CombatCard:
			return TopActionType.Bolster;
		case GainType.Produce:
			return TopActionType.Produce;
		}
		return TopActionType.Factory;
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x0002B632 File Offset: 0x00029832
	private DownActionType GetDownActionType(ResourceType type)
	{
		switch (type)
		{
		case ResourceType.oil:
			return DownActionType.Upgrade;
		case ResourceType.metal:
			return DownActionType.Deploy;
		case ResourceType.food:
			return DownActionType.Enlist;
		case ResourceType.wood:
			return DownActionType.Build;
		default:
			return DownActionType.Factory;
		}
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x0006B580 File Offset: 0x00069780
	private Vector3 GetBonusMarkerPosition(Faction faction)
	{
		switch (faction)
		{
		case Faction.Polania:
			return this.recruitBonusAnimationInfo.bonusMarkerPositions[2].topPartOfBase;
		case Faction.Albion:
			return this.recruitBonusAnimationInfo.bonusMarkerPositions[3].topPartOfBase;
		case Faction.Nordic:
			return this.recruitBonusAnimationInfo.bonusMarkerPositions[4].topPartOfBase;
		case Faction.Rusviet:
			return this.recruitBonusAnimationInfo.bonusMarkerPositions[5].topPartOfBase;
		case Faction.Togawa:
			return this.recruitBonusAnimationInfo.bonusMarkerPositions[6].topPartOfBase;
		case Faction.Crimea:
			return this.recruitBonusAnimationInfo.bonusMarkerPositions[0].topPartOfBase;
		case Faction.Saxony:
			return this.recruitBonusAnimationInfo.bonusMarkerPositions[1].topPartOfBase;
		default:
			return this.recruitBonusAnimationInfo.bonusMarkerPositions[0].topPartOfBase;
		}
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x0006B66C File Offset: 0x0006986C
	private GameObject GetStatMarkerModel(GainType gainType)
	{
		switch (gainType)
		{
		case GainType.Coin:
			return ResourcesObjectPoolScript.Instance.GetPooledObject(4);
		case GainType.Popularity:
			return ResourcesObjectPoolScript.Instance.GetPooledObject(5);
		case GainType.Power:
			return ResourcesObjectPoolScript.Instance.GetPooledObject(6);
		case GainType.CombatCard:
			return ResourcesObjectPoolScript.Instance.GetPooledObject(7);
		default:
			if (gainType != GainType.Recruit)
			{
				return null;
			}
			return ResourcesObjectPoolScript.Instance.GetPooledObject(8);
		}
	}

	// Token: 0x06000589 RID: 1417 RVA: 0x0002B655 File Offset: 0x00029855
	public bool AllAnimationsFinished()
	{
		return !this.animationInProgress && this.enemyActionAnimations.Count == 0;
	}

	// Token: 0x04000445 RID: 1093
	[SerializeField]
	private Vector3 rotationAdjustment = Vector3.zero;

	// Token: 0x04000446 RID: 1094
	private Vector3 bonusAnimationTargetAdjustment = Vector3.zero;

	// Token: 0x04000447 RID: 1095
	public MatPlayerPresenter matPlayerPresenter;

	// Token: 0x04000448 RID: 1096
	public static ShowEnemyMoves Instance;

	// Token: 0x04000449 RID: 1097
	public bool animationInProgress;

	// Token: 0x0400044A RID: 1098
	public float zoomBaseHeight = 0.23f;

	// Token: 0x0400044B RID: 1099
	public float zoomBaseTopFactionsHeight = 0.38f;

	// Token: 0x0400044C RID: 1100
	public bool fastForwardAnimationsActive;

	// Token: 0x0400044D RID: 1101
	[SerializeField]
	private float coinMaxHeight = 2.5f;

	// Token: 0x0400044E RID: 1102
	[SerializeField]
	private float coinMinHeight = -0.9f;

	// Token: 0x0400044F RID: 1103
	[SerializeField]
	private float cameraAnimationTime = 1f;

	// Token: 0x04000450 RID: 1104
	[SerializeField]
	private float gainAnimationTime = 1f;

	// Token: 0x04000451 RID: 1105
	[SerializeField]
	private float payAnimationTime = 1f;

	// Token: 0x04000452 RID: 1106
	[SerializeField]
	private float payEndPosition = 1f;

	// Token: 0x04000453 RID: 1107
	[SerializeField]
	private float payAnimationTimeOffset = 1f;

	// Token: 0x04000454 RID: 1108
	[SerializeField]
	public ShowEnemyMoves.BasePositions[] basePositions;

	// Token: 0x04000455 RID: 1109
	[SerializeField]
	private GameObject[] enemyPlayerMats;

	// Token: 0x04000456 RID: 1110
	[SerializeField]
	private GameObject enemyMat;

	// Token: 0x04000457 RID: 1111
	[SerializeField]
	private GameObject enlistFlashAnimation;

	// Token: 0x04000458 RID: 1112
	[SerializeField]
	private GameObject upgradeGreenFlashAnimation;

	// Token: 0x04000459 RID: 1113
	[SerializeField]
	private GameObject upgradeRedFlashAnimation;

	// Token: 0x0400045A RID: 1114
	[HideInInspector]
	public List<GameHex> payGainHexes = new List<GameHex>();

	// Token: 0x0400045B RID: 1115
	[SerializeField]
	private RecruitBonusAnimationInfo recruitBonusAnimationInfo;

	// Token: 0x0400045C RID: 1116
	[SerializeField]
	private float recruitModelYOffset = 1f;

	// Token: 0x0400045D RID: 1117
	[SerializeField]
	private float normalGainTopStatsDelay = 0.8f;

	// Token: 0x0400045E RID: 1118
	[SerializeField]
	private float bonusGainTopStatsDelay = 0.8f;

	// Token: 0x0400045F RID: 1119
	[SerializeField]
	private float fastForwardSpeed = 3f;

	// Token: 0x04000460 RID: 1120
	[SerializeField]
	private Button fastForwardButton;

	// Token: 0x04000461 RID: 1121
	[SerializeField]
	private Sprite speedDownImage;

	// Token: 0x04000462 RID: 1122
	[SerializeField]
	private Sprite speedUpImage;

	// Token: 0x04000463 RID: 1123
	private CameraMovementEffects cameraMovementEffects;

	// Token: 0x04000464 RID: 1124
	private GameController GameController;

	// Token: 0x04000465 RID: 1125
	private Vector3 actualFactionBasePosition;

	// Token: 0x04000466 RID: 1126
	private bool firstProduceReceived;

	// Token: 0x04000467 RID: 1127
	private Transform topAnimationPosition;

	// Token: 0x04000468 RID: 1128
	private Transform downAnimationPosition;

	// Token: 0x04000469 RID: 1129
	private Transform enlistAnimationPosition;

	// Token: 0x0400046A RID: 1130
	private Queue<EnemyActionAnimation> enemyActionAnimations = new Queue<EnemyActionAnimation>();

	// Token: 0x0400046B RID: 1131
	private Queue<Worker> workersToAnimation = new Queue<Worker>();

	// Token: 0x0400046C RID: 1132
	private Faction actualFaction;

	// Token: 0x0400046D RID: 1133
	private PostProcessingProfile postProcessingProfile;

	// Token: 0x0400046E RID: 1134
	private float vinietteIntensityDefault = 0.38f;

	// Token: 0x0400046F RID: 1135
	private List<EnemyActionAnimation> bonusAnimationList = new List<EnemyActionAnimation>();

	// Token: 0x04000470 RID: 1136
	private Queue<OneLoadWorkerAnimation> loadToMechAnimations = new Queue<OneLoadWorkerAnimation>();

	// Token: 0x04000471 RID: 1137
	private EnemyActionAnimation lastAnimation;

	// Token: 0x04000472 RID: 1138
	private int workersToLoad;

	// Token: 0x04000473 RID: 1139
	private int workersToLoadReported;

	// Token: 0x04000474 RID: 1140
	private int workersToUnload;

	// Token: 0x04000475 RID: 1141
	private int workersToUnloadReported;

	// Token: 0x04000476 RID: 1142
	private int unitsToChangePositions;

	// Token: 0x04000477 RID: 1143
	private int unitsToChangePositionsReported;

	// Token: 0x04000478 RID: 1144
	private int animationFailsafeQueueSize;

	// Token: 0x04000479 RID: 1145
	private float animationFailsafeTimestamp;

	// Token: 0x020000A1 RID: 161
	[Serializable]
	public struct BasePositions
	{
		// Token: 0x0400047A RID: 1146
		public Faction faction;

		// Token: 0x0400047B RID: 1147
		public Vector2 baseHex;

		// Token: 0x0400047C RID: 1148
		public Vector2 resourceImagePosition;
	}
}
