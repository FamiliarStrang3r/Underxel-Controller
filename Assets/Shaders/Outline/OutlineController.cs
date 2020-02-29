using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OutlineController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr = null;
    [SerializeField] private Color outlineColor = Color.white;
    [SerializeField] private bool outline = false;

    private void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnValidate()
    {
        MakeOutline(outline);
    }

    private void MakeOutline(bool make)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        sr.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", make ? 1 : 0);
        mpb.SetColor("_OutlineColor", outlineColor);
        sr.SetPropertyBlock(mpb);
    }
}
