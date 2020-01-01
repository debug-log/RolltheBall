using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public BlockType blockType;
    public DynamicBlockType dynamicBlockType;

    public Vector2 currentPosition;
    public BlockMovement blockMovement;

    private bool initialized = false;

    public void Init(int x, int y)
    {
        blockType = BlockType.None;
        dynamicBlockType = DynamicBlockType.None;

        currentPosition = new Vector2 (x, y);

        if(blockMovement == null)
        {
            blockMovement = this.GetComponent<BlockMovement> ();
        }
        
        blockMovement.Init ();
    }

    public void SetPosition(int x, int y)
    {
        if (initialized)
        {
            currentPosition = new Vector2 (x, y);
        }
        else
        {
            Init (x, y);
            initialized = true;
        }
    }
}