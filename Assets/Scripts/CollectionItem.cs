using System;
using UnityEngine;

public class CollectionItem : MonoBehaviour{
    public event Action<CollectionItem> OnTriggerWithPlayer;

    public void HideSelf(){
        OnTriggerWithPlayer.Invoke(this);
    }
}