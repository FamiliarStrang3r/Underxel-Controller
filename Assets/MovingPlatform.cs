using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum PlatformType
    {
        Endless,
        OnInteract
    }

    [Header("Movement")]
    public PlatformType Type;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float timeDelay = 2f;

    private float timer;
    private Vector3 nextPos;
    private int wayDir;
    private int curPointNum;
    private bool onPlatform;
    private Transform childHolder;

    [Header("Way Points")]
    [SerializeField] private List<Vector3> points = new List<Vector3>();
    public List<Vector3> Points => points;

    private void Start()
    {
        transform.position = this.points[0];
        childHolder = transform.parent;
    }

    private void Update()
    {
        SetNextPos();
        if (Type == PlatformType.Endless)
            EndlessMove();
        else
            OnInteractMove();
    }

    private void FixedUpdate()
    {
        PlayerOnPlatform();
    }

    private void SetNextPos()
    {
        // Присваивает nextPos позицию следующего пункта.
        if (transform.position == this.points[0] && nextPos != this.points[1]){
            nextPos = this.points[1];
            curPointNum = 1;
            wayDir = 1;
            timer = 0;
        }
        else if (transform.position == this.points[this.points.Count - 1] && nextPos != this.points[this.points.Count - 2]){
            nextPos = this.points[curPointNum - 1];
            curPointNum = this.points.Count - 2;
            wayDir = -1;
            timer = 0;

        }
        else if(transform.position == this.points[curPointNum] && (nextPos != this.points[curPointNum + 1] || nextPos != this.points[curPointNum - 1])){
            curPointNum += wayDir;
            nextPos = this.points[curPointNum];
            if (Type == PlatformType.Endless)
            {
                timer = 0;
            }
        }
    }

    private void EndlessMove()
    {
        // Движение для платформы, двигающейся бесконечно.
        if (timer >= timeDelay){
            transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
        }
        else{
            timer += Time.deltaTime;
        }
        
    }

    private void OnInteractMove()
    {
        // Движение для платформы, реагирующей на воздействие игрока.
        if (onPlatform && timer >= timeDelay)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
        }
        else if (onPlatform && timer < timeDelay)
        {
            timer += Time.deltaTime;
        }
    }
    
    private void PlayerOnPlatform()
    {
        // Находится ли игрок на платформе. Пэрентит игрока к платформе.
        if (Physics.BoxCast(transform.position, transform.lossyScale * 0.4f, transform.up, out var hit,
            transform.rotation, transform.lossyScale.y * 1.3f))
        {
            if (hit.collider.CompareTag("Player"))
            {
                onPlatform = true;
                hit.collider.transform.parent.SetParent(transform);
            }
            else if (transform.childCount > 0)
            {
                transform.DetachChildren();
                onPlatform = false;
            }
        }
        else if (transform.childCount > 0)
        {
            transform.DetachChildren();
            onPlatform = false;
        }
            
    }
}
