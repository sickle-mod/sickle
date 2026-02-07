using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000089 RID: 137
public class CombatPanelCardSlot : MonoBehaviour
{
	// Token: 0x1400002A RID: 42
	// (add) Token: 0x060004A2 RID: 1186 RVA: 0x00064E38 File Offset: 0x00063038
	// (remove) Token: 0x060004A3 RID: 1187 RVA: 0x00064E70 File Offset: 0x00063070
	public event Action<CombatPanelCardSlot> SlotClicked;

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x060004A4 RID: 1188 RVA: 0x0002AE8B File Offset: 0x0002908B
	public bool IsAvailable
	{
		get
		{
			return this._isAvailable;
		}
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x0002AE93 File Offset: 0x00029093
	private void Awake()
	{
		this._slotButton.onClick.AddListener(new UnityAction(this.OnSlotButtonClicked));
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x00064EA8 File Offset: 0x000630A8
	public void SelectSlot(int cardPower)
	{
		this._cardImage.sprite = this._usedCardSprite;
		this._isAvailable = false;
		this._cardBonusPowerImage.gameObject.SetActive(true);
		switch (cardPower)
		{
		case 2:
			this._cardBonusPowerImage.sprite = this._twoPowerCardSprite;
			return;
		case 3:
			this._cardBonusPowerImage.sprite = this._threePowerCardSprite;
			return;
		case 4:
			this._cardBonusPowerImage.sprite = this._fourPowerCardSprite;
			return;
		case 5:
			this._cardBonusPowerImage.sprite = this._fivePowerCardSprite;
			return;
		default:
			Debug.LogError("There's no bonus card power sprite for given bonus");
			return;
		}
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x0002AEB1 File Offset: 0x000290B1
	public void DeselectSlot()
	{
		this._cardBonusPowerImage.gameObject.SetActive(false);
		this._cardImage.sprite = this._unusedCardSprite;
		this._isAvailable = true;
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x0002AEDC File Offset: 0x000290DC
	public void DetachEvent()
	{
		Debug.Log("Event detached");
		this.SlotClicked = null;
	}

	// Token: 0x060004A9 RID: 1193 RVA: 0x0002AEEF File Offset: 0x000290EF
	private void OnSlotButtonClicked()
	{
		if (this.SlotClicked != null)
		{
			this.SlotClicked(this);
		}
	}

	// Token: 0x040003D4 RID: 980
	private bool _isAvailable = true;

	// Token: 0x040003D5 RID: 981
	[SerializeField]
	private Image _cardImage;

	// Token: 0x040003D6 RID: 982
	[SerializeField]
	private Sprite _usedCardSprite;

	// Token: 0x040003D7 RID: 983
	[SerializeField]
	private Sprite _unusedCardSprite;

	// Token: 0x040003D8 RID: 984
	[SerializeField]
	private Button _slotButton;

	// Token: 0x040003D9 RID: 985
	[SerializeField]
	private Image _cardBonusPowerImage;

	// Token: 0x040003DA RID: 986
	[Space]
	[SerializeField]
	private Sprite _twoPowerCardSprite;

	// Token: 0x040003DB RID: 987
	[SerializeField]
	private Sprite _threePowerCardSprite;

	// Token: 0x040003DC RID: 988
	[SerializeField]
	private Sprite _fourPowerCardSprite;

	// Token: 0x040003DD RID: 989
	[SerializeField]
	private Sprite _fivePowerCardSprite;
}
