using UnityEngine;

namespace GameKit.Pooling
{
    public sealed class PooledObject : MonoBehaviour
    {
        public GameObject Prefab { get; internal set; }
    }
}
