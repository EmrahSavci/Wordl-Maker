using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Lean.Touch;
public class PlayerControllerRunner : PlayerController
{
    private Vector2 ClampX;
    private Vector2 ClampY;
    protected override void Awake()
    {
        base.Awake();
        ClampX = new Vector2(playerData.ClampX.x, playerData.ClampX.y);
        ClampY = new Vector2(playerData.ClampY.x, playerData.ClampY.y);
    }


    public void Move(Vector2 target)
    {
        if (!playerData.Move | !Base.IsPlaying()) return;
        RotationMove(target);

        var targetToVector3 = Vector3.zero;
        if (playerData.XMoving) targetToVector3.x = target.x;
        if (playerData.YMoving) targetToVector3.y = target.y + offsetY;
        targetToVector3.z = 0;

        var check = targetPos + targetToVector3;

        if (playerData.YMoving) check.y = Mathf.Clamp(check.y, ClampY.x, ClampY.y);
        else check.y = offsetY;
        if (playerData.XMoving) check.x = Mathf.Clamp(check.x, ClampX.x, ClampX.y);

        targetPos = check;
    }

    protected override void OnFingerUp(LeanFinger obj)
    {
        base.OnFingerUp(obj);
        if (!playerData.MoveRotation | !Base.IsPlaying()) return;
        targetRot = Vector3.zero;
    }

    public void RotationMove(Vector2 rot)
    {
        if (playerData.MoveRotation)
        {
            targetRot = new Vector3(0, playerData.RotationSpeed * rot.x, 0);

            if (Mathf.Abs(targetRot.y) > playerData.MaxYangle)
            {
                if (targetRot.y > 0) targetRot.y = playerData.MaxYangle;
                else targetRot.y = -playerData.MaxYangle;
            }
        }
    }

    private void Update()
    {
        if (!Base.IsPlaying()) return;
        transform.position = Vector3.Lerp(transform.position, new Vector3(targetPos.x, 0.5f, targetPos.z), playerData.MoveSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRot),
        playerData.RotationTurnSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (!Base.IsPlaying()) return;
        if (playerData.ZMoving) targetPos.z += Time.fixedDeltaTime * playerData.MoveSpeed;
    }

    private void FinishLine()
    {
        if (!Base.IsPlaying()) return;
        
    }


}
