using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Gun
{
    public bool isShooting = false;
    private float firingRate = 0.1f;
    private float lastShotTime = 0f;
    private float tiltDelay = 0.5f; // Delay before tilting resets
    private float maxTilt = 200f; // Maximum tilt range
    private int shootCount = 0; // Count the number of shots

    private Coroutine firingCoroutine;

    public override void Fire()
    {
        if (isReloading) return;

        Vector3 targetPoint = ScreenRaycast.Instance.ScreenRaycastEquation(crosshair, shootingPoint);

        GameObject bullet = CreateBulletAndShoot(targetPoint);

        if (bullet != null && currentAmmo > 0 && !isReloading)
        {
            isShooting = true;
            SoundManager.Instance.PlayShootSound(shootSound);
            gunAnimator.Play(SHOOTING_ANIMATION, -1, 0f);
            PlayMuzzleFlashParticle();
            lastShotTime = Time.time;
            shootCount++;
            StartCoroutine(CrosshairTilting.Instance.TiltCrossHair(crosshair, shootCount, maxTilt));
            currentAmmo--;
            ShowCurrentAmmo(); // Update the ammo display each time a bullet is fired
            if (currentAmmo <= 0)
            {
                StartCoroutine(Reload());
                StartCoroutine(CrosshairTilting.Instance.ResetTilt(crosshair));
            }
        }
    }

    public override void StartFiring()
    {
        if(!isShooting)
        {
            isShooting = true;
            firingCoroutine = StartCoroutine(FireContinuously());
        }
    }

    public override void StopFiring()
    {
        if(isShooting)
        {
            StopCoroutine(firingCoroutine);
            isShooting = false;
        }
    }

    void Update()
    {
        if (!isShooting && (Time.time - lastShotTime) > tiltDelay)
        {
            StartCoroutine(CrosshairTilting.Instance.ResetTilt(crosshair));
            shootCount = 0; // Reset shot count after tilting resets
        }
    }

    IEnumerator FireContinuously()
    {
        while(isShooting)
        {
            Fire();
            yield return new WaitForSeconds(firingRate);
        }
    }
}
