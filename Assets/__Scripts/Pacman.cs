using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Pacman : MonoBehaviour
{
    [SerializeField] LayerMask wallLayer;
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
                internalPos = (Vector2)currentPos + (moveInput.normalized * 1.5f);
                targetPos = internalPos;
                targetPos.y = Mathf.Clamp(targetPos.y, -14.5f, 13.5f);
                targetPos.x = Mathf.Clamp(targetPos.x, -12.5f, 12.5f);
                if (targetPos.x == -11)
                {
                    targetPos.x = -12;
                }
                if (targetPos.x == 11)
                {
                    targetPos.x = 12;
                }
                directionDecided = true;
            }
        }
    }

    Vector2 MovementRestrict(Vector2 inputDirection)
    {
        foreach (var direction in OpenPath)
        {
            if (direction == inputDirection.normalized)
            {
                return inputDirection;
            }
        }
        return Vector2.zero;
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
