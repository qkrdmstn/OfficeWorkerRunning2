using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : Item
{
    private MeshRenderer meshRenderer;
    public GameObject effect;

    public void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void ChangeColor()
    {
        meshRenderer.materials[1].color = Color.white;
        effect.SetActive(false);
    }
}
