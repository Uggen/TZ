using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using MyNameSpace;

public class Export : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            ExportModels();
        }
    }

    public void OnExportButtonPressed() 
    {
        ExportModels();
    }

    private static string GenerateUniqueFileName(string baseFileName)
    {
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        return baseFileName + "_" + timestamp;
    }

    void ExportModels()
    {
        List<MeshFilter> filteredMeshFilters = new List<MeshFilter>();

        StateController[] stateControllers = StateManager.Instance.GetAllStateControllers();

        foreach (StateController stateController in stateControllers)
        {
            if (stateController.isSelected)
            {
                MeshFilter mf = stateController.GetComponent<MeshFilter>();
                if (mf != null)
                {
                    filteredMeshFilters.Add(mf);
                }
            }
        }

        if (filteredMeshFilters.Count > 0)
        {
         string uniqueFileName = GenerateUniqueFileName("ExportedModel");
         MyNameSpace.ObjExporter.ExportSelectedObj("SelectedGlowMaterial", filteredMeshFilters.ToArray(), uniqueFileName, "ExportedObj");
         Debug.Log("Exported " + filteredMeshFilters.Count + " objects into " + uniqueFileName);
        }
    }


}
