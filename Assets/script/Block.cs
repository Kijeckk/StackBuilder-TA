using UnityEngine;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour
{
    public float speed = 3f;
    public bool startMoveRight = true;

    public float minX = -3f;
    public float maxX = 3f;

    public bool useAutoBoundsFromWidth = true;

    private bool moving = true;
    private bool moveRight;
    private float halfWidth;

    void Start()
    {
        GameManager gm = FindFirstObjectByType<GameManager>();

        if (gm != null)
        {
            float centerX = gm.GetLastBlock().position.x;

            float range = 1.2f; // 🔥 kecilin biar gak jauh

            minX = centerX - range;
            maxX = centerX + range;
        }
    }

    void Update()
    {
        if (!moving) return;
        if (Time.timeScale == 0f) return;

        // 🔥 cegah klik UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        float move = speed * Time.deltaTime;

        transform.Translate((moveRight ? Vector3.right : Vector3.left) * move);

        if (transform.position.x > maxX) moveRight = false;
        if (transform.position.x < minX) moveRight = true;

        GameManager gm = FindFirstObjectByType<GameManager>();

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            moving = false;

            if (gm != null)
                gm.PlaceBlock(this);
        }
    }
}