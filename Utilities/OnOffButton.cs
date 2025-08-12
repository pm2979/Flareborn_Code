using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnOffButton:MonoBehaviour
{
    public List<Sprite> sprites;
    public Image buttonImage;

    public void On()
    {
        this.buttonImage.sprite = this.sprites[0];
    }

    public void Off()
    {
        this.buttonImage.sprite = this.sprites[1];
    }
}
