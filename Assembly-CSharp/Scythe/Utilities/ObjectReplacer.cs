using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scythe.Utilities
{
	// Token: 0x020001D3 RID: 467
	[ExecuteInEditMode]
	public class ObjectReplacer : MonoBehaviour
	{
		// Token: 0x06000D81 RID: 3457 RVA: 0x00085B54 File Offset: 0x00083D54
		public void Replace()
		{
			if (this.targetNameBeggining.Equals(""))
			{
				Debug.LogError("Empty string. Enter the name of the object to replace into targetNameBeggining variable.");
				return;
			}
			int i = 0;
			List<Transform> list = new List<Transform>();
			while (i < base.transform.childCount)
			{
				Transform child = base.transform.GetChild(i);
				if (child.name.StartsWith(this.targetNameBeggining))
				{
					list.Add(child);
				}
				i++;
			}
			while (list.Count > 0)
			{
				global::UnityEngine.Object.Instantiate<GameObject>(this.prefabToSpawn, list[0].position, this.prefabToSpawn.transform.rotation).transform.parent = list[0].parent;
				global::UnityEngine.Object.DestroyImmediate(list[0].gameObject);
				list.RemoveAt(0);
			}
			Debug.Log("Done. Changed " + i.ToString() + " objects.");
		}

		// Token: 0x04000ACB RID: 2763
		[SerializeField]
		private string targetNameBeggining;

		// Token: 0x04000ACC RID: 2764
		[SerializeField]
		private GameObject prefabToSpawn;
	}
}
