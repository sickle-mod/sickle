using System;
using HoneyFramework;
using UnityEngine;

// Token: 0x0200001C RID: 28
public class RotateTowardsCamera : MonoBehaviour
{
	// Token: 0x0600006D RID: 109 RVA: 0x0002839C File Offset: 0x0002659C
	private void Start()
	{
		if (this.target == null)
		{
			this.target = CameraControler.Instance.gameObject.transform;
		}
	}

	// Token: 0x0600006E RID: 110 RVA: 0x000283C1 File Offset: 0x000265C1
	private void Update()
	{
		this.RotateTowardsCameraOnce();
	}

	// Token: 0x0600006F RID: 111 RVA: 0x0005385C File Offset: 0x00051A5C
	public void RotateTowardsCameraOnce()
	{
		if (base.name == "CircleContainer" || base.name == "IconContainer")
		{
			Quaternion quaternion = Quaternion.LookRotation(base.transform.position - this.target.position, this.target.up);
			base.transform.rotation = Quaternion.Euler(90f, quaternion.eulerAngles.y, quaternion.eulerAngles.z);
		}
		else if (!this.ignoreRotation)
		{
			base.transform.rotation = Quaternion.LookRotation(base.transform.position - this.target.position, this.target.up);
		}
		if (this.target == null)
		{
			Debug.LogError("Target is null!");
			Debug.Log(string.Concat(new string[]
			{
				base.transform.parent.gameObject.GetInstanceID().ToString(),
				" ",
				base.transform.parent.name,
				" ",
				base.transform.position.ToString()
			}));
		}
		if (!this.ignoreScaling)
		{
			float num = (base.transform.position - this.target.position).magnitude + this.distanceModifier;
			num *= this.distanceScaleMultiplier;
			if (num > 1f)
			{
				num = 1f;
			}
			if (num < 0f)
			{
				num = 0f;
			}
			base.transform.localScale = Vector3.one * num;
		}
	}

	// Token: 0x04000067 RID: 103
	public Transform target;

	// Token: 0x04000068 RID: 104
	public bool ignoreRotation;

	// Token: 0x04000069 RID: 105
	public bool ignoreXRotation;

	// Token: 0x0400006A RID: 106
	public bool ignoreScaling;

	// Token: 0x0400006B RID: 107
	public float distanceScaleMultiplier = 1f;

	// Token: 0x0400006C RID: 108
	public float distanceModifier;
}
