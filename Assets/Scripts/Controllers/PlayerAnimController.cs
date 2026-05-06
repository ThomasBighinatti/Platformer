using UnityEngine;

namespace Controllers
{
    public class PlayerAnimController : MonoBehaviour
    {
        
        [SerializeField] private Animator animator;
        
        private static readonly int IdleAnimToPlay = Animator.StringToHash("idleAnimToPlay");
        private static readonly int CurrentState = Animator.StringToHash("currentState");

        private enum AnimState
        {
            Idle, 
            Walk, 
            Jump, 
            Slide
        }
        
        private AnimState _currentAnimState = AnimState.Idle;
        private AnimState CurrentAnimState
        {
            get => _currentAnimState;
            set
            {
                _currentAnimState = value;
                animator.SetInteger(CurrentState, (int)_currentAnimState);
            }
        }

        public void IdleState() => CurrentAnimState = AnimState.Idle;
        public void WalkState() => CurrentAnimState = AnimState.Walk;
        public void JumpState() => CurrentAnimState = AnimState.Jump;
        public void SlideState() => CurrentAnimState = AnimState.Slide;
        
        public void RandomIdleAnim()
        {
            float idleAnimToPlay = Random.value switch
            {
                >= 0.6f => 0,
                >= 0.2f => 1,
                _ => 2
            };
            animator.SetFloat(IdleAnimToPlay, idleAnimToPlay);
        }
    
    }
}
