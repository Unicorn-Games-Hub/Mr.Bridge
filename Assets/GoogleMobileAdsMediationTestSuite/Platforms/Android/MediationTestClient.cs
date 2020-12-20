// <copyright company="Google" file="MediationTestClient.cs"> Copyright (C) 2017 Google, Inc. </copyright>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#if UNITY_ANDROID

namespace GoogleMobileAdsMediationTestSuite.Android
{
    using System;
    using GoogleMobileAdsMediationTestSuite.Common;
    using UnityEngine;
    using GoogleMobileAds.Api;
    using GoogleMobileAds.Android;

    /// <summary>
    /// Android implementation of Mediation test client interface <see cref="IMediationTestClient"/> .
    /// </summary>
    public class MediationTestClient : AndroidJavaProxy, IMediationTestClient
    {
        private static string UnityActivityClassName = "com.unity3d.player.UnityPlayer";
        private static string MediationTestSuiteClassName = "com.google.android.ads.mediationtestsuite.MediationTestSuite";
        private static string UnityMediationTestSuiteListenerName = "com.google.unity.ads.mediationtestsuite.UnityMediationTestSuiteListener";
        private static string UnityMediationTestSuiteEventForwarderName = "com.google.unity.ads.mediationtestsuite.UnityMediationTestSuiteEventForwarder";
        private static MediationTestClient instance = new MediationTestClient();

        private AndroidJavaObject listener;

        private MediationTestClient() : base(UnityMediationTestSuiteListenerName)
        {
          this.listener = new AndroidJavaObject(UnityMediationTestSuiteEventForwarderName, this);
        }

        public void Dispose()
        {
            AndroidJavaClass mediationTestSuiteClass = new AndroidJavaClass(MediationTestSuiteClassName);
            mediationTestSuiteClass.CallStatic("setListener", null);
        }

        ~MediationTestClient()
        {
            this.Dispose();
        }

        public event EventHandler<EventArgs> OnMediationTestSuiteDismissed;

        public AdRequest AdRequest {
            set
            {
                this.SetAdRequestImpl(value);
            }
        }

        public static MediationTestClient Instance
        {
            get
            {
                return instance;
            }
        }

        public void Show(string appId)
        {
            AndroidJavaClass playerClass = new AndroidJavaClass(UnityActivityClassName);
            AndroidJavaObject activity =
                    playerClass.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass mediationTestSuiteClass = new AndroidJavaClass(MediationTestSuiteClassName);
            mediationTestSuiteClass.CallStatic("launch", activity, appId);
            mediationTestSuiteClass.CallStatic("setListener", this.listener);
            mediationTestSuiteClass.CallStatic("setUserAgentSuffix", "unity");
        }

        private void onMediationTestSuiteDismissed()
        {
            if (this.OnMediationTestSuiteDismissed != null)
            {
                this.OnMediationTestSuiteDismissed(this, EventArgs.Empty);
            }
        }

        private void SetAdRequestImpl(AdRequest adRequest) {
            AndroidJavaObject androidRequest = Utils.GetAdRequestJavaObject(adRequest);
            AndroidJavaClass mediationTestSuiteClass = new AndroidJavaClass(MediationTestSuiteClassName);
            mediationTestSuiteClass.CallStatic("setAdRequest", androidRequest);

            foreach (string deviceHash in adRequest.TestDevices) {
                this.AddTestDevice(deviceHash);
            }
        }

        private void AddTestDevice(string deviceHash) {
            AndroidJavaClass mediationTestSuiteClass = new AndroidJavaClass(MediationTestSuiteClassName);
            mediationTestSuiteClass.CallStatic("addTestDevice", deviceHash);
        }


    }
}

#endif
