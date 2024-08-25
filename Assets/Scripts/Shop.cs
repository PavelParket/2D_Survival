using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] int[] costs;
    [SerializeField] Button[] buttons;
    [SerializeField] TextMeshProUGUI[] boughtTexts;

    protected PlayerMove player;

    private void Start()
    {
        player = PlayerMove.instance;

        for (int i = 0; i < buttons.Length; i++)
        {
            if (!PlayerPrefs.HasKey("Position" + i))
            {
                PlayerPrefs.SetInt("Position" + i, 0);
            }
            else
            {
                if (PlayerPrefs.GetInt("Position" + i) == 1)
                {
                    buttons[i].interactable = false;
                    boughtTexts[i].text = "Sold";
                }
            }
        }

        Check();
    }

    private void OnEnable()
    {
        Check();
    }

    public void Buy(int index)
    {
        buttons[index].interactable = false;
        boughtTexts[index].text = "Sold";

        PlayerPrefs.SetInt("Position" + index, 1);
    }

    public void Check()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (PlayerPrefs.GetInt("Position" + i) == 1) break;

                if (player.money < costs[i])
            {
                buttons[i].interactable = false;
                boughtTexts[i].text = "Not enough money";
            }
        }
    }

    [ContextMenu("Delete Prefs")]
    void DeletePlayerPrefs() => PlayerPrefs.DeleteAll();
}
