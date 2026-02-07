using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004B2 RID: 1202
	public class ObjectivePreview : MonoBehaviour
	{
		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06002625 RID: 9765 RVA: 0x0004068F File Offset: 0x0003E88F
		// (set) Token: 0x06002626 RID: 9766 RVA: 0x00040697 File Offset: 0x0003E897
		public bool IsActiveToClear { get; set; }

		// Token: 0x06002627 RID: 9767 RVA: 0x000406A0 File Offset: 0x0003E8A0
		private void Awake()
		{
			this.SwitchStateToIdle();
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x000E2B64 File Offset: 0x000E0D64
		public void SwitchStateToIdle()
		{
			this.isHighlighted = false;
			this.text.color = this.idleTextColor;
			this.text.fontMaterial.DisableKeyword("GLOW_ON");
			this.symbol.color = this.idleSymbolColor;
			this.UpdateText();
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x000E2BB8 File Offset: 0x000E0DB8
		public void SwitchStateToHighlighted()
		{
			this.isHighlighted = true;
			this.text.color = this.highlightedTextColor;
			this.symbol.color = this.highlightedSymbolColor;
			this.text.fontMaterial.EnableKeyword("GLOW_ON");
			this.UpdateText();
		}

		// Token: 0x0600262A RID: 9770 RVA: 0x000406A8 File Offset: 0x0003E8A8
		public void SetText(string objectiveTitle)
		{
			this.objectiveTitle = objectiveTitle;
			this.UpdateText();
		}

		// Token: 0x0600262B RID: 9771 RVA: 0x000E2C0C File Offset: 0x000E0E0C
		private void UpdateText()
		{
			if (this.isHighlighted)
			{
				this.text.text = this.objectiveTitle + " (!)";
			}
			else
			{
				this.text.text = this.objectiveTitle;
			}
			this.text.fontSize = 9f;
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x000406B7 File Offset: 0x0003E8B7
		private void Update()
		{
			if (this.isHighlighted)
			{
				this.text.fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0.3f + Mathf.Sin(Time.time * this.glowingSpeed) * 0.7f);
			}
		}

		// Token: 0x04001B14 RID: 6932
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04001B15 RID: 6933
		[SerializeField]
		private Image symbol;

		// Token: 0x04001B16 RID: 6934
		[Space(10f)]
		[SerializeField]
		private Color idleTextColor;

		// Token: 0x04001B17 RID: 6935
		[SerializeField]
		private Color highlightedTextColor;

		// Token: 0x04001B18 RID: 6936
		[SerializeField]
		private Color idleSymbolColor;

		// Token: 0x04001B19 RID: 6937
		[SerializeField]
		private Color highlightedSymbolColor;

		// Token: 0x04001B1A RID: 6938
		[SerializeField]
		private float glowingSpeed;

		// Token: 0x04001B1C RID: 6940
		private string objectiveTitle;

		// Token: 0x04001B1D RID: 6941
		private bool isHighlighted;
	}
}
