using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput
{
	// Token: 0x020006F7 RID: 1783
	public class Joystick : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IDragHandler
	{
		// Token: 0x060035D3 RID: 13779 RVA: 0x0004A529 File Offset: 0x00048729
		private void OnEnable()
		{
			this.CreateVirtualAxes();
		}

		// Token: 0x060035D4 RID: 13780 RVA: 0x0004A531 File Offset: 0x00048731
		private void Start()
		{
			this.m_StartPos = base.transform.position;
		}

		// Token: 0x060035D5 RID: 13781 RVA: 0x00140524 File Offset: 0x0013E724
		private void UpdateVirtualAxes(Vector3 value)
		{
			Vector3 vector = this.m_StartPos - value;
			vector.y = -vector.y;
			vector /= (float)this.MovementRange;
			if (this.m_UseX)
			{
				this.m_HorizontalVirtualAxis.Update(-vector.x);
			}
			if (this.m_UseY)
			{
				this.m_VerticalVirtualAxis.Update(vector.y);
			}
		}

		// Token: 0x060035D6 RID: 13782 RVA: 0x00140590 File Offset: 0x0013E790
		private void CreateVirtualAxes()
		{
			this.m_UseX = this.axesToUse == Joystick.AxisOption.Both || this.axesToUse == Joystick.AxisOption.OnlyHorizontal;
			this.m_UseY = this.axesToUse == Joystick.AxisOption.Both || this.axesToUse == Joystick.AxisOption.OnlyVertical;
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

		// Token: 0x060035D7 RID: 13783 RVA: 0x0014061C File Offset: 0x0013E81C
		public void OnDrag(PointerEventData data)
		{
			Vector3 zero = Vector3.zero;
			if (this.m_UseX)
			{
				int num = (int)(data.position.x - this.m_StartPos.x);
				num = Mathf.Clamp(num, -this.MovementRange, this.MovementRange);
				zero.x = (float)num;
			}
			if (this.m_UseY)
			{
				int num2 = (int)(data.position.y - this.m_StartPos.y);
				num2 = Mathf.Clamp(num2, -this.MovementRange, this.MovementRange);
				zero.y = (float)num2;
			}
			base.transform.position = new Vector3(this.m_StartPos.x + zero.x, this.m_StartPos.y + zero.y, this.m_StartPos.z + zero.z);
			this.UpdateVirtualAxes(base.transform.position);
		}

		// Token: 0x060035D8 RID: 13784 RVA: 0x0004A544 File Offset: 0x00048744
		public void OnPointerUp(PointerEventData data)
		{
			base.transform.position = this.m_StartPos;
			this.UpdateVirtualAxes(this.m_StartPos);
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x00027EF0 File Offset: 0x000260F0
		public void OnPointerDown(PointerEventData data)
		{
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x0004A563 File Offset: 0x00048763
		private void OnDisable()
		{
			if (this.m_UseX)
			{
				this.m_HorizontalVirtualAxis.Remove();
			}
			if (this.m_UseY)
			{
				this.m_VerticalVirtualAxis.Remove();
			}
		}

		// Token: 0x04002764 RID: 10084
		public int MovementRange = 100;

		// Token: 0x04002765 RID: 10085
		public Joystick.AxisOption axesToUse;

		// Token: 0x04002766 RID: 10086
		public string horizontalAxisName = "Horizontal";

		// Token: 0x04002767 RID: 10087
		public string verticalAxisName = "Vertical";

		// Token: 0x04002768 RID: 10088
		private Vector3 m_StartPos;

		// Token: 0x04002769 RID: 10089
		private bool m_UseX;

		// Token: 0x0400276A RID: 10090
		private bool m_UseY;

		// Token: 0x0400276B RID: 10091
		private CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis;

		// Token: 0x0400276C RID: 10092
		private CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis;

		// Token: 0x020006F8 RID: 1784
		public enum AxisOption
		{
			// Token: 0x0400276E RID: 10094
			Both,
			// Token: 0x0400276F RID: 10095
			OnlyHorizontal,
			// Token: 0x04002770 RID: 10096
			OnlyVertical
		}
	}
}
