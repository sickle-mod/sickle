using System;
using UnityEngine;

// Token: 0x0200000D RID: 13
public class GunShoot : MonoBehaviour
{
	// Token: 0x0600002A RID: 42 RVA: 0x00027F17 File Offset: 0x00026117
	private void Start()
	{
		this.anim = base.GetComponent<Animator>();
		this.gunAim = base.GetComponentInParent<GunAim>();
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00052D1C File Offset: 0x00050F1C
	private void Update()
	{
		if (Input.GetButtonDown("Fire1") && Time.time > this.nextFire && !this.gunAim.GetIsOutOfBounds())
		{
			this.nextFire = Time.time + this.fireRate;
			this.muzzleFlash.Play();
			this.cartridgeEjection.Play();
			this.anim.SetTrigger("Fire");
			RaycastHit raycastHit;
			if (Physics.Raycast(this.gunEnd.position, this.gunEnd.forward, out raycastHit, this.weaponRange))
			{
				this.HandleHit(raycastHit);
			}
		}
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00052DB4 File Offset: 0x00050FB4
	private void HandleHit(RaycastHit hit)
	{
		if (hit.collider.sharedMaterial != null)
		{
			string name = hit.collider.sharedMaterial.name;
			uint num = global::<PrivateImplementationDetails>.ComputeStringHash(name);
			if (num <= 1044434307U)
			{
				if (num != 81868168U)
				{
					if (num != 970575400U)
					{
						if (num != 1044434307U)
						{
							return;
						}
						if (!(name == "Sand"))
						{
							return;
						}
						this.SpawnDecal(hit, this.sandHitEffect);
						return;
					}
					else
					{
						if (!(name == "WaterFilled"))
						{
							return;
						}
						this.SpawnDecal(hit, this.waterLeakEffect);
						this.SpawnDecal(hit, this.metalHitEffect);
						return;
					}
				}
				else
				{
					if (!(name == "Wood"))
					{
						return;
					}
					this.SpawnDecal(hit, this.woodHitEffect);
					return;
				}
			}
			else if (num <= 2840670588U)
			{
				if (num != 1842662042U)
				{
					if (num != 2840670588U)
					{
						return;
					}
					if (!(name == "Metal"))
					{
						return;
					}
					this.SpawnDecal(hit, this.metalHitEffect);
					return;
				}
				else
				{
					if (!(name == "Stone"))
					{
						return;
					}
					this.SpawnDecal(hit, this.stoneHitEffect);
					return;
				}
			}
			else if (num != 3966976176U)
			{
				if (num != 4022181330U)
				{
					return;
				}
				if (!(name == "Meat"))
				{
					return;
				}
				this.SpawnDecal(hit, this.fleshHitEffects[global::UnityEngine.Random.Range(0, this.fleshHitEffects.Length)]);
				return;
			}
			else
			{
				if (!(name == "Character"))
				{
					return;
				}
				this.SpawnDecal(hit, this.fleshHitEffects[global::UnityEngine.Random.Range(0, this.fleshHitEffects.Length)]);
			}
		}
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00027F31 File Offset: 0x00026131
	private void SpawnDecal(RaycastHit hit, GameObject prefab)
	{
		global::UnityEngine.Object.Instantiate<GameObject>(prefab, hit.point, Quaternion.LookRotation(hit.normal)).transform.SetParent(hit.collider.transform);
	}

	// Token: 0x04000015 RID: 21
	public float fireRate = 0.25f;

	// Token: 0x04000016 RID: 22
	public float weaponRange = 20f;

	// Token: 0x04000017 RID: 23
	public Transform gunEnd;

	// Token: 0x04000018 RID: 24
	public ParticleSystem muzzleFlash;

	// Token: 0x04000019 RID: 25
	public ParticleSystem cartridgeEjection;

	// Token: 0x0400001A RID: 26
	public GameObject metalHitEffect;

	// Token: 0x0400001B RID: 27
	public GameObject sandHitEffect;

	// Token: 0x0400001C RID: 28
	public GameObject stoneHitEffect;

	// Token: 0x0400001D RID: 29
	public GameObject waterLeakEffect;

	// Token: 0x0400001E RID: 30
	public GameObject[] fleshHitEffects;

	// Token: 0x0400001F RID: 31
	public GameObject woodHitEffect;

	// Token: 0x04000020 RID: 32
	private float nextFire;

	// Token: 0x04000021 RID: 33
	private Animator anim;

	// Token: 0x04000022 RID: 34
	private GunAim gunAim;
}
