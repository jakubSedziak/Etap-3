using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Projekt.Model;
using Projekt.ViewModel;
using Zycie.Services;

namespace Projekt.View
{
    public class ConsoleView
    {

        private ITreeViewItem lastone;
        private Services.IDeserialize des;
        private Services.ISerialize srv;
        private AssemblyMetadata assemblyMetadata;
        private string pathVariable;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ITreeViewItem rootItem;
        public ConsoleView()
        {
            des = new Services.JsonDeserialization();
            srv = new Services.JsonSerialization();
            Console.WriteLine("1:Wczytaj Assebbli z pliku  ");
            Console.WriteLine("2:Wczytaj drzewo z pliku");
            Console.WriteLine("3:Wczytaj drzewo z Bazy");
            char key = Console.ReadKey().KeyChar;
            if (key == 49)
            {
                Console.WriteLine("Podaj sciezke");
                string path = Console.ReadLine();
                setPathVariable(path);

                try
                {
                    if (System.IO.Path.GetExtension(pathVariable) == ".dll" || System.IO.Path.GetExtension(pathVariable) == ".exe")
                    {
                        log.Info("Zaladowano plik: " + pathVariable);
                        run();
                        Console.WriteLine(rootItem.Name);
                        lastone = rootItem;
                        ExpandTree(rootItem, "/0");
                        WriteTree(rootItem, 1);
                        petla();
                    }
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine(e);
                }
            }
            if (key == 50)
            {
                Console.WriteLine("Podaj sciezke skad to odczytac");
                string path;
                path = Console.ReadLine();
                try
                {
                    assemblyMetadata = des.Deserialize(path);
                    pathVariable = "DeserializedItemsJSON";
                    TreeViewLoaded(assemblyMetadata);
                    Console.WriteLine(rootItem.Name);
                    lastone = rootItem;
                    ExpandTree(rootItem, "/0");
                    WriteTree(rootItem, 1);
                    petla();
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine(e);
                }

            }
            if (key == 51)
            {
                Console.WriteLine("Podaj numer assembly");
                string path = Console.ReadLine();
                SQLDeserialization ss = new SQLDeserialization();
                assemblyMetadata = ss.Deserialize(path);
                pathVariable = "DeserializedItemsSQL";
                TreeViewLoaded(assemblyMetadata);
                Console.WriteLine(rootItem.Name);
                lastone = rootItem;
                ExpandTree(rootItem, "/0");
                WriteTree(rootItem, 1);
                petla();
            }
        }
        public void petla()
        {

            while (true)
            {
                string path = "";
                path = Console.ReadLine();
                if (path.Contains("info"))
                {
                    path = path.Replace("info", "");
                    while (path.Contains(" "))
                    {
                        path = path.Replace(" ", "");
                    }
                    bool finded = false;
                    Console.Clear();
                    foreach (ITreeViewItem tree in lastone.Children)
                    {
                        if (tree != null)
                            if (tree.Name == path)
                            {
                                Console.WriteLine(tree.ToString());
                                tree.IsExpanded = true;
                                finded = true;
                                break;
                            }
                    }

                    if (!finded)
                        Console.WriteLine("There is no such item");

                    Console.ReadKey();
                    Console.WriteLine(rootItem.Name);
                    Console.Clear();
                    WriteTree(rootItem, 1);
                }
                else
                {
                    if (path.Contains("serialize"))
                        serialize();
                    else
                    {
                        if (path == "exit")
                            break;
                        else if (path.Contains("tostart"))
                        {
                            Console.Clear();
                            rootItem.Children.Clear();
                            rootItem.IsExpanded = false;

                            ExpandTree(rootItem, "/0");

                            Console.WriteLine(rootItem.Name);
                            WriteTree(rootItem, 1);
                        }
                        else
                        {
                            Console.Clear();
                            if (path.Contains("/"))
                            {
                                path += '/';
                                path += '\0';
                                string enopath = "";
                                int g = 0;
                                while (path[g] != '/')
                                {
                                    g++;
                                }
                                g++;
                                while (path[g] != '\0')
                                {
                                    enopath += path[g];
                                    g++;
                                }
                                enopath += '\0';
                                ExpandTree(rootItem, enopath);
                                Console.WriteLine();
                                Console.WriteLine(rootItem.Name);
                                WriteTree(rootItem, 1);
                            }
                            else
                            {
                                bool finded = false;
                                foreach (ITreeViewItem tree in lastone.Children)
                                {
                                    if (tree != null)
                                        if (tree.Name == path)
                                        {
                                            lastone = tree;
                                            tree.IsExpanded = true;
                                            finded = true;
                                            break;
                                        }

                                }
                                if (!finded)
                                    Console.WriteLine("There is no such item");
                                Console.WriteLine();
                                Console.WriteLine(rootItem.Name);
                                WriteTree(rootItem, 1);
                            }
                        }
                    }
                }
            }
        }
        public ConsoleView(string path) //for testing purpose
        {
            this.pathVariable = path;
            run();
        }
        public void setPathVariable(string sciezka)
        {
            bool isBadExtension = false;
            string path = sciezka;
            do
            {
                if (path == null || path.Length <= 4 || isBadExtension == true)
                {
                    do
                    {
                        log.Error("Zla sciezka, podaj jeszcze raz sciezke:\n");
                        path = Console.ReadLine();
                    } while (path.Length <= 4);
                }
                string extensionCheck = path.Substring(path.Length - 3);
                if (extensionCheck == "exe" || extensionCheck == "dll")
                    isBadExtension = false;
                else
                {
                    log.Error("plik ma bledne rozszerzenie");
                    isBadExtension = true;
                    continue;
                }
            } while (isBadExtension);
            pathVariable = path;
        }
        public void run()
        {
            Assembly assembly = Assembly.LoadFrom(pathVariable);
            assemblyMetadata = new AssemblyMetadata(assembly);
            TreeViewLoaded(assemblyMetadata);
        }

