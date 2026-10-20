using UnityEngine;

// A StateMachineBehaviour runs on an Animator state,
// not on a GameObject.
// This one is attached to the Stand to Roll state.
public class RollBehaviour : StateMachineBehaviour
{
    // Called when the Animator starts playing this state
    public override void OnStateEnter(Animator animator,
        AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Let the roll animation itself move the character forward
        animator.applyRootMotion = true;
    }

    // NEW FIX: The rool animation now applies its root movement to the Rigidbody
    // using animator.deltaPoistion and rb.MovePosition()
    // Allows Player to move forward during the roll instead of rolling in place. 
    // Root motion is enabled when the roll starts and disabled when the roll ends, 
    // returning movement control to the CharacterMovement
    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        Rigidbody rb = animator.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.MovePosition(rb.position + animator.deltaPosition);
        }
    }

    // Called when the Animator finishes this state
    public override void OnStateExit(Animator animator,
        AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Hand movement back to the PlayerMovement script
        animator.applyRootMotion = false;
    }
}
