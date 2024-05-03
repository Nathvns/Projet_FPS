using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Mob : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float mobHp = 100f;
    [SerializeField] private float actualMobHp;

    void Start()
    {
        actualMobHp = mobHp;
        

    }

    // Update is called once per frame
    void Update()
    {
        if(actualMobHp <= 0){Destroy(gameObject);}
    }
 public void Attacked()
{
    actualMobHp -= 50; 
    Debug.Log("Mob HP after being attacked: " + actualMobHp);
}
}
