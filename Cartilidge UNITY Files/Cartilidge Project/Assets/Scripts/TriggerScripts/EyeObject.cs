using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeObject : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// The instance that exists throughout the game
    /// </summary>
    private GameInstanceInformation instanceInformation;
    #endregion

    /// <summary>
    /// Call this once the scene fades in.
    /// </summary>
    public void SceneInitialization ()
    {
        instanceInformation = FindObjectOfType<GameInstanceInformation>();
        if (instanceInformation.hasEye)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            instanceInformation.hasEye = true;
            Destroy(gameObject);
        }
    }
}