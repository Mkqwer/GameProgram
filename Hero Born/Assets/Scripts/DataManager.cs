using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataManager : MonoBehaviour, IManager
{
    private string _state;
    public string State
    {
        get { return _state; }
        set { _state = value; }
    }
    

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        
    }

    public void Initialize()
    {
        _state = "Data Manager initialized...";
        Debug.Log(_state);
    }
}
