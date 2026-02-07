using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000134 RID: 308
[RequireComponent(typeof(InputField))]
public class InputEnterKeyHandler : MonoBehaviour
{
	// Token: 0x06000944 RID: 2372 RVA: 0x0002E582 File Offset: 0x0002C782
	private void Start()
	{
		this.input = base.GetComponent<InputField>();
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x0002E590 File Offset: 0x0002C790
	private void Update()
	{
		if ((Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter)) && this.input.IsActive())
		{
			this.input.ActivateInputField();
			this.button.onClick.Invoke();
		}
	}

	// Token: 0x0400087E RID: 2174
	public Button button;

	// Token: 0x0400087F RID: 2175
	private InputField input;
}
