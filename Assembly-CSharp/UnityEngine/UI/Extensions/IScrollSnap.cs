using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200070A RID: 1802
	internal interface IScrollSnap
	{
		// Token: 0x06003648 RID: 13896
		void ChangePage(int page);

		// Token: 0x06003649 RID: 13897
		void SetLerp(bool value);

		// Token: 0x0600364A RID: 13898
		int CurrentPage();

		// Token: 0x0600364B RID: 13899
		void StartScreenChange();
	}
}
