using UnityEditor;

[CustomEditor(typeof(Interactable), true)]

public class InteractableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Interactable interactable = (Interactable)target;

        if (target.GetType() == typeof(EventOnlyInteractable))
        {
            interactable.PromptMessage = EditorGUILayout.TextField("Prompt Message: ", interactable.PromptMessage);
            EditorGUILayout.HelpBox("EventOnlyInteract can ONLY use UnityEvents.", MessageType.Info);
            if (interactable.gameObject.GetComponent<InteractionEvents>() == null)
            {
                interactable.useEvents = true;
                interactable.gameObject.AddComponent<InteractionEvents>();
            }
        }
        else
        {
            base.OnInspectorGUI();

            if (interactable.useEvents)
            {
                if (interactable.GetComponent<InteractionEvents>() == null)
                {
                    interactable.gameObject.AddComponent<InteractionEvents>();
                }
            }

            else
            {
                if (interactable.GetComponent<InteractionEvents>() != null)
                {
                    DestroyImmediate(interactable.GetComponent<InteractionEvents>());
                }
            }
        }
    }
}
