using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000E7 RID: 231
public class CampaignMenuTutorialBox : MonoBehaviour
{
	// Token: 0x060006C8 RID: 1736 RVA: 0x0002C59D File Offset: 0x0002A79D
	private void Awake()
	{
		this.localizedBoxTitle = this.boxTitle.text;
	}

	// Token: 0x060006C9 RID: 1737 RVA: 0x00071D84 File Offset: 0x0006FF84
	public void FillTutorialBox(int index, int maxIndex, string tutorialTitle, string tutorialDescription, Sprite tutorialSprite, bool isCompleted)
	{
		this.boxTitle.text = string.Format(" {0}/{1}", index, maxIndex);
		this.tutorialTitle.text = tutorialTitle;
		this.tutorialDescription.text = tutorialDescription;
		this.tutorialImage.sprite = tutorialSprite;
		this.completionMark.SetActive(isCompleted);
	}

	// Token: 0x04000611 RID: 1553
	[SerializeField]
	private TMP_Text boxTitle;

	// Token: 0x04000612 RID: 1554
	[SerializeField]
	private TMP_Text tutorialTitle;

	// Token: 0x04000613 RID: 1555
	[SerializeField]
	private TMP_Text tutorialDescription;

	// Token: 0x04000614 RID: 1556
	[SerializeField]
	private GameObject completionMark;

	// Token: 0x04000615 RID: 1557
	[SerializeField]
	private Image tutorialImage;

	// Token: 0x04000616 RID: 1558
	private string localizedBoxTitle;
}
