using UnityEngine;

namespace DefaultNamespace
{
    public abstract class Interactable : MonoBehaviour
    {
        public virtual void InteractStart() {}
        public virtual void InteractUpdate() {}
        public virtual void InteractEnd() {}
    }

}
