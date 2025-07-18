using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMultiManager : MonoBehaviour
{
    public int level;
    public GameObject[] spawnPoint;
    public GameObject playerSpawnPoint;
    public GameObject player;
    public GameObject[] enemyPrefabs;
    public Text levelText;
    public Text winLoseText;
    private int enemyLeft;

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
        int enemyNum = level < 3 ? 2 : 3;
        int enemyLevel = level < 5 ? 0 : 1;
        enemyLeft = enemyNum;
        for (int i =0; i< enemyNum; i++)
        {
            CharGenerator charGen = new CharGenerator();
            GameObject newCharacter = Instantiate(enemyPrefabs[enemyLevel], spawnPoint[i].transform.position, spawnPoint[i].transform.rotation);
            CharacterController enemyController = newCharacter.GetComponent<CharacterController>();
            CharStructure charGened = charGen.GenerateEnemyByWeight(level/enemyNum);
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
        if(enemyLeft <= 0)
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
        /*if(GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG) != null)
        {
            Destroy(GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG));
        }
        Instantiate(playerPrefab, playerSpawnPoint.transform.position, playerSpawnPoint.transform.rotation);*/
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
