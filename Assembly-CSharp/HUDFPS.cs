using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000133 RID: 307
public class HUDFPS : MonoBehaviour
{
	// Token: 0x06000941 RID: 2369 RVA: 0x0002E545 File Offset: 0x0002C745
	private void Start()
	{
		this.fpsLabel = base.GetComponent<Text>();
		this.timeleft = this.updateInterval;
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x0007B12C File Offset: 0x0007932C
	private void Update()
	{
		this.timeleft -= Time.deltaTime;
		this.accum += Time.timeScale / Time.deltaTime;
		this.frames++;
		if ((double)this.timeleft <= 0.0)
		{
			float num = this.accum / (float)this.frames;
			string text = string.Format("{0:F2} FPS", num);
			if (this.fpsLabel)
			{
				this.fpsLabel.text = text;
				if (num >= (float)this.greenFPSLimit)
				{
					this.fpsLabel.color = Color.green;
				}
				else if (num >= (float)this.yellowFPSLimit)
				{
					this.fpsLabel.color = Color.yellow;
				}
				else
				{
					this.fpsLabel.color = Color.red;
				}
			}
			this.timeleft = this.updateInterval;
			this.accum = 0f;
			this.frames = 0;
		}
	}

	// Token: 0x04000877 RID: 2167
	public float updateInterval = 0.5f;

	// Token: 0x04000878 RID: 2168
	public int greenFPSLimit = 30;

	// Token: 0x04000879 RID: 2169
	public int yellowFPSLimit = 20;

	// Token: 0x0400087A RID: 2170
	private float accum;

	// Token: 0x0400087B RID: 2171
	private int frames;

	// Token: 0x0400087C RID: 2172
	private float timeleft;

	// Token: 0x0400087D RID: 2173
	private Text fpsLabel;
}
