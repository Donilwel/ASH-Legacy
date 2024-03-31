using UnityEngine;

public class Car : MonoBehaviour
{
    public WheelCollider[] wheelColliders = new WheelCollider[4];
    public Transform[] tireMeshes = new Transform[4];
    public float maxTorque = 1500f;
    public float maxSteerAngle = 30f;
    public float brakeForce = 6000f;
    public Rigidbody carRigidbody;
    public float downforce = 100f;
    public float driftFactorSteer = 0.5f; // Фактор скольжения для управления

    private void Start()
    {
        carRigidbody.centerOfMass -= new Vector3(0, 0.5f, 0);
    }

    void FixedUpdate()
    {
        float torque = maxTorque * Input.GetAxis("Vertical");
        float steerAngle = maxSteerAngle * Input.GetAxis("Horizontal");
        bool isBraking = Input.GetKey(KeyCode.Space);

        // При торможении и повороте одновременно увеличиваем угол заноса
        if (isBraking && Input.GetAxis("Horizontal") != 0)
        {
            ApplyDrifting(steerAngle, true);
        }
        else
        {
            ApplySteering(steerAngle);
            ApplyThrottle(torque, isBraking);
        }

        UpdateWheelPoses();
        ApplyDownforce();
    }

    void ApplyDrifting(float steerAngle, bool isBraking)
    {
        float driftSteerAngle = steerAngle * driftFactorSteer;
        foreach (var wheelCollider in wheelColliders)
        {
            if (wheelCollider.transform.localPosition.z > 0)
            {
                wheelCollider.steerAngle = driftSteerAngle;
            }
            if (isBraking)
            {
                wheelCollider.brakeTorque = brakeForce;
            }
            else
            {
                wheelCollider.motorTorque = maxTorque;
            }
        }
    }

    void ApplySteering(float steerAngle)
    {
        foreach (var wheelCollider in wheelColliders)
        {
            if (wheelCollider.transform.localPosition.z > 0)
            {
                wheelCollider.steerAngle = steerAngle;
            }
        }
    }

    void ApplyThrottle(float torque, bool isBraking)
    {
        foreach (var wheelCollider in wheelColliders)
        {
            wheelCollider.motorTorque = isBraking ? 0 : torque;
            wheelCollider.brakeTorque = isBraking ? brakeForce : 0;
        }
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

    void ApplyDownforce()
    {
        carRigidbody.AddForce(-transform.up * downforce * carRigidbody.velocity.magnitude);
    }
}
