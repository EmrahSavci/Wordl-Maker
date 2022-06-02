using UnityEngine;
using Lean.Touch;
using System;

public abstract class PlayerController : MonoBehaviour
{
    protected PlayerData playerData => PlayerData.Current;
    protected Vector3 firstPosition;

    [SerializeField]
    protected Vector3 targetPos = Vector3.zero;
    [SerializeField]
    protected Vector3 targetRot = Vector3.zero;
    public float offsetY = 0.5f;

    protected virtual void Awake()
    {
        firstPosition = transform.position;
    }

    public void ResetPos()
    {
        targetPos = firstPosition;
        transform.position = firstPosition;
    }

    protected virtual void OnEnable()
    {
        LeanTouch.OnFingerUp += OnFingerUp;
        LeanTouch.OnFingerDown += OnFingerDown;
    }


    protected virtual void OnDisable()
    {
        LeanTouch.OnFingerUp -= OnFingerUp;
        LeanTouch.OnFingerDown -= OnFingerDown;
    }

    protected virtual void OnFingerDown(LeanFinger obj)
    {
        Debug.Log("Touch Down");
    }

    protected virtual void OnFingerUp(LeanFinger obj)
    {
        Debug.Log("Touch Up");
    }


    //Gizmos for targetPos
    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPos, 0.5f);

        //gizmos for distance between player and targetPos
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, targetPos);
    }
}
