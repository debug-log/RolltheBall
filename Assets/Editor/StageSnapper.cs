using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StageSnapper : MonoBehaviour
{
    private static BlockManager blockManager;

    [MenuItem ("Tools/Snap All Blocks")]
    public static void SnapStageBlocks ()
    {
        blockManager = GameObject.FindObjectOfType<BlockManager> ();
        if (blockManager == null)
        {
            Debug.LogError ("BlockManager does not exists.");
            return;
        }


        Vector2[] tempArray = new Vector2[BlockManager.BLOCK_ROW_AND_COL_COUNT * BlockManager.BLOCK_ROW_AND_COL_COUNT];
        int i = 0;
        float endValue = (BlockManager.BLOCK_ROW_AND_COL_COUNT - 1) * 0.5f;

        for (float dx = -endValue * BlockManager.BLOCK_WIDTH; dx <= endValue * BlockManager.BLOCK_WIDTH; dx += BlockManager.BLOCK_WIDTH)
        {
            for (float dy = -endValue * BlockManager.BLOCK_HEIGHT; dy <= endValue * BlockManager.BLOCK_HEIGHT; dy += BlockManager.BLOCK_HEIGHT)
            {
                float tx = Mathf.Round (dx * 1000) / 1000;
                float ty = Mathf.Round (dy * 1000) / 1000;
                tempArray[i] = new Vector2 (tx, ty);
                i++;
            }
        }

        for (i = 0; i < blockManager.transform.childCount; i++)
        {
            var block = blockManager.transform.GetChild (i);

            foreach (var cell in tempArray)
            {
                if (Vector2.Distance (block.transform.position, cell) < 1f)
                {
                    Undo.RecordObject (block, "Move");
                    block.transform.position = cell;
                    break;
                }
            }
        }
    }
}
