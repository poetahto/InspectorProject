using UnityEngine;

namespace DefaultNamespace
{
    public class TwistInteractable : Interactable
    {
        public float accel = 5;
        public float maxRotateSpeed = 1;
        public AK.Wwise.RTPC twistRtpc;
        public AK.Wwise.Event twistEvent;
        private Vector3 _rotation;
        private Camera _camera;
        private float _currentRotateSpeed;
        private bool _canRotate;

        private void Start()
        {
            _rotation = transform.rotation.eulerAngles;
            _camera = Camera.main;
            twistEvent.Post(gameObject);
        }

        public override void InteractEnd()
        {
            base.InteractEnd();
            _canRotate = false;
        }
        
        public override void InteractUpdate()
        {
            base.InteractUpdate();

            Vector3 interactableScreenPoint = _camera.WorldToScreenPoint(transform.position);
            Vector3 mouseScreenPoint = Input.mousePosition;
            Vector3 offset = mouseScreenPoint - interactableScreenPoint;
            Vector3 mouseDelta = new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"), 0);

            _canRotate = false;
            if ((offset.x > 0 && offset.y > 0) && (mouseDelta.x > 0 && mouseDelta.y < 0)) _canRotate = true;
            if ((offset.x > 0 && offset.y < 0) && (mouseDelta.x < 0 && mouseDelta.y < 0)) _canRotate = true;
            if ((offset.x < 0 && offset.y < 0) && (mouseDelta.x < 0 && mouseDelta.y > 0)) _canRotate = true;
            if ((offset.x < 0 && offset.y > 0) && (mouseDelta.x > 0 && mouseDelta.y > 0)) _canRotate = true;
        }

        private void Update()
        {
            _currentRotateSpeed = Mathf.Lerp(_currentRotateSpeed, _canRotate ? maxRotateSpeed : 0, accel * Time.deltaTime);
            twistRtpc.SetValue(gameObject, (_currentRotateSpeed / maxRotateSpeed) * 100);
            _rotation.z += _currentRotateSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(_rotation);
        }
    }
}
