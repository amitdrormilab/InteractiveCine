﻿using System.Collections; 
using UnityEngine; 
using Assets.LSL4Unity.Scripts.AbstractInlets;
using System.IO;
using UnityEngine.SceneManagement;

namespace Assets.LSL4Unity.Scripts.Examples {

    /// <summary>
    /// Example that works with the Resolver component.
    /// This script waits for the resolver to resolve a Stream which matches the Name and Type.
    /// See the base class for more details. 
    /// 
    /// The specific implementation should only deal with the moment when the samples need to be pulled
    /// and how they should processed in your game logic
    ///
    /// </summary>
    public class DemoInletForFloatSamples : InletFloatSamples
    {
        public Transform targetTransform;

        public bool useX;
        public bool useY;
        public bool useZ;

        private bool pullSamplesContinuously = false;
        private System.Diagnostics.Process p = new System.Diagnostics.Process();



        void Start()
        {
            /*
            p.StartInfo.FileName = Directory.GetCurrentDirectory().ToString() + @"Assets\LSL4Unity\recieveData.py";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();

            Debug.Log(p.StandardOutput.ReadToEnd());
            */
        }

        protected override bool isTheExpected(LSLStreamInfoWrapper stream)
        {
            // the base implementation just checks for stream name and type
            var predicate = base.isTheExpected(stream);
            // add a more specific description for your stream here specifying hostname etc.
            //predicate &= stream.HostName.Equals("Expected Hostname");
            return predicate;
        }

        /// <summary>
        /// Override this method to implement whatever should happen with the samples...
        /// IMPORTANT: Avoid heavy processing logic within this method, update a state and use
        /// coroutines for more complexe processing tasks to distribute processing time over
        /// several frames
        /// </summary>
        /// <param name="newSample"></param>
        /// <param name="timeStamp"></param>
        protected override void Process(float[] newSample, double timeStamp)
        {
            //float x = useX ? newSample[0] : 1;
            float f_postPyCalc = useY ? newSample[1] : 1;
            //float z = useZ ? newSample[2] : 1;
            
            Debug.Log("SDNN = " + f_postPyCalc);

            if (f_postPyCalc > 299)
            {
                SceneManager.LoadScene("Intro", LoadSceneMode.Additive);
            }

            /*------ original code
            //Assuming that a sample contains at least 3 values for x,y,z
            float x = useX ? newSample[0] : 1;
            float y = useY ? newSample[1] : 1;
            float z = useZ ? newSample[2] : 1;

            // we map the data to the scale factors
            var targetScale = new Vector3(x, y, z);

            // apply the rotation to the target transform
            targetTransform.localScale = targetScale;
            */
        }

        protected override void OnStreamAvailable()
        {
            pullSamplesContinuously = true;
            

        }

        protected override void OnStreamLost()
        {
            pullSamplesContinuously = false;
        }
         
        private void Update()
        {
            if(pullSamplesContinuously)
                pullSamples();
        }

       
    }
}