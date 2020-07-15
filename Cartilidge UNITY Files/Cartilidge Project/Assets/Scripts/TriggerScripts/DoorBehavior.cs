using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// The color of the door when it's locked.
    /// </summary>
    public Color lockedColor = new Color(0.678f, 0.408f, 0.408f, 1f);
    /// <summary>
    /// The color of the door when it's unlocked.
    /// </summary>
    public Color unlockedColor = new Color(0.263f, 0.325f, 0.341f, 1f);
    /// <summary>
    /// The solid collider on the door that isn't a trigger.
    /// </summary>
    public BoxCollider2D solidDoorCollider;
    /// <summary>
    /// Return true if door is unlocked, or false if not.
    /// </summary>
    public bool unlocked;

    /// <summary>
    /// The animator attached to the door.
    /// </summary>
    private Animator attachedAnimator;
    /// <summary>
    /// The Sprite Renderer attached to this door.
    /// </summary>
    private SpriteRenderer attachedSprite;
    #endregion

    private void Start()
    {
        attachedSprite = GetComponent<SpriteRenderer>();
        attachedAnimator = GetComponent<Animator>();
        UpdateDoorStatus(unlocked);
    }

    public void UpdateDoorStatus(bool newStatus)
    {
        unlocked = newStatus;

        if (unlocked)
        {
            attachedSprite.color = unlockedColor;
        }
        else
        {
            attachedSprite.color = lockedColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (unlocked)
        {
            if (collision.tag == "Player")
            {
                attachedAnimator.SetBool("Open", true);
                solidDoorCollider.enabled = false;
            } 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (unlocked)
        {
            if (collision.tag == "Player")
            {
                attachedAnimator.SetBool("Open", false);
                solidDoorCollider.enabled = true;
            } 
        }
    }
}