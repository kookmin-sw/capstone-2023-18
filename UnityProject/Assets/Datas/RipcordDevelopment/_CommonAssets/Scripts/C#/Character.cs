using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// /-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\
//
// 						Ripcord Tools, Copyright © 2022, Ripcord Development
//										      Character.cs
//												 v1.0.0
//										   info@ripcorddev.com
//
// \-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/

//ABOUT - This script move and rotates the camera player object and its camera using keyboard and mouse controls.

namespace Ripcord.Common
{
    [RequireComponent(typeof(CharacterController))]
    public class Character : MonoBehaviour
    {
        CharacterController character;

        public float moveSpeed = 5.0f;

        float mouseX;
        float mouseY;

        float horizontal;
        float vertical;

        public float sensitivityX = 100.0f;
        public float sensitivityY = 150.0f;

        public float minAngle;
        public float maxAngle;

        float xRotation;
        float yRotation;

        public bool lockCursor = true;

        Transform playerCamera;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // START
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        void Start()
        {
            character = GetComponent<CharacterController>();

            playerCamera = transform.GetComponentInChildren<Camera>().transform;
            if (!playerCamera)
            {
                Debug.LogError("<color=#ff4444><b>Character.cs</b></color> - Player Camera not found.  Please make sure the Player object includes a camera");
            }

            if (lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        Vector3 move;
        Vector3 rotate;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // UPDATE
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        void Update()
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            mouseX = Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime;
            mouseY = -Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;

            xRotation += mouseY;
            xRotation = Mathf.Clamp(xRotation, minAngle, maxAngle);

            yRotation = mouseX;

            move = new Vector3(horizontal, 0.0f, vertical) * moveSpeed * Time.deltaTime;
            transform.Translate(move, Space.Self);

            playerCamera.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
            transform.Rotate(Vector3.up * yRotation);
        }
    }
}
