using UnityEngine;

namespace GameKit.Singleton
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T s_instance;

        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = FindFirstObjectByType<T>();
                    if (s_instance == null)
                    {
                        s_instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    }
                }

                return s_instance;
            }
        }

        public static bool HasInstance => s_instance != null;

        protected virtual void Awake()
        {
            if (s_instance != null && s_instance != this)
            {
                Destroy(gameObject);
                return;
            }

            s_instance = (T)this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }

        protected virtual void OnDestroy()
        {
            if (s_instance == this)
            {
                s_instance = null;
            }
        }
    }
}
