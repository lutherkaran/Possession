using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayerTests
{
    CameraManager cameraManager;
    GameObject player;
    PlayerController playerController;
    float speed = 5f;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {

        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.gameObject.transform.position = new Vector3(0, 0, 0);

        GameObject Camera = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Camera"));
        player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        player.transform.position = new Vector3(0, 1f, 0);
        playerController = player.GetComponent<PlayerController>();
        Assert.NotNull(playerController, "Player is NULL");
        cameraManager = player.GetComponent<CameraManager>();
    }

    [UnityTest]
    public IEnumerator PlayerMovesForward()
    {
        Vector3 startPos = player.transform.position;
        Debug.Log("StartPos: " + startPos.z);

        playerController.ProcessMove(new Vector2(0, 1)); // Move forward
        for (int i = 0; i < 50; i++) yield return new WaitForFixedUpdate();

        Debug.Log("CurrentPos: " + player.transform.position.z);
        Assert.Greater(player.transform.position.z, startPos.z, "Player should move forward.");
    }

    [UnityTest]
    public IEnumerator PlayerMovesBackward()
    {
        Vector3 startPos = player.transform.position;
        Debug.Log("StartPos: " + startPos.z);

        playerController.ProcessMove(new Vector2(0, -1)); // Move backward
        for (int i = 0; i < 50; i++) yield return new WaitForFixedUpdate();

        Debug.Log("CurrentPos: " + player.transform.position.z);
        Assert.Less(player.transform.position.z, startPos.z, "Player should move backward.");
    }

    [UnityTest]
    public IEnumerator PlayerMovesRight()
    {
        Vector3 startPos = player.transform.position;
        Debug.Log("StartPos: " + startPos.x);

        playerController.ProcessMove(new Vector2(1, 0)); // Move right
        for (int i = 0; i < 50; i++) yield return new WaitForFixedUpdate();

        Debug.Log("CurrentPos: " + player.transform.position.x);
        Assert.Greater(player.transform.position.x, startPos.x, "Player should move right.");
    }

    [UnityTest]
    public IEnumerator PlayerMovesLeft()
    {
        Vector3 startPos = player.transform.position;
        Debug.Log("StartPos: " + startPos);

        playerController.ProcessMove(new Vector2(-1, 0)); // Move left
        for (int i = 0; i < 50; i++) yield return new WaitForFixedUpdate();

        Debug.Log("CurrentPos: " + player.transform.position.x);
        Assert.Less(player.transform.position.x, startPos.x, "Player should move left.");
    }

    [UnityTest]
    public IEnumerator PlayerDoesNotMoveWithoutInput()
    {
        Vector3 startPos = player.transform.position;
        for (int i = 0; i < 50; i++) yield return new WaitForFixedUpdate(); // Wait without movement
        Assert.AreEqual(startPos, player.transform.position, "Player should not move without input.");
    }

    [UnityTest]
    public IEnumerator PlayerMovesAtCorrectSpeed()
    {
        Vector3 startPos = player.transform.position;
        Debug.Log("StartPos: " + startPos);

        playerController.ProcessMove(new Vector2(0, 1)); // Move forward
        for (int i = 0; i < 50; i++) yield return new WaitForFixedUpdate();

        float distanceMoved = Vector3.Distance(startPos, player.transform.position);
        float expectedDistance = speed * 1f; // speed * time
        Debug.Log($"Expected: {expectedDistance}, Moved: {distanceMoved}");

        Assert.AreEqual(expectedDistance, distanceMoved, 0.5f, "Player should move at the correct speed.");
    }
}
