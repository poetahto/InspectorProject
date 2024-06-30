using UnityEngine;

namespace DefaultNamespace
{
    public abstract class Interactable : MonoBehaviour
    {
        public virtual void InteractStart() {}
        public virtual void InteractUpdate() {}
        public virtual void InteractEnd() {}

        public bool IsInteractable { get; set; } = true;

        protected static void ForceStopInteraction()
        {
            FindAnyObjectByType<InputController>().StopInteracting();
        }
    }

}
