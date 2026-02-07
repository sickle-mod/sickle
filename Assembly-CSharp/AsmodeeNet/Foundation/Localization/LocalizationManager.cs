using System;
using System.Collections.Generic;
using System.Linq;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.Foundation.Localization
{
	// Token: 0x02000968 RID: 2408
	public class LocalizationManager
	{
		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x060040C3 RID: 16579 RVA: 0x0002FC16 File Offset: 0x0002DE16
		public LocalizationManager.Language DefaultLanguage
		{
			get
			{
				return LocalizationManager.Language.en_US;
			}
		}

		// Token: 0x1400015A RID: 346
		// (add) Token: 0x060040C4 RID: 16580 RVA: 0x0015F0F0 File Offset: 0x0015D2F0
		// (remove) Token: 0x060040C5 RID: 16581 RVA: 0x0015F128 File Offset: 0x0015D328
		public event Action<LocalizationManager> OnLanguageChanged;

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x060040C6 RID: 16582 RVA: 0x00051A16 File Offset: 0x0004FC16
		// (set) Token: 0x060040C7 RID: 16583 RVA: 0x0015F160 File Offset: 0x0015D360
		public LocalizationManager.Language CurrentLanguage
		{
			get
			{
				if (this._currentLanguage == null)
				{
					return LocalizationManager.Language.en_US;
				}
				return this._currentLanguage.Value;
			}
			set
			{
				if (this._supportedLanguages.Contains(value))
				{
					this._currentLanguage = new LocalizationManager.Language?(value);
				}
				else
				{
					AsmoLogger.Warning("LocalizationManager", string.Format("{0} is not part of supported languages (Check CoreApplication > Supported Languages). Fall back to default: {1}", value.ToString(), LocalizationManager.Language.en_US.ToString()), null);
					this._currentLanguage = new LocalizationManager.Language?(LocalizationManager.Language.en_US);
				}
				this._WritePreferredLanguage();
				this._LoadLanguage();
				if (this.OnLanguageChanged != null)
				{
					this.OnLanguageChanged(this);
				}
			}
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x060040C8 RID: 16584 RVA: 0x0015F1E8 File Offset: 0x0015D3E8
		public string CurrentLanguageCode
		{
			get
			{
				return this.CurrentLanguage.ToString().Substring(0, 2);
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x060040C9 RID: 16585 RVA: 0x0015F210 File Offset: 0x0015D410
		public string DefaultLanguageCode
		{
			get
			{
				return LocalizationManager.Language.en_US.ToString().Substring(0, 2);
			}
		}

		// Token: 0x1400015B RID: 347
		// (add) Token: 0x060040CA RID: 16586 RVA: 0x0015F234 File Offset: 0x0015D434
		// (remove) Token: 0x060040CB RID: 16587 RVA: 0x0015F26C File Offset: 0x0015D46C
		public event LocalizationManager.LocalizationManagerInitialized LocalizationManagerInitializedEvent;

		// Token: 0x060040CC RID: 16588 RVA: 0x0015F2A4 File Offset: 0x0015D4A4
		public LocalizationManager(List<LocalizationManager.Language> supportedLanguages)
		{
			this._supportedLanguages = supportedLanguages ?? new List<LocalizationManager.Language>();
			AsmoLogger.Info("LocalizationManager", "Supported Languages: " + string.Join(", ", this._supportedLanguages.Select((LocalizationManager.Language lang) => lang.ToString())), null);
		}

		// Token: 0x060040CD RID: 16589 RVA: 0x00051A32 File Offset: 0x0004FC32
		public void Init()
		{
			this._LoadXliffFiles();
			this._InitLanguage();
			if (this.LocalizationManagerInitializedEvent != null)
			{
				this.LocalizationManagerInitializedEvent();
			}
			if (this.OnLanguageChanged != null)
			{
				this.OnLanguageChanged(this);
			}
		}

		// Token: 0x060040CE RID: 16590 RVA: 0x00051A67 File Offset: 0x0004FC67
		public bool HasTranslationInCurrentLanguage(string key)
		{
			return this._ready && !this._useDefaultLanguage && this._keyToLocalizedText.ContainsKey(key);
		}

		// Token: 0x060040CF RID: 16591 RVA: 0x0015F3A0 File Offset: 0x0015D5A0
		public string GetLocalizedText(string key)
		{
			if (!this._ready)
			{
				return key;
			}
			if (!this._keyToLocalizedText.ContainsKey(key))
			{
				AsmoLogger.Warning("LocalizationManager", string.Format("The key: \"{0}\" is not defined in the .xliff file and therefore doesn't have any localization", key), null);
				return key;
			}
			if (this._keyToLocalizedText[key] == null)
			{
				AsmoLogger.Warning("LocalizationManager", string.Format("The key: \"{0}\" is defined but has no translation available", key), null);
				return key;
			}
			return this._keyToLocalizedText[key];
		}

		// Token: 0x060040D0 RID: 16592 RVA: 0x0015F410 File Offset: 0x0015D610
		private void _WritePreferredLanguage()
		{
			string text = this.CurrentLanguage.ToString();
			AsmoLogger.Info("LocalizationManager", "Save language preference: " + text, null);
			KeyValueStore.SetString("CurrentLanguage", text);
			KeyValueStore.Save();
		}

		// Token: 0x060040D1 RID: 16593 RVA: 0x0015F458 File Offset: 0x0015D658
		private LocalizationManager.Language? _ReadPreferredLanguage()
		{
			string @string = KeyValueStore.GetString("CurrentLanguage", "");
			if (string.IsNullOrEmpty(@string))
			{
				AsmoLogger.Warning("LocalizationManager", "Couldn't find a saved language in PlayerPrefs", null);
				return null;
			}
			AsmoLogger.Info("LocalizationManager", "Using saved language preference: " + @string, null);
			return new LocalizationManager.Language?(LocalizationManager._GetLanguageFromString(@string));
		}

		// Token: 0x060040D2 RID: 16594 RVA: 0x0015F4B8 File Offset: 0x0015D6B8
		private LocalizationManager.Language? _GetSystemLanguage()
		{
			SystemLanguage systemLanguage = Application.systemLanguage;
			if (this._unityLanguageToIsoLanguage.ContainsKey(systemLanguage))
			{
				LocalizationManager.Language language = this._unityLanguageToIsoLanguage[systemLanguage];
				AsmoLogger.Info("LocalizationManager", "Using system language: " + language.ToString(), null);
				return new LocalizationManager.Language?(language);
			}
			AsmoLogger.Warning("LocalizationManager", "System language is not supported: " + systemLanguage.ToString(), null);
			return null;
		}

		// Token: 0x060040D3 RID: 16595 RVA: 0x0015F53C File Offset: 0x0015D73C
		private void _LoadLanguage()
		{
			this._ready = false;
			this._keyToLocalizedText = null;
			IEnumerable<TextAsset> enumerable = this.xliffFiles.Where((TextAsset x) => XliffUtility.GetXliffTargetLangFromXml(x.text) == this.CurrentLanguage);
			if (enumerable.Count<TextAsset>() == 0)
			{
				this._useDefaultLanguage = true;
				enumerable = this.xliffFiles.Where((TextAsset x) => XliffUtility.GetXliffTargetLangFromXml(x.text) == LocalizationManager.Language.en_US);
				AsmoLogger.Warning("LocalizationManager", string.Format("No translation file for the language: {0} has been found, English (en-US) will be used instead", this.CurrentLanguage), null);
			}
			else
			{
				this._useDefaultLanguage = false;
			}
			foreach (TextAsset textAsset in enumerable)
			{
				LocalizationDataModel localizationDataModel = LocalizationDataModel.CreateModelFromTextAsset(textAsset);
				if (this._keyToLocalizedText == null)
				{
					this._keyToLocalizedText = localizationDataModel.Parse();
				}
				else
				{
					Dictionary<string, string> dictionary = localizationDataModel.Parse();
					IEnumerable<string> enumerable2 = this._keyToLocalizedText.Keys.Intersect(dictionary.Keys);
					if (enumerable2.Count<string>() != 0)
					{
						AsmoLogger.Warning("LocalizationManager", "2 files managing the same \"target-language\" have duplicated keys. Keys are", null);
						foreach (string text in enumerable2)
						{
							AsmoLogger.Warning("LocalizationManager", text, null);
						}
					}
					this._keyToLocalizedText = this._keyToLocalizedText.Union(dictionary, new CustomDictionnaryComparer()).ToDictionary((KeyValuePair<string, string> x) => x.Key, (KeyValuePair<string, string> x) => x.Value);
				}
			}
			this._ready = this._keyToLocalizedText != null;
		}

		// Token: 0x060040D4 RID: 16596 RVA: 0x00051A87 File Offset: 0x0004FC87
		private void _LoadXliffFiles()
		{
			this.xliffFiles = Resources.LoadAll<TextAsset>("Localization/").ToArray<TextAsset>();
		}

		// Token: 0x060040D5 RID: 16597 RVA: 0x0015F728 File Offset: 0x0015D928
		private void _InitLanguage()
		{
			if (this._currentLanguage == null)
			{
				this._currentLanguage = this._ReadPreferredLanguage();
			}
			if (this._currentLanguage == null)
			{
				this._currentLanguage = this._GetSystemLanguage();
			}
			if (this._currentLanguage == null)
			{
				AsmoLogger.Warning("LocalizationManager", "Couldn't detect language, falling back on default: " + LocalizationManager.Language.en_US.ToString(), null);
				this._currentLanguage = new LocalizationManager.Language?(LocalizationManager.Language.en_US);
			}
			this.CurrentLanguage = this._currentLanguage.Value;
		}

		// Token: 0x060040D6 RID: 16598 RVA: 0x0015F7B8 File Offset: 0x0015D9B8
		private static LocalizationManager.Language _GetLanguageFromString(string lang)
		{
			LocalizationManager.Language language;
			try
			{
				language = (LocalizationManager.Language)Enum.Parse(typeof(LocalizationManager.Language), lang);
			}
			catch
			{
				language = LocalizationManager.Language.en_US;
			}
			return language;
		}

		// Token: 0x04003112 RID: 12562
		private const string _kModuleName = "LocalizationManager";

		// Token: 0x04003113 RID: 12563
		private const string _kCurrentLanguagePreferenceKey = "CurrentLanguage";

		// Token: 0x04003114 RID: 12564
		private const LocalizationManager.Language _kDefaultLanguage = LocalizationManager.Language.en_US;

		// Token: 0x04003116 RID: 12566
		private LocalizationManager.Language? _currentLanguage;

		// Token: 0x04003117 RID: 12567
		private bool _useDefaultLanguage;

		// Token: 0x04003118 RID: 12568
		private List<LocalizationManager.Language> _supportedLanguages;

		// Token: 0x04003119 RID: 12569
		private readonly Dictionary<SystemLanguage, LocalizationManager.Language> _unityLanguageToIsoLanguage = new Dictionary<SystemLanguage, LocalizationManager.Language>
		{
			{
				SystemLanguage.Chinese,
				LocalizationManager.Language.zh_CN
			},
			{
				SystemLanguage.ChineseSimplified,
				LocalizationManager.Language.zh_CHS
			},
			{
				SystemLanguage.ChineseTraditional,
				LocalizationManager.Language.zh_CHT
			},
			{
				SystemLanguage.Dutch,
				LocalizationManager.Language.nl_NL
			},
			{
				SystemLanguage.English,
				LocalizationManager.Language.en_US
			},
			{
				SystemLanguage.French,
				LocalizationManager.Language.fr_FR
			},
			{
				SystemLanguage.German,
				LocalizationManager.Language.de_DE
			},
			{
				SystemLanguage.Italian,
				LocalizationManager.Language.it_IT
			},
			{
				SystemLanguage.Japanese,
				LocalizationManager.Language.ja_JP
			},
			{
				SystemLanguage.Korean,
				LocalizationManager.Language.ko_KR
			},
			{
				SystemLanguage.Portuguese,
				LocalizationManager.Language.pt_PT
			},
			{
				SystemLanguage.Russian,
				LocalizationManager.Language.ru_RU
			},
			{
				SystemLanguage.Spanish,
				LocalizationManager.Language.es_ES
			},
			{
				SystemLanguage.Swedish,
				LocalizationManager.Language.sv_SE
			}
		};

		// Token: 0x0400311A RID: 12570
		public TextAsset[] xliffFiles;

		// Token: 0x0400311B RID: 12571
		private Dictionary<string, string> _keyToLocalizedText;

		// Token: 0x0400311C RID: 12572
		private bool _ready;

		// Token: 0x02000969 RID: 2409
		[Serializable]
		public enum Language
		{
			// Token: 0x0400311F RID: 12575
			unknown,
			// Token: 0x04003120 RID: 12576
			zh_CN,
			// Token: 0x04003121 RID: 12577
			zh_CHS,
			// Token: 0x04003122 RID: 12578
			zh_CHT,
			// Token: 0x04003123 RID: 12579
			nl_NL,
			// Token: 0x04003124 RID: 12580
			en_US,
			// Token: 0x04003125 RID: 12581
			fr_FR,
			// Token: 0x04003126 RID: 12582
			de_DE,
			// Token: 0x04003127 RID: 12583
			it_IT,
			// Token: 0x04003128 RID: 12584
			ja_JP,
			// Token: 0x04003129 RID: 12585
			ko_KR,
			// Token: 0x0400312A RID: 12586
			pt_PT,
			// Token: 0x0400312B RID: 12587
			ru_RU,
			// Token: 0x0400312C RID: 12588
			es_ES,
			// Token: 0x0400312D RID: 12589
			sv_SE
		}

		// Token: 0x0200096A RID: 2410
		// (Invoke) Token: 0x060040D9 RID: 16601
		public delegate void LocalizationManagerInitialized();
	}
}
