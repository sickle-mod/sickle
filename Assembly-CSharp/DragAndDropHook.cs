using System;
using System.Collections.Generic;
using HoneyFramework;
using Scythe.BoardPresenter;
using Scythe.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000074 RID: 116
public class DragAndDropHook : MonoBehaviour
{
	// Token: 0x060003E8 RID: 1000 RVA: 0x0002A6F5 File Offset: 0x000288F5
	private void OnDestroy()
	{
		CameraControler.CameraDragMovementBlocked = false;
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x00062C18 File Offset: 0x00060E18
	public void Attach(GameObject objectToMove, IDraggableObject objectLogic)
	{
		this.objectToMove = objectToMove;
		this.objectLogic = objectLogic;
		this.currentDistanceToPivot = Vector3.Distance(this.PivotPosition(), objectToMove.transform.position);
		CameraControler.CameraDragMovementBlocked = true;
		if (objectLogic == null)
		{
			objectLogic = objectToMove.transform.GetComponent<IDraggableObject>();
		}
		if (objectLogic != null)
		{
			this.objectInitialRotation = objectToMove.transform.eulerAngles.y;
			objectLogic.OnDragBegin(this.PivotPosition(), this.rodLength, 0f);
			this.canSimulatePendulum = true;
			this.isSnappingToPivot = true;
			GameController.Instance.cameraControler.SetEdgeScrollingEnabled(true);
		}
	}

	// Token: 0x060003EA RID: 1002 RVA: 0x00062CB4 File Offset: 0x00060EB4
	public void Detach(Vector3 position, bool loadingToUnit = false)
	{
		if (this.objectToMove == null)
		{
			return;
		}
		this.isSnappingToPivot = false;
		this.canSimulatePendulum = false;
		this.isCursorOnObject = false;
		this.objectStartingPositionSet = false;
		this.objectLogic.OnDragEnd(position, this.timeToLand, loadingToUnit);
		this.objectLogic = null;
		this.objectToMove = null;
		this.currentVelocity = Vector3.zero;
		this.isMouseAboveDragAndDropPanel = false;
		CameraControler.CameraDragMovementBlocked = false;
		GameController.Instance.cameraControler.SetEdgeScrollingEnabled(false);
	}

	// Token: 0x060003EB RID: 1003 RVA: 0x0002A6FD File Offset: 0x000288FD
	public void Break()
	{
		this.Detach(this.objectLogic.GetDefaultPosition(), false);
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x0002A711 File Offset: 0x00028911
	private void Update()
	{
		this.isMouseAboveDragAndDropPanel = false;
		if (this.CanSimulatePendulum())
		{
			this.UpdateObjectUnderTheCursor();
			this.UpdatePendulum();
		}
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x0002A72E File Offset: 0x0002892E
	private void LateUpdate()
	{
		if (this.CanSimulatePendulum() && !this.isSnappingToPivot)
		{
			this.CorrectObjectPosition(this.PivotPosition(), this.objectToMove.transform.position);
		}
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x0002A75C File Offset: 0x0002895C
	private void Init()
	{
		this.objectStartingPosition = this.objectToMove.transform.position;
		this.objectStartingPositionSet = true;
		this.currentTime = Time.time;
		this.PendulumInit();
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x0002A78C File Offset: 0x0002898C
	private bool CanSimulatePendulum()
	{
		return this.objectToMove != null && this.canSimulatePendulum;
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x0002A7A4 File Offset: 0x000289A4
	private void ResetPendulumPosition()
	{
		if (this.objectStartingPositionSet)
		{
			this.MoveBob(this.objectStartingPosition);
			return;
		}
		this.PendulumInit();
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x0002A7C1 File Offset: 0x000289C1
	private void ResetPendulumForces()
	{
		this.currentVelocity = Vector3.zero;
		this.currentStatePosition = this.objectToMove.transform.position;
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x0002A7E4 File Offset: 0x000289E4
	private void PendulumInit()
	{
		this.ResetPendulumForces();
	}

	// Token: 0x060003F3 RID: 1011 RVA: 0x0002A7EC File Offset: 0x000289EC
	private void MoveBob(Vector3 resetBobPosition)
	{
		this.objectToMove.transform.position = resetBobPosition;
		this.currentStatePosition = resetBobPosition;
	}

	// Token: 0x060003F4 RID: 1012 RVA: 0x00062D38 File Offset: 0x00060F38
	private void UpdatePendulum()
	{
		if (this.isSnappingToPivot)
		{
			Vector3 vector = this.PivotPosition();
			this.UpdateObjectRotation(vector);
			float num = (this.isSnappingToPivot ? this.currentDistanceToPivot : Vector3.Distance(vector, this.objectToMove.transform.position));
			num -= this.snapSpeed * Time.deltaTime;
			if (num <= this.rodLength)
			{
				this.Init();
				this.ResetPendulumPosition();
				num = this.rodLength;
				this.isSnappingToPivot = false;
			}
			this.objectToMove.transform.position = vector + (this.objectToMove.transform.position - vector).normalized * num;
			this.currentDistanceToPivot = num;
			return;
		}
		float num2 = Time.time - this.currentTime;
		this.currentTime = Time.time;
		this.accumulator += num2;
		while (this.accumulator >= this.dt)
		{
			this.previousStatePosition = this.currentStatePosition;
			this.currentStatePosition = this.PendulumUpdate(this.currentStatePosition, this.dt);
			this.accumulator -= this.dt;
			this.t += this.dt;
		}
		float num3 = this.accumulator / this.dt;
		Vector3 vector2 = this.currentStatePosition * num3 + this.previousStatePosition * (1f - num3);
		this.objectToMove.transform.position = vector2;
	}

	// Token: 0x060003F5 RID: 1013 RVA: 0x00062EC4 File Offset: 0x000610C4
	private Vector3 PendulumUpdate(Vector3 currentStatePosition, float deltaTime)
	{
		Vector3 vector = this.PivotPosition();
		Vector3 vector2 = this.currentStatePosition;
		this.UpdateObjectRotation(vector);
		this.gravityForce = this.mass * Physics.gravity.magnitude;
		this.gravityDirection = Physics.gravity.normalized;
		this.currentVelocity += this.gravityDirection * this.gravityForce * deltaTime;
		Vector3 vector3 = this.currentVelocity * deltaTime;
		float num = Vector3.Distance(vector, vector2 + vector3);
		if (num > this.rodLength || Mathf.Approximately(num, this.rodLength))
		{
			this.tensionDirection = (vector - vector2).normalized;
			this.pendulumSideDirection = Quaternion.Euler(0f, 90f, 0f) * this.tensionDirection;
			this.pendulumSideDirection.Scale(new Vector3(1f, 0f, 1f));
			this.pendulumSideDirection.Normalize();
			this.tangentDirection = (-1f * Vector3.Cross(this.tensionDirection, this.pendulumSideDirection)).normalized;
			float num2 = Vector3.Angle(vector2 - vector, this.gravityDirection);
			this.tensionForce = this.mass * Physics.gravity.magnitude * Mathf.Cos(0.017453292f * num2);
			float num3 = this.mass * Mathf.Pow(this.currentVelocity.magnitude, 2f) / this.rodLength;
			this.tensionForce += num3;
			this.currentVelocity += this.tensionDirection * this.tensionForce * deltaTime;
			this.currentVelocity += -this.damping * (this.gravityForce * this.gravityDirection + this.tensionForce * this.tensionDirection) * deltaTime;
		}
		Vector3 vector4 = Vector3.zero;
		vector4 += this.currentVelocity * deltaTime;
		float num4 = Vector3.Distance(vector, currentStatePosition + vector4);
		return this.GetPointOnLine(vector, currentStatePosition + vector4, (num4 <= this.rodLength) ? num4 : this.rodLength);
	}

	// Token: 0x060003F6 RID: 1014 RVA: 0x00063134 File Offset: 0x00061334
	private void UpdateObjectRotation(Vector3 pivotPosition)
	{
		this.objectToMove.transform.LookAt(pivotPosition, -Vector3.forward);
		this.objectToMove.transform.Rotate(new Vector3(90f, 0f, 0f));
		this.objectToMove.transform.Rotate(new Vector3(0f, this.objectInitialRotation, 0f));
	}

	// Token: 0x060003F7 RID: 1015 RVA: 0x000631A8 File Offset: 0x000613A8
	private void CorrectObjectPosition(Vector3 pivotPosition, Vector3 objectPosition)
	{
		this.UpdateObjectRotation(pivotPosition);
		Vector3 vector = pivotPosition - objectPosition;
		Vector3 vector2 = vector - vector.normalized * this.rodLength;
		this.objectToMove.transform.position = this.objectToMove.transform.position + vector2;
		this.currentStatePosition = this.objectToMove.transform.position;
	}

	// Token: 0x060003F8 RID: 1016 RVA: 0x0002A806 File Offset: 0x00028A06
	private Vector3 GetPointOnLine(Vector3 start, Vector3 end, float distanceFromStart)
	{
		return start + distanceFromStart * Vector3.Normalize(end - start);
	}

	// Token: 0x060003F9 RID: 1017 RVA: 0x0002A820 File Offset: 0x00028A20
	public void SetMovingActive(bool active)
	{
		this.canPivotMove = active;
	}

	// Token: 0x060003FA RID: 1018 RVA: 0x0006321C File Offset: 0x0006141C
	private Vector3 PivotPosition()
	{
		if (this.canPivotMove)
		{
			GameObject objectUnderCursor = this.GetObjectUnderCursor();
			if (objectUnderCursor != null && this.objectLogic.SnapsTo(objectUnderCursor))
			{
				this.lastPivotPosition = objectUnderCursor.transform.position + new Vector3(0f, this.pivotHeight * 2f, 0f);
				if (!this.isCursorOnObject)
				{
					this.isCursorOnObject = true;
					this.isSnappingToPivot = true;
					this.currentDistanceToPivot = Vector3.Distance(this.lastPivotPosition, this.objectToMove.transform.position);
				}
			}
			else
			{
				Vector3 cursorWorldPosition = this.GetCursorWorldPosition();
				this.lastPivotPosition = cursorWorldPosition + this.cursorOffset * (Camera.main.transform.position - cursorWorldPosition).normalized + new Vector3(0f, this.pivotHeight, 0f);
				if (this.isCursorOnObject)
				{
					this.isCursorOnObject = false;
					this.isSnappingToPivot = true;
					this.currentDistanceToPivot = Vector3.Distance(this.lastPivotPosition, this.objectToMove.transform.position);
				}
			}
		}
		return this.lastPivotPosition;
	}

	// Token: 0x060003FB RID: 1019 RVA: 0x0002A829 File Offset: 0x00028A29
	public bool IsSomethingDragged()
	{
		return this.objectToMove != null;
	}

	// Token: 0x060003FC RID: 1020 RVA: 0x0002A837 File Offset: 0x00028A37
	public bool CursorSnapsToObject()
	{
		return (this.isCursorOnObject || this.IsCursorOnDragAndDropPanel()) && this.objectLogic.SnapsTo(this.GetObjectUnderCursor());
	}

	// Token: 0x060003FD RID: 1021 RVA: 0x0002A85C File Offset: 0x00028A5C
	public bool IsCursorOnDragAndDropPanel()
	{
		return this.isMouseAboveDragAndDropPanel;
	}

	// Token: 0x060003FE RID: 1022 RVA: 0x0002A864 File Offset: 0x00028A64
	public GameObject GetDraggedObject()
	{
		return this.objectToMove;
	}

	// Token: 0x060003FF RID: 1023 RVA: 0x0002A86C File Offset: 0x00028A6C
	public IDraggableObject GetDraggedObjectLogic()
	{
		return this.objectLogic;
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x00063354 File Offset: 0x00061554
	private void UpdateObjectUnderTheCursor()
	{
		List<RaycastResult> list = new List<RaycastResult>();
		this.dragAndDropRaycaster.Raycast(new PointerEventData(null)
		{
			position = Input.mousePosition
		}, list);
		this.isMouseAboveDragAndDropPanel = list.Count != 0;
		GameObject gameObject = null;
		if (this.IsCursorOnDragAndDropPanel() && this.dragAndDropPanel.IsPanelVisible())
		{
			gameObject = this.dragAndDropPanel.GetUnit().gameObject;
		}
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit raycastHit;
		if (gameObject == null && Physics.Raycast(ray, out raycastHit, 100f, LayerMask.GetMask(new string[] { "Units" })))
		{
			gameObject = raycastHit.collider.gameObject;
		}
		this.unitUnderCursor = gameObject;
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x0002A874 File Offset: 0x00028A74
	private GameObject GetObjectUnderCursor()
	{
		return this.unitUnderCursor;
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x00063410 File Offset: 0x00061610
	private Vector3 GetCursorWorldPosition()
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, 100f, LayerMask.GetMask(new string[] { "Hex2d" }) | LayerMask.GetMask(new string[] { "CameraPlane" })))
		{
			return raycastHit.point;
		}
		return -Vector3.one;
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x0002A87C File Offset: 0x00028A7C
	public HooksResourcesModule GetResourceFocusDetector()
	{
		return this.resourceFocusDetector;
	}

	// Token: 0x06000404 RID: 1028 RVA: 0x0002A884 File Offset: 0x00028A84
	public DragAndDropPanel GetDragAndDropPanel()
	{
		return this.dragAndDropPanel;
	}

	// Token: 0x06000405 RID: 1029 RVA: 0x0002A88C File Offset: 0x00028A8C
	private void Clear()
	{
		this.objectToMove = null;
		this.objectLogic = null;
		CameraControler.CameraMovementBlocked = false;
		this.canSimulatePendulum = false;
		this.isSnappingToPivot = false;
		this.canPivotMove = true;
		this.isCursorOnObject = false;
	}

	// Token: 0x0400035A RID: 858
	[SerializeField]
	private float snapSpeed = 20f;

	// Token: 0x0400035B RID: 859
	[SerializeField]
	private float timeToLand = 0.3f;

	// Token: 0x0400035C RID: 860
	[SerializeField]
	private float pivotHeight = 1f;

	// Token: 0x0400035D RID: 861
	[SerializeField]
	private float rodLength = 1f;

	// Token: 0x0400035E RID: 862
	private float currentDistanceToPivot;

	// Token: 0x0400035F RID: 863
	[SerializeField]
	[Range(0f, 1f)]
	private float damping = 0.7f;

	// Token: 0x04000360 RID: 864
	[SerializeField]
	public float mass = 1f;

	// Token: 0x04000361 RID: 865
	private Vector3 lastPivotPosition;

	// Token: 0x04000362 RID: 866
	private float cursorOffset = 5f;

	// Token: 0x04000363 RID: 867
	private float objectInitialRotation;

	// Token: 0x04000364 RID: 868
	private bool canSimulatePendulum;

	// Token: 0x04000365 RID: 869
	private bool canPivotMove = true;

	// Token: 0x04000366 RID: 870
	private bool isSnappingToPivot;

	// Token: 0x04000367 RID: 871
	private bool isCursorOnObject;

	// Token: 0x04000368 RID: 872
	[SerializeField]
	private HooksResourcesModule resourceFocusDetector;

	// Token: 0x04000369 RID: 873
	[SerializeField]
	private GraphicRaycaster dragAndDropRaycaster;

	// Token: 0x0400036A RID: 874
	private bool isMouseAboveDragAndDropPanel;

	// Token: 0x0400036B RID: 875
	private GameObject unitUnderCursor;

	// Token: 0x0400036C RID: 876
	[SerializeField]
	private DragAndDropPanel dragAndDropPanel;

	// Token: 0x0400036D RID: 877
	private GameObject objectToMove;

	// Token: 0x0400036E RID: 878
	private IDraggableObject objectLogic;

	// Token: 0x0400036F RID: 879
	private Vector3 objectStartingPosition;

	// Token: 0x04000370 RID: 880
	private bool objectStartingPositionSet;

	// Token: 0x04000371 RID: 881
	private Vector3 gravityDirection;

	// Token: 0x04000372 RID: 882
	private Vector3 tensionDirection;

	// Token: 0x04000373 RID: 883
	private Vector3 tangentDirection;

	// Token: 0x04000374 RID: 884
	private Vector3 pendulumSideDirection;

	// Token: 0x04000375 RID: 885
	private float tensionForce;

	// Token: 0x04000376 RID: 886
	private float gravityForce;

	// Token: 0x04000377 RID: 887
	private float t;

	// Token: 0x04000378 RID: 888
	private float dt = 0.01f;

	// Token: 0x04000379 RID: 889
	private float currentTime;

	// Token: 0x0400037A RID: 890
	private float accumulator;

	// Token: 0x0400037B RID: 891
	private Vector3 currentVelocity;

	// Token: 0x0400037C RID: 892
	private Vector3 currentStatePosition;

	// Token: 0x0400037D RID: 893
	private Vector3 previousStatePosition;
}
