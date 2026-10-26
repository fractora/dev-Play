using UnityEngine;
using TMPro;
using Fusion;
using UnityEngine.SceneManagement;
using System.Linq;

public class LobbyManager : NetworkBehaviour
{
    [Header("UI References (À placer sur le Canvas de la scène)")]
    private GameObject hostStartButton;
    private TMP_Text playerCounterText;

    [Header("Scenes")]
    [SerializeField] private string gameSceneName = "GameScene";

    public override void Spawned()
    {
        // Recherche des éléments UI dans la scène si non assignés
        FindUIElements();

        // 1. Gestion de la visibilité du bouton
        if (hostStartButton != null)
        {
            // Seul l'hôte (Master Client / State Authority) voit le bouton
            hostStartButton.SetActive(HasStateAuthority);
        }

        UpdatePlayerCount();
    }

    private void FindUIElements()
    {
        // Recherche automatique du texte du compteur
        if (playerCounterText == null)
        {
            GameObject textObj = GameObject.FindWithTag("LobbyPlayerCountText"); // Ajoutez le Tag sur votre texte UI ou cherchez par nom
            if (textObj != null) playerCounterText = textObj.GetComponent<TMP_Text>();
        }

        // Recherche du bouton
        if (hostStartButton == null)
        {
            GameObject btnObj = GameObject.FindWithTag("LobbyStartButton"); // Ajoutez le Tag sur votre bouton UI
            if (btnObj != null) hostStartButton = btnObj;
        }
    }

    public override void Render()
    {
        UpdatePlayerCount();
    }

    private void UpdatePlayerCount()
    {
        if (Runner != null && Runner.ActivePlayers != null)
        {
            int playerCount = Runner.ActivePlayers.Count();

            if (playerCounterText != null)
            {
                playerCounterText.text = $"Joueurs dans le lobby : {playerCount}";
            }
        }
    }

    // --- APPELÉ PAR LE BOUTON DE L'HÔTE ---
    public void StartGameForAll()
    {
        if (HasStateAuthority && Runner != null)
        {
            int sceneIndex = SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/" + gameSceneName + ".unity");

            if (sceneIndex >= 0)
            {
                Runner.LoadScene(SceneRef.FromIndex(sceneIndex));
            }
            else
            {
                Debug.LogError($"Scène '{gameSceneName}' introuvable dans les Build Settings !");
            }
        }
    }
}