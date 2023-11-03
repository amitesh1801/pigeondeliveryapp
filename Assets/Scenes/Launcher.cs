using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


namespace Com.MyCompany.MyGame
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields
        #endregion
        #region Private Fields
        [SerializeField]
        private byte maxPlayersPerRoom = 4;
        public GameObject ControlPanel;
        public GameObject ProgressLabel;
        public GameObject SettingsSymbol;

        string GameVersion = "1";

        #endregion
        #region MonoBehaviour CallBacks

        void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true; //make sure that the loaded scene is the same for every connected player
        }

        void Start()
        {
            SettingsSymbol.SetActive(false);
            ProgressLabel.SetActive(false);
            ControlPanel.SetActive(true);
            //Connect();
        }

        #endregion

        #region Public Methods

        void Update()
        {
           
        }

        public void Connect()  //connect to Photon Cloud
        {
            Debug.Log("CAME IN");
            ControlPanel.SetActive(false);
            ProgressLabel.SetActive(true);
            SettingsSymbol.SetActive(true);
            StartCoroutine(LoadToNextScene());
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = GameVersion;
            }
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinRandomRoom();
            Debug.Log("OnConnectedToMaster() was called by PUN");
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            ProgressLabel.SetActive(false);
            ControlPanel.SetActive(true);
            SettingsSymbol.SetActive(false);
            Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);
        }

        #endregion

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available.");
            PhotonNetwork.CreateRoom(null, new RoomOptions());
        }

        public override void OnJoinedRoom()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the 'Room for 1' ");
                PhotonNetwork.LoadLevel("Room for 1");
            }
            Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room.");
        }

        IEnumerator LoadToNextScene()
        {
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene(1);
        }
    }
}