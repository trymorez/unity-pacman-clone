using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Pacman : MonoBehaviour
{
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

    void MovementProcess()
    {
        //Vector2 movement = MovementRestrict(moveInput);
        //if (movement == Vector2.left || movement == Vector2.right)
        //{
        //    var positionY = Mathf.Round(transform.position.y * 2) / 2;
        //    transform.position = new Vector3(transform.position.x, positionY, 0);

        //}
        //if (movement == Vector2.up || movement == Vector2.down)
        //{
        //    var positionX = Mathf.Round(transform.position.x * 2) / 2;
        //    transform.position = new Vector3(positionX, transform.position.y, 0);

        //}
        
        //transform.Translate(movement * moveSpeed * Time.deltaTime);
        transform.Translate(moveInput * moveSpeed * Time.deltaTime);
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Intersection"))
        {
            return;
        }

        var intersection = other.GetComponent<Intersection>();
        transform.position = intersection.transform.position;

        OpenPath.Clear();
        foreach (var path in intersection.OpenPath)
        {
            OpenPath.Add(path);
        }
    }
}
