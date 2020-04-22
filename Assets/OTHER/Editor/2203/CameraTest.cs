using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour
{
    [SerializeField] private Camera cam = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cameraPosition = transform.position;
        var cameraSize = cam.orthographicSize;
        var cameraAspect = cam.aspect;

        var pos = cameraPosition - new Vector2(cameraSize * cameraAspect, cameraSize);
        var size = new Vector2(cameraSize * 2f * cameraAspect, cameraSize * 2f);
        var cameraRect = new Rect(pos, size);
    }
}
