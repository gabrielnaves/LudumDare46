using UnityEngine;
using UnityEngine.Animations;

public class SetMainCameraAsParent : MonoBehaviour {

    public bool useParentConstraintComponent;

    void Start() {
        if (useParentConstraintComponent)
            SetupParentConstraint();
        else
            transform.SetParent(Camera.main.transform);
    }

    void SetupParentConstraint() {
        ParentConstraint constraint = gameObject.AddComponent<ParentConstraint>();
        constraint.AddSource(MakeConstraintSourceFromMainCamera());
        constraint.SetTranslationOffset(0, new Vector3(0, 0, 10));
        constraint.constraintActive = true;
    }

    ConstraintSource MakeConstraintSourceFromMainCamera() {
        return new ConstraintSource() {
            sourceTransform = Camera.main.transform,
            weight = 1
        };
    }
}
