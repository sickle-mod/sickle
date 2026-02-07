using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000111 RID: 273
[ExecuteInEditMode]
public class SFXEnumSetup : MonoBehaviour
{
	// Token: 0x060008DF RID: 2271 RVA: 0x00027EF0 File Offset: 0x000260F0
	public void SetupEnums()
	{
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x00027EF0 File Offset: 0x000260F0
	private void LoadSounds()
	{
	}

	// Token: 0x060008E1 RID: 2273 RVA: 0x0007A2B0 File Offset: 0x000784B0
	private static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
	{
		string[] array = searchPattern.Split('|', StringSplitOptions.None);
		List<string> list = new List<string>();
		foreach (string text in array)
		{
			list.AddRange(Directory.GetFiles(path, text, searchOption));
		}
		list.Sort();
		return list.ToArray();
	}

	// Token: 0x060008E2 RID: 2274 RVA: 0x0007A2FC File Offset: 0x000784FC
	public void GenerateEnum()
	{
		string text = "SoundEnum";
		string text2 = "Assets/Scripts/Sounds/SFXLabel.cs";
		File.WriteAllText(text2, string.Empty);
		using (StreamWriter streamWriter = new StreamWriter(text2))
		{
			streamWriter.WriteLine("public enum " + text);
			streamWriter.WriteLine("{");
			for (int i = 0; i < this.clips.Count; i++)
			{
				if (i != this.clips.Count - 1)
				{
					streamWriter.WriteLine("\t" + this.clips[i].name.ToString() + ",");
				}
				else
				{
					streamWriter.WriteLine("\t" + this.clips[i].name.ToString());
				}
			}
			streamWriter.WriteLine("}");
		}
	}

	// Token: 0x0400077F RID: 1919
	public WorldSFX worldSFX;

	// Token: 0x04000780 RID: 1920
	private List<AudioClip> clips = new List<AudioClip>();
}
