using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007DD RID: 2013
	public class DefaultUINavigationInput : UINavigationInput
	{
		// Token: 0x0600399A RID: 14746 RVA: 0x0014E618 File Offset: 0x0014C818
		public void ProcessInput(UINavigationManager navigationManager)
		{
			StandaloneInputModule component = EventSystem.current.GetComponent<StandaloneInputModule>();
			bool flag = false;
			if (Input.GetMouseButtonDown(0))
			{
				navigationManager.LoseFocus();
				flag = true;
			}
			if (Input.GetButtonDown("Tab"))
			{
				if (Input.GetButton("Shift"))
				{
					navigationManager.MoveFocus(UINavigationManager.Direction.Previous);
				}
				else
				{
					navigationManager.MoveFocus(UINavigationManager.Direction.Next);
				}
				flag = true;
			}
			if (Input.GetButtonDown("Previous"))
			{
				navigationManager.MoveFocus(UINavigationManager.Direction.Previous);
				flag = true;
			}
			if (Input.GetButtonDown("Next"))
			{
				navigationManager.MoveFocus(UINavigationManager.Direction.Next);
				flag = true;
			}
			if (Input.GetButtonDown(component.submitButton))
			{
				navigationManager.ValidateFocusable();
				flag = true;
			}
			if (Input.GetButtonDown(component.cancelButton))
			{
				navigationManager.CancelCurrentContext();
				flag = true;
			}
			if (Input.GetButtonDown("Edit"))
			{
				navigationManager.FindFocusableInputFieldAndEnterEditMode();
				flag = true;
			}
			float axisRaw = Input.GetAxisRaw(component.horizontalAxis);
			if ((axisRaw < -0.1f || axisRaw > 0.1f) && !this._horizontalAxisInUse)
			{
				this._horizontalAxisInUse = true;
				if (axisRaw > 0f)
				{
					navigationManager.MoveFocus(UINavigationManager.Direction.Right);
				}
				else
				{
					navigationManager.MoveFocus(UINavigationManager.Direction.Left);
				}
				flag = true;
			}
			if (axisRaw == 0f)
			{
				this._horizontalAxisInUse = false;
			}
			float axisRaw2 = Input.GetAxisRaw(component.verticalAxis);
			if ((axisRaw2 < -0.1f || axisRaw2 > 0.1f) && !this._verticalAxisInUse)
			{
				this._verticalAxisInUse = true;
				if (axisRaw2 > 0f)
				{
					navigationManager.MoveFocus(UINavigationManager.Direction.Up);
				}
				else
				{
					navigationManager.MoveFocus(UINavigationManager.Direction.Down);
				}
				flag = true;
			}
			if (axisRaw2 == 0f)
			{
				this._verticalAxisInUse = false;
			}
			if (!flag && Input.anyKeyDown)
			{
				string inputString = Input.inputString;
				if (!string.IsNullOrEmpty(inputString))
				{
					navigationManager.HandleInputString(inputString);
				}
			}
		}

		// Token: 0x04002B7B RID: 11131
		private bool _horizontalAxisInUse;

		// Token: 0x04002B7C RID: 11132
		private bool _verticalAxisInUse;
	}
}
