using UnityEngine;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour
{
    public float speed = 3f;
    private bool moving = true;
    private bool moveRight = true;

void Update()
{
    if (!moving) return;

    // ⛔ stop kalau pause
    if (Time.timeScale == 0f) return;

    // ⛔ kalau klik UI (biar pause button aman)
    if (EventSystem.current.IsPointerOverGameObject()) return;

    float move = speed * Time.deltaTime;

    if (moveRight)
        transform.Translate(Vector3.right * move);
    else
        transform.Translate(Vector3.left * move);

    if (transform.position.x > 3f) moveRight = false;
    if (transform.position.x < -3f) moveRight = true;

    // ✅ ambil GameManager SEKALI
    GameManager gm = FindFirstObjectByType<GameManager>();

    // ✅ input klik + spasi
    if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
    {
        moving = false;
        gm.PlaceBlock(this);
    }
}
}