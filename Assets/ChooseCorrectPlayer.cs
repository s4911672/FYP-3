﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCorrectPlayer : MonoBehaviour
{
    public GameObject GamepadPlayer; //references for the players versions
    public GameObject VRSRPlayer;
    public GameObject VRLMSRPlayer;

    public GameObject Player;

    public bool DebugSpawnS;
    public bool DebugSpawnVR;
    public bool DebugSpawnVRR;

    public GameObject ModalityController;
    // Start is called before the first frame update
    void Awake()
    {
        ModalityController = GameObject.FindGameObjectWithTag("ModalityController");
        if (ModalityController.GetComponent<ModalityController2>().Gamepad_Chosen == true)
        {
            Player =Instantiate(GamepadPlayer, gameObject.transform.position, gameObject.transform.rotation, transform.parent);
            /*
            GamepadPlayer.SetActive(true);
            VRSRPlayer.SetActive(false);
            VRLMSRPlayer.SetActive(false);
            */
        }

        else if (ModalityController.GetComponent<ModalityController2>().VRSR_Chosen == true)
        {
            Player = Instantiate(VRSRPlayer, gameObject.transform.position, gameObject.transform.rotation, transform.parent);
        }

        else if (ModalityController.GetComponent<ModalityController2>().VRLMSR_Chosen == true)
        {
            Player = Instantiate(VRLMSRPlayer, gameObject.transform.position, gameObject.transform.rotation, transform.parent);
        }

        Player.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}