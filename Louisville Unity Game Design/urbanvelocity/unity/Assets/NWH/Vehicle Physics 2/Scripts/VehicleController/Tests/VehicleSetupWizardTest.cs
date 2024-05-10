﻿using NWH.VehiclePhysics2.SetupWizard;
using UnityEngine;

namespace NWH.VehiclePhysics2.Tests
{
    /// <summary>
    ///     Runs VehicleSetupWizard on Start.
    /// </summary>
    public partial class VehicleSetupWizardTest : MonoBehaviour
    {
        private void Start()
        {
            VehicleSetupWizard vsw = GetComponent<VehicleSetupWizard>();
            if (vsw != null)
            {
                VehicleSetupWizard.RunSetup(vsw.gameObject, vsw.wheelGameObjects);
                Destroy(this);
                Destroy(vsw);
            }
            else
            {
                Debug.LogWarning("VehicleSetupWizard does not exist.");
            }
        }
    }
}