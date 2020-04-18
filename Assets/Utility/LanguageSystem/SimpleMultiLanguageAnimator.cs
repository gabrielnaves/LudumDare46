using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Animator))]
public class SimpleMultiLanguageAnimator : MonoBehaviour {

    public AnimatorOverrideController[] animators = new AnimatorOverrideController[Language.supportedLanguages.Length];

    Animator animator;

    void Awake() {
        animator = GetComponent<Animator>();
        Update();
    }

    public void Update() {
        animator.runtimeAnimatorController = animators[Language.GetCurrentLanguage()];
    }
}
