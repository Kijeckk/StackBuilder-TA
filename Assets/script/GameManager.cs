using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject blockPrefab;

    private Transform lastBlock;
    private float blockHeight = 1f;
    private CameraFollow cam;
    private float currentSizeX = 3f;

    void Start()
    {
        lastBlock = GameObject.Find("StartBlock").transform;

        cam = Camera.main.GetComponent<CameraFollow>();
        cam.SetTarget(lastBlock);
        
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
        float maxSize = lastBlock.localScale.x;
        float overlap = maxSize - Mathf.Abs(hangover);

        if (overlap <= 0)
        {
            Debug.Log("Game Over");
            return;
        }

        // Resize block
        Vector3 newScale = currentBlock.localScale;
        newScale.x = overlap;
        currentBlock.localScale = newScale;

        // 🔥 TAMBAHKAN INI
        currentSizeX = overlap;

        // Reposition
        float newX = lastBlock.position.x + (hangover / 2);
        currentBlock.position = new Vector3(newX, currentBlock.position.y, currentBlock.position.z);

        lastBlock = currentBlock;
        cam.SetTarget(lastBlock);

        SpawnBlock();
    }
}