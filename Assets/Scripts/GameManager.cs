using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class is the manager of all.
 */
public class GameManager : MonoBehaviour
{
    #region Constant variables
    public const int COUNTER_INSTRUCTIONS = -1;
    public const int COUNTER_MENU = 0;
    public const int COUNTER_L1 = 1;
    public const int COUNTER_L2 = 2;
    public const int COUNTER_L3 = 3;
    public const int COUNTER_PAUSE = 4;
    public const int COUNTER_END_GAME = 5;
    public const int COUNTER_CREDITS = 6;

    private const int L1_GAME_SCORE = 5;
    private const int L2_GAME_SCORE = 10;
    private const int L3_GAME_SCORE = 15;

    private const int L1_GAME_MIN = 1;
    private const int L2_GAME_MIN = 2;
    private const int L3_GAME_MIN = 5;
    #endregion

    #region Variables
    public enum StateMachine
    {
        startMenu,        // 0
        instructionsMenu, // 1
        playGame,         // 2
        gamePause,        // 3
        gameEnd,          // 4
        endMenu,          // 5
        creditsMenu       // 6
    }
    public StateMachine systemState;

    public GameObject menuCounter;
    public GameObject level1Counter;
    public GameObject level2Counter;
    public GameObject level3Counter;

    private List<GameObject> pickedFoods;

    public GameObject menuScreen;
    public GameObject instructionsScreen;
    public GameObject creditsScreen;
    public GameObject ordersScreen;
    public GameObject pauseScreen;
    public GameObject endGameScreen;

    private int currentButton;
    private Level level;

    private int currentPicked;
    private int foodsToPick;
    private int gameScore;
    private int bestScore;
    
    private int gameMins;
    private int currentSecs;
    private int currentMins;
    private int secsSpent;
    private int minsSpent;
    private int minButton;
    private int maxButton;
    
    private float t;
    private float t0;

    private string currentOrder;
    private string orderMatchName;
    private string wrongMatchName;

    private bool orderMatch;
    private bool wrongMatch;
    private bool justPicked;
    private bool animateButton;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        systemState = StateMachine.instructionsMenu;
        SetInstructions();

        pickedFoods = new List<GameObject>();

        currentButton = 0;
        minButton = 0;
        maxButton = 0;
        bestScore = 0;

