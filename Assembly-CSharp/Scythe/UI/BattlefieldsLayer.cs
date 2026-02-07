using System;
using System.Collections.Generic;
using HoneyFramework;
using Scythe.BoardPresenter;
using UnityEngine;

namespace Scythe.UI
{
	// Token: 0x0200042C RID: 1068
	public class BattlefieldsLayer : MonoBehaviour
	{
		// Token: 0x060020C7 RID: 8391 RVA: 0x000C61E0 File Offset: 0x000C43E0
		public void Init()
		{
			this.fields.Clear();
			this.fieldMappings.Clear();
			foreach (object obj in base.transform)
			{
				global::UnityEngine.Object.Destroy(((Transform)obj).gameObject);
			}
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x000C6254 File Offset: 0x000C4454
		public void AddHex(Scythe.BoardPresenter.GameHexPresenter hex)
		{
			if (!this.fields.Contains(hex) && hex.GetGameHexLogic() != null)
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.battlefieldPrefab);
				gameObject.transform.SetParent(base.transform);
				this.fieldMappings.Add(gameObject.transform, hex);
				this.fields.Add(hex);
			}
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x000C62B4 File Offset: 0x000C44B4
		public void UpdateLayer()
		{
			foreach (Transform transform in this.fieldMappings.Keys)
			{
				transform.position = Camera.main.WorldToScreenPoint(HexCoordinates.HexToWorld3D(this.fieldMappings[transform].position));
			}
		}

		// Token: 0x060020CA RID: 8394 RVA: 0x000C632C File Offset: 0x000C452C
		public void Clear()
		{
			foreach (Scythe.BoardPresenter.GameHexPresenter gameHexPresenter in this.fields)
			{
				gameHexPresenter.SetFocus(false, HexMarkers.MarkerType.Battle, 0f, false);
			}
			this.fields.Clear();
			this.fieldMappings.Clear();
			foreach (object obj in base.transform)
			{
				global::UnityEngine.Object.Destroy(((Transform)obj).gameObject);
			}
		}

		// Token: 0x040016E9 RID: 5865
		public GameObject battlefieldPrefab;

		// Token: 0x040016EA RID: 5866
		private Dictionary<Transform, Scythe.BoardPresenter.GameHexPresenter> fieldMappings = new Dictionary<Transform, Scythe.BoardPresenter.GameHexPresenter>();

		// Token: 0x040016EB RID: 5867
		private HashSet<Scythe.BoardPresenter.GameHexPresenter> fields = new HashSet<Scythe.BoardPresenter.GameHexPresenter>();
	}
}
