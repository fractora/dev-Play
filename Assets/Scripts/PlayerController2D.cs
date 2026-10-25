using UnityEngine;
using Fusion;

public class PlayerController2D : NetworkBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 5f;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private NetworkMecanimAnimator networkAnimator;

    private InputSystem_Actions inputActions;

    public bool canInteract = false;
    public TaskArea currentTaskArea;

    // Fonction se lance avant le lancement du jeu
    private void Awake()
    {
        // Initialisation de l'Input System
        inputActions = new InputSystem_Actions();
    }

    public override void Spawned()
    {
        //Verifie si c'est le bon joueur qui a le controle du personnage
        if (HasStateAuthority)
        {
            // Remplacez "Player" par le nom de votre Action Map (ex: Gameplay)
            inputActions.Player.Enable();
        }
    }

    //fait en sorte de faire disparaitre le bon joueur au moment de la deconnection
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        if (HasStateAuthority)
        {
            inputActions.Player.Disable();
        }
    }

    private void Update()
    {
        if (HasStateAuthority && inputActions != null)
        {
            // Recupere les valeurs de l'input du joueur pour l'interaction
            if (inputActions.Player.Interact.WasPressedThisFrame())
            {
                if (canInteract && currentTaskArea != null)
                {
                    // Appelle la fonction d'interaction dans le TaskArea
                    currentTaskArea.OnPlayerInteract(this);
                }
            }
        }

    }

    //deplacement du personnage en fonction de l'input du joueur
    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority && inputActions != null)
        {
            // Recupere les valeurs de l'input du joueur pour le deplacement
            Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

            if (inputVector.magnitude > 1f)
            {
                inputVector.Normalize();
            }

            Vector3 movement = new Vector3(inputVector.x, inputVector.y, 0f) * moveSpeed * Runner.DeltaTime;
            transform.position += movement;

            networkAnimator.Animator.SetFloat("Horizontal", inputVector.x);
            networkAnimator.Animator.SetFloat("Vertical", inputVector.y);
            networkAnimator.Animator.SetFloat("Speed", inputVector.sqrMagnitude);
        }
    }
}
