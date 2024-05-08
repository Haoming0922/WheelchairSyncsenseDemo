using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Game.Util;

namespace Game.Sensor
{
    public class SensorInput
    {
        public float value;
        private float baseValue = 0;
        
        private string sensorAddress = "";
        private float currentRotationRaw = 0;
        private RotationDirection direction = RotationDirection.NULL;

        private Queue<float> dataWindow = new Queue<float>();
        private int lowPassWindowSize = 6;
        public float averageValue { get; private set; }
        public bool IsMove { get; private set; }

        private float angleStart = 1f;
        private float angleEnd = 1f;
        
        public SensorInput(SensorPosition position, SensorPairingData pairingData)
        {
            switch (position)
            {
                case SensorPosition.LEFT:
                    direction = pairingData.leftSensorDirection;
                    sensorAddress = pairingData.leftSensorAddress;
                    
                    break;
                case SensorPosition.RIGHT:
                    direction = pairingData.rightSensorDirection;
                    sensorAddress = pairingData.rightSensorAddress;
                    break;
            }
            
        }



        public void WheelchairControlEvent(SensorDataReceived sensorData)
        {
            if (sensorAddress == sensorData.deviceAddress)
            {
                WheelchairLowPassFiltRotation(sensorData);
                WheelchairRotationToGameInput();
            }
        }
        
        
        private void WheelchairLowPassFiltRotation(SensorDataReceived sensorData)
        {
            float data = 0;
            
            switch (direction)
            {
                case RotationDirection.XPOSITIVE:
                case RotationDirection.XNEGATIVE:
                    data = sensorData.gyroX;
                    break;
                case RotationDirection.YPOSITIVE:
                case RotationDirection.YNEGATIVE:
                    data = sensorData.gyroY;
                    break;
                case RotationDirection.ZPOSITIVE:
                case RotationDirection.ZNEGATIVE:
                    data = sensorData.gyroZ;
                    break;
                default: break;
            }
            
            if (dataWindow.Count < lowPassWindowSize)
            {
                dataWindow.Enqueue(data);
            }
            else
            {
                dataWindow.Dequeue();
                dataWindow.Enqueue(data);
            }

            averageValue = Calculation.AverageQueue(dataWindow);
            
            IsMove = Calculation.IsMove(sensorData);
        }


        private void WheelchairRotationToGameInput()
        {
            switch (direction)
            {
                case RotationDirection.XPOSITIVE:
                case RotationDirection.ZPOSITIVE:
                case RotationDirection.YPOSITIVE:
                    if (Mathf.Abs(averageValue) > baseValue)
                    {
                        value = Calculation.ToWheelchairRacingInput(Mathf.Abs(averageValue), baseValue);
                        value = Mathf.Sign(averageValue) * value;
                    }
                    else
                    {
                        value = 0;
                    }
                    break;
                case RotationDirection.XNEGATIVE:
                case RotationDirection.YNEGATIVE:
                case RotationDirection.ZNEGATIVE:
                    if (Mathf.Abs(averageValue) > baseValue)
                    {
                        value = Calculation.ToWheelchairRacingInput(Mathf.Abs(averageValue), baseValue);
                        value = -Mathf.Sign(averageValue) * value;
                    }
                    else
                    {
                        value = 0;
                    }
                    break;
                default: break;
            }
        }
        
        
    }

}