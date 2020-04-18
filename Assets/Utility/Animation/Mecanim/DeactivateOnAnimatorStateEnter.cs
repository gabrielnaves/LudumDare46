using UnityEngine;

public class DeactivateOnAnimatorStateEnter : StateMachineBehaviour {

    public float delay = 0;

    float elapsedTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        elapsedTime = 0;
        if (delay <= 0)
            animator.gameObject.SetActive(false);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > delay)
            animator.gameObject.SetActive(false);
    }
}
