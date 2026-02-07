using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityStandardAssets.CrossPlatformInput
{
	// Token: 0x020006FE RID: 1790
	[RequireComponent(typeof(Image))]
	public class TouchPad : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
	{
		// Token: 0x060035E6 RID: 13798 RVA: 0x0004A638 File Offset: 0x00048838
		private void OnEnable()
		{
			this.CreateVirtualAxes();
		}

		// Token: 0x060035E7 RID: 13799 RVA: 0x0004A640 File Offset: 0x00048840
		private void Start()
		{
			this.m_Image = base.GetComponent<Image>();
			this.m_Center = this.m_Image.transform.position;
		}

		// Token: 0x060035E8 RID: 13800 RVA: 0x00140860 File Offset: 0x0013EA60
		private void CreateVirtualAxes()
		{
			this.m_UseX = this.axesToUse == TouchPad.AxisOption.Both || this.axesToUse == TouchPad.AxisOption.OnlyHorizontal;
			this.m_UseY = this.axesToUse == TouchPad.AxisOption.Both || this.axesToUse == TouchPad.AxisOption.OnlyVertical;
			if (this.m_UseX)
			{
				this.m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(this.horizontalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(this.m_HorizontalVirtualAxis);
			}
			if (this.m_UseY)
			{
				this.m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(this.verticalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(this.m_VerticalVirtualAxis);
			}
		}

		// Token: 0x060035E9 RID: 13801 RVA: 0x0004A664 File Offset: 0x00048864
		private void UpdateVirtualAxes(Vector3 value)
		{
			value = value.normalized;
			if (this.m_UseX)
			{
				this.m_HorizontalVirtualAxis.Update(value.x);
			}
			if (this.m_UseY)
			{
				this.m_VerticalVirtualAxis.Update(value.y);
			}
		}

		// Token: 0x060035EA RID: 13802 RVA: 0x0004A6A1 File Offset: 0x000488A1
		public void OnPointerDown(PointerEventData data)
		{
			this.m_Dragging = true;
			this.m_Id = data.pointerId;
			if (this.controlStyle != TouchPad.ControlStyle.Absolute)
			{
				this.m_Center = data.position;
			}
		}

		// Token: 0x060035EB RID: 13803 RVA: 0x001408EC File Offset: 0x0013EAEC
		private void Update()
		{
			if (!this.m_Dragging)
			{
				return;
			}
			if (Input.touchCount >= this.m_Id + 1 && this.m_Id != -1)
			{
				if (this.controlStyle == TouchPad.ControlStyle.Swipe)
				{
					this.m_Center = this.m_PreviousTouchPos;
					this.m_PreviousTouchPos = Input.touches[this.m_Id].position;
				}
				Vector2 normalized = new Vector2(Input.touches[this.m_Id].position.x - this.m_Center.x, Input.touches[this.m_Id].position.y - this.m_Center.y).normalized;
				normalized.x *= this.Xsensitivity;
				normalized.y *= this.Ysensitivity;
				this.UpdateVirtualAxes(new Vector3(normalized.x, normalized.y, 0f));
			}
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x0004A6CF File Offset: 0x000488CF
		public void OnPointerUp(PointerEventData data)
		{
			this.m_Dragging = false;
			this.m_Id = -1;
			this.UpdateVirtualAxes(Vector3.zero);
		}

		// Token: 0x060035ED RID: 13805 RVA: 0x0004A6EA File Offset: 0x000488EA
		private void OnDisable()
		{
			if (CrossPlatformInputManager.AxisExists(this.horizontalAxisName))
			{
				CrossPlatformInputManager.UnRegisterVirtualAxis(this.horizontalAxisName);
			}
			if (CrossPlatformInputManager.AxisExists(this.verticalAxisName))
			{
				CrossPlatformInputManager.UnRegisterVirtualAxis(this.verticalAxisName);
			}
		}

		// Token: 0x04002780 RID: 10112
		public TouchPad.AxisOption axesToUse;

		// Token: 0x04002781 RID: 10113
		public TouchPad.ControlStyle controlStyle;

		// Token: 0x04002782 RID: 10114
		public string horizontalAxisName = "Horizontal";

		// Token: 0x04002783 RID: 10115
		public string verticalAxisName = "Vertical";

		// Token: 0x04002784 RID: 10116
		public float Xsensitivity = 1f;

		// Token: 0x04002785 RID: 10117
		public float Ysensitivity = 1f;

		// Token: 0x04002786 RID: 10118
		private Vector3 m_StartPos;

		// Token: 0x04002787 RID: 10119
		private Vector2 m_PreviousDelta;

		// Token: 0x04002788 RID: 10120
		private Vector3 m_JoytickOutput;

		// Token: 0x04002789 RID: 10121
		private bool m_UseX;

		// Token: 0x0400278A RID: 10122
		private bool m_UseY;

		// Token: 0x0400278B RID: 10123
		private CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis;

		// Token: 0x0400278C RID: 10124
		private CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis;

		// Token: 0x0400278D RID: 10125
		private bool m_Dragging;

		// Token: 0x0400278E RID: 10126
		private int m_Id = -1;

		// Token: 0x0400278F RID: 10127
		private Vector2 m_PreviousTouchPos;

		// Token: 0x04002790 RID: 10128
		private Vector3 m_Center;

		// Token: 0x04002791 RID: 10129
		private Image m_Image;

		// Token: 0x020006FF RID: 1791
		public enum AxisOption
		{
			// Token: 0x04002793 RID: 10131
			Both,
			// Token: 0x04002794 RID: 10132
			OnlyHorizontal,
			// Token: 0x04002795 RID: 10133
			OnlyVertical
		}

		// Token: 0x02000700 RID: 1792
		public enum ControlStyle
		{
			// Token: 0x04002797 RID: 10135
			Absolute,
			// Token: 0x04002798 RID: 10136
			Relative,
			// Token: 0x04002799 RID: 10137
			Swipe
		}
	}
}
