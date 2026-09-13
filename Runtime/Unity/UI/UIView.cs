using UnityEngine;

namespace GameKit.UI
{
    public abstract class UIView : MonoBehaviour
    {
        public virtual void OnOpen(object args)
        {
        }

        public virtual void OnClose()
        {
        }
    }
}
