using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameManager;

public class Ghost : MonoBehaviour
{
    [SerializeField] GhostState firstState;
    [SerializeField] Transform scatterPoint;
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
    [SerializeField] int currentSlot;
    Bounds ghostBounds;
    Rigidbody2D rb;
    float stayingHomeTime;
    bool moveBetweenSlot;

    public static Action<GhostState> OnBeforeGhostStateChange;
    public static Action<GhostState> OnAfterGhostStateChange;


    void Awake()
    {
        ghostBounds = GetComponent<Collider2D>().bounds;
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        GhostStateChange(firstState);
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
                GhostScattering();
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
        switch (newState)
        {
            case GhostState.InHome:
                for (int i = 0; i < 3; i++)
                {
                    if (transform.position.x == Home.Instance.SlotXPos[i])
                    {
                        currentSlot = i;
                    }
                }
            break;
            case GhostState.ExitingHome:
                //add left and right as initial available path
                OpenPath.Clear();
                OpenPath.Add(Vector2.left);
                OpenPath.Add(Vector2.right);
                break;
            case GhostState.Scattering:
                directionDecided = false;
                break;
        }
    }


    #region --- Scattering ---
    [SerializeField] List<Vector2> OpenPath = new List<Vector2>();
    [SerializeField] bool directionDecided = false;

    void GhostScattering()
    {
        direction = CheckDirectionToScatter();
        DrawEyes(direction);
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    Vector2 CheckDirectionToScatter()
    {
        if (directionDecided == true)
        {
            return direction;
        }

        Vector2 currentPos = transform.position;
        Vector2 bestDirection = Vector2.zero;
        float minDistance = float.MaxValue;

        foreach (var dir in OpenPath)
        {
            //prevent to go back
            if (dir == -direction)
            {
                continue;
            }
            Vector2 newPos = currentPos + dir * 2f;
            float distance = Vector2.Distance(newPos, scatterPoint.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                bestDirection = dir;
            }
        }
        directionDecided = true;
        return bestDirection;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Intersection"))
        {
            return;
        }

        directionDecided = false;
        var intersection = other.GetComponent<Intersection>();
        transform.position = intersection.transform.position;

        OpenPath.Clear();
        foreach (var path in intersection.OpenPath)
        {
            OpenPath.Add(path);
        }
    }

    #endregion
    #region --- Exiting Home ---
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
    #endregion
    #region --- In Home ---
    void GhostInHome()
    {
        stayingHomeTime += Time.deltaTime;

        transform.Translate(direction * moveSpeed * Time.deltaTime);

        //MoveBetweenSlotCompleted();
        DrawEyes(direction);
        CheckIfHitWall();
        MoveBetweenSlot();
        MoveBetweenSlotCompleted();
        //Debug.DrawRay(startPos, direction * layLength, Color.red);
        //Debug.Log(bounds.extents);
    }

    private void MoveBetweenSlotCompleted()
    {
        if (!moveBetweenSlot)
        {
            return;
        }

        if (currentSlot == 0 && transform.position.x >= 0 ||
            currentSlot == 2 && transform.position.x <= 0)
        {
            transform.position = new Vector3(0, transform.position.y, 0);
            Home.Instance.GhostInSlot[currentSlot] = false;
            currentSlot = 1;
            direction = Vector2.up;
        }
    }

    private void CheckIfHitWall()
    {
        float layLength = 0.7f;

        Vector2 startPos = transform.position + (Vector3)(ghostBounds.extents * direction);
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
            if (stayingHomeTime > timeHome)
            {
                direction = Vector2.up;
                stayingHomeTime = 0;
                GhostStateChange(GhostState.ExitingHome);
            }
        }
    }
    #endregion

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
