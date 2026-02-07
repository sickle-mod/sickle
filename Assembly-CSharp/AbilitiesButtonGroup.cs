using System;
using UnityEngine;

// Token: 0x020000F2 RID: 242
public class AbilitiesButtonGroup : MonoBehaviour
{
	// Token: 0x060007F2 RID: 2034 RVA: 0x000776B8 File Offset: 0x000758B8
	private void OnEnable()
	{
		this.mechAbilitiesExpandObject[0].SetActive(true);
		for (int i = 1; i < this.mechAbilitiesExpandObject.Length; i++)
		{
			this.mechAbilitiesExpandObject[i].SetActive(false);
		}
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x000776F4 File Offset: 0x000758F4
	public void ButtonClicked(int buttonIndex)
	{
		ButtonsSFXManager.Instance.PlaySound(SoundEnum.GuiChatButton);
		for (int i = 0; i < this.mechAbilitiesExpandObject.Length; i++)
		{
			if (i != buttonIndex)
			{
				this.mechAbilitiesExpandObject[i].SetActive(false);
			}
		}
		this.mechAbilitiesExpandObject[buttonIndex].SetActive(true);
	}

	// Token: 0x040006CA RID: 1738
	[SerializeField]
	private GameObject[] mechAbilitiesExpandObject;
}
