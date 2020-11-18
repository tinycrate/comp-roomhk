using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour {

    private static object instance;
    public static T GetInstance {
        get {
            if (instance == null) {
                Debug.LogError($"{typeof(T).Name} is not initialized in the scene yet.");
            }
            return (T) instance;
        }
    }

    void Awake () {
        if(GetInstance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Debug.LogError($"Attempted to initialize a {this.GetType().Name} that already exist in the game, destroying...");
            Destroy(this);
        }
    }

}
