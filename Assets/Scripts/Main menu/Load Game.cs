using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    [SerializeField]
    private string mainSceneName;

    // Update is called once per frame
    public void LoadScene()
    {
        SceneManager.LoadSceneAsync(mainSceneName);
    }
}
