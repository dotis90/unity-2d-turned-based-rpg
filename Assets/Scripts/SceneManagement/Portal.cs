﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] int sceneToLoad = -1;
    [SerializeField] Transform spawnPoint;

    PlayerController player;

    public void OnPlayerTriggered(PlayerController player)
    {
        this.player = player;
        StartCoroutine(SwitchScene());
    }

    IEnumerator SwitchScene()
    {
        DontDestroyOnLoad(gameObject);

        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        var destinationPortal = FindObjectsOfType<Portal>().First(x => x != this);
        player.Characters.SetPositionAndSnapToTile(destinationPortal.spawnPoint.position);

        Destroy(gameObject);
    }

    public Transform SpawnPoint => spawnPoint;
}
