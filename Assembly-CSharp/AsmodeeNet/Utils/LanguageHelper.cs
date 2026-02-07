using System;
using UnityEngine;

namespace AsmodeeNet.Utils
{
	// Token: 0x0200084A RID: 2122
	public class LanguageHelper
	{
		// Token: 0x06003BFB RID: 15355 RVA: 0x00154258 File Offset: 0x00152458
		public static string Get2LetterISOCodeFromSystemLanguage(SystemLanguage language)
		{
			string text = "en";
			switch (language)
			{
			case SystemLanguage.Afrikaans:
				text = "af";
				break;
			case SystemLanguage.Arabic:
				text = "ar";
				break;
			case SystemLanguage.Basque:
				text = "eu";
				break;
			case SystemLanguage.Belarusian:
				text = "by";
				break;
			case SystemLanguage.Bulgarian:
				text = "bg";
				break;
			case SystemLanguage.Catalan:
				text = "ca";
				break;
			case SystemLanguage.Chinese:
				text = "zh";
				break;
			case SystemLanguage.Czech:
				text = "cs";
				break;
			case SystemLanguage.Danish:
				text = "da";
				break;
			case SystemLanguage.Dutch:
				text = "nl";
				break;
			case SystemLanguage.English:
				text = "en";
				break;
			case SystemLanguage.Estonian:
				text = "et";
				break;
			case SystemLanguage.Faroese:
				text = "fo";
				break;
			case SystemLanguage.Finnish:
				text = "fi";
				break;
			case SystemLanguage.French:
				text = "fr";
				break;
			case SystemLanguage.German:
				text = "de";
				break;
			case SystemLanguage.Greek:
				text = "el";
				break;
			case SystemLanguage.Hebrew:
				text = "iw";
				break;
			case SystemLanguage.Hungarian:
				text = "hu";
				break;
			case SystemLanguage.Icelandic:
				text = "is";
				break;
			case SystemLanguage.Indonesian:
				text = "in";
				break;
			case SystemLanguage.Italian:
				text = "it";
				break;
			case SystemLanguage.Japanese:
				text = "ja";
				break;
			case SystemLanguage.Korean:
				text = "ko";
				break;
			case SystemLanguage.Latvian:
				text = "lv";
				break;
			case SystemLanguage.Lithuanian:
				text = "lt";
				break;
			case SystemLanguage.Norwegian:
				text = "no";
				break;
			case SystemLanguage.Polish:
				text = "pl";
				break;
			case SystemLanguage.Portuguese:
				text = "pt";
				break;
			case SystemLanguage.Romanian:
				text = "ro";
				break;
			case SystemLanguage.Russian:
				text = "ru";
				break;
			case SystemLanguage.SerboCroatian:
				text = "sh";
				break;
			case SystemLanguage.Slovak:
				text = "sk";
				break;
			case SystemLanguage.Slovenian:
				text = "sl";
				break;
			case SystemLanguage.Spanish:
				text = "es";
				break;
			case SystemLanguage.Swedish:
				text = "sv";
				break;
			case SystemLanguage.Thai:
				text = "th";
				break;
			case SystemLanguage.Turkish:
				text = "tr";
				break;
			case SystemLanguage.Ukrainian:
				text = "uk";
				break;
			case SystemLanguage.Vietnamese:
				text = "vi";
				break;
			case SystemLanguage.Unknown:
				text = "en";
				break;
			}
			return text;
		}
	}
}
