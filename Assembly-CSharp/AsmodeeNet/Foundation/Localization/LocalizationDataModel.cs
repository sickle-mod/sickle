using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.Foundation.Localization
{
	// Token: 0x0200095F RID: 2399
	public class LocalizationDataModel
	{
		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06004093 RID: 16531 RVA: 0x000518A1 File Offset: 0x0004FAA1
		// (set) Token: 0x06004094 RID: 16532 RVA: 0x000518A9 File Offset: 0x0004FAA9
		public LocalizationManager.Language TargetLanguage { get; private set; }

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06004095 RID: 16533 RVA: 0x0015E1E4 File Offset: 0x0015C3E4
		public Dictionary<string, string> KeyToTranslation
		{
			get
			{
				return this._keyToTranslation.ToDictionary((KeyValuePair<string, string> x) => x.Key, (KeyValuePair<string, string> x) => x.Value);
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06004096 RID: 16534 RVA: 0x000518B2 File Offset: 0x0004FAB2
		// (set) Token: 0x06004097 RID: 16535 RVA: 0x000518BA File Offset: 0x0004FABA
		public List<string> KeyNeedingTranslation { get; private set; }

		// Token: 0x06004098 RID: 16536 RVA: 0x000518C3 File Offset: 0x0004FAC3
		private LocalizationDataModel()
		{
			this.TargetLanguage = LocalizationManager.Language.unknown;
			this.KeyNeedingTranslation = new List<string>();
		}

		// Token: 0x06004099 RID: 16537 RVA: 0x0015E23C File Offset: 0x0015C43C
		public static LocalizationDataModel CreateModelFromTextAsset(TextAsset source)
		{
			LocalizationDataModel localizationDataModel = new LocalizationDataModel();
			localizationDataModel._xmlDocument = new XmlDocument();
			localizationDataModel._xmlDocument.LoadXml(source.text);
			localizationDataModel.TargetLanguage = LocalizationDataModel._GetFileTargetLanguage(localizationDataModel);
			if (localizationDataModel.TargetLanguage == LocalizationManager.Language.unknown)
			{
				return null;
			}
			localizationDataModel.Parse();
			return localizationDataModel;
		}

		// Token: 0x0600409A RID: 16538 RVA: 0x0015E28C File Offset: 0x0015C48C
		public static LocalizationDataModel CreateModelFromTextFile(string path)
		{
			LocalizationDataModel localizationDataModel = new LocalizationDataModel();
			localizationDataModel._xmlDocument = new XmlDocument();
			localizationDataModel._xmlDocument.Load(path);
			localizationDataModel.TargetLanguage = LocalizationDataModel._GetFileTargetLanguage(localizationDataModel);
			if (localizationDataModel.TargetLanguage == LocalizationManager.Language.unknown)
			{
				return null;
			}
			localizationDataModel.Parse();
			return localizationDataModel;
		}

		// Token: 0x0600409B RID: 16539 RVA: 0x0015E2D4 File Offset: 0x0015C4D4
		public static LocalizationDataModel CreateModelFromString(string content)
		{
			LocalizationDataModel localizationDataModel = new LocalizationDataModel();
			localizationDataModel._xmlDocument = new XmlDocument();
			localizationDataModel._xmlDocument.LoadXml(content);
			localizationDataModel.TargetLanguage = LocalizationDataModel._GetFileTargetLanguage(localizationDataModel);
			if (localizationDataModel.TargetLanguage == LocalizationManager.Language.unknown)
			{
				return null;
			}
			localizationDataModel.Parse();
			return localizationDataModel;
		}

		// Token: 0x0600409C RID: 16540 RVA: 0x0015E31C File Offset: 0x0015C51C
		public static LocalizationDataModel CreateModelByMerging2Files(string importedFile, string previousFile)
		{
			LocalizationDataModel localizationDataModel = LocalizationDataModel.CreateModelFromTextFile(importedFile);
			LocalizationDataModel localizationDataModel2 = LocalizationDataModel.CreateModelFromTextFile(previousFile);
			if (localizationDataModel == null || localizationDataModel2 == null)
			{
				return null;
			}
			LocalizationDataModel localizationDataModel3 = new LocalizationDataModel();
			localizationDataModel3._xmlDocument = localizationDataModel._xmlDocument.Clone() as XmlDocument;
			localizationDataModel3.TargetLanguage = localizationDataModel.TargetLanguage;
			if (localizationDataModel.TargetLanguage != localizationDataModel2.TargetLanguage)
			{
				return null;
			}
			bool flag = false;
			foreach (object obj in localizationDataModel._xmlDocument.GetElementsByTagName("trans-unit"))
			{
				XmlNode xmlNode = (XmlNode)obj;
				flag = false;
				XmlNode xmlNode2 = LocalizationDataModel._GetSourceNodeFromTransUnit(xmlNode);
				foreach (object obj2 in localizationDataModel._xmlDocument.GetElementsByTagName("trans-unit"))
				{
					XmlNode xmlNode3 = (XmlNode)obj2;
					if (LocalizationDataModel._GetSourceNodeFromTransUnit(xmlNode).InnerText == xmlNode2.InnerText)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					localizationDataModel3.AddTranslation(localizationDataModel.TargetLanguage, xmlNode2.InnerText, LocalizationDataModel._GetTargetNodeFromTransUnit(xmlNode).InnerText);
				}
			}
			return localizationDataModel3;
		}

		// Token: 0x0600409D RID: 16541 RVA: 0x0015E478 File Offset: 0x0015C678
		public Dictionary<string, string> Parse()
		{
			if (this.TargetLanguage == LocalizationManager.Language.unknown)
			{
				return null;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			this.KeyNeedingTranslation.Clear();
			XmlNodeList elementsByTagName = this._xmlDocument.GetElementsByTagName("trans-unit");
			for (int i = 0; i < elementsByTagName.Count; i++)
			{
				XmlNode xmlNode = LocalizationDataModel._GetSourceNodeFromTransUnit(elementsByTagName[i]);
				XmlNode xmlNode2 = LocalizationDataModel._GetTargetNodeFromTransUnit(elementsByTagName[i]);
				if (xmlNode != null)
				{
					if (xmlNode2 == null)
					{
						AsmoLogger.Warning("LocalizationDataModel", string.Format("No traduction have been found for the key: '{0}' for the language: '{1}'. Key will be ignored at runtime.\nFile: {2}", xmlNode.InnerText, this.TargetLanguage, this._xmlDocument.BaseURI), null);
					}
					else
					{
						XmlNode namedItem = xmlNode2.Attributes.GetNamedItem("xml:lang");
						if (namedItem == null)
						{
							AsmoLogger.Warning("LocalizationDataModel", string.Format("The key: '{0}' has a translation but not a target language associated. Key will be ignored at runtime\nFile: {1}", xmlNode.InnerText, this._xmlDocument.BaseURI), null);
						}
						else
						{
							if (namedItem.InnerText != this.TargetLanguage.ToXsdLanguage())
							{
								XmlNode xmlNode3 = xmlNode2.Attributes["state"];
								if (xmlNode3 == null || (!(xmlNode3.InnerText == "needs-translation") && !(xmlNode3.InnerText == "needs-review-translation")))
								{
									AsmoLogger.Warning("LocalizationDataModel", string.Format("The key: '{0}' is defined for the following language: '{1}' which does not match the target language of the file : '{2}'. Key will be ignored at runtime\nFile: {3}", new object[]
									{
										xmlNode.InnerText,
										namedItem.InnerText,
										this.TargetLanguage,
										this._xmlDocument.BaseURI
									}), null);
									goto IL_01F6;
								}
								AsmoLogger.Warning("LocalizationDataModel", string.Format("The key: '{0}' is still waiting for being translated. Key will be ignored at runtime\nFile: {1}", xmlNode.InnerText, this._xmlDocument.BaseURI), null);
								if (Application.isPlaying)
								{
									goto IL_01F6;
								}
								this.KeyNeedingTranslation.Add(xmlNode.InnerText);
							}
							if (dictionary.ContainsKey(xmlNode.InnerText))
							{
								AsmoLogger.Warning("LocalizationDataModel", string.Format("The key: {0} is present more than one time in the file. The previous translation will be overridden.", xmlNode.InnerText), null);
							}
							else
							{
								dictionary[xmlNode.InnerText] = xmlNode2.InnerText;
							}
						}
					}
				}
				IL_01F6:;
			}
			this._keyToTranslation = dictionary.Select((KeyValuePair<string, string> x) => new KeyValuePair<string, string>(x.Key, x.Value)).ToList<KeyValuePair<string, string>>();
			return dictionary;
		}

		// Token: 0x0600409E RID: 16542 RVA: 0x0015E6BC File Offset: 0x0015C8BC
		public bool AddTranslation(LocalizationManager.Language targetLang, string key, string val)
		{
			if (this.TargetLanguage != targetLang)
			{
				AsmoLogger.Warning("LocalizationDataModel", string.Format("Unable to create the translation for the keyword: \"{0}\": the chosen _targetLang does not match with the file target lang", key), null);
				return false;
			}
			XmlNode xmlNode = this._xmlDocument.GetElementsByTagName("body")[0];
			XmlNode xmlNode2;
			if ((xmlNode2 = this._GetTransUnitFromKey(key)) != null)
			{
				if (LocalizationDataModel._GetTargetNodeFromTransUnit(xmlNode2) != null)
				{
					AsmoLogger.Warning("LocalizationDataModel", string.Format("Unable to create the translation for the keyword : \"{0}\" : this keyword already exists", key), null);
					return false;
				}
				XmlNode xmlNode3 = this._CreateTargetNode(val, targetLang);
				xmlNode2.AppendChild(xmlNode3);
			}
			else
			{
				xmlNode2 = this._xmlDocument.CreateElement("trans-unit");
				XmlAttribute xmlAttribute = this._xmlDocument.CreateAttribute("xmlns");
				xmlAttribute.InnerText = this._xmlDocument.DocumentElement.NamespaceURI;
				xmlNode2.Attributes.Append(xmlAttribute);
				XmlAttribute xmlAttribute2 = this._xmlDocument.CreateAttribute("id");
				xmlAttribute2.InnerText = key.GetHashCode().ToString();
				xmlNode2.Attributes.Append(xmlAttribute2);
				XmlNode xmlNode4 = this._xmlDocument.CreateElement("source");
				xmlNode4.InnerText = key;
				XmlNode xmlNode3 = this._CreateTargetNode(val, targetLang);
				xmlNode2.AppendChild(xmlNode4);
				xmlNode2.AppendChild(xmlNode3);
				xmlNode.AppendChild(xmlNode2);
			}
			this._keyToTranslation.Add(new KeyValuePair<string, string>(key, val));
			if (!Application.isPlaying)
			{
				this.KeyNeedingTranslation.Remove(key);
			}
			return true;
		}

		// Token: 0x0600409F RID: 16543 RVA: 0x0015E824 File Offset: 0x0015CA24
		public bool UpdateTargetLangTranslation(LocalizationManager.Language targetLang, string key, string val)
		{
			if (this.TargetLanguage != targetLang)
			{
				AsmoLogger.Warning("LocalizationDataModel", string.Format("Unable to update the translation for the keyword : \"{0}\" : the chosen _targetLang does not match with the file target lang", key), null);
				return false;
			}
			XmlNode xmlNode = this._GetTransUnitFromKey(key);
			if (xmlNode == null)
			{
				AsmoLogger.Warning("LocalizationDataModel", string.Format("Unable to update the translation for the keyword : \"{0}\" : this keyword does not exist yet", key), null);
				return false;
			}
			XmlNode xmlNode2 = LocalizationDataModel._GetTargetNodeFromTransUnit(xmlNode);
			if (xmlNode2 == null)
			{
				AsmoLogger.Warning("LocalizationDataModel", string.Format("Unable to update the translation for the keyword : \"{0}\" : this keyword has no target field and you try to update it", key), null);
				return false;
			}
			xmlNode.RemoveChild(xmlNode2);
			xmlNode2 = this._CreateTargetNode(val, targetLang);
			xmlNode.AppendChild(xmlNode2);
			KeyValuePair<string, string> keyValuePair = this._keyToTranslation.SingleOrDefault((KeyValuePair<string, string> x) => x.Key == key);
			this._keyToTranslation[this._keyToTranslation.IndexOf(keyValuePair)] = new KeyValuePair<string, string>(key, val);
			if (!Application.isPlaying)
			{
				this.KeyNeedingTranslation.Remove(key);
			}
			return true;
		}

		// Token: 0x060040A0 RID: 16544 RVA: 0x0015E928 File Offset: 0x0015CB28
		public bool RemoveTranslation(string key)
		{
			XmlNode xmlNode = this._GetTransUnitFromKey(key);
			if (xmlNode == null)
			{
				AsmoLogger.Warning("LocalizationDataModel", string.Format("Unable to remove the translation associated with the keyword : \"{0}\" : this keyword does not exist", key), null);
				return false;
			}
			xmlNode.ParentNode.RemoveChild(xmlNode);
			this._keyToTranslation.Remove(this._keyToTranslation.Single((KeyValuePair<string, string> x) => x.Key == key));
			if (!Application.isPlaying)
			{
				this.KeyNeedingTranslation.Remove(key);
			}
			return true;
		}

		// Token: 0x060040A1 RID: 16545 RVA: 0x0015E9BC File Offset: 0x0015CBBC
		public bool UpdateKey(string oldKey, string newKey)
		{
			XmlNode xmlNode = this._GetTransUnitFromKey(oldKey);
			if (xmlNode == null)
			{
				AsmoLogger.Warning("LocalizationDataModel", string.Format("Unable to update the translation associated with the keyword : \"{0}\" : this keyword does not exist", oldKey), null);
				return false;
			}
			XmlNode xmlNode2 = LocalizationDataModel._GetSourceNodeFromTransUnit(xmlNode);
			if (xmlNode2 != null)
			{
				xmlNode2.InnerText = newKey;
			}
			XmlAttribute xmlAttribute = xmlNode.Attributes["id"];
			if (xmlAttribute != null)
			{
				xmlAttribute.InnerText = newKey.GetHashCode().ToString();
			}
			KeyValuePair<string, string> keyValuePair = this._keyToTranslation.SingleOrDefault((KeyValuePair<string, string> x) => x.Key == oldKey);
			KeyValuePair<string, string> keyValuePair2 = new KeyValuePair<string, string>(newKey, keyValuePair.Value);
			this._keyToTranslation[this._keyToTranslation.IndexOf(keyValuePair)] = keyValuePair2;
			if (!Application.isPlaying)
			{
				this.KeyNeedingTranslation.Remove(newKey);
			}
			return true;
		}

		// Token: 0x060040A2 RID: 16546 RVA: 0x0015EA94 File Offset: 0x0015CC94
		public void MoveKeyUp(string key)
		{
			KeyValuePair<string, string> keyValuePair = this._keyToTranslation.SingleOrDefault((KeyValuePair<string, string> x) => x.Key == key);
			int num = this._keyToTranslation.IndexOf(keyValuePair);
			this._MoveKey(num, num - 1);
		}

		// Token: 0x060040A3 RID: 16547 RVA: 0x0015EAE0 File Offset: 0x0015CCE0
		public void MoveKeyDown(string key)
		{
			KeyValuePair<string, string> keyValuePair = this._keyToTranslation.SingleOrDefault((KeyValuePair<string, string> x) => x.Key == key);
			int num = this._keyToTranslation.IndexOf(keyValuePair);
			this._MoveKey(num, num + 1);
		}

		// Token: 0x060040A4 RID: 16548 RVA: 0x0015EB2C File Offset: 0x0015CD2C
		public void SortKeys()
		{
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>(this._keyToTranslation);
			list.Sort((KeyValuePair<string, string> kTT1, KeyValuePair<string, string> kTT2) => string.Compare(kTT1.Key, kTT2.Key, true));
			for (int i = 0; i < list.Count; i++)
			{
				string key = list[i].Key;
				KeyValuePair<string, string> keyValuePair = this._keyToTranslation.SingleOrDefault((KeyValuePair<string, string> x) => x.Key == key);
				int num = this._keyToTranslation.IndexOf(keyValuePair);
				this._MoveKey(num, i);
			}
		}

		// Token: 0x060040A5 RID: 16549 RVA: 0x0015EBC8 File Offset: 0x0015CDC8
		private void _MoveKey(int sourceIdx, int destinationIdx)
		{
			if (sourceIdx == destinationIdx)
			{
				return;
			}
			if (sourceIdx < 0 || destinationIdx < 0 || sourceIdx >= this._keyToTranslation.Count || destinationIdx >= this._keyToTranslation.Count)
			{
				AsmoLogger.Warning("LocalizationDataModel", string.Format("Index out of bounds sourceIdx: {0} destinationIdx: {1} [0..{2}]", sourceIdx, destinationIdx, this._keyToTranslation.Count - 1), null);
			}
			string key = this._keyToTranslation[sourceIdx].Key;
			XmlNode xmlNode = this._GetTransUnitFromKey(key);
			string key2 = this._keyToTranslation[destinationIdx].Key;
			XmlNode xmlNode2 = this._GetTransUnitFromKey(key2);
			AsmoLogger.Warning("LocalizationDataModel", string.Format("{0} {1} - {2} {3}", new object[] { sourceIdx, key, destinationIdx, key2 }), null);
			xmlNode.ParentNode.RemoveChild(xmlNode);
			xmlNode2.ParentNode.InsertBefore(xmlNode, xmlNode2);
			KeyValuePair<string, string> keyValuePair = this._keyToTranslation[sourceIdx];
			this._keyToTranslation.RemoveAt(sourceIdx);
			this._keyToTranslation.Insert(destinationIdx, keyValuePair);
		}

		// Token: 0x060040A6 RID: 16550 RVA: 0x000518E8 File Offset: 0x0004FAE8
		public void SaveModification(string path = null)
		{
			if (this.TargetLanguage == LocalizationManager.Language.unknown)
			{
				return;
			}
			this._xmlDocument.Save(path ?? new Uri(this._xmlDocument.BaseURI).LocalPath);
		}

		// Token: 0x060040A7 RID: 16551 RVA: 0x0015ECE8 File Offset: 0x0015CEE8
		public void ExportFile(string path, LocalizationManager.Language exportLanguage, Dictionary<string, string> alreadyTranslatedKeys)
		{
			XmlDocument xmlDocument = this._xmlDocument.CloneNode(true) as XmlDocument;
			xmlDocument.GetElementsByTagName("file")[0].Attributes["target-language"].Value = exportLanguage.ToXsdLanguage();
			foreach (object obj in xmlDocument.GetElementsByTagName("trans-unit"))
			{
				XmlNode xmlNode = (XmlNode)obj;
				XmlNode xmlNode2 = LocalizationDataModel._GetSourceNodeFromTransUnit(xmlNode);
				if (xmlNode2 != null)
				{
					if (alreadyTranslatedKeys.ContainsKey(xmlNode2.InnerText))
					{
						XmlNode xmlNode3 = LocalizationDataModel._GetTargetNodeFromTransUnit(xmlNode);
						if (xmlNode3 != null)
						{
							xmlNode3.InnerText = alreadyTranslatedKeys[xmlNode2.InnerText];
							xmlNode3.Attributes["xml:lang"].Value = exportLanguage.ToXsdLanguage();
						}
						else
						{
							xmlNode.AppendChild(this._CreateTargetNode(alreadyTranslatedKeys[xmlNode2.InnerText], exportLanguage));
						}
					}
					else
					{
						XmlNode xmlNode4 = LocalizationDataModel._GetTargetNodeFromTransUnit(xmlNode);
						XmlAttribute xmlAttribute = xmlNode4.Attributes["state"];
						if (xmlAttribute == null)
						{
							xmlAttribute = xmlDocument.CreateAttribute("state");
							xmlNode4.Attributes.Append(xmlAttribute);
						}
						xmlNode4.Attributes["xml:lang"].Value = exportLanguage.ToXsdLanguage();
						xmlAttribute.Value = "needs-translation";
					}
				}
			}
			xmlDocument.Save(path);
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x060040A8 RID: 16552 RVA: 0x00051918 File Offset: 0x0004FB18
		public string GetFileName
		{
			get
			{
				return Path.GetFileName(this._xmlDocument.BaseURI);
			}
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x060040A9 RID: 16553 RVA: 0x0005192A File Offset: 0x0004FB2A
		public string GetUri
		{
			get
			{
				return this._xmlDocument.BaseURI;
			}
		}

		// Token: 0x060040AA RID: 16554 RVA: 0x0015EE64 File Offset: 0x0015D064
		private static LocalizationManager.Language _GetFileTargetLanguage(LocalizationDataModel dm)
		{
			string value = dm._xmlDocument.GetElementsByTagName("file")[0].Attributes.GetNamedItem("target-language").Value;
			LocalizationManager.Language language = LanguageHelper.LanguageFromXsdLanguage(value);
			if (language == LocalizationManager.Language.unknown)
			{
				AsmoLogger.Error("LocalizationDataModel", string.Format("target-language {0} is not supported. Unable to use this file: {1}", value, dm._xmlDocument.BaseURI), null);
			}
			return language;
		}

		// Token: 0x060040AB RID: 16555 RVA: 0x0015EEC8 File Offset: 0x0015D0C8
		private XmlNode _GetTransUnitFromKey(string key)
		{
			XmlNodeList elementsByTagName = this._xmlDocument.GetElementsByTagName("trans-unit");
			for (int i = 0; i < elementsByTagName.Count; i++)
			{
				for (int j = 0; j < elementsByTagName[i].ChildNodes.Count; j++)
				{
					if (elementsByTagName[i].ChildNodes[j].Name == "source" && elementsByTagName[i].ChildNodes[j].InnerText == key)
					{
						return elementsByTagName[i];
					}
				}
			}
			return null;
		}

		// Token: 0x060040AC RID: 16556 RVA: 0x0015EF60 File Offset: 0x0015D160
		private static XmlNode _GetSourceNodeFromTransUnit(XmlNode transUnit)
		{
			foreach (object obj in transUnit.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "source")
				{
					return xmlNode;
				}
			}
			return null;
		}

		// Token: 0x060040AD RID: 16557 RVA: 0x0015EFCC File Offset: 0x0015D1CC
		private static XmlNode _GetTargetNodeFromTransUnit(XmlNode transUnit)
		{
			foreach (object obj in transUnit.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "target")
				{
					return xmlNode;
				}
			}
			return null;
		}

		// Token: 0x060040AE RID: 16558 RVA: 0x0015F038 File Offset: 0x0015D238
		private XmlNode _CreateTargetNode(string val, LocalizationManager.Language _targetLang)
		{
			XmlElement xmlElement = this._xmlDocument.CreateElement("target", this._xmlDocument.DocumentElement.NamespaceURI);
			xmlElement.InnerText = val;
			XmlAttribute xmlAttribute = this._xmlDocument.CreateAttribute("xml:lang");
			xmlAttribute.InnerText = _targetLang.ToXsdLanguage();
			xmlElement.Attributes.Append(xmlAttribute);
			return xmlElement;
		}

		// Token: 0x04003102 RID: 12546
		private const string _kModuleName = "LocalizationDataModel";

		// Token: 0x04003103 RID: 12547
		private XmlDocument _xmlDocument;

		// Token: 0x04003105 RID: 12549
		private List<KeyValuePair<string, string>> _keyToTranslation = new List<KeyValuePair<string, string>>();
	}
}
