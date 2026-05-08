using System;
using System.Collections;
using DG.Tweening;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

[SelectionBase]
public class EyeMove : MonoBehaviour
{

    [SerializeField] private GameObject pupil;
    [SerializeField] private float centerDistance;
    [SerializeField, Range(0, 1)] private float offsetDistanceOnLook;
    [SerializeField] private float randomTimeWaitMin;
    [SerializeField] private float randomTimeWaitMax;
    
    private GameObject _player;
    private Vector3 _pupilCenter;
    
    private void Start()
    { 
        _player = LevelManager.Instance.player;
        _pupilCenter = pupil.transform.position;
        StartCoroutine(WaitForLookCycle());
    }
    
    private void LookTowardsPlayer()
    {
        
        Vector2 targetPosition = _pupilCenter + (_player.transform.position - pupil.transform.position).normalized * centerDistance;
        targetPosition += Random.insideUnitCircle * offsetDistanceOnLook;

        pupil.transform.DOKill();
        pupil.transform.DOMove(new Vector3(targetPosition.x,targetPosition.y,pupil.transform.position.z), 0.2f).SetEase(Ease.InOutSine);
    }

    private IEnumerator WaitForLookCycle()
    {
        LookTowardsPlayer();
        yield return new WaitForSeconds(Random.Range(randomTimeWaitMin, randomTimeWaitMax));
        StartCoroutine(WaitForLookCycle());
    }
}
