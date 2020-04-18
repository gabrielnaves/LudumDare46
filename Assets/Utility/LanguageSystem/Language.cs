using UnityEngine;

static public class Language {

    static public readonly string languageKey = "CurrentLanguage";

    static int defaultLanguage = 0;

    static public SystemLanguage[] supportedLanguages = new SystemLanguage[] {
        SystemLanguage.English,
        SystemLanguage.Portuguese,
    };

    static public int GetCurrentLanguage() {
        return Mathf.Clamp(PlayerPrefs.GetInt(languageKey, defaultLanguage), 0, supportedLanguages.Length - 1);
    }

    static public void SetCurrentLanguage(int lang) {
        PlayerPrefs.SetInt(languageKey, Mathf.Clamp(lang, 0, supportedLanguages.Length - 1));
    }

    static public void SelectNextLanguage() {
        int language = GetCurrentLanguage();
        language++;
        if (language >= supportedLanguages.Length)
            language = 0;
        SetCurrentLanguage(language);
    }
}
