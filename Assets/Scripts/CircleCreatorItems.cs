using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CircleCreatorItems : MonoBehaviour{
    public static CircleCreatorItems Instance;
    public int AmountOfItems => _amountOfItems;
    private int _amountOfItems;
    [SerializeField] private List<CollectionItem> items;
    private float _radius;

    private void Awake(){
        Instance = this;
        _radius = PlayerSettings.Instance.radius;
        _amountOfItems = items.Count;
        var itemCount = items.Count;
        Vector3[] dots = new Vector3[itemCount];
        var step = 2 * Mathf.PI / itemCount;
        for (int i = 0; i < itemCount; i++){
            float angle = i * step;
            dots[i] = new Vector3(_radius * Mathf.Sin(angle), 1f, _radius * Mathf.Cos(angle));
        }

        int j = 0;
        while (items.Count > 0){
            int randomValue = Random.Range(0, items.Count);
            Instantiate(items[randomValue], dots[j], Quaternion.identity);
            items.Remove(items[randomValue]);
            j++;
        }
    }
}