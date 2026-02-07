using System;
using AsmodeeNet.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface.SSO
{
	// Token: 0x020007E8 RID: 2024
	public class SSOBasePanel : MonoBehaviour
	{
		// Token: 0x060039D9 RID: 14809 RVA: 0x0004D2D0 File Offset: 0x0004B4D0
		public virtual void Show()
		{
			this.SwitchWaitingPanelMode(false, -1);
			base.gameObject.SetActive(true);
		}

		// Token: 0x060039DA RID: 14810 RVA: 0x00029172 File Offset: 0x00027372
		public virtual void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x060039DB RID: 14811 RVA: 0x0014F57C File Offset: 0x0014D77C
		public void SwitchWaitingPanelMode(bool isWaiting, int waitingButtonIndex = -1)
		{
			if (isWaiting && waitingButtonIndex == -1)
			{
				AsmoLogger.Warning("SSOBasePanel", "In " + base.gameObject.name + " you may want to specify a \"waitingButtonIndex\"", null);
			}
			for (int i = 0; i < this._buttons.Length; i++)
			{
				ActivityIndicatorButton component = this._buttons[i].GetComponent<ActivityIndicatorButton>();
				if (component != null)
				{
					if (!isWaiting)
					{
						component.Waiting = false;
						component.Interactable = true;
					}
					else if (i == waitingButtonIndex)
					{
						component.Waiting = true;
						component.Interactable = false;
					}
					else
					{
						component.Waiting = false;
						component.Interactable = false;
					}
				}
				else
				{
					this._buttons[i].interactable = !isWaiting;
				}
			}
		}

		// Token: 0x04002BAA RID: 11178
		private const string _kModuleName = "SSOBasePanel";

		// Token: 0x04002BAB RID: 11179
		[Tooltip("In this order: no, yes, others")]
		[SerializeField]
		protected Button[] _buttons;
	}
}
