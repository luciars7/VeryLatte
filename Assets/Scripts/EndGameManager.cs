using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    public Text objectsPickedText;
    public Text timeText;
    public Text scoreText;
    public Text bestScoreText;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        ResetVariables();
    }

    private void ResetVariables()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        SetEndMenuTexts();
    }

    private void OnEnable()
    {
        ResetVariables();
    }

    private void SetEndMenuTexts()
    {
        objectsPickedText.text = gameManager.GetCurrentPicked().ToString();
        SetTimeText(gameManager.GetMinsSpent(), gameManager.GetSecsSpent());
        scoreText.text = gameManager.GetScore().ToString();
        bestScoreText.text = gameManager.GetBestScore().ToString();
    }

    private void SetTimeText(int mins, int secs)
    {
        string minsString, secsString;

        if (mins < 10) minsString = "0" + mins.ToString();
        else minsString = mins.ToString();

        if (secs < 10) secsString = "0" + secs.ToString();
        else secsString = secs.ToString();

        timeText.text = minsString + ":" + secsString;
    }
}
