using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Pacman : MonoBehaviour
{
    [SerializeField] LayerMask wallLayer;
    [SerializeField] LayerMask intersectionLayer;
    [SerializeField] float moveSpeed = 0.5f;
    [SerializeField] List<Vector2> OpenPath = new List<Vector2>();
    Vector2 moveInput;

    void Start()
    {
        OpenPath.Clear();
        OpenPath.Add(Vector2.left);
        OpenPath.Add(Vector2.right);
    }

    void Update()
    {
        MovementProcess();
    }

    bool directionDecided = false;
    Vector2 targetPos;
    Vector2 internalPos;

    void MovementProcess()
    {
        var currentPos = transform.position;

        if (directionDecided)
        {
            //arrived at destination
            if (Vector2.Distance(currentPos, targetPos) < 0.1f)
            {
                directionDecided = false;
                transform.position = targetPos;
            }
            else
            {
                transform.position = Vector2.MoveTowards(currentPos, targetPos, moveSpeed * Time.deltaTime);
            }
        }
        else if (moveInput != Vector2.zero)
        {
            if (DirectionOpen(moveInput))
            {
                MovementRestrict(currentPos);
            }
        }
    }

    void MovementRestrict(Vector2 currentPos)
    {
        var moveVector = moveInput.normalized;
        targetPos = (Vector2)currentPos + (moveVector * 1.5f);

        if (currentPos.x == -10.5 && moveVector == Vector2.left)
            targetPos.x = -12.5f;
        else if (currentPos.x == -12.5 && moveVector == Vector2.right)
            targetPos.x = -10.5f;
        else if (currentPos.x == 10.5 && moveVector == Vector2.right)
            targetPos.x = 12.5f;
        else if (currentPos.x == 12.5 && moveVector == Vector2.left)
            targetPos.x = 10.5f;
        if (currentPos.y == 11 && moveVector == Vector2.up)
            targetPos.y = 13.5f;
        if (currentPos.y == 13.5 && moveVector == Vector2.down)
            targetPos.y = 11;
        directionDecided = true;
    }

    public void OnMove(InputAction.CallbackContext callback)
    {
        moveInput = callback.ReadValue<Vector2>();
    }

    bool DirectionOpen(Vector2 moveInput)
    {
        var vectorToCheck = moveInput.normalized;
        var origin = (Vector2)transform.position + vectorToCheck * 0.8f;
        var layLenth = 0.1f;

        var ray = Physics2D.Raycast(origin, vectorToCheck, layLenth, wallLayer);
        Debug.DrawLine(origin, origin + vectorToCheck * layLenth, Color.red);
        Debug.Log(ray.collider == null);
        return (ray.collider == null);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if (!other.CompareTag("Intersection"))
        //{
        //    return;
        //}

        //var intersection = other.GetComponent<Intersection>();
        //transform.position = intersection.transform.position;

        //OpenPath.Clear();
        //foreach (var path in intersection.OpenPath)
        //{
        //    OpenPath.Add(path);
        //}
    }
}
