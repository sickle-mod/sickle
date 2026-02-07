using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000152 RID: 338
public class ScrollbarHandleUpdater : MonoBehaviour
{
	// Token: 0x060009F4 RID: 2548 RVA: 0x0002EC4A File Offset: 0x0002CE4A
	private void LateUpdate()
	{
		this._scrollbar.size = this._size;
	}

	// Token: 0x040008ED RID: 2285
	[SerializeField]
	private Scrollbar _scrollbar;

	// Token: 0x040008EE RID: 2286
	[SerializeField]
	[Range(0f, 1f)]
	private float _size = 0.3f;
}
