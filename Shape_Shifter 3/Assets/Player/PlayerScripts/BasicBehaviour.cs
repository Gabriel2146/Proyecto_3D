using UnityEngine;
using System.Collections.Generic;

// Esta clase gestiona qué comportamiento del jugador está activo o sobrescrito, y llama a sus funciones locales.
// Contiene la configuración básica y funciones comunes usadas por todos los comportamientos del jugador.
public class BasicBehaviour : MonoBehaviour
{
    public Transform playerCamera;                        // Referencia a la cámara que sigue al jugador.
    public float turnSmoothing = 0.06f;                   // Velocidad de giro al mover al jugador para que coincida con la orientación de la cámara.
    public float sprintFOV = 100f;                        // FOV de la cámara cuando el jugador está corriendo.
    public string sprintButton = "Sprint";                // Nombre del botón por defecto para correr.

    private float h;                                      // Eje horizontal.
    private float v;                                      // Eje vertical.
    private int currentBehaviour;                         // Referencia al comportamiento actual del jugador.
    private int defaultBehaviour;                         // Comportamiento por defecto del jugador cuando ningún otro está activo.
    private int behaviourLocked;                          // Referencia al comportamiento temporalmente bloqueado que impide que se sobrescriba.
    private Vector3 lastDirection;                        // Última dirección en la que el jugador estaba moviéndose.
    private Animator anim;                                // Referencia al componente Animator.
    private ThirdPersonOrbitCam camScript;                // Referencia al script de la cámara en tercera persona.
    private bool sprint;                                  // Booleano para determinar si el jugador ha activado el modo de sprint.
    private bool changedFOV;                              // Booleano para almacenar si la acción de sprint ha cambiado el FOV de la cámara.
    private int hFloat;                                   // Variable del Animator relacionada con el eje horizontal.
    private int vFloat;                                   // Variable del Animator relacionada con el eje vertical.
    private List<GenericBehaviour> behaviours;            // Lista que contiene todos los comportamientos habilitados del jugador.
    private List<GenericBehaviour> overridingBehaviours;  // Lista de comportamientos que están sobrescribiendo el comportamiento activo.
    private Rigidbody rBody;                              // Referencia al cuerpo rígido del jugador.
    private int groundedBool;                             // Variable del Animator relacionada con si el jugador está en el suelo.
    private Vector3 colExtents;                           // Extensiones del colisionador para la verificación de suelo.

    // Obtener los ejes horizontal y vertical actuales.
    public float GetH { get { return h; } }
    public float GetV { get { return v; } }

    // Obtener el script de la cámara del jugador.
    public ThirdPersonOrbitCam GetCamScript { get { return camScript; } }

    // Obtener el cuerpo rígido del jugador.
    public Rigidbody GetRigidBody { get { return rBody; } }

    // Obtener el controlador del Animator del jugador.
    public Animator GetAnim { get { return anim; } }

    // Obtener el comportamiento por defecto.
    public int GetDefaultBehaviour { get { return defaultBehaviour; } }

    void Awake()
    {
        // Configurar las referencias.
        behaviours = new List<GenericBehaviour>();
        overridingBehaviours = new List<GenericBehaviour>();
        anim = GetComponent<Animator>();
        hFloat = Animator.StringToHash("H");
        vFloat = Animator.StringToHash("V");
        camScript = playerCamera.GetComponent<ThirdPersonOrbitCam>();
        rBody = GetComponent<Rigidbody>();

        // Variables de verificación de contacto con el suelo.
        groundedBool = Animator.StringToHash("Grounded");
        colExtents = GetComponent<Collider>().bounds.extents;
    }

    void Update()
    {
        // Almacenar los ejes de entrada.
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        // Establecer los ejes de entrada en el Animator Controller.
        anim.SetFloat(hFloat, h, 0.1f, Time.deltaTime);
        anim.SetFloat(vFloat, v, 0.1f, Time.deltaTime);

        // Activar o desactivar sprint con la entrada.
        sprint = Input.GetButton(sprintButton);

        // Establecer el FOV correcto de la cámara cuando el jugador está corriendo.
        if (IsSprinting())
        {
            changedFOV = true;
            camScript.SetFOV(sprintFOV);
        }
        else if (changedFOV)
        {
            camScript.ResetFOV();
            changedFOV = false;
        }
        // Establecer la verificación de estar en el suelo en el Animator Controller.
        anim.SetBool(groundedBool, IsGrounded());
    }

    // Llamar a las funciones FixedUpdate de los comportamientos activos o sobrescritos.
    void FixedUpdate()
    {
        // Llamar al comportamiento activo si no hay ninguno sobrescrito.
        bool isAnyBehaviourActive = false;
        if (behaviourLocked > 0 || overridingBehaviours.Count == 0)
        {
            foreach (GenericBehaviour behaviour in behaviours)
            {
                if (behaviour.isActiveAndEnabled && currentBehaviour == behaviour.GetBehaviourCode())
                {
                    isAnyBehaviourActive = true;
                    behaviour.LocalFixedUpdate();
                }
            }
        }
        // Llamar a los comportamientos sobrescritos si los hay.
        else
        {
            foreach (GenericBehaviour behaviour in overridingBehaviours)
            {
                behaviour.LocalFixedUpdate();
            }
        }

        // Asegurarse de que el jugador esté de pie en el suelo si no hay ningún comportamiento activo o sobrescrito.
        if (!isAnyBehaviourActive && overridingBehaviours.Count == 0)
        {
            rBody.useGravity = true;
            Repositioning();
        }
    }

