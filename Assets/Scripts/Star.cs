using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public Sprite spriteAfter;

    public void GetStar()
    {
        this.GetComponent<SpriteRenderer> ().sprite = spriteAfter;
    }
}
