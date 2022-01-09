using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSound : MonoBehaviour
{
    private SoundReader soundReader;
    private void OnCollisionEnter(Collision collision)
    {
        var _soundreader = collision.gameObject.GetComponentInParent<SoundReader>();
        if (soundReader == null && _soundreader != null)
        {
            soundReader = _soundreader;
            soundReader.secondClipName = "PointCollision";
            soundReader.PlaySeconde();
            return;
        }
        if (_soundreader!= soundReader)
        {
            soundReader = _soundreader;
            soundReader.secondClipName = "PointCollision";
            soundReader.PlaySeconde();
        }
    }
}
