using System;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020003D0 RID: 976
	public class ActionContextWindow : MonoBehaviour
	{
		// Token: 0x140000B6 RID: 182
		// (add) Token: 0x06001C93 RID: 7315 RVA: 0x000B2578 File Offset: 0x000B0778
		// (remove) Token: 0x06001C94 RID: 7316 RVA: 0x000B25B0 File Offset: 0x000B07B0
		public event Action OnClose;

		// Token: 0x06001C95 RID: 7317 RVA: 0x0002920A File Offset: 0x0002740A
		protected void SetActive(bool active)
		{
			base.gameObject.SetActive(active);
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x000B25E8 File Offset: 0x000B07E8
		private void OnEnable()
		{
			OptionsManager.OnLanguageChanged += this.UpdateLocalization;
			if (this.PayLabel != null)
			{
				if (this.PayAmount == 0)
				{
					this.PayLabel.gameObject.SetActive(false);
				}
				else
				{
					this.PayLabel.gameObject.SetActive(true);
					this.PayLabel.text = this.PayOptions[this.PayAmount];
				}
			}
			this.UpdateGainLabelText();
			if (this.DownActionCoinGain != null)
			{
				this.DownActionCoinGain.SetActive(this.GainAmount > 1);
			}
			if (this.Instruction != null)
			{
				this.Instruction.SetActive(false);
			}
			if (this.Description != null)
			{
				this.Description.SetActive(true);
			}
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x0003AA81 File Offset: 0x00038C81
		private void OnDisable()
		{
			OptionsManager.OnLanguageChanged -= this.UpdateLocalization;
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x000B26B4 File Offset: 0x000B08B4
		public void SetProductionCost(int cost)
		{
			for (int i = 0; i < this.productionCosts.Length; i++)
			{
				this.productionCosts[i].SetActive(i < cost / 2);
			}
		}

		// Token: 0x06001C99 RID: 7321 RVA: 0x000B26E8 File Offset: 0x000B08E8
		public void SetCanAfford(bool canAfford)
		{
			if (this.notEnoughResources != null)
			{
				if (!PlatformManager.IsStandalone && !canAfford)
				{
					this.HintLabel.text = ScriptLocalization.Get("GameScene/NotEnoughResourcesMobile");
					this.HintLabel.color = this.NotEnoughResourcesHintColor;
				}
				this.notEnoughResources.SetActive(!canAfford);
			}
		}

		// Token: 0x06001C9A RID: 7322 RVA: 0x000B2744 File Offset: 0x000B0944
		public void SetMaxReached(bool isMaxReached)
		{
			if (this.maxReached != null)
			{
				if (!PlatformManager.IsStandalone && isMaxReached)
				{
					this.HintLabel.text = ScriptLocalization.Get("GameScene/MaxReachedMobile");
					this.HintLabel.color = this.MaximumReachedHintColor;
				}
				this.maxReached.SetActive(isMaxReached);
			}
		}

		// Token: 0x06001C9B RID: 7323 RVA: 0x000B27A0 File Offset: 0x000B09A0
		public void SetNoSuitablePlace(bool noSuitablePlace)
		{
			if (this.noSuitablePlaceLabel != null)
			{
				if (!PlatformManager.IsStandalone && noSuitablePlace)
				{
					this.HintLabel.text = "<color=#EB5757>" + ScriptLocalization.Get("GameScene/NoSuitablePlace") + "</color>";
					this.HintLabel.color = this.MaximumReachedHintColor;
				}
				this.noSuitablePlaceLabel.SetActive(noSuitablePlace);
			}
		}

		// Token: 0x06001C9C RID: 7324 RVA: 0x000B2808 File Offset: 0x000B0A08
		public void SetUsedLastTurn(bool usedLastTurn)
		{
			if (this.usedLastTurnLabel != null)
			{
				if (!PlatformManager.IsStandalone && usedLastTurn)
				{
					this.HintLabel.text = ScriptLocalization.Get("GameScene/UsedLastTurnWithoutBrackets");
					this.HintLabel.color = this.NotEnoughResourcesHintColor;
				}
				this.usedLastTurnLabel.SetActive(usedLastTurn);
			}
		}

		// Token: 0x06001C9D RID: 7325 RVA: 0x0003AA94 File Offset: 0x00038C94
		public void SetDescription(string description)
		{
			if (this.Description != null)
			{
				this.Description.SetActive(true);
				this.Description.GetComponent<TextMeshProUGUI>().text = description;
			}
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x0003AAC1 File Offset: 0x00038CC1
		public void Close()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.CommonCheckBoxV1);
			this.SetActive(false);
			if (this.OnClose != null)
			{
				this.OnClose();
				this.ClearCloseEvent();
			}
		}

		// Token: 0x06001C9F RID: 7327 RVA: 0x0003AAEF File Offset: 0x00038CEF
		public void ClearCloseEvent()
		{
			this.OnClose = null;
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x0003AAF8 File Offset: 0x00038CF8
		private void UpdateLocalization()
		{
			this.UpdateGainLabelText();
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x000B2864 File Offset: 0x000B0A64
		private void UpdateGainLabelText()
		{
			if (this.GainLabel != null)
			{
				int num;
				if (int.TryParse(this.GainOptions[this.GainAmount], out num))
				{
					this.GainLabel.text = this.GainOptions[this.GainAmount];
					return;
				}
				this.GainLabel.text = ScriptLocalization.Get(this.GainOptions[this.GainAmount]);
			}
		}

		// Token: 0x040014A3 RID: 5283
		public int PayAmount;

		// Token: 0x040014A4 RID: 5284
		public int GainAmount;

		// Token: 0x040014A5 RID: 5285
		public string[] PayOptions;

		// Token: 0x040014A6 RID: 5286
		public string[] GainOptions;

		// Token: 0x040014A7 RID: 5287
		public GameObject[] productionCosts;

		// Token: 0x040014A8 RID: 5288
		public GameObject DownActionCoinGain;

		// Token: 0x040014A9 RID: 5289
		public TextMeshProUGUI PayLabel;

		// Token: 0x040014AA RID: 5290
		public TextMeshProUGUI GainLabel;

		// Token: 0x040014AB RID: 5291
		public GameObject Description;

		// Token: 0x040014AC RID: 5292
		public GameObject Instruction;

		// Token: 0x040014AD RID: 5293
		public GameObject notEnoughResources;

		// Token: 0x040014AE RID: 5294
		public GameObject maxReached;

		// Token: 0x040014AF RID: 5295
		public GameObject noSuitablePlaceLabel;

		// Token: 0x040014B0 RID: 5296
		public GameObject usedLastTurnLabel;

		// Token: 0x040014B1 RID: 5297
		public Button ConfirmButton;

		// Token: 0x040014B3 RID: 5299
		public TMP_Text HintLabel;

		// Token: 0x040014B4 RID: 5300
		private Color MaximumReachedHintColor = new Color32(33, 150, 83, byte.MaxValue);

		// Token: 0x040014B5 RID: 5301
		private Color NotEnoughResourcesHintColor = new Color32(235, 87, 87, byte.MaxValue);
	}
}
