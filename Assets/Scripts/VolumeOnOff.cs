using UnityEngine;
using UnityEngine.EventSystems;

public class VolumeOnOff : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        if (_audioSource.volume == 1f)
        {
            _audioSource.volume = 0f;
        }
        else
        {
            _audioSource.volume = 1f;
        }
    }
}
