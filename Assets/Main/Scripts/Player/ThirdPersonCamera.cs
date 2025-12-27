using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform _cameraRoot;
    [SerializeField] private Vector3 _posOffset;

    [SerializeField] private GameObject _lookAtObj;

    private void LateUpdate()
    {
        if (_cameraRoot != null)
        {
            transform.position = _cameraRoot.TransformPoint(_posOffset);

            transform.LookAt(_lookAtObj.transform);
        }
    }
}
