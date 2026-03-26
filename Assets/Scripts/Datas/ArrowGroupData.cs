using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Datas
{
    [CreateAssetMenu(fileName = "ArrowGroupData", menuName = "Scriptable Objects/ArrowGroupData")]
    public class ArrowGroupData : ScriptableObject
    {
        [field:SerializeField] public List<ArrowType> ArrowTypeList { get; private set; }
    }
}
