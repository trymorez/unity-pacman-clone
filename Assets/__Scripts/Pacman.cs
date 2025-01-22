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

    void Awake()
    {
        GameManager.OnGamePlaying += OnGamePlaying;
    }

    void OnDestroy()
    {
        GameManager.OnGamePlaying -= OnGamePlaying;
    }

    void Start()
    {
        OpenPath.Clear();
        OpenPath.Add(Vector2.left);
        OpenPath.Add(Vector2.right);
    }

    void OnGamePlaying()
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
            if (CheckIfDirectionOpen(moveInput))
            {
                IntersectionGet();
            }
        }
    }

    void IntersectionGet()
    {
        var moveVector = moveInput.normalized;
        var origin = (Vector2)transform.position + moveVector * 0.8f;
        var layLenth = 12f;

        var ray = Physics2D.Raycast(origin, moveVector, layLenth, intersectionLayer);
        Debug.DrawLine(origin, origin + moveVector * layLenth, Color.red);
        if (ray.collider)
        {
            targetPos = ray.collider.transform.position;
            PacmanRotate(moveVector);
            Debug.Log(targetPos);
            directionDecided = true;
        }
    }

    public void OnMove(InputAction.CallbackContext callback)
    {
        moveInput = callback.ReadValue<Vector2>();
    }

    bool CheckIfDirectionOpen(Vector2 moveInput)
    {
        var vectorToCheck = moveInput.normalized;
        var origin = (Vector2)transform.position + vectorToCheck * 0.8f;
        var layLenth = 0.1f;

        var ray = Physics2D.Raycast(origin, vectorToCheck, layLenth, wallLayer);
        Debug.DrawLine(origin, origin + vectorToCheck * layLenth, Color.red);
        return (ray.collider == null);
    }

    void PacmanRotate(Vector2 dir)
    {
        if (dir == Vector2.up)
        {
            transform.eulerAngles = new Vector3(0, 0, 90);
        }
        if (dir == Vector2.down)
        {
            transform.eulerAngles = new Vector3(0, 0, -90);
        }
        if (dir == Vector2.left)
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
        }
        if (dir == Vector2.right)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {

    }
}
