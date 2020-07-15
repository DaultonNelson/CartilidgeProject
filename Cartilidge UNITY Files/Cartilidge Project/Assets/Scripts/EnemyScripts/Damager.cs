using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// The amount of force this spike uses to push Cartilidge away.
    /// </summary>
    public float pushForce = 25f;

    /// <summary>
    /// The controller attached to Cartilidge.
    /// </summary>
    private CartilidgeController cartilidgeController;
    /// <summary>
    /// The health that Cartilidge has.
    /// </summary>
    private CartilidgeHealth cartilidgeHealth; 
    #endregion

    protected virtual void Start ()
    {
        cartilidgeController = FindObjectOfType<CartilidgeController>();
        cartilidgeHealth = FindObjectOfType<CartilidgeHealth>();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!cartilidgeHealth.isDead)
            {
                cartilidgeHealth.DamageCartilidge(1);

                Vector2 pushDirection = collision.contacts[0].point - (Vector2)transform.position;
                pushDirection = -pushDirection.normalized;
                cartilidgeController.rb2D.AddForce(pushDirection * pushForce, ForceMode2D.Impulse); 
            }
        }
    }
}