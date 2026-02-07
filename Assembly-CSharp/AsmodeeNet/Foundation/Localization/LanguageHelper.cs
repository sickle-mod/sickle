using System;

namespace AsmodeeNet.Foundation.Localization
{
	// Token: 0x02000967 RID: 2407
	public static class LanguageHelper
	{
		// Token: 0x060040C1 RID: 16577 RVA: 0x000519F8 File Offset: 0x0004FBF8
		public static string ToXsdLanguage(this LocalizationManager.Language lang)
		{
			return lang.ToString().Replace("_", "-");
		}

		// Token: 0x060040C2 RID: 16578 RVA: 0x0015F098 File Offset: 0x0015D298
		public static LocalizationManager.Language LanguageFromXsdLanguage(string xsdLang)
		{
			if (string.IsNullOrEmpty(xsdLang))
			{
				return LocalizationManager.Language.unknown;
			}
			LocalizationManager.Language language;
			try
			{
				language = (LocalizationManager.Language)Enum.Parse(typeof(LocalizationManager.Language), xsdLang.Replace("-", "_"));
			}
			catch
			{
				language = LocalizationManager.Language.unknown;
			}
			return language;
		}
	}
}
