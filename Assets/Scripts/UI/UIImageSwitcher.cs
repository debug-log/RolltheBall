using UnityEngine;
using UnityEngine.UI;

public class UIImageSwitcher : MonoBehaviour
{
    public Sprite spriteDefault;
    public Sprite spriteChanged;

    public bool changeColorEnable = true;
    public Color colorDefault;
    public Color colorChanged;

    private Image image;

    private void Awake ()
    {
        this.image = this.GetComponent<Image> ();
        SetImageDefault ();
    }

    public bool IsImageChanged()
    {
        if (image == null)
            return false;

        return image.sprite.Equals(spriteChanged);
    }

    public void SetImageDefault()
    {
        if (image != null)
        {
            image.sprite = spriteDefault;

            if (changeColorEnable == true)
                image.color = colorDefault;
        }
    }

    public void SetImageChanged()
    {
        if (image != null)
        {
            image.sprite = spriteChanged;
            if (changeColorEnable == true)
                image.color = colorChanged;
        }
    }
}
