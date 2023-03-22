using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section4
{
    public class AngryPigController : MonoBehaviour
    {
        public enum State { Idle, Walk, Run, Hit, }

        [SerializeField] Animator animator;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float walkDistance;
        [SerializeField] private Transform castWallPoint;
        [SerializeField] private Transform castGroundPoint;
        [SerializeField] private LayerMask platformLayerMask;

        private State currentState;
        private float moveSpeed = 2f;
        private float runSpeed = 4f;
        private int m_Direction = 1;
        private Vector3 startPosition;
        private bool getHit;

        private int hp = 3;
        /*private int attack = 1;*/
        /*private int exp = 1;*/

/*        public int Attack
        {
            get { return attack; }
        }*/

        void Start()
        {
            startPosition = transform.position;
            SetState(State.Idle);
            SetDirection(1);
            StartCoroutine(UpdateAI());
        }


        void Update()
        {

        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            if (!Application.isPlaying)
                startPosition = transform.position;
            Gizmos.DrawLine(new Vector2(startPosition.x - walkDistance, startPosition.y),
                new Vector2(startPosition.x + walkDistance, startPosition.y));

            Gizmos.color = Color.cyan;
            Vector3 fromPos = castWallPoint.position;
            Vector3 toPos = fromPos;
            toPos.x += m_Direction * 0.5f;
            Gizmos.DrawLine(fromPos, toPos);

            Gizmos.color = Color.magenta;
            fromPos = castGroundPoint.position;
            toPos = fromPos;
            toPos.y -= 0.5f;
            Gizmos.DrawLine(fromPos, toPos);
        }

        private IEnumerator UpdateAI()
        {
            while (true)
            {
                if (currentState == State.Idle)
                {
                    //yield return new WaitForSeconds(1f);
                    float time = 0;
                    while(time < 2 && !getHit)
                    {
                        time += Time.deltaTime;
                        yield return null;
                    }
                    if (!getHit)
                        SetState(State.Walk);
                }
                else if (currentState == State.Walk)
                {
                    float distance = Vector2.Distance(startPosition, transform.position);
                    if (distance > walkDistance || CheckWallAndGround())
                    {
                        if (transform.position.x > startPosition.x && m_Direction == 1)
                        {
                            PlayIdleAnimation();
                            rb.velocity = new Vector2(moveSpeed * 0, rb.velocity.y);
                            float time = 0;
                            while (time < 1 && !getHit)
                            {
                                time += Time.deltaTime;
                                yield return null;
                            }
                            if (!getHit)
                                PlayWalkAnimation();
                            SetDirection(-1);
                        }
                        else if (transform.position.x < startPosition.x && m_Direction == -1)
                        {
                            PlayIdleAnimation();
                            rb.velocity = new Vector2(moveSpeed * 0, rb.velocity.y);
                            float time = 0;
                            while (time < 1 && !getHit)
                            {
                                time += Time.deltaTime;
                                yield return null;
                            }
                            if (!getHit)
                                PlayWalkAnimation();
                            SetDirection(1);
                        }
                    }
                    rb.velocity = new Vector2(moveSpeed * m_Direction, rb.velocity.y);
                }
                else if (currentState == State.Hit)
                {
                    yield return new WaitForSeconds(0.5f);
                    getHit = false;
                    SetState(State.Run);
                }
                else if (currentState == State.Run)
                {
                    float distance = Vector2.Distance(startPosition, transform.position);
                    if (distance > walkDistance || CheckWallAndGround())
                    {
                        if (transform.position.x > startPosition.x && m_Direction == 1)
                        {
                            SetDirection(-1);
                        }
                        else if (transform.position.x < startPosition.x && m_Direction == -1)
                        {
                            SetDirection(1);
                        }
                    }
                    rb.velocity = new Vector2(runSpeed * m_Direction, rb.velocity.y);
                }
                yield return null;
            }
        }

        private bool CheckWallAndGround() 
        {
            bool hitWall = false;
            Vector3 fromPos = castWallPoint.position;
            Vector3 toPos = fromPos;
            toPos.x += m_Direction * 0.5f;
            hitWall = Physics2D.Linecast(fromPos, toPos, platformLayerMask);

            bool noGround = true;
            fromPos = castGroundPoint.position;
            toPos = fromPos;
            toPos.y -= 0.5f;
            noGround = !Physics2D.Linecast(fromPos, toPos, platformLayerMask);

            return hitWall || noGround;
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

        private void SetDirection(int direction)
        {
            m_Direction = direction;
            transform.localScale = new Vector3(-m_Direction, 1, 1);
        }

        private void SetState(State state)
        {
            currentState = state;
            switch (state)
            {
                case State.Idle:
                    PlayIdleAnimation();
                    break;
                case State.Walk:
                    PlayWalkAnimation();
                    break;
                case State.Run:
                    PlayRunAnimation();
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
        private void PlayWalkAnimation()
        {
            animator.SetTrigger("Change");
            animator.SetInteger("State", 2);
        }
        private void PlayRunAnimation()
        {
            animator.SetTrigger("Change");
            animator.SetInteger("State", 3);
        }
        private void PlayHitAnimation()
        {
            animator.SetTrigger("Change");
            animator.SetInteger("State", 4);
        }
    }
}

