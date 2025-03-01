// using System.Numerics;
using System;
using System.Security.Cryptography;
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
        CORNER_UP_LEFT,
        CORNER_UP_RIGHT,
        CORNER_DOWN_LEFT,
        CORNER_DOWN_RIGHT,
        TAIL_UP,
        TAIL_RIGHT,
        TAIL_DOWN,
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
        SpriteRotator(direction);
    }

    // Update is called once per frame
    void Update()
    {
        if (direction != lastDirection)
        {
            SpriteRotator(direction);
            lastDirection = direction;
        }
    }

    void Awake()
    {
        lastDirection = direction;
    }

    // Have snake call this function to update the sprite direction as the snake moves
    public void SetSpriteDirectionBody(Vector2Int prevSegmentPos, Vector2Int thisSegmentPos, Vector2Int nextSegmentPos) => direction = CalculateBodyDirection(prevSegmentPos, thisSegmentPos, nextSegmentPos);
    public void SetSpriteDirectionTail(Vector2Int thisSegmentPos, Vector2Int prevSegmentPos) => direction = CalculateTailDirection(thisSegmentPos, prevSegmentPos);

    private void SpriteRotator(Direction direction)
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
            case Direction.CORNER_UP_LEFT:
                spriteRenderer.sprite = cornerSprite;
                spriteObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case Direction.CORNER_UP_RIGHT:
                spriteRenderer.sprite = cornerSprite;
                spriteObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.CORNER_DOWN_LEFT:
                spriteRenderer.sprite = cornerSprite;
                spriteObject.transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case Direction.CORNER_DOWN_RIGHT:
                spriteRenderer.sprite = cornerSprite;
                spriteObject.transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
            case Direction.TAIL_UP:
                spriteRenderer.sprite = tailSprite;
                spriteObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.TAIL_RIGHT:
                spriteRenderer.sprite = tailSprite;
                spriteObject.transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
            case Direction.TAIL_DOWN:
                spriteRenderer.sprite = tailSprite;
                spriteObject.transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case Direction.TAIL_LEFT:
                spriteRenderer.sprite = tailSprite;
                spriteObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
        }
    }

    private Direction CalculateBodyDirection(Vector2Int prevSegmentPos, Vector2Int thisSegmentPos, Vector2Int nextSegmentPos)
    {
        // var from = (thisSegmentPos.x - prevSegmentPos.x, thisSegmentPos.y - prevSegmentPos.y);
        // var to = (thisSegmentPos.x - nextSegmentPos.x, thisSegmentPos.y - nextSegmentPos.y);
        Vector2Int from = new Vector2Int(prevSegmentPos.x - thisSegmentPos.x, prevSegmentPos.y - thisSegmentPos.y);
        Vector2Int to = new Vector2Int(nextSegmentPos.x - thisSegmentPos.x, nextSegmentPos.y - thisSegmentPos.y);
        // 0, 0 = same spot
        // 0, 1 = down
        // 0, -1 = up
        // 1, 0 = left
        // // -1, 0 = right
        // var up = (0, -1);
        // var down = (0, 1);
        // var right = (-1, 0);
        // var left = (1, 0);
        
        // if this segment is on the exact same spot as the previous segment, then it is stacked on top of the previous segment
        // display tail
        // if (from == (0, 0))
        //     return Direction.TAIL_UP;

        if (from == Vector2Int.up && to == Vector2Int.down) return Direction.STRAIGHT_VERTICAL;
        if (from == Vector2Int.down && to == Vector2Int.up) return Direction.STRAIGHT_VERTICAL;
        if (from == Vector2Int.left && to == Vector2Int.right) return Direction.STRAIGHT_HORIZONTAL;
        if (from == Vector2Int.right && to == Vector2Int.left) return Direction.STRAIGHT_HORIZONTAL;

        if (from == Vector2Int.down && to == Vector2Int.right) return Direction.CORNER_DOWN_RIGHT;
        if (from == Vector2Int.down && to == Vector2Int.left) return Direction.CORNER_DOWN_LEFT;
        if (from == Vector2Int.up && to == Vector2Int.right) return Direction.CORNER_UP_RIGHT;
        if (from == Vector2Int.up && to == Vector2Int.left) return Direction.CORNER_UP_LEFT;
        if (from == Vector2Int.left && to == Vector2Int.down) return Direction.CORNER_DOWN_LEFT;
        if (from == Vector2Int.left && to == Vector2Int.up) return Direction.CORNER_UP_LEFT;
        if (from == Vector2Int.right && to == Vector2Int.down) return Direction.CORNER_DOWN_RIGHT;
        if (from == Vector2Int.right && to == Vector2Int.up) return Direction.CORNER_UP_RIGHT;

        
        Debug.Log("where am i going");
        return Direction.STRAIGHT_VERTICAL;
    }

    private Direction CalculateTailDirection(Vector2Int thisSegmentPos, Vector2Int prevSegmentPos)
    {
        switch ((prevSegmentPos.x - thisSegmentPos.x, prevSegmentPos.y - thisSegmentPos.y))
        {
            case (< 0, _):
            // this means the previous segment is to the left of this segment
                return Direction.TAIL_LEFT;
            case (> 0, _):
            // this means the previous segment is to the right of this segment
                return Direction.TAIL_RIGHT;
            case (_, < 0):
            // this means the previous segment is below this segment
                return Direction.TAIL_DOWN;
            case (_, > 0):
            // this means the previous segment is above this segment
                return Direction.TAIL_UP;
            default:
                return Direction.TAIL_RIGHT;
        }
    }
}
