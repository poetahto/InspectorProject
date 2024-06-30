using UnityEngine;

namespace DefaultNamespace
{
    public class CameraOrbitController : MonoBehaviour
    {
        public float orbitSpeed = 15;
        public float zIncreaseSpeed = 5;
        public float minZDistance = 3;
        public float maxZDistance = 10;
        public Transform zDistanceTransform;
        public Transform rotatingTransform;

        private Vector2 _targetRotation;
        private float _targetZDistance;

        private void Start()
        {
            _targetZDistance = zDistanceTransform.position.z;
        }

        public void ProcessMouseInput()
        {
            Vector2 mouseDelta = new Vector2(
                Input.GetAxisRaw("Mouse X"),
                Input.GetAxisRaw("Mouse Y")
            );
            
            _targetRotation += mouseDelta * orbitSpeed;
            _targetRotation.y = Mathf.Clamp(_targetRotation.y, -90, 90);
        }
        
        private void Update()
        {
            _targetZDistance -= Input.mouseScrollDelta.y * zIncreaseSpeed;
            _targetZDistance = Mathf.Clamp(_targetZDistance, minZDistance, maxZDistance);

            rotatingTransform.localRotation = Quaternion.Lerp(rotatingTransform.localRotation, Quaternion.Euler(_targetRotation.y, _targetRotation.x, 0), orbitSpeed * Time.deltaTime);
            
            Vector3 pos = zDistanceTransform.localPosition;
            pos.z = Mathf.Lerp(pos.z, _targetZDistance, orbitSpeed * Time.deltaTime);
            zDistanceTransform.localPosition = pos;
        }
    }
}
