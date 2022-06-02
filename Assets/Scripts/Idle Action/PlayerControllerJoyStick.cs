using UnityEngine;

public class PlayerControllerJoyStick : PlayerController
{
    [SerializeField]
    private float maxDistance;
    private Rigidbody _rigidbody;

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        targetPos = transform.position;
    }

    private void Update()
    {
        if(!Base.IsPlaying()) return;
        JoyStickMove();
    }

    private void JoyStickMove()
    {
        if (JoyStick.Instance == null)
        {
            Debug.LogError("JoyStick is null");
            return;
        }

        var posPower = Vector3.zero;
        posPower += JoyStick.Instance.GetVector();


        if (Vector3.Distance(transform.position, targetPos) < maxDistance)
        {
            targetPos += (posPower * playerData.RotationSpeed) * Time.deltaTime;
        }

        transform.LookAt(targetPos);
        transform.position = Vector3.MoveTowards(transform.position,
        targetPos, Time.deltaTime * playerData.MoveSpeed);
    }

}
