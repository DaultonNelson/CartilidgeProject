using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingObject : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// How much health this object will give to the player.
    /// </summary>
    public int healthValue = 1;

    /// <summary>
    /// The health attached to cartilidge.
    /// </summary>
    private CartilidgeHealth cartilidgeHealth;
    #endregion

    private void Start()
    {
        cartilidgeHealth = FindObjectOfType<CartilidgeHealth>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            cartilidgeHealth.HealCartilidge(healthValue);
            Destroy(gameObject);
        }
    }
}