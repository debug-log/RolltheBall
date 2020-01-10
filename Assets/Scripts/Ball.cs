using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ball : MonoBehaviour
{
    Vector2 targetPosition = Vector2.zero;
    Vector2 pivot = Vector2.zero;

    Vector2 velocity = Vector2.zero;
    const float SPEED = 1f;

    float speed = SPEED;
    float radius = BlockManager.BLOCK_WIDTH * 0.5f;
    float angle = 0f;
    float angleSpeed = SPEED * 2f;

    bool moveStart = false;
    bool moveEnd = false;

    List<PathPoint> solutionPath = null;
    PathPoint curPathPoint = null;
    int curPathPointIndex = 0;

    void Update()
    {
        if (moveStart == false)
            return;

        if (moveEnd == true)
            return;

        if (solutionPath == null || curPathPoint == null)
            return;

        Move (curPathPoint);
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        var star = collision.gameObject.GetComponent<Star> ();
        if(star != null)
        {
            star.GetStar ();

            var stageManager = StageManager.Instance;
            UIManager.Instance.GetStar (stageManager.GetNumStars());
            stageManager.IncreaseNumStars ();

            star.enabled = false;
        }
    }

    private void Move(PathPoint pathPoint)
    {
        Vector2 curPosition = transform.localPosition;

        if(Vector2.Distance(targetPosition, curPosition) < 0.05f)
        {
            transform.localPosition = targetPosition;
            if(MoveToNextPoint () != true)
            {
                moveEnd = true;
                StageManager.Instance.InvokeStageEndEvent ();
                return;
            }
            return;
        }

        if(curPathPoint.hasPivot)
        {
            angle += angleSpeed * Time.deltaTime;

            float x = curPathPoint.pivotPosition.x + Mathf.Cos (angle) * radius;
            float y = curPathPoint.pivotPosition.y + Mathf.Sin (angle) * radius;

            curPosition = new Vector2 (x, y);
        }
        else
        {
            curPosition += velocity * Time.deltaTime;
        }

        this.transform.localPosition = curPosition;
    }

    private bool MoveToNextPoint()
    {
        if(curPathPointIndex < solutionPath.Count - 1)
        {
            curPathPointIndex++;
            curPathPoint = solutionPath[curPathPointIndex];

            var nextPathPoint = solutionPath.ElementAtOrDefault (curPathPointIndex + 1);
            targetPosition = GetTargetPosition (curPathPoint, nextPathPoint);

            Vector2 curPosition = this.transform.localPosition;
            velocity = (targetPosition - curPosition) * speed;

            if(nextPathPoint == null)
            {
                velocity *= 2f;
            }

            if(curPathPoint.hasPivot)
            {
                SetRotateValue (curPathPoint);
            }

            return true;
        }
        return false;
    }

    private void SetRotateValue (PathPoint curPathPoint)
    {
        if (curPathPoint == null)
            return;

        Vector3 lhs = (Vector2) this.transform.localPosition - curPathPoint.pivotPosition;
        Vector3 rhs = targetPosition - curPathPoint.pivotPosition;

        float z = Vector3.Cross (lhs, rhs).z;
        bool rotateClockwise = z < 0;

        if(rotateClockwise)
        {
            angleSpeed = -1 * Mathf.Abs (angleSpeed);
        }
        else
        {
            angleSpeed = Mathf.Abs (angleSpeed);
        }

        if(lhs.x > 0f)
        {
            angle = 0f;
        }
        else if(lhs.y > 0f)
        {
            angle = 90f * Mathf.Deg2Rad;
        }
        else if(lhs.x < 0f)
        {
            angle = 180f * Mathf.Deg2Rad;
        }
        else
        {
            angle = 270f * Mathf.Deg2Rad;
        }
    }

    private Vector2 GetTargetPosition(PathPoint curPathPoint, PathPoint nextPathPoint)
    {
        if(curPathPoint == null && nextPathPoint == null)
        {
            Debug.LogError ("[NullReferenceException] GetTargetPosition");
            return Vector2.zero;
        }

        if (nextPathPoint == null)
        {
            return curPathPoint.position;
        }

        if (curPathPoint == null)
        {
            return nextPathPoint.position;
        }

        return (curPathPoint.position + nextPathPoint.position) * 0.5f;
    }

    public void StartAlongPath(List<PathPoint> path)
    {
        solutionPath = path;
        moveStart = true;
        moveEnd = false;

        curPathPoint = solutionPath.FirstOrDefault ();
        curPathPointIndex = 0;

        targetPosition = GetTargetPosition (curPathPoint, solutionPath.ElementAtOrDefault (curPathPointIndex + 1));

        Vector2 curPosition = this.transform.localPosition;
        velocity = (targetPosition - curPosition) * speed * 2f;
    }
}
