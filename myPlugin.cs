//// (C) Copyright 2002-2009 by Autodesk, Inc. 
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted, 
// provided that the above copyright notice appears in all copies and 
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting 
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC. 
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to 
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//
using Autodesk.AutoCAD.ApplicationServices;
using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;

// This line is not mandatory, but improves loading performances
[assembly: ExtensionApplication(typeof(EJ_.MyPlugin))]

namespace EJ_
{
    /// This class is instantiated by AutoCAD once and kept alive for the 
    /// duration of the session. If you don't do any one time initialization 
    /// then you should remove this class.

    public class MyPlugin : IExtensionApplication
    {

        void IExtensionApplication.Initialize()
        {
            AcAp.Idle += OnIdle;
        }

        void IExtensionApplication.Terminate()
        {
            // Do plug-in application clean up here
        }
        private void OnIdle(object sender, EventArgs e)
        {
            var doc = AcAp.DocumentManager.MdiActiveDocument;
            if (doc != null)
            {
                Application.Idle -= OnIdle;
                try
                {
                    doc.Editor.WriteMessage("\nEJ_PageSetupImport loaded.\n");
                }
                catch (System.Exception ex)
                {
                    doc.Editor.WriteMessage($"\nInitilization error: {ex.Message}");
                }                
            }
        }
    }
}
