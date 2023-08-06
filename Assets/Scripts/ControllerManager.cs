using UnityEngine;

/**
 * This script is going to manage controller interactions, such as picking up objects and selecting different options from a menu.
 */
public class ControllerManager : MonoBehaviour
{
    private string controllerName;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        controllerName = gameObject.name;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.GetSystemState() != GameManager.StateMachine.playGame && gameManager.GetSystemState() != GameManager.StateMachine.gameEnd)
            ManageControllers(false);
        else if (gameManager.GetSystemState() == GameManager.StateMachine.playGame)
            ManageControllers(true);
    }

    private void ManageControllers(bool pause)
    {
        if (controllerName.Contains("Right"))
        {
            if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickRight))
                gameManager.SetMoveDirection("right");

            if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickLeft))
                    gameManager.SetMoveDirection("left");

            if (!pause)
            {
                if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
                    gameManager.SetAnimateButton(true);
            }
            else
            {
                if (OVRInput.GetDown(OVRInput.RawButton.A))
                    gameManager.SelectLevel();
            }
        }

        if (controllerName.Contains("Left"))
        {
            if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickRight))
                gameManager.SetMoveDirection("right");

            if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickLeft))
                    gameManager.SetMoveDirection("left");

            if (!pause)
            {
                if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
                    gameManager.SetAnimateButton(true);
            }
            else
            {
                if (OVRInput.GetDown(OVRInput.RawButton.X))
                    gameManager.SelectLevel();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("food"))
        {
            if (gameManager.GetSystemState() != GameManager.StateMachine.gameEnd)
                gameManager.TriggeredFood(other.name);
        }
    }
}
