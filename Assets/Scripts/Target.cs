using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Vector3 firstRotation;
    [SerializeField] private float durationTime=3f;
    private void Start()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
        firstRotation = transform.rotation.ToEuler();
        transform.DORotate(Vector3.zero, 0.2f, RotateMode.Fast);
        StartCoroutine(DurationLifeTime());
    }


    public void OnHit()
    {
        transform.DORotate(new Vector3(90, 0, 0), 0.2f, RotateMode.Fast).OnComplete(()=> KillTarget());
        Debug.Log(" Hit To Target");
    }

    IEnumerator DurationLifeTime()
    {
        yield return new WaitForSeconds(durationTime);
        transform.DORotate(new Vector3(90,0,0), 0.2f, RotateMode.Fast).OnComplete(() => KillTarget());
    }

    private void KillTarget()
    {
        Destroy(gameObject);
        Debug.Log(gameObject.name + " Destroyed !! ");
    }
}
