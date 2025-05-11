using UnityEngine;

public class DeathBehavior : StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Destroy the parent game object when the death animation completes
        Destroy(animator.gameObject);
    }
}