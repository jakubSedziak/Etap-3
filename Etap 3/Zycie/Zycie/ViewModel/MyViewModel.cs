using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using System.Reflection;
using Projekt.Model;
using System.Collections;

using System.Linq;
using Projekt.Services;
using Zycie.CustomLoggers;
using Zycie.Services;

namespace Projekt.ViewModel
{
    /// <summary>
    /// Class MyViewModel - ViewModel implementation 
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class MyViewModel : INotifyPropertyChanged
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        FileLogger flog=new FileLogger();
        #region constructors
        public MyViewModel()
        {
            des = new Services.JsonDeserialization();
            srv  = new Services.JsonSerialization();
            HierarchicalAreas = new ObservableCollection<ITreeViewItem>();
            Click_Button = new DelegateCommand(LoadDLL);
            Click_Browse = new DelegateCommand(Browse);
            Click_SerializeJson = new DelegateCommand(Jsonserialize);
            Click_DeserializeJson = new DelegateCommand(Jsondeserialize);
            Click_SerializeSQL = new DelegateCommand(SQLserialize);
            Click_DeserializeSQL = new DelegateCommand(SQLdeserialize);
        }
        #endregion

        #region DataContext

        private Services.ISerialize srv;
        private Services.IDeserialize des;
       AssemblyMetadata assemblyMetadata;
        public ObservableCollection<ITreeViewItem> HierarchicalAreas { get; set; }
      
        public string PathVariable
        {
            get { return pathVariable; }
            set { pathVariable = value; }
        }
        public Visibility ChangeControlVisibility
        {
            get { return _visibility; }
            set
            {
                _visibility = value;
            }
        }
        public ICommand Click_Browse { get; }
        public ICommand Click_Button { get; }
        public ICommand Click_SerializeJson { get; }
        public ICommand Click_DeserializeJson { get; }
        public ICommand Click_SerializeSQL { get; }
        public ICommand Click_DeserializeSQL { get; }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName_)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName_));
        }
        #endregion

        #region private
        private void  SQLdeserialize()
        {
          
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Wprowadz sciezke ", "Serialization", "");
            if (UserAnswer == "")
            {
                MessageBox.Show("nic nie wprowadziles");
            }
            else
            {
                string path = UserAnswer;
                _visibility = Visibility.Visible;
                RaisePropertyChanged("ChangeControlVisibility");
                RaisePropertyChanged("PathVariable");
                log.Info("Deserializowano Assembly: " + path);
                flog.Log("Deserializowano Assembly: " + path);
                HierarchicalAreas.Clear();
                
                SQLDeserialization ss = new SQLDeserialization();
                assemblyMetadata = ss.Deserialize(path);
                pathVariable = "DeserializedSQLItems";
               // assemblyMetadata.IsExpanded = true;
                TreeViewLoaded(assemblyMetadata);
                
            }
        }
        private void Jsondeserialize()
        {
            OpenFileDialog test = new OpenFileDialog();
            test.Filter = "Text files (*.txt)|*.txt";
            test.ShowDialog();
            if (test.FileName.Length == 0)
            {
                MessageBox.Show("No files selected");
                log.Info("Wybrano pusty plik: ");
                flog.Log("Wybrano pusty plik: ");
            }
            else
            {
                string path = test.FileName;
                _visibility = Visibility.Visible;
                RaisePropertyChanged("ChangeControlVisibility");
                RaisePropertyChanged("PathVariable");
                log.Info("Deserializowano plik: " + path);
                flog.Log("Deserializowano plik: " + path);
                HierarchicalAreas.Clear();
                assemblyMetadata = des.Deserialize(path);
           //  SqlSerialization ss = new SqlSerialization();
             //   assemblyMetadata = ss.Deserialize(2);
                pathVariable = "DeserializedJsonItems";
                TreeViewLoaded(assemblyMetadata);
                
            }
        }
        private void SQLserialize()
        {
                      SqlSerialization ssq= new SqlSerialization();
                      ssq.Serialize(assemblyMetadata,"nic))");
             
        }
        private void Jsonserialize()
        {
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Wprowadz sciezke ", "Serialization", "");
            if (UserAnswer == "")
            {
                MessageBox.Show("nic nie wprowadziles");
            }
            else
            {
                if (assemblyMetadata == null)
                {
                    MessageBox.Show("NAJPIERW COS WYBIERZ");
                }
                else
                {             
                   srv.Serialize(assemblyMetadata, UserAnswer);
                }
            }
        }
        public void setPath(string path)
        {
            pathVariable = path;
        }
        private string pathVariable;
        private Visibility _visibility = Visibility.Hidden;
        public  void LoadDLL()
        {
            if ( System.IO.Path.GetExtension(pathVariable)==".dll" || System.IO.Path.GetExtension(pathVariable) == ".exe")
            {
                log.Info("Zaladowano plik: " + pathVariable);
                flog.Log("Zaladowano plik: " + pathVariable);
                Assembly assembly = Assembly.LoadFrom(PathVariable);
                assemblyMetadata= new AssemblyMetadata(assembly);
                TreeViewLoaded(assemblyMetadata);
            }
        }
        private void TreeViewLoaded(AssemblyMetadata assembly)
        {
            ITreeViewItem rootItem = new AssemblyTreeViewItem(assembly) { Name = pathVariable.Substring(pathVariable.LastIndexOf('\\') + 1) };
            HierarchicalAreas.Add(rootItem);
        }
        private void Browse()
        {
            OpenFileDialog test = new OpenFileDialog();
            test.Filter = "Execute files (*.exe)|*.exe|Dynamic Library File(*.dll)| *.dll";
            test.ShowDialog();
            if (test.FileName.Length == 0)
            {
                MessageBox.Show("No files selected");
                log.Info("Wybrano pusty plik: ");
            }
            else
            {
                pathVariable = test.FileName;
                _visibility = Visibility.Visible;
                RaisePropertyChanged("ChangeControlVisibility");
                RaisePropertyChanged("PathVariable");
                log.Info("Wybrano plik: " + pathVariable);
            }
        }
        #endregion

        public AssemblyMetadata GetAssemblyMetadata()
        {
            return assemblyMetadata;
        }

    }
}