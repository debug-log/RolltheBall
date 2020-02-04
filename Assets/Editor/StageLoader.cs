using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class StageLoader : MonoBehaviour
{
    private static BlockManager blockManager;
    private static Ball ball;

    [MenuItem ("Tools/Load Stage Data")]
    public static void LoadStageData ()
    {
        blockManager = GameObject.FindObjectOfType<BlockManager> ();
        if (blockManager == null)
        {
            Debug.LogError ("StageManager does not exists.");
            return;
        }

        ball = GameObject.FindObjectOfType<Ball> ();
        if (ball == null)
        {
            Debug.LogError ("Ball does not exists.");
            return;
        }

        string filePath = EditorUtility.OpenFilePanel ("Save your stage Data", "Assets/Resources/Stages", "csv");
        if (string.IsNullOrEmpty (filePath) == true)
        {
            return;
        }

        RemoveAllChildObjects (blockManager.transform);
        LoadStageDataFromCsvFile (filePath);
        SetUITitleText (filePath);
    }

    private static void RemoveAllChildObjects (Transform transform)
    {
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Undo.DestroyObjectImmediate (transform.GetChild(i).gameObject);
        }
    }

    private static void LoadStageDataFromCsvFile(string stageDataPath)
    {
        var fileName = Path.GetFileNameWithoutExtension (stageDataPath);
        if (string.IsNullOrEmpty (fileName) == true)
        {
            return;
        }

        var csvFile = Resources.Load (string.Format ("Stages/{0}", fileName)) as TextAsset;
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
            int col = index % 4;
            int row = index / 4;
            
            GameObject prefabBlock = Resources.Load ("Prefabs/Block") as GameObject;
            GameObject instObject = null;
            if (prefabBlock == null)
            {
                continued = true;
                continue;
            }

            instObject = GameObject.Instantiate (prefabBlock, blockManager.transform, false);
            instObject.transform.localPosition =new Vector2 (
                -1.5f * BlockManager.BLOCK_WIDTH + BlockManager.BLOCK_WIDTH * col,
                -1.5f * BlockManager.BLOCK_HEIGHT + BlockManager.BLOCK_HEIGHT * row);
            Undo.RegisterCreatedObjectUndo (instObject, "Loaded objects");

            Sprite sprite = Resources.Load<Sprite> (string.Format ("Blocks/{0}", spriteName));

            if (spriteName.EndsWith ("_start") == true)
            {
                ball.transform.position = instObject.transform.localPosition;
            }

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
                            Undo.RegisterCreatedObjectUndo (subInstObject, "Loaded objects");

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
                            Undo.RegisterCreatedObjectUndo (subInstObject, "Loaded objects");
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
    }

    private static void SetUITitleText (string stageDataPath)
    {
        string fileName = Path.GetFileNameWithoutExtension (stageDataPath);

        if (string.IsNullOrEmpty (fileName) == true)
        {
            return;
        }

        var uiManager = GameObject.FindObjectOfType<UIManager> ();
        if (uiManager == null)
        {
            return;
        }

        uiManager.SetStageText (fileName);
    }

    private static Direction GetDirectionFromSpriteName (string spriteName)
    {
        var words = spriteName.Split ('_');
        if (words.Length != 3)
        {
            return Direction.Null;
        }

        switch(words[1][0])
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
}
