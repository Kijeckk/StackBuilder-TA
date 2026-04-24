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
    public GameObject gameOverUI;
    public GameObject optionsUI;
    public bool isPaused = false;
    public float blockSpeed = 2f;
    public float maxSpeed = 6f;
    public float minSpeed = 1f;
    private int highScore = 0;
    public TextMeshProUGUI highScoreText;

    void Start()
{
    lastBlock = GameObject.Find("StartBlock").transform;

    cam = Camera.main.GetComponent<CameraFollow>();
    cam.SetTarget(lastBlock);

    scoreText.text = "Score: 0";
        // 🔥 HIGH SCORE LOAD
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore;


        SpawnBlock();
}

    public void OpenOptions()
{
    optionsUI.SetActive(true);
    Time.timeScale = 0f;
    isPaused = true;
}

    public void ResumeGame()
    {
        optionsUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    void SpawnBlock()
    {
        Renderer lastRenderer = lastBlock.GetComponent<Renderer>();

        float height = lastRenderer.bounds.size.y;
        float topY = lastRenderer.bounds.max.y;

        Vector3 pos = new Vector3(
            lastBlock.position.x,
            topY + (height / 2f),
            lastBlock.position.z
        );

        // 🔥 BUAT DULU BLOCKNYA
        GameObject newBlock = Instantiate(blockPrefab, pos, Quaternion.identity);

        newBlock.transform.localScale = lastBlock.localScale;

        newBlock.GetComponent<Renderer>().material.color = Random.ColorHSV();

        // 🔥 BARU SET SPEED (SETELAH ADA OBJECT)
        Block blockScript = newBlock.GetComponent<Block>();

        if (blockScript != null)
        {
            blockScript.speed = blockSpeed;
        }
    }

    public void PlaceBlock(Block current)
{
    Transform currentBlock = current.transform;

    float hangover = Mathf.Abs(currentBlock.position.x - lastBlock.position.x);
        if (hangover < 0.1f) // PERFECT
        {
            blockSpeed += 0.5f;
        }
        else if (hangover > 0.5f) // MISS
        {
            blockSpeed -= 0.4f;
        }

        blockSpeed = Mathf.Clamp(blockSpeed, minSpeed, maxSpeed);

        float shrinkAmount = hangover * 0.5f;

    Vector3 newScale = currentBlock.localScale;
    newScale.x -= shrinkAmount;
    newScale.z -= shrinkAmount;

        if (newScale.x <= 0.3f)
        {
            Debug.Log("Game Over");

            // 🔥 CEK HIGH SCORE
            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt("HighScore", highScore);
                PlayerPrefs.Save();
            }

            highScoreText.text = "High Score: " + highScore;

            gameOverUI.SetActive(true);
            Time.timeScale = 0f;
            return;
        }

        currentBlock.localScale = newScale;

    float newX = Mathf.Lerp(currentBlock.position.x, lastBlock.position.x, 0.5f);

currentBlock.position = new Vector3(
    newX,
    currentBlock.position.y,
    currentBlock.position.z
);

    currentSizeX = newScale.x;

    lastBlock = currentBlock;
    cam.SetTarget(lastBlock);

    score++;
    scoreText.text = "Score: " + score;

    SpawnBlock();
}
    
}