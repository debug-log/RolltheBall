using UnityEngine;

public class PathPoint
{
    public Vector2 position;
    public Vector2 pivotPosition;
    public bool hasPivot = false;

    public PathPoint(Vector2 position, Vector2 pivot, bool hasPivot)
    {
        this.position = position;
        this.pivotPosition = pivot;
        this.hasPivot = hasPivot;
    }

    public override string ToString ()
    {
        if (hasPivot)
        {
            return string.Format ("[PathPoint] position : {0}, pivot : {1}", this.position, this.pivotPosition);
        }
        else
        {
            return string.Format ("[PathPoint] position : {0}", this.position);
        }        
    }
}
