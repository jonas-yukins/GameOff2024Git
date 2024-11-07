using UnityEditor;

[CustomEditor(typeof(Interactable), true)]
public class InteractableEditor : Editor
{
    public override void OnInspectorGUI() {
        Interactable interactable = (Interactable)target;
        base.OnInspectorGUI();
        if (interactable.useEvents) {
            interactable.gameObject.AddComponent<InteractionEvent>();
        }
    }
}
