﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField]
    private Camera _camera = default;
    [SerializeField]
    private Vector3 _defaultPos = new Vector3(-18, 30, -27);
    [SerializeField]
    private Vector3 _defaultRot = new Vector3(35, 90, 0);


    [SerializeField]
    private Vector3 _orthoPos = new Vector3(22, 48, -36);
    [SerializeField]
    private Vector3 _orthoRot = new Vector3(90, 90, 0);


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetCamera(_defaultPos, _defaultRot);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SetCamera(_orthoPos, _orthoRot);
        }
    }

    private void SetCamera(Vector3 pos, Vector3 rot)
    {
        _camera.transform.position = pos;
        _camera.transform.rotation = Quaternion.Euler(rot) ;
    }
}
