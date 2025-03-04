using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;

public class PlayerPossessionTests
{
    CameraManager cameraManager;
    GameObject player;
    PlayerController playerController;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {

        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.gameObject.transform.position = new Vector3(0, 0, 0);
        plane.gameObject.transform.localScale = (new Vector3(10, 1, 10));

        GameObject Camera = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Camera"));
        player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        player.transform.position = new Vector3(0, 0f, 0);
        playerController = player.GetComponent<PlayerController>();
        Assert.NotNull(playerController, "Player is NULL");
        cameraManager = player.GetComponent<CameraManager>();
    }

    //[UnityTest]
    //public IEnumerator CheckPossesion()
    //{
    //    GameObject enemy = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"));
    //    Assert.IsNotNull(enemy, "Enemy is not NULL");
    //    enemy.transform.position = new Vector3(0, 0, 20f);
    //    //enemy.transform.Rotate(0, 0, 0);
    //    enemy.GetComponent<StateMachine>().enabled = false;
    //    enemy.GetComponent<NavMeshAgent>().enabled = false;
    //    playerController.transform.LookAt(enemy.transform.position);
    //    playerController.PossessEntities();
    //    yield return new WaitForSeconds(2f);
    //    Assert.AreEqual(PossessionManager.Instance.currentlyPossessed, enemy);
    //}
}
