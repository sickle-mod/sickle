using System;
using TMPro;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000709 RID: 1801
	public static class ExtensionsMethods
	{
		// Token: 0x06003645 RID: 13893 RVA: 0x00140DC0 File Offset: 0x0013EFC0
		public static T GetOrAddComponent<T>(this GameObject child) where T : Component
		{
			T t = child.GetComponent<T>();
			if (t == null)
			{
				t = child.AddComponent<T>();
			}
			return t;
		}

		// Token: 0x06003646 RID: 13894 RVA: 0x0004ACB8 File Offset: 0x00048EB8
		public static void Clear(this TMP_InputField tmp_InputField)
		{
			tmp_InputField.text = "";
			tmp_InputField.caretPosition = 0;
		}

		// Token: 0x06003647 RID: 13895 RVA: 0x0004ACCC File Offset: 0x00048ECC
		public static void UpdateTextPositionForMobileInput(this TMP_InputField inputField)
		{
			inputField.shouldHideMobileInput = true;
			inputField.ActivateInputField();
			inputField.ForceLabelUpdate();
			inputField.textComponent.ForceMeshUpdate(false, false);
			inputField.MoveTextEnd(false);
			inputField.Rebuild(CanvasUpdate.LatePreRender);
			inputField.shouldHideMobileInput = false;
		}
	}
}
