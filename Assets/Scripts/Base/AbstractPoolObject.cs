using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractPoolObject<T> where T : Component
{
    public GameObject Prefab;
    public List<GameObject> pool = new List<GameObject>();
    public int SpawnCount = 25;
    public bool isActive = false;

    public void AddObject(GameObject obj)
    {
        pool.Add(obj);
        obj.SetActive(false);
    }

    public void Setup(Transform parent = null, Enum_PoolObject en = Enum_PoolObject.Empty)
    {
        var diffCount = SpawnCount - pool.Count;
        if (diffCount <= 0) return;

        for (int i = 0; i < diffCount; i++)
        {
            var spawned = GameObject.Instantiate(Prefab, parent);

            var poolItem = spawned.GetComponent<PoolItem>();
            if (!poolItem) poolItem = spawned.AddComponent<PoolItem>();

            poolItem._PoolEnum = en;

            //pool.Add(spawned);
            spawned.SetActive(false);
        }
    }

    public void Setup(Transform parent = null, Enum_PoolParticle en = Enum_PoolParticle.Empty)
    {
        var diffCount = SpawnCount - pool.Count;
        if (diffCount <= 0) return;

        for (int i = 0; i < diffCount; i++)
        {
            var spawned = GameObject.Instantiate(Prefab, parent);

            var poolItem = spawned.GetComponent<ParticleItem>();
            if (!poolItem) poolItem = spawned.AddComponent<ParticleItem>();

            poolItem.Enum = en;

            //pool.Add(spawned);
            spawned.SetActive(false);
        }
    }

    public void Check()
    {
        // var backUp = pool;

        // foreach (var item in backUp)
        // {
        //     if (item == null)
        //     {
        //         Debug.Log("Null");
        //         pool.RemoveAt(pool.IndexOf(item));
        //     }
        // }
    }

    public T GetObject()
    {
        GameObject obj = null;

        if (pool.Count > 0)
        {
            obj = pool[0];
            pool.RemoveAt(0);
            obj.SetActive(true);

            return obj.GetComponent<T>();
        }
        else
        {
            obj = GameObject.Instantiate(Prefab, PoolManager.Instance.holdPool);
            obj.SetActive(true);

            return obj.GetComponent<T>();
        }

    }
}