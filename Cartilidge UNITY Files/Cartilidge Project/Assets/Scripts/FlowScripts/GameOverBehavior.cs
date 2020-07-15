using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverBehavior : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Return ture if player is able to close the game, or false if not.
    /// </summary>
    private bool ableToClose = false;
    #endregion

    private void Update()
    {
        if (ableToClose && Input.GetButtonDown("Jump"))
        {
            Application.Quit();
            Debug.Log("Player has quit game");
        }
    }

    /// <summary>
    /// Toggles the player's ability to close the game on.
    /// </summary>
    public void ToggleGameClosureAbilityOn()
    {
        ableToClose = true;
    }
}