using System;
using UnityEngine;

namespace Scythe.BoardPresenter
{
	// Token: 0x020001ED RID: 493
	public interface ISeismograph
	{
		// Token: 0x06000E49 RID: 3657
		void OnQuakeDetected(Vector3 epicenter, float force, float radius);
	}
}
