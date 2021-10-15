using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour
{
    private TcpClient socketConnection;

	public RawImage testImage;

    public CameraManagerScript CameraManager;

    public void Start()
  	{
        ConnectToTcpServer();
    }

    private void ConnectToTcpServer()
    {
        try
        {
            socketConnection = new TcpClient("125.130.0.42", 17000);
        }
        catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
        }
    }

    /// Send message to server using socket connection.     
    private void SendMessage(Byte[] buffer)
    {
        if (socketConnection == null)
        {
            return;
        }
        try
        {
            // Get a stream object for writing.             
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                // Write byte array to socketConnection stream.                 
                stream.Write(buffer, 0, buffer.Length);
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }

	public void SendToServerMessage()
	{
		string test = "id test_message 1024";
		byte[] byteMessage = null;

		byteMessage = System.Text.Encoding.Default.GetBytes(test);

		SendMessage(byteMessage);
	}

	public void SendToServerImage()
	{
        CameraManagerScript CameraManager = GameObject.Find("CameraManager").GetComponent<CameraManagerScript>();
        Texture2D testImageTexture = new Texture2D(CameraManager.camTexture.width, CameraManager.camTexture.height);
        testImageTexture.SetPixels(CameraManager.camTexture.GetPixels());
        testImageTexture.Apply();
        byte[] byteTestImageTexture = testImageTexture.EncodeToPNG();
        // string userID = "kyj0701";
        // byte[] byteUserID = System.Text.Encoding.Default.GetBytes(userID);
        // string imageSize = "1024";
        // byte[] byteImageSize = System.Text.Encoding.Default.GetBytes(imageSize);
        // byte[] byteMessage = new byte[byteUserID.Length + byteTestImageTexture.Length + byteImageSize.Length];

        // Array.Copy(byteUserID, 0, byteMessage, 0, byteUserID.Length);
        // Array.Copy(byteTestImageTexture, 0, byteMessage, byteUserID.Length, byteTestImageTexture.Length);
        // Array.Copy(byteImageSize, 0, byteMessage, byteUserID.Length + byteTestImageTexture.Length, byteImageSize.Length);

        string requestMessage = "kyj0701 CameraImage.png" + byteTestImageTexture.Length;
        Debug.Log(requestMessage);
        byte[] byteMessage = System.Text.Encoding.Default.GetBytes(requestMessage);

		SendMessage(byteMessage);

        byteMessage = byteTestImageTexture;
        SendMessage(byteMessage);
	}
}