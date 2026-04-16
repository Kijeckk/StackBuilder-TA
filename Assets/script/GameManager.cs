using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject blockPrefab;

    private Transform lastBlock;
    private float blockHeight = 1f;
    private CameraFollow cam;
    private float currentSizeX = 3f;

    private int score = 0;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        lastBlock = GameObject.Find("StartBlock").transform;

        cam = Camera.main.GetComponent<CameraFollow>();
        cam.SetTarget(lastBlock);

        scoreText.text = "Score: 0";
        
        SpawnBlock();
        
    }

    void SpawnBlock()
{
    Vector3 pos = lastBlock.position + Vector3.up * blockHeight;
    GameObject newBlock = Instantiate(blockPrefab, pos, Quaternion.identity);

    // 🔥 SET SIZE SESUAI HASIL POTONGAN
    newBlock.transform.localScale = new Vector3(currentSizeX, 1f, 3f);

    newBlock.GetComponent<Renderer>().material.color = Random.ColorHSV();
}

    public void PlaceBlock(Block current)
{
    Transform currentBlock = current.transform;

    float hangover = currentBlock.position.x - lastBlock.position.x;

    // ✅ PERFECT CHECK
    bool isPerfect = Mathf.Abs(hangover) < 0.05f;

    float maxSize = lastBlock.localScale.x;
    float overlap = maxSize - Mathf.Abs(hangover);

    if (overlap <= 0)
    {
        Debug.Log("Game Over");
        return;
    }

    // 🔥 SCORING SYSTEM
    if (isPerfect)
    {
        score += 2;
        Debug.Log("PERFECT! Score: " + score);
    }
    else
    {
        score++;
        Debug.Log("Score: " + score);
    }

    scoreText.text = "Score: " + score;

    // 🔥 PERFECT SNAP (biar pas banget)
    if (isPerfect)
    {
        overlap = maxSize;
        currentBlock.position = new Vector3(lastBlock.position.x, currentBlock.position.y, currentBlock.position.z);
    }

    // Resize block
    Vector3 newScale = currentBlock.localScale;
    newScale.x = overlap;
    currentBlock.localScale = newScale;

    currentSizeX = overlap;

    // Reposition
    float newX = lastBlock.position.x + (hangover / 2);
    currentBlock.position = new Vector3(newX, currentBlock.position.y, currentBlock.position.z);

    lastBlock = currentBlock;
    cam.SetTarget(lastBlock);

    SpawnBlock();
}
}