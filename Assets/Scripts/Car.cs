using UnityEngine;

public class Car : MonoBehaviour
{
    public UnityEngine.UI.Slider fuelSlider;
    public WheelCollider[] wheelColliders = new WheelCollider[4];
    public Transform[] tireMeshes = new Transform[4];
    public float maxTorque = 1500f;
    public float maxSteerAngle = 30f;
    public float brakeForce = 6000f;
    public Rigidbody carRigidbody;
    public float downforce = 100f;
    public float driftFactorSteer = 0.5f;
    private AudioSource audioSource; // Для воспроизведения звуков автомобиля

    public float fuel = 100f; // Уровень топлива
    public float fuelConsumptionRate = 0.1f; // Скорость расхода топлива
    private bool isEngineOn = true; // Состояние двигателя

    private void Start()
    {
        carRigidbody.centerOfMass -= new Vector3(0, 0.5f, 0);
    }

    private void FixedUpdate()
    {

        if (fuelSlider != null)
        {
            fuelSlider.value = fuel / 100f; // предполагается, что fuelSlider.maxValue = 100
        }

        float throttleInput = Input.GetAxis("Vertical");
        bool isCarMovingForward = throttleInput > 0;
        bool isBraking = Input.GetKey(KeyCode.Space);

        if (isEngineOn && isCarMovingForward)
        {
            // Расход топлива
            fuel -= fuelConsumptionRate * throttleInput * Time.fixedDeltaTime; // Топливо тратится только при движении вперед
            if (fuel < 0)
            {
                fuel = 0;
            }

            if (fuel == 0)
            {
                isEngineOn = false;
                StopCar();
                SoundManager.Instance.engineSoundSource.Stop();
                if (isCarMovingForward)
                {
                    SoundManager.Instance.PlayEngineStallSound();
                }

            }
        }
        if (fuel == 0)
        {
            SoundManager.Instance.engineSoundSource.Stop();
            if (isCarMovingForward)
            {
                SoundManager.Instance.PlayEngineStallSound();
            }
        }

        if (isEngineOn)
        {
            // Логика управления автомобилем
            float torque = maxTorque * throttleInput;
            float steerAngle = maxSteerAngle * Input.GetAxis("Horizontal");

            ApplySteering(steerAngle);
            ApplyThrottle(torque, isBraking);
            ApplyBrakes(isBraking);

            UpdateWheelPoses();
            ApplyDownforce();
        }
    }

    internal void Refuel(float amount)
    {
        fuel += amount;
        if (fuel > 100f) // Предполагаем, что 100 - это максимальный объём топлива
        {
            fuel = 100f;
        }

        if (!isEngineOn && fuel > 0)
        {
            isEngineOn = true;
        }
    }


    private void StopCar()
    {
        foreach (var wheel in wheelColliders)
        {
            wheel.motorTorque = 0;
            wheel.brakeTorque = brakeForce;
        }
    }

    private void ApplySteering(float steerAngle)
    {
        for (int i = 0; i < 2; i++)
        {
            wheelColliders[i].steerAngle = steerAngle;
        }
    }

    private void ApplyThrottle(float torque, bool isBraking)
    {
        foreach (var wheel in wheelColliders)
        {
            wheel.motorTorque = isBraking ? 0 : torque;
        }
    }

    private void ApplyBrakes(bool isBraking)
    {
        foreach (var wheel in wheelColliders)
        {
            wheel.brakeTorque = isBraking ? brakeForce : 0;
        }
    }

    private void UpdateWheelPoses()
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

    private void ApplyDownforce()
    {
        carRigidbody.AddForce(-transform.up * downforce * carRigidbody.velocity.magnitude);
    }
}
