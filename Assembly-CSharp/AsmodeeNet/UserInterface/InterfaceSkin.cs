using System;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007BE RID: 1982
	[CreateAssetMenu]
	public class InterfaceSkin : ScriptableObject
	{
		// Token: 0x04002AEB RID: 10987
		public GameObject alertControllerPrefab;

		// Token: 0x04002AEC RID: 10988
		[Header("SSO")]
		public GameObject ssoPrefab;

		// Token: 0x04002AED RID: 10989
		public GameObject updateEmailPopUpPrefab;

		// Token: 0x04002AEE RID: 10990
		[Header("Cross-Promotion")]
		public GameObject BannerPrefab;

		// Token: 0x04002AEF RID: 10991
		public GameObject InterstitialPopupPrefab;

		// Token: 0x04002AF0 RID: 10992
		public GameObject MoreGamesPopupPrefab;

		// Token: 0x04002AF1 RID: 10993
		public GameObject GameDetailsPopupPrefab;
	}
}
