public class TranslationSettings {
	public string HowToUse1 = "If you want to play with (a) specific language(s) only, set UseAllLanguages to false and remove all other language codes from the LanguagePool list.";
	public string HowToUse2 = "If UseAllLanguages is set to true, the module will ignore the LanguagePool list and instead pick any language.";
	public string HowToUse3 = "Set LanguagesWithManualOnly to true if you only want the module to select languages that have a dedicated manual available in that language.";
	public string HowToUse4 = "If UseGlobalSettings is set to true, module specific settings will be ignored and instead the settings from ";
	public bool UseGlobalSettings = true;
	public bool UseAllLanguages = true;
	public bool UseLanguagesWithManualOnly = false;
	public string[] LanguagePool = {
        "af", "ar", "az",
        "be", "bg", "bn", "br",
        "ca", "cs",
        "da", "de",
        "el", "en", "eo", "es", "eu", "et",
        "fi", "fr", "fy",
        "ga", "gl",
        "he", "hi", "hr", "hu",
        "it",
        "ja", "jv",
        "ko",
        "la", "lb",
        "mk", "ml",
        "nl", "no",
        "oc",
        "pl", "pt",
        "ro",
        "so",
        "sv",
        "ru",
        "th", "tl", "tr", "tt",
        "uk",
        "vi",
        "zh",
    };
}


