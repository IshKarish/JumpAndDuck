using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] private Image skinImage;
    [SerializeField] private Sprite defaultSkin;

    [SerializeField] private TextMeshProUGUI controlsText;
    
    private void Awake()
    {
        Sprite loadedSkin = SkinsManager.LoadSkinSprite();
        
        if (loadedSkin) skinImage.sprite = loadedSkin;
        else skinImage.sprite = defaultSkin;
    }

    public void ChangeControls()
    {
        if (controlsText.text == "BUTTONS") controlsText.text = "JOYSTICK";
        else if (controlsText.text == "JOYSTICK") controlsText.text = "BUTTONS";
        
        PlayerPrefs.SetString("Controls", controlsText.text);
    }

    public void RemoveSkin()
    {
        SkinsManager.RemoveSkin();
        skinImage.sprite = defaultSkin;
    }

    public void LoadScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }
}
