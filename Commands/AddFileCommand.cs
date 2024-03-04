using _3D_viewer.ViewModels;
namespace _3D_viewer.Commands
{
    internal class AddFileCommand : CommandBase
    {
        
        public override void Execute(object sender)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Document"; // Default file name
            dialog.DefaultExt = ".txt"; // Default file extension
            dialog.Filter = "*All files|*.obj*"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();
            
             //Process open file dialog box results
             if (result == true)
             {
                 // Open document
                 string filename = dialog.FileName;
                
                //NameObjFile
                //filename = filename.Remove(0, filename.LastIndexOf(@"\") + 1);
                //NameObjFile.Add(new CheckBox());
                //NameObjFile[NameObjFile.Count - 1].Content = filename;
                //AddGeometricModel(filename);
             }
        }
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        

    }
}
