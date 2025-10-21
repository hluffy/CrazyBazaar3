using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{

    public static SceneTransitionManager Instance { get; private set; }

    public enum Location
    {
        Town,TreeHouse,PlayerHouse
    }
    public Location currentLocation;
     
    Transform player;
    PlayerCtl playerCtl;

    private void Awake()
    {
        if (Instance != null  )
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnLocationLoad;
        playerCtl = FindObjectOfType<PlayerCtl>();
        player = playerCtl?.transform;
    }

    public void SwitchLocation(Location locationSwitch)
    {
        SceneManager.LoadScene(locationSwitch.ToString());
    }

    public void OnLocationLoad(Scene scene,LoadSceneMode mode)
    {
        Location oldLocation = currentLocation;
        Location newLocation = (Location)Enum.Parse(typeof(Location), scene.name);
 
        if (currentLocation == newLocation) return;

        Transform startPoint = LocationManger.Instance.GetPlayerStartPosition(oldLocation);

        if (User.userState == UserState.Navi)
        {
            playerCtl.OnPassFinish();
        }
        player.position= startPoint.position;
        /*CharacterController playerCharacter = player.GetComponent<CharacterController>();
        playerCharacter.enabled = false;

        playerCharacter.transform.position= startPoint.position;
        playerCharacter.enabled = true;*/

        print("---------:"+ player.position);
        currentLocation = newLocation;
    }

}
