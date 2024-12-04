using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonaCaptura : MonoBehaviour
{
    public Flag baseFlag; // La bandera en la base
    private bool flagCaptured = false; // Estado de la captura de la bandera

    void OnTriggerEnter(Collider other)
    {
        // Verificamos si el objeto que entra en la zona es el jugador
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            // Verificamos si el jugador tiene la bandera que corresponde a esta zona
            if (player.currentFlag == baseFlag)
            {
                // Aquí ocurre la captura de la bandera
                CaptureFlag(player);
            }
        }
    }

    void CaptureFlag(Player player)
    {
        if (!flagCaptured)
        {
            flagCaptured = true;
            Debug.Log("¡Bandera capturada!");

            // Llamamos al método para aumentar el puntaje
            GameManager.CaptureFlag();

            // Aquí puedes agregar lo que desees, como desactivar la bandera
            // o hacer que vuelva a su posición inicial si fuera necesario
        }
    }
}
