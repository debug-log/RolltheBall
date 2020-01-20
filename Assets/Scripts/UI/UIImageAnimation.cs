using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIImageAnimation : MonoBehaviour
{
    protected Image image;
    public Sprite[] sprites;

    public float frameSeconds = 0.5f;
    private float curSeconds = 0f;

    public bool loop = false;
    private bool stop = false;

    private int curSpriteIndex = 0;

    protected virtual void Start ()
    {
        this.image = this.GetComponent<Image> ();
    }

    protected virtual void Update ()
    {
        if (this.image == null)
        {
            return;
        }

        if (this.sprites.Length == 0)
        {
            return;
        }

        if(stop == true)
        {
            return;
        }

        curSeconds += Time.deltaTime;
        if(curSeconds >= frameSeconds)
        {
            curSeconds = 0f;
            if(SetImageNextSprite() == false)
            {
                stop = true;
                return;
            }
        }
    }

    private bool SetImageNextSprite()
    {
        curSpriteIndex++;
        if (curSpriteIndex >= this.sprites.Length)
        {
            if (loop == true)
            {
                curSpriteIndex = 0;
            }
            else
            {
                return false;
            }
        }

        image.sprite = sprites[curSpriteIndex];
        return true;
    }
}
