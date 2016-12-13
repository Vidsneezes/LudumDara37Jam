using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleScene : MonoBehaviour {

    public GameObject Credits;
    private PageManager pageManager;

    private void Start()
    {
        ToTitle();
        pageManager = GameObject.FindGameObjectWithTag("PageManager").GetComponent<PageManager>();
    }

    public void StartGame()
    {
        pageManager.LoadScene("GameScene");
    }

    public void ToCredits()
    {
        Credits.SetActive(true);
    }

    public void ToTitle()
    {
        Credits.SetActive(false);
    }
}
