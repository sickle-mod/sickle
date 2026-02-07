using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Reworked.Main.DLC
{
	// Token: 0x02000195 RID: 405
	public class TrapPointerEvents : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
	{
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000BD8 RID: 3032 RVA: 0x0002FFBA File Offset: 0x0002E1BA
		// (set) Token: 0x06000BD9 RID: 3033 RVA: 0x0002FFC2 File Offset: 0x0002E1C2
		public Action OnEnter { get; set; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000BDA RID: 3034 RVA: 0x0002FFCB File Offset: 0x0002E1CB
		// (set) Token: 0x06000BDB RID: 3035 RVA: 0x0002FFD3 File Offset: 0x0002E1D3
		public Action OnExit { get; set; }

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000BDC RID: 3036 RVA: 0x0002FFDC File Offset: 0x0002E1DC
		// (set) Token: 0x06000BDD RID: 3037 RVA: 0x0002FFE4 File Offset: 0x0002E1E4
		public Action OnClick { get; set; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000BDE RID: 3038 RVA: 0x0002FFED File Offset: 0x0002E1ED
		// (set) Token: 0x06000BDF RID: 3039 RVA: 0x0002FFF5 File Offset: 0x0002E1F5
		public bool CanClose { get; private set; } = true;

		// Token: 0x06000BE0 RID: 3040 RVA: 0x0002FFFE File Offset: 0x0002E1FE
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!this.CanClose)
			{
				return;
			}
			if (this.tryExit != null)
			{
				base.StopCoroutine(this.tryExit);
			}
			this.CanClose = false;
			Action onEnter = this.OnEnter;
			if (onEnter == null)
			{
				return;
			}
			onEnter();
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x0007FD54 File Offset: 0x0007DF54
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.CanClose || !eventData.fullyExited)
			{
				this.tryExit = base.StartCoroutine(this.SkipFramesAndTryExit(eventData.enterEventCamera));
				return;
			}
			this.CanClose = true;
			Action onExit = this.OnExit;
			if (onExit == null)
			{
				return;
			}
			onExit();
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00030034 File Offset: 0x0002E234
		private IEnumerator SkipFramesAndTryExit(Camera cam)
		{
			int num;
			for (int i = 0; i < 10; i = num + 1)
			{
				yield return null;
				num = i;
			}
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			List<RaycastResult> list = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerEventData, list);
			if (list.Any((RaycastResult hit) => hit.gameObject == base.gameObject))
			{
				yield return this.SkipFramesAndTryExit(cam);
				yield break;
			}
			this.CanClose = true;
			Action onExit = this.OnExit;
			if (onExit != null)
			{
				onExit();
			}
			yield break;
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x0003004A File Offset: 0x0002E24A
		public void OnPointerClick(PointerEventData eventData)
		{
			Action onClick = this.OnClick;
			if (onClick == null)
			{
				return;
			}
			onClick();
		}

		// Token: 0x04000996 RID: 2454
		private Coroutine tryExit;

		// Token: 0x04000997 RID: 2455
		private const int FRAMES_TO_SKIP = 10;
	}
}
