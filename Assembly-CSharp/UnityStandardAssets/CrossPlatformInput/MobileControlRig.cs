using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput
{
	// Token: 0x020006F9 RID: 1785
	[ExecuteInEditMode]
	public class MobileControlRig : MonoBehaviour
	{
		// Token: 0x060035DC RID: 13788 RVA: 0x0004A5B1 File Offset: 0x000487B1
		private void OnEnable()
		{
			this.CheckEnableControlRig();
		}

		// Token: 0x060035DD RID: 13789 RVA: 0x0004A5B9 File Offset: 0x000487B9
		private void Start()
		{
			if (global::UnityEngine.Object.FindObjectOfType<EventSystem>() == null)
			{
				GameObject gameObject = new GameObject("EventSystem");
				gameObject.AddComponent<EventSystem>();
				gameObject.AddComponent<StandaloneInputModule>();
			}
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x0004A5DF File Offset: 0x000487DF
		private void CheckEnableControlRig()
		{
			this.EnableControlRig(false);
		}

		// Token: 0x060035DF RID: 13791 RVA: 0x00140704 File Offset: 0x0013E904
		private void EnableControlRig(bool enabled)
		{
			foreach (object obj in base.transform)
			{
				((Transform)obj).gameObject.SetActive(enabled);
			}
		}
	}
}
