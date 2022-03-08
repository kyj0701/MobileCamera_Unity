using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour
{
    public static Client Instance { set; get; }
    private TcpClient socketConnection;

	public RawImage testImage;

    public CameraManagerScript CameraManager;
    public Text trajectory;
    public string traj;
    public string[] split_traj;
    public float qw, qx, qy, qz, tx, ty, tz;

    public void Awake()
  	{
        // ConnectToTcpServer();
    }

    private void ConnectToTcpServer()
    {
        try
        {
            socketConnection = new TcpClient("166.104.246.60", 17000);
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
            socketConnection = new TcpClient("166.104.246.60", 17000);
            // return;
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

    private void RecvMessage()
    {
        try
        {
            // Get a stream object for writing.             
            NetworkStream stream = socketConnection.GetStream();
            byte[] recvMessage = new Byte[1024];

            stream.Read(recvMessage, 0, recvMessage.Length);

            trajectory.text = System.Text.Encoding.UTF8.GetString(recvMessage);
            traj = trajectory.ToString();
            split_traj = traj.Split(' ');
            qw = float.Parse(split_traj[0]);
            qx = float.Parse(split_traj[1]);
            qy = float.Parse(split_traj[2]);
            qz = float.Parse(split_traj[3]);
            tx = float.Parse(split_traj[4]);
            ty = float.Parse(split_traj[5]);
            tz = float.Parse(split_traj[6]);

        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }

	public void SendToServerMessage()
	{
		string test = "test query.jpg 1024";
		byte[] byteMessage = null;

		byteMessage = System.Text.Encoding.Default.GetBytes(test);

		SendMessage(byteMessage);
	}

	public void SendToServerImage()
	{
        ConnectToTcpServer();

        CameraManagerScript CameraManager = GameObject.Find("CameraManager").GetComponent<CameraManagerScript>();
        Texture2D testImageTexture = new Texture2D(CameraManager.camTexture.width, CameraManager.camTexture.height);
        testImageTexture.SetPixels(CameraManager.camTexture.GetPixels());
        testImageTexture.Apply();
        byte[] byteTestImageTexture = testImageTexture.EncodeToPNG();

        string requestMessage = "kyj0701 CameraImage.png " + byteTestImageTexture.Length;
        Debug.Log(requestMessage);

        SendMessage(System.Text.Encoding.Default.GetBytes(requestMessage));

        SendMessage(byteTestImageTexture);
        SendMessage(System.Text.Encoding.Default.GetBytes("EOF"));

        RecvMessage();

        socketConnection = null;
	}

    async void CommunitcateWithServer()
    {
        await Task.Run(() =>
        {
            ConnectToTcpServer();

            CameraManagerScript CameraManager = GameObject.Find("CameraManager").GetComponent<CameraManagerScript>();
            Texture2D testImageTexture = new Texture2D(CameraManager.camTexture.width, CameraManager.camTexture.height);
            testImageTexture.SetPixels(CameraManager.camTexture.GetPixels());
            testImageTexture.Apply();
            byte[] byteTestImageTexture = testImageTexture.EncodeToPNG();

            string requestMessage = "kyj0701 CameraImage.png " + byteTestImageTexture.Length;
            Debug.Log(requestMessage);

            SendMessage(System.Text.Encoding.Default.GetBytes(requestMessage));

            SendMessage(byteTestImageTexture);
            SendMessage(System.Text.Encoding.Default.GetBytes("EOF"));

            RecvMessage();

            socketConnection = null;
        });
    }
}