using UnityEngine;

public class LearningCurve : MonoBehaviour
{
  
    public int CurrentAge = 30;
    public int AddedAge = 1;

    public float Pi = 3.14f;
    public string FisrName = "Harrison";
    public bool IsAuthor = true;

    void Start()
    {
        ComputeAge();
    }

    void ComputeAge()
    {
        Debug.Log(CurrentAge + AddedAge);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
