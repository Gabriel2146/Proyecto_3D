using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Flag currentFlag; // Bandera que el jugador está llevando
    public Transform basePosition; // La base del jugador

    void Update()
    {
        // Si el jugador tiene una bandera y la lleva a la base
        if (currentFlag != null && Vector3.Distance(transform.position, basePosition.position) < 2f)
        {
            CaptureFlag();
        }
    }

    public void PickupFlag(Flag flag)
    {
        // Cuando el jugador recoge la bandera
        currentFlag = flag;
        currentFlag.isCaptured = true; // La bandera ya está siendo llevada
        currentFlag.gameObject.SetActive(false); // La bandera desaparece del mapa
    }

    public void CaptureFlag()
    {
        if (currentFlag != null)
        {
            // El jugador ha llegado a la base con la bandera
            currentFlag.CaptureFlag();
            Debug.Log("¡Bandera capturada!");
            currentFlag = null;
            // Aquí puedes añadir lógica para la recompensa por capturar la bandera, como puntos o un respawn
        }
    }

    public void ResetFlag()
    {
        if (currentFlag != null)
        {
            // Resetear la bandera si el jugador muere o es derrotado
            currentFlag.ResetFlagPosition();
            currentFlag = null;
        }
    }
}