        ResetVariables();
    }

    // Update is called once per frame
    void Update()
    {
        if (systemState == StateMachine.playGame)
            GameTimer();

        if (systemState == StateMachine.gameEnd)
        {
            t0 += Time.deltaTime;

            if (t0 >= 3)
                SetEndGame();
        }
    }

    public StateMachine GetSystemState()
    {
        return systemState;
    }

    #region Buttons in menus.
    public int GetCurrentButton()
    {
        return currentButton;
    }

    public void SetCurrentButtons(int current, int min, int max)
    {
        currentButton = current;
        minButton = min;
        maxButton = max;
    }

    public bool GetAnimateButton()
    {
        return animateButton;
    }

    public void SetAnimateButton(bool animate)
    {
        animateButton = animate;
    }

    /// <summary>
    /// Function that sets the direction where the menu button should be moving to.
    /// </summary>
    /// <param name="direction">Can be: 0-left; 1-right.</param>
    public void SetMoveDirection(string direction)
    {
        if (direction.Equals("right") && currentButton < maxButton)
            currentButton += 1;
        else if (direction.Equals("left") && currentButton > minButton)
            currentButton -= 1;

        Debug.Log("GAME MANAGER: Direction change! Current button = " + currentButton);
    }
    #endregion

    #region Levels.
    public void SelectLevel()
    {
        switch(systemState)
        {
            case StateMachine.instructionsMenu:
                SetLevelMenu();
                break;
            case StateMachine.creditsMenu:
                SetLevelMenu();
                break;
            case StateMachine.startMenu:
                if (currentButton >= 1 && currentButton <= 3)
                    SetLevelCounter(currentButton);
                else if (currentButton == 4)
                    SetInstructions();
                else if (currentButton == 6)
                    SetCredits();
                else
                    Application.Quit();
                break;
            case StateMachine.playGame:
                SetGamePause();
                break;
            case StateMachine.gamePause:
                if (currentButton == minButton) ResumeGame();
                else if (currentButton == maxButton) SetLevelMenu(); 
                break;
            case StateMachine.endMenu:
                if (currentButton == minButton) SetLevelMenu();
                else if (currentButton == maxButton) Application.Quit();
                break;
        }
    }

    /// <summary>
    /// Activates or deactivates objects in scene according to level selected in menu.
    /// </summary>
    /// <param name="counterTop">Counter top value. Can be 0-menu; 1-level 1; 2-level 2; 3-level 3; or 4-end game.</param>
    private void SetLevelCounter(int counterTop)
    {
        switch(counterTop)
        {
            case COUNTER_L1:
                SetLevel1();
                break;
            case COUNTER_L2:
                SetLevel2();
                break;
            case COUNTER_L3:
                SetLevel3();
                break;
        }
    }

    private void SetInstructions()
    {
        menuCounter.SetActive(true);
        level1Counter.SetActive(false);
        level2Counter.SetActive(false);
        level3Counter.SetActive(false);

        instructionsScreen.SetActive(true);
        creditsScreen.SetActive(false);
        menuScreen.SetActive(false);
        ordersScreen.SetActive(false);
        pauseScreen.SetActive(false);
        endGameScreen.SetActive(false);

        systemState = StateMachine.instructionsMenu;
    }

    private void SetCredits()
    {
        menuCounter.SetActive(true);
        level1Counter.SetActive(false);
        level2Counter.SetActive(false);
        level3Counter.SetActive(false);

        instructionsScreen.SetActive(false);
        creditsScreen.SetActive(true);
        menuScreen.SetActive(false);
        ordersScreen.SetActive(false);
        pauseScreen.SetActive(false);
        endGameScreen.SetActive(false);

        systemState = StateMachine.creditsMenu;
    }

    private void SetLevelMenu()
    {
        menuCounter.SetActive(true);
        level1Counter.SetActive(false);
        level2Counter.SetActive(false);
        level3Counter.SetActive(false);

        instructionsScreen.SetActive(false);
        creditsScreen.SetActive(false);
        menuScreen.SetActive(true);
        ordersScreen.SetActive(false);
        pauseScreen.SetActive(false);
        endGameScreen.SetActive(false);

        systemState = StateMachine.startMenu;

        ResetVariables();
    }

    private void SetLevel1()
    {
        menuCounter.SetActive(false);
        level1Counter.SetActive(true);
        level2Counter.SetActive(false);
        level3Counter.SetActive(false);

        instructionsScreen.SetActive(false);
        creditsScreen.SetActive(false);
        menuScreen.SetActive(false);
        if (ordersScreen != null)
            ordersScreen.SetActive(true);
        pauseScreen.SetActive(false);
        endGameScreen.SetActive(false);

        level = new Level(1);

        foodsToPick = L1_GAME_SCORE;
        gameMins = L1_GAME_MIN;
        currentMins = L1_GAME_MIN;

        systemState = StateMachine.playGame;
    }

    private void SetLevel2()
    {
        menuCounter.SetActive(false);
        level1Counter.SetActive(false);
        level2Counter.SetActive(true);
        level3Counter.SetActive(false);

        instructionsScreen.SetActive(false);
        creditsScreen.SetActive(false);
        menuScreen.SetActive(false);
        ordersScreen.SetActive(true);
        pauseScreen.SetActive(false);
        endGameScreen.SetActive(false);

        level = new Level(2);

        foodsToPick = L2_GAME_SCORE;
        gameMins = L2_GAME_MIN;
        currentMins = L2_GAME_MIN;

        systemState = StateMachine.playGame;
    }

    private void SetLevel3()
    {       
        menuCounter.SetActive(false);
        level1Counter.SetActive(false);
        level2Counter.SetActive(false);
        level3Counter.SetActive(true);

        instructionsScreen.SetActive(false);
        creditsScreen.SetActive(false);
        menuScreen.SetActive(false);
        ordersScreen.SetActive(true);
        pauseScreen.SetActive(false);
        endGameScreen.SetActive(false);

        level = new Level(3);

        foodsToPick = L3_GAME_SCORE;
        gameMins = L3_GAME_MIN;
        currentMins = L3_GAME_MIN;

        systemState = StateMachine.playGame;
    }

    private void SetGamePause()
    {
        ordersScreen.GetComponentInChildren<ScreenManager>().RemoveOrder();

        menuCounter.SetActive(true);
        level1Counter.SetActive(false);
        level2Counter.SetActive(false);
        level3Counter.SetActive(false);

        instructionsScreen.SetActive(false);
        creditsScreen.SetActive(false);
        menuScreen.SetActive(false);
        ordersScreen.SetActive(false);
        pauseScreen.SetActive(true);
        endGameScreen.SetActive(false);

        systemState = StateMachine.gamePause;
    }

    private void ResumeGame()
    {
        menuCounter.SetActive(false);
        switch (level.GetGameLevel())
        {
            case 1:
                level1Counter.SetActive(true);
                break;
            case 2:
                level2Counter.SetActive(true);
                break;
            case 3:
                level3Counter.SetActive(true);
                break;
        }
        pauseScreen.SetActive(false);
        ordersScreen.SetActive(true);

        systemState = StateMachine.playGame;
    }

    private void SetLevelEndGame()
    {
        menuCounter.SetActive(true);
        level1Counter.SetActive(false);
        level2Counter.SetActive(false);
        level3Counter.SetActive(false);

        instructionsScreen.SetActive(false);
        creditsScreen.SetActive(false);
        menuScreen.SetActive(false);
        ordersScreen.SetActive(false);
        pauseScreen.SetActive(false);
        endGameScreen.SetActive(true);

        systemState = StateMachine.endMenu;
    }

    public void AddPickedFood(GameObject food)
    {
        pickedFoods.Add(food);
    }

    private void EnablePickedFoods()
    {
        foreach (GameObject food in pickedFoods)
            food.SetActive(true);

        pickedFoods.Clear();
    }

    public Level GetLevel()
    {
        return level;
    }
    #endregion

    #region Timer.
    /**
     * Timer of the game. When level starts, timer starts.
     */
    private void GameTimer() {
        if (currentSecs <= 0 && currentMins <= 0)
            systemState = StateMachine.gameEnd;
        
        if (systemState == StateMachine.playGame)
        {
            t += Time.deltaTime;

            if (currentSecs <= 0)
            {
                currentMins -= 1;
                currentSecs = 59;
            }
        
            if (t >= 1)
            {
                t = 0f;
                currentSecs -= 1;
            }
        }
    }

    public int GetCurrentMinutes()
    {
        return currentMins;
    }

    public int GetCurrentSeconds()
    {
        return currentSecs;
    }

    public int GetSecsSpent()
    {
        return secsSpent;
    }

    public int GetMinsSpent()
    {
        return minsSpent;
    }

    private void ComputeTimeSpent()
    {
        int min, sec = 60;

        if (currentSecs == 0 && currentMins == 0)
        {
            secsSpent = 0;
            minsSpent = gameMins;
        }
        else if (currentSecs == 0 && currentMins > 0)
        {
            secsSpent = 0;
            minsSpent = gameMins - currentMins;
        }
        else
        {
            min = gameMins - 1;

            secsSpent = sec - currentSecs;
            minsSpent = min - currentMins;
        }
    }
    #endregion

    #region Orders.
    public void SetCurrentOrder (string foodName)
    {
        currentOrder = foodName;
    }

    public string GetCurrentOrder()
    {
        return currentOrder;
    }

    public bool GetOrderMatch()
    {
        return orderMatch;
    }

    public void SetOrderMatch(bool match)
    {
        orderMatch = match;
    }

    public bool GetWrongMatch()
    {
        return wrongMatch;
    }

    public void SetWrongMatch(bool match)
    {
        wrongMatch = match;
    }

    public string GetOrderMatchName()
    {
        return orderMatchName;
    }

    private void SetOrderMatchName(string name)
    {
        orderMatchName = name;
    }

    public string GetWrongMatchName()
    {
        return wrongMatchName;
    }

    private void SetWrongMatchName(string name)
    {
        wrongMatchName = name;
    }
    #endregion

    public void TriggeredFood (string foodName)
    {
        if (!justPicked)
        {
            justPicked = true;

            if (foodName.Contains(currentOrder))
            {
                SetOrderMatch(true);
                SetOrderMatchName(foodName);
                string food = foodName.Substring(0, foodName.Length - 1);

                level.RemoveFood(food);
                SetPickedFoods();
            }
            else
            {
                SetWrongMatch(true);
                SetWrongMatchName(foodName);
            }
        }
    }

    #region Picked foods and game score.
    public void SetJustPicked(bool picked)
    {
        justPicked = picked;
    }

    public int GetCurrentPicked()
    {
        return currentPicked;
    }

    public void SetPickedFoods()
    {
        if (currentPicked < foodsToPick)
            currentPicked += 1;

        if (currentPicked == foodsToPick)
            systemState = StateMachine.gameEnd;
    }

    public int GetPickedFoods()
    {
        return foodsToPick;
    }

    private void SetScore()
    {
        float totalSpentMins = minsSpent + (secsSpent / 60f);
        gameScore = (int) (currentPicked/totalSpentMins) * 100;
    }

    public int GetScore()
    {
        return gameScore;
    }

    private void SetBestScore()
    {
        if (gameScore > bestScore)
            bestScore = gameScore;
    }

    public int GetBestScore()
    {
        return bestScore;
    }
    #endregion

    private void SetEndGame()
    {
        ComputeTimeSpent();
        SetScore();
        SetBestScore();
        
        SetLevelEndGame();
    }

    private void ResetVariables()
    {
        EnablePickedFoods();

        currentPicked = 0;
        foodsToPick = 0;
        gameScore = 0;

        gameMins = 0;
        currentSecs = 0;
        currentMins = 0;
        secsSpent = 0;
        minsSpent = 0;

        t = 0f;
        t0 = 0f;
        
        currentOrder = "";
        orderMatchName = "";
        wrongMatchName = "";

        orderMatch = false;
        wrongMatch = false;
        justPicked = false;
        animateButton = false;
    }
}
