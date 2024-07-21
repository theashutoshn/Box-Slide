using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI movesText;
    private int movesRemaining = 10;

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void DecrementMoves()
    {
        if (movesRemaining > 0)
        {
            movesRemaining--;
            UpdateMovesText();
        }

        if (movesRemaining <= 0)
        {
            // Game over logic here
            Debug.Log("Game Over: No moves remaining!");
            // You might want to show a game over screen or restart the level
        }
    }

    private void UpdateMovesText()
    {
        if (movesText != null)
        {
            movesText.text = movesRemaining + " Moves";
        }
        else
        {
            Debug.LogError("Moves text not assigned in UIManager");
        }
    }

    
}
