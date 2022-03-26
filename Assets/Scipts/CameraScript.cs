using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private GameObject cameraCentre;
    [SerializeField] private float cameraMovementSpeedConstant;

    public NewControls newControls;

    bool held = false;
    Vector2 rotateVector;

    // Start is called before the first frame update
    void Awake()
    {
        newControls = new NewControls();

        newControls.Camera.Rotate.started += context =>
        {
            held = true;
            rotateVector = context.ReadValue<Vector2>();
        };

        newControls.Camera.Rotate.performed += context =>
        {
            held = true;
            rotateVector = context.ReadValue<Vector2>();
        };

        newControls.Camera.Rotate.canceled += _ =>
        {
            held = false;
        };
    }

    void OnEnable()
    {
        newControls.Camera.Enable();
    }

    void OnDisable()
    {
        newControls.Camera.Disable();
    }

    private void Update()
    {
        if (held && rotateVector != null)
        {
            float distance = Vector3.Distance(cameraCentre.transform.position, transform.position);
            float xAngle = this.transform.eulerAngles.x;
            float cameraMovementSpeed = distance * cameraMovementSpeedConstant * (1 / xAngle);

            if (xAngle >= 75 && rotateVector.y > 0)
                rotateVector.y = 0;
            if (xAngle <= 30 && rotateVector.y < 0)
                rotateVector.y = 0;

            transform.Translate(new Vector3(rotateVector.x, rotateVector.y, 0) * Time.deltaTime * cameraMovementSpeed);
            //transform.Translate(Vector3.left * Time.deltaTime * cameraMovementSpeed);
            transform.LookAt(cameraCentre.transform);
        }
    }

    /*
     * // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(cameraCentre.transform.position, transform.position);
        float xAngle = this.transform.eulerAngles.x;
        float cameraMovementSpeed = distance * cameraMovementSpeedConstant * (1/xAngle);

        if (Input.GetKey(KeyCode.A)) 
            transform.Translate(Vector3.left * Time.deltaTime * cameraMovementSpeed);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * Time.deltaTime * cameraMovementSpeed);
        if (Input.GetKey(KeyCode.W) && xAngle <= 75)
            transform.Translate(Vector3.up * Time.deltaTime * cameraMovementSpeed);
        if (Input.GetKey(KeyCode.S) && xAngle >= 30)
            transform.Translate(Vector3.down * Time.deltaTime * cameraMovementSpeed);
        if (Input.GetKey(KeyCode.E) && distance >= 7)
            transform.Translate(Vector3.forward * Time.deltaTime * cameraMovementSpeed);
        if (Input.GetKey(KeyCode.Q) && distance <= 25)
            transform.Translate(Vector3.back * Time.deltaTime * cameraMovementSpeed);
        transform.LookAt(cameraCentre.transform);
    }*/
}
