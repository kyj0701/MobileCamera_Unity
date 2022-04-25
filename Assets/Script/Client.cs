using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour
{
    // private static Client instance = null;
    public static Client Instance { get; set; }
    private TcpClient socketConnection;
	public RawImage testImage;
    private CameraManager mobileCamera;
    public GameObject arCamera;
    public Text trajectory;
    public Text transX;
    public Text transY;
    public Text transZ;
    public int[,] rotationMat = new int [3,3];

    public string traj;
    public string[] split_traj;
    public float qw, qx, qy, qz, tx, ty, tz;
    public Boolean connecting = false;

    private void Awake() 
    {
        // if (null == instance)
        // {
        //     instance = this;
        //     DontDestroyOnLoad(this.gameObject);
        // } 
        // else
        // {
        //     Destroy(this.gameObject);
        // }
        Instance = this;
    }

    void Start()
    {
        qw = 0;
        qx = 0;
        qy = 0;
        qz = 0;
        tx = 0;
        ty = 0;
        tz = 0;
    }

    private void ConnectToTcpServer()
    {
        try
        {
            socketConnection = new TcpClient("166.104.246.62", 17000);
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
            ConnectToTcpServer();
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
            Debug.Log("Receive Start!");
            // Get a stream object for writing.             
            NetworkStream stream = socketConnection.GetStream();
            // Debug.Log("Receiving " + stream);
            byte[] recvMessage = new Byte[1024];

            stream.Read(recvMessage, 0, recvMessage.Length);
            // Debug.Log("Receiving 1");

            trajectory.text = System.Text.Encoding.UTF8.GetString(recvMessage);
            // Debug.Log("Receiving 2");
            traj = trajectory.text.ToString();
            // Debug.Log("Receiving 3 " + traj);
            split_traj = traj.Split(' ');
            // Debug.Log("Receiving 4 - tx: " + split_traj[4]);

            qw = float.Parse(split_traj[0]);
            qx = float.Parse(split_traj[1]);
            qy = float.Parse(split_traj[2]);
            qz = float.Parse(split_traj[3]);
            tx = float.Parse(split_traj[4]);
            ty = float.Parse(split_traj[5]);
            tz = float.Parse(split_traj[6]);

            transX.text = tx.ToString();
            transY.text = ty.ToString();
            transZ.text = tz.ToString();
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }

    public void SendToServerImage()
	{
        Debug.Log("Task Start!");
        mobileCamera = GameObject.Find("Camera Manager").GetComponent<CameraManager>();
        mobileCamera.ARCamera();
        CommunitcateWithServer(mobileCamera.m_LastCameraTexture.EncodeToPNG());
	}

    async void CommunitcateWithServer(byte[] byteTestImageTexture)
    {
        await Task.Run(() =>
        {
            ConnectToTcpServer();

            string requestMessage = "kyj0701 CameraImage.png " + byteTestImageTexture.Length;
            Debug.Log(requestMessage);

            SendMessage(System.Text.Encoding.Default.GetBytes(requestMessage));
            SendMessage(byteTestImageTexture);
            SendMessage(System.Text.Encoding.Default.GetBytes("EOF"));

            RecvMessage();
            if (tx != 0) 
            {
                connecting = true;
                arCamera.transform.Translate(new Vector3(tx, ty, tz));
            }

            socketConnection = null;
        });
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    // public static Client Instance
    // {
    //     get
    //     {
    //         if (null == instance)
    //         {
    //             return null;
    //         }
    //         return instance;
    //     }
    // }
}
