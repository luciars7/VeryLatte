using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * This class manages the HUD. It changes texts when needed (such as timer and points).
 */
public class HUDManager : MonoBehaviour
{
    public Text timeText;
    public Text scoreText;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        SetTimeText(gameManager.GetCurrentMinutes(), gameManager.GetCurrentSeconds());
        SetScoreText(gameManager.GetCurrentPicked());
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

    private void SetScoreText(int score)
    {
        scoreText.text = score.ToString() + "/" + gameManager.GetPickedFoods();
    }
}
