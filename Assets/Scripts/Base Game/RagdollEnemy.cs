using UnityEngine;

public class RagdollEnemy : Enemy
{
    private Rigidbody[] rigidbodies;
    private Collider[] colliders;
    [SerializeField]
    private Rigidbody spine;
    protected override void Setup()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        SetBodysKinematic(true);

        base.Setup();
    }

    internal override void TakeDamage(int damage = 1)
    {
        base.TakeDamage(damage);
        if(isDead)
            SetBodysKinematic(false);
    }

    private void SetBodysKinematic(bool val)
    {
        _animator.enabled = val;
        _collider.enabled = val;
        _agent.enabled = val;

        foreach (var rb in rigidbodies)
        {
            if (rb.transform != transform)
            {
                rb.isKinematic = val;
                rb.GetComponent<Collider>().isTrigger = val;
            }
        }

        if(!val) spine.AddForce(new Vector3(0,0.3f,1) * 50,ForceMode.VelocityChange);
    }
}
