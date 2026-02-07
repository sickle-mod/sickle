using System;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x020004E8 RID: 1256
	public class HotseatAbilityChangeInfoMaximized : SingletonMono<HotseatAbilityChangeInfoMaximized>
	{
		// Token: 0x0600284B RID: 10315 RVA: 0x000EA64C File Offset: 0x000E884C
		public void Activate(Faction faction)
		{
			this.title.text = this.GetTitle(faction);
			this.description.text = this.GetDescription(faction);
			this.factionEmblem.sprite = this.GetEmblem(faction);
			this.mainPanel.gameObject.SetActive(true);
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x00042070 File Offset: 0x00040270
		public void Deactivate_OnClick()
		{
			this.mainPanel.gameObject.SetActive(false);
		}

		// Token: 0x0600284D RID: 10317 RVA: 0x00042083 File Offset: 0x00040283
		private string GetTitle(Faction faction)
		{
			if (faction == Faction.Polania)
			{
				return ScriptLocalization.Get("FactionMat/PolaniaFactionAbilityName");
			}
			if (faction != Faction.Crimea)
			{
				return string.Empty;
			}
			return ScriptLocalization.Get("FactionMat/CrimeaMechAbilityTitle2");
		}

		// Token: 0x0600284E RID: 10318 RVA: 0x000420A9 File Offset: 0x000402A9
		private string GetDescription(Faction faction)
		{
			if (faction == Faction.Polania)
			{
				return ScriptLocalization.Get("FactionMat/PolaniaFactionAbilityDescriptionA");
			}
			if (faction != Faction.Crimea)
			{
				return string.Empty;
			}
			return ScriptLocalization.Get("FactionMat/CrimeaMechAbilityDescription2A");
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x000420CF File Offset: 0x000402CF
		private Sprite GetEmblem(Faction faction)
		{
			if (faction == Faction.Polania)
			{
				return this.polaniaEmblem;
			}
			if (faction != Faction.Crimea)
			{
				throw new ArgumentOutOfRangeException("Faction " + faction.ToString() + " is invalid faction.");
			}
			return this.crimeaEmblem;
		}

		// Token: 0x04001CE3 RID: 7395
		[SerializeField]
		private GameObject mainPanel;

		// Token: 0x04001CE4 RID: 7396
		[SerializeField]
		private Sprite polaniaEmblem;

		// Token: 0x04001CE5 RID: 7397
		[SerializeField]
		private Sprite crimeaEmblem;

		// Token: 0x04001CE6 RID: 7398
		[SerializeField]
		private TextMeshProUGUI title;

		// Token: 0x04001CE7 RID: 7399
		[SerializeField]
		private TextMeshProUGUI description;

		// Token: 0x04001CE8 RID: 7400
		[SerializeField]
		private Image factionEmblem;
	}
}
