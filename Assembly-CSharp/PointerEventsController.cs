using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200010E RID: 270
public class PointerEventsController : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x14000038 RID: 56
	// (add) Token: 0x060008D0 RID: 2256 RVA: 0x0007A1D0 File Offset: 0x000783D0
	// (remove) Token: 0x060008D1 RID: 2257 RVA: 0x0007A208 File Offset: 0x00078408
	public event PointerEventsController.ButtonHoover buttonHoover;

	// Token: 0x14000039 RID: 57
	// (add) Token: 0x060008D2 RID: 2258 RVA: 0x0007A240 File Offset: 0x00078440
	// (remove) Token: 0x060008D3 RID: 2259 RVA: 0x0007A278 File Offset: 0x00078478
	public event PointerEventsController.ButtonHooverExit buttonHooverExit;

	// Token: 0x060008D4 RID: 2260 RVA: 0x0002E106 File Offset: 0x0002C306
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.buttonHoover != null)
		{
			this.buttonHoover();
		}
	}

	// Token: 0x060008D5 RID: 2261 RVA: 0x0002E11B File Offset: 0x0002C31B
	public void OnPointerExit(PointerEventData eventData)
	{
		if (this.buttonHooverExit != null)
		{
			this.buttonHooverExit();
		}
	}

	// Token: 0x0200010F RID: 271
	// (Invoke) Token: 0x060008D8 RID: 2264
	public delegate void ButtonHoover();

	// Token: 0x02000110 RID: 272
	// (Invoke) Token: 0x060008DC RID: 2268
	public delegate void ButtonHooverExit();
}
