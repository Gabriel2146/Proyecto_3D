using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : PowerUpBase
{
    public float speedMultiplier = 2f; // Multiplicador de velocidad

    protected override void ApplyEffect(PlayerPowerUpManager player)
    {
        player.ModifySpeed(speedMultiplier);
        Debug.Log("¡Velocidad aumentada!");
    }

    protected override void RemoveEffect(PlayerPowerUpManager player)
    {
        player.ModifySpeed(1f); // Restaurar la velocidad a la normalidad
    }
}
