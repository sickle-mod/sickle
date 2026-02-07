using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200012A RID: 298
public class AddAchievementDebug : MonoBehaviour
{
	// Token: 0x06000920 RID: 2336 RVA: 0x0002E36B File Offset: 0x0002C56B
	private void OnEnable()
	{
		this.addAchievmentWindowButton.onClick.AddListener(new UnityAction(this.OpenAchivementsWindow));
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x0002E389 File Offset: 0x0002C589
	private void OnDisable()
	{
		this.addAchievmentWindowButton.onClick.RemoveListener(new UnityAction(this.OpenAchivementsWindow));
	}

	// Token: 0x06000922 RID: 2338 RVA: 0x0002E3A7 File Offset: 0x0002C5A7
	private void OpenAchivementsWindow()
	{
		this.achievementsWindow.gameObject.SetActive(true);
	}

	// Token: 0x0400085E RID: 2142
	[SerializeField]
	private Button addAchievmentWindowButton;

	// Token: 0x0400085F RID: 2143
	[SerializeField]
	private Transform achievementsWindow;
}
