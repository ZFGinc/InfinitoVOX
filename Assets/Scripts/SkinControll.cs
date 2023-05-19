using UnityEngine;

public class SkinControll : MonoBehaviour
{
    [SerializeField] private PlayerSkin[] _skins;
    [SerializeField] private PlayerMove _playerMove;

    public void SetSkin()
    {
        int currentSkin = PlayerPrefs.GetInt("PlayerSkin", 0);

        Player.Init.SetCurrentDamgeEffect(_skins[currentSkin]);
        _playerMove.SetAnimator(_skins[currentSkin].animator);
        _playerMove.SetTors(_skins[currentSkin].tors);

        _skins[currentSkin].gameObject.SetActive(true);
    }
}
