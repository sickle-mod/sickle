using System;
using UnityEngine;

namespace UnityStandardAssets.CrossPlatformInput.PlatformSpecific
{
	// Token: 0x02000703 RID: 1795
	public class StandaloneInput : VirtualInput
	{
		// Token: 0x06003615 RID: 13845 RVA: 0x0004AA71 File Offset: 0x00048C71
		public override float GetAxis(string name, bool raw)
		{
			if (!raw)
			{
				return Input.GetAxis(name);
			}
			return Input.GetAxisRaw(name);
		}

		// Token: 0x06003616 RID: 13846 RVA: 0x0004AA83 File Offset: 0x00048C83
		public override bool GetButton(string name)
		{
			return Input.GetButton(name);
		}

		// Token: 0x06003617 RID: 13847 RVA: 0x0004AA8B File Offset: 0x00048C8B
		public override bool GetButtonDown(string name)
		{
			return Input.GetButtonDown(name);
		}

		// Token: 0x06003618 RID: 13848 RVA: 0x0004AA93 File Offset: 0x00048C93
		public override bool GetButtonUp(string name)
		{
			return Input.GetButtonUp(name);
		}

		// Token: 0x06003619 RID: 13849 RVA: 0x0004AA9B File Offset: 0x00048C9B
		public override void SetButtonDown(string name)
		{
			throw new Exception(" This is not possible to be called for standalone input. Please check your platform and code where this is called");
		}

		// Token: 0x0600361A RID: 13850 RVA: 0x0004AA9B File Offset: 0x00048C9B
		public override void SetButtonUp(string name)
		{
			throw new Exception(" This is not possible to be called for standalone input. Please check your platform and code where this is called");
		}

		// Token: 0x0600361B RID: 13851 RVA: 0x0004AA9B File Offset: 0x00048C9B
		public override void SetAxisPositive(string name)
		{
			throw new Exception(" This is not possible to be called for standalone input. Please check your platform and code where this is called");
		}

		// Token: 0x0600361C RID: 13852 RVA: 0x0004AA9B File Offset: 0x00048C9B
		public override void SetAxisNegative(string name)
		{
			throw new Exception(" This is not possible to be called for standalone input. Please check your platform and code where this is called");
		}

		// Token: 0x0600361D RID: 13853 RVA: 0x0004AA9B File Offset: 0x00048C9B
		public override void SetAxisZero(string name)
		{
			throw new Exception(" This is not possible to be called for standalone input. Please check your platform and code where this is called");
		}

		// Token: 0x0600361E RID: 13854 RVA: 0x0004AA9B File Offset: 0x00048C9B
		public override void SetAxis(string name, float value)
		{
			throw new Exception(" This is not possible to be called for standalone input. Please check your platform and code where this is called");
		}

		// Token: 0x0600361F RID: 13855 RVA: 0x0004AAA7 File Offset: 0x00048CA7
		public override Vector3 MousePosition()
		{
			return Input.mousePosition;
		}
	}
}
