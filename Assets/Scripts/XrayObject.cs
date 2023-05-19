using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Unity.VisualScripting;
using UnityEngine;

public class XrayObject : MonoBehaviour
{
    [SerializeField] private GameObject _normalObj;
    [SerializeField] private GameObject _xrayObj;

    private void Awake()
    {
        ShowSolid();
    }

    public void ShowTranperant()
    {
        _normalObj.SetActive(false);
        _xrayObj.SetActive(true);
    }
    public void ShowSolid()
    {
        _normalObj.SetActive(true);
        _xrayObj.SetActive(false);
    }
}
