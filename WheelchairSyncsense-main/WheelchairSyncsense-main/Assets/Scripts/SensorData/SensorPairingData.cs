using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Game.Util;

namespace Game.Sensor
{

    [Serializable]
    public class SensorPairingData
    {
        public Exercise exercise;
        public string leftSensorAddress;
        public RotationDirection leftSensorDirection;
        public float leftSensorGravity;
        public string rightSensorAddress;
        public RotationDirection rightSensorDirection;
        public float rightSensorGravity;


        public SensorPairingData()
        {
        }

        public SensorPairingData(Exercise e)
        {
            exercise = e;
        }


        #region SetData

        public void SetSensorAddress(SensorPosition position, string id)
        {
            switch (position)
            {
                case SensorPosition.LEFT:
                    leftSensorAddress = id;
                    break;
                case SensorPosition.RIGHT:
                    rightSensorAddress = id;
                    break;
                default: break;
            }
        }

        public void SetSensorDirection(SensorPosition position, RotationDirection direction)
        {
            switch (position)
            {
                case SensorPosition.LEFT:
                    leftSensorDirection = direction;
                    break;
                case SensorPosition.RIGHT:
                    rightSensorDirection = direction;
                    break;
                default: break;
            }
        }
        
        public void SetSensorGravity(SensorPosition position, float gravity)
        {
            switch (position)
            {
                case SensorPosition.LEFT:
                    leftSensorGravity = gravity;
                    break;
                case SensorPosition.RIGHT:
                    rightSensorGravity = gravity;
                    break;
                default: break;
            }
        }
        

        #endregion

    }

}