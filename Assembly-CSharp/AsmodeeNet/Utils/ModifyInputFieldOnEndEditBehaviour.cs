using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace AsmodeeNet.Utils
{
	// Token: 0x0200084E RID: 2126
	public static class ModifyInputFieldOnEndEditBehaviour
	{
		// Token: 0x06003C05 RID: 15365 RVA: 0x00154580 File Offset: 0x00152780
		public static void ModifyBehaviour(TMP_InputField inputField)
		{
			for (int i = 0; i < inputField.onEndEdit.GetPersistentEventCount(); i++)
			{
				int index = i;
				inputField.onEndEdit.SetPersistentListenerState(i, UnityEventCallState.Off);
				inputField.onEndEdit.AddListener(delegate(string value)
				{
					if (Input.GetButtonDown(EventSystem.current.GetComponent<StandaloneInputModule>().submitButton))
					{
						((Component)inputField.onEndEdit.GetPersistentTarget(index)).SendMessage(inputField.onEndEdit.GetPersistentMethodName(index), value);
					}
				});
			}
		}
	}
}
