using System;
using Scythe.Multiplayer;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200009B RID: 155
public class CombatPanelSummary : MonoBehaviour
{
	// Token: 0x14000031 RID: 49
	// (add) Token: 0x0600050A RID: 1290 RVA: 0x00066148 File Offset: 0x00064348
	// (remove) Token: 0x0600050B RID: 1291 RVA: 0x00066180 File Offset: 0x00064380
	public event Action FightButtonClicked;

	// Token: 0x14000032 RID: 50
	// (add) Token: 0x0600050C RID: 1292 RVA: 0x000661B8 File Offset: 0x000643B8
	// (remove) Token: 0x0600050D RID: 1293 RVA: 0x000661F0 File Offset: 0x000643F0
	public event Action OkButtonClicked;

	// Token: 0x0600050E RID: 1294 RVA: 0x00066228 File Offset: 0x00064428
	public void SetUpForPreparation()
	{
		this._fightButton.gameObject.SetActive(true);
		this._fightButton.onClick.RemoveAllListeners();
		this._fightButton.onClick.AddListener(new UnityAction(this.OnFightButtonClicked));
		this._okButton.gameObject.SetActive(false);
	}

	// Token: 0x0600050F RID: 1295 RVA: 0x00066284 File Offset: 0x00064484
	public void SetUpForResult()
	{
		this._fightButton.gameObject.SetActive(false);
		this._okButton.onClick.RemoveAllListeners();
		this._okButton.onClick.AddListener(new UnityAction(this.OnOkButtonClicked));
		this._okButton.gameObject.SetActive(true);
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x0002B23B File Offset: 0x0002943B
	public void UpdatePlayerPower(int power)
	{
		this._playerTotalPowerText.text = power.ToString();
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x0002B24F File Offset: 0x0002944F
	public void UpdateOpponentPower(int power)
	{
		this._opponentTotalPowerText.text = power.ToString();
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x0002B263 File Offset: 0x00029463
	public void SetOpponentMaxPossiblePower(int maxPower)
	{
		this._opponentTotalPowerText.text = string.Format("?\n<size=11>0-{0}</size>", maxPower);
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x0002B280 File Offset: 0x00029480
	private void OnFightButtonClicked()
	{
		if (this.FightButtonClicked != null)
		{
			this.FightButtonClicked();
			if (MultiplayerController.Instance.IsMultiplayer)
			{
				this._fightButton.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x0002B2B2 File Offset: 0x000294B2
	private void OnOkButtonClicked()
	{
		if (this.OkButtonClicked != null)
		{
			this.OkButtonClicked();
		}
	}

	// Token: 0x04000425 RID: 1061
	[SerializeField]
	private TMP_Text _playerTotalPowerText;

	// Token: 0x04000426 RID: 1062
	[SerializeField]
	private TMP_Text _opponentTotalPowerText;

	// Token: 0x04000427 RID: 1063
	[SerializeField]
	private Button _fightButton;

	// Token: 0x04000428 RID: 1064
	[SerializeField]
	private Button _okButton;
}
