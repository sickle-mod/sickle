using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000128 RID: 296
public class AchievementButton : MonoBehaviour
{
	// Token: 0x06000915 RID: 2325 RVA: 0x0002E2AD File Offset: 0x0002C4AD
	private void OnEnable()
	{
		this.achivementButton.onClick.AddListener(new UnityAction(this.UnlockAchievementThirdParty));
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x0002E2CB File Offset: 0x0002C4CB
	private void OnDisable()
	{
		this.achivementButton.onClick.RemoveListener(new UnityAction(this.UnlockAchievementThirdParty));
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x0002E2E9 File Offset: 0x0002C4E9
	public void InstantiateButton(Achievements achievement)
	{
		this.achievementToSend = achievement;
		this.buttonName.text = achievement.ToString();
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x0002E30A File Offset: 0x0002C50A
	private void UnlockAchievementThirdParty()
	{
		GameServiceController.Instance.SetAchievement(this.achievementToSend);
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04000858 RID: 2136
	public Achievements achievementToSend;

	// Token: 0x04000859 RID: 2137
	public TMP_Text buttonName;

	// Token: 0x0400085A RID: 2138
	[SerializeField]
	private Button achivementButton;
}
