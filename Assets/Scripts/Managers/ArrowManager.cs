using System;
using System.Collections.Generic;
using Arrows;
using Controllers;
using Datas;
using UnityEngine;

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

        [SerializeField] private ButterflyController butterfly;
        
        [SerializeField] private List<ArrowGroupData> arrowGroupDatas;
        
        [SerializeField] private Momentum momentumPrefab;

        [SerializeField] private GameObject pointerParent;
        [SerializeField] private GameObject pointer;
        
        [SerializeField] private Transform playerTransform;
        public static Transform PlayerTransform { get; private set; }
        
        private void Start()
        {
            CurrentArrowGroupData = arrowGroupDatas[0];
            PlayerTransform = playerTransform;
        }

        private ArrowGroupData CurrentArrowGroupData { get; set; }

        private Arrow GetArrowByType(ArrowType arrowType)
        {
            return arrowType switch
            {
                ArrowType.Momentum => momentumPrefab,
                _ => throw new ArgumentOutOfRangeException(nameof(arrowType), arrowType, null)
            };
        }

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
                pointerParent.transform.eulerAngles = rotation;
            }
        }
        
        public void CreateArrow()
        {
            Debug.Log("Shoot");
            butterfly.ToTransState();
            
            if (_arrowNum < CurrentArrowGroupData.ArrowTypeList.Count)
            {
                CurrentArrowScript = GetArrowByType(CurrentArrowGroupData.ArrowTypeList[_arrowNum]);
            }
            else
            {
                Debug.Log("no more arrows");
                _arrowNum = 0;
                CurrentArrowScript = GetArrowByType(CurrentArrowGroupData.ArrowTypeList[0]);
            }

            GameObject arrowCreation = Instantiate(CurrentArrowScript.gameObject,pointer.transform);
            CurrentArrowScript = arrowCreation.GetComponent<Arrow>();

            float angle = Mathf.Atan2(LookingTowards.y, LookingTowards.x) * Mathf.Rad2Deg;
            Vector3 rotation = new Vector3(0, 0, angle);
            arrowCreation.transform.eulerAngles = rotation;
        }
        
        public void ShootArrow()
        {
            butterfly.ToShootState();
            
            CurrentArrowScript.SetDynamic();
            CurrentArrowScript.gameObject.transform.SetParent(null);
            CurrentArrowScript.CanStartMoving = true;
            
            CurrentArrowScript = null;
            _arrowNum++;
        }
        
        private readonly Queue<Arrow> _momentumQueue = new Queue<Arrow>();
        public void EnqueueMomentumArrow(Arrow arrow) => _momentumQueue.Enqueue(arrow);
        private Momentum DequeueMomentumArrow() => _momentumQueue.Dequeue() as Momentum;
        private bool MomentumQueueEmpty => _momentumQueue.Count <= 0;

        public void RecallArrow()
        {
            if (MomentumQueueEmpty)
                return;
            
            Debug.Log("T'as cliqué frr");
            Momentum momentumArrowCalled = DequeueMomentumArrow();
            momentumArrowCalled?.Recall();
        }
    }
}
