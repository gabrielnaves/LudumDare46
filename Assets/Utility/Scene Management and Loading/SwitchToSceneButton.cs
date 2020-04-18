using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchToSceneButton : MonoBehaviour {

    public string targetScene;

    bool loading;

    public void SwitchToScene() {
        if (!loading)
            StartCoroutine(SwitchSceneRoutine());
    }

    IEnumerator SwitchSceneRoutine() {
        loading = true;
        yield return new WaitUntil(() => ScreenFader.instance.IsIdle());
        ScreenFader.instance.RequestFadeOut();
        yield return new WaitUntil(() => ScreenFader.instance.IsIdle());
        SceneManager.LoadScene(targetScene);
    }
}
