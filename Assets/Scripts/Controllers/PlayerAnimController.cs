using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimController : MonoBehaviour
    {
        
        private static readonly int HashIdleVariant = Animator.StringToHash("idleAnimToPlay");
        private static readonly int HashMainState = Animator.StringToHash("currentState");
        private static readonly int HashJumpPhase = Animator.StringToHash("jumpState");
        private static readonly int HashChangeJump = Animator.StringToHash("changeJump");
        
        private Animator _animator;

        private void Awake()
        {
            _animator = gameObject.GetComponent<Animator>();
        }

        #region Main State

        private enum MainAnimState
        {
            Idle, 
            Walk, 
            Jump, 
            Land, 
            Slide
        }

        private MainAnimState _mainState;
        private MainAnimState MainState
        {
            get => _mainState;
            set
            {
                if (_mainState == value)
                    return;
                _mainState = value;
                if (GetJump())
                {
                    ChangeToJumpState = true;
                }
                _animator.SetInteger(HashMainState, (int)_mainState);
            }
        }

        public void SetIdle()  => MainState = MainAnimState.Idle;
        
        public void SetWalk()  => MainState = MainAnimState.Walk;
        
        public bool GetJump()  => MainState == MainAnimState.Jump;

        public void SetJump()
        {
            MainState = MainAnimState.Jump;
        }
        private bool _changeToJumpState;
        public bool ChangeToJumpState
        {
            set
            {
                if (_changeToJumpState == value)
                    return;
                
                _animator.SetBool(HashChangeJump, value);
                _changeToJumpState = value;
            } 
        }

        public void SetLand()  => MainState = MainAnimState.Land;
        [HideInInspector] public bool landed;
        public void SetLandedDefault() => landed = false;
        
        public void SetSlide() => MainState = MainAnimState.Slide;

        #endregion
        
        #region Jump Phase

        private enum JumpAnimPhase
        {
            Contact, 
            Rise, 
            Float, 
            Fall
        }

        private JumpAnimPhase _jumpPhase;

        private JumpAnimPhase JumpPhase
        {
            set
            {
                _jumpPhase = value;
                _animator.SetInteger(HashJumpPhase, (int)_jumpPhase);
            }
        }

        public void SetJumpContact() => JumpPhase = JumpAnimPhase.Contact;
        public void SetJumpRise() => JumpPhase = JumpAnimPhase.Rise;
        public void SetJumpFloat() => JumpPhase = JumpAnimPhase.Float;
        public void SetJumpFall() => JumpPhase = JumpAnimPhase.Fall;
        
        #endregion
        
        public void RandomIdleAnim()
        {
            float idleAnimToPlay = Random.value switch
            {
                >= 0.6f => 0,
                >= 0.2f => 1,
                _ => 2
            };
            _animator.SetFloat(HashIdleVariant, idleAnimToPlay);
        }
        
    }
}
