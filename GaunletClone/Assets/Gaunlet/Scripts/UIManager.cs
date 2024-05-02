using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Main Canvas
    public GameObject Canvas;

    public Transform LevelNumber;
    public List<UIPlayerData> playerData;

    private void Start()
    {
        AssetFinder();
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
                    {
                        // Loop through each player card
                        foreach(Transform playerCards in panelContent)
                        {
                            // Loop through each card Content
                            foreach(Transform playerCardsContent in playerCards)
                            {
                                // Note: Iterate through each Player Card and add the values accordingly.

                                if(playerCardsContent.name == "HealthNum")
                                {

                                }
                            }
                        }
                    }
                }
            }
        }
    }

    [System.Serializable]
    public struct UIPlayerData
    {
        public Transform health;
        public Transform score;
        public Transform name;
        public Transform insertCoin;
    }
}
