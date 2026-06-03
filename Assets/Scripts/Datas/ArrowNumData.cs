using System.Collections.Generic;
using UnityEngine;


namespace Datas
{
    [CreateAssetMenu(fileName = "ArrowNumData", menuName = "Scriptable Objects/ArrowNumData")]
    public class ArrowNumData : ScriptableObject
    {
        [field:SerializeField] public List<int> ArrowNumList { get; private set; }
    }
}
