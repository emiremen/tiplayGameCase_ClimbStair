using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    public float spawnSpeed = 0.17f;
    public bool isRunning = false;


    private StepSpawnerScript stepSpawner;
    private VerticalWoodSpawnerScript woodSpawner;
    private UIScript uIScript;

    float stepSpawnWaiting = 0f;

    public GameObject scoreSign;

    public float stamina;
    public float totalStamina;
    public ParticleSystem sweatingParticle;

    public Animation anim;
    public AnimationClip breathingClip;

    void Start()
    {
        isRunning = false;
        stepSpawner = GameObject.FindGameObjectWithTag("StepSpawner").GetComponent<StepSpawnerScript>();
        woodSpawner = GameObject.FindGameObjectWithTag("WoodSpawner").GetComponent<VerticalWoodSpawnerScript>();
        uIScript = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIScript>();
        totalStamina += (PlayerPrefs.GetInt("staminaLevel", 1) * 5);
        stamina = totalStamina;

        Time.timeScale = 1;

    }

    void FixedUpdate()
    {
        //transform.LookAt(new Vector3(stepSpawner.spawnedObj.transform.GetChild(0).GetChild(0).transform.position.x,
        //        transform.position.y, stepSpawner.spawnedObj.transform.GetChild(0).GetChild(0).transform.position.z));
        //transform.position = Vector3.Lerp(transform.position, stepSpawner.spawnedObj.transform.position, 0.001f);

        transform.rotation = Quaternion.LookRotation(stepSpawner.spawnedObj ? -stepSpawner.spawnedObj.transform.GetChild(0).GetChild(0).right : Vector3.right);

        if (Input.GetKey(KeyCode.Mouse0) && uIScript.isStarted)
        {
            run();


            stepSpawnWaiting -= Time.deltaTime;
            if (stepSpawnWaiting <= 0)
            {
                stepSpawner.spawnStep();
                woodSpawner.spawnWood();
                woodSpawner.spawnBackground();
                stepSpawnWaiting = spawnSpeed;
            }
        }
        else
        {
            isRunning = false;
            GetComponent<Animator>().SetBool("isRunning", false);
            stepSpawnWaiting = 0f;
            if (stamina <= totalStamina)
            {
                stamina += Time.deltaTime * 5;

                if (transform.GetChild(1).GetComponent<Renderer>().material.mainTextureScale.y >= 0.2f && stamina >= (totalStamina * 30) / 100)
                {
                    anim.Stop();
                    var tilingY = transform.GetChild(1).GetComponent<Renderer>().material.mainTextureScale.y;
                    tilingY -= 0.0005f;
                    transform.GetChild(1).GetComponent<Renderer>().material.mainTextureScale = new Vector2(-2, tilingY);
                    if (stamina >= (totalStamina * 40) / 100)
                    {
                        sweatingParticle.Stop();
                    }
                }
            }

        }
        if (stamina <= 0)
        {
            uIScript.tryAgain();

            this.gameObject.SetActive(false);
            GameObject[] woods = GameObject.FindGameObjectsWithTag("Step");
            foreach (GameObject wood in woods)
            {
                wood.AddComponent<Rigidbody>();
                wood.GetComponent<Rigidbody>().AddForce(Vector3.up * 3, ForceMode.Impulse);
            }
        }
        if (stepSpawner.spawnedObj)
        {
            scoreSign.transform.position = Vector3.MoveTowards(scoreSign.transform.position, new Vector3(0, woodSpawner.lastSpawnedWood.transform.position.y + 0.23f, 0), Time.deltaTime * .5f);
        }
    }

    private void Update()
    {


    }

    public void run()
    {
        GetComponent<Animator>().SetBool("isRunning", true);
        isRunning = true;
    }

    public void sweatingCharacter()
    {
        stamina--;
        if (stamina < (totalStamina * 40) / 100)
        {
            sweatingParticle.Play();
            if (transform.GetChild(1).GetComponent<Renderer>().material.mainTextureScale.y <= 0.29f && stamina < (totalStamina * 30) / 100)
            {
                anim.Play();
                StartCoroutine(changeColorToRed());
            }
        }
    }

    IEnumerator changeColorToRed()
    {
        while (transform.GetChild(1).GetComponent<Renderer>().material.mainTextureScale.y <= 0.29f)
        {
            yield return new WaitForSeconds(0.08f);
            var tilingY = transform.GetChild(1).GetComponent<Renderer>().material.mainTextureScale.y;
            tilingY += 0.001f;
            transform.GetChild(1).GetComponent<Renderer>().material.mainTextureScale = new Vector2(-2, tilingY);
        }
    }

}
