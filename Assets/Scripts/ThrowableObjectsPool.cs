using System.Collections.Generic;

public class ThrowableObjectsPool{
    private Queue<ThrowableObject> _queue = new();

    public void HideObject(ThrowableObject throwableObject){
        throwableObject.gameObject.SetActive(false);
        _queue.Enqueue(throwableObject);
    }

    public ThrowableObject GetObject(){
        if (_queue.TryDequeue(out var throwableObject)){
            throwableObject.gameObject.SetActive(true);
        }

        return throwableObject;
    }
}