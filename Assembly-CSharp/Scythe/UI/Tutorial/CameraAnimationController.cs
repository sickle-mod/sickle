using System;
using System.Linq;
using Scythe.GameLogic;
using Scythe.Utilities;
using UnityEngine;

namespace Scythe.UI.Tutorial
{
	// Token: 0x020004FE RID: 1278
	public class CameraAnimationController : SingletonMono<CameraAnimationController>
	{
		// Token: 0x060028EC RID: 10476 RVA: 0x000429A3 File Offset: 0x00040BA3
		public void AnimateToAllUnits(params Unit[] units)
		{
			this.AnimateToAllHexes(units.Select((Unit u) => u.position).ToArray<GameHex>());
		}

		// Token: 0x060028ED RID: 10477 RVA: 0x000EB9A0 File Offset: 0x000E9BA0
		public void AnimateToAllHexes(params GameHex[] hexes)
		{
			Vector3[] array = hexes.Select((GameHex h) => GameController.Instance.GetGameHexPresenter(h).GetWorldPosition()).ToArray<Vector3>();
			Vector3 vector = AnimateCamera.Instance.CalculateCenterOfHexes(array);
			float num = AnimateCamera.Instance.CalculateZoomToShowGivenHexes(hexes);
			ShowEnemyMoves.Instance.AnimateCamToShowAllHexes(vector, num);
		}
	}
}
