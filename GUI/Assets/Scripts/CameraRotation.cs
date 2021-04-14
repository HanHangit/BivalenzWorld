using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField]
    private Camera _camera = default;
    [SerializeField]
    private Camera _orthoCamera = default;
    [SerializeField]
    private Vector3 _defaultPos = new Vector3(-18, 30, -27);
    [SerializeField]
    private Vector3 _defaultRot = new Vector3(35, 90, 0);
    [SerializeField]
    private Vector3 _orthoPos = new Vector3(22, 48, -36);
    [SerializeField]
    private Vector3 _orthoRot = new Vector3(90, 90, 0);
    public GenericEvent<CameraArgs> CameraChangedEvent = new GenericEvent<CameraArgs>();
    private Camera _currentCamera = default;
    private CameraMode _currentCameraMode = CameraMode.Cam3D;

    public CameraMode GetCurrentCamMode()
    {
        return _currentCameraMode;
    }


    public void SetCameraDefault()
    {
        _currentCameraMode = CameraMode.Cam2D;

        CameraChangedEvent.InvokeEvent(new CameraArgs
        {
            Camera = _camera,
            CameraMode = CameraMode.Cam2D
        });
    }

    public void SetCameraOrthogonal()
    {
        _currentCameraMode = CameraMode.Cam3D;

        CameraChangedEvent.InvokeEvent(new CameraArgs
        {
            Camera = _orthoCamera,
            CameraMode = CameraMode.Cam3D
        });
    }

    private void SetCamera(Vector3 pos, Vector3 rot)
    {
        _camera.transform.position = pos;
        _camera.transform.rotation = Quaternion.Euler(rot);
    }

    public Camera GetCurrentCamera()
    {
        return _currentCamera;
    }

    public class CameraArgs
    {
        public Camera Camera;

        public CameraMode CameraMode;
    }

    public enum CameraMode
    {
        Cam2D,
        Cam3D
    }

}
