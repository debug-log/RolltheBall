using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public static float BLOCK_WIDTH = 1.0f;
    public static float BLOCK_HEIGHT = 1.0f;

    private Block[] blocks = new Block[16];
    private Block startPoint = null;
    private Block endPoint = null;
    
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

                if (blockType == BlockType.StartPoint)
                {
                    this.startPoint = block;
                }

                if (blockType == BlockType.EndPoint)
                {
                    this.endPoint = block;
                }

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

    private bool CheckSolutionFound()
    {
        var curBlock = startPoint;
        Direction fromDirection = Direction.Null;
        int whileIterCount = 0;

        while (curBlock != null && whileIterCount < 20)
        {
            var x = (int)curBlock.currentPosition.x;
            var y = (int)curBlock.currentPosition.y;
            var direction = curBlock.blockDirectionType;
            
            Block nextBlock = null;

            if (curBlock.Equals (endPoint))
            {
                return true;
            }
            else if(curBlock.Equals(startPoint))
            {
                switch(direction)
                {
                    case BlockDirectionType.Left:
                        nextBlock = GetBlock (x - 1, y);
                        fromDirection = Direction.Right;
                        break;
                    case BlockDirectionType.Right:
                        nextBlock = GetBlock (x + 1, y);
                        fromDirection = Direction.Left;
                        break;
                    case BlockDirectionType.Up:
                        nextBlock = GetBlock (x, y + 1);
                        fromDirection = Direction.Down;
                        break;
                    case BlockDirectionType.Down:
                        nextBlock = GetBlock (x, y - 1);
                        fromDirection = Direction.Up;
                        break;
                    default:
                        nextBlock = null;
                        fromDirection = Direction.Null;
                        break;
                }
            }
            else
            {
                switch (direction)
                {
                    case BlockDirectionType.Vertical:
                        {
                            if (fromDirection == Direction.Up)
                            {
                                nextBlock = GetBlock (x, y - 1);
                                fromDirection = Direction.Up;
                            }
                            else if(fromDirection == Direction.Down)
                            {
                                nextBlock = GetBlock (x, y + 1);
                                fromDirection = Direction.Down;
                            }
                        }
                        break;
                    case BlockDirectionType.Horizontal:
                        {
                            if (fromDirection == Direction.Left)
                            {
                                nextBlock = GetBlock (x + 1, y);
                                fromDirection = Direction.Left;
                            }
                            else if (fromDirection == Direction.Right)
                            {
                                nextBlock = GetBlock (x - 1, y);
                                fromDirection = Direction.Right;
                            }
                        }
                        break;
                    case BlockDirectionType.UpAndLeft:
                        {
                            if (fromDirection == Direction.Up)
                            {
                                nextBlock = GetBlock (x - 1, y);
                                fromDirection = Direction.Right;
                            }
                            else if (fromDirection == Direction.Left)
                            {
                                nextBlock = GetBlock (x, y + 1);
                                fromDirection = Direction.Down;
                            }
                        }
                        break;
                    case BlockDirectionType.UpAndRight:
                        {
                            if (fromDirection == Direction.Up)
                            {
                                nextBlock = GetBlock (x + 1, y);
                                fromDirection = Direction.Left;
                            }
                            else if (fromDirection == Direction.Right)
                            {
                                nextBlock = GetBlock (x, y + 1);
                                fromDirection = Direction.Down;
                            }
                        }
                        break;
                    case BlockDirectionType.DownAndLeft:
                        {
                            if (fromDirection == Direction.Down)
                            {
                                nextBlock = GetBlock (x - 1, y);
                                fromDirection = Direction.Right;
                            }
                            else if (fromDirection == Direction.Left)
                            {
                                nextBlock = GetBlock (x, y - 1);
                                fromDirection = Direction.Up;
                            }
                        }
                        break;
                    case BlockDirectionType.DownAndRight:
                        {
                            if (fromDirection == Direction.Down)
                            {
                                nextBlock = GetBlock (x + 1, y);
                                fromDirection = Direction.Left;
                            }
                            else if (fromDirection == Direction.Right)
                            {
                                nextBlock = GetBlock (x, y - 1);
                                fromDirection = Direction.Up;
                            }
                        }
                        break;
                    default:
                        nextBlock = null;
                        fromDirection = Direction.Null;
                        break;
                }
            }

            curBlock = nextBlock;
            whileIterCount++;
        }
        
        return false;
    }

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
        if (index > blocks.Length || index < 0)
            return;
        blocks[index] = null;
    }

    public Block GetBlock (int x, int y)
    {
        int index = y * 4 + x;
        if (index > blocks.Length || index < 0)
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

    public void MoveEnd()
    {
        bool isSolution = CheckSolutionFound ();
        Debug.Log (isSolution);
    }
}
