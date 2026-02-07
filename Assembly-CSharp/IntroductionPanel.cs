using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.Multiplayer;
using Scythe.Multiplayer.Data;
using Scythe.UI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000D7 RID: 215
public class IntroductionPanel : MonoBehaviour
{
	// Token: 0x0600065F RID: 1631 RVA: 0x0002C12A File Offset: 0x0002A32A
	public void ActivateIntroductionPanel()
	{
		if (this.actionLog != null)
		{
			this.actionLog.SetActive(false);
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x06000660 RID: 1632 RVA: 0x0002C152 File Offset: 0x0002A352
	public void DisactivateIntroductionPanel()
	{
		if (this.actionLog != null)
		{
			this.actionLog.SetActive(true);
		}
		base.GetComponent<Animator>().Play("GameIntro");
		base.StartCoroutine(this.DisableMyself());
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x0002C18B File Offset: 0x0002A38B
	public IEnumerator DisableMyself()
	{
		yield return new WaitForSeconds(1f);
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x0006FB00 File Offset: 0x0006DD00
	public void ShowInfo(Faction faction, float duration)
	{
		PlayerMatType matType = GameController.GameManager.GetPlayerByFaction(faction).matPlayer.matType;
		this.PlayerName.text = this.GetPlayerName(faction);
		this.FactionName.text = ScriptLocalization.Get("FactionMat/" + faction.ToString());
		if (matType < PlayerMatType.Campaign00)
		{
			this.playerMatName.text = ScriptLocalization.Get("PlayerMat/" + matType.ToString());
		}
		else
		{
			this.playerMatName.text = ScriptLocalization.Get("MainMenu/Tutorial");
		}
		this.FactionInfo.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000f, 0f);
		this.FactionInfo.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-1000f, 0f), duration * 0.95f, false).SetEase(this.animationCurve);
		this.PlayerMatInfo.GetComponent<RectTransform>().anchoredPosition = new Vector2(-2000f, 0f);
		this.PlayerMatInfo.GetComponent<RectTransform>().DOAnchorPos(new Vector2(1000f, 0f), duration * 0.95f, false).SetEase(this.animationCurve);
	}

	// Token: 0x06000663 RID: 1635 RVA: 0x0006FC44 File Offset: 0x0006DE44
	private string GetPlayerName(Faction faction)
	{
		if (GameController.GameManager.IsMultiplayer)
		{
			return MultiplayerController.Instance.GetPlayersInGame().ToList<PlayerData>().Find((PlayerData player) => player.Faction == (int)faction)
				.Name;
		}
		return GameController.GameManager.GetPlayerByFaction(faction).Name;
	}

	// Token: 0x04000570 RID: 1392
	[SerializeField]
	private GameObject FactionInfo;

	// Token: 0x04000571 RID: 1393
	[SerializeField]
	private Text PlayerName;

	// Token: 0x04000572 RID: 1394
	[SerializeField]
	private Text FactionName;

	// Token: 0x04000573 RID: 1395
	[SerializeField]
	private GameObject PlayerMatInfo;

	// Token: 0x04000574 RID: 1396
	[SerializeField]
	private Text playerMatName;

	// Token: 0x04000575 RID: 1397
	[SerializeField]
	private GameObject actionLog;

	// Token: 0x04000576 RID: 1398
	[SerializeField]
	private AnimationCurve animationCurve;
}
