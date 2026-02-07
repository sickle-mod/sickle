using System;
using Scythe.Multiplayer.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.Multiplayer
{
	// Token: 0x02000221 RID: 545
	public class ELOAmount : MonoBehaviour
	{
		// Token: 0x06001020 RID: 4128 RVA: 0x00032666 File Offset: 0x00030866
		private void Start()
		{
			this.Reset();
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0003266E File Offset: 0x0003086E
		public void Reset()
		{
			this.currentMin = PlayerInfo.me.PlayerStats.ELO - 50;
			this.currentMax = PlayerInfo.me.PlayerStats.ELO + 50;
			this.CheckValues();
			this.UpdateTexts();
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x000326AC File Offset: 0x000308AC
		public void DecreaseMin()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.currentMin -= 50;
			this.CheckValues();
			this.UpdateTexts();
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x000326D5 File Offset: 0x000308D5
		public void IncreaseMin()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.currentMin += 50;
			this.CheckValues();
			this.UpdateTexts();
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x000326FE File Offset: 0x000308FE
		public void DecreaseMax()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.currentMax -= 50;
			this.CheckValues();
			this.UpdateTexts();
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x00032727 File Offset: 0x00030927
		public void IncreaseMax()
		{
			ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiUpperBeltClick);
			this.currentMax += 50;
			this.CheckValues();
			this.UpdateTexts();
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x0008F5D4 File Offset: 0x0008D7D4
		private void CheckValues()
		{
			if (this.currentMin >= PlayerInfo.me.PlayerStats.ELO)
			{
				this.currentMin = PlayerInfo.me.PlayerStats.ELO;
			}
			if (this.currentMax <= PlayerInfo.me.PlayerStats.ELO)
			{
				this.currentMax = PlayerInfo.me.PlayerStats.ELO;
			}
			if (this.currentMin <= 100)
			{
				this.currentMin = 100;
			}
			if (this.currentMax >= 2000)
			{
				this.currentMax = 2000;
			}
			if (this.currentMin >= this.currentMax)
			{
				this.currentMin = this.currentMax;
			}
			if (this.currentMax <= this.currentMin)
			{
				this.currentMax = this.currentMin;
			}
			this.CheckButtons();
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x0008F6A0 File Offset: 0x0008D8A0
		private void CheckButtons()
		{
			if (this.currentMin == 100)
			{
				this.decreaseMinElo.interactable = false;
			}
			else
			{
				this.decreaseMinElo.interactable = true;
			}
			if (this.currentMin == PlayerInfo.me.PlayerStats.ELO || this.currentMin == this.currentMax)
			{
				this.increaseMinElo.interactable = false;
			}
			else
			{
				this.increaseMinElo.interactable = true;
			}
			if (this.currentMax == this.currentMin || this.currentMax == PlayerInfo.me.PlayerStats.ELO)
			{
				this.decreaseMaxElo.interactable = false;
			}
			else
			{
				this.decreaseMaxElo.interactable = true;
			}
			if (this.currentMax == 2000)
			{
				this.increaseMaxElo.interactable = false;
				return;
			}
			this.increaseMaxElo.interactable = true;
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x00032750 File Offset: 0x00030950
		private void UpdateTexts()
		{
			this.minELOText.text = this.currentMin.ToString();
			this.maxELOText.text = this.currentMax.ToString();
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x0003277E File Offset: 0x0003097E
		public void OnMinValueChanged(string newValue)
		{
			if (!int.TryParse(newValue, out this.currentMin))
			{
				this.currentMin = 100;
			}
			this.CheckValues();
			this.UpdateTexts();
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x000327A2 File Offset: 0x000309A2
		public void OnMaxValueChanged(string newValue)
		{
			if (!int.TryParse(newValue, out this.currentMax))
			{
				this.currentMax = 2000;
			}
			this.CheckValues();
			this.UpdateTexts();
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x000327C9 File Offset: 0x000309C9
		public int GetMin()
		{
			return this.currentMin;
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x000327D1 File Offset: 0x000309D1
		public int GetMax()
		{
			return this.currentMax;
		}

		// Token: 0x04000C72 RID: 3186
		[SerializeField]
		private TMP_InputField minELOText;

		// Token: 0x04000C73 RID: 3187
		[SerializeField]
		private TMP_InputField maxELOText;

		// Token: 0x04000C74 RID: 3188
		[SerializeField]
		private Button decreaseMinElo;

		// Token: 0x04000C75 RID: 3189
		[SerializeField]
		private Button increaseMinElo;

		// Token: 0x04000C76 RID: 3190
		[SerializeField]
		private Button decreaseMaxElo;

		// Token: 0x04000C77 RID: 3191
		[SerializeField]
		private Button increaseMaxElo;

		// Token: 0x04000C78 RID: 3192
		private const int MinELO = 100;

		// Token: 0x04000C79 RID: 3193
		private const int MaxELO = 2000;

		// Token: 0x04000C7A RID: 3194
		private const int ChangeRatio = 50;

		// Token: 0x04000C7B RID: 3195
		private int currentMin;

		// Token: 0x04000C7C RID: 3196
		private int currentMax;
	}
}
