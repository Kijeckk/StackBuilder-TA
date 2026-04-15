using UnityEngine;

public class Block : MonoBehaviour
{
    public float speed = 3f;
    private bool moving = true;
    private bool moveRight = true;

    void Update()
    {
        if (!moving) return;

        float move = speed * Time.deltaTime;

        if (moveRight)
            transform.Translate(Vector3.right * move);
        else
            transform.Translate(Vector3.left * move);

        if (transform.position.x > 3f) moveRight = false;
        if (transform.position.x < -3f) moveRight = true;

        if (Input.GetMouseButtonDown(0))
        {
            moving = false;
            FindFirstObjectByType<GameManager>().PlaceBlock(this);
        }
    }
}