    // Llamar a las funciones LateUpdate de los comportamientos activos o sobrescritos.
    private void LateUpdate()
    {
        // Llamar al comportamiento activo si no hay ninguno sobrescrito.
        if (behaviourLocked > 0 || overridingBehaviours.Count == 0)
        {
            foreach (GenericBehaviour behaviour in behaviours)
            {
                if (behaviour.isActiveAndEnabled && currentBehaviour == behaviour.GetBehaviourCode())
                {
                    behaviour.LocalLateUpdate();
                }
            }
        }
        // Llamar a los comportamientos sobrescritos si los hay.
        else
        {
            foreach (GenericBehaviour behaviour in overridingBehaviours)
            {
                behaviour.LocalLateUpdate();
            }
        }
    }

    // Suscribir un nuevo comportamiento a la lista de observación.
    public void SubscribeBehaviour(GenericBehaviour behaviour)
    {
        behaviours.Add(behaviour);
    }

    // Establecer el comportamiento por defecto del jugador.
    public void RegisterDefaultBehaviour(int behaviourCode)
    {
        defaultBehaviour = behaviourCode;
        currentBehaviour = behaviourCode;
    }

    // Intentar establecer un comportamiento personalizado como el activo.
    // Siempre cambia del comportamiento por defecto al que se pasa.
    public void RegisterBehaviour(int behaviourCode)
    {
        if (currentBehaviour == defaultBehaviour)
        {
            currentBehaviour = behaviourCode;
        }
    }

    // Intentar desactivar un comportamiento del jugador y volver al comportamiento por defecto.
    public void UnregisterBehaviour(int behaviourCode)
    {
        if (currentBehaviour == behaviourCode)
        {
            currentBehaviour = defaultBehaviour;
        }
    }

    // Intentar sobrescribir cualquier comportamiento activo con los comportamientos de la cola.
    // Usar para cambiar a uno o más comportamientos que deben solaparse con el activo (por ejemplo, comportamiento de apuntado).
    public bool OverrideWithBehaviour(GenericBehaviour behaviour)
    {
        // El comportamiento no está en la cola.
        if (!overridingBehaviours.Contains(behaviour))
        {
            // Ningún comportamiento está siendo sobrescrito.
            if (overridingBehaviours.Count == 0)
            {
                // Llamar a la función OnOverride del comportamiento activo antes de sobrescribirlo.
                foreach (GenericBehaviour overriddenBehaviour in behaviours)
                {
                    if (overriddenBehaviour.isActiveAndEnabled && currentBehaviour == overriddenBehaviour.GetBehaviourCode())
                    {
                        overriddenBehaviour.OnOverride();
                        break;
                    }
                }
            }
            // Añadir el comportamiento sobrescrito a la cola.
            overridingBehaviours.Add(behaviour);
            return true;
        }
        return false;
    }

    // Intentar revocar el comportamiento sobrescrito y volver al comportamiento activo.
    // Llamado cuando se sale del comportamiento sobrescrito (por ejemplo, al dejar de apuntar).
    public bool RevokeOverridingBehaviour(GenericBehaviour behaviour)
    {
        if (overridingBehaviours.Contains(behaviour))
        {
            overridingBehaviours.Remove(behaviour);
            return true;
        }
        return false;
    }

    // Comprobar si cualquier comportamiento o un comportamiento específico está sobrescribiendo el activo.
    public bool IsOverriding(GenericBehaviour behaviour = null)
    {
        if (behaviour == null)
            return overridingBehaviours.Count > 0;
        return overridingBehaviours.Contains(behaviour);
    }

    // Comprobar si el comportamiento activo es el que se pasa.
    public bool IsCurrentBehaviour(int behaviourCode)
    {
        return this.currentBehaviour == behaviourCode;
    }

    // Comprobar si algún comportamiento está bloqueado temporalmente.
    public bool GetTempLockStatus(int behaviourCodeIgnoreSelf = 0)
    {
        return (behaviourLocked != 0 && behaviourLocked != behaviourCodeIgnoreSelf);
    }

    // Intentar bloquear un comportamiento específico temporalmente.
    // Ningún otro comportamiento puede sobrescribir mientras el bloqueo esté activo.
    public void LockTempBehaviour(int behaviourCode)
    {
        if (behaviourLocked == 0)
        {
            behaviourLocked = behaviourCode;
        }
    }

    // Desbloquear un comportamiento previamente bloqueado.
    public void UnlockTempBehaviour()
    {
        behaviourLocked = 0;
    }

    // Realizar la verificación de estar en el suelo.
    private bool IsGrounded()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up;
        float range = colExtents.y + 0.1f;
        if (Physics.Raycast(origin, Vector3.down, out hit, range))
        {
            return true;
        }
        return false;
    }

    // Función para reposicionar al jugador si está caído.
    private void Repositioning()
    {
        if (!IsGrounded())
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }

    // Comprobar si el jugador está corriendo.
    private bool IsSprinting()
    {
        return sprint && (h > 0 || v > 0);
    }
}
