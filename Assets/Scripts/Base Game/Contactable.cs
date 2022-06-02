using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Contactable : MonoBehaviour
{
    [Title("Generaly")]
    public int Value = 1;
    public bool MakeTrigger = true;
    public Enum_Audio Audio;
    public Enum_PoolParticle Particle;
    public LayerMask DetectedMask;
    [Title("Contact After")]
    public bool AfterDestory;
    [ShowIf("AfterDestory")]
    public float DestoryDelay;
    [Title("Effect")]
    
//
    private Collider _collider;

    private void Awake()
    {
        Setup();
    }

    protected virtual void Setup()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = MakeTrigger;
    }

    protected virtual void Contant(GameObject _gObject)
    {
        if (Audio != Enum_Audio.Empty)
            Audio.Play();
        if (Particle != Enum_PoolParticle.Empty)
            Particle.GetParticle().SetPosition(transform.position);
        if (AfterDestory)
            StartCoroutine("DelayDestory");
    }

    IEnumerator DelayDestory()
    {
        yield return new WaitForSeconds(DestoryDelay);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ExtensionMethods.CheckLayer(other.gameObject, DetectedMask))
        {
            if (!MakeTrigger | !Base.IsPlaying()) return;
            Contant(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (ExtensionMethods.CheckLayer(other.gameObject, DetectedMask))
        {
            if (MakeTrigger | !Base.IsPlaying()) return;
            Contant(other.gameObject);
        }
    }

}
