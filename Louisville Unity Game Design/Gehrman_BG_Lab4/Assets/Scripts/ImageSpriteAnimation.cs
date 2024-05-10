using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSpriteAnimation : MonoBehaviour
{
    public Sprite[] sprites;
    public float framesPerSecond = 30f;

    private Image image;

    // Start is called before the first frame update
    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = sprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        int index = (int)(Time.time * framesPerSecond) % sprites.Length;
        image.sprite = sprites[index];
    }
}
