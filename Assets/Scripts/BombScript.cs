using UnityEngine;

public class BombScript : MonoBehaviour
{
    [Header("Explotion Cast")]
    [SerializeField] int range;
    [SerializeField] float explotionTimer;

    [Header("Bomb Stats")]
    [SerializeField] LayerMask layerMask; 
    [SerializeField] float sphereCastRad; 

    float spawnTime;

    private void OnEnable()
    {
        spawnTime = Time.time;
    }

    void Update()
    {
        if (Time.time - spawnTime >= explotionTimer)
        {
            Explode(); 
            gameObject.SetActive(false);
        }
    }

    void Explode()
    {
        Ray ray = new Ray(transform.position, Vector3.right);

        RaycastHit[] hits = Physics.SphereCastAll(ray, sphereCastRad, range, layerMask);
        
        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.tag == "Unbreakable") break;
                hit.transform.gameObject.SetActive(false);
                if (hit.transform.tag == "Breakable")
                {
                    break;
                }
            }
        }
    }
}
