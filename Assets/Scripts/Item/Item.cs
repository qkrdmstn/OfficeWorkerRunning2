using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float rotateSpeed;

    private void Update()
    {
        Rotate();
    }

    protected virtual void Rotate()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

}
