using System.Collections.Generic;
using UnityEngine;

public class CircleCreatorItems : MonoBehaviour{
    public int AmountOfItems => items.Count;
    [SerializeField] private List<CollectionItem> items;
    CollectionPool _itemPool = new CollectionPool();
    private float _radius;
    private Vector3[] dots;

    private System.Random _random = new System.Random();

    private void Awake(){
        _radius = ParametersOfPlayer.Instance.radius;

        dots = new Vector3[items.Count];
        var step = 2 * Mathf.PI / items.Count;
        for (int i = 0; i < items.Count; i++){
            float angle = i * step;
            dots[i] = new Vector3(_radius * Mathf.Sin(angle), 1f, _radius * Mathf.Cos(angle));
        }

        for (int i = 0; i < items.Count; i++){
            CollectionItem item = Instantiate(items[i]);
            item.OnTriggerWithPlayer += _itemPool.HideObject;
            _itemPool.AddObjectTolist(item);
            _itemPool.HideObject(item);
        }
    }

    public void Init(){
        Shuffle();
        _itemPool.CreateAllObjects(dots);
    }


    public void Shuffle(){
        System.Random rng = new System.Random();
        int n = dots.Length;
        while (n > 1){
            n--;
            int k = rng.Next(n + 1);
            (dots[k], dots[n]) = (dots[n], dots[k]);
        }
    }
}