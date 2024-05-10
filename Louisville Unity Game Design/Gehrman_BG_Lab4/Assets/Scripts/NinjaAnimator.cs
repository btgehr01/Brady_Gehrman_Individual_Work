using UnityEngine;

public class NinjaAnimation : MonoBehaviour
{
    public Sprite[] ninjaSprites;
    public float moveSpeed = 5f;
    public float framesPerSecond;
    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = ninjaSprites[0];
    }

    private void Update()
    {
        // Check for left and right arrow key input
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // Update the transform position based on input
        transform.position += new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0f, 0f);

        // Check if we need to flip the sprite horizontally
        if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
            int index = (int)(Time.time * framesPerSecond) % ninjaSprites.Length;
            spriteRenderer.sprite = ninjaSprites[index];
        }
        else if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
            int index = (int)(Time.time * framesPerSecond) % ninjaSprites.Length;
            spriteRenderer.sprite = ninjaSprites[index];
        }

        // // Update the sprite animation
        // currentSpriteIndex = (currentSpriteIndex + 1) % ninjaSprites.Length;
        // spriteRenderer.sprite = ninjaSprites[currentSpriteIndex];
    }
}
