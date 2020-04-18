using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimpleMultiLanguageImage : MonoBehaviour {

    public Sprite[] sprites = new Sprite[Language.supportedLanguages.Length];

    Image image;
    SpriteRenderer sprite;

    void Awake() {
        image = GetComponent<Image>();
        sprite = GetComponent<SpriteRenderer>();
        Update();
    }

    public void Update() {
        if (image) image.sprite = sprites[Language.GetCurrentLanguage()];
        if (sprite) sprite.sprite = sprites[Language.GetCurrentLanguage()];
    }
}
