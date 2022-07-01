
using UnityEngine;
using System.Collections;
using TMPro;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;

    private bool isReloading = false;


    public int maxAmmo = 10;

    public int stashedAmmo = 60;
    private int currentAmmo;
    public float reloadTime = 1f;

    public Camera fpsCam;

    public ParticleSystem muzzleFlash;

    public GameObject impactEffect;

    public GameObject bloodEffect;

    private float nextTimeToFire = 0f;

    public Vector3 recoil = new Vector3(0, 1, 0);

    public Animator animator;

    public TextMeshProUGUI ammoText;

    void Start() {
        currentAmmo = maxAmmo;
    }

    void OnEnable() {
        isReloading = false;
        animator.SetBool("Reloading", false);
        ammoText.text = currentAmmo.ToString() + "/" + stashedAmmo.ToString();
    }


    // Update is called once per frame
    void Update()
    {
        if (isReloading) {
            return;
        }

        if (currentAmmo <= 0 && stashedAmmo > 0) {
            StartCoroutine(Reload());
            return;
        }
        if (Input.GetKeyDown(KeyCode.R) && stashedAmmo > 0) {
            //Debug.Log("reload");
            StartCoroutine(Reload());
        }
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && currentAmmo > 0) 
        {
            nextTimeToFire = Time.time + 1f/fireRate;
            Shoot();
        }
    }

    IEnumerator Reload() {
        isReloading = true;
        //Debug.Log(currentAmmo);

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime - .25f);

        animator.SetBool("Reloading", false);

        yield return new WaitForSeconds(.25f);
        int usedAmmo = maxAmmo - currentAmmo;
        currentAmmo = stashedAmmo <= maxAmmo ? stashedAmmo : maxAmmo;
        stashedAmmo = Mathf.Max(stashedAmmo - usedAmmo, 0);
        ammoText.text = currentAmmo.ToString() + "/" + stashedAmmo.ToString();
        isReloading = false;
    }

    void Shoot() 
    {
        muzzleFlash.Play();

        //this.transform.Rotate(3, 0, 0, Space.World);

        currentAmmo--;

        ammoText.text = currentAmmo.ToString() + "/" + stashedAmmo.ToString();


        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)) {
            //Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null) {
                target.TakeDamage(damage);
            }
            if (hit.transform.tag == "Player") {
                GameObject impactGO = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);
            }
            else {
                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);
            }

        }
    }
}
