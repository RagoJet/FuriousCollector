using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class GameManager : MonoBehaviour{
    public static GameManager Instance;
    [SerializeField] private CanvasGroup curtain;
    [SerializeField] CircleCreatorItems creatorItems;

    [SerializeField] private MusicPlayer musicPlayer;
    public int LevelGame => _levelGame;
    private int _levelGame = 1;
    private int _countOfScenes;

    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI scoreText;
    private string _scoreString;
    string _bestScoreString;
    private int _score = 0;
    private int _bestScore = 0;

    private int _amountOfItems;

    [SerializeField] private Image restartGamePanel;
    [SerializeField] private TextMeshProUGUI restartText;

    [SerializeField] Volume volume;
    private ColorAdjustments _CA;
    float _caSaturationValue = 0;
    private ParticleSystem _particleSystem;
    private Coroutine _deadCoroutine;

    private void OnEnable() => YandexGame.GetDataEvent += Init;

    private void OnDisable() => YandexGame.GetDataEvent -= Init;

    private void Awake(){
        if (YandexGame.SDKEnabled == true){
            Init();
        }
    }

    private void Init(){
        _bestScore = YandexGame.savesData.bestScore;
        Instance = this;
        string lang = YandexGame.savesData.language;
        switch (lang){
            case "ru":
                _scoreString = "Очки: ";
                _bestScoreString = "Рекорд: ";
                restartText.text = "Рестарт";

                break;
            case "en":
                _scoreString = "Score: ";
                _bestScoreString = "Best score: ";
                restartText.text = "Restart";
                break;
        }


        volume.profile.TryGet<ColorAdjustments>(out _CA);
        _caSaturationValue = _CA.saturation.value;

        UpdateUIScore();
        UpdateUIBestScore();
        _countOfScenes = SceneManager.sceneCountInBuildSettings;
        StartCoroutine(StartLevel());
    }

    private IEnumerator StartLevel(){
        AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(_levelGame, LoadSceneMode.Additive);
        while (!loadSceneAsync.isDone){
            yield return null;
        }

        _particleSystem = GameObject.Find("FXGameOver").GetComponent<ParticleSystem>();

        musicPlayer.PlayNewLevelSound();
        while (curtain.alpha > 0){
            curtain.alpha -= 0.1f;
            yield return null;
        }

        creatorItems.Init();
        _amountOfItems = creatorItems.AmountOfItems;
    }

    private IEnumerator UnloadScene(bool nextOrRestartLevel){
        curtain.alpha = 1;
        AsyncOperation unloadSceneAsync = SceneManager.UnloadSceneAsync(_levelGame);
        while (!unloadSceneAsync.isDone){
            yield return null;
        }

        if (nextOrRestartLevel){
            _levelGame++;
            if (_levelGame >= _countOfScenes){
                _levelGame = 1;
            }
        }
        else{
            _levelGame = 1;
            _score = 0;
        }

        StartCoroutine(StartLevel());
    }

    public void AddScore(){
        _amountOfItems--;
        _score++;
        UpdateUIScore();
        if (_amountOfItems == 0){
            Player.Instance.SavePos();
            StartCoroutine(UnloadScene(true));
        }
    }

    public void LoseGame(){
        if (_score > _bestScore){
            _bestScore = _score;
            UpdateUIBestScore();
            YandexGame.savesData.bestScore = _bestScore;
            YandexGame.SaveProgress();
            YandexGame.NewLeaderboardScores("LeaderBoardFC", _bestScore);
        }

        musicPlayer.PlayDeathSound();
        restartGamePanel.gameObject.SetActive(true);
        _particleSystem.Play();
        _deadCoroutine = StartCoroutine(DeadVisibleMode());
    }

    IEnumerator DeadVisibleMode(){
        while (_CA.saturation.value > -90f){
            _CA.saturation.value = _CA.saturation.value - 0.5f;
            yield return null;
        }
    }

    public void RestartGame(){
        UpdateUIScore();
        _CA.saturation.value = _caSaturationValue;
        StopCoroutine(_deadCoroutine);
        YandexGame.FullscreenShow();
        StartCoroutine(UnloadScene(false));
        restartGamePanel.gameObject.SetActive(false);
    }

    private void UpdateUIScore(){
        scoreText.text = _scoreString + _score;
    }

    private void UpdateUIBestScore(){
        bestScoreText.text = _bestScoreString + _bestScore;
    }
}