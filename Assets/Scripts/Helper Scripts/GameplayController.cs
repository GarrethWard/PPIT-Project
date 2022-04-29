using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance;

    public GameObject fruitPickup, bombPickup;

    private float minX = -4.25f, maxX = 4.25f, minY = -2.26f, maxY = 2.26f;
    private float zPos = -0.01f;

    private Text scoreText;
    private int scoreCount;
    
    // Start is called before the first frame update
    void Awake()
    {
        MakeInstance();
    }

    void Start()
    {
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        Invoke("StartSpawning", 0.5f);
    }

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void StartSpawning()
    {
        StartCoroutine(SpawnPickups());
    }

    public void CancelSpawning()
    {
        CancelInvoke("StartSpawning");
    }
    IEnumerator SpawnPickups()
    {
        yield return new WaitForSeconds(Random.Range(1f, 5f));
        if (Random.Range(0, 10) >= 2)
        {
            Instantiate(fruitPickup, new Vector3(Random.Range(minX, maxX),
                                                Random.Range(minY, maxY), zPos),
                                                Quaternion.identity);
        }
        else
        {
            Instantiate(bombPickup, new Vector3(Random.Range(minX, maxX),
                                              Random.Range(minY, maxY), zPos),
                                              Quaternion.identity);
        }

        Invoke("StartSpawning",0f);

        AudioManager.instance.PlaySpawnPickUpSound();


    }

    public void IncreaseScore()
    {
        scoreCount++;
        scoreText.text = "Score: " + scoreCount;

    }
}
