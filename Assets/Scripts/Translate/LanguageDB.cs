using System.Collections.Generic;
using TMPro;
using UnityEngine;

public delegate void UpdateText();
public class LanguageDB : MonoBehaviour
{
    [SerializeField] private TMP_Text _textButton = null;
    [SerializeField] private GameObject[] _disableObjects;
    private static LanguageDB _instance = null;
    private List<string[]> _db;

    public static event UpdateText UpdateLanguageEvent;
    public static string CurrentLanguage { get; private set; }

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);

        LoadLanguageDB();
    }

    private void Start()
    {
        foreach(TranslateText text in FindObjectsOfType<TranslateText>())
            UpdateLanguageEvent += text.UpdateText;
        
        if (PlayerPrefs.GetString("Language", "EN") != "RU")
            UpdateLanguage();

        CurrentLanguage = PlayerPrefs.GetString("Language", "EN");

        foreach (GameObject obj in _disableObjects) 
            obj.SetActive(false);
    }

    public void UpdateLanguage()
    {
        if (_textButton == null) return;

        if(_textButton.text == "RU") _textButton.text = "EN";
        else _textButton.text = "RU";

        PlayerPrefs.SetString("Language", _textButton.text);
        CurrentLanguage = _textButton.text;

        UpdateLanguageEvent.Invoke();
    }

    private void LoadLanguageDB()
    {
        _db = new List<string[]>();
        string[] texts;

        TextAsset mytxtData = (TextAsset)Resources.Load("TranslationDataBase");
        texts = mytxtData.text.Split('\n');

        for (int i = 1; i < texts.Length; i++)
        {
            string[] cuted_row = texts[i].Split("\t");
            if (cuted_row.Length < 2) continue;

            _db.Add(cuted_row);
        }
    }

    public static string GetTranslate(string value)
    {
        value = value.Replace("\r", "");

        foreach (string[] row in _instance._db)
        {
            row[0] = row[0].Replace("\r", "");
            row[1] = row[1].Replace("\r", "");

            if (value == row[0])
            {
                if(CurrentLanguage == "RU") return row[0];
                else return row[1];
            }
            else if (value == row[1])
            {
                if (CurrentLanguage == "EN") return row[1];
                else return row[0];
            }
        }
        return "404";
    }
}
