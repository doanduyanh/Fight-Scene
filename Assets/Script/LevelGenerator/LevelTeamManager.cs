using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelTeamManager : MonoBehaviour
{
    public static LevelTeamManager instance;
    public int level;
    public GameObject playerSpawnPoint;
    public GameObject player;
    public GameObject[] enemyPrefabs;
    public GameObject alliesPrefabs;
    public Text levelText;
    public Text winLoseText;
    private int enemyLeft;

    public float minZ = -3.5f;
    public float maxZ = 4.2f;
    public float minX = -4f;
    public float maxX = 3.9f;




    private bool win;
    private void Awake()
    {
        MakeInstance();
    }

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //MakeSingleton();
        ReGeneratePlayer();
        GenerateLevel();
        levelText.text = "Level: " + level;
    }
    // Update is called once per frame
    void Update()
    {
        if (win)
        {
            winLoseText.gameObject.SetActive(true);
            winLoseText.text = "WIN!";
            win = false;
            StartCoroutine(LevelUp());
        }
    }
    void GenerateLevel()
    {
        int enemyNum = level * 3;
        int enemyLevel = level < 5 ? 0 : 1;

        enemyLeft = enemyNum;
        for (int i = 0; i < enemyNum; i++)
        {
            // Generate random position within the specified ranges
            float randomX = Random.Range(minX, maxX);
            float randomY = 0.5f;
            float randomZ = Random.Range(1f, maxZ);

            Vector3 randomPosition = new Vector3(randomX, randomY, randomZ);

            CharGenerator charGen = new CharGenerator();
            GameObject newCharacter = Instantiate(enemyPrefabs[enemyLevel], randomPosition, Quaternion.identity);
            CharacterController enemyController = newCharacter.GetComponent<CharacterController>();
            CharStructure charGened = charGen.GenerateEnemyByWeight(level / enemyNum);
            enemyController.updateValue(charGened);
            HealthScript enemyHealth = newCharacter.GetComponent<HealthScript>();
            if (enemyHealth != null)
            {
                enemyHealth.OnDeath += HandleEnemyDeath;
            }

        }
        win = false;

    }
    private void HandleEnemyDeath()
    {
        enemyLeft -= 1;
        if (enemyLeft <= 0)
        {
            win = true;
        }
    }
    private void HandlePlayerDeath()
    {
        StartCoroutine(BackToMainMenu());
    }

    IEnumerator LevelUp()
    {
        yield return new WaitForSeconds(1f);
        winLoseText.text = "Level Up!";
        yield return new WaitForSeconds(2f);
        winLoseText.gameObject.SetActive(false);
        level++;
        levelText.text = "Level: " + level;
        ReGeneratePlayer();
        GenerateLevel();
    }
    IEnumerator BackToMainMenu()
    {
        winLoseText.gameObject.SetActive(true);
        winLoseText.text = "Lose...";
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneIndex.MAINMENU);
    }
    void ReGeneratePlayer()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag(Tags.ALLIES_TAG))
        {
            Destroy(go);
        }

        int alliesNum = level * 3 - 1;

        for (int i = 0; i < alliesNum; i++)
        {
            // Generate random position within the specified ranges
            float randomX = Random.Range(minX, maxX);
            float randomY = 0.5f;
            float randomZ = Random.Range(minZ, -1f);

            Vector3 randomPosition = new Vector3(randomX, randomY, randomZ);

            CharGenerator charGen = new CharGenerator();
            GameObject newCharacter = Instantiate(alliesPrefabs, randomPosition, Quaternion.identity);
            CharacterController enemyController = newCharacter.GetComponent<CharacterController>();
            CharStructure charGened = charGen.GenerateEnemyByWeight(level / alliesNum);
            enemyController.updateValue(charGened);

        }


        HealthScript playerHealth = player.GetComponent<HealthScript>();
        player.transform.position = playerSpawnPoint.transform.position;
        player.transform.rotation = Quaternion.identity;
        playerHealth.PlayerReset();
        if (playerHealth != null)
        {
            playerHealth.OnDeath += HandlePlayerDeath;
        }
    }
}
