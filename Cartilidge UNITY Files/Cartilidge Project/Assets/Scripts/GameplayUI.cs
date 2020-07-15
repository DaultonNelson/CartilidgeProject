using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// The player character, Cartilidge.
    /// </summary>
    public CartilidgeController cartilidge;
    /// <summary>
    /// The gradient that displays the speed of the player.
    /// </summary>
    public Image speedGradient;
    #endregion
    
    void Update()
    {
        speedGradient.fillAmount = (cartilidge.currentPlayerSpeed/cartilidge.velocityCap);
    }
}