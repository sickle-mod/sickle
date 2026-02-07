using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000FF RID: 255
public class SetupMenuGraphics : MonoBehaviour
{
	// Token: 0x06000862 RID: 2146 RVA: 0x0002DA82 File Offset: 0x0002BC82
	private void Awake()
	{
		this.SetupNewBackground();
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x00078380 File Offset: 0x00076580
	private void SetupNewBackground()
	{
		AssetBundle assetBundle = AssetBundleManager.LoadAssetBundle("graphic_backgrounds_menu");
		int num = assetBundle.GetAllAssetNames().Length;
		int num2 = PlayerPrefs.GetInt(SaveBackgroundIndex.BackgroundPrefs, 0);
		int num3 = ((!GameServiceController.Instance.InvadersFromAfarUnlocked()) ? (num - 3) : (num - 1));
		if (num2 >= num3)
		{
			num2 = 0;
		}
		else
		{
			num2++;
		}
		Debug.Log("Background id: " + num2.ToString());
		if (assetBundle != null && num2 < num)
		{
			this.menuBackground.sprite = assetBundle.LoadAsset<Sprite>("Background_Menu_" + num2.ToString().PadLeft(2, '0'));
		}
		SaveBackgroundIndex.BackgroundIndex = int.Parse(this.menuBackground.sprite.name.Substring(this.menuBackground.sprite.name.Length - 2));
		this.SetupLogoColor(num2);
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x00078458 File Offset: 0x00076658
	private void SetupLogoColor(int backgroundIndex)
	{
		switch (backgroundIndex)
		{
		case 1:
			this.gameLogo.color = Color.black;
			return;
		case 2:
			this.gameLogo.color = Color.white;
			return;
		case 3:
			this.gameLogo.color = Color.black;
			return;
		case 4:
			this.gameLogo.color = Color.black;
			return;
		case 5:
			this.gameLogo.color = Color.white;
			return;
		case 6:
			this.gameLogo.color = Color.black;
			return;
		case 7:
			this.gameLogo.color = Color.white;
			return;
		case 8:
			this.gameLogo.color = Color.black;
			return;
		case 9:
			this.gameLogo.color = Color.black;
			return;
		default:
			this.gameLogo.color = Color.black;
			return;
		}
	}

	// Token: 0x04000710 RID: 1808
	[SerializeField]
	private Image menuBackground;

	// Token: 0x04000711 RID: 1809
	[SerializeField]
	private Image gameLogo;
}
