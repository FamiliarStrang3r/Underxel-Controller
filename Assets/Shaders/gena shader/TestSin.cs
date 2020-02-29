using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSin : MonoBehaviour
{
    public bool isSin = false;

    public float speed = 0;
    public float percent01 = 0;

    private float offset = 0;

    private void Start()
    {
        offset = transform.position.y;
    }

    private void Update()
    {
        float value = isSin ? Mathf.Sin(Time.time * speed) : Mathf.Cos(Time.time * speed);

        percent01 = (value + 1) / 2;

        var pos = transform.position;
        pos.y = percent01 + offset;
        transform.position = pos;
    }
}
