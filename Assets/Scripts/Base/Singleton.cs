
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{

    private static T _instance;

    #region Life Cycle

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    protected virtual void OnDisable()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    #endregion


    #region Pattern

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();
                }
            }

            return _instance;
        }
    }

    #endregion
}
