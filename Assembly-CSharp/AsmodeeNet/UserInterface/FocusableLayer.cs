using System;
using System.Collections.Generic;
using AsmodeeNet.Foundation;
using UnityEngine;
using UnityEngine.Events;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007DB RID: 2011
	public class FocusableLayer : MonoBehaviour
	{
		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06003993 RID: 14739 RVA: 0x0004CFB6 File Offset: 0x0004B1B6
		public IList<Focusable> Focusables
		{
			get
			{
				return this._focusables.AsReadOnly();
			}
		}

		// Token: 0x06003994 RID: 14740 RVA: 0x0004CFC3 File Offset: 0x0004B1C3
		private void OnEnable()
		{
			CoreApplication.Instance.UINavigationManager.RegisterFocusableLayer(this);
		}

		// Token: 0x06003995 RID: 14741 RVA: 0x0004CFD5 File Offset: 0x0004B1D5
		private void OnDisable()
		{
			if (CoreApplication.IsQuitting)
			{
				return;
			}
			CoreApplication.Instance.UINavigationManager.UnRegisterFocusableLayer(this);
		}

		// Token: 0x06003996 RID: 14742 RVA: 0x0004CFEF File Offset: 0x0004B1EF
		public void RegisterFocusable(Focusable focusable)
		{
			this._focusables.Add(focusable);
		}

		// Token: 0x06003997 RID: 14743 RVA: 0x0004CFFD File Offset: 0x0004B1FD
		public void UnRegisterFocusable(Focusable focusable)
		{
			this._focusables.Remove(focusable);
		}

		// Token: 0x04002B77 RID: 11127
		private const string _documentation = "<b>FocusableLayer</b> aggregates <b>Focusable</b> controls. It is usally used for the root node of a modal dialog box with the <b>modal</b> flag set to true.";

		// Token: 0x04002B78 RID: 11128
		public bool modal = true;

		// Token: 0x04002B79 RID: 11129
		public UnityEvent OnBackAction;

		// Token: 0x04002B7A RID: 11130
		private List<Focusable> _focusables = new List<Focusable>();
	}
}
