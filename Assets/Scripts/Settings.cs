using UnityEngine.UI;

public static class Settings
{
    public static Slider volumeSlider;
    public static float volume;

    public static Dropdown playerDropdown;
    public static int numPlayers;

    public static void Set()
    {
        SetVolume();
        SetPlayers();
    }

    public static void SetVolume()
    {
        volume = volumeSlider.value;
    }

    public static void SetPlayers()
    {
        numPlayers = int.Parse(playerDropdown.options[playerDropdown.value].text);
    }
}
