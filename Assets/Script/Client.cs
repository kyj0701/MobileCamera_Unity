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
    public float qw, qx, qy, qz;
    private float tx, ty, tz;
    private Vector3 rotatePos;
    public Boolean connecting = false;

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
        // if state = 1, client can't receive traj from server 
        if (state == 0 || state == 2) SendToServerImage();
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
            Debug.Log("Receiving " + stream);
            byte[] recvMessage = new Byte[4096];

            stream.Read(recvMessage, 0, recvMessage.Length);
            Debug.Log("Receiving 1 " + recvMessage);

            traj = System.Text.Encoding.UTF8.GetString(recvMessage).ToString();
            if (traj .Equals("error"))
            {
                state = 2;
                Debug.Log("UnicodeDecodeError");
            }
            else
            {
                Debug.Log("Receiving 2 " + traj);
                split_traj = traj.Split(' ');
                Debug.Log("Receiving 3 - tx: " + split_traj[4]);

                qw = float.Parse(split_traj[0]);
                qx = float.Parse(split_traj[1]);
                qy = float.Parse(split_traj[2]);
                qz = float.Parse(split_traj[3]);
                tx = float.Parse(split_traj[4]);
                ty = float.Parse(split_traj[5]);
                tz = float.Parse(split_traj[6]);

                rotatePos = EulerFromQuarternion(qw, qx, qy, qz);

                transX.text = tx.ToString();
                transY.text = ty.ToString();
                transZ.text = tz.ToString();

                rotateX.text = rotatePos.x.ToString();
                rotateY.text = rotatePos.y.ToString();
                rotateZ.text = rotatePos.z.ToString();
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
	}

    async void CommunitcateWithServer(byte[] byteTestImageTexture)
    {
        await Task.Run(() =>
        {
            ConnectToTcpServer();

            string requestMessage = "kyj0701 CameraImage.png " + byteTestImageTexture.Length;
            Debug.Log(requestMessage);

            SendMessage(System.Text.Encoding.UTF8.GetBytes(requestMessage));
            SendMessage(byteTestImageTexture);
            SendMessage(System.Text.Encoding.UTF8.GetBytes("EOF"));

            RecvMessage();

            if (tx != 0 || ty != 0 || tz != 0) 
            {
                connecting = true;
                arCamera.transform.position = new Vector3(tx, ty, tz);
            }

            socketConnection = null;
        });
    }

    private Vector3 EulerFromQuarternion(float qw, float qx, float qy, float qz)
    {
        float t0 = 2.0f * (qw * qx + qy * qz);
        float t1 = 1.0f - 2.0f * (qx * qx + qy * qy);
        float rx = MathF.Atan2(t0, t1);

        float t2 = 2.0f * (qw * qy - qz * qx);
        if (t2 > 1.0f) t2 = 1.0f;
        if (t2 < -1.0f) t2= -1.0f;
        float ry = MathF.Asin(t2);

        float t3 = 2.0f * (qw * qz + qx * qy);
        float t4 = 1.0f - 2.0f * (qy * qy + qz * qz);
        float rz = MathF.Atan2(t3, t4);

        return new Vector3(rx, ry, rz);
    }
}
