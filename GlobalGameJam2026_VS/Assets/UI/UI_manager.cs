using UnityEngine;

public class UI_manager : MonoBehaviour
{
    [SerializeField] private string sceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    void exit()
    {
        Application.Quit();
    }
}
