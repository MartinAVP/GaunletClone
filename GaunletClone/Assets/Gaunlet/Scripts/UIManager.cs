using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Main Canvas")]
    // Main Canvas
    [SerializeField]private GameObject canvas;

    // UI Variables
    private Transform levelNumber;
    private Transform playerCards;

    // PlayerData
    public List<UICardData> playerData;

/*    // Test Variables
    public Players warrior;
    public Players valkyrie;
    public Players elf;
    public Players wizard;*/

    [Header("Prefabs")]
    // UI Prefabs
    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private GameObject potionPrefab;

    private void Awake()
    {
        AssetFinder();
    }
    void Start()
    {
        InitPlayerCards();
    }

/*    // OnGUI Test Buttons
       private void OnGUI()
        {
            if (GUILayout.Button("Add Warrior"))
            {
                //spawnPlayer(warrior);
                AddPlayer(warrior);
            }
            if (GUILayout.Button("Add Valkyrie"))
            {
                //spawnPlayer(warrior);
                AddPlayer(valkyrie);
            }
            if (GUILayout.Button("Add Elf"))
            {
                //spawnPlayer(warrior);
                AddPlayer(elf);
            }
            if (GUILayout.Button("Add Wizard"))
            {
                //spawnPlayer(warrior);
                AddPlayer(wizard);
            }
            if (GUILayout.Button("Remove Warrior"))
            {
                //spawnPlayer(warrior);
                RemovePlayer(warrior);
            }
            if (GUILayout.Button("Remove Valkyrie"))
            {
                //spawnPlayer(warrior);
                RemovePlayer(valkyrie);
            }
            if (GUILayout.Button("Remove Elf"))
            {
                //spawnPlayer(warrior);
                RemovePlayer(elf);
            }
            if (GUILayout.Button("Remove Wizard"))
            {
                //spawnPlayer(warrior);
                RemovePlayer(wizard);
            }

            if (GUILayout.Button("Add Potion"))
            {
                //spawnPlayer(warrior);
                AddPotion(wizard);
            }
            if (GUILayout.Button("Remove Potion"))
            {
                //spawnPlayer(warrior);
                RemovePotion(wizard);
            }

            if (GUILayout.Button("Add Keys"))
            {
                //spawnPlayer(warrior);
                AddKeys(wizard);
            }
            if (GUILayout.Button("Remove Keys"))
            {
                //spawnPlayer(warrior);
                RemoveKeys(wizard);
            }
            if (GUILayout.Button("Multiplier 0"))
            {
                //spawnPlayer(warrior);
                changeMultiplier(wizard, 0);
            }
            if (GUILayout.Button("Multiplier 5"))
            {
                //spawnPlayer(warrior);
                changeMultiplier(wizard, 5);
            }
        }
    */

    // External Methods
    public void AddPlayer(Players player)
    {
        int spaceFound = -1;

        for (int i = 0; i < playerData.Count; i++)
        {
            if (playerData[i].player == null)
            {
                spaceFound = i;

                playerData[i].player = player;
                playerData[i].name.GetComponent<TextMeshProUGUI>().text = player.name;

                playerData[i].health.GetComponent<TextMeshProUGUI>().text = player.health.ToString();
                playerData[i].score.GetComponent<TextMeshProUGUI>().text = player.score.ToString();

                playerData[i].insertCoin.gameObject.SetActive(false);

                playerData[i].keysContent.gameObject.SetActive(true);
                playerData[i].potionContent.gameObject.SetActive(true);

                SetUIColor(playerData[i], player);
                break;
            }
        }

        if (spaceFound == -1) { Debug.LogError("All Players are already joined, can't add more!"); return; }

        //print("Space found at " + spaceFound);
    }
    public void RemovePlayer(Players player)
    {
        for (int i = (playerData.Count - 1); i >= 0; i--)
        {
            if (playerData[i].player == player)
            {
                // Remove the Player being held
                playerData[i].player = null;

                // Set the texts to default state
                playerData[i].name.GetComponent<TextMeshProUGUI>().text = "Slot Available";

                playerData[i].health.GetComponent<TextMeshProUGUI>().text = "0";
                playerData[i].score.GetComponent<TextMeshProUGUI>().text = "0";

                // Clear the multiplier
                playerData[i].multiplier.GetComponent<TextMeshProUGUI>().text = "";

                // Show the Insert Coin Text
                playerData[i].insertCoin.gameObject.SetActive(true);

                // clear the keys and potions lists
                playerData[i].keys.Clear();
                playerData[i].potions.Clear();

                // Remove all the keys and potions on screen
                RemoveAllChildren(playerData[i].keysContent.GetChild(0).GetChild(0));
                RemoveAllChildren(playerData[i].potionContent.GetChild(0).GetChild(0));

                // Hide the lists
                playerData[i].keysContent.gameObject.SetActive(false);
                playerData[i].potionContent.gameObject.SetActive(false);

                // Make the color white
                CleanUIColor(playerData[i]);
                break;
            }
        }
    }
    public void setLevel(int level)
    {
        levelNumber.GetComponent<TextMeshProUGUI>().text = level.ToString();
    }
    public void AddScore(Players player, int score)
    {
        int id = FindPlayer(player);
        if (id == -1) { Debug.LogError("Couldn't find player"); return; }
        /*        for (int i = 0; i < playerData.Length; i++)
                {
                    if (playerData[i].player == player)
                    {
                        playerData[i].score.GetComponent<TextMeshProUGUI>().text = score.ToString();
                    }
                }*/

        playerData[id].score.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }
    public void changeMultiplier(Players player, int multiplier)
    {
        int id = FindPlayer(player);
        if (id == -1) { Debug.LogError("Couldn't find player"); return; }

        if(multiplier == 0)
        {
            playerData[id].multiplier.GetComponent<TextMeshProUGUI>().text = "";
        }
        else
        {
            playerData[id].multiplier.GetComponent<TextMeshProUGUI>().text = multiplier + "x";
        }
    }
    public void AddPotion(Players player)
    {
        int id = FindPlayer(player);
        if (id == -1) { Debug.LogError("Couldn't find player"); return; }

        GameObject tmp = Instantiate(potionPrefab);
        tmp.transform.SetParent(playerData[id].potionContent.GetChild(0).GetChild(0).transform);
        tmp.transform.localScale = new Vector3(-1, 1 ,1);
        playerData[id].potions.Add(tmp);
    }
    public void AddKeys(Players player)
    {
        /*        for (int i = 0; i < playerData.Length; i++)
                {
                    if (playerData[i].player == player)
                    {
                        GameObject tmp = Instantiate(potionPrefab);
                        tmp.transform.parent = playerData[i].keysContent.GetChild(0).GetChild(0).transform;
                        playerData[i].potions.Add(tmp);
                    }
                }*/
        int id = FindPlayer(player);
        if (id == -1) { Debug.LogError("Couldn't find player"); return; }

        GameObject tmp = Instantiate(keyPrefab);
        //tmp.transform.parent = playerData[id].keysContent.GetChild(0).GetChild(0).transform;
        tmp.transform.SetParent(playerData[id].keysContent.GetChild(0).GetChild(0).transform);
        tmp.transform.localScale = Vector3.one;
        playerData[id].keys.Add(tmp);
    }
    public void RemovePotion(Players player)
    {
/*        for (int i = 0; i < playerData.Length; i++)
        {
            if (playerData[i].player == player)
            {
                playerData[i].potions.RemoveAt(0);
            }
        }
*/
        int id = FindPlayer(player);
        if (id == -1) { Debug.LogError("Couldn't find player"); return; }
        if (playerData[id].potions.Count == 0) { Debug.LogWarning("No more potions"); return; }

        Destroy(playerData[id].potions[0]);
        playerData[id].potions.RemoveAt(0);
    }
    public void RemoveKeys(Players player)
    {
        /*        for (int i = 0; i < playerData.Length; i++)
                {
                    if (playerData[i].player == player)
                    {
                        playerData[i].potions.RemoveAt(0);
                    }
                }
        */
        int id = FindPlayer(player);
        if (id == -1) { Debug.LogError("Couldn't find player"); return; }
        if (playerData[id].keys.Count == 0) { Debug.LogWarning("No more keys"); return; }

        Destroy(playerData[id].keys[0]);
        playerData[id].keys.RemoveAt(0);
    }

    // Internal Methods
    private void SetUIColor(UICardData data, Players player)
    {
        data.health.GetComponent<TextMeshProUGUI>().color = player.UIColor;
        data.score.GetComponent<TextMeshProUGUI>().color = player.UIColor;
        data.name.GetComponent<TextMeshProUGUI>().color = player.UIColor;
        data.insertCoin.GetComponent<TextMeshProUGUI>().color = player.UIColor;
        data.scoreTitle.GetComponent<TextMeshProUGUI>().color = player.UIColor;
        data.healthTitle.GetComponent<TextMeshProUGUI>().color = player.UIColor;
    }
    private void CleanUIColor(UICardData data)
    {
        data.health.GetComponent<TextMeshProUGUI>().color = Color.white;
        data.score.GetComponent<TextMeshProUGUI>().color = Color.white;
        data.name.GetComponent<TextMeshProUGUI>().color = Color.white;
        data.insertCoin.GetComponent<TextMeshProUGUI>().color = Color.white;
        data.scoreTitle.GetComponent<TextMeshProUGUI>().color = Color.white;
        data.healthTitle.GetComponent<TextMeshProUGUI>().color = Color.white;
    }
    private void RemoveAllChildren(Transform parent)
    {
        foreach(Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
    private int FindPlayer(Players player)
    {
        for (int i = 0; i < playerData.Count; i++)
        {
            if (playerData[i].player == player)
            {
                return i;
            }
        }
        return -1;
    }
    private void AssetFinder()
    {
        // Iterate through all the panels in the canvas
        foreach (Transform gamePanel in canvas.transform)
        {
            // Find  the Game Panel
            if (gamePanel.gameObject.name == "Game")
            {
                // Iterate through the panels contents
                foreach (Transform panelContent in gamePanel)
                {
                    // Find the Level Number
                    if (panelContent.name == "LevelNum")
                        levelNumber = panelContent;
                    // Find The Player Cards
                    if (panelContent.name == "PlayerList")
                        playerCards = panelContent.GetChild(0).GetChild(0);
                }
            }
        }
    }
    private void InitPlayerCards()
    {
        playerData = new List<UICardData>(4);

        //print(playerData[0].name.ToString());
        for (int i = 0; i < playerData.Count; i++)
        {
            //playerData[i].id = i;

            playerData[i].card = playerCards.GetChild(i).transform;

            foreach (Transform cardInfo in playerData[i].card)
            {
                // General Variables
                if (cardInfo.name == "TypeName")
                    playerData[i].name = cardInfo;
                if (cardInfo.name == "Multiplier")
                    playerData[i].multiplier = cardInfo;
                if (cardInfo.name == "InsertCoinText")
                    playerData[i].insertCoin = cardInfo;
                // Number Variables
                if (cardInfo.name == "HealthNum")
                    playerData[i].health = cardInfo;
                if (cardInfo.name == "ScoreNum")
                    playerData[i].score = cardInfo;
                // Var Titles
                if (cardInfo.name == "HealthTitle")
                    playerData[i].healthTitle = cardInfo;
                if (cardInfo.name == "ScoreTitle")
                    playerData[i].scoreTitle = cardInfo;
                // Lists
                if (cardInfo.name == "KeyList")
                    playerData[i].keysContent = cardInfo;
                if (cardInfo.name == "PotionList")
                    playerData[i].potionContent = cardInfo;
            }
        }
    }

    //[System.Serializable]
    public class UICardData
    {
        public int id = -1;
        public Players player;
        public Transform card;

        // General Variables
        public Transform name;
        public Transform insertCoin;
        public Transform multiplier;

        // Num Variable
        public Transform health;
        public Transform score;

        // Titles
        public Transform healthTitle;
        public Transform scoreTitle;

        // Find Lists
        public Transform keysContent;
        public Transform potionContent;

        public List<GameObject> keys;
        public List<GameObject> potions;
    }
}
