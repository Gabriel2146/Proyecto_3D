using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformation : MonoBehaviour
{
    public GameObject playerModel; // Modelo del jugador
    public GameObject transformedObject; // Objeto en el que el jugador se transforma
    public Collider playerCollider; // Collider del jugador para desactivarlo cuando est� transformado
    public Collider transformedCollider; // Collider del objeto transformado
    public KeyCode transformationKey = KeyCode.T; // Tecla para transformar

    private bool isTransformed = false; // Estado de transformaci�n

    // Propiedad p�blica para que otros scripts accedan al estado de transformaci�n
    public bool IsTransformed
    {
        get { return isTransformed; }
    }

    void Update()
    {
        // Detectar la entrada para la transformaci�n
        if (Input.GetKeyDown(transformationKey))
        {
            ToggleTransformation();
        }
    }

    // Alterna entre el estado normal y transformado
    void ToggleTransformation()
    {
        if (isTransformed)
        {
            DeactivateTransformation();
        }
        else
        {
            ActivateTransformation();
        }
    }

    // Activa la transformaci�n
    void ActivateTransformation()
    {
        transformedObject.SetActive(true); // Activa el objeto transformado
        playerModel.SetActive(false); // Desactiva el modelo del jugador

        // Desactiva el collider del jugador y activa el del objeto transformado
        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }
        if (transformedCollider != null)
        {
            transformedCollider.enabled = true;
        }

        isTransformed = true;
    }

    // Desactiva la transformaci�n
    void DeactivateTransformation()
    {
        transformedObject.SetActive(false); // Desactiva el objeto transformado
        playerModel.SetActive(true); // Activa el modelo del jugador

        // Reactiva el collider del jugador y desactiva el del objeto transformado
        if (playerCollider != null)
        {
            playerCollider.enabled = true;
        }
        if (transformedCollider != null)
        {
            transformedCollider.enabled = false;
        }

        isTransformed = false;
    }
}
