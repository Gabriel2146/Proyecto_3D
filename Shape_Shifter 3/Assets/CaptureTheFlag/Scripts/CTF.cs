using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public bool isCaptured = false; // Indica si la bandera ha sido capturada
    public Transform flagBase; // La base donde debe ser entregada la bandera

    void OnTriggerEnter(Collider other)
    {
        if (!isCaptured && other.CompareTag("Player"))
        {
            // Cuando el jugador recoge la bandera
            other.GetComponent<Player>().PickupFlag(this);
        }
    }

    public void ResetFlagPosition()
    {
        // Resetear la bandera a su posición original si es necesario
        transform.position = flagBase.position;
        isCaptured = false;
        gameObject.SetActive(true);
    }

    public void CaptureFlag()
    {
        // Cuando la bandera es capturada y llevada a la base
        isCaptured = true;
        gameObject.SetActive(false); // La bandera desaparece hasta que se reinicie
    }
}
