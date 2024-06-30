using UnityEngine;

namespace DefaultNamespace
{
    public class TwistInteractable : Interactable
    {
        public float rotateSpeed = 1;
        public AK.Wwise.RTPC twistRtpc;
        public AK.Wwise.Event twistEvent;
        private Vector3 _rotation;
        private Camera _camera;

        private void Start()
        {
            _rotation = transform.rotation.eulerAngles;
            _camera = Camera.main;
        }

        public override void InteractUpdate()
        {
            base.InteractUpdate();

            Vector3 interactableScreenPoint = _camera.WorldToScreenPoint(transform.position);
            Vector3 mouseScreenPoint = Input.mousePosition;
            Vector3 offset = mouseScreenPoint - interactableScreenPoint;
            Vector3 mouseDelta = new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"), 0);

            bool canRotate = false;
            if ((offset.x > 0 && offset.y > 0) && (mouseDelta.x > 0 && mouseDelta.y < 0)) canRotate = true;
            if ((offset.x > 0 && offset.y < 0) && (mouseDelta.x < 0 && mouseDelta.y < 0)) canRotate = true;
            if ((offset.x < 0 && offset.y < 0) && (mouseDelta.x < 0 && mouseDelta.y > 0)) canRotate = true;
            if ((offset.x < 0 && offset.y > 0) && (mouseDelta.x > 0 && mouseDelta.y > 0)) canRotate = true;

            if (canRotate)
            {
                _rotation.x -= Mathf.Max(Mathf.Abs(mouseDelta.x), Mathf.Abs(mouseDelta.y)) * rotateSpeed;
                transform.rotation = Quaternion.Euler(_rotation);
            }
        }
    }
}
