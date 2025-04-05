using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;
    public Camera rearCamera;

    public enum CameraType { Main, Rear }
    private CameraType currentCamera = CameraType.Main;

    private void Start()
    {
        SwitchCamera(CameraType.Main);
    }

    public void SwitchCamera(CameraType type)
    {
        mainCamera.gameObject.SetActive(type == CameraType.Main);
        rearCamera.gameObject.SetActive(type == CameraType.Rear);
        currentCamera = type;
    }

    // ÇÊ¿ä½Ã toggle
    public void ToggleCamera()
    {
        if (currentCamera == CameraType.Main)
            SwitchCamera(CameraType.Rear);
        else
            SwitchCamera(CameraType.Main);
    }
}

