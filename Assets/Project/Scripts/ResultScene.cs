using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResultScene : MonoBehaviour {

    public Text score;
    public Text highScore;
    private PageManager pageManager;
    private float prewarm;


    private void Start()
    {
        prewarm = 0;
        pageManager = GameObject.FindGameObjectWithTag("PageManager").GetComponent<PageManager>();
        score.text = PlayerPrefs.GetInt(PageManager.lastScore).ToString();
        highScore.text = PlayerPrefs.GetInt(PageManager.highScore).ToString();
    }

    private void Update()
    {
        prewarm += Time.deltaTime;
        if (Input.GetAxis("Fire1") > 0 && prewarm > 0.6f)
        {
            pageManager.LoadScene("TitleScene");
        }
    }
}
