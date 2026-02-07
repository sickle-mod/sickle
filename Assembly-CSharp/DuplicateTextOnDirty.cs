using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200012C RID: 300
public class DuplicateTextOnDirty : MonoBehaviour
{
	// Token: 0x06000927 RID: 2343 RVA: 0x0002E3D2 File Offset: 0x0002C5D2
	private void OnEnable()
	{
		this.DuplicateText();
		this.onSourceTextLayoutGetDirty = (UnityAction)Delegate.Combine(this.onSourceTextLayoutGetDirty, new UnityAction(this.DuplicateText));
		this.sourceText.RegisterDirtyLayoutCallback(this.onSourceTextLayoutGetDirty);
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x0002E40D File Offset: 0x0002C60D
	private void OnDisable()
	{
		this.sourceText.UnregisterDirtyLayoutCallback(this.onSourceTextLayoutGetDirty);
		this.onSourceTextLayoutGetDirty = (UnityAction)Delegate.Remove(this.onSourceTextLayoutGetDirty, new UnityAction(this.DuplicateText));
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x0002E442 File Offset: 0x0002C642
	private void DuplicateText()
	{
		this.targetText.text = this.sourceText.text;
	}

	// Token: 0x04000861 RID: 2145
	[SerializeField]
	private Text sourceText;

	// Token: 0x04000862 RID: 2146
	[SerializeField]
	private TextMeshProUGUI targetText;

	// Token: 0x04000863 RID: 2147
	private UnityAction onSourceTextLayoutGetDirty;
}
