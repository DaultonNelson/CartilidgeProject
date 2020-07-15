using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Invokes when the scene first starts.
    /// </summary>
    public UnityEvent onSceneEnter;
    /// <summary>
    /// The event that triggers once the fade in is complete.
    /// </summary>
    public UnityEvent fadeInComplete;

    /// <summary>
    /// The Animator attached to this Game Object.
    /// </summary>
    public Animator attachedAnimator { get; private set; }

    /// <summary>
    /// The scene this transitioner is taking the player to.
    /// </summary>
    private string gotoScene;
    #endregion

    private void Start()
    {
        attachedAnimator = GetComponent<Animator>();
        onSceneEnter.AddListener(FadeInTrigger);
        onSceneEnter.Invoke();
    }

    private void FadeInTrigger()
    {
        attachedAnimator.SetTrigger("FadeIn");
    }

    public void OnFadeInComplete()
    {
        //Debug.Log("Fade In Complete");
        fadeInComplete.Invoke();
    }

    public void OnFadeOutComplete()
    {
        SceneManager.LoadScene(gotoScene);
    }

    /// <summary>
    /// Causes the transitioner to fade out for a new scene.
    /// </summary>
    /// <param name="sceneName">
    /// The name of the scene the transition will fade out to.
    /// </param>
    public void FadeToNewScene (string sceneName)
    {
        attachedAnimator.SetTrigger("FadeOut");
        gotoScene = sceneName;
    }
}