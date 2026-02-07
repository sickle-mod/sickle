using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200008F RID: 143
public class CombatPanelPlayerCard : MonoBehaviour
{
	// Token: 0x1400002C RID: 44
	// (add) Token: 0x060004C4 RID: 1220 RVA: 0x0006545C File Offset: 0x0006365C
	// (remove) Token: 0x060004C5 RID: 1221 RVA: 0x00065494 File Offset: 0x00063694
	public event Action CombatCardClicked;

	// Token: 0x060004C6 RID: 1222 RVA: 0x0002AFB9 File Offset: 0x000291B9
	private void Awake()
	{
		this._combatCardButton.onClick.AddListener(new UnityAction(this.OnCombatCardButtonClicked));
	}

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x060004C7 RID: 1223 RVA: 0x0002AFD7 File Offset: 0x000291D7
	// (set) Token: 0x060004C8 RID: 1224 RVA: 0x0002AFE4 File Offset: 0x000291E4
	public bool IsAvailable
	{
		get
		{
			return this._combatCardButton.interactable;
		}
		set
		{
			this._combatCardButton.interactable = value;
			this._cardImage.sprite = (value ? this._availableSprite : this._notAvailableSprite);
		}
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x000654CC File Offset: 0x000636CC
	public void SelectPowerValueSprite(int power)
	{
		this._combatBonusImage.enabled = true;
		switch (power)
		{
		case 2:
			this._combatBonusImage.sprite = this._twoPowerBonusSprite;
			return;
		case 3:
			this._combatBonusImage.sprite = this._threePowerBonusSprite;
			return;
		case 4:
			this._combatBonusImage.sprite = this._fourPowerBonusSprite;
			return;
		case 5:
			this._combatBonusImage.sprite = this._fivePowerBonusSprite;
			return;
		default:
			Debug.LogError("There's no card bonus power sprite for given power bonus");
			return;
		}
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x0002B00E File Offset: 0x0002920E
	private void OnCombatCardButtonClicked()
	{
		if (this.CombatCardClicked != null)
		{
			this.CombatCardClicked();
		}
	}

	// Token: 0x040003F1 RID: 1009
	[SerializeField]
	private Image _cardImage;

	// Token: 0x040003F2 RID: 1010
	[SerializeField]
	private Image _combatBonusImage;

	// Token: 0x040003F3 RID: 1011
	[SerializeField]
	private Button _combatCardButton;

	// Token: 0x040003F4 RID: 1012
	[SerializeField]
	private Sprite _availableSprite;

	// Token: 0x040003F5 RID: 1013
	[SerializeField]
	private Sprite _notAvailableSprite;

	// Token: 0x040003F6 RID: 1014
	[SerializeField]
	private Sprite _twoPowerBonusSprite;

	// Token: 0x040003F7 RID: 1015
	[SerializeField]
	private Sprite _threePowerBonusSprite;

	// Token: 0x040003F8 RID: 1016
	[SerializeField]
	private Sprite _fourPowerBonusSprite;

	// Token: 0x040003F9 RID: 1017
	[SerializeField]
	private Sprite _fivePowerBonusSprite;
}
