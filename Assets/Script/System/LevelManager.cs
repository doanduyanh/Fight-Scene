using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //public static LevelManager instance;
    public int level;
    public GameObject[] spawnPoint;
    public GameObject playerSpawnPoint;
    public GameObject player;
    public GameObject[] enemyPrefabs;
    public Text levelText;
    public Text winLoseText;

    private bool win;
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

        CharGenerator charGen = new CharGenerator();
        GameObject newCharacter = Instantiate(enemyPrefabs[level<5?0:1], spawnPoint[0].transform.position, spawnPoint[0].transform.rotation);
        EnemyController enemyController = newCharacter.GetComponent<EnemyController>();
        CharStructure charGened = charGen.GenerateEnemyByWeight(level);
        enemyController.updateValue(charGened);
        HealthScript enemyHealth = newCharacter.GetComponent<HealthScript>();
        if (enemyHealth != null)
        {
            enemyHealth.OnEnemyDeath += HandleEnemyDeath;
        }
        win = false;

    }
    private void HandleEnemyDeath()
    {
        win = true;
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
        /*if(GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG) != null)
        {
            Destroy(GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG));
        }
        Instantiate(playerPrefab, playerSpawnPoint.transform.position, playerSpawnPoint.transform.rotation);*/
        HealthScript playerHealth = player.GetComponent<HealthScript>();
        player.transform.position = playerSpawnPoint.transform.position;
        player.transform.rotation = Quaternion.identity;
        playerHealth.playerReset();
        if (playerHealth != null)
        {
            playerHealth.OnPlayerDeath += HandlePlayerDeath;
        }
    }
}
