using UnityEngine;

public class SpawnSuperPowers : MonoBehaviour
{
    [SerializeField]
    private GameObject superPowersPrefab;

    [SerializeField]
    private float frequency = 1.0f;

    [SerializeField]
    private float destroyAfterSeconds = 5.0f;

    [SerializeField]
    private float force = 5.0f;

    private float timerElapsed = 0;
    
    public bool Started { get;set; } = false;
    
    void FixedUpdate() 
    {
        if(!Started)
            return;
            
        if(timerElapsed >= frequency)
        {
            // spawn particles
            GameObject superPower = Instantiate(superPowersPrefab, Vector3.zero, Quaternion.identity);
            superPower.transform.parent = transform;
            superPower.transform.localPosition = Vector3.zero;

            ApplyForce(superPower);

            // destroy super powers after x seconds
            Destroy(superPower, destroyAfterSeconds);

            // reset timer
            timerElapsed = 0;
        }    
        timerElapsed += Time.deltaTime * 1.0f;
    }

    void ApplyForce(GameObject go)
    {
        Rigidbody rigidbody = go.GetComponent<Rigidbody>();
        rigidbody.AddForceAtPosition(go.transform.parent.forward * force, 
            go.transform.position, ForceMode.Force);
    }
}
