using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C9 RID: 201
[CreateAssetMenu(fileName = "ProperCameraZoom", menuName = "CameraZooms")]
public class HexesAmountZoom : ScriptableObject
{
	// Token: 0x04000506 RID: 1286
	public List<ZoomForHexesAmount> zoomDependentOnHexAmountHeight = new List<ZoomForHexesAmount>();

	// Token: 0x04000507 RID: 1287
	public List<ZoomForHexesAmount> zoomDependentOnHexAmountWidth = new List<ZoomForHexesAmount>();
}
