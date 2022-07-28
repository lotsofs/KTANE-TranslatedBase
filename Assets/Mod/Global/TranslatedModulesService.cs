using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEngine;

public class TranslatedModulesService : MonoBehaviour {
	public GameObject Sticker;
	public string SettingsFileName = "TranslatedModules-Settings";

	public void Start() {
		Configuration<TranslationSettingsGlobal> config = new Configuration<TranslationSettingsGlobal>(SettingsFileName);
		TranslationSettingsGlobal settings = config.Settings;
		if (settings.UseAllLanguages) {
			TranslationSettingsGlobal defaultValues = new TranslationSettingsGlobal();
			settings.LanguagePool = defaultValues.LanguagePool;
		}
		config.Settings = settings;
	}



}
