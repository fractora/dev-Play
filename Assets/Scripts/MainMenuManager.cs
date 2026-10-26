using UnityEngine;
using TMPro;
using Fusion;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Inputs")]
    public TMP_InputField roomCodeInput;  // Champ pour saisir le code
    public TMP_InputField nickNameInput;  // Champ pour le pseudo (optionnel)

    [Header("Scene Settings")]
    public string gameSceneName = "GameScene"; // Le nom exact de votre scène de jeu

    private NetworkRunner _runner;

    // --- 1. CRÉER UNE ROOM PRIVÉE (AVEC CODE) ---
    public void CreatePrivateRoom()
    {
        string roomCode = roomCodeInput.text.ToUpper(); // On met le code en majuscules

        if (string.IsNullOrEmpty(roomCode))
        {
            Debug.LogWarning("Veuillez entrer un code de session !");
            return;
        }

        StartSession(roomCode, GameMode.Shared, isVisible: false);
    }

    // --- 2. REJOINDRE UNE ROOM PRIVÉE (AVEC CODE) ---
    public void JoinPrivateRoom()
    {
        string roomCode = roomCodeInput.text.ToUpper();

        if (string.IsNullOrEmpty(roomCode))
        {
            Debug.LogWarning("Veuillez entrer le code de la session à rejoindre !");
            return;
        }

        StartSession(roomCode, GameMode.Shared, isVisible: false);
    }

    // --- 3. REJOINDRE OU CRÉER UNE ROOM PUBLIQUE ---
    public void MatchmakingPublic()
    {
        // En passant une chaîne vide ou null, Fusion cherchera une room publique disponible
        StartSession(roomName: null, GameMode.Shared, isVisible: true);
    }

    // --- MÉTHODE PRINCIPALE DE LANCEMENT ---
    private async void StartSession(string roomName, GameMode mode, bool isVisible)
    {
        // On s'assure qu'un NetworkRunner existe
        if (_runner == null)
        {
            GameObject runnerObject = new GameObject("NetworkRunner");

            _runner = runnerObject.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true;
            DontDestroyOnLoad(runnerObject);
        }

        int sceneIndex = SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/LobbyScene.unity");

        // Configuration des paramètres de la partie
        var startGameArgs = new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomName, // Si null/vide, Fusion génère un nom automatique
            Scene = SceneRef.FromIndex(sceneIndex),
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            IsOpen = true,
            IsVisible = isVisible // True = Publique, False = Privée (accessible uniquement via le code exact)
        };

        // Lancement de la session
        var result = await _runner.StartGame(startGameArgs);

        if (result.Ok)
        {
            Debug.Log("Connexion réussie à la session !");
        }
        else
        {
            Debug.LogError($"Échec de connexion : {result.ShutdownReason}");
        }
    }
}