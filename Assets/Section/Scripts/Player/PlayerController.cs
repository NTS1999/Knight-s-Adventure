using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section4
{
    public class PlayerController : MonoBehaviour
    {
        /*private AngryPigController angryPig;*/
        private float horizontal;
        private float vertical;
        private float moveSpeed = 5f;
        private bool isMoving;
        private bool isFacingRight;
        private float jumpForce = 12f;
        private bool doubleJump;
        private bool attackInput;
        private float climbSpeed = 4f;
        private Collider2D playerCollider2D;
        private int curHP;
        private bool getHit;
        private float getHitTime;
        private bool death;

        [SerializeField] private int maxHP;
        [SerializeField] private HPbarController hpBar;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] Animator animator;
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private Transform playerFoots;
        [SerializeField] private Vector2 groundCastSize;
        [SerializeField] private LayerMask ladderLayerMask;

        private void Start()
        {
            TryGetComponent(out playerCollider2D);
            curHP = maxHP;
            hpBar.SetMaxHealth(maxHP);
        }

        private void Update()
        {
            //Action
            vertical = Input.GetAxisRaw("Vertical");
            Flip();
            Jump();
            Attack();

            //GetHitCD
            if (getHit)
            {
                getHitTime -= Time.deltaTime;
                if (getHitTime <= 0)
                    getHit = false;
            }
        }

        private void FixedUpdate()
        {
            if (death)
            {
                rb.velocity = new Vector2(horizontal * 0, rb.velocity.y);
                return;
            }
            else if (getHit)
                return;
            Movement();
            CheckLadder();
        }

        private void Movement()
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            animator.SetFloat("moveX", Mathf.Abs(horizontal));
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        }

        private void Flip()
        {
            if (death)
                return;
            if (isFacingRight && horizontal > 0f || !isFacingRight && horizontal < 0f)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1;
                transform.localScale = localScale;
            }
        }

        private void Jump()
        {
            if (death)
                return;
            //Jump
            if (OnGround() && !Input.GetButton("Jump"))
            {
                doubleJump = false;
            }
            if (Input.GetButtonDown("Jump"))
            {
                if (OnGround() || doubleJump)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    doubleJump = !doubleJump;
                }
            }
            if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
        }

        private void Attack()
        {
            if (death)
                return;
            //Attack
            if (Input.GetButtonDown("Fire1"))
            {
                animator.SetTrigger("Attack");
            }
        }

        private void CheckLadder()
        {
            if (playerCollider2D.IsTouchingLayers(ladderLayerMask))
            {
                Vector2 velocity = rb.velocity;
                velocity.y = climbSpeed * vertical;
                rb.velocity = velocity;
                rb.gravityScale = 0;
            }
            else
            {
                rb.gravityScale = 5f;
            }
        }

        private bool OnGround()
        {
            return Physics2D.OverlapCircle(playerFoots.position, 0.2f, groundLayerMask);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(playerFoots.position, groundCastSize);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (getHit || death)
                return;
            if (collision.CompareTag("Enemy"))
            {
                curHP -= 1;
                getHit = true;
                getHitTime = 0.5f;
                hpBar.SetHealth(curHP);
                if (curHP <= 0)
                {
                    death = true;
                    AudioManager.Instance.PlaySFX_PlayerDead();
                    GamePlayManager.Instance.Gameover(false);
                    animator.SetTrigger("isDead");
                    return;
                }

                AudioManager.Instance.PlaySFX_PlayerGetHit();

                Vector2 knockbackDirection = transform.position - collision.transform.position;
                knockbackDirection = knockbackDirection.normalized;
                rb.AddForce(knockbackDirection * 10, ForceMode2D.Impulse);

                StartCoroutine(GetHitFX());
            }
            else if (collision.CompareTag("Trap"))
            {
                curHP = 0;
                hpBar.SetHealth(curHP);
                if (curHP <= 0)
                {
                    death = true;
                    AudioManager.Instance.PlaySFX_PlayerDead();
                    GamePlayManager.Instance.Gameover(false);
                    animator.SetTrigger("isDead");
                    return;
                }
            }
        }

        private IEnumerator GetHitFX()
        {
            CameraShake.Instance.Shake(0.1f);

            SpriteRenderer spt;
            TryGetComponent(out spt);
            Color transparent = Color.white;
            transparent.a = 0.25f;
            int i = 0;
            while (getHitTime > 0)
            {
                if (i % 2 == 0)
                    spt.color = Color.white;
                else
                    spt.color = transparent;
                i++;
                yield return null;
            }
            spt.color = Color.white;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("MoveablePlatform"))
            {
                transform.SetParent(collision.transform);
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("MoveablePlatform"))
            {
                transform.SetParent(collision.transform);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("MoveablePlatform"))
            {
                transform.SetParent(null);
            }
        }

        private void PlayAttackSFX()
        {
            AudioManager.Instance.PlaySFX_MeleeSplash();
        }
    }
}
