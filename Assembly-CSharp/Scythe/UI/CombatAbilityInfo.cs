using System;
using I2.Loc;
using Scythe.GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000432 RID: 1074
	public class CombatAbilityInfo : MonoBehaviour
	{
		// Token: 0x06002101 RID: 8449 RVA: 0x000C7470 File Offset: 0x000C5670
		public void ShowInfoAboutUsedCombatAbility(AbilityPerk combatAbility)
		{
			if (combatAbility <= AbilityPerk.Scout)
			{
				if (combatAbility != AbilityPerk.Camaraderie)
				{
					if (combatAbility != AbilityPerk.Artillery)
					{
						if (combatAbility == AbilityPerk.Scout)
						{
							this._abilityText.text = ScriptLocalization.Get("GameScene/ScoutUsed");
						}
					}
					else
					{
						this._abilityText.text = ScriptLocalization.Get("GameScene/ArtilleryUsed");
					}
				}
				else
				{
					this._abilityText.text = ScriptLocalization.Get("GameScene/CamaraderieUsed");
				}
			}
			else if (combatAbility <= AbilityPerk.Sword)
			{
				if (combatAbility != AbilityPerk.Disarm)
				{
					if (combatAbility == AbilityPerk.Sword)
					{
						this._abilityText.text = ScriptLocalization.Get("GameScene/SwordUsed");
					}
				}
				else
				{
					this._abilityText.text = ScriptLocalization.Get("GameScene/DisarmUsed");
				}
			}
			else if (combatAbility != AbilityPerk.Shield)
			{
				if (combatAbility == AbilityPerk.Ronin)
				{
					this._abilityText.text = ScriptLocalization.Get("GameScene/RoninUsed");
				}
			}
			else
			{
				this._abilityText.text = ScriptLocalization.Get("GameScene/ShieldUsed");
			}
			base.gameObject.SetActive(true);
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x0003D090 File Offset: 0x0003B290
		public void Hide()
		{
			WorldSFXManager.PlaySound(SoundEnum.CommonBgGreenButton, AudioSourceType.Buttons);
			base.gameObject.SetActive(false);
		}

		// Token: 0x0400170C RID: 5900
		[SerializeField]
		private Text _abilityText;
	}
}
