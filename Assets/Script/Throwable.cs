using UnityEngine;

public class Throwable : MonoBehaviour
{
    public Camera fpsCam;
    public float AOE = 5f;

    public float maximumDamage = 75f;

    public float fuzeTime = 3f;

    private float timeToExplode = Mathf.Infinity;

    public GameObject explosionEffect;

    Rigidbody grenadeRigid;

    Collider grenadeCollider;

    bool explosionDone = false;

    bool thrown = false;

    // Update is called once per frame

    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        grenadeRigid = GetComponent<Rigidbody>();
        grenadeCollider = GetComponent<CapsuleCollider>();
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !thrown) 
        {
            timeToExplode = Time.time + fuzeTime;
            grenadeRigid.AddForce(fpsCam.transform.forward * 1000f);
            grenadeRigid.useGravity = true;
            grenadeCollider.isTrigger = false;
            transform.parent = null;
            thrown = true;
        }
        else if (Input.GetButtonDown("Fire2") && !thrown) 
        {
            timeToExplode = Time.time + fuzeTime;
            grenadeRigid.AddForce(fpsCam.transform.forward * 500f);
            grenadeRigid.useGravity = true;
            grenadeCollider.isTrigger = false;
            transform.parent = null;
            thrown = true;
        }
        if (Time.time >= timeToExplode && !explosionDone) {
            Explode();
        }
    }



    void Explode() {


        GameObject explosionGO = Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, AOE);
        if (!explosionDone) {
            foreach (Collider objectInRadius in colliders) {
                Target target = objectInRadius.transform.GetComponent<Target>();
                if (target != null) {
                    Vector3 distance = objectInRadius.transform.position - transform.position;
                    float damage = maximumDamage - Mathf.Abs(distance.magnitude) * 10f;
                    //Debug.Log(Mathf.Abs(distance.magnitude));
                    //Debug.Log(damage);
                    if (damage > 0) {
                        target.TakeDamage(damage);
                    }
                }
            }
            explosionDone = true;
        }
        
        Destroy(explosionGO, 12f);
        Destroy(gameObject);
    }
}
