using UnityEngine;

namespace DefaultNamespace
{
    public abstract class Interactable : MonoBehaviour
    {
        public abstract void InteractStart();
        public abstract void InteractUpdate();
        public abstract void InteractEnd();
    }
}
