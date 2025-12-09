using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public interface IManager 
{
    string State { get; set; }

    void Initialize();
}
