using UnityEngine;

public class SkinsMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] _skins;

    public static SkinsMenu instance = null;

    private void Start()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);

        SetSkin();
    }

    public void SetSkin()
    {
        DisableAllSkins();

        int currentSkin = PlayerPrefs.GetInt("PlayerSkin", 0);
        _skins[currentSkin].SetActive(true);
    }

    private void DisableAllSkins()
    {
        foreach (GameObject obj in _skins) obj.SetActive(false);
    }
}
