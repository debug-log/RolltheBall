using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    private enum MoveState
    {
        None,
        MoveBegin,
        Moving,
        MoveEnd,
    }

    public float moveThreshold = 0.05f;
    public float moveSpeed = 4f;

    private BlockManager blockManager;
    private Block block;

    private Vector2 screenSize;
    private Vector2 moveVelocity;
    private Vector2 mousePositionDown;
    private Vector2 targetPosition;
    private MoveState moveState;

    private bool onMouseDown = false;

    public void Init ()
    {
        moveThreshold = 0.05f;
        moveState = MoveState.None;

        blockManager = StageManager.Instance.blockManager;
        block = this.GetComponent<Block> ();
    }

    private void Update ()
    {
        UpdateMovement (Time.deltaTime);
    }

    private void UpdateMovement(float deltaTime)
    {
        if(moveState == MoveState.None)
        {
            return;
        }

        if(moveState == MoveState.MoveEnd)
        {
            blockManager.MoveEnd ();

            moveState = MoveState.None;
            targetPosition = Vector2.zero;
            return;
        }

        if(moveState == MoveState.MoveBegin)
        {
            moveState = MoveState.Moving;
            return;
        }

        if(moveState == MoveState.Moving)
        {
            Vector2 position2d = transform.position;
            position2d += moveVelocity * deltaTime;

            if(Vector2.Distance(position2d, targetPosition) < moveSpeed * deltaTime)
            {
                moveState = MoveState.MoveEnd;
                position2d = targetPosition;
            }

            transform.position = position2d;
        }
    }

    public void Move(Direction direction)
    {
        if(moveState != MoveState.None)
        {
            return;
        }

        moveState = MoveState.MoveBegin;

        switch (direction)
        {
            case Direction.Left:
                moveVelocity = new Vector2 (-moveSpeed, 0f);
                targetPosition = new Vector2 (transform.position.x - 1f, transform.position.y);
                break;
            case Direction.Right:
                moveVelocity = new Vector2 (moveSpeed, 0f);
                targetPosition = new Vector2 (transform.position.x + 1f, transform.position.y);
                break;
            case Direction.Up:
                moveVelocity = new Vector2 (0f, moveSpeed);
                targetPosition = new Vector2 (transform.position.x, transform.position.y + 1f);
                break;
            case Direction.Down:
                moveVelocity = new Vector2 (0f, -moveSpeed);
                targetPosition = new Vector2 (transform.position.x, transform.position.y - 1f);
                break;
        }
    }

    private void SetScreenSize()
    {
        screenSize = new Vector2 (Screen.width, Screen.height);
    }

    private Vector2 GetMousePosition()
    {
        if (screenSize == Vector2.zero)
            SetScreenSize ();

        return new Vector2 (Input.mousePosition.x / screenSize.x, Input.mousePosition.y / screenSize.y);
    }

    public bool IsMoving()
    {
        if (this.moveState == MoveState.None)
            return false;
        return true;
    }

    private void OnMouseDown ()
    {
        SetScreenSize ();
        mousePositionDown = GetMousePosition ();

        onMouseDown = true;
    }

    private void OnMouseDrag ()
    {
        if (onMouseDown != true)
            return;

        var mousePosition = GetMousePosition ();
        var diff = mousePosition - mousePositionDown;
        var diffAbs = new Vector2 (Mathf.Abs (diff.x), Mathf.Abs (diff.y));
        bool moveHorizontal = false;

        if(diffAbs.x > diffAbs.y)
        {
            moveHorizontal = true;
        }
        else
        {
            moveHorizontal = false;
        }

        if(moveHorizontal)
        {
            if(diff.x > moveThreshold)
            {
                blockManager.Move (block, Direction.Right);
                onMouseDown = false;
            }
            else if(diff.x < -moveThreshold)
            {
                blockManager.Move (block, Direction.Left);
                onMouseDown = false;
            }
        }
        else
        {
            if(diff.y > moveThreshold)
            {
                blockManager.Move (block, Direction.Up);
                onMouseDown = false;
            }
            else if(diff.y < -moveThreshold)
            {
                blockManager.Move (block, Direction.Down);
                onMouseDown = false;
            }
        }
    }

    private void OnMouseUp ()
    {
        onMouseDown = false;
    }
}
