using System;
using System.Xml;

namespace AsmodeeNet.Foundation.Localization
{
	// Token: 0x0200096D RID: 2413
	public static class XliffUtility
	{
		// Token: 0x060040E8 RID: 16616 RVA: 0x00051B40 File Offset: 0x0004FD40
		public static LocalizationManager.Language GetXliffTargetLang(string xliffFilePath)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(xliffFilePath);
			return LanguageHelper.LanguageFromXsdLanguage(xmlDocument.GetElementsByTagName("file")[0].Attributes.GetNamedItem("target-language").Value);
		}

		// Token: 0x060040E9 RID: 16617 RVA: 0x00051B77 File Offset: 0x0004FD77
		public static LocalizationManager.Language GetXliffTargetLangFromXml(string xml)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			return LanguageHelper.LanguageFromXsdLanguage(xmlDocument.GetElementsByTagName("file")[0].Attributes.GetNamedItem("target-language").Value);
		}
	}
}
