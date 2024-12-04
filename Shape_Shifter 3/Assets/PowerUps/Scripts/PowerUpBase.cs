using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpBase : MonoBehaviour
{
    public float duration = 5f; // Duración del efecto
    protected bool isActive = false;

    protected abstract void ApplyEffect(PlayerPowerUpManager player);
    protected abstract void RemoveEffect(PlayerPowerUpManager player);

    void OnTriggerEnter(Collider other)
    {
        PlayerPowerUpManager player = other.GetComponent<PlayerPowerUpManager>();
        if (player != null && !isActive)
        {
            isActive = true;
            ApplyEffect(player);
            StartCoroutine(PowerUpDuration(player));
        }
    }

    private IEnumerator PowerUpDuration(PlayerPowerUpManager player)
    {
        yield return new WaitForSeconds(duration);
        RemoveEffect(player);
        Destroy(gameObject); // Destruir el power-up después de usarlo
    }
}
