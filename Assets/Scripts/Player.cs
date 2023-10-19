using UnityEngine;


public class Player : MonoBehaviour{
    public static Player Instance;
    private float _speed;
    private float _angle;
    private float _radius;

    private Animator _animator;
    private static readonly int Die = Animator.StringToHash("Die");

    private bool _isAlive = true;

    private void Awake(){
        Instance = this;
        _animator = GetComponent<Animator>();
    }

    private void Start(){
        _radius = PlayerSettings.Instance.radius;
        _speed = PlayerSettings.Instance.speed;
        _angle = PlayerSettings.Instance.angle;
    }

    private void Update(){
        if (_isAlive){
            _angle += _speed * Time.deltaTime;

            float x = Mathf.Cos(_angle) * _radius;
            float z = Mathf.Sin(_angle) * _radius;

            transform.forward = new Vector3(x, 0, z) - transform.position;
            transform.position = new Vector3(x, 0, z);

            if (Input.GetMouseButtonDown(0)){
                _speed = -_speed;
            }
        }
    }

    private void OnTriggerEnter(Collider other){
        if (other.TryGetComponent(out CollectionItem item)){
            if (_isAlive){
                GameManager.Instance.AddScore();
                Destroy(item.gameObject);
            }
        }
        else if (other.TryGetComponent(out ThrowableObject throwableObject)){
            if (_isAlive){
                SavePos();
                GameManager.Instance.LoseGame();
                _isAlive = false;
                transform.position = new Vector3(transform.position.x, 0.25f, transform.position.z);
                _animator.SetTrigger(Die);
            }
        }
    }

    public void SavePos(){
        PlayerSettings.Instance.angle = _angle;
        PlayerSettings.Instance.speed = _speed;
    }
}