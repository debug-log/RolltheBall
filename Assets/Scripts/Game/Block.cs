using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public BlockType blockType;
    public BlockDirectionType blockDirectionType;

    public Vector2 currentPosition;
    public BlockMovement blockMovement;

    private bool initialized = false;

    public void Init(int x, int y)
    {
        blockType = BlockType.Null;
        blockDirectionType = BlockDirectionType.Null;

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

    public void SetBlockType (BlockType blockType)
    {
        this.blockType = blockType;
    }

    public void SetBlockDirectionType(BlockDirectionType blockDirectionType)
    {
        this.blockDirectionType = blockDirectionType;
    }

    public void SetBlockDimmed ()
    {
        var sr = this.GetComponent<SpriteRenderer> ();
        if (sr != null)
        {
            sr.color = Color.gray;
        }
    }
}