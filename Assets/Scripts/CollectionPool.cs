using System.Collections.Generic;
using UnityEngine;

public class CollectionPool{
    private List<CollectionItem> _list = new List<CollectionItem>();

    public void AddObjectTolist(CollectionItem item){
        _list.Add(item);
    }

    public void HideObject(CollectionItem item){
        item.gameObject.SetActive(false);
    }

    public void CreateAllObjects(Vector3[] dots){
        var count = _list.Count;
        for (int i = 0; i < count; i++){
            CollectionItem item = _list[i];
            item.gameObject.SetActive(true);
            item.transform.position = dots[i];
        }
    }
}