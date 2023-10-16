using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{
    public static GameManager Instance;

    private string _scoreString;
    private int _score = 0;
    public int LevelGame => _levelGame;
    private int _levelGame = 1;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private CanvasGroup curtain;

    private int _amountOfItems;


    private void Start(){
        Instance = this;
        string lang = "ru";
        switch (lang){
            case "ru":
                _scoreString = "Очки: ";
                break;
            case "en":
                _scoreString = "Score: ";
                break;
        }


        UpdateUIScore();
        StartCoroutine(StartLevel());
    }

    public void Update(){
        if (Input.GetKeyDown(KeyCode.R)){
            _score = 0;
            UpdateUIScore();
            StartCoroutine(UnloadScene(false));
        }
    }

    private IEnumerator StartLevel(){
        AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(_levelGame, LoadSceneMode.Additive);
        while (!loadSceneAsync.isDone){
            yield return null;
        }

        _amountOfItems = CircleCreatorItems.Instance.AmountOfItems;
        while (curtain.alpha > 0){
            curtain.alpha -= 0.03f;
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
            StartCoroutine(UnloadScene(true));
        }
    }

    private void UpdateUIScore(){
        scoreText.text = _scoreString + _score;
    }
}