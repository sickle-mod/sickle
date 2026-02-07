using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Common.GameSaves
{
	// Token: 0x0200019D RID: 413
	public class SaveMetadata
	{
		// Token: 0x06000C20 RID: 3104 RVA: 0x0008058C File Offset: 0x0007E78C
		public static string Serialize(SaveMetadata details)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaveMetadata));
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
			{
				Indent = false
			};
			xmlWriterSettings.OmitXmlDeclaration = true;
			XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
			xmlSerializerNamespaces.Add("", "");
			string text;
			using (StringWriter stringWriter = new StringWriter())
			{
				using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
				{
					xmlSerializer.Serialize(xmlWriter, details, xmlSerializerNamespaces);
					text = stringWriter.ToString();
				}
			}
			return text;
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x0008062C File Offset: 0x0007E82C
		public static SaveMetadata Deserialize(string data)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaveMetadata));
			SaveMetadata saveMetadata;
			using (StringReader stringReader = new StringReader(data))
			{
				saveMetadata = (SaveMetadata)xmlSerializer.Deserialize(stringReader);
			}
			return saveMetadata;
		}

		// Token: 0x040009A7 RID: 2471
		public int SaveSlotId;

		// Token: 0x040009A8 RID: 2472
		public string SaveName;

		// Token: 0x040009A9 RID: 2473
		public DateTime LastWriteDate;

		// Token: 0x040009AA RID: 2474
		public string SaveHash;

		// Token: 0x040009AB RID: 2475
		public string GameId;
	}
}
