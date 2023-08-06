using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsManager : MonoBehaviour
{
    /// <summary>
	/// Instructions menu:   0;
    ///     - Back.
    /// Main menu:           1-6;
    ///     - Level 1,
    ///     - Level 2,
    ///     - Level 3,
    ///     - Instructions,
    ///     - Exit,
    ///     - Credits.
    /// Pause menu:          7-8;
    ///     - Cancel,
    ///     - OK.
    /// End game menu:       9-10.
    ///     - Menu,
    ///     - Exit.
    /// Credits menu:
    ///     - Back.          11.
	/// </summary>
    public Image[] buttonBackgrounds;

    /// <summary>
	/// Same order as "button backgrounds".
	/// </summary>
    public Renderer[] buttonsMaterial;

    public Material unselectedButton;
    public Material selectedButton;

    private Color unselectedBackButton;
    private Color selectedBackButton;

    private int currentMinButton;
    private int currentMaxButton;
    private int currentButton;

    private GameManager gameManager;
    private GameManager.StateMachine gameState;

    private bool isAnimated;

    private GameObject buttonObject;

    private float t;

    // Start is called before the first frame update
    void Start()
    {
        unselectedBackButton = new Color(0.9716981f, 0.7553577f, 0.8620045f);
        selectedBackButton = new Color(0.5660378f, 0f, 0.2830189f);

        currentMinButton = 0;
        currentMaxButton = 0;
        currentButton = 0;

        isAnimated = false;

        t = 0f;

        gameManager = GetComponent<GameManager>();
        
        gameState = gameManager.GetSystemState();
        SetCurrentState();
        
        ChangeButtonState();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState != gameManager.GetSystemState())
            SetCurrentState();

        if (currentButton != gameManager.GetCurrentButton())
            ChangeButtonState();

        if (gameManager.GetAnimateButton() && !isAnimated)
            AnimateButton();

        if (isAnimated)
        {
            t += Time.deltaTime;

            if (t > 0.5f)
                EndAnimation();
        }
    }

    private void SetCurrentState()
    {
        gameState = gameManager.GetSystemState();

        if (gameState == GameManager.StateMachine.instructionsMenu)
        {
            currentMinButton = 0;
            currentMaxButton = 0;
        }
        else if (gameState == GameManager.StateMachine.startMenu)
        {
            currentMinButton = 1;
            currentMaxButton = 6;
        }
        else if (gameState == GameManager.StateMachine.gamePause)
        {
            currentMinButton = 7;
            currentMaxButton = 8;
        }
        else if (gameState == GameManager.StateMachine.endMenu)
        {
            currentMinButton = 9;
            currentMaxButton = 10;
        }
        else if (gameState == GameManager.StateMachine.creditsMenu)
        {
            currentMinButton = 11;
            currentMaxButton = 11;
        }

        gameManager.SetCurrentButtons(currentMinButton, currentMinButton, currentMaxButton);

        Debug.Log("BUTTONS MANAGER: Currently in menu with total buttons of = " + (currentMaxButton - currentMinButton + 1) + "(gameState=" + gameState + ")");
    }

    private void ChangeButtonState()
    {
        currentButton = gameManager.GetCurrentButton();

        Debug.Log("BUTTONS MANAGER: Current button = " + currentButton);

        for (int i = currentMinButton; i < currentMaxButton+1; i++)
        {
            if (i == currentButton)
            {
                buttonBackgrounds[i].color = new Color(selectedBackButton.r, selectedBackButton.g, selectedBackButton.b);
                buttonsMaterial[i].material.color = selectedButton.color;
            }
            else
            {
                buttonBackgrounds[i].color = new Color(unselectedBackButton.r, unselectedBackButton.g, unselectedBackButton.b);
                buttonsMaterial[i].material.color = unselectedButton.color;
            }
        }
    }

    private void AnimateButton()
    {
        isAnimated = true;

        buttonObject = buttonsMaterial[currentButton].gameObject;
        buttonObject.GetComponent<Animator>().SetTrigger("click");
        buttonObject.GetComponent<AudioSource>().Play();
    }

    private void EndAnimation()
    {
        isAnimated = false;
        t = 0f;

        gameManager.SetAnimateButton(false);
        gameManager.SelectLevel();
    }
}