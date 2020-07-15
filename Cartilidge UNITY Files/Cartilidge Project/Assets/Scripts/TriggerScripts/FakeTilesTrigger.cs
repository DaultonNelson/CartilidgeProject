using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FakeTilesTrigger : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// The tiles that are fake and will disappear.
    /// </summary>
    public Tilemap fakeTiles;
    /// <summary>
    /// The rate at which the fake tiles will fade away.
    /// </summary>
    public float fadeRate = 0.008f;

    /// <summary>
    /// Return true if trigger has been hit by player, or false if not.
    /// </summary>
    private bool hitbyPlayer = false;
    #endregion

    private void Update()
    {
        if (hitbyPlayer)
        {
            FadeFakeTiles();
        }
    }

    private void FadeFakeTiles()
    {
        Color newColor = fakeTiles.color;
        newColor.a -= fadeRate;
        fakeTiles.color = newColor;

        if (fakeTiles.color.a <= 0)
        {
            fakeTiles.enabled = false;
            this.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            hitbyPlayer = true;
        }
    }
}
