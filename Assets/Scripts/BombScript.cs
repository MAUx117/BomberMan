using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BombScript : MonoBehaviour
{
    [SerializeField] int gridOffSet;
    [SerializeField] int spawnHeight; 


    [Header("Explotion Cast")]
    [SerializeField] int range;
    [SerializeField] float explotionTimer;

    [Header("Bomb Stats")]
    [SerializeField] LayerMask layerMask; 
    [SerializeField] float sphereCastRad; 

    float spawnTime;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        spawnTime = Time.time;
        Vector2 spawnPos = new Vector2 (transform.position.x, transform.position.z);
        int divMain =  (int) Mathf.Floor (spawnPos.x / gridOffSet);
        float module = spawnPos.x % gridOffSet;
        if (Mathf.Abs (module) > gridOffSet / 2) divMain++; 
        spawnPos.x = divMain * gridOffSet;

        divMain = (int)Mathf.Floor(Mathf.Abs (spawnPos.y / gridOffSet));
        module = spawnPos.y % gridOffSet;
        if (Mathf.Abs(module) > gridOffSet / 2) divMain++;
        spawnPos.y = divMain * -gridOffSet;

        transform.position = new Vector3(spawnPos.x, spawnHeight, spawnPos.y);
    }

    void Update()
    {
        if (Time.time - spawnTime >= explotionTimer)
        {
            animator.SetTrigger("Explode");
            spawnTime = Time.time;  
        }
    }

    public void Explode()
    {
        ExplodeInDirection(Vector3.right); 
        ExplodeInDirection(Vector3.left);
        ExplodeInDirection(Vector3.forward);
        ExplodeInDirection(Vector3.back);
    }

    public void ExplodeInDirection(Vector3 direction)
    {
        Ray ray = new Ray(transform.position, direction);
        RaycastHit[] hits = Physics.SphereCastAll(ray, sphereCastRad, range, layerMask);
        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));   
        
        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.tag == "Unbreakable") break;
                if (hit.transform.tag == "Player")
                    hit.transform.GetComponent<MovementController>().DisablePlayer(); 
                hit.transform.gameObject.SetActive(false);
                if (hit.transform.tag == "Breakable") break ;
            }
        }
    }

    public void DisableObject()
    {
        gameObject.SetActive(false);
    }

    public void SetBombRange(int range)
    {
        this.range = range;
    }
}
