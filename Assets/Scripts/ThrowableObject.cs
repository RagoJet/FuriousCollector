using System;
using UnityEngine;

public class ThrowableObject : MonoBehaviour{
    public event Action<ThrowableObject> OnTimeOut;
    private float _time = 0;

    private void TimeOut(){
        OnTimeOut?.Invoke(this);
    }

    private void Update(){
        transform.Translate(Vector3.forward * Time.deltaTime * 7f);

        _time += Time.deltaTime;
        if (_time >= 2f){
            TimeOut();
            _time = 0;
        }
    }
}