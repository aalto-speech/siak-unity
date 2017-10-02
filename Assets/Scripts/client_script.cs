using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;

using SimpleJSON;



/*

A class that communicates with the speech server:

Relevant functions:

  startSession()
  - checks that server is up and username and password are ok
  - returns 200, 401 or 500.

  defineWord(string newword)
  - define the word to be repeated
  - returns unique id number

  startRec()
  - starts upload for the chosen word
  - returns integer:
      1-5: score for successful evaluation
        0: package_received
       -1: audio_end (time to stop recording!)
       -2: segmentation failure * Player's fault
       -3: segmentation_error * Server's fault
       -4  classification_error * Server's fault
       -5: no_audio_activity * Microphone problem
       -6: mic_off * Microphone problem
       -7: late_arrival * Network problem
       -8: duplicate * Network problem
       -9: network_lag_error * Network problem
     -100  generic_server_error

  getWordlist(string newlevel)
  - get a list of words from the server
  - returns a list of words in JSON

 */


public class client_script : MonoBehaviour {

    // Variables for the game project:
    string recUrl = "https://asr.aalto.fi/siak-devel/asr";
    string logUrl = "https://asr.aalto.fi/siak-devel/log-action";
    string loginUrl = "https://asr.aalto.fi/siak-devel/login";
    string wordListUrl = "https://asr.aalto.fi/siak-devel/start-level";
    string exitUrl = "https://asr.aalto.fi/siak-devel/exit-level";

    public string playername = "";
    public string playerpassword = "";

    public string serverState = "not ok";
    public string wordID = "-1";
    public string scoreFromServer = "-1";
    public string wordListJSON = "[]";


    public Boolean isRecording = false;

    // if or when the server crashes during the game session, the password will
    // be useful to avoid logging in again. Should be strongly encrypted in https anyway!

    // Variables for defining audio:
    int fs = 16000;
    int packetspersecond = 3;
    int maxAudioLen = 4; //seconds

    string currentword = "choose";
    string currentlevel = "L0";
    string clientId;

    // Variables for recording:
    string micstring;
    AudioSource aud;
    int lastPos;
    bool micOn = false;
    int recstart;

    // Variables for controlling packet sending:
    int sentpacketnr = 0;
    int samplessent = 0;
    bool finalpacket = false;
    int packetsize;




    // Use this for initialization
    void Start() {
        playername = GameManager.GetUsername();
        playerpassword = GameManager.GetPassword();
        // Calculate some essential values:
        packetsize = (int)Math.Floor((double)(fs / packetspersecond));

        foreach (string device in Microphone.devices) {
            Debug.Log("Name: " + device);
        }

        // Hard-coded to use mic number 0; Bad Idea but where to choose these?
        //micstring = Microphone.devices[Microphone.devices.Length-1];
        micstring = Microphone.devices[0]; //changed from original to access the mic on this computer correctly...!

        recstart = Environment.TickCount;

        sentpacketnr = 0;
        finalpacket = false;
        GameManager.SetServer(this);
        aud = GetComponent<AudioSource>();
    }


    //
    //
    //  This bit should be replaced by commands coming from 
    //  the game engine. Now how would we do that?
    //
    // 
    // Update is called once per frame
    void Update() {

        /* This is being done from inside the game in the playmaker script (CardFrame:FSM:PrepareForPlay, State14, Start_Recording 2, Stop_Recording) now...!
         * 

         if (Input.GetKeyUp(KeyCode.S)) {
           startSession();
         }

         if (Input.GetKeyUp(KeyCode.D))
         {
         defineWord();
         }

         // Simple controls for recording with key "r" pressed down:   
         else if (Input.GetKeyDown(KeyCode.R) && micOn == false)
         {
         recstart = Environment.TickCount;
         Debug.Log("Starting second: " + recstart.ToString());
         startRec();            
         }
         else if (micOn == true && ( Input.GetKeyUp(KeyCode.R) ))// || recstart + 1000*maxAudioLen < Environment.TickCount ) ) //I've removed the 10 second waiting time ...!
         {
         Debug.Log("Stop recording: " + System.DateTime.Now);
         // Stop recording!
         stopRec();
         }
        */
        if (isRecording || finalpacket)
            checkStartUpload();

    }

    public void startRec() {

        isRecording = true;

        //Start recording:
        // Device 0, no looping, 10 s record at 16 kHz:
        if (aud.clip != null)
            Destroy(aud.clip);
        aud.clip = Microphone.Start(micstring, false, maxAudioLen, fs);
        micOn = true;

        sentpacketnr = 0;
        samplessent = 0;
        finalpacket = false;

        //aud.Play();
    }

