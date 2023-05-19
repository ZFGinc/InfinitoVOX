using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ShakerEvent))]
[RequireComponent(typeof(AudioSource))]
public class Altar : MonoBehaviour
{
    [SerializeField] private AltarController controller;
    [SerializeField] private GameObject _fire;
    [SerializeField] private Transform _bossPositionSpawn;
    
    private Animator _animator;
    private AudioSource _audio;
    private bool _isActive = false;
    private ShakerEvent _shakerEvent;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _shakerEvent = GetComponent<ShakerEvent>();
        _audio = GetComponent<AudioSource>();

        _fire.SetActive(false);
        _isActive = false;
    }

    private void FixedUpdate()
    {
        if (_fire.activeSelf != _isActive)
            _fire.SetActive(_isActive);
    }

    public void StartAnimation()
    {
        _audio.Play();

        _animator.SetBool("active", !_animator.GetBool("active"));
        UIDataPlayer.Instance.BlockMove();

        _shakerEvent.ShakeCamera();
    }

    public void EndAnimation()
    {
        try
        {
            controller.SetDefaultCamera();
        }
        catch (Exception ex) { }
        _isActive = false;
        UIDataPlayer.Instance.UnblockMove();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isActive) return;

        if(other.gameObject.tag == "Player")
        {
            try
            {
                _isActive = true;
                Instantiate(controller.GetRandomBoss(), _bossPositionSpawn.position, Quaternion.identity);

                DestroySuperfluousBosses();
                controller.DisableArrow();
            }
            catch (Exception ex) { }
        }
    }

    private void DestroySuperfluousBosses()
    {
        List<GameObject> bosses = GameObject.FindGameObjectsWithTag("Boss").ToList();

        while (bosses.Count > 1)
        {
            Destroy(bosses[0]);
            bosses.RemoveAt(0);
        }
    }
}
