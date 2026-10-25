using UnityEngine;
using Fusion;
using System.Threading.Tasks;

public class GameLauncher : MonoBehaviour
{
    public NetworkPrefabRef playerPrefab;

    async void Start()
    {
        NetworkRunner runner = GetComponent<NetworkRunner>();
        runner.ProvideInput = true;

        // Connexion automatique à une room de test en mode Shared
        await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = "JamRoom2D",
            SceneManager = GetComponent<NetworkSceneManagerDefault>()
        });

        // Spawn du joueur local
        if (runner.IsSharedModeMasterClient || runner.IsClient)
        {
            runner.Spawn(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity, runner.LocalPlayer);
        }
    }
}