using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(Image))]
public class Emotions : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;

    private Image _image;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        _image.sprite = _sprites[Random.Range(0, _sprites.Length)];
        _animator.SetBool("PlayAnimation", true);
    }

    public void StopAnimation()
    {
        _animator.SetBool("PlayAnimation", false);
        gameObject.SetActive(false);
    }
}
