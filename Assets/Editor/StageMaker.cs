using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;

public class StageMaker : MonoBehaviour
{
    [MenuItem ("Tools/Create Stage Data")]
    public static void CreateStageData()
    {
        string[] datas = new string[16];
        var blockManager = GameObject.FindObjectOfType<BlockManager> ();
        if (blockManager == null)
            return;

        for (int i = 0; i < blockManager.transform.childCount; i++)
        {
            var child = blockManager.transform.GetChild (i);
            if (child == null)
                continue;

            var block = child.GetComponent<Block> ();
            if (block == null)
                continue;

            //위치 세팅
            float posX = block.transform.localPosition.x;
            float posY = block.transform.localPosition.y;
            int x;
            int y;

            if (!IsValidPosition (posX, posY, out x, out y))
            {
                continue;
            }
            
            //블록 타입설정
            var blockSpriteRenderer = block.GetComponent<SpriteRenderer> ();
            if (blockSpriteRenderer == null)
                continue;

            var data = new StringBuilder ();
            var blockSpriteName = blockSpriteRenderer.sprite.name;

            bool hasStar = false;
            bool hasGoal = false;
            Vector2 childPosition = Vector2.zero;

            data.Append (blockSpriteName);

            for(int j=0; j<block.transform.childCount; j++)
            {
                var child2nd = block.transform.GetChild (j);
                var star = child2nd.GetComponent<Star> ();
                if (star != null) 
                {
                    hasStar = true;
                    childPosition = star.transform.localPosition;
                    break;
                }

                var goal = child2nd.GetComponent<Goal> ();
                if(goal != null)
                {
                    hasGoal = true;
                    childPosition = goal.transform.localPosition;
                    break;
                }
            }

            if (hasStar == true)
            {
                data.Append (",[");
                data.Append ("star");
                data.Append (",");
                data.Append (childPosition.x);
                data.Append (",");
                data.Append (childPosition.y);
                data.Append ("]");
            }
            else if(hasGoal == true)
            {
                data.Append (",[");
                data.Append ("goal");
                data.Append (",");
                data.Append (childPosition.x);
                data.Append (",");
                data.Append (childPosition.y);
                data.Append ("]");
            }

            var index = y * 4 + x;
            if (index >= 0 && index < 16)
            {
                datas[index] = data.ToString();
            }
        }

        string outputPath = EditorUtility.SaveFilePanel ("Save your stage Data", "Assets/Resources/Stages", "stage", "csv");
        if(string.IsNullOrEmpty(outputPath) == true)
        {
            return;
        }

        var outputText = new StringBuilder ();
        for(int i=0; i<16; i++)
        {
            var data = datas[i];
            if(string.IsNullOrEmpty(data))
            {
                data = "null";
            }
            outputText.AppendLine (data);
        }

        StreamWriter writer = new StreamWriter (outputPath, false);
        writer.WriteLine (outputText.ToString());
        writer.Close ();

        string relativePath = outputPath;
        if (outputPath.StartsWith (Application.dataPath))
        {
            relativePath = "Assets" + outputPath.Substring (Application.dataPath.Length);
        }
        AssetDatabase.ImportAsset (relativePath);
    }

    private static bool IsValidPosition (float x, float y, out int ox, out int oy)
    {
        int i = 0;
        int j = 0;
        bool valid = false;

        for (float dx = -1.5f * BlockManager.BLOCK_WIDTH; dx <= 1.5f * BlockManager.BLOCK_WIDTH; dx += BlockManager.BLOCK_WIDTH, i++)
        {
            if (dx == x)
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

        for (float dy = -1.5f * BlockManager.BLOCK_HEIGHT; dy <= 1.5f * BlockManager.BLOCK_HEIGHT; dy += BlockManager.BLOCK_HEIGHT, j++)
        {
            if (dy == y)
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
}