    void stopRec() {
        if (!finalpacket && isRecording) {
            Debug.Log("Setting finalpacket = true");
            finalpacket = true;
            micOn = false;
            lastPos = Microphone.GetPosition(micstring);
            Microphone.End(micstring);
        }
    }

    void checkStartUpload() {
        int writeHead = Microphone.GetPosition(micstring);
        //Debug.Log("writeHead: "+writeHead.ToString());    

        if (aud && ((micOn && writeHead > samplessent + packetsize) || finalpacket)) {
            int thispacketsize = packetsize;
            // The last packet might be smaller than the standard packet size:
            /*if (finalpacket) {
                thispacketsize = writeHead - samplessent;
            }*/

            // Copy the relevant audio data to a float array at the samplessent point:
            float[] samples = new float[thispacketsize];
            aud.clip.GetData(samples, samplessent);

            startUpload(sentpacketnr++, finalpacket, samplessent, samples);


            // Book-keeping for packets:
            samplessent += samples.Length;

            if (finalpacket) {
                finalpacket = false;
                if (lastPos > 0) {
                    float[] bigSamples = new float[aud.clip.samples];
                    aud.clip.GetData(bigSamples, 0);
                    float[] newSamples = new float[lastPos];
                    for (int i = 0; i < lastPos; i++) {
                        newSamples[i] = bigSamples[i];
                    }
                    bigSamples[0] = 0.1f;
                    AudioClip shortClip = AudioClip.Create("trimmedRecording", lastPos, aud.clip.channels, aud.clip.frequency, false);
                    shortClip.SetData(newSamples, 0);
                    Destroy(aud.clip);
                    aud.clip = shortClip;
                }
                isRecording = false;
            }
        }


    }

    public void startSession() {

        WWWForm sessionStartForm = new WWWForm();

        var customheaders = sessionStartForm.headers;
        customheaders = addCustomHeaders(customheaders, "-2", null);

        // Start the upload in a new thread:
        StartCoroutine(patientlyStartSession(recUrl, customheaders));

        return;
    }


    public void defineWord(string newword) {
        currentword = newword;
        defineWord();
    }

    void defineWord() {
        WWWForm sessionStartForm = new WWWForm();

        var customheaders = addCustomHeaders(sessionStartForm.headers, "-1", currentword);

        // Start the upload in a new thread:
        StartCoroutine(patientlyDefineWord(recUrl, customheaders));
    }

    public void getWordList(string newlevel) {
        currentlevel = newlevel;

        WWWForm getWordListForm = new WWWForm();
        var customheaders = addCustomHeaders(getWordListForm.headers, "-2", currentword);

        // Start the upload in a new thread:
        StartCoroutine(patientlyGetWordList(wordListUrl, customheaders));
    }

    public void login() {
        playername = GameManager.GetUsername();
        playerpassword = GameManager.GetPassword();
        WWWForm sessionStartForm = new WWWForm();
        var customheaders = new System.Collections.Generic.Dictionary<string, string>();
        customheaders.Add("x-siak-user", playername);
        customheaders.Add("x-siak-password", playerpassword);
        StartCoroutine(patientlyLogin(loginUrl, customheaders));
    }

    IEnumerator patientlyLogin(string targetUrl, System.Collections.Generic.Dictionary<string, string> customheaders) {
        WWW wwwResponse = new WWW(targetUrl, null, customheaders);
        yield return wwwResponse;
        GameManager.GetLoginScreen().GiveResponse(wwwResponse);
    }

    public void exit(int stars, System.Collections.Generic.Dictionary<string, string> spends) {
        JSONClass root = new JSONClass();
        JSONClass levelStars = new JSONClass();
        JSONClass objects = new JSONClass();

        levelStars[LevelManager.GetLevel().ToString()] = new JSONData(stars);
        root["star_update"] = levelStars;

        System.Collections.Generic.Dictionary<string, string>.KeyCollection keys = spends.Keys;

        foreach (string s in keys)
            objects[s] = spends[s];

        root["object_update"] = objects;
        root["highest_level"] = new JSONData(GameManager.GetCompleted());
        MemoryStream stream = new MemoryStream();
        BinaryWriter bin = new BinaryWriter(stream);
        root.Serialize(bin);

        WWWForm exitform = new WWWForm();
        var customheaders = exitform.headers;
        customheaders.Add("x-siak-user", playername);
        customheaders.Add("x-siak-password", playerpassword);
        customheaders.Add("x-siak-level", LevelManager.GetLevel().ToString());
        byte[] data = Encoding.ASCII.GetBytes(root.ToString().ToCharArray());
        Debug.Log(root.ToString());
        StartCoroutine(patientlyExit(exitUrl, data, customheaders));
    }

