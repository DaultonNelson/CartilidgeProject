using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartilidgeHealth : MonoBehaviour {
    #region Variables
    /// <summary>
    /// The graphic that shows Cartilidge's health.
    /// </summary>
    public Image healthGraphic;
    /// <summary>
    /// The sprite that represents Cartilidge.
    /// </summary>
    public SpriteRenderer cartilidgeSprite;
    /// <summary>
    /// Controls the transitioning between scenes.
    /// </summary>
    public SceneTransitionManager transitionManager;
    /// <summary>
    /// The time in which Cartilidge is invincible when it's hit.
    /// </summary>
    public float invincibleFrames;
    /// <summary>
    /// The time in which the Cartilidge will be blink.
    /// </summary>
    public float blinkRate;

    /// <summary>
    /// The current amount of health Cartilidge has.
    /// </summary>
    public int currentHealth { get; set; }
    /// <summary>
    /// Return true if Cartilidge is dead, or false if not.
    /// </summary>
    public bool isDead { get; set; }

    /// <summary>
    /// The current game instance's information.
    /// </summary>
    private GameInstanceInformation instanceInformation;
    /// <summary>
    /// The maximum amount of health the player can have.
    /// </summary>
    private const int maxHealth = 8;
    /// <summary>
    /// The name of the Game Over death scene.
    /// </summary>
    private const string deathScene = "5 - GameOverScene";
    /// <summary>
    /// The floating point values that the health will switch to when updating.
    /// </summary>
    private List<float> fillValues = new List<float> { 0f, 0.063f, 0.245f, 0.366f, 0.5f, 0.622f, 0.75f, 0.831f };
    /// <summary>
    /// Return true if Cartilidge is invincible, or false if not.
    /// </summary>
    private bool isInvincible = false;
    #endregion

    /// <summary>
    /// Should be done once the scene is ready to be played.
    /// </summary>
    public void SceneInitialization()
    {
        instanceInformation = FindObjectOfType<GameInstanceInformation>();
        currentHealth = instanceInformation.currentPlayerHealth;
        UpdateHealthGraphic();
    }

    /// <summary>
    /// Damages Cartilidge by the amount given.
    /// </summary>
    /// <param name="incomingDamage">
    /// The amount of Damage Cartilidge will take.
    /// </param>
    public void DamageCartilidge(int incomingDamage)
    {
        if (!isInvincible)
        {
            currentHealth -= incomingDamage;
            UpdateHealthGraphic();
            CheckForDeath();
            if (!isDead)
            {
                isInvincible = true;
                StartCoroutine(MakeVulnerable());
                StartCoroutine(BlinkSprite());
            }
        }
    }

    /// <summary>
    /// Heals Cartilidge by the amount given.
    /// </summary>
    /// <param name="incomingRelief">
    /// The amount of health Cartilidge is getting
    /// </param>
    public void HealCartilidge(int incomingRelief)
    {
        if (currentHealth < maxHealth)
        {
            int healthlost = maxHealth - currentHealth;

            if (incomingRelief > healthlost)
            {
                currentHealth = maxHealth;
            }
            else
            {
                currentHealth += incomingRelief;
            }

            UpdateHealthGraphic();
        }
        else
        {
            return;
        }
    }

    IEnumerator BlinkSprite ()
    {
        yield return new WaitForSeconds(blinkRate);
        cartilidgeSprite.enabled = !cartilidgeSprite.enabled;
        if (isInvincible || cartilidgeSprite.enabled == false)
        {
            StartCoroutine(BlinkSprite()); 
        }
    }

    IEnumerator MakeVulnerable()
    {
        yield return new WaitForSeconds(invincibleFrames);
        isInvincible = false;
    }

    private void CheckForDeath()
    {
        if (currentHealth <= 0)
        {
            isDead = true;
            Debug.Log("Cartilidge has died");
            transitionManager.FadeToNewScene(deathScene);
            gameObject.SetActive(false);
        }
    }

    private void UpdateHealthGraphic()
    {
        if (currentHealth < maxHealth)
        {
            healthGraphic.fillAmount = fillValues[currentHealth]; 
        }
        else
        {
            healthGraphic.fillAmount = maxHealth;
        }
    }
}