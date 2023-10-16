using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ProjectileLauncher : MonoBehaviour{
    [SerializeField] private Player player;

    [SerializeField] private ThrowableObject[] throwableObjects;
    private ThrowableObjectsPool _pool = new();

    [SerializeField] private float minTimeToThrow;
    [SerializeField] private float maxTimeToThrow;
    private float _timeToThrowObject = 0.7f;
    private float _timeFromLastThrow = 0;

    private Vector3 _startPos;

    private void Awake(){
        _startPos = new Vector3(transform.position.x, transform.position.y + 1.2f, transform.position.z);
    }


    private void Update(){
        _timeFromLastThrow += Time.deltaTime;
        if (_timeFromLastThrow >= _timeToThrowObject){
            InitThrowableObject();
        }
    }

    private void InitThrowableObject(){
        ThrowableObject throwableObject = _pool.GetObject();

        if (throwableObject == null){
            throwableObject = Instantiate(throwableObjects[Random.Range(0, throwableObjects.Length)], _startPos,
                Quaternion.identity);
            throwableObject.OnTimeOut += _pool.HideObject;
            SceneManager.MoveGameObjectToScene(throwableObject.gameObject,
                SceneManager.GetSceneByBuildIndex(GameManager.Instance.LevelGame));
        }
        else{
            throwableObject.transform.position = _startPos;
        }

        Transform playerTransform = player.transform;
        throwableObject.transform.forward = playerTransform.position + playerTransform.forward * Random.Range(3f, 8f);
        _timeFromLastThrow = 0;
        _timeToThrowObject = Random.Range(minTimeToThrow, maxTimeToThrow);
    }
}