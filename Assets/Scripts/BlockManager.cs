using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public static float BLOCK_WIDTH = 1.0f;
    public static float BLOCK_HEIGHT = 1.0f;

    private Block[] blocks = new Block[16];
    
    public void Init()
    {
        
    }

    public void SetBlock(Block target, int col, int row)
    {
        int index = row * 4 + col;
        if (index > blocks.Length)
            return;
        blocks[index] = target;
    }

    public Block GetBlock (int col, int row)
    {
        int index = row * 4 + col;
        if (index > blocks.Length)
            return null;
        return blocks[index];
    }

#if UNITY_EDITOR
    public void InitFromScene()
    {
        for(int i=0; i<transform.childCount; i++)
        {
            var child = this.transform.GetChild (i);
            if (child == null)
                continue;

            var block = child.GetComponent<Block> ();
            if (block == null)
                continue;

            float posX = block.transform.localPosition.x;
            float posY = block.transform.localPosition.y;
            int col;
            int row;

            if(!IsValidPosition(posX, posY, out col, out row))
            {
                continue;
            }

            SetBlock (block, col, row);
        }
    }

    private bool IsValidPosition(float x, float y, out int col, out int row)
    {
        int i = 0;
        int j = 0;
        bool valid = false;

        for(float dx = -1.5f * BLOCK_WIDTH; dx <= 1.5f * BLOCK_WIDTH; dx += BLOCK_WIDTH, i++)
        {
            if(dx == x)
            {
                valid = true;
                break;
            }
        }

        if(valid == false)
        {
            col = -1;
            row = -1;
            return false;
        }

        valid = false;

        for (float dy = -1.5f * BLOCK_HEIGHT; dy <= 1.5f * BLOCK_HEIGHT; dy += BLOCK_HEIGHT, j++)
        {
            if(dy == y)
            {
                valid = true;
                break;
            }
        }

        if (valid == false)
        {
            col = -1;
            row = -1;
            return false;
        }

        col = i;
        row = j;
        return true;
    }
#endif
}
