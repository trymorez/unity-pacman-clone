using System;
using UnityEngine;

public class PowerBeans : Bean
{
    
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pacman"))
        {
            GameManager GM = GameManager.Instance;

            base.OnTriggerEnter2D(other);
            GM.GameStateChange(GameManager.GameState.PacmanPowerUp);
        }
    }
}
