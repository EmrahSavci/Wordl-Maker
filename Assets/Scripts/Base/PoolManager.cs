using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Threading.Tasks;
public class PoolManager : Singleton<PoolManager>
{
    public List<PoolObject> PoolObjects = new List<PoolObject>();
    public List<PoolParticle> PoolParticles = new List<PoolParticle>();
    [HideInInspector]
    public Transform holdPool;
    protected override void Awake()
    {
        base.Awake();
        holdPool = new GameObject("Pool").transform;
        holdPool.SetParent(transform);
    }

    public void StartPool()
    {
        foreach (var item in PoolObjects)
        {
            item.Setup(holdPool, item.Enum);
        }

        foreach (var item in PoolParticles)
        {
            item.Setup(holdPool, item.Enum);
        }
    }


    public void BackToList(PoolItem poolItem)
    {
        foreach (var item in PoolObjects)
        {
            if (item.Enum == poolItem._PoolEnum)
            {
                item.AddObject(poolItem.gameObject);
                break;
            }
        }
    }

    public void BackToList(ParticleItem poolItem)
    {
        foreach (var item in PoolParticles)
        {
            if (item.Enum == poolItem.Enum)
            {
                item.AddObject(poolItem.gameObject);
                break;
            }
        }
    }

    public void ReLoad()
    {
        foreach (Transform item in holdPool)
        {
            item.gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        EventManager.OnBeforeLoadedLevel += ReLoad;
        EventManager.OnAfterLoadedLevel += CheckEmpty;
    }

    private async void CheckEmpty()
    {
        await Task.Delay(200);
        foreach (var item in PoolObjects)
        {
            item.pool.RemoveAll(x => x == null);
        }
        foreach (var item in PoolParticles)
        {
            item.pool.RemoveAll(x => x == null);
        }


        foreach (var item in PoolObjects)
        {
            item.Setup(holdPool, item.Enum);
        }

        foreach (var item in PoolParticles)
        {
            item.Setup(holdPool, item.Enum);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventManager.OnBeforeLoadedLevel -= ReLoad;
        EventManager.OnAfterLoadedLevel -= CheckEmpty;
    }

#if UNITY_EDITOR
    [Button]
    private void CreateEnums()
    {
        EnumCreator.CreateEnum("PoolObject",
         PoolObjects.Select(x => x.Prefab.name).ToArray());

        for (int i = 0; i < PoolObjects.Count; i++)
        {
            PoolObjects[i].Enum = (Enum_PoolObject)i + 1;
        }

        EnumCreator.CreateEnum("PoolParticle",
        PoolParticles.Select(x => x.Prefab.name).ToArray());

        for (int i = 0; i < PoolParticles.Count; i++)
        {
            PoolParticles[i].Enum = (Enum_PoolParticle)i + 1;
        }
    }
#endif

}

public static class PoolEvents
{
    public static PoolItem GetObject(this Enum_PoolObject poolObject)
    {
        if (poolObject == Enum_PoolObject.Empty) return null;

        foreach (var item in PoolManager.Instance.PoolObjects)
        {

            if (item.Enum == poolObject)
            {
                var obj = item.GetObject();
                obj.SetEnum(poolObject);
                return obj;
            }
        }

        return null;
    }


    public static ParticleItem GetParticle(this Enum_PoolParticle poolParticle)
    {
        if (poolParticle == Enum_PoolParticle.Empty) return null;
        
        foreach (var item in PoolManager.Instance.PoolParticles)
        {

            if (item.Enum == poolParticle)
            {
                var obj = item.GetObject();
                obj.SetEnum(poolParticle);
                return obj;
            }
        }

        return null;
    }


    #region ParticleItem

    public static void SetPosition(this ParticleItem particleItem, Vector3 pos)
    {
        particleItem.transform.position = pos;
    }

    public static void DelayPlay(this ParticleItem particleItem, float delay)
    {
        particleItem.DelayPlay(delay);
    }

    public static void SetRotation(this ParticleItem particleItem, Vector3 eulerAngles)
    {
        particleItem.transform.eulerAngles = eulerAngles;
    }

    public static void SetParent(this ParticleItem particleItem, Transform trans)
    {
        particleItem.transform.parent = trans;
    }

    #endregion

}

[System.Serializable]
public class PoolObject : AbstractPoolObject<PoolItem>
{
    [HideInInspector]
    public Enum_PoolObject Enum;
}
[System.Serializable]
public class PoolParticle : AbstractPoolObject<ParticleItem>
{
    [HideInInspector]
    public Enum_PoolParticle Enum;
}

