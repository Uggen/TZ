//Content is available under Creative Commons Attribution Share Alike(CC BY-SA 3.0).
//ObjExporter Source Code : http://wiki.unity3d.com/index.php/ObjExporter

//============================================
//
//Modified By Bathur Lu
//
//Date:     2019.3.3
//Website:  http://bathur.cn/
//
//============================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

struct ObjMaterial
{
    public string name;
    public string textureName;
}

namespace MyNameSpace
{
    public class ObjExporter : MonoBehaviour
    {
        public List<MeshFilter> meshFilters;
        public static string targetFolder = "ExportedObj";

        private static int vertexOffset = 0;
        private static int normalOffset = 0;
        private static int uvOffset = 0;

        private static string MeshToString(MeshFilter mf, Dictionary<string, ObjMaterial> materialList, string textureFolderPath)
        {
            Mesh m = mf.sharedMesh;
            Material[] mats = mf.GetComponent<Renderer>().sharedMaterials;

            StringBuilder sb = new StringBuilder();

            sb.Append("g ").Append(mf.name).Append("\n");
            foreach (Vector3 lv in m.vertices)
            {
                Vector3 wv = mf.transform.TransformPoint(lv);
                sb.Append(string.Format("v {0} {1} {2}\n", -wv.x, wv.y, wv.z));
            }
            sb.Append("\n");

            foreach (Vector3 lv in m.normals)
            {
                Vector3 wv = mf.transform.TransformDirection(lv);

                sb.Append(string.Format("vn {0} {1} {2}\n", -wv.x, wv.y, wv.z));
            }
            sb.Append("\n");

            foreach (Vector3 v in m.uv)
            {
                sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
            }

            for (int material = 0; material < m.subMeshCount; material++)
            {
                sb.Append("\n");
                sb.Append("usemtl ").Append(mats[material].name).Append("\n");
                sb.Append("usemap ").Append(mats[material].name).Append("\n");

                try
                {
                    ObjMaterial objMaterial = new ObjMaterial
                    {
                        name = mats[material].name
                    };

                    if (mats[material].mainTexture)
                    {
                        objMaterial.textureName = Path.Combine(textureFolderPath, mats[material].mainTexture.name);
                    }
                    else
                    {
                        objMaterial.textureName = null;
                    }

                    materialList.Add(objMaterial.name, objMaterial);
                }
                catch (ArgumentException)
                {
                }

                int[] triangles = m.GetTriangles(material);
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    sb.Append(string.Format("f {1}/{1}/{1} {0}/{0}/{0} {2}/{2}/{2}\n",
                        triangles[i] + 1 + vertexOffset, triangles[i + 1] + 1 + normalOffset, triangles[i + 2] + 1 + uvOffset));
                }
            }

            vertexOffset += m.vertices.Length;
            normalOffset += m.normals.Length;
            uvOffset += m.uv.Length;

            return sb.ToString();
        }


        private static void Clear()
        {
            vertexOffset = 0;
            normalOffset = 0;
            uvOffset = 0;
        }

        private static Dictionary<string, ObjMaterial> PrepareFileWrite()
        {
            Clear();
            return new Dictionary<string, ObjMaterial>();
        }

        private static void MaterialsToFile(Dictionary<string, ObjMaterial> materialList, string folder, string filename)
        {
            using (StreamWriter sw = new StreamWriter(folder + Path.DirectorySeparatorChar + filename + ".mtl"))
            {
                foreach (KeyValuePair<string, ObjMaterial> kvp in materialList)
                {
                    sw.Write("\n");
                    sw.Write("newmtl {0}\n", kvp.Key);
                    sw.Write("Ka  0.6 0.6 0.6\n");
                    sw.Write("Kd  0.6 0.6 0.6\n");
                    sw.Write("Ks  0.9 0.9 0.9\n");
                    sw.Write("d  1.0\n");
                    sw.Write("Ns  0.0\n");
                    sw.Write("illum 2\n");

                    if (kvp.Value.textureName != null)
                    {
                        string destinationFile = kvp.Value.textureName;


                        int stripIndex = destinationFile.LastIndexOf(Path.DirectorySeparatorChar);

                        if (stripIndex >= 0)
                            destinationFile = destinationFile.Substring(stripIndex + 1).Trim();


                        string relativeFile = destinationFile;

                        destinationFile = folder + Path.DirectorySeparatorChar + destinationFile;

                        Debug.Log("Copying texture from " + kvp.Value.textureName + " to " + destinationFile);

                        try
                        {
                            File.Copy(kvp.Value.textureName, destinationFile);
                        }
                        catch
                        {

                        }


                        sw.Write("map_Kd {0}", relativeFile);
                    }

                    sw.Write("\n\n\n");
                }
            }
        }

        private static void MeshToFile(MeshFilter mf, string folder, string filename, string textureFolderPath)
        {
            Dictionary<string, ObjMaterial> materialList = PrepareFileWrite();

            using (StreamWriter sw = new StreamWriter(folder + Path.DirectorySeparatorChar + filename + ".obj"))
            {
                sw.Write("mtllib ./" + filename + ".mtl\n");

                sw.Write(MeshToString(mf, materialList, textureFolderPath));
            }

            MaterialsToFile(materialList, folder, filename);
        }
        private static bool CreateTargetFolder()
        {
            try
            {
                Directory.CreateDirectory(targetFolder);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static void ExportSelectedObj(string materialName, MeshFilter[] meshFilters, string exportFileName, string textureFolderPath)
        { 
            string folderPath = Path.GetDirectoryName(Application.dataPath); // Или Application.persistentDataPath
            string fullPath = Path.Combine(folderPath, exportFileName + ".obj");
            if (!CreateTargetFolder())
                return;

            MeshFilter[] allMeshFilters = FindObjectsOfType<MeshFilter>();
            List<MeshFilter> filteredMeshFilters = new List<MeshFilter>();

            foreach (MeshFilter mf in allMeshFilters)
            {
                Renderer renderer = mf.GetComponent<Renderer>();
                if (renderer != null)
                {
                    foreach (Material mat in renderer.sharedMaterials)
                    {
                        if (mat != null && mat.name.Equals(materialName, StringComparison.OrdinalIgnoreCase))
                        {
                            filteredMeshFilters.Add(mf);
                                break; 
                        }
                    }
                }
            }

            if (filteredMeshFilters.Count > 0)
            {
                ExportMeshesToFile(filteredMeshFilters.ToArray(), targetFolder, exportFileName, textureFolderPath);
            }
        }
        private static void ExportMeshesToFile(MeshFilter[] meshFilters, string folder, string filename, string textureFolderPath)
        {   
            Dictionary<string, ObjMaterial> materialList = PrepareFileWrite();

            using (StreamWriter sw = new StreamWriter(folder + Path.DirectorySeparatorChar + filename + ".obj"))
            {
                sw.Write("mtllib ./" + filename + ".mtl\n");

                for (int i = 0; i < meshFilters.Length; i++)
                {
                    sw.Write(MeshToString(meshFilters[i], materialList, textureFolderPath));
                }
            }

            MaterialsToFile(materialList, folder, filename);
        }
    }
}
