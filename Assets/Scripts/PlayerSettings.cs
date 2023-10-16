using UnityEngine;

public class PlayerSettings : MonoBehaviour{
    public static PlayerSettings Instance;

    public float speed = 1.4f;
    public float angle = 0;
    public float radius = 5.5f;

    private void Awake(){
        Instance = this;
    }
}