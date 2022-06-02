using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using System.Collections;
using DG.Tweening;
using UnityEditor;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(PoolItem))]
[RequireComponent(typeof(CapsuleCollider))]
public class Enemy : MonoBehaviour
{
    [Title("Stats")]
    public float FarSpeed;
    public float CloseSpeed;
    public int MaxHealth;
    public int Damage;
    public int Money = 1;
    public bool Attack;
    [Title("No Brain")]
    public bool NoBrainRun;
    [ShowIf("NoBrainRun")]
    public float NoBrainRunRange;
    [Title("Generaly")]
    public bool AngleView;
    [ShowIf("AngleView")]
    public float ViewAngle;
    [ShowIf("AngleView")]
    public float ViewDistance;
    [ShowIf("Attack")]
    public bool AttackAfterDead;
    [ShowIf("Attack")]
    public bool AttackAfterDelay;
    [Title("Attack After")]
    [ShowIf("AttackAfterDelay")]
    public float Delay;
    [Title("Range")]
    public float FarDistance;
    [ShowIf("Attack")]
    public float AttackRange;
    protected int health;
    [Title("After Dead")]
    public bool AfterDeadMaterialAnim;
    [ShowIf("AfterDeadMaterialAnim")]
    public Color DeadMaterialColor = Color.black;
    [ShowIf("AfterDeadMaterialAnim")]
    public float DeadMaterialTime;
    protected Color characterColor;

    //Generally
    protected float speed;
    protected Transform target;
    protected Collider _collider;
    protected Animator _animator;
    protected bool isDead;
    protected float animSpeed;
    protected float randomExtraSpeed;
    protected NavMeshAgent _agent;
    protected Renderer _renderer;
    #region Delays

    private IEnumerator DelayDie()
    {
        yield return new WaitForEndOfFrame();
        transform.DOScale(0, 1f).OnComplete(() => gameObject.SetActive(false));
    }

    private IEnumerator DelayAfterAttack()
    {
        yield return new WaitForSeconds(Delay);
        if (AttackAfterDead)
            TakeDamage();
        else isDead = false;
    }

    #endregion

    #region Unity Methods
    void OnEnable()
    {
        EventManager.BeforeFinishGame += WhenFinish;
        Setup();
    }

    void OnDisable()
    {
        EventManager.BeforeFinishGame -= WhenFinish;
        if (EnemySpawnerManager.Instance)
            EnemySpawnerManager.Instance.RemoveEnemy(transform);
    }

    void Update()
    {
        if (isDead | !Base.IsPlaying() | !target) return;

        if (SpeedChanger())
        {
            _animator.SetFloat("Speed", _agent.velocity.magnitude);
            if (!_agent.enabled) return;
            _agent.speed = speed;

            if (!NoBrainDetectDistance())
                _agent.SetDestination(target.position);
            else
                _agent.SetDestination(transform.position + Vector3.forward);
        }
        else
        {
            if (Attack)
                AttackFunc();
        }
    }

    protected virtual void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        characterColor = _renderer.material.color;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _animator.SetInteger("Type", Random.Range(0, 2));
        _collider = GetComponent<Collider>();
    }
    #endregion

    #region Calculations

    private bool SpeedChanger()
    {
        if (DistanceWithPlayer() > FarDistance)
        {
            speed = FarSpeed;
            return true;
        }
        else if (DistanceWithPlayer() > 2)
        {
            speed = CloseSpeed;
            return true;
        }
        else return false;
    }

    private bool NoBrainDetectDistance()
    {
        return DistanceWithPlayer() > NoBrainRunRange & NoBrainRun;
    }

    private float DistanceWithPlayer()
    {
        return Vector3.Distance(new Vector3(0, 0, transform.position.z), new Vector3(0, 0, Player.Instance.transform.position.z));
    }

    #endregion

    #region Methods
    protected virtual void Setup()
    {
        target = FindObjectOfType<Player>().transform;
        transform.DOScale(Vector3.one, 0.5f);
        _renderer.material.color = characterColor;
        health = MaxHealth;
        if (NoBrainRunRange != 0)
            NoBrainRun = true;
        randomExtraSpeed = Random.Range(0.25f, 2f);
        _agent.enabled = true;
        _animator.enabled = true;
        _collider.enabled = true;
        isDead = false;
    }

    protected virtual void AttackFunc()
    {
        if (AngleView)
            if (!CheckAngle()) return;

        isDead = true;
        target.GetComponent<Player>().HealthSystem(Damage);
        _animator.SetTrigger("Attack");
        StartCoroutine("DelayAfterAttack");
    }

    private bool CheckAngle()
    {
        Vector3 targetDir = target.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);
        return angle < ViewAngle;
    }

    internal virtual void TakeDamage(int damage = 1)
    {
        health -= damage;
        if (health > 0 | isDead) return;
        _renderer.material.DOColor(DeadMaterialColor, DeadMaterialTime);
        isDead = true;
        CameraParticle.Coin.PlayCoinEffect(transform.position);
        Datas.Coin.CoinAdd(Money);
        _animator.SetTrigger("Die");
        transform.DOScale(0, 1f).OnComplete(() => gameObject.SetActive(false));
        //StartCoroutine("DelayDie");
    }

    private void WhenFinish(GameStat stat)
    {
        if (stat == GameStat.Win)
        {
            TakeDamage(health);
        }
        else
        {
            _animator.SetTrigger("Victory");

            if (_agent.enabled)
            {
                _agent.SetDestination(transform.position);
                _agent.isStopped = true;
                _agent.enabled = false;
            }
        }
    }



    #endregion
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        ExtensionMethods.DrawDisc(transform.position, FarDistance, Color.green);

        if (Attack)
        {
            ExtensionMethods.DrawDisc(transform.position, AttackRange, Color.red);
        }
        if (NoBrainRun)
        {
            ExtensionMethods.DrawDisc(transform.position, NoBrainRunRange, Color.blue);
        }
        if (_agent)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, _agent.destination);
            Gizmos.color = Color.red;
            Gizmos.DrawCube(_agent.destination, new Vector3(0.5f, 0.5f, 0.5f));
        }
        if (AngleView) { AngleGizmos(); }
    }

    void AngleGizmos()
    {
        float totalFOV = ViewAngle;
        float rayRange = ViewDistance;
        float halfFOV = totalFOV / 2.0f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, leftRayDirection * rayRange);
        Gizmos.DrawRay(transform.position, rightRayDirection * rayRange);
    }
#endif
}
