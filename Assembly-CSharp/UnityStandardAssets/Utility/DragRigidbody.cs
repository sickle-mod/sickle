using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	// Token: 0x02000666 RID: 1638
	public class DragRigidbody : MonoBehaviour
	{
		// Token: 0x060033BF RID: 13247 RVA: 0x00132F00 File Offset: 0x00131100
		private void Update()
		{
			if (!Input.GetMouseButtonDown(0))
			{
				return;
			}
			Camera camera = this.FindCamera();
			RaycastHit raycastHit = default(RaycastHit);
			if (!Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition).origin, camera.ScreenPointToRay(Input.mousePosition).direction, out raycastHit, 100f, -5))
			{
				return;
			}
			if (!raycastHit.rigidbody || raycastHit.rigidbody.isKinematic)
			{
				return;
			}
			if (!this.m_SpringJoint)
			{
				GameObject gameObject = new GameObject("Rigidbody dragger");
				Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
				this.m_SpringJoint = gameObject.AddComponent<SpringJoint>();
				rigidbody.isKinematic = true;
			}
			this.m_SpringJoint.transform.position = raycastHit.point;
			this.m_SpringJoint.anchor = Vector3.zero;
			this.m_SpringJoint.spring = 50f;
			this.m_SpringJoint.damper = 5f;
			this.m_SpringJoint.maxDistance = 0.2f;
			this.m_SpringJoint.connectedBody = raycastHit.rigidbody;
			base.StartCoroutine("DragObject", raycastHit.distance);
		}

		// Token: 0x060033C0 RID: 13248 RVA: 0x00048ADC File Offset: 0x00046CDC
		private IEnumerator DragObject(float distance)
		{
			float oldDrag = this.m_SpringJoint.connectedBody.drag;
			float oldAngularDrag = this.m_SpringJoint.connectedBody.angularDrag;
			this.m_SpringJoint.connectedBody.drag = 10f;
			this.m_SpringJoint.connectedBody.angularDrag = 5f;
			Camera mainCamera = this.FindCamera();
			while (Input.GetMouseButton(0))
			{
				Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
				this.m_SpringJoint.transform.position = ray.GetPoint(distance);
				yield return null;
			}
			if (this.m_SpringJoint.connectedBody)
			{
				this.m_SpringJoint.connectedBody.drag = oldDrag;
				this.m_SpringJoint.connectedBody.angularDrag = oldAngularDrag;
				this.m_SpringJoint.connectedBody = null;
			}
			yield break;
		}

		// Token: 0x060033C1 RID: 13249 RVA: 0x00048AF2 File Offset: 0x00046CF2
		private Camera FindCamera()
		{
			if (base.GetComponent<Camera>())
			{
				return base.GetComponent<Camera>();
			}
			return Camera.main;
		}

		// Token: 0x04002402 RID: 9218
		private const float k_Spring = 50f;

		// Token: 0x04002403 RID: 9219
		private const float k_Damper = 5f;

		// Token: 0x04002404 RID: 9220
		private const float k_Drag = 10f;

		// Token: 0x04002405 RID: 9221
		private const float k_AngularDrag = 5f;

		// Token: 0x04002406 RID: 9222
		private const float k_Distance = 0.2f;

		// Token: 0x04002407 RID: 9223
		private const bool k_AttachToCenterOfMass = false;

		// Token: 0x04002408 RID: 9224
		private SpringJoint m_SpringJoint;
	}
}
