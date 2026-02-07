using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200011D RID: 285
public class BuildVersion : MonoBehaviour
{
	// Token: 0x06000901 RID: 2305 RVA: 0x0002E208 File Offset: 0x0002C408
	private void Start()
	{
		this.buildVersion = ((this.buildVersion != null) ? this.buildVersion : base.GetComponent<Text>());
		this.buildVersion.text = BuildVersionUtility.GetBuildVersionWithEndpoint();
	}

	// Token: 0x04000839 RID: 2105
	public Text buildVersion;
}
