using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionUnits : MonoBehaviour
{
    [SerializeField] private bool isPixel = false;
    [SerializeField] private RectTransform box = null;
    private Vector3 startPosition = Vector3.zero;
    private Vector3 endPosition = Vector3.zero;

    private bool isDragging = false;

    private SelectableObject[] selectableObjects = null;
    private Camera cam = null;
    private Canvas canvas = null;

    private void Start()
    {
        selectableObjects = FindObjectsOfType<SelectableObject>();
        cam = Camera.main;
        canvas = FindObjectOfType<Canvas>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isDragging = true;
            startPosition = Input.mousePosition;
            box.gameObject.SetActive(true);

            foreach (var obj in selectableObjects)
            {
                obj.Select(false);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isDragging = false;
            box.gameObject.SetActive(false);

            Vector2 min = box.anchoredPosition - box.sizeDelta / 2;
            Vector2 max = box.anchoredPosition + box.sizeDelta / 2;

            foreach(var obj in selectableObjects)
            {
                Vector3 pos = cam.WorldToScreenPoint(obj.transform.position);

                if (!isPixel) pos /= canvas.scaleFactor;

                if (pos.x > min.x && pos.x < max.x && pos.y > min.y && pos.y < max.y)
                {
                    obj.Select(true);
                }
            }
        }

        if (isDragging)
        {
            endPosition = Input.mousePosition;
            Vector3 diff = endPosition - startPosition;

            Vector3 pos = startPosition + new Vector3(diff.x / 2, diff.y / 2);
            if (!isPixel) pos /= canvas.scaleFactor;
            box.anchoredPosition = pos;

            diff.x = Mathf.Abs(diff.x);
            diff.y = Mathf.Abs(diff.y);

            Vector2 size = diff;
            if (!isPixel) size /= canvas.scaleFactor;
            box.sizeDelta = size;
        }
    }

    private void OnDrawGizmos()
    {
        if (!cam) return;

        var z = 9;

        var p1 = startPosition;
        p1.z = z;
        p1 = cam.ScreenToWorldPoint(p1);

        var p2 = endPosition;
        p2.z = z;
        p2 = cam.ScreenToWorldPoint(p2);

        var c1 = Color.green;
        var c2 = Color.blue;
        c1.a = c2.a = 0.5f;

        var size = 0.2f;
        Gizmos.color = c1;
        Gizmos.DrawSphere(p1, size);
        Gizmos.color = c2;
        Gizmos.DrawSphere(p2, size);
    }
}
