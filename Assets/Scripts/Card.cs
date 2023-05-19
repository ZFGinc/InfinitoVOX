using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public sealed class Card : MonoBehaviour
{
    [SerializeField] private Image _imageCard;
    [SerializeField] private TMP_Text _nameCard;
    [SerializeField] private TMP_Text _descriprionCard;
    [SerializeField] private int Index = 0;
    [SerializeField] private GameObject Backside;
    [SerializeField] private AudioSource _audioSource;

    private Animator _animator;
    private static List<InfoCard> _cards = null;
    private static List<InfoCard> _initializedCards = new List<InfoCard>(2) { new InfoCard(), new InfoCard() };
    private bool isHide = true;

    private void Awake()
    {
        _cards = null;

        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (_cards == null) _cards = LoadInfoCards();

        GenerateRandomCard();
    }

    private InfoCard GetRandomCard() => _cards[Random.Range(0, _cards.Count)];
    private bool RepetitionCheck() => _initializedCards[0].id == _initializedCards[1].id;

    private void GenerateRandomCard()
    {
        InfoCard card = GetRandomCard();

        while(!IsValidCard(card)) card = GetRandomCard();

        if (RepetitionCheck()) card = RegenerateCard();

        _initializedCards[Index] = card;
        UIDraw(card);
    }

    private InfoCard RegenerateCard()
    {
        InfoCard card;
        do
        {
            card = GetRandomCard();
            _initializedCards[Index] = card;
        } while (RepetitionCheck() || !IsValidCard(_initializedCards[Index]));
        return card;
    }
    private void UIDraw(InfoCard card)
    {
        _imageCard.sprite = UIDataPlayer.Instance.cardImages[card.id];
        _nameCard.text = card.name;
        _descriprionCard.text = card.descriprion;
    }
    private bool IsValidCard(InfoCard card)
    {
        if(card.id == 5 && Weapons.Init.currentIdWeapon == 14) return false;

        return true;
    }

    public void ShowCard()
    {
        if (!isHide) return;
        isHide = false;
        _animator.SetBool("Flip", true);
    }
    public void HideCard()
    {
        if (isHide) return;
        isHide = true;
        _animator.SetBool("Flip", true);
    }
    public void FlipTriggerAnimation()
    {
        Backside.SetActive(isHide);
        _animator.SetBool("Flip", false);
        if (isHide) gameObject.SetActive(false);
    }

    private List<InfoCard> LoadInfoCards()
    {
        string[] texts;

        TextAsset mytxtData = (TextAsset)Resources.Load("CardsInfo");
        texts = mytxtData.text.Split('\n');

        List<InfoCard> listCards = new List<InfoCard>();

        for (int i = 1; i < texts.Length; i++)
        {
            string[] cuted_row = texts[i].Split(",");
            if (cuted_row.Length != 4) continue;

            InfoCard card = new InfoCard();
            card.id = int.Parse(cuted_row[0]);
            if (LanguageDB.CurrentLanguage == "RU")
            {
                card.name = cuted_row[1];
                card.descriprion = cuted_row[2];
            }
            else
            {
                card.name = LanguageDB.GetTranslate(cuted_row[1]);
                card.descriprion = LanguageDB.GetTranslate(cuted_row[2]);
            }
            card.IDBoost = int.Parse(cuted_row[3]);

            listCards.Add(card);
        }

        return listCards;
    }

    public void ChoosesCard()
    {
        if (!AudioController.IsMuteSfx) _audioSource.Play();
        BoostApplication.Instance.SetConstantBoost((ConstantBoosts)_initializedCards[Index].IDBoost);
        UIDataPlayer.Instance.CloseLvlUpWindow();
    }
}

public struct InfoCard
{
    public int id;
    public string name;
    public string descriprion;
    public int IDBoost;
}
