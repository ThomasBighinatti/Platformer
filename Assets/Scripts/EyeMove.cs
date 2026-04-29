using System;
using System.Collections;
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
    
    [SerializeField, Range(0, 1)][Obsolete] private float probaLookPlayer;
    [SerializeField, Range(0, 1)][Obsolete] private float offsetDistance;
    
    [SerializeField] private float detectionDistance;
    private GameObject _player;
    private Vector3 _pupilCenter;
    
    private void Start()
    { 
        _player = LevelManager.Instance.player;
        _pupilCenter = pupil.transform.position;
        StartCoroutine(WaitForLookCycle());
        //Debug.DrawRay(transform.position,Vector3.left * detectionDistance, Color.brown,float.MaxValue);
    }

    private bool IsInRange => Vector2.Distance(_player.transform.position, transform.position) <= detectionDistance;

    private void LookTowardsPlayer()
    {
        pupil.transform.position = _pupilCenter + (_player.transform.position - pupil.transform.position).normalized * centerDistance;
        pupil.transform.position += (Vector3)Random.insideUnitCircle * offsetDistanceOnLook;
    }

    private void VariationCenterLook()
    {
        pupil.transform.position = _pupilCenter + (Vector3)Random.insideUnitCircle * offsetDistance;
    }

    /*private IEnumerator WaitForLookCycle()
    {
        //LookTowardsPlayer();
        yield return new WaitForSeconds(Random.Range(randomTimeWaitMin, randomTimeWaitMax));
        StartCoroutine(WaitForLookCycle());
    }*/
    
    private IEnumerator WaitForLookCycle()
    {
        if (IsInRange)
        {
            if (Random.value <= probaLookPlayer)
            {
                LookTowardsPlayer();
                yield return new WaitForSeconds(Random.Range(randomTimeWaitMin, randomTimeWaitMax));
                StartCoroutine(WaitForLookCycle());
                yield break;
            }
        }
        VariationCenterLook();
        yield return new WaitForSeconds(Random.Range(randomTimeWaitMin, randomTimeWaitMax));
        StartCoroutine(WaitForLookCycle());
    }
    
}
