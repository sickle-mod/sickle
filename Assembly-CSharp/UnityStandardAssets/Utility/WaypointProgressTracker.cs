using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	// Token: 0x02000684 RID: 1668
	public class WaypointProgressTracker : MonoBehaviour
	{
		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06003439 RID: 13369 RVA: 0x00048F01 File Offset: 0x00047101
		// (set) Token: 0x0600343A RID: 13370 RVA: 0x00048F09 File Offset: 0x00047109
		public WaypointCircuit.RoutePoint targetPoint { get; private set; }

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x0600343B RID: 13371 RVA: 0x00048F12 File Offset: 0x00047112
		// (set) Token: 0x0600343C RID: 13372 RVA: 0x00048F1A File Offset: 0x0004711A
		public WaypointCircuit.RoutePoint speedPoint { get; private set; }

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x0600343D RID: 13373 RVA: 0x00048F23 File Offset: 0x00047123
		// (set) Token: 0x0600343E RID: 13374 RVA: 0x00048F2B File Offset: 0x0004712B
		public WaypointCircuit.RoutePoint progressPoint { get; private set; }

		// Token: 0x0600343F RID: 13375 RVA: 0x00048F34 File Offset: 0x00047134
		private void Start()
		{
			if (this.target == null)
			{
				this.target = new GameObject(base.name + " Waypoint Target").transform;
			}
			this.Reset();
		}

		// Token: 0x06003440 RID: 13376 RVA: 0x001343E4 File Offset: 0x001325E4
		public void Reset()
		{
			this.progressDistance = 0f;
			this.progressNum = 0;
			if (this.progressStyle == WaypointProgressTracker.ProgressStyle.PointToPoint)
			{
				this.target.position = this.circuit.Waypoints[this.progressNum].position;
				this.target.rotation = this.circuit.Waypoints[this.progressNum].rotation;
			}
		}

		// Token: 0x06003441 RID: 13377 RVA: 0x00134450 File Offset: 0x00132650
		private void Update()
		{
			if (this.progressStyle == WaypointProgressTracker.ProgressStyle.SmoothAlongRoute)
			{
				if (Time.deltaTime > 0f)
				{
					this.speed = Mathf.Lerp(this.speed, (this.lastPosition - base.transform.position).magnitude / Time.deltaTime, Time.deltaTime);
				}
				this.target.position = this.circuit.GetRoutePoint(this.progressDistance + this.lookAheadForTargetOffset + this.lookAheadForTargetFactor * this.speed).position;
				this.target.rotation = Quaternion.LookRotation(this.circuit.GetRoutePoint(this.progressDistance + this.lookAheadForSpeedOffset + this.lookAheadForSpeedFactor * this.speed).direction);
				this.progressPoint = this.circuit.GetRoutePoint(this.progressDistance);
				Vector3 vector = this.progressPoint.position - base.transform.position;
				if (Vector3.Dot(vector, this.progressPoint.direction) < 0f)
				{
					this.progressDistance += vector.magnitude * 0.5f;
				}
				this.lastPosition = base.transform.position;
				return;
			}
			if ((this.target.position - base.transform.position).magnitude < this.pointToPointThreshold)
			{
				this.progressNum = (this.progressNum + 1) % this.circuit.Waypoints.Length;
			}
			this.target.position = this.circuit.Waypoints[this.progressNum].position;
			this.target.rotation = this.circuit.Waypoints[this.progressNum].rotation;
			this.progressPoint = this.circuit.GetRoutePoint(this.progressDistance);
			Vector3 vector2 = this.progressPoint.position - base.transform.position;
			if (Vector3.Dot(vector2, this.progressPoint.direction) < 0f)
			{
				this.progressDistance += vector2.magnitude;
			}
			this.lastPosition = base.transform.position;
		}

		// Token: 0x06003442 RID: 13378 RVA: 0x00134694 File Offset: 0x00132894
		private void OnDrawGizmos()
		{
			if (Application.isPlaying)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawLine(base.transform.position, this.target.position);
				Gizmos.DrawWireSphere(this.circuit.GetRoutePosition(this.progressDistance), 1f);
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine(this.target.position, this.target.position + this.target.forward);
			}
		}

		// Token: 0x0400248C RID: 9356
		[SerializeField]
		private WaypointCircuit circuit;

		// Token: 0x0400248D RID: 9357
		[SerializeField]
		private float lookAheadForTargetOffset = 5f;

		// Token: 0x0400248E RID: 9358
		[SerializeField]
		private float lookAheadForTargetFactor = 0.1f;

		// Token: 0x0400248F RID: 9359
		[SerializeField]
		private float lookAheadForSpeedOffset = 10f;

		// Token: 0x04002490 RID: 9360
		[SerializeField]
		private float lookAheadForSpeedFactor = 0.2f;

		// Token: 0x04002491 RID: 9361
		[SerializeField]
		private WaypointProgressTracker.ProgressStyle progressStyle;

		// Token: 0x04002492 RID: 9362
		[SerializeField]
		private float pointToPointThreshold = 4f;

		// Token: 0x04002496 RID: 9366
		public Transform target;

		// Token: 0x04002497 RID: 9367
		private float progressDistance;

		// Token: 0x04002498 RID: 9368
		private int progressNum;

		// Token: 0x04002499 RID: 9369
		private Vector3 lastPosition;

		// Token: 0x0400249A RID: 9370
		private float speed;

		// Token: 0x02000685 RID: 1669
		public enum ProgressStyle
		{
			// Token: 0x0400249C RID: 9372
			SmoothAlongRoute,
			// Token: 0x0400249D RID: 9373
			PointToPoint
		}
	}
}
