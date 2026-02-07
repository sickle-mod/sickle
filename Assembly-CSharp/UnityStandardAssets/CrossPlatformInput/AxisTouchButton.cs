using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput
{
	// Token: 0x020006F0 RID: 1776
	public class AxisTouchButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
	{
		// Token: 0x06003593 RID: 13715 RVA: 0x00140428 File Offset: 0x0013E628
		private void OnEnable()
		{
			if (!CrossPlatformInputManager.AxisExists(this.axisName))
			{
				this.m_Axis = new CrossPlatformInputManager.VirtualAxis(this.axisName);
				CrossPlatformInputManager.RegisterVirtualAxis(this.m_Axis);
			}
			else
			{
				this.m_Axis = CrossPlatformInputManager.VirtualAxisReference(this.axisName);
			}
			this.FindPairedButton();
		}

		// Token: 0x06003594 RID: 13716 RVA: 0x00140478 File Offset: 0x0013E678
		private void FindPairedButton()
		{
			AxisTouchButton[] array = global::UnityEngine.Object.FindObjectsOfType(typeof(AxisTouchButton)) as AxisTouchButton[];
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].axisName == this.axisName && array[i] != this)
					{
						this.m_PairedWith = array[i];
					}
				}
			}
		}

		// Token: 0x06003595 RID: 13717 RVA: 0x0004A1D4 File Offset: 0x000483D4
		private void OnDisable()
		{
			this.m_Axis.Remove();
		}

		// Token: 0x06003596 RID: 13718 RVA: 0x001404D4 File Offset: 0x0013E6D4
		public void OnPointerDown(PointerEventData data)
		{
			if (this.m_PairedWith == null)
			{
				this.FindPairedButton();
			}
			this.m_Axis.Update(Mathf.MoveTowards(this.m_Axis.GetValue, this.axisValue, this.responseSpeed * Time.deltaTime));
		}

		// Token: 0x06003597 RID: 13719 RVA: 0x0004A1E1 File Offset: 0x000483E1
		public void OnPointerUp(PointerEventData data)
		{
			this.m_Axis.Update(Mathf.MoveTowards(this.m_Axis.GetValue, 0f, this.responseSpeed * Time.deltaTime));
		}

		// Token: 0x0400274E RID: 10062
		public string axisName = "Horizontal";

		// Token: 0x0400274F RID: 10063
		public float axisValue = 1f;

		// Token: 0x04002750 RID: 10064
		public float responseSpeed = 3f;

		// Token: 0x04002751 RID: 10065
		public float returnToCentreSpeed = 3f;

		// Token: 0x04002752 RID: 10066
		private AxisTouchButton m_PairedWith;

		// Token: 0x04002753 RID: 10067
		private CrossPlatformInputManager.VirtualAxis m_Axis;
	}
}
