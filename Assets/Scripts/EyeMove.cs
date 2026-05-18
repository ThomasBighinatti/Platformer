using System.Collections;
using DG.Tweening;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;
// ReSharper disable FunctionRecursiveOnAllPaths

[SelectionBase]
public class EyeMove : MonoBehaviour
{
    
    [Header("Eye Settings")]
    [SerializeField] private float centerDistance;
    [SerializeField, Range(0, 1)] private float offsetDistanceOnLook;
    [SerializeField] private float randomTimeWaitMin;
    [SerializeField] private float randomTimeWaitMax;
    [SerializeField] private bool useShake = true;
    [SerializeField] private float shakeDistance = 5;
    [SerializeField] private float shakeStrength = 0.05f;

    [Header("Eye Parts")]
    [SerializeField] private GameObject pupil;
    [SerializeField] private GameObject shakeParent;
    
    private GameObject _player;
    
    private Vector3 _pupilCenter;
    
    private void Start()
    { 
        _player = LevelManager.Instance.player;
        _pupilCenter = pupil.transform.position;
        StartCoroutine(WaitForLookCycle());
    }
    
    private bool IsInRange => Vector2.Distance(_player.transform.position, transform.position) <= shakeDistance;
    private void LookTowardsPlayer(float shakeTime)
    {
        Vector2 targetPosition = _pupilCenter + (_player.transform.position - pupil.transform.position).normalized * centerDistance;
        targetPosition += Random.insideUnitCircle * offsetDistanceOnLook;

        pupil.transform.DOKill();
        pupil.transform.DOMove(new Vector3(targetPosition.x,targetPosition.y,pupil.transform.position.z), 0.2f).SetEase(Ease.InOutSine);
        if (!IsInRange || !useShake)
            return;
        
        shakeParent.transform.DOKill();
        shakeParent.transform.DOShakePosition(shakeTime,new Vector3(shakeStrength, shakeStrength, 0),15,90f,false,false);
    }

    private IEnumerator WaitForLookCycle()
    {
        while (this)
        {
            float randomTime = Random.Range(randomTimeWaitMin, randomTimeWaitMax);
            LookTowardsPlayer(randomTime);
            yield return new WaitForSeconds(Random.Range(randomTimeWaitMin, randomTimeWaitMax));
        }
    }
    
}