    IEnumerator patientlyExit(string targeturl, byte[] bytedata, System.Collections.Generic.Dictionary<string, string> customheaders) {
        WWW wwwRec = new WWW(targeturl, bytedata, customheaders);

        yield return wwwRec;

        Debug.Log(wwwRec.text);
    }

    IEnumerator patientlyStartSession(string targetUrl, System.Collections.Generic.Dictionary<string, string> customheaders) {
        WWW wwwResponse = new WWW(targetUrl, null, customheaders);

        yield return wwwResponse;
        // Our answer from the server:
        Debug.Log("Server State: " + wwwResponse.text);
        serverState = wwwResponse.text;
    }


    IEnumerator patientlyDefineWord(string targetUrl, System.Collections.Generic.Dictionary<string, string> customheaders) {
        WWW wwwResponse = new WWW(targetUrl, null, customheaders);

        yield return wwwResponse;
        // Our answer from the server:
        Debug.Log("WordID on Server: " + wwwResponse.text);
        wordID = wwwResponse.text;
    }

    IEnumerator patientlyGetWordList(string targetUrl, System.Collections.Generic.Dictionary<string, string> customheaders) {
        WWW wwwResponse = new WWW(targetUrl, null, customheaders);

        yield return wwwResponse;
        // Our answer from the server:
        Debug.Log("WordList from Server: " + wwwResponse.text);
        wordListJSON = wwwResponse.text;
        LevelManager.ProcessWordList(wordListJSON);
    }

    void startUpload(int thispacketnr, bool thisfinalpacket, int startsample, float[] samples) {
        // Make a byte array of the float array:

        // from http://stackoverflow.com/questions/4635769/how-do-i-convert-an-array-of-floats-to-a-byte-and-back
        // This only copies the first item in the float array.
        // var byteArray = new byte[audiodata.Length * 4];
        // Buffer.BlockCopy(audiodata, 0, byteArray, 0, byteArray.Length);

        var bytesamples = new byte[(samples.Length * 4)];
        Buffer.BlockCopy(samples, 0, bytesamples, 0, samples.Length * 4);

        WWWForm audioForm = new WWWForm();

        var customheaders = audioForm.headers;
        addCustomHeaders(customheaders, thispacketnr.ToString(), currentword); // should this be customheaders = addCustomHeaders(.,.,.)....???!

        customheaders["X-siak-packet-arraystart"] = startsample.ToString();
        customheaders["X-siak-packet-arrayend"] = (startsample + samples.Length).ToString();
        customheaders["X-siak-packet-arraylength"] = (samples.Length).ToString();
        customheaders["X-siak-final-packet"] = finalpacket == true ? "true" : "false";

        // Start the upload in a new thread:
        StartCoroutine(patientlyUpload(recUrl, bytesamples, customheaders));
    }

    IEnumerator doSomeLogging(string actionName) // is this being used at all...?
    {
        WWWForm loggingForm = new WWWForm();

        var customheaders = addCustomHeaders(loggingForm.headers, null, null);
        customheaders["X-siak-game-action"] = actionName;

        WWW wwwRec = new WWW(logUrl, null, customheaders);

        yield return wwwRec;
    }

    IEnumerator patientlyUpload(string targeturl, byte[] bytedata, System.Collections.Generic.Dictionary<string, string> customheaders) {
        Debug.Log("starting upload");
        // Uploading:
        WWW wwwRec = new WWW(targeturl, Encoding.UTF8.GetBytes(Convert.ToBase64String(bytedata)), customheaders);

        yield return wwwRec;

        // Our answer from the server:
        scoreFromServer = wwwRec.text;// so that any score can be handled in unity
        Debug.Log("Score: " + ((wwwRec.text == "") ? "-9" : wwwRec.text)); //is the score supposed to improve as more data is sent..???

        if (wwwRec.text == "-1") {
            stopRec();
        } else if (wwwRec.text != "0") {
            stopRec();
            scoreFromServer = (wwwRec.text == "") ? "-9" : wwwRec.text;
        }
    }

    System.Collections.Generic.Dictionary<string, string> addCustomHeaders(System.Collections.Generic.Dictionary<string, string> customheaders, string packetnr, string word) {

        customheaders["X-siak-user"] = playername;
        customheaders["X-siak-password"] = playerpassword;
        customheaders["X-siak-level"] = currentlevel;
        customheaders["X-siak-region"] = "2";
        customheaders["X-siak-current-word-id"] = wordID;
        //customheaders["x-siak-client-id"] = clientId;



        if (packetnr != null)
            customheaders["X-siak-packetnr"] = packetnr;
        else
            customheaders["X-siak-packetnr"] = "-2";

        if (word != null)
            customheaders["X-siak-current-word"] = currentword;

        return customheaders;
    }

    public void SetClientId(string s) {
        clientId = s;
    }
}
