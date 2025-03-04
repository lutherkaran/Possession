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
        player.transform.position = new Vector3(0, 0f, 0);
        playerController = player.GetComponent<PlayerController>();
        Assert.NotNull(playerController, "Player is NULL");
        cameraManager = player.GetComponent<CameraManager>();
    }
    #region PlayerMovement
    [UnityTest]
    public IEnumerator PlayerMovesForward()
    {
        Vector3 startPos = player.transform.position;
        Debug.Log("StartPos: " + startPos);

        playerController.ProcessMove(new Vector2(0, 1).normalized); // Move forward
        yield return new WaitForSeconds(1f);

        Debug.Log("CurrentPos: " + player.transform.position);
        Assert.Greater(startPos.z, player.transform.position.z, "Player should move forward.");
    }

    [UnityTest]
    public IEnumerator PlayerMovesBackward()
    {
        Vector3 startPos = player.transform.position;
        Debug.Log("StartPos: " + startPos);

        playerController.ProcessMove(new Vector2(0, -1).normalized); // Move backward
        yield return new WaitForSeconds(1f);

        Debug.Log("CurrentPos: " + player.transform.position);
        Assert.Less(startPos.z, player.transform.position.z, "Player should move backward.");
    }

    [UnityTest]
    public IEnumerator PlayerMovesRight()
    {
        Vector3 startPos = player.transform.position;
        Debug.Log("StartPos: " + startPos);

        playerController.ProcessMove(new Vector2(1, 0).normalized); // Move right
        yield return new WaitForSeconds(1f);

        Debug.Log("CurrentPos: " + player.transform.position);
        Assert.Greater(startPos.x, player.transform.position.x, "Player should move right.");
    }

    [UnityTest]
    public IEnumerator PlayerMovesLeft()
    {
        Vector3 startPos = player.transform.position;
        Debug.Log("StartPos: " + startPos);

        playerController.ProcessMove(new Vector2(-1, 0).normalized); // Move left
        yield return new WaitForSeconds(1f);

        Debug.Log("CurrentPos: " + player.transform.position);
        Assert.Less(startPos.x, player.transform.position.x, "Player should move left.");
    }

    [UnityTest]
    public IEnumerator PlayerDoesNotMoveWithoutInput()
    {
        Vector3 startPos = player.transform.position;
        yield return new WaitForSeconds(1f);
        Assert.AreEqual(player.transform.position, startPos, "Player should not move without input.");
    }

    [UnityTest]
    public IEnumerator PlayerMovesAtCorrectSpeed()
    {
        Vector3 startPos = player.transform.position;
        playerController.ProcessMove(new Vector2(0, 1).normalized); // Move forward
        yield return new WaitForSeconds(1f);

        float distanceMoved = Vector3.Distance(startPos, player.transform.position);
        float expectedDistance = speed * 1f * Time.fixedDeltaTime;
        Debug.Log("Expected: " + expectedDistance + " Actual: " + distanceMoved);
        Assert.AreEqual(expectedDistance, distanceMoved, 0.5f, "Player should move at the correct speed.");
    }
    #endregion

    [UnityTest]
    public IEnumerator CheckPlayerHealthUI()
    {
        PlayerHealthUI playerHealthUI = playerController.GetPlayerHealthUIReference();
        yield return new WaitForSeconds(1f);
        Assert.IsNotNull(playerHealthUI);
    }

    [UnityTest]
    public IEnumerator CheckPlayerInputManagerReference()
    {
        InputManager inputManager = playerController.GetInputManager();
        yield return new WaitForSeconds(1f);
        Assert.IsNotNull(inputManager);
    }

    [UnityTest]
    public IEnumerator CheckCharacterControllerReference()
    {
        CharacterController characterController = playerController.GetCharacterControllerReference();
        yield return new WaitForSeconds(1f);
        Assert.IsNotNull(characterController);
    }
}
