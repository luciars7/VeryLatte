using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class manages foods independently. It is in charge of
 *  - Activating a "light" animation when a food has to be picked up.
 *  - Activating an animation when the correct food is picked up.
 *  - Activating a blinking animation when the wrong food is selected.
 */
public class FoodManager : MonoBehaviour
{
    private GameManager gameManager;
    
    public Material originalMaterial;
    public Material highlightMaterial;

    private bool isHighlight;
    private bool isPicked;
    private float t;
    private string thisFoodName;

    private Animator animator;
    public ParticleSystem disappearEffect;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        gameObject.GetComponent<Renderer>().material = originalMaterial;

        isHighlight = false;
        isPicked = false;
        t = 0f;
        thisFoodName = gameObject.name;

        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        ResetVariables();
    }

    private void ResetVariables()
    {
        isHighlight = false;
        isPicked = false;
        t = 0f;
        thisFoodName = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        // Color blinking
        t += Time.deltaTime;

        if (t > 0.5f && thisFoodName.Contains(gameManager.GetCurrentOrder()) && !gameManager.GetCurrentOrder().Equals(""))
            ChangeMaterial();

        // Remove food if correct
        if (gameManager.GetOrderMatch())
        {
            isHighlight = true;
            ChangeMaterial();

            if (thisFoodName.Equals(gameManager.GetOrderMatchName()))
                Animate(true);
        }

        if (gameManager.GetWrongMatch() && thisFoodName.Equals(gameManager.GetWrongMatchName()))
            Animate(false);

        if (isPicked && disappearEffect.isStopped)
            TurnObjectOff();
    }

    private void ChangeMaterial()
    {
        t = 0f;

        if (isHighlight)
        {
            isHighlight = false;
            gameObject.GetComponent<Renderer>().material = originalMaterial;
        }
        else
        {
            isHighlight = true;
            gameObject.GetComponent<Renderer>().material = highlightMaterial;
        }   
    }

    private void Animate(bool isGood)
    {
        if (isGood)
        {
            animator.SetTrigger("goodSelection");
            disappearEffect.Play();
            isPicked = true;
        }
        else
            animator.SetTrigger("wrongSelection");
    }

    private void TurnObjectOff()
    {
        isPicked = false;
        gameObject.SetActive(false);
        gameManager.AddPickedFood(gameObject);
    }
}
