using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000129 RID: 297
public class AchievementsDebugWindow : MonoBehaviour
{
	// Token: 0x0600091A RID: 2330 RVA: 0x0002E327 File Offset: 0x0002C527
	private void OnEnable()
	{
		this.exitButton.onClick.AddListener(new UnityAction(this.ExitWindow));
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x0002E345 File Offset: 0x0002C545
	private void OnDisable()
	{
		this.exitButton.onClick.RemoveListener(new UnityAction(this.ExitWindow));
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x0002E363 File Offset: 0x0002C563
	private void Start()
	{
		this.SpawnAchivementsButtons();
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x0007AC78 File Offset: 0x00078E78
	private void SpawnAchivementsButtons()
	{
		foreach (object obj in Enum.GetValues(typeof(Achievements)))
		{
			Achievements achievements = (Achievements)obj;
			if (!AchievementManager.IsAchievementUnlocked(achievements, false))
			{
				global::UnityEngine.Object.Instantiate<GameObject>(this.achivementButtonPrefab.gameObject, this.achivementsScrollViewContent).GetComponent<AchievementButton>().InstantiateButton(achievements);
			}
		}
	}

	// Token: 0x0600091E RID: 2334 RVA: 0x00029172 File Offset: 0x00027372
	private void ExitWindow()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x0400085B RID: 2139
	[SerializeField]
	private Transform achivementsScrollViewContent;

	// Token: 0x0400085C RID: 2140
	[SerializeField]
	private AchievementButton achivementButtonPrefab;

	// Token: 0x0400085D RID: 2141
	[SerializeField]
	private Button exitButton;
}
