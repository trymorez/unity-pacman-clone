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
    [SerializeField] float timeHome = 5f;
    [SerializeField] float timeExiting = 2f;
    [SerializeField] float timeScattering = 5f;

    [SerializeField] Sprite[] eyes;
    [SerializeField] SpriteRenderer eyeRenderer; 
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] Vector2 direction = Vector2.up;
    [SerializeField] Vector2 eyeDirection = Vector2.up;
    Bounds bounds;
    Rigidbody2D rb;
    float stayingDuration;
    [SerializeField] int currentSlot;

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
                GhostExitingHome();
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
        BeforeGhostStateChange(newState);
        State = newState;
        OnAfterGhostStateChange?.Invoke(newState);
    }

    void BeforeGhostStateChange(GhostState newState)
    {
        if (newState == GhostState.InHome)
        {
            for (int i = 0; i < 3; i++)
            {
                if (transform.position.x == Home.Instance.SlotXPos[i])
                {
                    currentSlot = i;
                }
            }
        }
    }

    void GhostExitingHome()
    {
        float exitPos = Home.Instance.SlotExitYPos;

        transform.Translate(direction * moveSpeed * Time.deltaTime);

        if (transform.position.y >= exitPos)
        {
            transform.position = new Vector3(transform.position.x, exitPos, 0);
            Home.Instance.GhostInSlot[1] = false;
            GhostStateChange(GhostState.Scattering);
        }
    }

    bool moveBetweenSlot;

    void GhostInHome()
    {
        stayingDuration += Time.deltaTime;

        transform.Translate(direction * moveSpeed * Time.deltaTime);

        if (moveBetweenSlot)
        {
            if (currentSlot == 0 && transform.position.x >= 0 ||
                currentSlot == 2 && transform.position.x <= 0)
            {
                transform.position = new Vector3(0, transform.position.y, 0);
                Home.Instance.GhostInSlot[currentSlot] = false;
                currentSlot = 1;
                direction = Vector2.up;
            }
        }

        DrawEyes(direction);
        CheckIfHitWall();
        MoveBetweenSlot();
        //Debug.DrawRay(startPos, direction * layLength, Color.red);
        //Debug.Log(bounds.extents);
    }

    private void CheckIfHitWall()
    {
        float layLength = 0.1f;

        Vector2 startPos = transform.position + (Vector3)(bounds.extents * direction);
        RaycastHit2D hit = Physics2D.Raycast(startPos, direction, layLength, wallLayer);

        if (hit.collider != null)
        {
            direction *= -1;
            //ghost is at the door
            if (transform.position.y > 0)
            {
                CheckIfReadyToExit();
            }
        }
    }

    void MoveBetweenSlot()
    {
        //there's a ghost in center slot
        if (Home.Instance.GhostInSlot[1])
        {
            return;
        }

        if (currentSlot == 0)
        {
            moveBetweenSlot = true;
            direction = Vector2.right;
        }
        else if (currentSlot == 2 && Home.Instance.GhostInSlot[0] == false)
        {
            moveBetweenSlot = true;
            direction = Vector2.left;
        }
    }

    void CheckIfReadyToExit()
    {
        //if the ghost is in the center slot
        if (transform.position.x == Home.Instance.SlotXPos[1])
        {
            if (stayingDuration > timeHome)
            {
                direction = Vector2.up;
                stayingDuration = 0;
                GhostStateChange(GhostState.ExitingHome);
            }
        }
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

    void TranslateX(float moveOffsetX)
    {
        transform.Translate(moveOffsetX, transform.position.y, 0);
    }

    void TranslateY(float moveOffsetY)
    {
        transform.Translate(transform.position.x, moveOffsetY, 0);
    }
}
