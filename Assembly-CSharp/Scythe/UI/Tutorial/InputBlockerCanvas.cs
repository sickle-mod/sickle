using System;
using Scythe.Utilities;
using UnityEngine;

namespace Scythe.UI.Tutorial
{
	// Token: 0x02000506 RID: 1286
	public class InputBlockerCanvas : MonoBehaviour
	{
		// Token: 0x06002912 RID: 10514 RVA: 0x00042AEC File Offset: 0x00040CEC
		private void Awake()
		{
			SingletonMono<InputBlockerController>.Instance.RegisterCanvas(this);
			SingletonMono<CanvasHider>.Instance.RegisterCanvas(base.GetComponent<Canvas>());
		}
	}
}
