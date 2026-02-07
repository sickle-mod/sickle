using System;
using UnityEngine;

// Token: 0x02000160 RID: 352
[RequireComponent(typeof(RectTransform))]
public class SimpleUIRotator : MonoBehaviour
{
	// Token: 0x06000A51 RID: 2641 RVA: 0x0002F0A1 File Offset: 0x0002D2A1
	private void Start()
	{
		this.rectTransform = base.GetComponent<RectTransform>();
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x0002F0AF File Offset: 0x0002D2AF
	private void Update()
	{
		this.rectTransform.Rotate(new Vector3(0f, 0f, Time.deltaTime * this.Speed));
	}

	// Token: 0x040008F7 RID: 2295
	public float Speed = 360f;

	// Token: 0x040008F8 RID: 2296
	private RectTransform rectTransform;
}
