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

    void Start()
{
    lastBlock = GameObject.Find("StartBlock").transform;

    cam = Camera.main.GetComponent<CameraFollow>();
    cam.SetTarget(lastBlock);

    scoreText.text = "Score: 0";

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

    GameObject newBlock = Instantiate(blockPrefab, pos, Quaternion.identity);

    newBlock.transform.position = new Vector3(
    lastBlock.position.x,
    pos.y,
    lastBlock.position.z
);
    newBlock.transform.localScale = new Vector3(currentSizeX, 0.2f, currentSizeX);

    newBlock.GetComponent<Renderer>().material.color = Random.ColorHSV();
}

   public void PlaceBlock(Block current)
{
    Transform currentBlock = current.transform;

    float hangover = Mathf.Abs(currentBlock.position.x - lastBlock.position.x);

    float shrinkAmount = hangover * 0.5f;

    Vector3 newScale = currentBlock.localScale;
    newScale.x -= shrinkAmount;
    newScale.z -= shrinkAmount;

    if (newScale.x <= 0.3f)
    {
        Debug.Log("Game Over");
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