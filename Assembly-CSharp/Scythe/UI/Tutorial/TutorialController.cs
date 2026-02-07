using System;
using System.Collections;
using Scythe.Utilities;
using UnityEngine;

namespace Scythe.UI.Tutorial
{
	// Token: 0x0200051C RID: 1308
	public class TutorialController : SingletonMono<TutorialController>
	{
		// Token: 0x060029C6 RID: 10694 RVA: 0x00043370 File Offset: 0x00041570
		private IEnumerator Start()
		{
			yield return null;
			if (GameController.GameManager.IsCampaign)
			{
				SingletonMono<InputBlockerController>.Instance.Initialize();
				this.BeginTutorial(GameController.GameManager.missionId);
			}
			yield break;
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x060029C7 RID: 10695 RVA: 0x0004337F File Offset: 0x0004157F
		public int TutorialCount
		{
			get
			{
				return this.tutorials.Length;
			}
		}

		// Token: 0x060029C8 RID: 10696 RVA: 0x00043389 File Offset: 0x00041589
		private void BeginTutorial(int tutorialId)
		{
			if (tutorialId < this.TutorialCount)
			{
				this.tutorials[tutorialId].Begin();
				return;
			}
			SingletonMono<TutorialEndScreen>.Instance.Show();
		}

		// Token: 0x04001DB2 RID: 7602
		[SerializeField]
		private Tutorial[] tutorials;
	}
}
