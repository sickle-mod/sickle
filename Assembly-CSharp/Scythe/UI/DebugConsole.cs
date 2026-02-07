using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x020004F4 RID: 1268
	public class DebugConsole : MonoBehaviour
	{
		// Token: 0x060028C3 RID: 10435 RVA: 0x000EB5C8 File Offset: 0x000E97C8
		private void Awake()
		{
			if (!Debug.isDebugBuild && !Application.isEditor)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			if (DebugConsole.instance == null)
			{
				DebugConsole.instance = base.gameObject;
				global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
			else if (DebugConsole.instance != base.gameObject)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			Application.logMessageReceived += this.Application_logMessageReceived;
		}

		// Token: 0x060028C4 RID: 10436 RVA: 0x000426BC File Offset: 0x000408BC
		private void OnDestroy()
		{
			Application.logMessageReceived -= this.Application_logMessageReceived;
			DebugConsole.instance = null;
		}

		// Token: 0x060028C5 RID: 10437 RVA: 0x000426D5 File Offset: 0x000408D5
		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.BackQuote))
			{
				this.consoleScrollView.SetActive(!this.consoleScrollView.activeInHierarchy);
			}
		}

		// Token: 0x060028C6 RID: 10438 RVA: 0x000EB644 File Offset: 0x000E9844
		private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
		{
			GameObject gameObject;
			if (this.consoleLogs.Count < 30)
			{
				gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.debugLogTemplate, this.consoleContent);
			}
			else
			{
				gameObject = this.consoleLogs[0].gameObject;
				gameObject.transform.SetParent(null);
				gameObject.transform.SetParent(this.consoleContent);
			}
			DebugConsoleLog component = gameObject.GetComponent<DebugConsoleLog>();
			if (component != null)
			{
				this.consoleLogs.Add(component);
				component.SetValues(condition, stackTrace, type);
			}
		}

		// Token: 0x04001D38 RID: 7480
		[SerializeField]
		private List<DebugConsoleLog> consoleLogs;

		// Token: 0x04001D39 RID: 7481
		[SerializeField]
		private GameObject consoleScrollView;

		// Token: 0x04001D3A RID: 7482
		[SerializeField]
		private Transform consoleContent;

		// Token: 0x04001D3B RID: 7483
		[SerializeField]
		private GameObject debugLogTemplate;

		// Token: 0x04001D3C RID: 7484
		private static GameObject instance;

		// Token: 0x04001D3D RID: 7485
		private const int MAX_NUMBER_OF_LOGS = 30;
	}
}
