using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField, Range(0, 1)] private float percentTreshold = 0.2f;

    [SerializeField] private int currentIndex = 0;
    [SerializeField] private bool moveLeftAllowed = false;
    [SerializeField] private bool moveRightAllowed = false;
    [SerializeField] private Button leftButton = null;
    [SerializeField] private Button rightButton = null;

    private Vector3 position = Vector3.zero;
    private float dragPercent = 0;
    private float elementWidth = 0;
    private int elementsCount = 0;

    private Canvas canvas = null;
    private RectTransform rt = null;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        position = rt.anchoredPosition;
        elementWidth = GetComponent<GridLayoutGroup>().cellSize.x;
        elementsCount = transform.childCount;

        canvas = FindObjectOfType<Canvas>();

        CheckMove();

        leftButton.onClick.AddListener(() => MoveTo(false));
        rightButton.onClick.AddListener(() => MoveTo(true));

        //Debug.Log(Screen.width);

        //var points = new Vector3[]
        //{
        //    new Vector3(0, 1), new Vector3(1, 1),
        //    new Vector3(0, 0), new Vector3(1, 0)
        //};
        //var center = Vector3.zero;
        //points.GetCenter(ref center);
        //Debug.Log($"center is: {center.ToString()}");

        //new Vector3(0, 0),
        //new Vector3(0, 1),
        //new Vector3(1, 0),
        //new Vector3(1, 1)
    }

    public void OnDrag(PointerEventData eventData)
    {
        float difference = eventData.pressPosition.x - eventData.position.x;
        dragPercent = difference / Screen.width;

        //first way to move, I think bad
        //rt.anchoredPosition = panelLocation - Vector3.right * difference;

        //second way to move
        //eventData.delta contains Y value, we dont need it
        Vector2 delta = Vector2.right * eventData.delta.x;
        //rt.anchoredPosition += eventData.delta / canvas.scaleFactor;
        rt.anchoredPosition += delta / canvas.scaleFactor;
        //Debug.Log(canvas.scaleFactor);
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        bool canMove = (dragPercent > 0 && moveRightAllowed) || (dragPercent < 0 && moveLeftAllowed);

        if (Mathf.Abs(dragPercent) >= percentTreshold && canMove)
        {
            Vector3 pos = position + Vector3.left * Mathf.Sign(dragPercent) * elementWidth;
            rt.anchoredPosition = position = pos;
            currentIndex = Mathf.Abs(Mathf.FloorToInt(rt.anchoredPosition.x / elementWidth));
        }
        else
        {
            rt.anchoredPosition = position;
        }

        dragPercent = 0;

        CheckMove();
    }

    private void MoveTo(bool right)
    {
        int index = right ? 1 : -1;

        bool canMove = (index > 0 && moveRightAllowed) || (index < 0 && moveLeftAllowed);

        if (canMove)
        {
            Vector3 pos = position + Vector3.left * Mathf.Sign(index) * elementWidth;
            rt.anchoredPosition = position = pos;
            currentIndex = Mathf.Abs(Mathf.FloorToInt(rt.anchoredPosition.x / elementWidth));
        }

        CheckMove();
    }

    private void CheckMove()
    {
        moveLeftAllowed = currentIndex > 0;
        moveRightAllowed = currentIndex < elementsCount - 1;
    }

    private void OnGUI()
    {
        Rect r = new Rect(10, 10, 50, 20);
        GUI.TextField(r, dragPercent.ToString("F3"));

    }
}

static class S//GameObjectExtensions
{
    public static void GetCenter(this Vector3[] points, ref Vector3 center)
    {
        var sum = Vector3.zero;
        foreach (var p in points)
        {
            sum += p;
        }
        center = sum / points.Length;
    }
}
