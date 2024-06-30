using UnityEngine;

namespace DefaultNamespace
{
    public class InputController : MonoBehaviour
    {
        public CameraOrbitController orbitController;
        public Camera mainCamera;

        private readonly RaycastHit[] _results = new RaycastHit[100];
        private Interactable _currentInteractable;
        private bool _hasCurrentInteractable;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Ray mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);
                int hits = Physics.RaycastNonAlloc(mouseRay, _results);
                bool foundInteractable = false;

                for (int i = 0; i < hits; i++)
                {
                    if (_results[i].collider.TryGetComponent(out Interactable interactable))
                    {
                        if (_hasCurrentInteractable) _currentInteractable.InteractEnd();
                        _currentInteractable = interactable;
                        _currentInteractable.InteractStart();
                        _hasCurrentInteractable = true;
                        foundInteractable = true;
                        break;
                    }
                }

                if (foundInteractable == false)
                {
                    if (_hasCurrentInteractable) _currentInteractable.InteractEnd();
                    _hasCurrentInteractable = false;
                    _currentInteractable = null;
                }
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if (_hasCurrentInteractable) _currentInteractable.InteractEnd();
                _hasCurrentInteractable = false;
                _currentInteractable = null;
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (!_hasCurrentInteractable)
                    orbitController.ProcessMouseInput();
                else _currentInteractable.InteractUpdate();
            }
        }
    }
}
