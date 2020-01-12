using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStageBox : MonoBehaviour
{
    public Button button;

    public Text textStageNum;
    public UIImageSwitcher imageBox;
    public UIImageSwitcher[] imageStars;

    public void Init(int stageNum, int numStars, bool cleared)
    {
        textStageNum.text = stageNum.ToString ();
        
        if(cleared == true)
        {
            imageBox.SetImageChanged ();
            for (int i = 0; i < numStars && i < 3; i++)
            {
                imageStars[i].SetImageChanged ();
            }
        }
    }
}
