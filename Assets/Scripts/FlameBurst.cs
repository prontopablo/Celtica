using UnityEngine;

public class FlameBurst : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem flameParticle;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            flameParticle.transform.position = transform.position;
            flameParticle.Play();
            Invoke("StopFlameParticle", 0.5f);
        }
    }

    private void StopFlameParticle()
    {
        flameParticle.Stop();
        //flameParticle.transform.position = transform.position;
    }
}