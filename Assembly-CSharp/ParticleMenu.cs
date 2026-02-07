using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200000F RID: 15
public class ParticleMenu : MonoBehaviour
{
	// Token: 0x06000030 RID: 48 RVA: 0x00027F80 File Offset: 0x00026180
	private void Start()
	{
		this.Navigate(0);
		this.currentIndex = 0;
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00052F2C File Offset: 0x0005112C
	public void Navigate(int i)
	{
		this.currentIndex = (this.particleSystems.Length + this.currentIndex + i) % this.particleSystems.Length;
		if (this.currentGO != null)
		{
			global::UnityEngine.Object.Destroy(this.currentGO);
		}
		this.currentGO = global::UnityEngine.Object.Instantiate<GameObject>(this.particleSystems[this.currentIndex].particleSystemGO, this.spawnLocation.position + this.particleSystems[this.currentIndex].particlePosition, Quaternion.Euler(this.particleSystems[this.currentIndex].particleRotation));
		this.gunGameObject.SetActive(this.particleSystems[this.currentIndex].isWeaponEffect);
		this.title.text = this.particleSystems[this.currentIndex].title;
		this.description.text = this.particleSystems[this.currentIndex].description;
		this.navigationDetails.text = (this.currentIndex + 1).ToString() + " out of " + this.particleSystems.Length.ToString();
	}

	// Token: 0x04000029 RID: 41
	public ParticleExamples[] particleSystems;

	// Token: 0x0400002A RID: 42
	public GameObject gunGameObject;

	// Token: 0x0400002B RID: 43
	private int currentIndex;

	// Token: 0x0400002C RID: 44
	private GameObject currentGO;

	// Token: 0x0400002D RID: 45
	public Transform spawnLocation;

	// Token: 0x0400002E RID: 46
	public Text title;

	// Token: 0x0400002F RID: 47
	public Text description;

	// Token: 0x04000030 RID: 48
	public Text navigationDetails;
}
