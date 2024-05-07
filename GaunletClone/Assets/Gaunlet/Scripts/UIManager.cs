using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Main Canvas
    public GameObject Canvas;

    public Transform LevelNumber;
    public Transform playerCards;
    public List<UIPlayerData> playerData;

    public Players warrior;

    private void Awake()
    {
        playerData = new List<UIPlayerData>();
        AssetFinder();
    }

    private void OnGUI()
    {
        if(GUILayout.Button("Add Warrior"))
        {
            spawnPlayer(warrior);
        }
    }

    private void AssetFinder()
    {
        // Iterate through all the panels in the canvas
        foreach(Transform gamePanel in Canvas.transform)
        {
            // Find  the Game Panel
            if (gamePanel.gameObject.name == "Game")
            {
                // Iterate through the panels contents
                foreach (Transform panelContent in gamePanel)
                {
                    // Find the Level Number
                    if (panelContent.name == "LevelNum")
                        LevelNumber = panelContent;
                    // Find The Player Cards
                    if(panelContent.name == "PlayerCards")
                        playerCards = panelContent;
                }
            }
        }

        // UI Data
        Transform currentName = null;
        Transform currentHealth = null;
        Transform currentScore = null;
        Transform currentInsertCoin = null;

        int k = 0;
        // Iterate through every card
        foreach (Transform card in playerCards)
        {
            card.name = "Player " + k + " card";
            // Iterate through every card content
            foreach(Transform cardInfo in card)
            {
                if (cardInfo.name == "TypeName")
                    currentName = cardInfo;
                if (cardInfo.name == "HealthNum")
                    currentHealth = cardInfo;
                if (cardInfo.name == "ScoreNum")
                    currentScore = cardInfo;
                if (cardInfo.name == "InsertCoinText")
                    currentInsertCoin = cardInfo;
            }
            playerData.Add(new UIPlayerData(currentHealth, currentScore, currentName, currentInsertCoin, false));

            k++;
        }
    }

    // Methods
    public void spawnPlayer(Players player)
    {
        // Check if list is empty
        bool listEmpty = true;

        int i = 0;
        foreach (UIPlayerData data in playerData)
        {
            if (data.used == true)
            {
                listEmpty = false;
                break;
            }

            i++;
        }

        //List empty set to first slot.
        if (listEmpty == false)
        {
            i = 0;
            print("List is empty");
        }

        print(i);

        playerData[i].used = true;
        playerData[i].health.GetComponent<TextMeshProUGUI>().text = player.health.ToString();
        playerData[i].score.GetComponent<TextMeshProUGUI>().text = 0.ToString();
        playerData[i].name.GetComponent<TextMeshProUGUI>().text = player.name;
        playerData[i].insertCoin.gameObject.SetActive(false);
        setColor(playerData[i], player);
    }

    private void setColor(UIPlayerData data, Players player)
    {
        data.health.GetComponent<TextMeshProUGUI>().color = player.UIColor;
        data.score.GetComponent<TextMeshProUGUI>().color = player.UIColor;
        data.name.GetComponent<TextMeshProUGUI>().color = player.UIColor;
        data.insertCoin.GetComponent<TextMeshProUGUI>().color = player.UIColor;
    }

    private void cleanColor(UIPlayerData data)
    {
        data.health.GetComponent<TextMeshProUGUI>().color = Color.white;
        data.score.GetComponent<TextMeshProUGUI>().color = Color.white;
        data.name.GetComponent<TextMeshProUGUI>().color = Color.white;
        data.insertCoin.GetComponent<TextMeshProUGUI>().color = Color.white;
    }

    [System.Serializable]
    public class UIPlayerData
    {
        public Transform health;
        public Transform score;
        public Transform name;
        public Transform insertCoin;
        public bool used;

        public UIPlayerData(Transform health, Transform score, Transform name, Transform insertCoin, bool used)
        {
            this.health = health;
            this.score = score;
            this.name = name;
            this.insertCoin = insertCoin;
            this.used = used;
        }
    }
}
