﻿using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager STATE;

    public Slider volumeSlider;
    public float volume;

    public Dropdown playerDropdown;
    public int numPlayers;

    public Dropdown enemyDropdown;
    public int computerLevel;

    public GameObject guyPrefab;

    public GameObject Machete;
    public GameObject Pistol;

    // Singleton Pattern
    void Awake()
    {
        if (STATE != null)
            GameObject.Destroy(STATE);
        else
            STATE = this;

        DontDestroyOnLoad(this);
    }

    public void FetchSettings()
    {
        volume = volumeSlider.value;
        numPlayers = int.Parse(playerDropdown.options[playerDropdown.value].text);
        computerLevel = enemyDropdown.value;
    }
}
