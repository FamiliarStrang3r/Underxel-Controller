using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    private MeshRenderer meshRenderer = null;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Select(bool select)
    {
        meshRenderer.material.color = select ? Color.green : Color.white;
    }
}
