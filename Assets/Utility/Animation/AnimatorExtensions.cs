using UnityEngine;

static public class AnimatorExtensions {

    static public bool IsCurrentState(this Animator animator, string name) {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
}
