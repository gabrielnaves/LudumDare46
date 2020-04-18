using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneLoader : MonoBehaviour {

    public float loadDelay = -1;
    public string defaultScene;

    bool invoked = false;

    public void LoadScene() {
        LoadScene(defaultScene);
    }

    public void LoadScene(string scene) {
        if (!invoked) {
            invoked = true;
            if (loadDelay > 0)
                StartCoroutine(DelayedLoad(scene));
            else
                SceneManager.LoadScene(scene);
        }
    }

    IEnumerator DelayedLoad(string scene) {
        yield return new WaitForSeconds(loadDelay);
        SceneManager.LoadScene(scene);
    }
}
