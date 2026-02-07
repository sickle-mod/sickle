using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000099 RID: 153
public class CombatPanelPowerDistribution : MonoBehaviour
{
	// Token: 0x14000030 RID: 48
	// (add) Token: 0x060004FE RID: 1278 RVA: 0x00065DA0 File Offset: 0x00063FA0
	// (remove) Token: 0x060004FF RID: 1279 RVA: 0x00065DD8 File Offset: 0x00063FD8
	public event Action<int> DistributedPowerChanged;

	// Token: 0x06000500 RID: 1280 RVA: 0x0002B1A8 File Offset: 0x000293A8
	private void Awake()
	{
		this._lessPowerButton.onClick.AddListener(new UnityAction(this.OnLessPowerButtonClicked));
		this._morePowerButton.onClick.AddListener(new UnityAction(this.OnMorePowerButtonClicked));
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x0002B1E2 File Offset: 0x000293E2
	public void Init(int playerPower)
	{
		this._playerPowerValue = playerPower;
		this._distributionText.text = string.Format("{0}/{1}", this._currentlyDistributedPower, Math.Min(this._playerPowerValue, this._maxDistributionValue));
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x00065E10 File Offset: 0x00064010
	public void Reset()
	{
		this._playerPowerValue = 0;
		this._currentlyDistributedPower = 0;
		this._distributionText.text = string.Format("{0}/{1}", this._currentlyDistributedPower, Math.Min(this._playerPowerValue, this._maxDistributionValue));
		this._lessPowerButton.interactable = true;
		this._morePowerButton.interactable = true;
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x0002B221 File Offset: 0x00029421
	public void LockState()
	{
		this._lessPowerButton.interactable = false;
		this._morePowerButton.interactable = false;
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x00065E7C File Offset: 0x0006407C
	private void OnLessPowerButtonClicked()
	{
		if (this._currentlyDistributedPower - 1 < 0)
		{
			this._currentlyDistributedPower = Math.Min(this._playerPowerValue, this._maxDistributionValue);
		}
		else
		{
			int num = this._currentlyDistributedPower - 1;
			this._currentlyDistributedPower = num;
			this._currentlyDistributedPower = Mathf.Max(num, 0);
		}
		this._distributionText.text = string.Format("{0}/{1}", this._currentlyDistributedPower, Math.Min(this._playerPowerValue, this._maxDistributionValue));
		if (this.DistributedPowerChanged != null)
		{
			this.DistributedPowerChanged(this._currentlyDistributedPower);
		}
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x00065F1C File Offset: 0x0006411C
	private void OnMorePowerButtonClicked()
	{
		if (this._currentlyDistributedPower + 1 > Math.Min(this._playerPowerValue, this._maxDistributionValue))
		{
			this._currentlyDistributedPower = 0;
		}
		else
		{
			int num = this._currentlyDistributedPower + 1;
			this._currentlyDistributedPower = num;
			this._currentlyDistributedPower = Mathf.Min(num, Math.Min(this._playerPowerValue, this._maxDistributionValue));
		}
		this._distributionText.text = string.Format("{0}/{1}", this._currentlyDistributedPower, Math.Min(this._playerPowerValue, this._maxDistributionValue));
		if (this.DistributedPowerChanged != null)
		{
			this.DistributedPowerChanged(this._currentlyDistributedPower);
		}
	}

	// Token: 0x04000417 RID: 1047
	[SerializeField]
	private int _maxDistributionValue;

	// Token: 0x04000418 RID: 1048
	[SerializeField]
	private TMP_Text _distributionText;

	// Token: 0x04000419 RID: 1049
	[SerializeField]
	private Button _lessPowerButton;

	// Token: 0x0400041A RID: 1050
	[SerializeField]
	private Button _morePowerButton;

	// Token: 0x0400041B RID: 1051
	private int _playerPowerValue;

	// Token: 0x0400041C RID: 1052
	private int _currentlyDistributedPower;
}