        public void serialize()
        {
            Console.WriteLine("Podaj sciezke");
            string path = Console.ReadLine();
            try
            {

                srv.Serialize(assemblyMetadata, path);
                Console.WriteLine("serialize succeed");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
        private void TreeViewLoaded(AssemblyMetadata assembly)
        {
            rootItem = new AssemblyTreeViewItem(assembly) { Name = pathVariable.Substring(pathVariable.LastIndexOf('\\') + 1) };
        }
        public void WriteTree(ITreeViewItem mainItem, int odstep)
        {
            foreach (ITreeViewItem tree in mainItem.Children)
            {
                if (tree != null)
                {
                    for (int i = 0; i < 2 * odstep; i++) Console.Write(" ");
                    if (tree.Children.Count > 0)
                    {
                        if (tree != lastone)
                            Console.WriteLine("<>" + tree.Name);
                        else
                        {
                            Console.WriteLine("*<>" + tree.Name);
                        }
                    }
                    else
                    {
                        if (tree != lastone)
                            Console.WriteLine(tree.Name);
                        else
                        {
                            Console.WriteLine("*" + tree.Name);
                        }
                    }
                    WriteTree(tree, odstep + 1);
                }
            }
        }
        public ITreeViewItem getRootItem()
        {
            return rootItem;
        }
        public void ExpandTree(ITreeViewItem mainItem, string path)
        {
            if (!mainItem.IsExpanded)
            {
                mainItem.IsExpanded = true;
                lastone = mainItem;
            }
            foreach (ITreeViewItem tree in mainItem.Children)
            {
                if (tree != null)
                {
                    string main = "";
                    int i = 0;
                    while (path[i] != '/' && path[i] != '\0')
                    {

                        main += path[i];
                        i++;
                    }
                    if (tree != null && tree.Name == main)
                    {

                        if (!tree.IsExpanded)
                        {
                            tree.IsExpanded = true;
                            lastone = tree;
                        }
                        string newpath = "";

                        while (path[i] != '\0')
                        {
                            newpath += path[i];
                            i++;
                        }
                        ExpandTree(tree, newpath);

                    }
                }

            }
        }
    }
}
