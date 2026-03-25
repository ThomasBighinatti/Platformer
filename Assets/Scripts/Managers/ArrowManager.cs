using System;
using System.Collections.Generic;
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

        [SerializeField] private List<ArrowGroupData> arrowGroupDatas;

        private ArrowGroupData _currentArrowGroupData;
        public ArrowGroupData CurrentArrowGroupData
        {
            get => _currentArrowGroupData;
            set
            {
                _currentArrowGroupData = value;
                FillArrowList();
            } 
        }

        private List<ArrowType> CurrentArrowTypes { get; set; }

        private void FillArrowList()
        {
            CurrentArrowTypes = CurrentArrowGroupData.ArrowTypeList;
        }

        private void Start()
        {
            CurrentArrowGroupData = arrowGroupDatas[0];
        }

        
        private ArrowData GetArrowDataByType(ArrowType arrowType)
        {
            return arrowType switch
            {
                ArrowType.Momentum => expr,
                _ => throw new ArgumentOutOfRangeException(nameof(arrowType), arrowType, null)
            }
        }
        
        
        
        
    }
}
