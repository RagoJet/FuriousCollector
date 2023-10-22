using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ProjectileLauncher : MonoBehaviour{
    [SerializeField] private Player player;

    [SerializeField] private ThrowableObject[] throwableObjects;
    private ObjectPoolT<ThrowableObject> _throwablePool = new();

    private float minTimeToThrow = 0.15f;
    private float maxTimeToThrow = 0.6f;
    private float _timeToThrowObject = 0.8f;
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
        ThrowableObject throwableObject = _throwablePool.GetObject();
        if (throwableObject == null){
            throwableObject = Instantiate(throwableObjects[Random.Range(0, throwableObjects.Length)], _startPos,
                Quaternion.identity);
            throwableObject.OnTimeOut += _throwablePool.HideObject;
            SceneManager.MoveGameObjectToScene(throwableObject.gameObject,
                SceneManager.GetSceneByBuildIndex(GameManager.Instance.LevelGame));
        }
        else{
            throwableObject.transform.position = _startPos;
        }

        float addAngle = Random.Range(-0.2f, 1f);
        if (player.Speed < 0){
            addAngle = -addAngle;
        }

        float angle = player.Angle + addAngle;
        float x = Mathf.Cos(angle);
        float z = Mathf.Sin(angle);
        Vector3 to = new Vector3(x, throwableObject.transform.position.y, z);
        Vector3 from = new Vector3(transform.position.x, throwableObject.transform.position.y, transform.position.z);
        throwableObject.transform.forward = to - from;


        _timeFromLastThrow = 0;
        _timeToThrowObject = Random.Range(minTimeToThrow, maxTimeToThrow);
    }
}