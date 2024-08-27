using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    [SerializeField] int[] costs;
    [SerializeField] Button[] buttons;
    [SerializeField] TextMeshProUGUI[] boughtTexts;
    [SerializeField] GameObject shopPanel;

    public delegate void BuyRifle();
    public event BuyRifle buyRifle;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        /*for (int i = 0; i < buttons.Length; i++)
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
        }*/

        DeletePlayerPrefs();
        for (int i = 0; i < buttons.Length; i++) 
            PlayerPrefs.SetInt("Position" + i, 0);

        shopPanel.SetActive(false);

        Check();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            shopPanel.SetActive(!shopPanel.activeInHierarchy);
            Check();

            if (shopPanel.activeInHierarchy)
            {
                Time.timeScale = 0;
            }
            else
                Time.timeScale = 1;
        }

    }

    public void Buy(int index)
    {
        buttons[index].interactable = false;
        boughtTexts[index].text = "Sold";

        PlayerPrefs.SetInt("Position" + index, 1);

        if (index == 1)
            buyRifle.Invoke();
    }

    public void Check()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (PlayerPrefs.GetInt("Position" + i) == 1) break;

            if (PlayerMove.instance.money < costs[i])
            {
                buttons[i].interactable = false;
                boughtTexts[i].text = "Not enough money";
            }
            else
            {
                buttons[i].interactable = true;
                boughtTexts[i].text = "Buy";
            }
        }
    }

    [ContextMenu("Delete Prefs")]
    void DeletePlayerPrefs() => PlayerPrefs.DeleteAll();
}
