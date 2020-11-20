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
        if (instance == null) {
            instance = this;
            AfterAwake();
        }
        else {
            Debug.Log($"Using an existing {this.GetType().Name} that already exist in the game instead of the current one...");
            Destroy(this);
        }
    }

    // Called only when the object is initialized and not destroyed
    protected virtual void AfterAwake() {}
}
