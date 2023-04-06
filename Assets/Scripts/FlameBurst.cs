using UnityEngine;

public class FlameBurst : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem flameParticle;

    [SerializeField]
    private float maxFireBurstLength = 1.0f;

    private float currentFireBurstLength = 0.0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            flameParticle.transform.position = transform.position;
            flameParticle.Play();
        }
        else if (Input.GetKey(KeyCode.I) && currentFireBurstLength < maxFireBurstLength)
        {
            currentFireBurstLength += Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.I))
        {
            flameParticle.Stop();
            flameParticle.transform.position = transform.position;
            currentFireBurstLength = 0.0f;
        }
    }
}
