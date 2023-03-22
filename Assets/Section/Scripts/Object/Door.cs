using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Section4
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Sprite openDoorSprite;
        [SerializeField] private Sprite closeDoorSprite;

        private Collider2D doorCollider;
        private SpriteRenderer spriteRenderer;
        public int nextSceneLoad;

        private void Start()
        {
            TryGetComponent(out doorCollider);
            TryGetComponent(out spriteRenderer);

            GamePlayManager.Instance.onEnemyDied += OnEnemyDied;
            nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
        }

        private void OnDestroyed()
        {
            GamePlayManager.Instance.onEnemyDied -= OnEnemyDied;
        }

        private void OnEnemyDied()
        {
            StartCoroutine(EnemyDied());
        }

        private IEnumerator EnemyDied()
        {
            yield return null;
            GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (allEnemies.Length > 0)
                CloseDoor();
            else
                OpenDoor();
        }

        private void OpenDoor()
        {
            doorCollider.enabled = true;
            spriteRenderer.sprite = openDoorSprite;
        }

        private void CloseDoor()
        {
            doorCollider.enabled = false;
            spriteRenderer.sprite = closeDoorSprite;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                GamePlayManager.Instance.Gameover(true);
                Time.timeScale = 0;
                if (nextSceneLoad > PlayerPrefs.GetInt("levelAt"))
                    PlayerPrefs.SetInt("levelAt", nextSceneLoad);
            }
        }
    }
}