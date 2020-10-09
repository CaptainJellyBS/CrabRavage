using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrabAI : MonoBehaviour
{
    public static CrabAI Instance { get; private set; }

    public GameObject eyeLeft, eyeRight;
    public GameObject pupilLeft, pupilRight;

    public GameObject armLeft, armRight;

    public GameObject babyCrab;
    public Transform bcSpawnLeft, bcSpawnRight;

    public Material pupilMat, laserMat;
    public GameObject laser;

    public int availableMoves;

    public float moveSpeed;

    public float slamTelegraphTime, slamRecoveryDelay, slamDownTime, slamRecoverTime;
    public float armAngleNormal, armAngleUp, armAngleDown, armAngleHalf;

    public float slamAttackDuration;

    public float throwTelegraphTime1, throwTelegraphTime2;
    public int throwMinAmount, throwMaxAmount;

    public float laserXOffset, laserXDuration, laserXStartDelay, laserXEndDelay;
    public float laserFollowDuration, laserFollowSpeed, laserFollowStartDelay, laserFollowEndDelay;

    public float timeBetweenAttacks;

    bool eyesFollowPlayer = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(Behavior());
    }

    private void Update()
    {
        EyeRotation();

    }

    void EyeRotation()
    {
        if (!eyesFollowPlayer) { return; }
        eyeLeft.transform.LookAt(PlayerMovement.Instance.transform.position, Vector3.up);
        eyeRight.transform.LookAt(PlayerMovement.Instance.transform.position, Vector3.up);
    }

    IEnumerator Behavior()
    {
        while(true)
        {           
            yield return new WaitForSeconds(timeBetweenAttacks);
            yield return StartCoroutine(PickBehavior());
        }
    }

    IEnumerator PickBehavior()
    {
        switch(Random.Range(0,availableMoves))
        {
            case 3: yield return StartCoroutine(LaserEyesX()); break;
            case 2: yield return StartCoroutine(LaserEyesFollow()); break;
            case 0: yield return StartCoroutine(FollowAndSlam()); break;
            case 1: yield return StartCoroutine(DoubleSlam()); break;
            case 4: yield return StartCoroutine(ThrowBabyCrabs(Random.Range(throwMinAmount, throwMaxAmount))); break;
        }
    }

    IEnumerator LaserEyesX()
    {
        eyesFollowPlayer = false;
        Vector3 targetLeft = new Vector3(PlayerMovement.Instance.transform.position.x - laserXOffset, -3f, PlayerMovement.Instance.transform.position.z) - eyeLeft.transform.position;
        Vector3 targetRight = new Vector3(PlayerMovement.Instance.transform.position.x + laserXOffset, -3f, PlayerMovement.Instance.transform.position.z) - eyeRight.transform.position;

        GameObject laserLeft = Instantiate(laser, eyeLeft.transform.position, Quaternion.LookRotation(targetLeft, Vector3.up));
        GameObject laserRight = Instantiate(laser, eyeRight.transform.position, Quaternion.LookRotation(targetRight, Vector3.up));

        eyeLeft.transform.LookAt(targetLeft, Vector3.up);
        eyeRight.transform.LookAt(targetRight, Vector3.up);

        pupilLeft.GetComponent<Renderer>().material = laserMat;
        pupilRight.GetComponent<Renderer>().material = laserMat;

        yield return new WaitForSeconds(laserXStartDelay);
        float t = 0;

        while(t<laserXDuration)
        {
            t += Time.deltaTime;
            Vector3 l = new Vector3(Mathf.Lerp(targetLeft.x, targetRight.x, t / laserXDuration), targetLeft.y, targetLeft.z);
            Vector3 r = new Vector3(Mathf.Lerp(targetRight.x, targetLeft.x, t / laserXDuration), targetRight.y, targetRight.z);
            laserLeft.transform.rotation = Quaternion.LookRotation(l, Vector3.up);
            laserRight.transform.rotation = Quaternion.LookRotation(r, Vector3.up);

            eyeLeft.transform.LookAt(l, Vector3.up);
            eyeRight.transform.LookAt(r, Vector3.up);

            yield return null;
        }

        yield return new WaitForSeconds(laserXEndDelay);

        Destroy(laserLeft);
        Destroy(laserRight);

        pupilLeft.GetComponent<Renderer>().material = pupilMat;
        pupilRight.GetComponent<Renderer>().material = pupilMat;

        eyesFollowPlayer = true;
    }

    IEnumerator LaserEyesFollow()
    {
        eyesFollowPlayer = false;
        Vector3 target = PlayerMovement.Instance.transform.position;

        //Telegraph attack
        eyeLeft.transform.LookAt(target, Vector3.up);
        eyeRight.transform.LookAt(target, Vector3.up);

        pupilLeft.GetComponent<Renderer>().material = laserMat;
        pupilRight.GetComponent<Renderer>().material = laserMat;

        yield return new WaitForSeconds(laserFollowStartDelay);

        //Start Attack
        GameObject laserLeft = Instantiate(laser, eyeLeft.transform.position, Quaternion.LookRotation(target, Vector3.up));
        GameObject laserRight = Instantiate(laser, eyeRight.transform.position, Quaternion.LookRotation(target, Vector3.up));        

        float t = 0;
        while (t < laserFollowDuration)
        {
            Vector3 dir = (PlayerMovement.Instance.transform.position - target).normalized;
            target += dir * laserFollowSpeed * Time.deltaTime;
            laserLeft.transform.rotation = Quaternion.LookRotation(target - eyeLeft.transform.position, Vector3.up);
            laserRight.transform.rotation = Quaternion.LookRotation(target - eyeRight.transform.position, Vector3.up);
            t += Time.deltaTime;
            eyeLeft.transform.LookAt(target, Vector3.up);
            eyeRight.transform.LookAt(target, Vector3.up);
            yield return null;
        }

        //End Attack
        yield return new WaitForSeconds(laserFollowEndDelay);
        Destroy(laserRight);
        Destroy(laserLeft);
        eyesFollowPlayer = true;

        pupilLeft.GetComponent<Renderer>().material = pupilMat;
        pupilRight.GetComponent<Renderer>().material = pupilMat;
    }
    IEnumerator FollowAndSlam()
    {
        Vector3 dir;
        float t = 0.0f;

        while(t<slamAttackDuration)
        {
            t += Time.deltaTime;

            if(PlayerMovement.Instance.transform.position.x <= transform.position.x) { dir = Vector3.left; }
            else { dir = Vector3.right; }

            transform.position += dir * moveSpeed * Time.deltaTime;

            bool underLeftArm = PlayerMovement.Instance.transform.position.x <= transform.position.x - 3.0f && PlayerMovement.Instance.transform.position.x >= transform.position.x - 5.0f;
            bool underRightArm = PlayerMovement.Instance.transform.position.x >= transform.position.x + 3.0f && PlayerMovement.Instance.transform.position.x <= transform.position.x + 5.0f;

            if (underLeftArm)
            {
                yield return StartCoroutine(Slam(armLeft));
            }
            if(underRightArm)
            {
                yield return StartCoroutine(Slam(armRight));
            }

            yield return null;
        }
    }

    IEnumerator DoubleSlam()
    {
        StartCoroutine(Slam(armLeft));
        yield return StartCoroutine(Slam(armRight));
    }

    IEnumerator Slam(GameObject arm)
    {
        //telegraph attack
        float curAngle = armAngleNormal;
        float t = 0;

        float startZAngle = arm.transform.localRotation.eulerAngles.z;

        while(t<slamTelegraphTime)
        {
            t += Time.deltaTime;
            curAngle = Mathf.Lerp(armAngleNormal, armAngleUp, t / slamTelegraphTime);
            arm.transform.localRotation = Quaternion.Euler(curAngle, 0, startZAngle);
            yield return null;
        }

        t = 0;

        while(t<slamDownTime)
        {
            t += Time.deltaTime;
            curAngle = Mathf.Lerp(armAngleUp, armAngleDown, t / slamDownTime);
            arm.transform.localRotation = Quaternion.Euler(curAngle, 0, startZAngle);
            yield return null;
        }

        t = 0;

        yield return new WaitForSeconds(slamRecoveryDelay);
        
        while (t < slamRecoverTime)
        {
            t += Time.deltaTime;
            curAngle = Mathf.Lerp(armAngleDown, armAngleNormal, t / slamRecoverTime);
            arm.transform.localRotation = Quaternion.Euler(curAngle, 0, startZAngle);
            yield return null;
        }
    }

    IEnumerator ThrowBabyCrabs(int amount)
    {
        //telegraph attack
        float curAngle = armAngleNormal;
        float t = 0;

        float startZAngleL = armLeft.transform.localRotation.eulerAngles.z;
        float startZAngleR = armRight.transform.localRotation.eulerAngles.z;

        while (t < throwTelegraphTime1)
        {
            t += Time.deltaTime;
            curAngle = Mathf.Lerp(armAngleNormal, armAngleHalf, t / throwTelegraphTime1);
            armLeft.transform.localRotation = Quaternion.Euler(curAngle, 0, startZAngleL);
            armRight.transform.localRotation = Quaternion.Euler(curAngle, 0, startZAngleR);
            yield return null;
        }

        t = 0;

        while (t < throwTelegraphTime2)
        {
            t += Time.deltaTime;
            curAngle = Mathf.Lerp(armAngleHalf, armAngleUp, t / throwTelegraphTime2);
            armLeft.transform.localRotation = Quaternion.Euler(curAngle, 0, startZAngleL);
            armRight.transform.localRotation = Quaternion.Euler(curAngle, 0, startZAngleR);
            yield return null;
        }

        //throw amount of crabs

        bool left = true;
        for (int i = 0; i < amount; i++)
        {
            if (left)
            { Instantiate(babyCrab, bcSpawnLeft.position, Quaternion.identity); }
            else { Instantiate(babyCrab, bcSpawnRight.position, Quaternion.identity); }
            left = !left;
            yield return new WaitForSeconds(0.5f);
        }

        //recover

        t = 0;

        while (t < throwTelegraphTime1)
        {
            t += Time.deltaTime;
            curAngle = Mathf.Lerp(armAngleUp, armAngleNormal, t / throwTelegraphTime1);
            armLeft.transform.localRotation = Quaternion.Euler(curAngle, 0, startZAngleL);
            armRight.transform.localRotation = Quaternion.Euler(curAngle, 0, startZAngleR);
            yield return null;
        }
    }
}
