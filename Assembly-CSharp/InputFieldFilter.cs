using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000135 RID: 309
public class InputFieldFilter : MonoBehaviour
{
	// Token: 0x06000947 RID: 2375 RVA: 0x0002E5CF File Offset: 0x0002C7CF
	private void Start()
	{
		this.IF = base.GetComponent<InputField>();
	}

	// Token: 0x06000948 RID: 2376 RVA: 0x0007B228 File Offset: 0x00079428
	public void UpdateField()
	{
		if (this.IF != null)
		{
			this.IF.text = this.IF.text.Replace("\r", "").Replace("\n", "");
		}
	}

	// Token: 0x04000880 RID: 2176
	private InputField IF;
}
