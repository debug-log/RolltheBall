﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public static int BLOCK_ROW_AND_COL_COUNT = 4;

    public static float BLOCK_WIDTH = 1.61f;
    public static float BLOCK_HEIGHT = 1.61f;

    private const float SOLUTION_READY_SECONDS = 0.7f;

    private Block[] blocks = new Block[16];

    private Block startPoint = null;
    private Block endPoint = null;

    private List<Block> pathSolution = new List<Block> ();
    private List<Block> remainBlocks = new List<Block> ();
    private bool isSolution = false;
    private bool completeSolution = false;
    private float readyGameEndSeconds = 0f;

    public void Init (string stageDataPath)
    {
        if (string.IsNullOrEmpty (stageDataPath) == true)
        {
            return;
        }

        var csvFile = Resources.Load (string.Format ("Stages/{0}", stageDataPath)) as TextAsset;
        if (csvFile == null)
        {
            return;
        }

        var lines = csvFile.text.Split ('\n');
        int index = 0;
        bool continued = false;
        foreach (var line in lines)
        {
            if (continued == true)
            {
                continued = false;
                index++;
            }

            if (index >= 16)
            {
                break;
            }

            var replacedLine = line.Replace ("\r", "");
            if (string.IsNullOrEmpty (replacedLine) || replacedLine.Equals ("null"))
            {
                continued = true;
                continue;
            }

            var datas = replacedLine.Split (new char[] { ',' }, 2);
            string data = string.Empty;
            string subData = string.Empty;

            if (datas.Length >= 1) data = datas[0];
            if (datas.Length >= 2) subData = datas[1];

            var spriteName = data;
            int col = index % BLOCK_ROW_AND_COL_COUNT;
            int row = index / BLOCK_ROW_AND_COL_COUNT;

            GameObject prefabBlock = Resources.Load ("Prefabs/Block") as GameObject;
            GameObject instObject = null;
            if (prefabBlock == null)
            {
                continued = true;
                continue;
            }

            instObject = GameObject.Instantiate (prefabBlock, this.transform, false);
            instObject.transform.localPosition = new Vector2 (-1.5f * BLOCK_WIDTH + BLOCK_WIDTH * col, -1.5f * BLOCK_HEIGHT + BLOCK_HEIGHT * row);

            Sprite sprite = Resources.Load<Sprite> (string.Format ("Blocks/{0}", spriteName));

            if (sprite == null)
            {
                continued = true;
                continue;
            }

            if (instObject.GetComponent<SpriteRenderer> () != null)
            {
                instObject.GetComponent<SpriteRenderer> ().sprite = sprite;
            }

            if (string.IsNullOrEmpty (subData) == false)
            {
                subData = subData.Replace ("[", "").Replace ("]", "");
                var subDatas = subData.Split (',');

                if (subDatas.Length == 3)
                {
                    var type = subDatas[0];
                    var posX = float.Parse (subDatas[1]);
                    var posY = float.Parse (subDatas[2]);

                    GameObject subInstObject = null;

                    if (type.Equals ("goal"))
                    {
                        GameObject prefabGoal = Resources.Load ("Prefabs/Goal") as GameObject;
                        if (prefabGoal != null)
                        {
                            subInstObject = GameObject.Instantiate (prefabGoal, instObject.transform, false);

                            Direction dirGoal = GetDirectionFromSpriteName (spriteName);
                            if (dirGoal == Direction.Left)
                            {
                                subInstObject.transform.localScale = new Vector3 (1f, 1f, 1f);
                            }
                            else if (dirGoal == Direction.Right || dirGoal == Direction.Down)
                            {
                                subInstObject.transform.localScale = new Vector3 (-1f, 1f, 1f);
                            }
                            else if (dirGoal == Direction.Up)
                            {
                                subInstObject.transform.localScale = new Vector3 (1f, -1f, 1f);
                            }
                        }
                    }
                    else if (type.Equals ("star"))
                    {
                        GameObject prefabStar = Resources.Load ("Prefabs/Star") as GameObject;
                        if (prefabStar != null)
                        {
                            subInstObject = GameObject.Instantiate (prefabStar, instObject.transform, false);
                        }
                    }

                    if (subInstObject != null)
                    {
                        subInstObject.transform.localPosition = new Vector2 (posX, posY);
                    }
                }
            }

            index++;
        }

        InitFromScene ();
    }

    public void InitFromScene ()
    {
        for (int i = 0; i < transform.childCount; i++)
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
                    StageManager.Instance.ball.transform.position = block.transform.position;
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

    private void Update ()
    {
        if (isSolution == true && completeSolution == false)
        {
            readyGameEndSeconds += Time.deltaTime;
            if (readyGameEndSeconds >= SOLUTION_READY_SECONDS)
            {
                completeSolution = true;

                BeginMoveBall ();

                return;
            }
        }
    }

    private void BeginMoveBall ()
    {
        var pathPoints = ProcBlocksToPathPoint (pathSolution);
        StageManager.Instance.ball.StartAlongPath (pathPoints);

        UIManager.Instance.HideTopButtons ();
        remainBlocks = FindRemainBlocks ();

        foreach (var block in remainBlocks)
        {
            block.SetBlockDimmed ();
        }

        foreach (var block in blocks)
        {
            if (block != null)
            {
                block.blockMovement.enabled = false;
            }
        }
    }

    private bool IsValidPosition (float x, float y, out int ox, out int oy)
    {
        int i = 0;
        int j = 0;
        bool valid = false;

        for (float dx = -1.5f * BLOCK_WIDTH; dx <= 1.5f * BLOCK_WIDTH; dx += BLOCK_WIDTH, i++)
        {
            if (Mathf.Approximately(dx, x) == true)
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

        valid = false;

        for (float dy = -1.5f * BLOCK_HEIGHT; dy <= 1.5f * BLOCK_HEIGHT; dy += BLOCK_HEIGHT, j++)
        {
            if (Mathf.Approximately (dy, y) == true)
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


    private bool CheckSolutionFound ()
    {
        var curBlock = startPoint;
        Direction fromDirection = Direction.Null;
        pathSolution.Clear ();
        int whileIterCount = 0;
        int iterCountMax = BLOCK_ROW_AND_COL_COUNT * BLOCK_ROW_AND_COL_COUNT + BLOCK_ROW_AND_COL_COUNT;

        while (curBlock != null && whileIterCount < iterCountMax)
        {
            var x = (int) curBlock.currentPosition.x;
            var y = (int) curBlock.currentPosition.y;
            var direction = curBlock.blockDirectionType;

            pathSolution.Add (curBlock);

            Block nextBlock = null;

            if (curBlock.Equals (endPoint))
            {
                bool isSolution = false;

                switch (direction)
                {
                    case BlockDirectionType.Up:
                        if (fromDirection == Direction.Up)
                        {
                            isSolution = true;
                        }
                        break;
                    case BlockDirectionType.Down:
                        if (fromDirection == Direction.Down)
                        {
                            isSolution = true;
                        }
                        break;
                    case BlockDirectionType.Left:
                        if (fromDirection == Direction.Left)
                        {
                            isSolution = true;
                        }
                        break;
                    case BlockDirectionType.Right:
                        if (fromDirection == Direction.Right)
                        {
                            isSolution = true;
                        }
                        break;
                    default:
                        break;
                }

                return isSolution;
            }
            else if (curBlock.Equals (startPoint))
            {
                switch (direction)
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
                            else if (fromDirection == Direction.Down)
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
        if (x < 0 || x >= BLOCK_ROW_AND_COL_COUNT || y < 0 || y >= BLOCK_ROW_AND_COL_COUNT)
        {
            return;
        }

        int index = y * BLOCK_ROW_AND_COL_COUNT + x;
        if (index > blocks.Length)
            return;
        blocks[index] = target;
        target.SetPosition (x, y);
    }

    public void UnsetBlock (int x, int y)
    {
        if (x < 0 || x >= BLOCK_ROW_AND_COL_COUNT || y < 0 || y >= BLOCK_ROW_AND_COL_COUNT)
        {
            return;
        }

        int index = y * BLOCK_ROW_AND_COL_COUNT + x;
        if (index > blocks.Length || index < 0)
            return;
        blocks[index] = null;
    }

    public Block GetBlock (int x, int y)
    {
        if (x < 0 || x >= BLOCK_ROW_AND_COL_COUNT || y < 0 || y >= BLOCK_ROW_AND_COL_COUNT)
        {
            return null;
        }

        int index = y * BLOCK_ROW_AND_COL_COUNT + x;
        if (index >= blocks.Length || index < 0)
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

        isSolution = false;
        readyGameEndSeconds = 0f;
    }

    public void MoveEnd()
    {
        isSolution = CheckSolutionFound ();
        readyGameEndSeconds = 0f;
    }

    private List<PathPoint> ProcBlocksToPathPoint(List<Block> blocks)
    {
        List<PathPoint> pathPoints = new List<PathPoint> ();

        foreach(var block in blocks)
        {
            var blockType = block.blockType;
            var directionType = block.blockDirectionType;

            PathPoint pathPoint = null;
            Vector2 pivotPoint = Vector2.zero;
            bool hasPivot = false;

            if(blockType == BlockType.StartPoint)
            {
                pivotPoint = Vector2.zero;
                hasPivot = false;
            }
            else if(blockType == BlockType.EndPoint)
            {
                pivotPoint = Vector2.zero;
                hasPivot = false;
            }
            else
            {
                switch(directionType)
                {
                    case BlockDirectionType.UpAndLeft:
                        pivotPoint = block.transform.localPosition;
                        pivotPoint += new Vector2 (-BLOCK_WIDTH * 0.5f, BLOCK_HEIGHT * 0.5f);
                        hasPivot = true;
                        break;
                    case BlockDirectionType.UpAndRight:
                        pivotPoint = block.transform.localPosition;
                        pivotPoint += new Vector2 (BLOCK_WIDTH * 0.5f, BLOCK_HEIGHT * 0.5f);
                        hasPivot = true;
                        break;
                    case BlockDirectionType.DownAndLeft:
                        pivotPoint = block.transform.localPosition;
                        pivotPoint += new Vector2 (-BLOCK_WIDTH * 0.5f, -BLOCK_HEIGHT * 0.5f);
                        hasPivot = true;
                        break;
                    case BlockDirectionType.DownAndRight:
                        pivotPoint = block.transform.localPosition;
                        pivotPoint += new Vector2 (BLOCK_WIDTH * 0.5f, -BLOCK_HEIGHT * 0.5f);
                        hasPivot = true;
                        break;
                    default:
                        pivotPoint = Vector2.zero;
                        hasPivot = false;
                        break;
                }
            }

            pathPoint = new PathPoint (block.transform.localPosition, pivotPoint, hasPivot);
            pathPoints.Add (pathPoint);
        }

        return pathPoints;
    }

    private Direction GetDirectionFromSpriteName (string spriteName)
    {
        var words = spriteName.Split ('_');
        if (words.Length != 3)
        {
            return Direction.Null;
        }

        switch (words[1][0])
        {
            case 'l':
                return Direction.Left;
            case 'r':
                return Direction.Right;
            case 'u':
                return Direction.Up;
            case 'd':
                return Direction.Down;
            default:
                return Direction.Null;
        }
    }

    private List<Block> FindRemainBlocks ()
    {
        List<Block> retBlocks = new List<Block> ();

        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] == null)
            {
                continue;
            }

            if (pathSolution.Contains (blocks[i]) == true)
            {
                continue;
            }

            retBlocks.Add (blocks[i]);
        }

        return retBlocks;
    }
}
