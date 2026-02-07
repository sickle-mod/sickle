using System;
using UnityEngine;

// Token: 0x02000011 RID: 17
public class SimpleShotScript : MonoBehaviour
{
	// Token: 0x06000036 RID: 54 RVA: 0x00027EF0 File Offset: 0x000260F0
	private void Awake()
	{
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00027F90 File Offset: 0x00026190
	private void Update()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			this.PlayShootAnimation();
		}
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00027FA1 File Offset: 0x000261A1
	public void PlayShootAnimation()
	{
		if (this.animator != null && Time.fixedTime - this.lastShootTime < this.shootTime)
		{
			this.animator.SetTrigger("Shoot");
			this.lastShootTime = Time.fixedTime;
		}
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00027EF0 File Offset: 0x000260F0
	public void Shoot()
	{
	}

	// Token: 0x04000036 RID: 54
	[SerializeField]
	private float shootTime;

	// Token: 0x04000037 RID: 55
	[SerializeField]
	private Animator animator;

	// Token: 0x04000038 RID: 56
	private float lastShootTime;

	// Token: 0x04000039 RID: 57
	[SerializeField]
	private ParticleSystem muzzleFlash;

	// Token: 0x0400003A RID: 58
	[SerializeField]
	private AudioSource audioSource;
}
