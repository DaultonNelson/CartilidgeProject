using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartilidgeMovementLines : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// The player controller, Cartilidge.
    /// </summary>
    public CartilidgeController cartilidgeController;
    /// <summary>
    /// The movement lines left of Cartilidge.
    /// </summary>
    public SpriteRenderer moveLinesLeft;
    /// <summary>
    /// The movement lines right of Cartilidge.
    /// </summary>
    public SpriteRenderer moveLinesRight;
    /// <summary>
    /// The alpha value of the movement line when it's on.
    /// </summary>
    [Range(0.16f, 1f)]
    public float onValue = 0.5f;

    /// <summary>
    /// The alpha value of the movement line when it's off.
    /// </summary>
    private const float offValue = 0.157f;
    #endregion

    private void Update()
    {
        transform.position = cartilidgeController.transform.position;

        if (cartilidgeController.controllable)
        {
            AlphaInterpolation();
        }
    }

    private void AlphaInterpolation()
    {
        float raw = Input.GetAxisRaw("Horizontal");

        //Left
        if (raw <= 0)
        {
            Color lCol = moveLinesLeft.color;
            lCol.a = Mathf.Lerp(offValue, onValue, Mathf.Abs(raw));
            moveLinesLeft.color = lCol;
        }

        //Right
        if (raw >= 0)
        {
            Color rCol = moveLinesRight.color;
            rCol.a = Mathf.Lerp(offValue, onValue, Mathf.Abs(raw));
            moveLinesRight.color = rCol;
        }
    }
}