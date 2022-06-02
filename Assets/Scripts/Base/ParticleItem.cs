using System.Threading.Tasks;
using UnityEngine;

public class ParticleItem : MonoBehaviour
{
    [HideInInspector]
    public Enum_PoolParticle Enum;
    private ParticleSystem _particleSystem;
    public void SetEnum(Enum_PoolParticle en)
    {
        Enum = en;
    }

    private void OnDisable() {
        PoolManager.Instance.BackToList(this);
    }

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        _particleSystem.Play();
    }

    public async void DelayPlay(float delay)
    {
        _particleSystem.Stop();
        _particleSystem.Clear();

        await Task.Delay((int)(delay * 1000));

        _particleSystem.Play();
    }

    public void SetPosition()
    {
        transform.position = Vector3.zero;
    }
}
