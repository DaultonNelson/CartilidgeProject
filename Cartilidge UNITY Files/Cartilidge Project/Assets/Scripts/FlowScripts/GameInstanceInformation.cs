using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstanceInformation : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// The instance for the game itself.
    /// Holds all values that need to be carried from scene to scene.
    /// </summary>
    public static GameInstanceInformation instance = null;

    /// <summary>
    /// The current amount of health the player should have.
    /// </summary>
    public int currentPlayerHealth = 8;
    /// <summary>
    /// The current talker tree layer the player should be on.
    /// </summary>
    public int talkerTreeLayer = 1;
    /// <summary>
    /// Return true if the player has the eye, or false if not.
    /// </summary>
    public bool hasEye = false;
    /// <summary>
    /// The name of the scene the player just exited.
    /// </summary>
    public string exitedSceneName;
    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Tells the instance that the player has the eye.
    /// </summary>
    public void AquireEye()
    {
        hasEye = true;
    }
}