using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUpManager : MonoBehaviour
{
    public float baseSpeed = 5f;
    public float baseJumpHeight = 5f;
    private float currentSpeed;
    private float currentJumpHeight;
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentSpeed = baseSpeed;
        currentJumpHeight = baseJumpHeight;
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Aquí podrías agregar el movimiento o interacción con el jugador
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log($"Salud actual: {currentHealth}");
    }

    public void ModifySpeed(float multiplier)
    {
        currentSpeed = baseSpeed * multiplier;
        Debug.Log($"Velocidad actual: {currentSpeed}");
    }

    public void ModifyJumpHeight(float multiplier)
    {
        currentJumpHeight = baseJumpHeight * multiplier;
        Debug.Log($"Altura de salto actual: {currentJumpHeight}");
    }
}
