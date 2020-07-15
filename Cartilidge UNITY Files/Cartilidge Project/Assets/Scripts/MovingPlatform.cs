using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Disable the platform at the start of play.
    /// </summary>
    public bool disableOnStart;
    /// <summary>
    /// The color of the platform when it is activated.
    /// </summary>
    public Color activatedColor = Color.white;

    /// <summary>
    /// The Animator attached to this moving platform.
    /// </summary>
    public Animator attachedAnimator { get; set; }

    /// <summary>
    /// The Sprite Renderer attached to this platform.
    /// </summary>
    private SpriteRenderer attachedRender;
    #endregion

    private void Start()
    {
        attachedAnimator = GetComponent<Animator>();
        attachedRender = GetComponent<SpriteRenderer>();

        if (disableOnStart)
        {
            attachedAnimator.enabled = false;
        }
    }

    /// <summary>
    /// Enables the attached animator component.
    /// </summary>
    public void EnableAnimator()
    {
        attachedAnimator.enabled = true;
    }

    /// <summary>
    /// Set's the color of the platform to something new.
    /// </summary>
    public void SetPlatformColorToBlue()
    {
        attachedRender.color = activatedColor;
    }
}