using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;

namespace EppLib.Tests
{
    class DeploymentUtility
    {
        public static void CopyDeploymentItems(object[] attributes)
        {
            foreach (DeploymentItemAttribute att in attributes)
            {
                var origin = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), att.Path);
                var targetDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), att.OutputDirectory);
                Directory.CreateDirectory(targetDir);
                try
                {
                    File.Copy(origin, Path.Combine(targetDir, Path.GetFileName(origin)));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error copying deployment item {0} to {1} message {2}", origin, targetDir, ex.Message);
                }
            }
        }
    }
}
