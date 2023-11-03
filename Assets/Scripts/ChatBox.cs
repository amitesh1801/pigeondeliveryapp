
//Created By Kaushik NS...
//UnityDeveloper Intern PlatypusBox...

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine.UI;

public class ChatBox : MonoBehaviour, IChatClientListener
{
    public GameObject JoinChatButton;
    ChatClient chatclient; 
    bool IsConnected;
    public string userID;
    public GameObject ChatWindow;
    public GameObject ConnectingPanel;
    public InputField chatField;
    public Text chatDisplay;
    string privateReceiver = " ";
    string currentChat;
    public GameObject Password;
    public GameObject PasswordText;



    void Start()
    {
        Password.SetActive(false);
        PasswordText.SetActive(false);
        JoinChatButton.GetComponent<Button>().enabled = false;
    }

    void Update()
    {
        if (IsConnected)
        {
            chatclient.Service(); //To maintain the chat connection continously....
        }

        if(chatField.text != "" && Input.GetKey(KeyCode.Return))
        {
            SubmitTheMessage(); // function to publish the message on the Photon Chat to the channel members
        }
    }

    public void UserIDOnValueChange(string UserIDvalue)
    // To check when the userID is changed or newly typed in the Chat login window
    {
        if (UserIDvalue == "")
        {
            JoinChatButton.GetComponent<Button>().enabled = false;
        }
        else
        {
            JoinChatButton.GetComponent<Button>().enabled = true;
        }
        userID = UserIDvalue;
        if(UserIDvalue.ToLower() == "admin")
        {
            Password.SetActive(true);
        }
    }

    public void ChatConnectOnClick()
    //This fucntion is called when user clicks the join chat button
    {
        if (userID.ToLower() == "admin")
        {
            if (Password.GetComponent<InputField>().text == "PhotonChat123$")
            {
                PasswordText.SetActive(false);
                Debug.Log("Came into ChatConnectedOnClick()");
                IsConnected = true;
                chatclient = new ChatClient(this);
                chatclient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(userID));
                //It connects to the Photon Chat using the Chat Client....
            }
            else
            {
                PasswordText.SetActive(true);
            }
        }
        else
        {
            IsConnected = true;
            chatclient = new ChatClient(this);
            chatclient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(userID));
        }
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state)
    {
        //throw new System.NotImplementedException();
    }

   
    public void OnConnected()
    //Once connected to chat server , OnConnected() method is called....
    {
        //Debug.Log("Came into Connected()");
        if (userID.ToLower() == "admin")
        {
            if (Password.GetComponent<InputField>().text == "PhotonChat123$")
            {
                PasswordText.SetActive(false);
                JoinChatButton.SetActive(false); // Disables the join window so that the chat window can be shown
                chatclient.Subscribe(new string[] { "KaushikChannel" }); //Subscribes to the channel mentioned....
                StartCoroutine(ShowConnectingPanel()); // to show "connecting" message for a few seconds till the connection completes
            }
            else
            {
                PasswordText.SetActive(true);
            }
        }
        else
        {
            JoinChatButton.SetActive(false); // Disables the join window so that the chat window can be shown
            chatclient.Subscribe(new string[] { "KaushikChannel" }); //Subscribes to the channel mentioned....
            StartCoroutine(ShowConnectingPanel()); // to show "connecting" message for a few seconds till the connection completes
        }
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    // Standard fucntion to be defined because of implementing IChatClientListener
    {
        throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    // this is called when user subscribes to a channel
    {
        ChatWindow.SetActive(true); //once subscription successful, the chat window is shown
        ConnectingPanel.SetActive(false); //the panel that shows "Connecting...." text is removed as user is subcribed
    }

    public void OnUnsubscribed(string[] channels)
    // this is called when user unsubscribes from a channel
    {
        throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    // Standard fucntion to be defined because of implementing IChatClientListener
    // called when a user subcribes to the chat channel 
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    // Standard fucntion to be defined because of implementing IChatClientListener
    // called when a user unsubcribes from the chat channel 
    {
        throw new System.NotImplementedException();
    }

    public void SubmitTheMessage()
    // This function is called when the user clicks the send button in the chat window
    {
        //chatDisplay.color = new Color(0, 255, 0);
        if (userID.ToLower() == "admin")
        {
            // using the chatclient, this message is published on the chat channel
            // the message to be published is taken from the chat input text

            chatclient.PublishMessage("KaushikChannel", chatField.text);


            chatField.text = ""; // the input text is made empty after the message is published
            

        }
        else
        {
            chatclient.SendPrivateMessage("Admin", chatField.text);
            chatField.text = "";
            //currentChat = "";
        }
    }

    public void ReceiverOnValueChanged(string valueIn)
    {
        privateReceiver = valueIn;
    }

    public void TypePrivateChatOnValueChange(string valueIn)
    {
        currentChat = valueIn;
    }

    //public void SubmitPrivateChatOnClick()
    //{
    //    if (privateReceiver != "")
    //    {
    //        chatclient.SendPrivateMessage(privateReceiver, currentChat);
    //        chatField.text = "";
    //        currentChat = "";
    //    }
    //}

    public void OnDisconnected()
    // Standard fucntion to be defined because of implementing IChatClientListener
    // called when a disconnection happens 
    {
        //throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    // this function is called when client gets new messages from server
    // Number of senders is equal to number of messages.
    {
        string msgs = "";
        for (int i = 0; i < senders.Length; i++)
        {
            msgs = string.Format("{0}: {1}", senders[i], messages[i]);
            if (senders[i] == userID)
            {
                chatDisplay.text += "\n " + "<color=red>" + msgs + "</color>";
                Debug.Log(msgs);
            }
            else
            {
                chatDisplay.text += "\n " + msgs;
            }
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    // called when a subscribed user gets a private message from a sender
    // Not used in this project
    {
        string msgs;
        msgs = string.Format("(private) {0}: {1}", sender, message);
        if (sender == userID)
        {
            chatDisplay.text += "\n " + "<color=red>" + msgs + "</color>";
            Debug.Log(msgs);
        }
        else
        {
            chatDisplay.text += "\n " + msgs;
        }
        Debug.Log(msgs);
    }


    IEnumerator ShowConnectingPanel()
    // to show "connecting" message for a few seconds till the connection completes
    {
        ConnectingPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
    }
}