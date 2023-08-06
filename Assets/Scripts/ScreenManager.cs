using Oculus.Platform.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * This class manages what image is displayed on the TV screen in front of the user.
 */
public class ScreenManager : MonoBehaviour
{
    public Text screenText;
    // 0: croissants; 1: choco dou; 2: white dou; 3: pink dou; 4: cherry cup; 5: choco cup
    public GameObject[] imagesList;
    private GameManager gameManager;
    private new AudioSource audio;
    public AudioClip[] goodAudioClips;
    public AudioClip wrongAudioClip;

    public ParticleSystem confettiFx1;
    public ParticleSystem confettiFx2;

    private int currentImage;

    private static int minImages;
    private static int maxImages;
    private static float MAX_WAIT_TIME = 1f;

    private float t;
    private float t0;
    private bool setTimer;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audio = GetComponent<AudioSource>();

        minImages = 0;
        maxImages = imagesList.Length;

        t = MAX_WAIT_TIME;
        t0 = 0f;
        setTimer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GetOrderMatch())
        {
            ManageEffects(true);
            RemoveOrder();
        }

        if (gameManager.GetWrongMatch())
        {
            gameManager.SetWrongMatch(false);
            ManageEffects(false);
        }

        if (gameManager.GetSystemState() == GameManager.StateMachine.playGame)
        {
            if (setTimer)
            {
                t -= Time.deltaTime;

                if (t <= 0)
                {
                    setTimer = false;
                    t = MAX_WAIT_TIME;
                    
                    SetNewOrder();
                }
            }
        }

        if (gameManager.GetSystemState() == GameManager.StateMachine.gameEnd)
        {
            t0 += Time.deltaTime;

            if (t0 >= 2.5f)
                SetCheeringEffect();
        }
    }

    private void ManageEffects(bool guessRight)
    {
        if (guessRight)
        {
            screenText.text = "Thank you!";

            int rnd = UnityEngine.Random.Range(0, goodAudioClips.Length);

            audio.clip = goodAudioClips[rnd];
            audio.Play();
        }
        else
        {
            audio.clip = wrongAudioClip;
            audio.Play();
            gameManager.SetJustPicked(false);
        }
    }

    public void RemoveOrder()
    {
        if (imagesList[currentImage].GetComponent<Image>().isActiveAndEnabled)
            imagesList[currentImage].GetComponent<Image>().enabled = false;

        gameManager.SetOrderMatch(false);
        setTimer = true;

        gameManager.SetCurrentOrder("");
    }

    private void SetNewOrder ()
    {
        int num = new System.Random().Next(minImages, maxImages);

        if (gameManager.GetLevel().CheckAvailability(num))
        {
            currentImage = num;
            imagesList[currentImage].GetComponent<Image>().enabled = true;
            screenText.text = "New order!";

            gameManager.SetJustPicked(false);
            gameManager.SetCurrentOrder(imagesList[currentImage].name);
        }
        else
            SetNewOrder();
    }

    private void SetCheeringEffect()
    {
        confettiFx1.Play();
        confettiFx2.Play();
        
        t0 = 0f;
    }
}
