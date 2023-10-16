using UnityEngine;

class RotateObject : MonoBehaviour{
    private void Update(){
        transform.Rotate(Vector3.right, Time.deltaTime * 350f);
    }
}