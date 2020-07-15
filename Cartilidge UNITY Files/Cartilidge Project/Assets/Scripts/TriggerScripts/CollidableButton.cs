using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollidableButton : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// The events that player when Cartilidge hits the button.
    /// </summary>
    public UnityEvent onGameButtonPressed;

    /// <summary>
    /// The Animator attached ot this game button.
    /// </summary>
    private Animator attachedAnimator;
    #endregion

    private void Start()
    {
        attachedAnimator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            onGameButtonPressed.Invoke();
            attachedAnimator.SetBool("Downed", true);
        }
    }
}