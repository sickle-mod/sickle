using System;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x02000801 RID: 2049
	public abstract class CrossPromoBasePopup : MonoBehaviour
	{
		// Token: 0x06003A78 RID: 14968 RVA: 0x0004DC21 File Offset: 0x0004BE21
		protected virtual void Awake()
		{
			this._responsivePopUp = base.GetComponent<ResponsivePopUp>();
			if (this._responsivePopUp == null)
			{
				AsmoLogger.Error("CrossPromoBasePopup", "Missing ResponsivePopUp behavior", null);
			}
		}

		// Token: 0x06003A79 RID: 14969 RVA: 0x0004DC4D File Offset: 0x0004BE4D
		public virtual void Dismiss()
		{
			if (base.gameObject.activeSelf)
			{
				this._responsivePopUp.FadeOut(delegate
				{
					global::UnityEngine.Object.Destroy(base.gameObject);
				});
				return;
			}
			global::UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04002C16 RID: 11286
		private const string _kModuleName = "CrossPromoBasePopup";

		// Token: 0x04002C17 RID: 11287
		private ResponsivePopUp _responsivePopUp;
	}
}
