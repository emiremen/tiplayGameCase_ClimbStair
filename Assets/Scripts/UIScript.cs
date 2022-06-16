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

    public float timeScale;


    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterScript>();
        stepSpawner = GameObject.FindGameObjectWithTag("StepSpawner").GetComponent<StepSpawnerScript>();
        woodSpawner = GameObject.FindGameObjectWithTag("WoodSpawner").GetComponent<VerticalWoodSpawnerScript>();

        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("totalMoney", 9999);

        score = totalScore;
        character.scoreSign.transform.GetChild(0).GetComponent<TextMeshPro>().text = totalScore + "m";
        totalMoney = PlayerPrefs.GetFloat("totalMoney", 0);
        totalMoneyTxt.text = ((int)totalMoney).ToString();
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
        timeScale = Time.timeScale;


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
            gameOverPanel.SetActive(true);
            comfettiParticle.Play();
        }
        scoreTxt.text = string.Format("{0:0.0}", score) + "m";
    }

    public void gameOver()
    {
        PlayerPrefs.SetFloat("totalMoney", totalMoney);
        if (score > PlayerPrefs.GetFloat("lastScore", 0))
        {
            PlayerPrefs.SetFloat("lastScore", score);
            PlayerPrefs.SetFloat("lastScorePosition", woodSpawner.lastSpawnedWood.transform.position.y);
        }


        tryAgainPanel.SetActive(true);
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

    //public void gainMoney(float yPos)
    //{
    //    totalMoney += 0.5f;
    //    totalMoneyTxt.text = string.Format("{0:0}", totalMoney);
    //    Destroy(Instantiate(gainMoneyTextObj, new Vector3(stepSpawner.spawnedObj.transform.position.x, yPos + .13f, gainMoneyTextObj.transform.position.z), gainMoneyTextObj.transform.rotation), 1);
    //}

    public void increaseStamina()
    {
        if (totalMoney >= PlayerPrefs.GetInt("staminaAmount"))
        {
            PlayerPrefs.SetInt("staminaLevel", PlayerPrefs.GetInt("staminaLevel", 1) + 1);
            PlayerPrefs.SetInt("staminaAmount", PlayerPrefs.GetInt("staminaAmount", 50) + 50);
            staminaLevel.text = "LVL " + PlayerPrefs.GetInt("staminaLevel", 1).ToString();
            staminaAmount.text = PlayerPrefs.GetInt("staminaAmount", 100).ToString();
            character.totalStamina += (PlayerPrefs.GetInt("staminaLevel", 1) * 5);
            character.stamina += (PlayerPrefs.GetInt("staminaLevel", 1) * 5);
        }
    }
    public void increaseIncome()
    {
        if (totalMoney >= PlayerPrefs.GetInt("incomeAmount"))
        {
            PlayerPrefs.SetInt("incomeLevel", PlayerPrefs.GetInt("incomeLevel", 1) + 1);
            PlayerPrefs.SetInt("incomeAmount", PlayerPrefs.GetInt("incomeAmount", 50) + 50);
            incomeLevel.text = "LVL " + PlayerPrefs.GetInt("incomeLevel", 1).ToString();
            incomeAmount.text = PlayerPrefs.GetInt("incomeAmount", 50).ToString();
        }
    }
    public void increaseSpeed()
    {
        if (totalMoney >= PlayerPrefs.GetInt("speedAmount"))
        {
            PlayerPrefs.SetInt("speedLevel", PlayerPrefs.GetInt("speedLevel") + 1);
            PlayerPrefs.SetInt("speedAmount", PlayerPrefs.GetInt("speedAmount", 50) + 50);
            speedLevel.text = "LVL " + PlayerPrefs.GetInt("speedLevel", 1).ToString();
            speedAmount.text = PlayerPrefs.GetInt("speedAmount", 50).ToString();
            Time.timeScale = (PlayerPrefs.GetInt("speedLevel") * .2f) + 1;

        }
    }
}
