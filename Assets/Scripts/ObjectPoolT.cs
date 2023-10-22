using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolT<T> where T : MonoBehaviour{
    private Queue<T> _queue = new();

    public void HideObject(T o){
        o.gameObject.SetActive(false);
        _queue.Enqueue(o);
    }

    public T GetObject(){
        if (_queue.TryDequeue(out var o)){
            o.gameObject.SetActive(true);
        }

        return o;
    }
}