using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIImageTitleAnimation : UIImageAnimation
{
    private RectTransform rtTransform;

    private bool isEndCondition;
    public float moveSpeed = 50f;

    public Sprite spriteEnd;

    protected override void Start ()
    {
        base.Start ();

        rtTransform = this.GetComponent<RectTransform> ();
    }

    protected override void Update()
    {
        if (isEndCondition)
        {
            return;
        }

        base.Update ();

        if (this.rtTransform.anchoredPosition.x > 220f)
        {
            OnEndEvent ();
            isEndCondition = true;
            return;
        }

        Vector2 pos = this.rtTransform.anchoredPosition;
        pos += new Vector2 (this.moveSpeed * Time.deltaTime, 0f);

        this.rtTransform.anchoredPosition = pos;
    }

    void OnEndEvent()
    {
        image.sprite = spriteEnd;
        image.SetNativeSize ();

        rtTransform.anchoredPosition = new Vector2 (235f, -468f);
    }
}
