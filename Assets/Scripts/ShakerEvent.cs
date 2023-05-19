using FirstGearGames.SmoothCameraShaker;
using UnityEngine;

public class ShakerEvent : MonoBehaviour
{
    [SerializeField] private bool _isStartShake;
    [SerializeField] private ShakeData _shakeData;

    private void Start()
    {
        if (_isStartShake)
            ShakeCamera();
    }

    public void ShakeCamera() => CameraShakerHandler.Shake(_shakeData);
}
