using UnityEngine;

public class SegmentDirection : MonoBehaviour
{
    // the state of the body.
    // straight vertical
    // straight horizontal
    // corner top left
    // corner top right
    // corner bottom left
    // corner bottom right
    // tail top
    // tail right
    // tail bottom
    // tail left
    // the direction is the side that connects to the previous segment (towards the head)
    public enum Direction
    {
        STRAIGHT_VERTICAL,
        STRAIGHT_HORIZONTAL,
        CORNER_TOP_LEFT,
        CORNER_TOP_RIGHT,
        CORNER_BOTTOM_LEFT,
        CORNER_BOTTOM_RIGHT,
        TAIL_TOP,
        TAIL_RIGHT,
        TAIL_BOTTOM,
        TAIL_LEFT
    }

    // the three different sprites for the body
    // there is straight, corner, and tail
    // they will be rotated and/or flipped to match the direction
    public Sprite straightSprite;
    public Sprite cornerSprite;
    public Sprite tailSprite;

    // the sprite being displayed
    public GameObject spriteObject;
    private SpriteRenderer spriteRenderer => spriteObject.GetComponent<SpriteRenderer>();

    // the direction of the segment
    public Direction direction;
    private Direction lastDirection;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetSprite(direction);
    }

    // Update is called once per frame
    void Update()
    {
        if (direction != lastDirection)
        {
            SetSprite(direction);
            lastDirection = direction;
        }
    }

    void Awake()
    {
        lastDirection = direction;
    }

    private void SetSprite(Direction direction)
    {
        switch (direction)
        {
            case Direction.STRAIGHT_VERTICAL:
                spriteRenderer.sprite = straightSprite;
                break;
            case Direction.STRAIGHT_HORIZONTAL:
                spriteRenderer.sprite = straightSprite;
                spriteObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case Direction.CORNER_TOP_LEFT:
                spriteRenderer.sprite = cornerSprite;
                spriteObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case Direction.CORNER_TOP_RIGHT:
                spriteRenderer.sprite = cornerSprite;
                spriteObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.CORNER_BOTTOM_LEFT:
                spriteRenderer.sprite = cornerSprite;
                spriteObject.transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case Direction.CORNER_BOTTOM_RIGHT:
                spriteRenderer.sprite = cornerSprite;
                spriteObject.transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
            case Direction.TAIL_TOP:
                spriteRenderer.sprite = tailSprite;
                spriteObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.TAIL_RIGHT:
                spriteRenderer.sprite = tailSprite;
                spriteObject.transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
            case Direction.TAIL_BOTTOM:
                spriteRenderer.sprite = tailSprite;
                spriteObject.transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case Direction.TAIL_LEFT:
                spriteRenderer.sprite = tailSprite;
                spriteObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
        }
    }
}
