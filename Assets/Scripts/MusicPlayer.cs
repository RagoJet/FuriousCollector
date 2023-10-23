using UnityEngine;

public class MusicPlayer : MonoBehaviour{
    [SerializeField] AudioClip newLevelClip;
    [SerializeField] AudioClip[] deathClips;
    [SerializeField] AudioClip[] musicClips;
    AudioSource _audioSource;
    private int _musicCount = 0;

    private void Awake(){
        _audioSource = GetComponent<AudioSource>();
        ShuffleMusics();
        _audioSource.PlayOneShot(musicClips[_musicCount]);
    }

    private void Update(){
        if (!_audioSource.isPlaying){
            _musicCount++;
            if (_musicCount >= musicClips.Length){
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

    public void PlayDeathSound(){
        _audioSource.PlayOneShot(deathClips[Random.Range(0, deathClips.Length)]);
    }

    public void PlayNewLevelSound(){
        _audioSource.PlayOneShot(newLevelClip);
    }
}