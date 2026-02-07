using System;
using UnityEngine;

// Token: 0x02000151 RID: 337
public class ScreenshotSaver : MonoBehaviour
{
	// Token: 0x060009F2 RID: 2546 RVA: 0x0002EC0A File Offset: 0x0002CE0A
	private void Update()
	{
		if (this.saveScreenshot)
		{
			ScreenCapture.CaptureScreenshot(this.fileName + ".png");
			this.saveScreenshot = false;
			Debug.Log("Screenshot " + this.fileName + ".png has been saved.");
		}
	}

	// Token: 0x040008EB RID: 2283
	public string fileName;

	// Token: 0x040008EC RID: 2284
	public bool saveScreenshot;
}
