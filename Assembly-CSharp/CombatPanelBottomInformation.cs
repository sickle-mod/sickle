using System;
using I2.Loc;
using Scythe.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000088 RID: 136
public class CombatPanelBottomInformation : MonoBehaviour
{
	// Token: 0x060004A0 RID: 1184 RVA: 0x00064DB8 File Offset: 0x00062FB8
	public void SetUpInformation(GameController.FactionInfo factionInfo, bool isAttacker)
	{
		this._factionEmblemImage.sprite = factionInfo.logo;
		this._factionNameText.text = LocalizationManager.GetTranslation("FactionMat/" + factionInfo.faction.ToString(), true, 0, true, false, null, null);
		this._battleSideText.text = (isAttacker ? LocalizationManager.GetTranslation("GameScene/Attacker", true, 0, true, false, null, null) : LocalizationManager.GetTranslation("GameScene/Defender", true, 0, true, false, null, null));
	}

	// Token: 0x040003CD RID: 973
	private const string _attackerString = "GameScene/Attacker";

	// Token: 0x040003CE RID: 974
	private const string _defenderString = "GameScene/Defender";

	// Token: 0x040003CF RID: 975
	private const string _factionKeyPrefix = "FactionMat/";

	// Token: 0x040003D0 RID: 976
	[SerializeField]
	private Image _factionEmblemImage;

	// Token: 0x040003D1 RID: 977
	[SerializeField]
	private TMP_Text _factionNameText;

	// Token: 0x040003D2 RID: 978
	[SerializeField]
	private TMP_Text _battleSideText;
}
