using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour{
    public static GameManager Instance;

    public int LevelGame => _levelGame;
    private int _levelGame = 1;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private CanvasGroup curtain;
    private string _scoreString;
    private int _score = 0;

    private int _amountOfItems;

    [SerializeField] private Image restartGamePanel;
    [SerializeField] private TextMeshProUGUI restartText;

    [SerializeField] Volume volume;
    private ColorAdjustments CA;
    private ParticleSystem _particleSystem;
    private float _saturationValueCA = 0;

    private Coroutine _deadCoroutine;

    private void Awake(){
        volume.profile.TryGet<ColorAdjustments>(out CA);
        _saturationValueCA = CA.saturation.value;
        Instance = this;
        string lang = "en";
        switch (lang){
            case "ru":
                _scoreString = "Очки: ";
                restartText.text = "Рестарт";
                break;
            case "en":
                _scoreString = "Score: ";
                restartText.text = "Restart";
                break;
        }

        UpdateUIScore();
    }

    private void Start(){
        StartCoroutine(StartLevel());
    }

    private IEnumerator StartLevel(){
        AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(_levelGame, LoadSceneMode.Additive);
        while (!loadSceneAsync.isDone){
            yield return null;
        }

        _particleSystem = GameObject.Find("FXGameOver").GetComponent<ParticleSystem>();
        _amountOfItems = CircleCreatorItems.Instance.AmountOfItems;
        while (curtain.alpha > 0){
            curtain.alpha -= 0.05f;
            yield return null;
        }
    }

    private IEnumerator UnloadScene(bool nextOrRestartLevel){
        curtain.alpha = 1;
        AsyncOperation unloadSceneAsync = SceneManager.UnloadSceneAsync(_levelGame);
        while (!unloadSceneAsync.isDone){
            yield return null;
        }

        if (nextOrRestartLevel){
            _levelGame++;
        }
        else{
            _levelGame = 1;
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
        Player.Instance.SavePos();
        restartGamePanel.gameObject.SetActive(true);
        _particleSystem.Play();
        _deadCoroutine = StartCoroutine(DeadVisibleMode());
    }

    IEnumerator DeadVisibleMode(){
        while (CA.saturation.value > -90f){
            CA.saturation.value = CA.saturation.value - 0.5f;
            yield return null;
        }
    }

    public void RestartGame(){
        _score = 0;
        UpdateUIScore();
        StopCoroutine(_deadCoroutine);
        CA.saturation.value = _saturationValueCA;
        StartCoroutine(UnloadScene(false));
        restartGamePanel.gameObject.SetActive(false);
    }

    private void UpdateUIScore(){
        scoreText.text = _scoreString + _score;
    }
}