using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    enum GameStates
    {
        Menu = 0,
        Playing = 1,
        Paused = 2,
        GameOver = 3,
    }

    private GameStates currentState;

    public AudioSource GameMusic; 

    [Header("Enemies")]
    bool waveSpawn;
    int numEnemies;
    public int maxEnemies;
    int enemiesSpawned;


    public GameObject[] enemyPrefabs;
    public Transform[] enemySpawns;
    public Transform[] minMaxSpawnPoints;
    public float enemySpawnDelay;
    float enemySpawnTimer;
    private int waveCounter;


    [Header("Player stuff")]
    GameObject player;
    private int score;
    private int hiscore;
    [Tooltip("Time decrease of spawn")]
    public float roundDifficultyIncrease;


    [Header("UI")]
    public GameObject menuUI;
    public GameObject ingameUI;
    public GameObject pausedUI;
    public GameObject gameoverUI;

    public Text hiscoreText;
    public Text currentscoreText; 

    [Header("Animation")]
    public Animator drawbridgeAnim;

    [Header("transparency")]
    public GameObject wall;
    Color originalColour;
    public float transparencyAmount;
    float t = 0.0f;
    float d = 0.0f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Use this for initialization
    void Start()
    {
        currentState = GameStates.Menu;
        enemySpawnTimer = enemySpawnDelay;
        waveCounter = 0;
        enemiesSpawned = 0;
        originalColour = wall.GetComponent<MeshRenderer>().material.color;
        GameMusic = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        switch (currentState)
        {

            case GameStates.Menu:
                Time.timeScale = 1;
                menuUI.SetActive(true);
                pausedUI.SetActive(false);
                ingameUI.SetActive(false);
                gameoverUI.SetActive(false);
                player.GetComponent<Movement>().enabled = false;
                player.GetComponent<Attack>().enabled = false;
                break;

            case GameStates.Playing:
                //ui

                score = player.GetComponent<Attack>().Score;
                hiscore = PlayerPrefs.GetInt("Hiscore", 0);
                hiscoreText.text = "Highscore: " + hiscore.ToString();
                currentscoreText.text = "Score: " + score.ToString();
                player.GetComponent<Movement>().enabled = true;
                player.GetComponent<Attack>().enabled = true;

                Time.timeScale = 1;
                pausedUI.SetActive(false);
                ingameUI.SetActive(true);
                gameoverUI.SetActive(false);
                menuUI.SetActive(false);
                numEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
                enemySpawnTimer -= Time.deltaTime;
                WaveSpawn();
                Debug.Log(waveCounter);
                CheckAlive();

                if(player.transform.position.z > wall.transform.position.z)
                {
                    d = 0.0f;
                    wall.GetComponent<MeshRenderer>().material.color = new Color(originalColour.r, originalColour.g, originalColour.b, Mathf.Lerp(originalColour.a, transparencyAmount, t));
                    t += Time.deltaTime;
                }
                else
                {
                    t = 0.0f;
                    wall.GetComponent<MeshRenderer>().material.color = new Color(originalColour.r, originalColour.g, originalColour.b, Mathf.Lerp(wall.GetComponent<MeshRenderer>().material.color.a, originalColour.a, d));
                    d += Time.deltaTime;
                }

                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    currentState = GameStates.Paused;
                }
                if(waveCounter == 7)
                {
                    drawbridgeAnim.Play(0);
                }

                break;
            case GameStates.Paused:
                //ui
                ingameUI.SetActive(false);
                pausedUI.SetActive(true);
                player.GetComponent<Movement>().enabled = false;
                Time.timeScale = 0;


                break;
            case GameStates.GameOver:
                //ui
                PlayerPrefs.SetInt("Hiscore", score);
                ingameUI.SetActive(false);
                gameoverUI.SetActive(true);
                player.GetComponent<Movement>().enabled = false;
                Time.timeScale = 0;


                break;
        }

    }

    public void Resume()
    {
        currentState = GameStates.Playing;
    }

    void CheckAlive()
    {
        if (player.GetComponent<Movement>().currentHealth <= 0)
        {
            currentState = GameStates.GameOver;
        }
    }

    void WaveSpawn()
    {
        if (waveSpawn)
        {
            if (enemySpawnTimer <= 0)
            {
                if (enemiesSpawned < maxEnemies)
                {
                    SpawnEnemy();
                    enemiesSpawned++;
                }
            }
        }
        if (numEnemies == 0)
        {
            if (!waveSpawn)
            {
                waveCounter++;
                enemiesSpawned = 0;
                enemySpawnDelay -= roundDifficultyIncrease;
            }
            waveSpawn = true;
        }
        if (enemiesSpawned == maxEnemies)
        {
            waveSpawn = false;
        }
    }
    void SpawnEnemy()
    {

        Transform randSpawn = enemySpawns[Random.Range(0, enemySpawns.Length)];
        GameObject randEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        Vector3 randV3 = new Vector3(Random.Range(minMaxSpawnPoints[0].position.x, minMaxSpawnPoints[1].position.x), 1, Random.Range(minMaxSpawnPoints[0].position.z, minMaxSpawnPoints[1].position.z));

        Instantiate(randEnemy, randV3, Quaternion.identity);
        enemySpawnTimer = enemySpawnDelay;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayGame()
    {
        currentState = GameStates.Playing;
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene(1);
    }
}
