using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private int scoreValue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.Play("Collect");
            GameManager.Instance.AddScore(scoreValue);
            gameObject.SetActive(false);
        }
    }
}
