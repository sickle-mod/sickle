using System;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x020007BF RID: 1983
	public static class KeyCombinationChecker
	{
		// Token: 0x060038FC RID: 14588 RVA: 0x0004CA6A File Offset: 0x0004AC6A
		public static bool IsDebugKeyCombination()
		{
			return (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));
		}
	}
}
