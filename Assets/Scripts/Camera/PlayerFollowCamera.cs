using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    public Transform target;
    public bool flag;

    public void Awake()
    {
        flag = false;
    }

    public void Update()
    {
        if (!flag)
        {
            Camera.main.transform.position = transform.position;
            Camera.main.transform.rotation = transform.rotation;
            flag = true;
        }
    }
}
