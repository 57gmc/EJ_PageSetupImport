# EJ_PageSetupImport
AutoCAD commands to import page setups into a drawing.
	PageSetupImportConfig - This command shows a dialog to choose a drawing or template to import page setups from. The configuration is saved in the registry.
	PageSetupImportND (No Dialog)- This command imports the page setups from the file set in the previous command. If you haven't run the configuration command prior to this, a reminder is issued.

The project compiles the Release version to an AutoCAD *.bundle folder. If you change the Assembly Information, then you also need to update the PackageContents.xml file.

The bundle also contains an AutoCAD menu cuix file that loads when the bundle loads.
