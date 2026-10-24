using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    public float detectionRange = 1.2f;
    public LayerMask interactLayer;

    private IInteractable currentTarget;


    void Update()
    {
        Detect();
    }


    private void Detect()
    {
        Collider2D hit = Physics2D.OverlapCircle(
            transform.position,
            detectionRange,
            interactLayer
        );


        if(hit != null)
        {
            currentTarget = hit.GetComponent<IInteractable>();
        }
        else
        {
            currentTarget = null;
        }
    }


    public void Interact()
    {
        if(currentTarget != null)
        {
            currentTarget.Interact();
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}