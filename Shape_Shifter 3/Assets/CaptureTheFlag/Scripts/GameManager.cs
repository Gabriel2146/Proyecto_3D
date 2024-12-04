using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int playerScore = 0; // Puntuación del jugador

    // Llamado cada vez que el jugador captura la bandera
    public static void CaptureFlag()
    {
        playerScore++; // Incrementar el puntaje
        Debug.Log("Puntaje del jugador: " + playerScore); // Imprimir en la consola
    }
}
