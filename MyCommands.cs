// (C) Copyright 2022 by Ed Jobe 
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose with fee is hereby granted, 
// provided that the above copyright notice appears in all copies and 
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting 
// documentation.
//


using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using EJ_.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using acApp = Autodesk.AutoCAD.ApplicationServices.Application;
using acWin = Autodesk.AutoCAD.Windows;


// This line is not mandatory, but improves loading performance
[assembly: CommandClass(typeof(EJ_.EJ_Commands))]

namespace EJ_
{
    /// <summary>
    /// This class is instantiated by AutoCAD for each document when
    /// a command is called by the user the first time in the context
    /// of a given document. In other words, non static data in this class
    /// is implicitly per-document!
    /// </summary>
    public class EJ_Commands
    {

        //  ImportPageSetupsND
        /// <summary>
        /// Using settings captured by the ImportPageSetupsConfig command, imports page setups
        /// from another drawing into the current drawing. ND suffix indicates that No Dialog
        /// is used in this command. Besides importing page setups, named views needed for plotting
        /// are also imported.
        /// </summary>
        [CommandMethod("EJ", "ImportPageSetupsND", CommandFlags.Modal)]
        public void ImportPageSetupsND()
        {
            try
            {
                if (Properties.Settings.Default.PlotSettingPath != "")
                {
                    // Read the DWG into a side database
                    Database sourceDb = new Database(false, true);
                    sourceDb.ReadDwgFile(Properties.Settings.Default.PlotSettingPath,
                                        System.IO.FileShare.Read,
                                        true,
                                        "");
                    // get the active db.
                    Database destDb = Active.Database;

                    using (Transaction destTrans = destDb.TransactionManager.StartTransaction())
                    {
                        // delete existing page setups
                        DBDictionary destDict = destTrans.GetObject(destDb.PlotSettingsDictionaryId, OpenMode.ForWrite) as DBDictionary;
                        foreach (DictionaryEntry de in destDict)
                        {
                            PlotSettings plotSettings = destTrans.GetObject((ObjectId)(de.Value), OpenMode.ForWrite) as PlotSettings;
                            plotSettings.Erase();

                        }
                        // rename existing plot views first in case a layout refers to one
                        // delete them after importing and setting layout.ViewToPlot to new view
                        ViewTable destVT = destTrans.GetObject(destDb.ViewTableId, OpenMode.ForWrite) as ViewTable;
                        foreach (ObjectId dId in destVT)
                        {
                            ViewTableRecord dVTR = destTrans.GetObject(dId, OpenMode.ForWrite) as ViewTableRecord;
                            if (Regex.IsMatch(dVTR.Name, "PLOT"))
                            {
                                dVTR.Name = "x" + dVTR.Name;
                            }
                        }

                        using (Transaction sourceTrans = sourceDb.TransactionManager.StartTransaction())
                        {
                            // import template page setups
                            // Importing page setups also brings in referenced views.
                            DBDictionary sourceDict = sourceTrans.GetObject(sourceDb.PlotSettingsDictionaryId, OpenMode.ForRead) as DBDictionary;
                            foreach (DictionaryEntry de in sourceDict)
                            {
                                PlotSettings SourcePlotSettings = sourceTrans.GetObject((ObjectId)(de.Value), OpenMode.ForRead) as PlotSettings;
                                PlotSettings tempPlotSettings = new PlotSettings(SourcePlotSettings.ModelType);
                                tempPlotSettings.CopyFrom(SourcePlotSettings);
                                tempPlotSettings.AddToPlotSettingsDictionary(destDb);
                                destTrans.AddNewlyCreatedDBObject(tempPlotSettings, true);
                            }
                            sourceTrans.Commit();
                        }
                        // iterate layouts and set a page setup
                        DBDictionary LayoutsDict = destTrans.GetObject(destDb.LayoutDictionaryId, OpenMode.ForRead) as DBDictionary;
                        foreach (DBDictionaryEntry entry in LayoutsDict)
                        {
                            DBDictionary plsDict = destTrans.GetObject(destDb.PlotSettingsDictionaryId, OpenMode.ForRead) as DBDictionary;
                            ObjectId lid = entry.Value;
                            Layout lay = destTrans.GetObject(lid, OpenMode.ForWrite) as Layout;
                            if (lay.ModelType == false)  // false = PaperSpace
                            {
                                PlotSettings pls = plsDict.GetAt(Properties.Settings.Default.PlotSettingPS).GetObject(OpenMode.ForRead) as PlotSettings;
                                lay.CopyFrom(pls);
                            }
                            else
                            {
                                PlotSettings pls = plsDict.GetAt(Properties.Settings.Default.PlotSettingMS).GetObject(OpenMode.ForRead) as PlotSettings;
                                lay.CopyFrom(pls);
                            }

                        }

                        // delete existing plot views
                        foreach (ObjectId dId in destVT)
                        {
                            ViewTableRecord dVTR = destTrans.GetObject(dId, OpenMode.ForWrite) as ViewTableRecord;
                            if (Regex.IsMatch(dVTR.Name, "xPLOT"))
                            {
                                dVTR.Erase();
                            }
                        }
                        destTrans.Commit();
                        Active.Editor.WriteMessage("\nPage setups imported successfully.");
                    }
                }
                else
                {
                    MessageBox.Show("You must first run the EJImportPageSetupsConfig command.", "EJImportPageSetupsND", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                MessageBox.Show("Error Occured in EJImportPageSetupsND command. " + ex.Message + ", " + ex.HelpLink, "EJImportPageSetupsND Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            };

        }

        //  ImportPageSetupsConfig
        /// <summary>
        /// Configures setup for the ImportPageSetupsND command.
        /// </summary>
        [CommandMethod("EJ", "ImportPageSetupsConfig", CommandFlags.Modal)]
        public void ImportPageSetupsConfig()
        {
            acWin.OpenFileDialog fb = new acWin.OpenFileDialog("Select a File to import page setups from.",
                                                                "",
                                                                "dwg;dwt",
                                                                "ImportPS",
                                                                acWin.OpenFileDialog.OpenFileDialogFlags.NoFtpSites |
                                                                acWin.OpenFileDialog.OpenFileDialogFlags.NoUrls
                                                                );
            System.Windows.Forms.DialogResult dr = fb.ShowDialog();

            try
            {
                if (dr != DialogResult.Cancel)
                {
                    // Store the dwg path.
                    Properties.Settings.Default.PlotSettingPath = fb.Filename;
                    Properties.Settings.Default.Save();
                    // Read the DWG into a side database
                    Database sourceDb = new Database(false, true);
                    sourceDb.ReadDwgFile(fb.Filename,
                                        System.IO.FileShare.Read,
                                        true,
                                        "");
                    // Get the page setups.
                    try
                    {
                        string[] plotSettingsMS = new string[] { "**none**" };
                        string[] plotSettingsPS = new string[] { "**none**" };
                        using (Transaction trans = sourceDb.TransactionManager.StartTransaction())
                        {
                            DBDictionary sourceDict = trans.GetObject(sourceDb.PlotSettingsDictionaryId, OpenMode.ForRead) as DBDictionary;
                            foreach (DictionaryEntry de in sourceDict)
                            {
                                PlotSettings plotSettings = trans.GetObject((ObjectId)(de.Value), OpenMode.ForRead) as PlotSettings;
                                // Separate MS page setups from PS page setups.
                                if (plotSettings.ModelType == true)
                                {
                                    Array.Resize(ref plotSettingsMS, plotSettingsMS.Length + 1);
                                    plotSettingsMS.SetValue(plotSettings.PlotSettingsName, plotSettingsMS.Length - 1);
                                }
                                else
                                {
                                    Array.Resize(ref plotSettingsPS, plotSettingsPS.Length + 1);
                                    plotSettingsPS.SetValue(plotSettings.PlotSettingsName, plotSettingsPS.Length - 1);
                                }
                            }
                            trans.Commit();
                        }

                        // Show Dialog with page setup names
                        // Have the user select a MS config.
                        EJ_.Forms.PageSetups frmPageSetups = new EJ_.Forms.PageSetups();
                        frmPageSetups.SetListItems = plotSettingsMS;
                        frmPageSetups.SetPrompt = "Select MS page setup.";
                        dr = acApp.ShowModalDialog(frmPageSetups);
                        string msPageSetup = frmPageSetups.Selected;
                        if (dr == DialogResult.OK)
                        {
                            Properties.Settings.Default.PlotSettingMS = frmPageSetups.Selected;
                            Properties.Settings.Default.Save();
                        }
                        else
                        {
                            return;  // User cancelled.
                        }

                        // Have the user select a PS config.
                        frmPageSetups.SetListItems = plotSettingsPS;
                        frmPageSetups.SetPrompt = "Select PS page setup.";
                        dr = acApp.ShowModalDialog(frmPageSetups);
                        string psPageSetup = frmPageSetups.Selected;
                        if (dr == DialogResult.OK)
                        {
                            Properties.Settings.Default.PlotSettingPS = frmPageSetups.Selected;
                            Properties.Settings.Default.Save();
                        }
                        else
                        {
                            return;  // User cancelled.
                        }

                    }
                    catch (Autodesk.AutoCAD.Runtime.Exception ex)
                    {
                        //Throw Exception or ignore if user cancelled command.
                        if (ex.ErrorStatus != ErrorStatus.NullObjectId)
                        {
                            MessageBox.Show("Error Occured in ImportPageSetupsConfig command. " + ex.Message + ", " + ex.HelpLink, "ImportPageSetupsConfig Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        };

                    }
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                MessageBox.Show("Error Occured in ImportPageSetupsConfig command. " + ex.Message + ", " + ex.HelpLink, "ImportPageSetupsConfig Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            };

        }

    }
}

