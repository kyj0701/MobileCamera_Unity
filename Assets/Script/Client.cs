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
	public RawImage testImage;
    private CameraManager mobileCamera;
    public GameObject arCamera;
    public Text trajectory;
    public Text transX;
    public Text transY;
    public Text transZ;

    public string traj;
    public string[] split_traj;
    public float qw, qx, qy, qz, tx, ty, tz;
    public Vector3 rotatePos;
    public float[][] rotationMat;
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

            rotationMat = QuaternionToMatrix3x3();
            rotatePos = rotaionMatrixToEulerAngles(rotationMat);

            transX.text = rotatePos.x.ToString();
            transY.text = rotatePos.y.ToString();
            transZ.text = rotatePos.z.ToString();
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

    private float[][] QuaternionToMatrix3x3()
    {
        float[][] matrix3x3 = {
            new float[3],
            new float[3],
            new float[3],
        };

        matrix3x3[0][0] = 2 * (qw * qw + qx * qx) -1;
        matrix3x3[0][1] = 2 * (qx * qy - qw * qz);
        matrix3x3[0][2] = 2 * (qx * qz + qw * qy);

        matrix3x3[1][0] = 2 * (qx * qy + qw * qz);
        matrix3x3[1][1] = 2 * (qw * qw + qy * qy - 1);
        matrix3x3[1][2] = 2 * (qy * qz - qw * qx);

        matrix3x3[2][0] = 2 * (qx * qz - qw * qy);
        matrix3x3[2][1] = 2 * (qy * qz + qw * qx);
        matrix3x3[2][2] = 2 * (qw * qw + qz * qz) - 1;

        return matrix3x3;
    }

    private Vector3 rotaionMatrixToEulerAngles(float[][] R)
    {
        float sy = MathF.Sqrt(R[0][0] * R[0][0] + R[1][0] * R[1][0]);

        bool singular = sy < 1e-6;

        float x,y,z;
        if (!singular)
        {
            x = MathF.Atan2(R[2][1], R[2][2]);
            y = MathF.Atan2(R[2][0], sy);
            z = MathF.Atan2(R[1][0], R[0][0]);
        }
        else
        {
            x = MathF.Atan2(R[1][2], R[1][1]);
            y = MathF.Atan2(R[2][0], sy);
            z = 0;
        }

        Vector3 newRotation = new Vector3(x,y,z);

        return newRotation;
    }

    // private bool IsRotationMatrix4x4()
    // {
    //     float[][] Rt = 
    //     {
    //         new float[3],
    //         new float[3],
    //         new float[3],

    //     };

    //     float[][] shouldBeIdentity =
    //     {
    //         new float[3],
    //         new float[3],
    //         new float[3],
    //     };

    //     float[][] I =
    //     {
    //         new float[3],
    //         new float[3],
    //         new float[3],
    //     };

    //     for (int i = 0; i< 3; i++)
    //     {
    //         for (int j = 0; j < 3; j++)
    //         {
    //             if (i == j) I[i][j] = 1.0f;
    //             else I[i][j] = 0.0f;
    //         }
    //     }

    //     for (int i = 0; i < 3; i++)
    //     {
    //         for (int j = 0; j < 3; j++)
    //         {
    //             Rt[i][j] = rotationMat[j][i];
    //         }
    //     }

    //     for (int k = 0; k < 3; k++)
    //     {
    //         for (int i = 0; i < 3; i++)
    //         {
    //             float sum = 0;
    //             for (int j = 0; j < 3; j++)
    //             {
    //                 sum += Rt[i][j] * rotationMat[j][k];
    //             }
    //             shouldBeIdentity[i][k] = sum;
    //         }
    //     }

    //     return true;
    // }
}
