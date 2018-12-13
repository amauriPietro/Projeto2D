using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour {

    public static SFXManager instance;

    public GameObject CoinParticle;
    public GameObject KillParticle;
    public GameObject StarParticle;
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public void ShowCoinParticle(GameObject obj)
    {
        Instantiate(CoinParticle, obj.transform.position, Quaternion.identity);

    }
    public void ShowKillParticle(GameObject obj)
    {
        Instantiate(KillParticle, obj.transform.position, Quaternion.identity);

    }
    public void ShowStarParticle(GameObject obj)
    {
        Instantiate(StarParticle, obj.transform.position, Quaternion.identity);

    }
}
