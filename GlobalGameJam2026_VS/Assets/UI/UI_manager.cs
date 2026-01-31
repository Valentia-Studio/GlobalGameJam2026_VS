using UnityEngine;

public class UI_manager : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject quite;
    [SerializeField] private GameObject creditos;
    [SerializeField] private GameObject modos;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject buttonBack;



    public void LoadPropertyFunction(string function)
    {
        Apagar();
        //if (function == "Play")
        //{
        //    Play();
        //}
        if (function == "Exit")
        {
            Quite();
        }
        else if (function == "Creditos")
        {
            Creditos();
        }
        else if (function == "Modos")
        {
            Modos();
        }
        else if(function == "Options")
        {
            Options();
        }

         
    }

    public void Apagar ()
    {
        quite.SetActive(false);
        creditos.SetActive(false);
        modos.SetActive(false);
        options.SetActive(false);
        buttonBack.SetActive(false);
    }

    void Quite()
    {
        quite.SetActive(true);
        buttonBack.SetActive(true);

    }

    public void Modos()
    {
        modos.SetActive(true);
        buttonBack.SetActive(true);

    }

    public void Exit()
    {
        Application.Quit();
    }

    void Creditos()
    {
        creditos.SetActive(true);
        buttonBack.SetActive(true);
    }

    public void Play(string scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }

    void Options()
    {
        options.SetActive(true);
        buttonBack.SetActive(true);

    }

    public void RandomScene()
    {
        int randomScene = Random.Range(1, 4);
        switch(randomScene)
        {
            case 1:
                Play("Fase1");
                break;
            case 2:
                Play("Fase2");
                break;
            case 3:
                Play("Fase3");
                break;
        }
    }
}
