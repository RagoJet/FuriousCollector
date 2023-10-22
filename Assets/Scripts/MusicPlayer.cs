using System;
using UnityEngine;

public class MusicPlayer : MonoBehaviour{
    [SerializeField] AudioClip[] musicClips;
    private AudioSource _audioSource;
    private int _musicCount = 0;

    private void Awake(){
        _audioSource = GetComponent<AudioSource>();
        ShuffleMusics();
        _audioSource.PlayOneShot(musicClips[_musicCount]);
    }

    private void Update(){
        if (!_audioSource.isPlaying){
            _musicCount++;
            if (_musicCount >= _musicCount){
                _musicCount = 0;
                ShuffleMusics();
            }

            _audioSource.PlayOneShot(musicClips[_musicCount]);
        }
    }

    private void ShuffleMusics(){
        System.Random rng = new System.Random();
        int n = musicClips.Length;
        while (n > 1){
            n--;
            int k = rng.Next(n + 1);
            (musicClips[k], musicClips[n]) = (musicClips[n], musicClips[k]);
        }
    }
}