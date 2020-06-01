using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    [SerializeField] private int moneyAmount = 10;
    [SerializeField, Range(0, 0.1f)] private float moveSpeed = 0;
    [SerializeField] private Vector3 offset = Vector3.up;

    private bool isPicked = false;
    private Transform target = null;

    private void Update()
    {
        if (isPicked && target)
        {
            var lerp = Vector3.Lerp(transform.position, target.position + offset, moveSpeed * 50 * Time.deltaTime);
            var moveTowards = Vector3.MoveTowards(transform.position, target.position + offset, moveSpeed * Time.deltaTime);
            transform.position = lerp;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var robot = other.GetComponent<RobotController>();

        if (robot)
        {
            var shop = FindObjectOfType<ShopController>();
            if (shop) shop.MoneyAmount += moneyAmount;

            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    public void StartMoving(Transform _target)
    {
        isPicked = true;
        target = _target;
    }
}
