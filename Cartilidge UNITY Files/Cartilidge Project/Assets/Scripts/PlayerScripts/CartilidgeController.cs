using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

public class CartilidgeController : MonoBehaviour {
    #region Variables
    /// <summary>
    /// The Rigidbody2D that is attached to the GameObject.
    /// </summary>
    public Rigidbody2D rb2D;
    /// <summary>
    /// The Collider2D that is attached to the GameObject.
    /// </summary>
    public CircleCollider2D col2D;
    /// <summary>
    /// The virtual camera that follows Cartilidge around.
    /// </summary>
    public CinemachineVirtualCamera cvc;
    /// <summary>
    /// The layers that will be hit by the grounded raycast.
    /// </summary>
    public LayerMask hittingLayers;
    /// <summary>
    /// The amount of force that pushes Cartilidge to move.
    /// </summary>
    public float pushForce;
    /// <summary>
    /// The initial amount of force that Cartilidge uses to jump.
    /// </summary>
    public float initialJumpForce;
    /// <summary>
    /// The maximum amount of force that Cartilidge uses to jump.
    /// </summary>
    public float maxJumpForce;
    /// <summary>
    /// The velocity Cartilidge is allowed to get to before it limits itself.
    /// </summary>
    public float velocityCap;
    /// <summary>
    /// The distance Cartilige needs to be from the ground in order to be grounded.
    /// </summary>
    public float groundedDistance = 0.01f;
    /// <summary>
    /// Return true if player is controllable, or false if not.
    /// </summary>
    public bool controllable = true;

    /// <summary>
    /// The current speed the player is moving at.
    /// </summary>
    public float currentPlayerSpeed { get; private set; }
    /// <summary>
    /// The current jump force that Cartilidge has access to.
    /// </summary>
    public float currentJumpForce { get; set; }

    /// <summary>
    /// The RaycastHit2D info.
    /// </summary>
    private RaycastHit2D hit;
    /// <summary>
    /// A number telling in which direction Cartilidge is moving horizontally.
    /// </summary>
    private float horiDirection = 0;
    /// <summary>
    /// Return true if controller is able to jump, or false if not.
    /// </summary>
    private bool ableToJump = true;
    #endregion

    //left - positive | right - negative

    //Best for Controller Input;
    private void Update()
    {
        if (controllable)
        {
            CalculateJumpForce();
            InputMovement();
        }
    }

    private void CalculateJumpForce()
    {
        currentJumpForce = Mathf.Lerp(initialJumpForce, maxJumpForce, (currentPlayerSpeed/velocityCap));
    }

    private void InputMovement()
    {
        #region Horizontal Movement
        float raw = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(rb2D.velocity.x) < velocityCap)
        {
            if (raw < 0 || raw > 0)
            {
                //left = -1 , right = 1
                horiDirection = Mathf.Sign(raw);
            }
            else if (raw == 0)
            {
                horiDirection = 0;
            }
        }
        else if (Mathf.Abs(rb2D.velocity.x) >= velocityCap)
        {
            horiDirection = 0;
        }

        if (Input.GetButtonUp("Horizontal"))
        {
            horiDirection = 0;
        }
        #endregion

        if (IsGrounded() == true && Input.GetButtonDown("Jump"))
        {
            if (ableToJump)
            {
                rb2D.AddForce(Vector3.up * currentJumpForce, ForceMode2D.Force);
            }
        }
    }

    //Best for physics
    void FixedUpdate()
    {
        if (controllable)
        {
            MoveCartilidge(horiDirection);
        }
    }

    void MoveCartilidge(float dir)
    {
        rb2D.AddForce(Vector3.right * (pushForce * dir) * Time.fixedDeltaTime);

        if (rb2D.velocity.magnitude > velocityCap)
        {
            rb2D.velocity = Vector3.ClampMagnitude(rb2D.velocity, velocityCap);
        }

        currentPlayerSpeed = rb2D.velocity.magnitude;
    }

    /// <summary>
    /// Return true if raycast is hitting something or not.
    /// </summary>
    bool hitting = false;

    public bool IsGrounded()
    {
        bool output = false;
        hit = Physics2D.CircleCast(col2D.bounds.center, col2D.radius, Vector2.down, col2D.bounds.extents.y + groundedDistance, hittingLayers);
        if (hit)
        {
            hitting = true;
            output = true;
        }
        else
        {
            hitting = false;
        }

        return output;
    }

    private void OnDrawGizmos()
    {
        if (hitting)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }

        if (col2D != null)
        {
            if (hit)
            {
                Gizmos.DrawLine(col2D.bounds.center, hit.point);
            }
            else
            {
                Gizmos.DrawRay(col2D.bounds.center, Vector2.down * (col2D.bounds.extents.y + groundedDistance));
            }
        }
    }

    /// <summary>
    /// Sets Cartilidge controllability to new status.
    /// </summary>
    /// <param name="newStatus">
    /// The new controllable status.
    /// </param>
    public void ToggleControllability(bool newStatus)
    {
        controllable = newStatus;
        if (controllable)
        {
            var frameTransposer = cvc.GetCinemachineComponent<CinemachineFramingTransposer>();
            frameTransposer.m_LookaheadTime = 0.5f;
            frameTransposer.m_LookaheadSmoothing = 18;
            frameTransposer.m_XDamping = 0.1f;
            frameTransposer.m_YDamping = 0.1f;
            frameTransposer.m_ZDamping = 0.1f;
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "SpriteShape")
        {
            groundedDistance = 1000f;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "SpriteShape")
        {
            groundedDistance = 0.01f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DialogueTrigger")
        {
            ableToJump = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "DialogueTrigger")
        {
            ableToJump = true;
        }
    }
}