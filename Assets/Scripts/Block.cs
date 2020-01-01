using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public BlockType blockType;
    public DynamicBlockType dynamicBlockType;

    public Vector2 currentPosition;

    private BlockMovement blockMovement;

    public void Init(int x, int y)
    {
        blockType = BlockType.None;
        dynamicBlockType = DynamicBlockType.None;

        currentPosition = new Vector2 (x, y);
        blockMovement.Init ();
    }
}