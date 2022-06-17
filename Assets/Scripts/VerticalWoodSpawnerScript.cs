using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VerticalWoodSpawnerScript : MonoBehaviour
{
    private StepSpawnerScript stepSpawner;
    private SoundManagerScript soundManagerScript;

    public GameObject woodObject;

    public GameObject backgroundObject;
    public GameObject lastSpawnedBackground;

    public GameObject lastSpawnedWood;
    public GameObject lastScoreWood;
    public GameObject scoreSignWithText;


    void Start()
    {
        stepSpawner = GameObject.FindGameObjectWithTag("StepSpawner").GetComponent<StepSpawnerScript>();
        soundManagerScript = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManagerScript>();

        if (PlayerPrefs.GetFloat("lastScorePosition") > 0.3)
        {
            lastScoreWood.SetActive(true);
            lastScoreWood.transform.position = lastScoreWood.transform.position + new Vector3(0, PlayerPrefs.GetFloat("lastScorePosition"), 0);
            var spawnedScoreSignWithText = Instantiate(scoreSignWithText, new Vector3(lastScoreWood.transform.position.x, PlayerPrefs.GetFloat("lastScorePosition") + 0.3f, lastScoreWood.transform.position.z), Quaternion.identity);
            spawnedScoreSignWithText.transform.GetChild(0).GetComponent<TextMeshPro>().text = string.Format("{0:0.0}", PlayerPrefs.GetFloat("lastScore")) + "m";
        }
    }

    private void FixedUpdate()
    {
        if (stepSpawner.spawnedObj != null)
        {
            transform.position = stepSpawner.spawnedObj.transform.position;
        }
    }

    public void spawnWood()
    {
        soundManagerScript.audioSource.PlayOneShot(soundManagerScript.audios[0]);
        lastSpawnedWood = Instantiate(woodObject, stepSpawner.spawnedObj.transform.position, Quaternion.identity);
    }

    public void spawnBackground()
    {
        if (Vector3.Distance(lastSpawnedWood.transform.position, lastSpawnedBackground.transform.position) < 3)
        {
            lastSpawnedBackground = Instantiate(backgroundObject, lastSpawnedBackground.transform.position + new Vector3(0, 5.322f, 0), Quaternion.identity);
            lastSpawnedBackground = Instantiate(backgroundObject, lastSpawnedBackground.transform.position + new Vector3(0, 5.322f, 0), Quaternion.identity);
            lastSpawnedBackground = Instantiate(backgroundObject, lastSpawnedBackground.transform.position + new Vector3(0, 5.322f, 0), Quaternion.identity);
        }
    }
}
