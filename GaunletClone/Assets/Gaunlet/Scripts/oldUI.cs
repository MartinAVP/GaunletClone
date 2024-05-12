using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class oldUI : MonoBehaviour
{
    // Main Canvas
    public GameObject Canvas;

    [Header("Prefabs")]
    [SerializeField] private GameObject playerCard;

    public Transform LevelNumber;
    public Transform playerCards;
    [SerializeField] public List<UIPlayerData> playerData;

    public Players warrior;
    public Players valkyrie;

    private void Awake()
    {
        playerData = new List<UIPlayerData>();
        AssetFinder();
    }

    private void Start()
    {
        // Find the Current Cards in the Game
        foreach (Transform children in playerCards)
        {
            AddCardInfo(children);
        }

        // Instantiate if Cards are missing.
        if (playerData.Count < 4)
        {
            GameObject tmp = Instantiate(playerCard);
            tmp.transform.parent = playerCards.transform;
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Add Warrior"))
        {
            //spawnPlayer(warrior);
            newPlayer(warrior);
        }
        if (GUILayout.Button("Add Valkyrie"))
        {
            //spawnPlayer(warrior);
            newPlayer(valkyrie);
        }
    }

    private void AssetFinder()
    {
        // Iterate through all the panels in the canvas
        foreach (Transform gamePanel in Canvas.transform)
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
                    if (panelContent.name == "PlayerCards")
                        playerCards = panelContent;
                    if (panelContent.name == "PlayerList")
                        playerCards = panelContent.GetChild(0).GetChild(0);
                }
            }
        }

        // UI Data
        Transform currentName = null;
        Transform currentHealth = null;
        Transform currentScore = null;
        Transform currentInsertCoin = null;

        Transform currentHealthTitle = null;
        Transform currentScoreTitle = null;

        int k = 0;
        // Iterate through every card
        foreach (Transform card in playerCards)
        {
            card.name = "Player " + k + " card";
            // Iterate through every card content
            foreach (Transform cardInfo in card)
            {
                if (cardInfo.name == "TypeName")
                    currentName = cardInfo;
                // Number Variables
                if (cardInfo.name == "HealthNum")
                    currentHealth = cardInfo;
                if (cardInfo.name == "ScoreNum")
                    currentScore = cardInfo;
                // Insert Coins
                if (cardInfo.name == "InsertCoinText")
                    currentInsertCoin = cardInfo;
                // Var Titles
                if (cardInfo.name == "HealthTitle")
                    currentHealthTitle = cardInfo;
                if (cardInfo.name == "ScoreTitle")
                    currentScoreTitle = cardInfo;
            }
            playerData.Add(new UIPlayerData(currentHealth, currentScore, currentName, currentInsertCoin, false, currentHealthTitle, currentScoreTitle));

            k++;
        }
    }

    private void AddCardInfo(Transform card)
    {
        Transform currentName = null;
        Transform currentHealth = null;
        Transform currentScore = null;
        Transform currentInsertCoin = null;

        Transform currentHealthTitle = null;
        Transform currentScoreTitle = null;

        card.name = "Player " + playerData.Count + " card";
        // Iterate through every card content
        foreach (Transform cardInfo in card)
        {
            if (cardInfo.name == "TypeName")
                currentName = cardInfo;
            // Number Variables
            if (cardInfo.name == "HealthNum")
                currentHealth = cardInfo;
            if (cardInfo.name == "ScoreNum")
                currentScore = cardInfo;
            // Insert Coins
            if (cardInfo.name == "InsertCoinText")
                currentInsertCoin = cardInfo;
            // Var Titles
            if (cardInfo.name == "HealthTitle")
                currentHealthTitle = cardInfo;
            if (cardInfo.name == "ScoreTitle")
                currentScoreTitle = cardInfo;
        }
        playerData.Add(new UIPlayerData(currentHealth, currentScore, currentName, currentInsertCoin, false, currentHealthTitle, currentScoreTitle));
    }

    // Methods
    public void spawnPlayer(Players player)
    {
        // Check if list is empty
        //bool spaceFound = true;

        //int i = 0;
        int foundAt = playerData.Count - 1;

        List<UIPlayerData> reversedData = playerData;
        reversedData.Reverse();

        for (int k = 0; k < playerData.Count; k++)
        {
            Debug.Log(k + " /// " + playerData[k].used);
            // Check if the Slot is being used
            if (playerData[k].used == true)
            {
                // Check if the slot used is the last slot in availavle
                if (k < playerData.Count)
                {
                    foundAt = k + 1;
                    Debug.Log("Found Space At" + (k + 1));
                    break;
                }
                else
                {
                    Debug.LogWarning("List is Full");
                    break;
                }
            }

            //i++;
        }

        /*        for (int k = playerData.Count; k > 0; k--)
                {
                    Debug.Log(k + " /// " + playerData[k].used);
                    // Check if the Slot is being used
                    if (playerData[k].used == true)
                    {
                        // Check if the slot used is the last slot in availavle
                        if (k + 1 < playerData.Count)
                        {
                            foundAt = k + 1;
                            Debug.Log("Found Space At");
                            break;
                        }
                        else
                        {
                            Debug.LogWarning("List is Full");
                            break;
                        }
                    }
                }*/

        //List empty set to first slot.
        /*        if (spaceFound == true)
                {
                    i = 0;
                    print("List is empty");
                }*/

        //print(foundAt);

        playerData[foundAt].used = true;
        //print(playerData[i].used);

        playerData[foundAt].health.GetComponent<TextMeshProUGUI>().text = player.health.ToString();
        playerData[foundAt].score.GetComponent<TextMeshProUGUI>().text = 0.ToString();
        playerData[foundAt].name.GetComponent<TextMeshProUGUI>().text = player.name;
        playerData[foundAt].insertCoin.gameObject.SetActive(false);
        setColor(playerData[foundAt], player);
    }

    List<bool> slotUsed = new List<bool>();
    private void newPlayer(Players player)
    {
        if (slotUsed.Count < 4)
        {
            int foundAt = slotUsed.Count;
            playerData[foundAt].used = true;
            //print(playerData[i].used);

            playerData[foundAt].health.GetComponent<TextMeshProUGUI>().text = player.health.ToString();
            playerData[foundAt].score.GetComponent<TextMeshProUGUI>().text = 0.ToString();
            playerData[foundAt].name.GetComponent<TextMeshProUGUI>().text = player.name;
            playerData[foundAt].insertCoin.gameObject.SetActive(false);
            playerData[foundAt].type = player.type;
            setColor(playerData[foundAt], player);

            slotUsed.Add(true);
        }
    }

    private void removePlayer(Players player)
    {
        int i = 0;
        foreach (UIPlayerData data in playerData)
        {
            if (data.type == player.type)
            {


                slotUsed.Remove(data.used);
            }

            i++;
        }
    }

    private void setColor(UIPlayerData data, Players player)
    {
        data.health.GetComponent<TextMeshProUGUI>().color = player.UIColor;
        data.score.GetComponent<TextMeshProUGUI>().color = player.UIColor;
        data.name.GetComponent<TextMeshProUGUI>().color = player.UIColor;
        data.insertCoin.GetComponent<TextMeshProUGUI>().color = player.UIColor;
        data.scoreTitle.GetComponent<TextMeshProUGUI>().color = player.UIColor;
        data.healthTitle.GetComponent<TextMeshProUGUI>().color = player.UIColor;
    }

    private void cleanColor(UIPlayerData data)
    {
        data.health.GetComponent<TextMeshProUGUI>().color = Color.white;
        data.score.GetComponent<TextMeshProUGUI>().color = Color.white;
        data.name.GetComponent<TextMeshProUGUI>().color = Color.white;
        data.insertCoin.GetComponent<TextMeshProUGUI>().color = Color.white;
        data.scoreTitle.GetComponent<TextMeshProUGUI>().color = Color.white;
        data.healthTitle.GetComponent<TextMeshProUGUI>().color = Color.white;
    }

    [System.Serializable]
    public class UIPlayerData
    {
        public playerType type;
        public Transform health;
        public Transform score;
        public Transform name;
        public Transform insertCoin;

        public Transform healthTitle;
        public Transform scoreTitle;

        public bool used;

        public UIPlayerData(Transform health, Transform score, Transform name, Transform insertCoin, bool used, Transform healthTitle, Transform scoreTitle)
        {
            this.health = health;
            this.score = score;
            this.name = name;
            this.insertCoin = insertCoin;
            this.used = used;
            this.healthTitle = healthTitle;
            this.scoreTitle = scoreTitle;
        }
    }
}
