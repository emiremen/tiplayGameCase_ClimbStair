using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StepSpawnerScript : MonoBehaviour
{
    public GameObject stepObject;

    private CharacterScript character;
    private UIScript uIScript;

    [HideInInspector]
    public GameObject spawnedObj = null, spawnedObj2 = null;

    [Range(0f, 0.1f)]
    public float stepHeight;
    [Range(0, 40)]
    public float stepRotation;
    float currentHeight = 0, currentRotation = 0;

    public float moveSpeed;
    public bool isSpawned;
    public Vector3 newPosForSpawn;




    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterScript>();
        uIScript = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIScript>();
    }

    private void Update()
    {
        if (spawnedObj2 != null)
        {
            character.transform.position = Vector3.Lerp(
                    character.transform.position,
                    spawnedObj2.transform.GetChild(0).GetChild(0).position + new Vector3(0, 0, 0),
                    Time.deltaTime * moveSpeed);
        }

    }

    public void spawnStep()
    {
        if (spawnedObj2 != null)
            spawnedObj2 = spawnedObj;

        spawnedObj = Instantiate(stepObject, new Vector3(0, currentHeight, 0), Quaternion.Euler(0, currentRotation, 0));


        spawnedObj.transform.parent = transform;
        currentHeight += stepHeight;
        currentRotation += stepRotation;
        uIScript.decreaseScore();
        gainMoney(spawnedObj.transform.position.y);
        character.sweatingCharacter();
        Invoke("step2", character.spawnSpeed);
    }
    void step2()
    {
        if (uIScript.isStarted)
        {
            spawnedObj2 = spawnedObj;
            isSpawned = true;
            spawnedObj = Instantiate(stepObject, new Vector3(0, currentHeight, 0), Quaternion.Euler(0, currentRotation, 0));
            spawnedObj.transform.parent = transform;
            currentHeight += stepHeight;
            currentRotation += stepRotation;
            uIScript.decreaseScore();
            gainMoney(spawnedObj.transform.position.y);

            character.sweatingCharacter();
        }
    }

    public void gainMoney(float yPos)
    {
        uIScript.totalMoney += PlayerPrefs.GetInt("incomeLevel", 1) * 0.5f;
        uIScript.totalMoneyTxt.text = ((int)uIScript.totalMoney).ToString();
        var gainedMoney = Instantiate(uIScript.gainMoneyTextObj, new Vector3(character.transform.position.x, yPos + .13f, uIScript.gainMoneyTextObj.transform.position.z), uIScript.gainMoneyTextObj.transform.rotation);
        gainedMoney.transform.GetComponentInChildren<TextMeshPro>().text = "$" + (PlayerPrefs.GetInt("incomeLevel", 1) * 0.5f).ToString();
        Destroy(gainedMoney, 1);
    }
}
