using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleCount : MonoBehaviour
{
    int count;
    TMPro.TMP_Text text;

void Awake()
    {
        text = GetComponent<TMPro.TMP_Text>();
    }
    void Start() => UpdateCount();
void OnEnable() => Collectible.OnCollected += OnCollectibleCollected;
void OnDisable() => Collectible.OnCollected -= OnCollectibleCollected; 
void OnCollectibleCollected()
{
count++;
UpdateCount();
}
void UpdateCount(){
    text.text = $"{count} / {Collectible.total}";
}
}

