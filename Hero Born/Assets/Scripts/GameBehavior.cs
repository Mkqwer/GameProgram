using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CustomExtensions;

public class GameBehavior : MonoBehaviour, IManager
{
    private string _state;

    public string State
    {
        get { return _state; }
        set { _state = value; }
    }



    private int ItemCollected = 1;
    private int PlayerHp = 2;
    public int MaxItems = 4;
    public Button WinButton;
    public Button LossButton;

    public TMP_Text HpText;
    public TMP_Text ItemsText;
    public TMP_Text ProgressText;

    void Start()
    {
        ItemsText.text += ItemCollected;
        HpText.text += PlayerHp;
        Initialize();
    }

    public void Initialize()
    {
        _state = "Game Manager initialized...";
        _state.FancyDebug();
        Debug.Log(_state);
    }

    public void UpdateScene(string upodateText)
    {
        HpText.text = "Hp: " + PlayerHp;
        ItemsText.text = "Items: " + ItemCollected;
    }

    public int Items
    {
        get { return ItemCollected; }
        set
        {
            ItemCollected = value;
            ItemsText.text = "Items: " + ItemCollected;
            if (ItemCollected >= MaxItems)
            {
                ProgressText.text = "You found all items!";
                WinButton.gameObject.SetActive(true);


                UpdateScene("You found all items!");
                Time.timeScale = 0f;
            }
            else
            {
                ProgressText.text = "Items found, only " + (MaxItems - ItemCollected) + " more to go!";
            }
        }
    }
    public int Hp
    {
        get { return PlayerHp; }
        set
        {
            PlayerHp = value;
            Debug.Log($"Hp : {PlayerHp}");
            if (PlayerHp <= 0)
            {
                ProgressText.text = "You have lost all your health!";
                LossButton.gameObject.SetActive(true);

                UpdateScene("You want another life with that?");
                Time.timeScale = 0f;
            }
            else
            {
                ProgressText.text = "Ouch... that's got to hurt!";
            }
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1f;
    }

  
}
