using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimpleMultiLanguageText : MonoBehaviour {

    [TextArea(1, 20)]
    public string[] texts = new string[Language.supportedLanguages.Length];

    Text text;
    TextMeshProUGUI tmProText;

    void Awake() {
        text = GetComponent<Text>();
        tmProText = GetComponent<TextMeshProUGUI>();
        Update();
    }

    public void Update() {
        if (text) text.text = texts[Language.GetCurrentLanguage()];
        if (tmProText) tmProText.text = texts[Language.GetCurrentLanguage()];
    }
}
