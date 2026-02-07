using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	// Token: 0x02000681 RID: 1665
	public class WaypointCircuit : MonoBehaviour
	{
		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x0600342B RID: 13355 RVA: 0x00048E67 File Offset: 0x00047067
		// (set) Token: 0x0600342C RID: 13356 RVA: 0x00048E6F File Offset: 0x0004706F
		public float Length { get; private set; }

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x0600342D RID: 13357 RVA: 0x00048E78 File Offset: 0x00047078
		public Transform[] Waypoints
		{
			get
			{
				return this.waypointList.items;
			}
		}

		// Token: 0x0600342E RID: 13358 RVA: 0x00048E85 File Offset: 0x00047085
		private void Awake()
		{
			if (this.Waypoints.Length > 1)
			{
				this.CachePositionsAndDistances();
			}
			this.numPoints = this.Waypoints.Length;
		}

		// Token: 0x0600342F RID: 13359 RVA: 0x00133F48 File Offset: 0x00132148
		public WaypointCircuit.RoutePoint GetRoutePoint(float dist)
		{
			Vector3 routePosition = this.GetRoutePosition(dist);
			return new WaypointCircuit.RoutePoint(routePosition, (this.GetRoutePosition(dist + 0.1f) - routePosition).normalized);
		}

		// Token: 0x06003430 RID: 13360 RVA: 0x00133F80 File Offset: 0x00132180
		public Vector3 GetRoutePosition(float dist)
		{
			int num = 0;
			if (this.Length == 0f)
			{
				this.Length = this.distances[this.distances.Length - 1];
			}
			dist = Mathf.Repeat(dist, this.Length);
			while (this.distances[num] < dist)
			{
				num++;
			}
			this.p1n = (num - 1 + this.numPoints) % this.numPoints;
			this.p2n = num;
			this.i = Mathf.InverseLerp(this.distances[this.p1n], this.distances[this.p2n], dist);
			if (this.smoothRoute)
			{
				this.p0n = (num - 2 + this.numPoints) % this.numPoints;
				this.p3n = (num + 1) % this.numPoints;
				this.p2n %= this.numPoints;
				this.P0 = this.points[this.p0n];
				this.P1 = this.points[this.p1n];
				this.P2 = this.points[this.p2n];
				this.P3 = this.points[this.p3n];
				return this.CatmullRom(this.P0, this.P1, this.P2, this.P3, this.i);
			}
			this.p1n = (num - 1 + this.numPoints) % this.numPoints;
			this.p2n = num;
			return Vector3.Lerp(this.points[this.p1n], this.points[this.p2n], this.i);
		}

		// Token: 0x06003431 RID: 13361 RVA: 0x00134128 File Offset: 0x00132328
		private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float i)
		{
			return 0.5f * (2f * p1 + (-p0 + p2) * i + (2f * p0 - 5f * p1 + 4f * p2 - p3) * i * i + (-p0 + 3f * p1 - 3f * p2 + p3) * i * i * i);
		}

		// Token: 0x06003432 RID: 13362 RVA: 0x001341F0 File Offset: 0x001323F0
		private void CachePositionsAndDistances()
		{
			this.points = new Vector3[this.Waypoints.Length + 1];
			this.distances = new float[this.Waypoints.Length + 1];
			float num = 0f;
			for (int i = 0; i < this.points.Length; i++)
			{
				Transform transform = this.Waypoints[i % this.Waypoints.Length];
				Transform transform2 = this.Waypoints[(i + 1) % this.Waypoints.Length];
				if (transform != null && transform2 != null)
				{
					Vector3 position = transform.position;
					Vector3 position2 = transform2.position;
					this.points[i] = this.Waypoints[i % this.Waypoints.Length].position;
					this.distances[i] = num;
					num += (position - position2).magnitude;
				}
			}
		}

		// Token: 0x06003433 RID: 13363 RVA: 0x00048EA6 File Offset: 0x000470A6
		private void OnDrawGizmos()
		{
			this.DrawGizmos(false);
		}

		// Token: 0x06003434 RID: 13364 RVA: 0x00048EAF File Offset: 0x000470AF
		private void OnDrawGizmosSelected()
		{
			this.DrawGizmos(true);
		}

		// Token: 0x06003435 RID: 13365 RVA: 0x001342D0 File Offset: 0x001324D0
		private void DrawGizmos(bool selected)
		{
			this.waypointList.circuit = this;
			if (this.Waypoints.Length > 1)
			{
				this.numPoints = this.Waypoints.Length;
				this.CachePositionsAndDistances();
				this.Length = this.distances[this.distances.Length - 1];
				Gizmos.color = (selected ? Color.yellow : new Color(1f, 1f, 0f, 0.5f));
				Vector3 vector = this.Waypoints[0].position;
				if (this.smoothRoute)
				{
					for (float num = 0f; num < this.Length; num += this.Length / this.editorVisualisationSubsteps)
					{
						Vector3 routePosition = this.GetRoutePosition(num + 1f);
						Gizmos.DrawLine(vector, routePosition);
						vector = routePosition;
					}
					Gizmos.DrawLine(vector, this.Waypoints[0].position);
					return;
				}
				for (int i = 0; i < this.Waypoints.Length; i++)
				{
					Vector3 position = this.Waypoints[(i + 1) % this.Waypoints.Length].position;
					Gizmos.DrawLine(vector, position);
					vector = position;
				}
			}
		}

		// Token: 0x04002478 RID: 9336
		public WaypointCircuit.WaypointList waypointList = new WaypointCircuit.WaypointList();

		// Token: 0x04002479 RID: 9337
		[SerializeField]
		private bool smoothRoute = true;

		// Token: 0x0400247A RID: 9338
		private int numPoints;

		// Token: 0x0400247B RID: 9339
		private Vector3[] points;

		// Token: 0x0400247C RID: 9340
		private float[] distances;

		// Token: 0x0400247D RID: 9341
		public float editorVisualisationSubsteps = 100f;

		// Token: 0x0400247F RID: 9343
		private int p0n;

		// Token: 0x04002480 RID: 9344
		private int p1n;

		// Token: 0x04002481 RID: 9345
		private int p2n;

		// Token: 0x04002482 RID: 9346
		private int p3n;

		// Token: 0x04002483 RID: 9347
		private float i;

		// Token: 0x04002484 RID: 9348
		private Vector3 P0;

		// Token: 0x04002485 RID: 9349
		private Vector3 P1;

		// Token: 0x04002486 RID: 9350
		private Vector3 P2;

		// Token: 0x04002487 RID: 9351
		private Vector3 P3;

		// Token: 0x02000682 RID: 1666
		[Serializable]
		public class WaypointList
		{
			// Token: 0x04002488 RID: 9352
			public WaypointCircuit circuit;

			// Token: 0x04002489 RID: 9353
			public Transform[] items = new Transform[0];
		}

		// Token: 0x02000683 RID: 1667
		public struct RoutePoint
		{
			// Token: 0x06003438 RID: 13368 RVA: 0x00048EF1 File Offset: 0x000470F1
			public RoutePoint(Vector3 position, Vector3 direction)
			{
				this.position = position;
				this.direction = direction;
			}

			// Token: 0x0400248A RID: 9354
			public Vector3 position;

			// Token: 0x0400248B RID: 9355
			public Vector3 direction;
		}
	}
}
