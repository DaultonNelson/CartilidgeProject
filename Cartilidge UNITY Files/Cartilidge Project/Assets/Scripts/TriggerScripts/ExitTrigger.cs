using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// The camera that is following the player.
    /// </summary>
    public CinemachineVirtualCamera cam;
    /// <summary>
    /// The player controller.
    /// </summary>
    public CartilidgeController playerController;
    /// <summary>
    /// The class that handles scene transitioning.
    /// </summary>
    public SceneTransitionManager transitionManager;
    /// <summary>
    /// The In Effector that is attached to this door.
    /// </summary>
    public GameObject correspondingInEffector;
    /// <summary>
    /// Return true if the player exits stage right, or false if not.
    /// </summary>
    public bool outRight = true;
    /// <summary>
    /// The scene this exit trigger takes you to.
    /// </summary>
    public string gotoScene;

    /// <summary>
    /// The current game instance's information.
    /// </summary>
    private GameInstanceInformation instanceInformation;
    /// <summary>
    /// The health that is attached to Cartilidge.
    /// </summary>
    private CartilidgeHealth cartilidgeHealth;
    /// <summary>
    /// The push force of the player controller halved.
    /// </summary>
    private float halvedFore;
    #endregion

    private void Start()
    {
        instanceInformation = FindObjectOfType<GameInstanceInformation>();
        cartilidgeHealth = playerController.GetComponent<CartilidgeHealth>();
        transitionManager = FindObjectOfType<SceneTransitionManager>();
        halvedFore = playerController.pushForce * 0.5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ExitFunction();
        }
    }

    private void ExitFunction()
    {
        correspondingInEffector.SetActive(false);
        cam.Follow = null;
        playerController.controllable = false;
        instanceInformation.currentPlayerHealth = cartilidgeHealth.currentHealth;
        instanceInformation.exitedSceneName = SceneManager.GetActiveScene().name;

        if (outRight)
        {
            playerController.rb2D.AddForce(Vector3.right * halvedFore, ForceMode2D.Force);
        }
        else
        {
            playerController.rb2D.AddForce(Vector3.left * halvedFore, ForceMode2D.Force);
        }

        transitionManager.FadeToNewScene(gotoScene);
    }
}