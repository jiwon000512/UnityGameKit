using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using GameKit.Singleton;

namespace GameKit.Pooling
{
    public sealed class PoolManager : MonoSingleton<PoolManager>
    {
        private readonly Dictionary<GameObject, ObjectPool<GameObject>> m_pools =
            new Dictionary<GameObject, ObjectPool<GameObject>>();

        public GameObject Get(GameObject prefab, Transform parent = null)
        {
            GameObject instance = GetPool(prefab).Get();
            instance.transform.SetParent(parent, false);
            return instance;
        }

        public T Get<T>(GameObject prefab, Transform parent = null) where T : Component
        {
            return Get(prefab, parent).GetComponent<T>();
        }

        public void Release(GameObject instance)
        {
            GameObject prefab = instance.GetComponent<PooledObject>().Prefab;
            GetPool(prefab).Release(instance);
        }

        private ObjectPool<GameObject> GetPool(GameObject prefab)
        {
            if (!m_pools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
            {
                pool = new ObjectPool<GameObject>(
                    () => Create(prefab),
                    instance => instance.SetActive(true),
                    instance => Park(instance),
                    Destroy);
                m_pools[prefab] = pool;
            }

            return pool;
        }

        private GameObject Create(GameObject prefab)
        {
            GameObject instance = Instantiate(prefab, transform);
            instance.AddComponent<PooledObject>().Prefab = prefab;
            return instance;
        }

        private void Park(GameObject instance)
        {
            instance.SetActive(false);
            instance.transform.SetParent(transform, false);
        }
    }
}
