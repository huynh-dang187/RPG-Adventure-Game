using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    private IInteractable currentTarget;

    void Update()
    {
        if (currentTarget != null && Input.GetKeyDown(interactKey))
        {
            if (currentTarget.CanInteract())
            {
                currentTarget.Interact();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            currentTarget = interactable;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            if (interactable == currentTarget)
            {
                currentTarget = null;
            }
        }
    }
}
