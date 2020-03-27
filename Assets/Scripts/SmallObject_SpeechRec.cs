﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;
using TMPro;


public class SmallObject_SpeechRec : MonoBehaviour
{
    public GameObject PlayerController; //reference to get the player controller
    public AudioSource AS; //reference for the audiosource on the player controller

    public bool InArtefactArea = false; //bool for checking if the player is in the target area
    public GameObject Artefact; //reference for the given artefact

    public bool SliderObject = false;

    public GameObject Temp;
    public string[] Keywords; //list of keywords that the player can say
    public AudioClip[] GatheredInfo; //list of relevant audioclips
    private KeywordRecognizer keywordRecogniser; //sets up speech rec
    public Dictionary<string, System.Action> actions = new Dictionary<string, System.Action>(); //dictionairy of keywords

    public TMP_Text ActiveCommand_Notebook;
    public TMP_Text ActiveInfo_Notebook;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerController = GameObject.FindGameObjectWithTag("Player");
        AS = PlayerController.AddComponent<AudioSource>(); //find the audio source on the player controller
    }

    // Update is called once per frame
    void Update()
    {
        if (SliderObject == true)
        {
            for (int j = 0; j < Temp.transform.childCount; j++)
            {
                if (Temp.transform.GetChild(j).gameObject.activeInHierarchy == true)
                {
                    //Debug.Log(Temp.transform.GetChild(j).gameObject.name);
                    Artefact = Temp.transform.GetChild(j).gameObject;

                }



            }
        }
    }
    public void OnTriggerStay(Collider other) //when this object's trigger area is entered
    {
        if (other.gameObject.tag == "InteractRing" & InArtefactArea == false) //if the object entering the trigger is the interact ring and the player hasnt already entered the area, latter is needed as sometimes the trigger would activate twice and bork the process
        {
            InArtefactArea = true; //turns the bool to true

            
            for (int i = 0; i < other.transform.parent.childCount; i++) //for each of the sibling gameobjects of the interact ring
            {
                if (other.gameObject.transform.parent.GetChild(i).gameObject.tag == "Artefact") //finds the artefact
                {

                    if (keywordRecogniser != null) //resets the keyword recogniser if it is not already null
                    {
                        keywordRecogniser.Dispose();
                    }




                    Artefact = other.gameObject.transform.parent.GetChild(i).gameObject; //assigns the artefact object
                    GatheredInfo = Artefact.GetComponent<AssignInformation>().AudioInfo; //gets the audio clips from the assign info script on the object


                    Keywords = Artefact.GetComponent<AssignInformation>().keywords; // gets the keywords from the assing info script on the object                    
                    actions.Add(Keywords[0], PointOfInterest1); //creates the commands from the keywords
                    actions.Add(Keywords[1], PointOfInterest2);
                    actions.Add(Keywords[2], PointOfInterest3);
                    actions.Add(Keywords[3], PointOfInterest4);
                    //Debug.Log("Break Two");


                    keywordRecogniser = new KeywordRecognizer(actions.Keys.ToArray()); //activates the speech rec
                    keywordRecogniser.OnPhraseRecognized += RecognisedSpeech;
                    keywordRecogniser.Start();

                    //Debug.Log("Break Three");

                }
                //TempObject = other.gameObject.transform.GetChild(i);
                //Debug.Log(other.transform.parent.GetChild(i));

                else if (other.gameObject.transform.parent.GetChild(i).gameObject.tag == "SliderArtefactParent")
                {
                    SliderObject = true;
                    Debug.Log("Found Slider Object");
                    Temp = other.gameObject.transform.parent.GetChild(i).gameObject;
                    for (int j = 0; j < Temp.transform.childCount; j++)
                    {
                        if (Temp.transform.GetChild(j).gameObject.activeInHierarchy == true)
                        {
                            //Debug.Log(Temp.transform.GetChild(j).gameObject.name);
                            Artefact = Temp.transform.GetChild(j).gameObject;

                        }
                        


                    }

                }
            }

        }
        
    }

    public void OnTriggerExit(Collider other) //resets everything when the player leaves the trigger area
    {
        if (other.gameObject.tag == "InteractRing")
        {

            InArtefactArea = false;
            SliderObject = false;
            Artefact = null;
            GatheredInfo = null;
            actions.Remove(Keywords[0]);
            actions.Remove(Keywords[1]);
            actions.Remove(Keywords[2]);
            actions.Remove(Keywords[3]);

            Keywords = null;
            keywordRecogniser.Dispose();
            keywordRecogniser.Stop();
            
            
            
            
            
        }

    }

    public void PointOfInterest1() //plays the releva
    {
        AS.PlayOneShot(GatheredInfo[0]);
        ActiveCommand_Notebook.text = Keywords[0];
        ActiveInfo_Notebook.text = Artefact.GetComponent<AssignInformation>().RelevantInfo[0];

    }
    public void PointOfInterest2()
    {
        AS.PlayOneShot(GatheredInfo[1]);
        ActiveCommand_Notebook.text = Keywords[1];
        ActiveInfo_Notebook.text = Artefact.GetComponent<AssignInformation>().RelevantInfo[1];

    }
    public void PointOfInterest3()
    {
        AS.PlayOneShot(GatheredInfo[2]);
        ActiveCommand_Notebook.text = Keywords[2];
        ActiveInfo_Notebook.text = Artefact.GetComponent<AssignInformation>().RelevantInfo[2];

    }
    public void PointOfInterest4()
    {
        AS.PlayOneShot(GatheredInfo[3]);
        ActiveCommand_Notebook.text = Keywords[3];
        ActiveInfo_Notebook.text = Artefact.GetComponent<AssignInformation>().RelevantInfo[3];

    }


    private void RecognisedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();

    }

    public void RegularArtefact()
    {




    }
}