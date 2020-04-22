using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraOrto : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap = null;

    private Camera cam = null;
    private Vector3[] tilePositions = new Vector3[2];

    private Vector3 min = Vector3.zero;
    private Vector3 max = Vector3.zero;

    void Start()
    {
        cam = Camera.main;

        tilePositions[0] = tilemap.CellToWorld(tilemap.cellBounds.min);
        tilePositions[1] = tilemap.CellToWorld(tilemap.cellBounds.max);
    }

    // Update is called once per frame
    void Update()
    {
        float height = 2 * cam.orthographicSize;
        float width = height * cam.aspect;

        min.x = tilePositions[0].x + width / 2;
        min.y = tilePositions[0].y + height / 2;

        max.x = tilePositions[1].x - width / 2;
        max.y = tilePositions[1].y - height / 2;

        var pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }

    private void OnDrawGizmos()
    {
        foreach (var p in tilePositions)
        {
            Gizmos.DrawWireSphere(p, 0.2f);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(min, 0.3f);
        Gizmos.DrawWireSphere(max, 0.3f);
    }
}
