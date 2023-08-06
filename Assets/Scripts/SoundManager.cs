using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private GameManager gameManager;

    public AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GetSystemState() == GameManager.StateMachine.gameEnd)
            SetCheeringSound();
    }

    private void SetCheeringSound()
    {
        audio.Play();
    }
}
