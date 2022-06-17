using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public bool isStarted = false;

    public TextMeshProUGUI totalMoneyTxt;
    public TextMeshProUGUI currentGameLevel;

    public TextMeshPro scoreTxt;
    public float score;
    public float totalScore = 499;
    public float totalMoney;
    public GameObject gainMoneyTextObj;

    private CharacterScript character;
    private StepSpawnerScript stepSpawner;
    private VerticalWoodSpawnerScript woodSpawner;

    public GameObject tryAgainPanel;
    public GameObject gameOverPanel;
    public GameObject shopPanel;
    public GameObject tapToStartPanel;

    public Image progressBar;

    public ParticleSystem comfettiParticle;

    [Header("SHOP", order = 0)]
    [Header("Levels", order = 1)]
    public TextMeshProUGUI staminaLevel;
    public TextMeshProUGUI incomeLevel;
    public TextMeshProUGUI speedLevel;

    [Header("Amounts")]
    public TextMeshProUGUI staminaAmount;
    public TextMeshProUGUI incomeAmount;
    public TextMeshProUGUI speedAmount;


    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterScript>();
        stepSpawner = GameObject.FindGameObjectWithTag("StepSpawner").GetComponent<StepSpawnerScript>();
        woodSpawner = GameObject.FindGameObjectWithTag("WoodSpawner").GetComponent<VerticalWoodSpawnerScript>();

        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.SetFloat("totalMoney", 9900);

        score = totalScore;
        character.scoreSign.transform.GetChild(0).GetComponent<TextMeshPro>().text = totalScore + "m";
        totalMoney = PlayerPrefs.GetFloat("totalMoney", 0);
        totalMoneyTxt.text = ((int)totalMoney).ToString();

        currentGameLevel.text = "Level " + PlayerPrefs.GetInt("currentGameLevel", 1);

        isStarted = false;
        gameOverPanel.SetActive(false);
        shopPanel.SetActive(true);
        tapToStartPanel.SetActive(true);

        //### SHOP Levels
        staminaLevel.text = "LVL " + PlayerPrefs.GetInt("staminaLevel", 1).ToString();
        incomeLevel.text = "LVL " + PlayerPrefs.GetInt("incomeLevel", 1).ToString();
        speedLevel.text = "LVL " + PlayerPrefs.GetInt("speedLevel", 1).ToString();
        //### SHOP Amounts
        staminaAmount.text = PlayerPrefs.GetInt("staminaAmount", 50).ToString();
        incomeAmount.text = PlayerPrefs.GetInt("incomeAmount", 50).ToString();
        speedAmount.text = PlayerPrefs.GetInt("speedAmount", 50).ToString();


    }


    void Update()
    {
        float currentLvlProgress = ((totalScore / (totalScore - score)) * 100);
        currentLvlProgress = (1 / currentLvlProgress) * 100;
        progressBar.fillAmount = currentLvlProgress;
    }

    public void decreaseScore()
    {
        if (score > 1)
        {
            score -= .2f;
        }
        else
        {
            isStarted = false;
            score = 0;
            comfettiParticle.Play();
            gameOver();
        }
        scoreTxt.text = string.Format("{0:0.0}", score) + "m";
    }

    public void gameOver()
    {
        savePlayerPrefs();
        gameOverPanel.SetActive(true);
    }
    public IEnumerator tryAgain()
    {
        savePlayerPrefs();
        yield return new WaitForSeconds(1.5f);
        tryAgainPanel.SetActive(true);
    }

    void savePlayerPrefs()
    {
        if (isStarted)
        {
            PlayerPrefs.SetFloat("totalMoney", totalMoney);
            if (score > PlayerPrefs.GetFloat("lastScore", 0))
            {
                PlayerPrefs.SetFloat("lastScore", score);
                PlayerPrefs.SetFloat("lastScorePosition", woodSpawner.lastSpawnedWood.transform.position.y);
            }

            PlayerPrefs.SetInt("currentGameLevel", PlayerPrefs.GetInt("currentGameLevel", 1) + 1);
        }
    }

    public void startGame()
    {
        shopPanel.SetActive(false);
        tapToStartPanel.SetActive(false);
        isStarted = true;
    }

    public void restartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void increaseStamina()
    {
        if (totalMoney >= PlayerPrefs.GetInt("staminaAmount", 50))
        {
            PlayerPrefs.SetInt("staminaLevel", PlayerPrefs.GetInt("staminaLevel", 1) + 1);
            staminaLevel.text = "LVL " + PlayerPrefs.GetInt("staminaLevel", 1).ToString();
            PlayerPrefs.SetInt("totalStamina", PlayerPrefs.GetInt("totalStamina", 100) + PlayerPrefs.GetInt("staminaLevel", 1) * 5);
            character.totalStamina = PlayerPrefs.GetInt("totalStamina", 100);
            character.stamina = character.totalStamina;
            PlayerPrefs.SetFloat("totalMoney", (int)PlayerPrefs.GetFloat("totalMoney") - PlayerPrefs.GetInt("staminaAmount", 50));
            totalMoney = PlayerPrefs.GetFloat("totalMoney", 0);
            totalMoneyTxt.text = ((int)totalMoney).ToString();
            PlayerPrefs.SetInt("staminaAmount", PlayerPrefs.GetInt("staminaAmount", 50) + 50);
            staminaAmount.text = PlayerPrefs.GetInt("staminaAmount", 50).ToString();
        }
    }
    public void increaseIncome()
    {
        if (totalMoney >= PlayerPrefs.GetInt("incomeAmount", 50))
        {
            PlayerPrefs.SetInt("incomeLevel", PlayerPrefs.GetInt("incomeLevel", 1) + 1);
            incomeLevel.text = "LVL " + PlayerPrefs.GetInt("incomeLevel", 1).ToString();
            PlayerPrefs.SetFloat("totalMoney", (int)PlayerPrefs.GetFloat("totalMoney") - PlayerPrefs.GetInt("incomeAmount", 50));
            totalMoney = PlayerPrefs.GetFloat("totalMoney", 0);
            totalMoneyTxt.text = ((int)totalMoney).ToString();
            PlayerPrefs.SetInt("incomeAmount", PlayerPrefs.GetInt("incomeAmount", 50) + 50);
            incomeAmount.text = PlayerPrefs.GetInt("incomeAmount", 50).ToString();
        }
    }
    public void increaseSpeed()
    {
        if (totalMoney >= PlayerPrefs.GetInt("speedAmount", 50))
        {
            PlayerPrefs.SetInt("speedLevel", PlayerPrefs.GetInt("speedLevel", 1) + 1);
            speedLevel.text = "LVL " + PlayerPrefs.GetInt("speedLevel", 1).ToString();
            Time.timeScale = (PlayerPrefs.GetInt("speedLevel") * .2f) + 1;
            PlayerPrefs.SetFloat("totalMoney", (int)PlayerPrefs.GetFloat("totalMoney") - PlayerPrefs.GetInt("speedAmount", 50));
            totalMoney = PlayerPrefs.GetFloat("totalMoney", 0);
            totalMoneyTxt.text = ((int)totalMoney).ToString();
            PlayerPrefs.SetInt("speedAmount", PlayerPrefs.GetInt("speedAmount", 50) + 50);
            speedAmount.text = PlayerPrefs.GetInt("speedAmount", 50).ToString();
        }
    }
}
