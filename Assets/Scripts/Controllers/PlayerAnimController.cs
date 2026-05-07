using UnityEngine;

namespace Controllers
{
    public class PlayerAnimController : MonoBehaviour
    {
        
        [SerializeField] private Animator animator;
        
        private static readonly int HashIdleVariant = Animator.StringToHash("idleAnimToPlay");
        private static readonly int HashMainState   = Animator.StringToHash("currentState");
        private static readonly int HashJumpPhase   = Animator.StringToHash("jumpState");

        #region Main State

        private enum MainAnimState { Idle, Walk, Jump, Land, Slide }

        private MainAnimState _mainState;
        private MainAnimState MainState
        {
            set
            {
                _mainState = value;
                animator.SetInteger(HashMainState, (int)_mainState);
            }
        }

        public void SetIdle()  => MainState = MainAnimState.Idle;
        public void SetWalk()  => MainState = MainAnimState.Walk;
        public void SetJump()  => MainState = MainAnimState.Jump;
        public void SetLand()  => MainState = MainAnimState.Land;
        public void SetSlide() => MainState = MainAnimState.Slide;

        #endregion
        
        #region Jump Phase

        private enum JumpAnimPhase { Contact, Rise, Float, Fall }

        private JumpAnimPhase _jumpPhase;
        private JumpAnimPhase JumpPhase
        {
            set
            {
                _jumpPhase = value;
                animator.SetInteger(HashJumpPhase, (int)_jumpPhase);
            }
        }

        public void SetJumpContact() => JumpPhase = JumpAnimPhase.Contact;
        public void SetJumpRise()    => JumpPhase = JumpAnimPhase.Rise;
        public void SetJumpFloat()   => JumpPhase = JumpAnimPhase.Float;
        public void SetJumpFall()    => JumpPhase = JumpAnimPhase.Fall;

        #endregion
        
        public void RandomIdleAnim()
        {
            float idleAnimToPlay = Random.value switch
            {
                >= 0.6f => 0,
                >= 0.2f => 1,
                _ => 2
            };
            animator.SetFloat(HashIdleVariant, idleAnimToPlay);
        }
    }
}
