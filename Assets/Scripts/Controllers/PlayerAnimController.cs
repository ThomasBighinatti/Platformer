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

        private float _idleAnimToPlay;
        public void RandomIdleAnim()
        {
            _idleAnimToPlay = Random.Range(0, 3);
            animator.SetFloat(IdleAnimToPlay, _idleAnimToPlay);
        }
    
    }
}
