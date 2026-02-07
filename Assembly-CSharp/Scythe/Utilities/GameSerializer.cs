using System;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Scythe.Utilities
{
	// Token: 0x020001D2 RID: 466
	public static class GameSerializer
	{
		// Token: 0x06000D78 RID: 3448 RVA: 0x000858CC File Offset: 0x00083ACC
		public static string Serialize<T>(T objectToSerialize)
		{
			if (objectToSerialize == null)
			{
				return "";
			}
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
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
					xmlSerializer.Serialize(xmlWriter, objectToSerialize, xmlSerializerNamespaces);
					text = stringWriter.ToString();
				}
			}
			return text;
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x00085980 File Offset: 0x00083B80
		public static T DeserializeObject<T>(string serializedObject)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			T t;
			using (StringReader stringReader = new StringReader(serializedObject))
			{
				t = (T)((object)xmlSerializer.Deserialize(stringReader));
			}
			return t;
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x000859D0 File Offset: 0x00083BD0
		public static string Serialize<T>(string filename, T objectToSerialize)
		{
			string text = "";
			if (objectToSerialize == null)
			{
				return text;
			}
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
			{
				Indent = false
			};
			xmlWriterSettings.OmitXmlDeclaration = true;
			XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
			xmlSerializerNamespaces.Add("", "");
			using (StringWriter stringWriter = new StringWriter())
			{
				using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
				{
					xmlSerializer.Serialize(xmlWriter, objectToSerialize, xmlSerializerNamespaces);
					text = stringWriter.ToString();
					File.WriteAllText(filename, text);
				}
			}
			return text;
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x00085A90 File Offset: 0x00083C90
		public static T Deserialize<T>(string filename)
		{
			T t = default(T);
			using (FileStream fileStream = File.OpenRead(filename))
			{
				t = (T)((object)new XmlSerializer(typeof(T)).Deserialize(fileStream));
			}
			return t;
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x00030EFD File Offset: 0x0002F0FD
		public static string GetInnerTextFromXmlDocument(string document)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(document);
			return xmlDocument.FirstChild.InnerText;
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x00085AE4 File Offset: 0x00083CE4
		public static string JsonMessageSerializer<T>(T message)
		{
			JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Objects,
				TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
				Binder = new DotNetCompatibleSerializationBinder()
			};
			return JsonConvert.SerializeObject(message, jsonSerializerSettings);
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x00085B1C File Offset: 0x00083D1C
		public static T JsonMessageDeserializer<T>(string json)
		{
			JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Objects,
				TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
				Binder = new DotNetCompatibleSerializationBinder()
			};
			return (T)((object)JsonConvert.DeserializeObject(json, jsonSerializerSettings));
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x00030F15 File Offset: 0x0002F115
		public static T JsonMessageDeserializerWithStringDeserialization<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(JsonConvert.DeserializeObject(json).ToString());
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x00030F27 File Offset: 0x0002F127
		public static T JsonObjectDeserializer<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json);
		}
	}
}
