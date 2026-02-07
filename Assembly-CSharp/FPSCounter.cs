using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200012E RID: 302
public class FPSCounter : MonoBehaviour
{
	// Token: 0x0600092D RID: 2349 RVA: 0x0002E486 File Offset: 0x0002C686
	private void Awake()
	{
		this.text = base.GetComponent<Text>();
		this.timeStamps = new List<float>();
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x0007AD78 File Offset: 0x00078F78
	private void Update()
	{
		this.timeStamps.Add(Time.unscaledTime);
		while (Time.unscaledTime - this.timeStamps[0] > 1f)
		{
			this.timeStamps.RemoveAt(0);
		}
		this.text.text = this.timeStamps.Count.ToString() + "<size=8> FPS</size>";
		if (Input.GetKeyDown(KeyCode.F1))
		{
			this.text.enabled = !this.text.enabled;
		}
	}

	// Token: 0x0400086A RID: 2154
	private Text text;

	// Token: 0x0400086B RID: 2155
	private List<float> timeStamps;
}
