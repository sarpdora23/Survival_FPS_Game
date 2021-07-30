using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private WeaponManager weapon_manager;
    public float FireRate = 15f;
    private float nextTimetoFire;
    public float damage = 20f;
    private Animator zoomIn_anim;
    private bool zoomed;
    private Camera mainCam;
    private GameObject crosshair;
    private bool is_Aiming;
    [SerializeField]
    private GameObject arrowPrefab, spearPrefab;
    [SerializeField]
    private Transform arrowStartPosition;
    // Start is called before the first frame update
    void Awake()
    {
        weapon_manager = gameObject.GetComponent<WeaponManager>();
        zoomIn_anim = gameObject.transform.Find(Tags.LOOK_ROOT).
            Find(Tags.ZOOM_CAMERA).GetComponent<Animator>();
        crosshair = GameObject.FindGameObjectWithTag(Tags.CROSSHAIR);
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        WeaponShoot();
        ZoomInOut();
    }
    void WeaponShoot()
    {
        if (weapon_manager.GetCurrentSelectedWeapon().fireType == WeaponHandler.WeaponFireType.MULTIPLE)
        {
            if (Input.GetMouseButton(0) && Time.time > nextTimetoFire)
            {
                nextTimetoFire = Time.time + 1f / FireRate;
                weapon_manager.GetCurrentSelectedWeapon().ShootAnimation();
                BulletFire();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (weapon_manager.GetCurrentSelectedWeapon().tag == Tags.AXE_TAG)
                {
                    weapon_manager.GetCurrentSelectedWeapon().ShootAnimation();
                }
                if (weapon_manager.GetCurrentSelectedWeapon().bulletType == WeaponHandler.WeaponBulletType.BULLET)
                {
                    weapon_manager.GetCurrentSelectedWeapon().ShootAnimation();
                    BulletFire();

                }
                else
                {
                    if (is_Aiming)
                    {
                        weapon_manager.GetCurrentSelectedWeapon().ShootAnimation();
                        if (weapon_manager.GetCurrentSelectedWeapon().bulletType == WeaponHandler.WeaponBulletType.ARROW)
                        {
                            ThrowSpearorArrow(true);
                        }
                        else if (weapon_manager.GetCurrentSelectedWeapon().bulletType == WeaponHandler.WeaponBulletType.SPEAR)
                        {
                            ThrowSpearorArrow(false);
                        }
                    }
                }
            }
        }
    }
    void BulletFire()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCam.transform.position,mainCam.transform.forward,out hit))
        {
            if (hit.transform.tag == Tags.ENEMY_TAG)
            {
                hit.transform.GetComponent<HealthScript>().ApplyDamage(damage);
            }
        }
    }
    void ThrowSpearorArrow(bool isArrow)
    {
        if (isArrow)
        {
            GameObject arrow = Instantiate(arrowPrefab);
            arrow.transform.position = arrowStartPosition.position;
            arrow.GetComponent<BowArrow>().Launch(mainCam);
        }
        else
        {
            GameObject spear = Instantiate(spearPrefab);
            spear.transform.position = arrowStartPosition.position;
            spear.GetComponent<BowArrow>().Launch(mainCam);
        }
    }
    void ZoomInOut()
    {
        if (weapon_manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponHandler.WeaponAim.AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                zoomIn_anim.Play(AnimationTags.ZOOM_IN_ANIM);
                crosshair.SetActive(false);
            }
            if (Input.GetMouseButtonUp(1))
            {
                zoomIn_anim.Play(AnimationTags.ZOOM_OUT_ANIM);
                crosshair.SetActive(true);
            }
        }
        if (weapon_manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponHandler.WeaponAim.SELF_AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                weapon_manager.GetCurrentSelectedWeapon().Aim(true);
                is_Aiming = true;
            }
            if (Input.GetMouseButtonUp(1))
            {
                weapon_manager.GetCurrentSelectedWeapon().Aim(false);
                is_Aiming = false;
            }
        }
    }
}
