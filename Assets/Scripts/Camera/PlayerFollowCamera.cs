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

    public void RotateCameraAroundPlayer()
    {
        StartCoroutine(RotateCameraAroundPlayerCoroutine());
    }

    public IEnumerator RotateCameraAroundPlayerCoroutine()
    {
        CinemachineVirtualCamera vcamPlayerFollow = GetComponent<CinemachineVirtualCamera>();
        vcamPlayerFollow.Follow = null;

        float t = 0.0f;
        while (true)
        {
            t += Time.fixedDeltaTime;
            this.transform.RotateAround(target.position, target.up, 180 * Time.fixedDeltaTime);
            if (t > 1f)
            {
                Vector3 p = target.position + new Vector3(0, 1.5f, 4f);
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
