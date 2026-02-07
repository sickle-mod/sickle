using System;
using Scythe.GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace Scythe.UI
{
	// Token: 0x02000476 RID: 1142
	public class ObjectiveCardPresenter : MonoBehaviour
	{
		// Token: 0x0600241A RID: 9242 RVA: 0x000D61A8 File Offset: 0x000D43A8
		public void SetCard(ObjectiveCard objectiveCard, Sprite logo)
		{
			base.gameObject.SetActive(true);
			this.title.text = GameController.GetObjectiveTitle(objectiveCard.CardId);
			this.description.text = GameController.GetObjectiveDescription(objectiveCard.CardId);
			this.logo.sprite = logo;
			if (PlatformManager.IsStandalone)
			{
				AssetBundle assetBundle = AssetBundleManager.LoadAssetBundle("graphic_objectives");
				this.picture.sprite = assetBundle.LoadAsset<Sprite>("objective_" + objectiveCard.CardId.ToString().PadLeft(2, '0'));
				return;
			}
			AssetBundle assetBundle2 = AssetBundleManager.LoadAssetBundle("graphic_objectivesmobileingame");
			this.picture.sprite = assetBundle2.LoadAllAssets<Sprite>()[objectiveCard.CardId - 1];
		}

		// Token: 0x0600241B RID: 9243 RVA: 0x0003EF60 File Offset: 0x0003D160
		public void Close()
		{
			AssetBundleManager.UnloadAssetBundle("graphic_objectives", false);
			base.gameObject.SetActive(false);
		}

		// Token: 0x04001936 RID: 6454
		public Text title;

		// Token: 0x04001937 RID: 6455
		public Text description;

		// Token: 0x04001938 RID: 6456
		public Image logo;

		// Token: 0x04001939 RID: 6457
		public Image picture;
	}
}
