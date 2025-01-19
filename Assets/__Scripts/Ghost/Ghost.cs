using System;
using System.Collections;
using UnityEngine;
using static GameManager;

public class Ghost : MonoBehaviour
{
    [SerializeField] GhostState firstState;
    public GhostState State { get; private set; }
    public enum GhostState
    {
        InHome,
        ExitingHome,
        Scattering,
        Chasing,
        Fleeing,
        ReturnHome,
    }
    [SerializeField] Sprite[] eyes;
    [SerializeField] SpriteRenderer eyeRenderer; 
    [SerializeField] float inHomeDuration = 5f;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] Vector2 direction = Vector2.up;
    [SerializeField] Vector2 eyeDirection = Vector2.up;
    Bounds bounds;
    Rigidbody2D rb;
    
    public static Action<GhostState> OnBeforeGhostStateChange;
    public static Action<GhostState> OnAfterGhostStateChange;


    void Awake()
    {
        bounds = GetComponent<Collider2D>().bounds;
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        GhostStateChange(GhostState.InHome);
    }

    void Update()
    {
        switch (State)
        {
            case GhostState.InHome:
                GhostInHome();
                break;
            case GhostState.ExitingHome:
                break;
            case GhostState.Scattering:
                break;
            case GhostState.Chasing:
                break;
            case GhostState.Fleeing:
                break;
            case GhostState.ReturnHome:
                break;
        }
    }

    public void GhostStateChange(GhostState newState)
    {
        OnBeforeGhostStateChange?.Invoke(newState);
        State = newState;
        OnAfterGhostStateChange?.Invoke(newState);
    }

    void GhostInHome()
    {
        float layLength = 0.1f;
        Vector2 pos;

        inHomeDuration += Time.deltaTime;
        pos = transform.position;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
        
        Vector2 startPos = pos + (bounds.extents * direction);
        RaycastHit2D hit = Physics2D.Raycast(startPos, direction, layLength, wallLayer);
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        if (hit.collider != null)
        {
            direction *= -1;
        }
        DrawEyes(direction);
        //Debug.DrawRay(startPos, direction * layLength, Color.red);
        //Debug.Log(bounds.extents);
    }

    void DrawEyes(Vector2 newDirection)
    {
        int eyeIndex = 0;

        if (eyeDirection == newDirection)
        {
            return;
        }

        eyeDirection = newDirection;

        if (newDirection == Vector2.up)
        {
            eyeIndex = 0;
        }
        else if (newDirection == Vector2.down)
        {
            eyeIndex = 1;
        }
        else if (newDirection == Vector2.left)
        {
            eyeIndex = 2;
        }
        else if (newDirection == Vector2.right)
        {
            eyeIndex = 3;
        }

        eyeRenderer.sprite = eyes[eyeIndex];
    }
}
