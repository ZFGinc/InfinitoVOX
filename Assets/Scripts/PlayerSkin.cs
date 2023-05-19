using System.Collections;
using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    [Header("Материалы и объекты тела для визуализации получения урона")]
    [SerializeField] private Material _defaultaterial;
    [SerializeField] private Material _materialTakeDamage;
    [SerializeField] private MeshRenderer[] _bodyParts;

    [Space]
    [Header("Объекты скина для передачи их в другие компоненты")]
    public GameObject tors;
    public Animator animator;

    private const int countBlick = 6;
    private bool _isTakeDamage = false;

    public void DamageEffect() => StartCoroutine(EffectTakeDamage());

    private IEnumerator EffectTakeDamage()
    {
        if (_isTakeDamage) yield break;

        _isTakeDamage = true;
        bool isWhite = false;

        for (int i = 0; i < countBlick; i++)
        {
            if (isWhite)
                foreach (MeshRenderer mesh in _bodyParts)
                    mesh.material = _defaultaterial;
            else
                foreach (MeshRenderer mesh in _bodyParts)
                    mesh.material = _materialTakeDamage;

            isWhite = !isWhite;
            yield return new WaitForSeconds(.3f);
        }

        _isTakeDamage = false;
    }
}
