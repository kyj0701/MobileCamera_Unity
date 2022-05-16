using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;


public class Client : MonoBehaviour
{
    // private static Client instance = null;
    public static Client Instance { get; set; }
    private TcpClient socketConnection;
    private CameraManager mobileCamera;
    public GameObject arCamera;
    public Text transX;
    public Text transY;
    public Text transZ;
    public Text rotateX;
    public Text rotateY;
    public Text rotateZ;

    private string traj;
    private string[] split_traj;
    private float qw, qx, qy, qz;
    private float tx, ty, tz;
    private Vector3 position;
    private Quaternion rotation;
    public Boolean connecting = false;
    private Vector3 t0;
    private Vector3 t1;

    private int state;

    private void Awake() 
    {
        Instance = this;
    }

    private void Start()
    {
        state = -1;
        qw = 0;
        qx = 0;
        qy = 0;
        qz = 0;
        tx = 0;
        ty = 0;
        tz = 0;
    }

    private void Update() 
    {
        // if state = 0, server sends traj to client
        // if state = 2, client can't receive traj from server 
        // if state = 3, SocketException: Connection reset by peer or Connection failed
        if (state == 0 || state == 2 || state == 3) SendToServerImage();
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
            state = 3;
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
            state = 3;
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
            byte[] recvMessage = new Byte[4096];

            stream.Read(recvMessage, 0, recvMessage.Length);
            // Debug.Log("Receiving 1 " + recvMessage);

            traj = System.Text.Encoding.UTF8.GetString(recvMessage).ToString();
            if (traj.Equals("error"))
            {
                state = 2;
                Debug.Log("UnicodeDecodeError");
            }
            else
            {
                // Debug.Log("Receiving 2 " + traj);
                split_traj = traj.Split(' ');
                // Debug.Log("Receiving 3 - tx: " + split_traj[3]);

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

                ConvertCoordinatesCOLMAPtoUnity(new Vector3(tx, ty, tz), new Quaternion(qw, qx, qy, qz));

                rotateX.text = rotation.x.ToString();
                rotateY.text = rotation.y.ToString();
                rotateZ.text = rotation.z.ToString();
                state = 0;
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }

    public void SendToServerImage()
	{
        state = 1;
        Debug.Log("Task Start!");
        mobileCamera = GameObject.Find("Camera Manager").GetComponent<CameraManager>();
        mobileCamera.ARCamera();
        CommunitcateWithServer(mobileCamera.m_LastCameraTexture.EncodeToPNG());
        t0 = new Vector3(arCamera.transform.position.x, arCamera.transform.position.y, arCamera.transform.position.z);

	}

    async void CommunitcateWithServer(byte[] byteTestImageTexture)
    {
        await Task.Run(() =>
        {
            ConnectToTcpServer();

            string requestMessage =  GameManager.Instance.playerID + " CameraImage.png " + byteTestImageTexture.Length;
            // string requestMessage =  "kyj0701" + " CameraImage.png " + byteTestImageTexture.Length;
            // Debug.Log(requestMessage);

            SendMessage(System.Text.Encoding.UTF8.GetBytes(requestMessage));
            SendMessage(byteTestImageTexture);
            SendMessage(System.Text.Encoding.UTF8.GetBytes("EOF"));

            RecvMessage();

            if (tx != 0 || ty != 0 || tz != 0) 
            {
                connecting = true;
                t1 = new Vector3(arCamera.transform.position.x, arCamera.transform.position.y, arCamera.transform.position.z);
                arCamera.transform.position = new Vector3(t1.x - t0.x + position.x, t1.y - t0.y + position.y, t1.z - t0.z + position.z);
                arCamera.transform.rotation = rotation;
            }

            socketConnection = null;
        });
    }

    private void ConvertCoordinatesCOLMAPtoUnity (Vector3 pos, Quaternion rot)
    {
        position = Quaternion.Inverse(rot) * -pos;
        position = Vector3.Scale(position, new Vector3(1,-1,1));
        rotation = Quaternion.Inverse(rot);
        rotation = new Quaternion(-rotation.x, rotation.y, -rotation.z, rotation.w);
    }
}
