using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public static float BLOCK_WIDTH = 1.0f;
    public static float BLOCK_HEIGHT = 1.0f;

    private Block[] blocks = new Block[16];
    
    public void Init ()
    {

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

            //위치 세팅
            {
                float posX = block.transform.localPosition.x;
                float posY = block.transform.localPosition.y;
                int x;
                int y;

                if (!IsValidPosition (posX, posY, out x, out y))
                {
                    continue;
                }

                SetBlock (block, x, y);
            }

            //블록 타입설정
            {
                var blockSpriteRenderer = block.GetComponent<SpriteRenderer> ();
                if (blockSpriteRenderer == null)
                    continue;

                var blockSpriteName = blockSpriteRenderer.sprite.name;
                var blockInfo = SpriteBlockInfo.GetSpriteBlockEnum (blockSpriteName);

                var blockType = SpriteBlockInfo.GetBlockType (blockInfo);
                var blockDirectionType = SpriteBlockInfo.GetBlockDirectionType (blockInfo);

                block.SetBlockType (blockType);
                block.SetBlockDirectionType (blockDirectionType);
            }
        }
    }

    private bool IsValidPosition(float x, float y, out int ox, out int oy)
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
            ox = -1;
            oy = -1;
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
            ox = -1;
            oy = -1;
            return false;
        }

        ox = i;
        oy = j;
        return true;
    }
#endif

    public void SetBlock (Block target, int x, int y)
    {
        int index = y * 4 + x;
        if (index > blocks.Length)
            return;
        blocks[index] = target;
        target.SetPosition (x, y);
    }

    public void UnsetBlock (int x, int y)
    {
        int index = y * 4 + x;
        if (index > blocks.Length)
            return;
        blocks[index] = null;
    }

    public Block GetBlock (int x, int y)
    {
        int index = y * 4 + x;
        if (index > blocks.Length)
            return null;
        return blocks[index];
    }

    public void Move(Block block, Direction direction)
    {
        if (block.blockType != BlockType.Dynamic)
            return;

        if (block.blockMovement.IsMoving () == true)
            return;

        int cx = (int)block.currentPosition.x;
        int cy = (int)block.currentPosition.y;

        int tx = -1;
        int ty = -1;

        switch(direction)
        {
            case Direction.Left:
                tx = cx - 1;
                ty = cy;
                break;
            case Direction.Right:
                tx = cx + 1;
                ty = cy;
                break;
            case Direction.Up:
                tx = cx;
                ty = cy + 1;
                break;
            case Direction.Down:
                tx = cx;
                ty = cy - 1;
                break;
        }

        if (tx < 0 || ty < 0 || tx > 3 || ty > 3)
            return;

        var targetPositionBlock = GetBlock (tx, ty);
        if (targetPositionBlock != null)
            return;
        
        block.blockMovement.Move (direction);

        UnsetBlock (cx, cy);
        SetBlock (block, tx, ty);
    }
}
