using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class TwistInteractable : Interactable
    {
        public Vector3 rotationAxis = Vector3.right;
        public float maxRotations = 360 * 2;
        public bool lockOnFinish;
        
        [Header("GameFeel")]
        public float accel = 5;
        public float maxRotateSpeed = 1;
        
        [Header("Audio")]
        public AK.Wwise.RTPC twistRtpc;
        public AnimationCurve rtpcCurve;
        public AK.Wwise.Event twistEvent;

        public float PercentComplete { get; private set; }
        
        private Vector3 _rotation;
        private Camera _camera;
        private float _currentRotateSpeed;
        private float _rotationDirection;

        private void Start()
        {
            _rotation = transform.rotation.eulerAngles;
            _camera = Camera.main;
            twistEvent.Post(gameObject);
        }

        public override void InteractEnd()
        {
            base.InteractEnd();
            _rotationDirection = 0;
        }
        
        public override void InteractUpdate()
        {
            if (PercentComplete >= 1 && lockOnFinish)
                return;
            
            base.InteractUpdate();

            Vector3 interactableScreenPoint = _camera.WorldToScreenPoint(transform.position);
            Vector3 mouseScreenPoint = Input.mousePosition;
            Vector3 offset = mouseScreenPoint - interactableScreenPoint;
            Vector3 mouseDelta = new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"), 0);

            // weird math for figuring out rotation directions, based on mouse movement
            _rotationDirection = 0;
            
            if ((offset.x > 0 && offset.y > 0) && (mouseDelta.x > 0 && mouseDelta.y < 0)) _rotationDirection = 1;
            if ((offset.x > 0 && offset.y < 0) && (mouseDelta.x < 0 && mouseDelta.y < 0)) _rotationDirection = 1;
            if ((offset.x < 0 && offset.y < 0) && (mouseDelta.x < 0 && mouseDelta.y > 0)) _rotationDirection = 1;
            if ((offset.x < 0 && offset.y > 0) && (mouseDelta.x > 0 && mouseDelta.y > 0)) _rotationDirection = 1;
            
            if ((offset.x > 0 && offset.y > 0) && (mouseDelta.x < 0 && mouseDelta.y > 0)) _rotationDirection = -1;
            if ((offset.x > 0 && offset.y < 0) && (mouseDelta.x > 0 && mouseDelta.y > 0)) _rotationDirection = -1;
            if ((offset.x < 0 && offset.y < 0) && (mouseDelta.x > 0 && mouseDelta.y < 0)) _rotationDirection = -1;
            if ((offset.x < 0 && offset.y > 0) && (mouseDelta.x < 0 && mouseDelta.y < 0)) _rotationDirection = -1;
        }

        private void Update()
        {
            PercentComplete = _rotation.magnitude / maxRotations;
            PercentComplete = Mathf.Clamp01(PercentComplete);

            if (PercentComplete >= 1 && IsInteractable)
            {
                IsInteractable = false;
                ForceStopInteraction();
            }
            
            // animate rotation speed and apply it
            _currentRotateSpeed = Mathf.Lerp(_currentRotateSpeed, _rotationDirection * maxRotateSpeed, accel * Time.deltaTime);
            _rotation += rotationAxis * (_currentRotateSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(_rotation);
                
            // audio
            twistRtpc.SetValue(gameObject, rtpcCurve.Evaluate(Mathf.Abs(_currentRotateSpeed / maxRotateSpeed)) * 100);
        }
    }
}
