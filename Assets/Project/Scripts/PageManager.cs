using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PageManager : MonoBehaviour {

    public static string lastScore = "lastscore";
    public static string highScore = "highscore";

    public Camera loadCamera;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoad;    
    }

    private void Start()
    {
        LoadScene("TitleScene");
    }

    private void OnSceneLoad(Scene scene,LoadSceneMode lsm)
    {
        loadCamera.gameObject.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        loadCamera.gameObject.SetActive(true);
        SceneManager.LoadScene(sceneName);
    }
}
