using System;
using System.Collections.Generic;
using Arrows;
using Datas;
using UnityEngine;
using UnityEngine.Serialization;

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
        
        [SerializeField] private List<ArrowGroupData> arrowGroupDatas;
        
        [SerializeField] private Momentum momentumPrefab;

        [SerializeField] private Transform playerTransform;
        public static Transform PlayerTransform;
        
        private void Start()
        {
            CurrentArrowGroupData = arrowGroupDatas[0];
            
            _bowSpriteRenderer = bow.GetComponent<SpriteRenderer>();
            PlayerTransform = playerTransform;
        }

        public ArrowGroupData CurrentArrowGroupData { get; set; }

        private Arrow GetArrowByType(ArrowType arrowType)
        {
            return arrowType switch
            {
                ArrowType.Momentum => momentumPrefab,
                _ => throw new ArgumentOutOfRangeException(nameof(arrowType), arrowType, null)
            };
        }

        private Arrow CurrentArrowScript { get; set; }

        public bool ArrowScriptIsNull()
        {
            return CurrentArrowScript is null;
        }

        private int _arrowNum;
        
        private Vector2 _lookingTowards = Vector2.right;
        public Vector2 LookingTowards
        {
            get => _lookingTowards;
            set
            {
                _lookingTowards = value;
                
                float angle = Mathf.Atan2(-_lookingTowards.y, _lookingTowards.x) * Mathf.Rad2Deg;
                Vector3 rotation = new Vector3(0, 0, angle);

                if (!ArrowScriptIsNull())
                {
                    CurrentArrowScript.transform.eulerAngles = rotation;
                    bow.gameObject.transform.eulerAngles = rotation;
                }
            }
        }

        [SerializeField] private GameObject bow;
        [SerializeField] private Sprite bowPlaceHolder1;
        [SerializeField] private Sprite bowPlaceHolder2;
        private SpriteRenderer _bowSpriteRenderer;

        public void CreateArrow()
        {
            Debug.Log("Shoot");
            if (_arrowNum < CurrentArrowGroupData.ArrowTypeList.Count)
            {
                CurrentArrowScript = GetArrowByType(CurrentArrowGroupData.ArrowTypeList[_arrowNum]);
            }
            else
            {
                Debug.Log("no more arrows");
                CurrentArrowScript = GetArrowByType(CurrentArrowGroupData.ArrowTypeList[0]);
            }

            GameObject arrowCreation = Instantiate(CurrentArrowScript.gameObject,bow.transform);
            CurrentArrowScript = arrowCreation.GetComponent<Arrow>();

            float angle = Mathf.Atan2(LookingTowards.y, LookingTowards.x) * Mathf.Rad2Deg;
            Vector3 rotation = new Vector3(0, 0, angle);

            arrowCreation.transform.eulerAngles = rotation;
            bow.gameObject.transform.eulerAngles = rotation;
                
            //Placeholder
            _bowSpriteRenderer.sprite = bowPlaceHolder2;
        }

        public Queue<Arrow> MomentumQueue = new Queue<Arrow>();


        public void ShootArrow()
        {
            CurrentArrowScript.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            CurrentArrowScript.gameObject.transform.parent = null;
            CurrentArrowScript.CanStartMoving = true;
            
            CurrentArrowScript = null;

            _arrowNum++;
                
            // Placeholder 
            _bowSpriteRenderer.sprite = bowPlaceHolder1;
            
        }

        public void RecallArrow()
        {
            if (MomentumQueue.Count <= 0)
                return;
            Debug.Log("T'as cliqué frr");
            Momentum momentumArrowCalled = MomentumQueue.Dequeue() as Momentum;
            if (momentumArrowCalled != null)
            {
                momentumArrowCalled.Recall();
                return;
            }
            Debug.Log("ya plu");
        }
        
    }
}
