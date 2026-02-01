using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] int selectedLevel;
    
    public void LoadScene()
    {
        SceneManager.LoadScene("Level " + selectedLevel.ToString());
    }
}
