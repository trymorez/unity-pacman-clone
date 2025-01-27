using System;
using UnityEngine;

public class Bean : MonoBehaviour
{
    [SerializeField] protected int score = 10;

    public static Action<int> OnScoreUpdate;

    void Awake()
    {
        BeanManager.Beans++;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pacman"))
        {
            SoundManager.Play("Bean");
            BeanManager.Beans--;
            GameManager.Score += score;
            OnScoreUpdate?.Invoke(GameManager.Score);
            gameObject.SetActive(false);
            if (BeanManager.Beans == 0)
            {
                GameManager.Instance.NextLevel();
            }
        }
    }
}
