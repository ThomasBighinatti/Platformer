using System;
using System.Collections;
using System.Collections.Generic;
using Arrows;
using Datas;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    
    public class ArrowManager : MonoBehaviour
    {
        
        public static ArrowManager Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(transform.parent.gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(transform.parent);
        }
        
        [Header("Settings")]
        [SerializeField] private ArrowNumData arrowNumDatas;
        [SerializeField] private Momentum momentumPrefab;

        private int _currentArrowNum;
        private Arrow CurrentArrowScript { get; set; }
        public bool ArrowScriptIsNull => CurrentArrowScript is null;
        
        private Vector2 _lookingTowards = Vector2.right;
        public Vector2 LookingTowards
        {
            get => _lookingTowards;
            set
            {
                _lookingTowards = value;
                
                float angle = Mathf.Atan2( _lookingTowards.y, _lookingTowards.x) * Mathf.Rad2Deg;
                Vector3 rotation = new Vector3(0, 0, angle);

                if (!ArrowScriptIsNull)
                {
                    CurrentArrowScript.transform.eulerAngles = rotation;
                }
                _pointerParent.transform.eulerAngles = rotation;
            }
        }
        
        private readonly Stack<Arrow> _momentumStack = new Stack<Arrow>();
        
        private GameObject _pointerParent;
        private GameObject _pointer;
        private GameObject _pinPointer;
        
        private void Start()
        {
            StartCoroutine(PinPointCoroutine());

            IEnumerator PinPointCoroutine()
            {
                while (true)
                {
                    PinPoint();
                    yield return new WaitForSeconds(0.016f);
                }
            }
        }
        
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (LevelManager.Instance != null)
            {
                _pointerParent = LevelManager.Instance.PointerParent;
                _pointer = LevelManager.Instance.Pointer;
                _pinPointer = LevelManager.Instance.PinPointer;
                StartCoroutine(PinPointCoroutine());

                //remettre les fleches
            }
            else
            {
                Debug.LogWarning("ArrowManager : No LevelManager");
            }
        }

        public void ChangeArrowNumByCheckpoint(int index)
        {
            _currentArrowNum = arrowNumDatas.ArrowNumList[index];
            Debug.Log("ArrowManager : " + _currentArrowNum);
        }
        
        public void CreateArrow()
        {
            
            if (_currentArrowNum <= 0)
            {
                Debug.LogWarning("ArrowManager : No More Arrows");
                return;
            }
            
            _currentArrowNum--;
            Debug.Log("ArrowManager : " + _currentArrowNum);
            CurrentArrowScript = momentumPrefab; //non adaptable mais on s'en fout
            
            GameObject arrowCreation = Instantiate(CurrentArrowScript.gameObject,_pointer.transform);
            CurrentArrowScript = arrowCreation.GetComponent<Arrow>();

            float angle = Mathf.Atan2(LookingTowards.y, LookingTowards.x) * Mathf.Rad2Deg;
            Vector3 rotation = new Vector3(0, 0, angle);
            arrowCreation.transform.eulerAngles = rotation;
        }
        
        public void ShootArrow()
        {
            if (ArrowScriptIsNull)
            {
                Debug.LogWarning("ArrowManager : No Created Arrow");
                return;
            }
            CurrentArrowScript.SetDynamic();
            CurrentArrowScript.gameObject.transform.SetParent(null);
            CurrentArrowScript.CanStartMoving = true;
            
            CurrentArrowScript = null;
        }
        
        public void PushMomentumArrow(Arrow arrow) => _momentumStack.Push(arrow);
        public void PopMomentumArrow() => _momentumStack.Pop();
        private Momentum PeekMomentumArrow() => _momentumStack.Peek() as Momentum;
        private bool MomentumStackEmpty => _momentumStack.Count <= 0;

        public void RecallArrow()
        {
            Debug.Log(_momentumStack.Count);
            if (MomentumStackEmpty)
                return;
            
            Momentum momentumArrowCalled = PeekMomentumArrow();
            if (!momentumArrowCalled.IsPlanted) 
                return;
            
            momentumArrowCalled.Recall();
            PopMomentumArrow();
        }

        private void PinPoint()
        {
            LayerMask checkMask = LayerMask.GetMask("Default");
            RaycastHit2D hit = Physics2D.Raycast(_pointer.transform.position, _lookingTowards, 40,checkMask);
            //Debug.DrawRay(pointer.transform.position, _lookingTowards * 10f, Color.white, 0.1f);

            if (hit.collider is null) 
                return;
            
            _pinPointer.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y);
            Debug.DrawLine(_pointer.transform.position, hit.point, Color.blue);
            // Debug.DrawRay(pointer.transform.position, _lookingTowards * 10f, Color.red, 0.1f);
                    
            _pinPointer.transform.position = hit.point;
        }
    }
}
