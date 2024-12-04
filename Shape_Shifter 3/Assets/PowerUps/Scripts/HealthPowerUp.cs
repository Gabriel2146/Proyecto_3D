using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUp : PowerUpBase
{
    public int healAmount = 20; // Cantidad de vida que cura

    protected override void ApplyEffect(PlayerPowerUpManager player)
    {
        player.Heal(healAmount);
        Debug.Log("¡Vida restaurada!");
    }

    protected override void RemoveEffect(PlayerPowerUpManager player)
    {
        // Este power-up no tiene efectos continuos que deban ser eliminados
    }
}
