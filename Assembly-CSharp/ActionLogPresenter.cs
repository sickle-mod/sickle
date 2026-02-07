using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Scythe.GameLogic;
using Scythe.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020000D0 RID: 208
public class ActionLogPresenter : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler, IScrollHandler, IPointerEnterHandler, IPointerExitHandler
{
	// Token: 0x0600060B RID: 1547 RVA: 0x0002BBA0 File Offset: 0x00029DA0
	private void Awake()
	{
		ActionLogPresenter.Instance = this;
	}

	// Token: 0x0600060C RID: 1548 RVA: 0x0002BBA8 File Offset: 0x00029DA8
	private void Start()
	{
		ActionLog.LogInfoCreated += this.AddLogToQueue;
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x0002BBBB File Offset: 0x00029DBB
	private void OnDestroy()
	{
		ActionLog.LogInfoCreated -= this.AddLogToQueue;
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x0002BBCE File Offset: 0x00029DCE
	protected void SetActive(bool active)
	{
		base.gameObject.SetActive(active);
		base.StartCoroutine(this.MoveLogToTheBottom());
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x0002BBE9 File Offset: 0x00029DE9
	public void Show()
	{
		this.SetActive(true);
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x0002BBF2 File Offset: 0x00029DF2
	public void Hide()
	{
		this.SetActive(false);
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x0006ECE0 File Offset: 0x0006CEE0
	private void Update()
	{
		while (this.logsToCreate.Count > 0)
		{
			KeyValuePair<LogInfo, int> keyValuePair = this.logsToCreate.Dequeue();
			this.CreateNewLog(keyValuePair.Key, keyValuePair.Value);
		}
		if (this.scrollLog)
		{
			this.ScrollLog();
		}
		if (this.hideLogIndicator)
		{
			this.HideLogIndicator();
		}
	}

	// Token: 0x06000612 RID: 1554 RVA: 0x0002BBFB File Offset: 0x00029DFB
	private void OnApplicationFocus(bool hasFocus)
	{
		if (!hasFocus && this.lastClickedLogInfoID != -1)
		{
			this.LogReleased(this.lastClickedLogInfoID, this.lastClickedLogInfo);
		}
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x0002BC1B File Offset: 0x00029E1B
	public void Clear()
	{
		while (this.GetLogContent().childCount != 0)
		{
			global::UnityEngine.Object.DestroyImmediate(this.GetLogContent().GetChild(0).gameObject);
		}
	}

	// Token: 0x06000614 RID: 1556 RVA: 0x0006ED3C File Offset: 0x0006CF3C
	public void Trim(int sizeFinal)
	{
		while (this.GetLogContent().childCount > sizeFinal)
		{
			global::UnityEngine.Object.DestroyImmediate(this.GetLogContent().GetChild(this.GetLogContent().childCount - 1).gameObject);
			GameController.GameManager.actionLog.RemoveLastLog();
		}
		base.StartCoroutine(this.MoveLogToTheBottom());
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x0002BC42 File Offset: 0x00029E42
	public Transform GetLogContent()
	{
		return this.logContent;
	}

	// Token: 0x06000616 RID: 1558 RVA: 0x0002BC4A File Offset: 0x00029E4A
	public int LogCount()
	{
		return this.logContent.childCount;
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x0002BC57 File Offset: 0x00029E57
	public void LogInfoReported(LogInfo logInfo)
	{
		GameController.GameManager.actionLog.LogInfoReported(logInfo);
	}

	// Token: 0x06000618 RID: 1560 RVA: 0x0002BC69 File Offset: 0x00029E69
	public void AddLogToQueue(LogInfo logInfo, int id)
	{
		this.logsToCreate.Enqueue(new KeyValuePair<LogInfo, int>(logInfo, id));
	}

	// Token: 0x06000619 RID: 1561 RVA: 0x0006ED98 File Offset: 0x0006CF98
	public void CreateNewLog(LogInfo logInfo, int id)
	{
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.singleLogPrefab);
		gameObject.transform.SetParent(this.GetLogContent(), false);
		this.FactionOutlineSetup(logInfo.PlayerAssigned, gameObject.GetComponent<Image>());
		this.AttachImageToLog(logInfo, gameObject.transform.GetChild(0).GetComponent<Image>());
		this.AddTriggerEventsToLog(gameObject, id);
		if (!this.userActivityDetected)
		{
			base.StartCoroutine(this.MoveLogToTheBottom());
			return;
		}
		if (this.CanShowLogIndicator())
		{
			this.ShowLogIndicator();
			this.newMessage = true;
		}
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x0002BC7D File Offset: 0x00029E7D
	public void FactionOutlineSetup(Faction faction, Image logOutline)
	{
		logOutline.sprite = this.logBackgrounds[(int)faction];
	}

	// Token: 0x0600061B RID: 1563 RVA: 0x0006EE20 File Offset: 0x0006D020
	public void AttachImageToLog(LogInfo logInfo, Image actionImage)
	{
		if (logInfo.IsEncounter)
		{
			actionImage.sprite = this.EncounterImage;
			return;
		}
		switch (logInfo.Type)
		{
		case LogInfoType.GainCoin:
			actionImage.sprite = this.GainCoinImage;
			return;
		case LogInfoType.GainPopularity:
			actionImage.sprite = this.GainPopularityImage;
			return;
		case LogInfoType.GainPower:
			actionImage.sprite = this.GainPowerImage;
			return;
		case LogInfoType.GainCombatCard:
			actionImage.sprite = this.GainCombatCardImage;
			return;
		case LogInfoType.Move:
			actionImage.sprite = this.MoveImage;
			return;
		case LogInfoType.MoveCoins:
			actionImage.sprite = this.MoveCoinsImage;
			return;
		case LogInfoType.TradeResources:
			actionImage.sprite = this.TradeResourcesImage;
			return;
		case LogInfoType.TradePopularity:
			actionImage.sprite = this.TradePopularityImage;
			return;
		case LogInfoType.BolsterPower:
			actionImage.sprite = this.BolsterPowerImage;
			return;
		case LogInfoType.BolsterCombatCard:
			actionImage.sprite = this.BolsterCombatCardImage;
			return;
		case LogInfoType.Produce:
			actionImage.sprite = this.ProduceImage;
			return;
		case LogInfoType.Upgrade:
			actionImage.sprite = this.UpgradeImage;
			return;
		case LogInfoType.Deploy:
			actionImage.sprite = this.DeployImage;
			return;
		case LogInfoType.Build:
			actionImage.sprite = this.BuildImage;
			return;
		case LogInfoType.Enlist:
			actionImage.sprite = this.EnlistImage;
			return;
		case LogInfoType.ForceWorkersToRetreat:
			actionImage.sprite = this.ForceWorkersToRetreatImage;
			return;
		case LogInfoType.Combat:
			actionImage.sprite = this.CombatImage;
			return;
		case LogInfoType.SpyPlayer:
			actionImage.sprite = this.SpyPlayerImage;
			return;
		case LogInfoType.FactoryCardGain:
			actionImage.sprite = this.FactoryImage;
			return;
		case LogInfoType.GainStar:
			actionImage.sprite = this.GainStarImage;
			return;
		case LogInfoType.FactoryTopAction:
			actionImage.sprite = this.FactoryTopActionImage;
			return;
		case LogInfoType.TokenAction:
		{
			TokenActionLogInfo tokenActionLogInfo = logInfo as TokenActionLogInfo;
			if (tokenActionLogInfo.token.Owner.matFaction.faction == Faction.Albion)
			{
				actionImage.sprite = this.FlagTokenImage;
				return;
			}
			if (tokenActionLogInfo.token.Owner.matFaction.faction == Faction.Togawa)
			{
				actionImage.sprite = this.TrapTokenImage;
				return;
			}
			return;
		}
		}
		actionImage.sprite = this.GainCoinImage;
	}

	// Token: 0x0600061C RID: 1564 RVA: 0x0006F058 File Offset: 0x0006D258
	private void AddTriggerEventsToLog(GameObject singleLog, int id)
	{
		EventTrigger component = singleLog.GetComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerEnter;
		entry.callback.AddListener(delegate(BaseEventData eventData)
		{
			this.LogClicked(id, singleLog);
		});
		component.triggers.Add(entry);
		EventTrigger.Entry entry2 = new EventTrigger.Entry();
		entry2.eventID = EventTriggerType.PointerExit;
		entry2.callback.AddListener(delegate(BaseEventData eventData)
		{
			this.LogReleased(id, singleLog);
		});
		component.triggers.Add(entry2);
		EventTrigger.Entry entry3 = new EventTrigger.Entry();
		entry3.eventID = EventTriggerType.Drag;
		entry3.callback.AddListener(delegate(BaseEventData eventData)
		{
			this.OnDrag(eventData as PointerEventData);
		});
		component.triggers.Add(entry3);
		EventTrigger.Entry entry4 = new EventTrigger.Entry();
		entry4.eventID = EventTriggerType.BeginDrag;
		entry4.callback.AddListener(delegate(BaseEventData eventData)
		{
			this.OnBeginDrag(eventData as PointerEventData);
		});
		component.triggers.Add(entry4);
		EventTrigger.Entry entry5 = new EventTrigger.Entry();
		entry5.eventID = EventTriggerType.EndDrag;
		entry5.callback.AddListener(delegate(BaseEventData eventData)
		{
			this.OnEndDrag(eventData as PointerEventData);
		});
		component.triggers.Add(entry5);
		EventTrigger.Entry entry6 = new EventTrigger.Entry();
		entry6.eventID = EventTriggerType.Scroll;
		entry6.callback.AddListener(delegate(BaseEventData eventData)
		{
			this.OnScroll(eventData as PointerEventData);
		});
		component.triggers.Add(entry6);
	}

	// Token: 0x0600061D RID: 1565 RVA: 0x0002BC91 File Offset: 0x00029E91
	public IEnumerator MoveLogToTheBottom()
	{
		yield return new WaitForEndOfFrame();
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.logContent);
		this.logFrameScrollView.verticalNormalizedPosition = 0f;
		this.HideLogIndicator();
		yield break;
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x0006F1B8 File Offset: 0x0006D3B8
	private bool CanShowLogIndicator()
	{
		return this.logFrameScrollView.verticalNormalizedPosition != 0f && !this.newMessage && this.logFrameScrollView.GetComponent<RectTransform>().rect.height < this.logContent.GetComponent<RectTransform>().rect.height;
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x0006F214 File Offset: 0x0006D414
	private void ShowLogIndicator()
	{
		this.newLogIndicator.SetActive(true);
		this.newLogIndicator.transform.GetChild(0).GetComponent<Image>().DOColor(Color.white, 3.5f)
			.SetLoops(-1)
			.SetEase(this.logIndicatiorAlphaCurve);
	}

	// Token: 0x06000620 RID: 1568 RVA: 0x0006F264 File Offset: 0x0006D464
	private void HideLogIndicator()
	{
		if (!this.newLogIndicator.activeSelf)
		{
			return;
		}
		this.newLogIndicator.transform.GetChild(0).GetComponent<Image>().DOKill(true);
		this.newLogIndicator.SetActive(false);
		Color white = Color.white;
		white.a = 0f;
		this.newLogIndicator.transform.GetChild(0).GetComponent<Image>().color = white;
		this.newMessage = false;
		this.userActivityDetected = false;
		base.StartCoroutine(this.MoveLogToTheBottom());
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x0002BCA0 File Offset: 0x00029EA0
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.userActivityDetected = true;
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x0002BCA9 File Offset: 0x00029EA9
	public void OnPointerExit(PointerEventData eventData)
	{
		this.userActivityDetected = false;
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x0002BCB2 File Offset: 0x00029EB2
	private void LogClicked(int id, GameObject singleLog)
	{
		this.logInterpreter.InterpretLog(GameController.GameManager.actionLog.GetLogInfo(id), singleLog);
		WorldSFXManager.PlaySound(SoundEnum.PlayersBoardShowEnemysboardRelease, AudioSourceType.Buttons);
		this.lastClickedLogInfoID = id;
		this.lastClickedLogInfo = singleLog;
		this.userActivityDetected = true;
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x0002BCED File Offset: 0x00029EED
	private void LogReleased(int id, GameObject singleLog)
	{
		this.logInterpreter.HideInterpretation(singleLog);
		this.lastClickedLogInfoID = -1;
		this.lastClickedLogInfo = null;
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x0006F2F4 File Offset: 0x0006D4F4
	private void ScrollRectHeightSanityCheck()
	{
		if (this.logFrameScrollView.verticalNormalizedPosition > 1f)
		{
			this.logFrameScrollView.verticalNormalizedPosition = 1f;
			return;
		}
		if (this.logFrameScrollView.verticalNormalizedPosition <= 0f)
		{
			this.logFrameScrollView.verticalNormalizedPosition = 0f;
			this.HideLogIndicator();
		}
	}

	// Token: 0x06000626 RID: 1574 RVA: 0x0006F34C File Offset: 0x0006D54C
	private void ScrollLog()
	{
		float height = this.singleLogPrefab.GetComponent<RectTransform>().rect.height;
		float height2 = this.logContent.GetComponent<RectTransform>().rect.height;
		float spacing = this.logContent.GetComponent<VerticalLayoutGroup>().spacing;
		float num = (height * 2f + spacing) / height2 * this.GetSpeed() * Time.deltaTime * (this.toNewer ? (-1f) : 1f);
		this.logFrameScrollView.verticalNormalizedPosition += num;
		this.ScrollRectHeightSanityCheck();
		this.scrollingTime += Time.deltaTime;
	}

	// Token: 0x06000627 RID: 1575 RVA: 0x0002BD09 File Offset: 0x00029F09
	public void StartScrollingLog(bool toNewer)
	{
		WorldSFXManager.PlaySound(SoundEnum.GuiChatButton, AudioSourceType.Buttons);
		this.toNewer = toNewer;
		this.scrollLog = true;
		this.userActivityDetected = true;
	}

	// Token: 0x06000628 RID: 1576 RVA: 0x0002BD28 File Offset: 0x00029F28
	public void StopScrollingLog()
	{
		this.scrollingTime = 0f;
		this.scrollLog = false;
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x0006F3F4 File Offset: 0x0006D5F4
	public float GetSpeed()
	{
		float num = 1f;
		float num2 = 5f;
		float num3 = 2f;
		float num4 = this.scrollingTime - (num - Mathf.Log(this.baseScrollingSpeed, num3));
		if (this.scrollingTime < num)
		{
			return this.baseScrollingSpeed;
		}
		if (this.scrollingTime >= num && this.scrollingTime < num2)
		{
			return Mathf.Pow(num3, num4);
		}
		return Mathf.Pow(num3, 4f);
	}

	// Token: 0x0600062A RID: 1578 RVA: 0x0002BD3C File Offset: 0x00029F3C
	public void OnBeginDrag(PointerEventData eventData)
	{
		((IBeginDragHandler)this.logFrameScrollView).OnBeginDrag(eventData);
		this.userActivityDetected = true;
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x0002BD51 File Offset: 0x00029F51
	public void OnDrag(PointerEventData eventData)
	{
		((IDragHandler)this.logFrameScrollView).OnDrag(eventData);
		if (this.logFrameScrollView.verticalNormalizedPosition <= 0f)
		{
			this.HideLogIndicator();
		}
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x0002BD77 File Offset: 0x00029F77
	public void OnEndDrag(PointerEventData eventData)
	{
		((IEndDragHandler)this.logFrameScrollView).OnEndDrag(eventData);
		if (this.logFrameScrollView.verticalNormalizedPosition <= 0f)
		{
			this.HideLogIndicator();
		}
	}

	// Token: 0x0600062D RID: 1581 RVA: 0x0002BD9D File Offset: 0x00029F9D
	public void OnScroll(PointerEventData eventData)
	{
		((IScrollHandler)this.logFrameScrollView).OnScroll(eventData);
		if (this.logFrameScrollView.verticalNormalizedPosition <= 0f)
		{
			this.HideLogIndicator();
		}
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x0006F460 File Offset: 0x0006D660
	public void AfterGameLoaded()
	{
		List<LogInfo> logHistory = GameController.GameManager.actionLog.GetLogHistory();
		for (int i = 0; i < logHistory.Count; i++)
		{
			this.CreateNewLog(logHistory[i], i);
		}
	}

	// Token: 0x0400052A RID: 1322
	public static ActionLogPresenter Instance;

	// Token: 0x0400052B RID: 1323
	public ActionLogInterpreter logInterpreter;

	// Token: 0x0400052C RID: 1324
	public RectTransform logContent;

	// Token: 0x0400052D RID: 1325
	public ScrollRect logFrameScrollView;

	// Token: 0x0400052E RID: 1326
	public GameObject newLogIndicator;

	// Token: 0x0400052F RID: 1327
	public List<Sprite> logBackgrounds;

	// Token: 0x04000530 RID: 1328
	[SerializeField]
	private GameObject singleLogPrefab;

	// Token: 0x04000531 RID: 1329
	[SerializeField]
	private AnimationCurve logIndicatiorAlphaCurve;

	// Token: 0x04000532 RID: 1330
	private Queue<KeyValuePair<LogInfo, int>> logsToCreate = new Queue<KeyValuePair<LogInfo, int>>();

	// Token: 0x04000533 RID: 1331
	[SerializeField]
	private Sprite GainCoinImage;

	// Token: 0x04000534 RID: 1332
	[SerializeField]
	private Sprite GainPowerImage;

	// Token: 0x04000535 RID: 1333
	[SerializeField]
	private Sprite GainPopularityImage;

	// Token: 0x04000536 RID: 1334
	[SerializeField]
	private Sprite GainCombatCardImage;

	// Token: 0x04000537 RID: 1335
	[SerializeField]
	private Sprite MoveImage;

	// Token: 0x04000538 RID: 1336
	[SerializeField]
	private Sprite MoveCoinsImage;

	// Token: 0x04000539 RID: 1337
	[SerializeField]
	private Sprite TradeResourcesImage;

	// Token: 0x0400053A RID: 1338
	[SerializeField]
	private Sprite TradePopularityImage;

	// Token: 0x0400053B RID: 1339
	[SerializeField]
	private Sprite BolsterPowerImage;

	// Token: 0x0400053C RID: 1340
	[SerializeField]
	private Sprite BolsterCombatCardImage;

	// Token: 0x0400053D RID: 1341
	[SerializeField]
	private Sprite ProduceImage;

	// Token: 0x0400053E RID: 1342
	[SerializeField]
	private Sprite UpgradeImage;

	// Token: 0x0400053F RID: 1343
	[SerializeField]
	private Sprite DeployImage;

	// Token: 0x04000540 RID: 1344
	[SerializeField]
	private Sprite BuildImage;

	// Token: 0x04000541 RID: 1345
	[SerializeField]
	private Sprite EnlistImage;

	// Token: 0x04000542 RID: 1346
	[SerializeField]
	private Sprite ForceWorkersToRetreatImage;

	// Token: 0x04000543 RID: 1347
	[SerializeField]
	private Sprite SpyPlayerImage;

	// Token: 0x04000544 RID: 1348
	[SerializeField]
	private Sprite FactoryImage;

	// Token: 0x04000545 RID: 1349
	[SerializeField]
	private Sprite GainStarImage;

	// Token: 0x04000546 RID: 1350
	[SerializeField]
	private Sprite CombatImage;

	// Token: 0x04000547 RID: 1351
	[SerializeField]
	private Sprite FactoryTopActionImage;

	// Token: 0x04000548 RID: 1352
	[SerializeField]
	private Sprite EncounterImage;

	// Token: 0x04000549 RID: 1353
	[SerializeField]
	private Sprite FlagTokenImage;

	// Token: 0x0400054A RID: 1354
	[SerializeField]
	private Sprite TrapTokenImage;

	// Token: 0x0400054B RID: 1355
	private bool scrollLog;

	// Token: 0x0400054C RID: 1356
	private float baseScrollingSpeed = 2f;

	// Token: 0x0400054D RID: 1357
	private bool toNewer = true;

	// Token: 0x0400054E RID: 1358
	private float scrollingTime;

	// Token: 0x0400054F RID: 1359
	private bool newMessage;

	// Token: 0x04000550 RID: 1360
	private bool userActivityDetected;

	// Token: 0x04000551 RID: 1361
	private bool hideLogIndicator;

	// Token: 0x04000552 RID: 1362
	private bool slideToBottom;

	// Token: 0x04000553 RID: 1363
	private int lastClickedLogInfoID = -1;

	// Token: 0x04000554 RID: 1364
	private GameObject lastClickedLogInfo;
}
