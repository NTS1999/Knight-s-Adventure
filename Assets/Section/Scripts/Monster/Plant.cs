using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section4
{
    public class Plant : MonoBehaviour
    {
        public enum State { Idle, Attack, Hit, }

        [SerializeField] Animator animator;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject bulletPrefab;

        private State currentState;
        private float inRange = 7f;
        private bool isFacingRight = false;
        private bool getHit;
        private float coolDown = 2f;
        private float tempCoolDown;
        private int hp = 3;
        Transform playerPos;

        void Start()
        {
            SetState(State.Idle);
            playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            StartCoroutine(UpdateAI());
            LookAtPlayer();
            if (Vector2.Distance(playerPos.position, rb.position) <= inRange)
            {
                if (tempCoolDown <= 0)
                {
                    StartCoroutine(Shoot());
                    tempCoolDown = coolDown;
                }
                tempCoolDown -= Time.deltaTime;
            }
        }

        private IEnumerator UpdateAI()
        {
            if (currentState == State.Hit)
            {
                yield return new WaitForSeconds(0.5f);
                getHit = false;
                SetState(State.Idle);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (getHit)
                return;
            if (collision.CompareTag("Attack"))
            {
                AudioManager.Instance.PlaySFX_EnemyGetHit();

                hp -= 1;
                if (hp <= 0)
                {
                    Destroy(gameObject);
                    GamePlayManager.Instance.EnemyDied();
                    return;
                }
                getHit = true;
                SetState(State.Hit);
                Vector2 knockbackDirection = transform.position - collision.transform.position;
                knockbackDirection = knockbackDirection.normalized;
                rb.AddForce(knockbackDirection * 10, ForceMode2D.Impulse);
            }
        }

        private void LookAtPlayer()
        {
            Vector3 fliped = transform.localScale;
            fliped.z *= -1f;

            if (transform.position.x > playerPos.position.x && isFacingRight)
            {
                transform.localScale = fliped;
                transform.Rotate(0f, 180f, 0f);
                isFacingRight = false;
            }
            else if (transform.position.x < playerPos.position.x && !isFacingRight)
            {
                transform.localScale = fliped;
                transform.Rotate(0f, 180f, 0f);
                isFacingRight = true;
            }
        }

        private IEnumerator Shoot()
        {
            SetState(State.Attack);
            yield return new WaitForSeconds(0.25f);
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Destroy(bullet, 1.3f);
        }

        private void SetDirection(bool isFacingRight)
        {
            if (isFacingRight)
                transform.localRotation = Quaternion.Euler(0,180,0);
            else
                transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        private void SetState(State state)
        {
            currentState = state;
            switch (state)
            {
                case State.Idle:
                    PlayIdleAnimation();
                    break;
                case State.Attack:
                    PlayAttackAnimation();
                    break;
                case State.Hit:
                    PlayHitAnimation();
                    break;
            }
        }

        private void PlayIdleAnimation()
        {
            animator.SetTrigger("Change");
            animator.SetInteger("State", 1);
        }
        private void PlayAttackAnimation()
        {
            animator.SetTrigger("Change");
            animator.SetInteger("State", 2);
        }
        private void PlayHitAnimation()
        {
            animator.SetTrigger("Change");
            animator.SetInteger("State", 3);
        }
    }
}
