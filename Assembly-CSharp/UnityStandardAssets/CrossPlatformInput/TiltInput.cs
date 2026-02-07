using System;
using UnityEngine;

namespace UnityStandardAssets.CrossPlatformInput
{
	// Token: 0x020006FA RID: 1786
	public class TiltInput : MonoBehaviour
	{
		// Token: 0x060035E1 RID: 13793 RVA: 0x0004A5E8 File Offset: 0x000487E8
		private void OnEnable()
		{
			if (this.mapping.type == TiltInput.AxisMapping.MappingType.NamedAxis)
			{
				this.m_SteerAxis = new CrossPlatformInputManager.VirtualAxis(this.mapping.axisName);
				CrossPlatformInputManager.RegisterVirtualAxis(this.m_SteerAxis);
			}
		}

		// Token: 0x060035E2 RID: 13794 RVA: 0x00140760 File Offset: 0x0013E960
		private void Update()
		{
			float num = 0f;
			if (Input.acceleration != Vector3.zero)
			{
				TiltInput.AxisOptions axisOptions = this.tiltAroundAxis;
				if (axisOptions != TiltInput.AxisOptions.ForwardAxis)
				{
					if (axisOptions == TiltInput.AxisOptions.SidewaysAxis)
					{
						num = Mathf.Atan2(Input.acceleration.z, -Input.acceleration.y) * 57.29578f + this.centreAngleOffset;
					}
				}
				else
				{
					num = Mathf.Atan2(Input.acceleration.x, -Input.acceleration.y) * 57.29578f + this.centreAngleOffset;
				}
			}
			float num2 = Mathf.InverseLerp(-this.fullTiltAngle, this.fullTiltAngle, num) * 2f - 1f;
			switch (this.mapping.type)
			{
			case TiltInput.AxisMapping.MappingType.NamedAxis:
				this.m_SteerAxis.Update(num2);
				return;
			case TiltInput.AxisMapping.MappingType.MousePositionX:
				CrossPlatformInputManager.SetVirtualMousePositionX(num2 * (float)Screen.width);
				return;
			case TiltInput.AxisMapping.MappingType.MousePositionY:
				CrossPlatformInputManager.SetVirtualMousePositionY(num2 * (float)Screen.width);
				return;
			case TiltInput.AxisMapping.MappingType.MousePositionZ:
				CrossPlatformInputManager.SetVirtualMousePositionZ(num2 * (float)Screen.width);
				return;
			default:
				return;
			}
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x0004A618 File Offset: 0x00048818
		private void OnDisable()
		{
			this.m_SteerAxis.Remove();
		}

		// Token: 0x04002771 RID: 10097
		public TiltInput.AxisMapping mapping;

		// Token: 0x04002772 RID: 10098
		public TiltInput.AxisOptions tiltAroundAxis;

		// Token: 0x04002773 RID: 10099
		public float fullTiltAngle = 25f;

		// Token: 0x04002774 RID: 10100
		public float centreAngleOffset;

		// Token: 0x04002775 RID: 10101
		private CrossPlatformInputManager.VirtualAxis m_SteerAxis;

		// Token: 0x020006FB RID: 1787
		public enum AxisOptions
		{
			// Token: 0x04002777 RID: 10103
			ForwardAxis,
			// Token: 0x04002778 RID: 10104
			SidewaysAxis
		}

		// Token: 0x020006FC RID: 1788
		[Serializable]
		public class AxisMapping
		{
			// Token: 0x04002779 RID: 10105
			public TiltInput.AxisMapping.MappingType type;

			// Token: 0x0400277A RID: 10106
			public string axisName;

			// Token: 0x020006FD RID: 1789
			public enum MappingType
			{
				// Token: 0x0400277C RID: 10108
				NamedAxis,
				// Token: 0x0400277D RID: 10109
				MousePositionX,
				// Token: 0x0400277E RID: 10110
				MousePositionY,
				// Token: 0x0400277F RID: 10111
				MousePositionZ
			}
		}
	}
}
