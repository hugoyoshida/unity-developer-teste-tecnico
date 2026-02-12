using UnityEngine;

public class AttachTarget : MonoBehaviour
{
    [Header("Settings")]
    public Animator animator;
    [SerializeField] private float stiffness = 10f;
    [SerializeField] private float damping = 5f;
    [SerializeField] private Vector3 linkTransformOffset = new(0, -1, 0);

    [Header("Display")]
    public Transform linkTransform;

    private float distance;
    private Vector3 direction;
    private Vector3 correction;
    private Vector3 force;
    private Vector3 relativeVelocity;
    private Rigidbody characterRigidbody;

    //
    //[SerializeField] private Rigidbody characterHipsRigidbody;
    //[SerializeField] private GameObject characterRig;
    //private Collider[] ragdollColliders;
    //public Rigidbody[] ragdollRigidbodies;
    //

    private void Start()
    {
        characterRigidbody = GetComponent<Rigidbody>();

        //
        //ragdollColliders = characterRig.GetComponentsInChildren<Collider>();
        //ragdollRigidbodies = characterRig.GetComponentsInChildren<Rigidbody>();
        //RagDollActive(false);
        //
    }

    private void FixedUpdate()
    {
        if (linkTransform == null) 
            return;

        direction = transform.position - linkTransform.position + linkTransformOffset;
        distance = direction.magnitude;

        if (distance > 0)
        {
            correction = direction.normalized * distance;
            force = stiffness * Time.fixedDeltaTime * -correction;
            characterRigidbody.AddForce(force, ForceMode.Acceleration);

            relativeVelocity = characterRigidbody.linearVelocity - linkTransform.GetComponent<Rigidbody>().linearVelocity;
            characterRigidbody.AddForce(damping * Time.fixedDeltaTime * -relativeVelocity, ForceMode.Acceleration);
        }

        //Quaternion targetRotation = linkTransform.rotation;
        characterRigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, linkTransform.rotation, 10f * Time.fixedDeltaTime));

        characterRigidbody.AddForce(10 * Time.fixedDeltaTime * Vector3.up, ForceMode.Acceleration);
    }

    //public void RagDollActive(bool value) // false
    //{
    //    foreach(Collider collider in ragdollColliders)
    //        collider.enabled = value;

    //    foreach (Rigidbody rigidbody in ragdollRigidbodies)
    //        rigidbody.isKinematic = !value;

    //    animator.enabled = !value;
    //    characterRigidbody.GetComponent<Collider>().enabled = !value;
    //    characterRigidbody.isKinematic = value;
    //}
}
