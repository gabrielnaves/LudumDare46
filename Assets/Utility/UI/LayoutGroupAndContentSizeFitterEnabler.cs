using UnityEngine;
using UnityEngine.UI;

/**
 * Layout groups and content size fitter components tend to make a scene dirty all the time,
 * which is annoying in source control. This is a known issue marked as "won't fix" in unity
 * issues, so the workaround is to disable them and re-enable on awake.
 */
public class LayoutGroupAndContentSizeFitterEnabler : MonoBehaviour {

    void Awake() {
        GetComponent<LayoutGroup>().enabled = true;
        GetComponent<ContentSizeFitter>().enabled = true;
    }
}
