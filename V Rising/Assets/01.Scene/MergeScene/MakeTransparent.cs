using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTransparent : MonoBehaviour
{
    public string targetTag = "Transparent";

    // This method is called when the collider attached to this object collides with another collider.
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
        // Check if the colliding object has the specified tag.
        if (collision.gameObject.CompareTag(targetTag))
        {
            Destroy(collision.gameObject);
            
            // Try to get the MeshRenderer component from the colliding object.
            MeshRenderer meshRenderer = collision.gameObject.GetComponent<MeshRenderer>();
            Debug.Log("success!2");

            // If a MeshRenderer component is found, disable it.
            if (meshRenderer != null)
            {
                Debug.Log("success!3");
                meshRenderer.enabled = false;
            }
        }
    }

}
