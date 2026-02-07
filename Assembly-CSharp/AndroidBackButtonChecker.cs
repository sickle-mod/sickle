using System;
using System.Collections.Generic;
using System.Linq;
using Scythe.UI;
using Scythe.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000F3 RID: 243
public class AndroidBackButtonChecker : GenericSingletonClass<AndroidBackButtonChecker>
{
	// Token: 0x060007F5 RID: 2037 RVA: 0x00077740 File Offset: 0x00075940
	private void Update()
	{
		if (PlatformManager.IsAndroid && Input.GetKeyDown(KeyCode.Escape))
		{
			if (SceneManager.GetActiveScene().name == SceneController.SCENE_MENU_NAME && AndroidBackButtonChecker.buttonsToInvoke.Count == 0)
			{
				SingletonMono<MainMenu>.Instance.EnableExitGamePopup();
			}
			if (AndroidBackButtonChecker.buttonsToInvoke.Count > 0)
			{
				AndroidBackButtonChecker.buttonsToInvoke.Last<InvokeButtonOnAndroidBack>().ClickButton();
			}
		}
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x0002D45C File Offset: 0x0002B65C
	public static void AddButtonToList(InvokeButtonOnAndroidBack buttonToInvoke)
	{
		AndroidBackButtonChecker.buttonsToInvoke.Add(buttonToInvoke);
	}

	// Token: 0x060007F7 RID: 2039 RVA: 0x0002D469 File Offset: 0x0002B669
	public static void RemoveButtonFromList(InvokeButtonOnAndroidBack buttonToInvoke)
	{
		AndroidBackButtonChecker.buttonsToInvoke.Remove(buttonToInvoke);
	}

	// Token: 0x040006CB RID: 1739
	public static List<InvokeButtonOnAndroidBack> buttonsToInvoke = new List<InvokeButtonOnAndroidBack>();
}
