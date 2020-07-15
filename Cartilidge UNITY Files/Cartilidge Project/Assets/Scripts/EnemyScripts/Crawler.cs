using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class Crawler : Damager
{
    #region Variables
    /// <summary>
    /// The initial direction this crawler moves in.
    /// </summary>
    public Vector2 movementDirection;
    /// <summary>
    /// The speed at which this crawler moves.
    /// </summary>
    public float movementSpeed = 5f;
    /// <summary>
    /// The time this crawler takes inbetween movements.
    /// </summary>
    public float timeBetweenMovements;
    /// <summary>
    /// The speed at which this crawler will flip.
    /// </summary>
    public float flipSpeed;
    
    /// <summary>
    /// Return true if crawler is flipping, or false if not.
    /// </summary>
    private bool flipping = false;
    /// <summary>
    /// Return true if crawler should move, or false if not.
    /// </summary>
    private bool moveCrawler = true;
    /// <summary>
    /// The newer scale value that the crawler will transition to.
    /// </summary>
    private float newScaleValue;
    #endregion

    protected override void Start()
    {
        base.Start();

        StartCoroutine(IncrementMovement());
    }
    
    void Update()
    {
        FlippingBehavior();
    }

    IEnumerator IncrementMovement()
    {
        yield return new WaitForSeconds(timeBetweenMovements);

        moveCrawler = !moveCrawler;

        StartCoroutine(IncrementMovement());
    }

    private void FlippingBehavior()
    {
        if (!flipping)
        {
            if (moveCrawler)
            {
                transform.Translate(movementDirection * movementSpeed * Time.deltaTime);
            }
        }

        if (flipping)
        {
            Vector3 newScale = transform.localScale;
            newScale.x += flipSpeed * Mathf.Sign(newScaleValue) * Time.deltaTime;
            transform.localScale = newScale;

            if (Mathf.Abs(transform.localScale.x) > Mathf.Abs(newScaleValue))
            {
                transform.localScale = new Vector3(newScaleValue, transform.localScale.y, transform.localScale.z);
                movementSpeed *= -1;
                flipping = false;
            }
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Crawler")
        {
            if (!flipping)
            {
                flipping = true;
                newScaleValue = transform.localScale.x * -1;
            }
        }
    }
}