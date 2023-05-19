using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] private Player _player;

    public void EndAnimationTrigger()
    {
        _player.EndVFXHealing();
    }
}
