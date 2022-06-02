using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PoolItem : MonoBehaviour
{
    public Enum_PoolObject _PoolEnum;
    public UnityEvent OnDeath;

    void OnEnable()
    {
        EventManager.OnBeforeLoadedLevel += Kill;
    }

    private void Kill()
    {
        gameObject.SetActive(false);
    }
    public void SetEnum(Enum_PoolObject en)
    {
        _PoolEnum = en;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
    public void SetLocalPosition(Vector3 position)
    {
        transform.localPosition = position;
    }

    public void SetRotation(Vector3 rot)
    {
        transform.eulerAngles = rot;
    }

    public void SetLocalRotation(Vector3 rot)
    {
        transform.localEulerAngles = rot;
    }

    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }

    public void SetLayer(int layer)
    {
        gameObject.layer = layer;
    }

    public void SetTag(string tag)
    {
        gameObject.tag = tag;
    }

    public void SetName(string name)
    {
        gameObject.name = name;
    }

    public void DeActive()
    {
        gameObject.SetActive(false);
    }

    public void AddPower(Vector3 pow)
    {
        GetComponent<Rigidbody>().velocity = pow;
    }

    private void OnDisable()
    {
        EventManager.OnBeforeLoadedLevel -= Kill;
        OnDeath.Invoke();
        PoolManager.Instance.BackToList(this);
    }

}
