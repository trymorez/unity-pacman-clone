using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pacman : MonoBehaviour
{
    [SerializeField] LayerMask wallLayer;
    [SerializeField] LayerMask intersectionLayer;
    [SerializeField] float moveSpeed = 0.5f;
    [SerializeField] List<Vector2> OpenPath = new List<Vector2>();
    Animator animator;
    Vector2 moveInput;
    Dictionary<Vector2, Vector3> rotationValue;
    public static Action<int> OnLifeUpdate;
    GameManager GM;
    bool isPowerUp { get { return GameManager.State == GameManager.GameState.PacmanPowerUp; } }

    void Awake()
    {
        GameManager.OnGamePlaying += OnGamePlaying;
        InitRotationValue();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        GM = GameManager.Instance;
        OpenPath.Clear();
        OpenPath.Add(Vector2.left);
        OpenPath.Add(Vector2.right);
    }

    private void InitRotationValue()
    {
        rotationValue = new Dictionary<Vector2, Vector3>
        {
            { Vector2.up, new Vector3(0, 0, 90) },
            { Vector2.down, new Vector3(0, 0, -90) },
            { Vector2.left, new Vector3(0, 0, 180) },
            { Vector2.right, new Vector3(0, 0, 0) }
        };
    }

    void OnDestroy()
    {
        GameManager.OnGamePlaying -= OnGamePlaying;
    }

    void OnGamePlaying()
    {
        ProcessMovement();
    }

    bool directionPicked = false;
    Vector2 destination;
    Vector2 direction;

    void ProcessMovement()
    {
        var currentPos = transform.position;

        if (directionPicked)
        {
            if (Vector2.Distance(currentPos, destination) < 0.1f)
            {
                directionPicked = false;
                transform.position = destination;
            }
            else
            {
                transform.position = Vector2.MoveTowards(currentPos, destination, moveSpeed * Time.deltaTime);
            }
        }
        else if (moveInput != Vector2.zero)
        {
            if (CheckIfDirectionOpen(moveInput))
            {
                GetIntersection();
            }
        }
    }

    void GetIntersection()
    {
        var moveVector = moveInput.normalized;
        var origin = (Vector2)transform.position + moveVector * 0.8f;
        var layLenth = 12f;

        var ray = Physics2D.Raycast(origin, moveVector, layLenth, intersectionLayer);
        Debug.DrawLine(origin, origin + moveVector * layLenth, Color.red);
        if (ray.collider)
        {
            destination = ray.collider.transform.position;
            PacmanRotate(moveVector);
            directionPicked = true;
        }
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

    public void OnMove(InputAction.CallbackContext callback)
    {
        moveInput = callback.ReadValue<Vector2>();
    }

    void PacmanRotate(Vector2 dir)
    {
        if (rotationValue.ContainsKey(dir))
        {
            transform.eulerAngles = rotationValue[dir];
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Portal"))
        {
            HandlePortal(other);
        }
        if (other.CompareTag("Ghost") && !isPowerUp)
        {
            HandleDying();
        }
    }

    void HandlePortal(Collider2D other)
    {
        transform.position = other.GetComponent<Portal>().otherPortal.position;
        directionPicked = false;
    }

    void HandleDying()
    {
        GM.GameStateChange(GameManager.GameState.PacmanDying);
        animator.SetBool("Dying", true);
        SoundManager.Play("Dying");
        
        StartCoroutine(WaitUntilDead());
    }

    IEnumerator WaitUntilDead()
    {
        yield return new WaitForSecondsRealtime(2.8f);

        animator.SetBool("Dying", false);
        GM.GameStateChange(GameManager.GameState.Playing);
        GM.DecreaseLife();
        ResetPacman();
    }

    void ResetPacman()
    {
        transform.position = new Vector3(0, -8.5f, 0);
        directionPicked = false;
        moveInput = Vector2.zero;
    }
}
