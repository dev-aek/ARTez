using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    private const string RELOAD_ANIMATION_TRIGGER = "Reload";
    private const string RELOAD_ANIMATION_SPEED = "Anim_Speed";
    private const string RELOAD_ANIMATION = "Reload_Anim";
    protected const string RELOAD_TIME = "Reload_Time";
    protected const string SHOOTING_ANIMATION = "Fire_Anim";


    [SerializeField] protected Transform shootingPoint;
    [SerializeField] protected Animator gunAnimator;
    [SerializeField] protected GameObject bulletPrefab; // The bullet prefab
    [SerializeField] protected AmmoUI ammoUI; // Reference to the AmmoUI script to update ammo count
    [SerializeField] protected RectTransform crosshair; // Reference to the player's camera
    [SerializeField] protected ParticleSystem[] muzzleFlashs;
    [SerializeField] protected AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;


    [SerializeField] protected int maxAmmo = 30; // Maximum ammo
    private float reloadAnimationDuration;
    protected int currentAmmo; // Current ammo count
    protected bool isReloading = false; // Is the gun currently reloading

    protected virtual void Awake()
    {
        gunAnimator = GetComponent<Animator>();
        muzzleFlashs = GetComponentsInChildren<ParticleSystem>();
    }

    protected virtual void Start()
    {

        reloadAnimationDuration = GetAnimationClipDuration(RELOAD_ANIMATION);
        currentAmmo = maxAmmo;
        ammoUI.UpdateAmmoDisplay(currentAmmo);
    }

    public abstract void Fire();

    public abstract void StartFiring();

    public abstract void StopFiring();

    public void StartReload()
    {
        if (!isReloading && currentAmmo < maxAmmo) // Check if not already reloading and ammo is not full
        {
            StartCoroutine(Reload());
        }
    }

    protected virtual IEnumerator Reload()
    {
        isReloading = true;
        SoundManager.Instance.PlayReloadSound(reloadSound);
        gunAnimator.SetTrigger(RELOAD_ANIMATION_TRIGGER);
        float animationSpeed = gunAnimator.GetFloat(RELOAD_ANIMATION_SPEED);
        float animationDuration = reloadAnimationDuration * animationSpeed;

        yield return new WaitForSeconds(animationDuration);

        currentAmmo = maxAmmo; // Refill Ammo
        ShowCurrentAmmo(); // Update UI
        isReloading = false;
    }

    protected void ShowCurrentAmmo()
    {
        ammoUI.UpdateAmmoDisplay(currentAmmo);
    }

    protected GameObject CreateBulletAndShoot(Vector3 targetPoint)
    {
        if (currentAmmo > 0)
        {
            Vector3 shootingDirection = (targetPoint - shootingPoint.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.LookRotation(shootingDirection));
            bullet.GetComponent<Bullet>().Initialize(shootingDirection);
            return bullet;
        }
        return null;
    }

    float GetAnimationClipDuration(string animationName)
    {
        AnimationClip[] clips = gunAnimator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }
        return 0f;  // Return 0 if not found (handle this case appropriately)
    }

    protected void PlayMuzzleFlashParticle()
    {
        foreach(ParticleSystem muzzleFlash in muzzleFlashs)
        {
            muzzleFlash.Play();
        }
    }
}
