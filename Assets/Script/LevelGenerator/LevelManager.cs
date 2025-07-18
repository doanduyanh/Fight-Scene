using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
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
        ReGeneratePlayer();
        GenerateLevel();
        levelText.text = "Level: " + level;
    }
    // Update is called once per frame
    void Update()
    {
        if (win && level < 10)
        {

            winLoseText.gameObject.SetActive(true);
            winLoseText.text = "Level Up!";
            win = false;
            StartCoroutine(LevelUp());
        }
        else if(win && level == 10)
        {
            winLoseText.gameObject.SetActive(true);
            winLoseText.text = "You WIN!";
            win = false;
            StartCoroutine(BackToMainMenu());
        }
    }
    void GenerateLevel()
    {

        CharGenerator charGen = new CharGenerator();
        GameObject newCharacter = Instantiate(enemyPrefabs[level<5?0:1], spawnPoint[0].transform.position, spawnPoint[0].transform.rotation);
        CharacterController enemyController = newCharacter.GetComponent<CharacterController>();
        CharStructure charGened = charGen.GenerateEnemyByWeight(level);
        enemyController.updateValue(charGened);
        HealthScript enemyHealth = newCharacter.GetComponent<HealthScript>();
        if (enemyHealth != null)
        {
            enemyHealth.OnDeath += HandleEnemyDeath;
        }
        win = false;

    }
    private void HandleEnemyDeath()
    {
        win = true;
    }
    private void HandlePlayerDeath()
    {
        winLoseText.gameObject.SetActive(true);
        winLoseText.text = "You Lose";
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
        yield return new WaitForSeconds(3f);
        winLoseText.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneIndex.MAINMENU);
    }
    void ReGeneratePlayer()
    {
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
