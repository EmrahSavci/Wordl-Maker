using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "HyperBase/PlayerData", order = 0)]
public class PlayerData : Settings<PlayerData>
{
    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed => moveSpeed;
    [SerializeField]
    private float rotationTurnSpeed;
    public float RotationTurnSpeed => rotationTurnSpeed;
    [SerializeField]
    private float rotationSpeed;
    public float RotationSpeed => rotationSpeed;
    [SerializeField]
    private bool controllReverse;
    public bool ControlReverse => controllReverse;


    [Header("Limits")]
    [SerializeField]
    private float maxYangle;
    public float MaxYangle => maxYangle;

    [SerializeField]
    private Vector2 clampX;
    public Vector2 ClampX => clampX;
    [SerializeField]
    private Vector2 clampY;
    public Vector2 ClampY => clampY;

    [Header("Locks")]
    [SerializeField]
    private bool move;
    public bool Move => move;
    public bool MoveRotation => moveRotation;
    [SerializeField]
    private bool moveRotation;

    [SerializeField]
    private bool yMoving = false;
    public bool YMoving => yMoving;
    [SerializeField]
    private bool xMoving = true;
    public bool XMoving => xMoving;
    [SerializeField]
    private bool zMoving = false;
    public bool ZMoving => zMoving;


}