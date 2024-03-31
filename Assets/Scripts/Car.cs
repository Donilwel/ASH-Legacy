using UnityEngine;

public class Car : MonoBehaviour
{
    public WheelCollider[] wheelColliders = new WheelCollider[4];
    public Transform[] tireMeshes = new Transform[4];
    public float maxTorque = 1500f;
    public float maxSteerAngle = 30f;


    void FixedUpdate()
    {
        float torque = maxTorque  * Input.GetAxis("Vertical");
        float steerAngle = maxSteerAngle * Input.GetAxis("Horizontal");

        wheelColliders[0].steerAngle = steerAngle;
        wheelColliders[1].steerAngle = steerAngle;

        foreach (var wheelCollider in wheelColliders)
        {
            wheelCollider.motorTorque = torque;
        }

        UpdateWheelPoses();
    }

    void UpdateWheelPoses()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            Vector3 pos;
            Quaternion quat;
            wheelColliders[i].GetWorldPose(out pos, out quat);

            tireMeshes[i].position = pos;
            tireMeshes[i].rotation = quat;
        }
    }
}
