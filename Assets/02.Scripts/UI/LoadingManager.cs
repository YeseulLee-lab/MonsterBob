using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    #region Singleton
    public static LoadingManager _instance;

    public static LoadingManager Instance
    {
        get
        {
            if (_instance == null)
            {
                LoadingManager find = GameObject.FindObjectOfType<LoadingManager>();
                if (find != null)
                {
                    _instance = find;
                }
                else
                    _instance = new GameObject().AddComponent<LoadingManager>();
            }
            return _instance;
        }
    }

    public void Awake()
    {
        if (FindObjectsOfType<LoadingManager>().Length != 1)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this.gameObject);

        LoadNextScene("01.Start");
    }
    #endregion

    public Text progressText;
    public float progress;

    public void LoadNextScene(string sceneName)
    {
        if (SceneManager.GetActiveScene().name != "00.Loading")
        {
            SceneManager.LoadScene("00.Loading");
        }
        Time.timeScale = 1;
        StartCoroutine(CoLoadScene(sceneName));
    }

    private IEnumerator CoLoadScene(string sceneName)
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOperation.isDone)
        {
            progressText.text = "Loading " + Mathf.Round(asyncOperation.progress * 100) + "%";
            yield return null;
        }
    }
}
