using System;
using UnityEngine;

namespace Scythe.BoardPresenter
{
	// Token: 0x020001EC RID: 492
	public interface IDraggableObject
	{
		// Token: 0x06000E42 RID: 3650
		void OnDragBegin(Vector3 pivotPosition, float rodLength, float timeToSnap);

		// Token: 0x06000E43 RID: 3651
		void OnDragEnd(Vector3 position, float timeToLand, bool loadingToUnit);

		// Token: 0x06000E44 RID: 3652
		void PickUpAnimation(Vector3 pivotPosition, float rodLength, float timeToSnap);

		// Token: 0x06000E45 RID: 3653
		void PutDownAnimation(Vector3 position, float timeToLand, bool loadingToUnit);

		// Token: 0x06000E46 RID: 3654
		GameHexPresenter GetPosition();

		// Token: 0x06000E47 RID: 3655
		Vector3 GetDefaultPosition();

		// Token: 0x06000E48 RID: 3656
		bool SnapsTo(GameObject otherObject);
	}
}
