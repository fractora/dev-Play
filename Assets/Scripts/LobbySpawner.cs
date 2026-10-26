using UnityEngine;
using Fusion;

public class LobbySpawner : MonoBehaviour
{
    [SerializeField] private NetworkObject lobbyManagerPrefab;

    private void Start()
    {
        // On récupère le NetworkRunner persistant (créé dans le MainMenu)
        NetworkRunner runner = FindFirstObjectByType<NetworkRunner>();

        if (runner != null && runner.IsServer) // En Shared Mode, l'hôte est considéré comme Master/Server
        {
            // Seul l'hôte spawn l'objet réseau du LobbyManager
            runner.Spawn(lobbyManagerPrefab);
        }
    }
}