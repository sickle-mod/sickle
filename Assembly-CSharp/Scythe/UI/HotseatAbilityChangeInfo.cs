using System;
using I2.Loc;
using Scythe.GameLogic;
using Scythe.Utilities;
using TMPro;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x020004E7 RID: 1255
	public class HotseatAbilityChangeInfo : MonoBehaviour
	{
		// Token: 0x06002845 RID: 10309 RVA: 0x0004201D File Offset: 0x0004021D
		public void Activate(int faction)
		{
			if (this.HasChangedAbility(faction))
			{
				this.faction = (Faction)faction;
				this.title.text = this.GetTitle();
				base.gameObject.SetActive(true);
				return;
			}
			this.Deactivate();
		}

		// Token: 0x06002846 RID: 10310 RVA: 0x00029172 File Offset: 0x00027372
		public void Deactivate()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06002847 RID: 10311 RVA: 0x00042053 File Offset: 0x00040253
		public void Maximize_OnClick()
		{
			SingletonMono<HotseatAbilityChangeInfoMaximized>.Instance.Activate(this.faction);
		}

		// Token: 0x06002848 RID: 10312 RVA: 0x00042065 File Offset: 0x00040265
		private bool HasChangedAbility(int faction)
		{
			return faction == 0 || faction == 5;
		}

		// Token: 0x06002849 RID: 10313 RVA: 0x000EA614 File Offset: 0x000E8814
		private string GetTitle()
		{
			Faction faction = this.faction;
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

		// Token: 0x04001CE1 RID: 7393
		[SerializeField]
		private TextMeshProUGUI title;

		// Token: 0x04001CE2 RID: 7394
		private Faction faction;
	}
}
