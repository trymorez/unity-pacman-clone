using System;
using UnityEngine;

public class PowerBeans : Bean
{
    
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.CompareTag("Pacman"))
        {
            GameManager GM = GameManager.Instance;

            GameManager.Instance.GameStateChange(GameManager.GameState.PacmanPowerUp);
        }
    }
}
