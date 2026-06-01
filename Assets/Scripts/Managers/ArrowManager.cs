using System;
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
        [SerializeField] private List<ArrowGroupData> arrowGroupDatas;
        [SerializeField] private Momentum momentumPrefab;
        
        private ArrowGroupData CurrentArrowGroupData { get; set; }
        private Arrow CurrentArrowScript { get; set; }
        public bool ArrowScriptIsNull => CurrentArrowScript is null;
        
        private int _arrowNum;
        
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
        
        private void Start()
        {
            CurrentArrowGroupData = arrowGroupDatas[0]; //changer le mode fonctionnement du group data
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
                
                //remettre les fleches
            }
            else
            {
                Debug.LogWarning("ArrowManager : No LevelManager");
            }
        }

        private Arrow GetArrowByType(ArrowType arrowType)
        {
            return arrowType switch
            {
                ArrowType.Momentum => momentumPrefab,
                _ => throw new ArgumentOutOfRangeException(nameof(arrowType), arrowType, null)
            };
        }
        
        public void CreateArrow()
        {
            
            if (_arrowNum >= CurrentArrowGroupData.ArrowTypeList.Count)
            {
                Debug.LogWarning("ArrowManager : No More Arrows");
                _arrowNum = 0;
                CurrentArrowScript = GetArrowByType(CurrentArrowGroupData.ArrowTypeList[0]);
                return;
            }
            
            CurrentArrowScript = GetArrowByType(CurrentArrowGroupData.ArrowTypeList[_arrowNum]);
            
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
            _arrowNum++;
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
        
    }
}
