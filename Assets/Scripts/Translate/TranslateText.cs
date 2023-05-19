using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TranslateText : MonoBehaviour
{
    private TMP_Text text;
    [SerializeField] private bool _isOnEnableTranslate = false;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        if(_isOnEnableTranslate) UpdateText();
    }

    public void UpdateText()
    {
        text.text = LanguageDB.GetTranslate(text.text);
    }
}
