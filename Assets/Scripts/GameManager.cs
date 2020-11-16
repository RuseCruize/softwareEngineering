using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager STATE;

    public Slider volumeSlider;
    public float volume;

    public Dropdown playerDropdown;
    public int numPlayers;

    public Dropdown rightDropdown;
    public Dropdown leftDropdown;
    public int rightPlayerLevels;
    public int leftPlayerLevels;

    public GameObject guyPrefab;

    public GameObject Machete;
    public GameObject Pistol;
    public GameObject Bullet;
    public GameObject MacheteFlash;

    public int MacheteDamage;
    public int BulletDamage;

    public enum WeaponType
    {
        Unarmed,
        Machete,
        Pistol
    }

    // Singleton Pattern
    void Awake()
    {
        if (STATE != null && STATE != this)
            GameObject.Destroy(STATE);
        else
            STATE = this;

        DontDestroyOnLoad(this);
    }

    public void FetchSettings()
    {
        volume = volumeSlider.value;
        numPlayers = int.Parse(playerDropdown.options[playerDropdown.value].text);
        // Debug.Log(rightDropdown.value + " " + leftDropdown.value);
        rightPlayerLevels = rightDropdown.value;
        leftPlayerLevels = leftDropdown.value;
    }
}
