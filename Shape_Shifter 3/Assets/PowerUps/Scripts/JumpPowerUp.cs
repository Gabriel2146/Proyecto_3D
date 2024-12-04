using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPowerUp : PowerUpBase
{
    public float jumpMultiplier = 1.5f; // Multiplicador de salto

    protected override void ApplyEffect(PlayerPowerUpManager player)
    {
        player.ModifyJumpHeight(jumpMultiplier);
        Debug.Log("¡Salto aumentado!");
    }

    protected override void RemoveEffect(PlayerPowerUpManager player)
    {
        player.ModifyJumpHeight(1f); // Restaurar la altura de salto
    }
}
