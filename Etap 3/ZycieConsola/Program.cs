using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows;
using Projekt.Services;
using Projekt.View;
using Zycie;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Application = System.Windows.Forms.Application;

//[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace ZycieConsola
{
    class Program
    {

        public static System.Windows.Application WinApp { get; private set; }
        public static System.Windows.Window MainWindow { get; private set; }

        private static bool isWindow = true;

           private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [STAThread]

        static void Main(string[] args)
        {
            if ((ParentProcessUtilities.GetParentProcess().ProcessName == "cmd") ||
                (ParentProcessUtilities.GetParentProcess().ProcessName == "powershell")
            ) // rozwiązuje problem uruchomienia programu w najpopularniejszych srodowiskach bezparametrowo
            {
                isWindow = false;
            }

            if (args.Length != 0
            ) //rozwiązuje problem uruchomienia programu w innych środowiskach lub z innymi parametrami
            {
                StringBuilder parametry = new StringBuilder();
                foreach (String arg in args)
                    parametry.Append(arg);
                log.Info("Wywołano aplikację z parametrami: " + parametry.ToString());
                if (args[0] == "-c")
                    isWindow = false;
                else if (args[0] == "-w")
                    isWindow = true;
            }
            if (!isWindow)
            {
                log.Info("Uruchomiono Aplikacje Konsolowa");
                ConsoleView konsola = new ConsoleView();

            }
            else
            {
             //   SqlSerialization ss = new SqlSerialization();
                //  ss.databaseConnection();
                log.Info("Uruchomiono Aplikacje Okienkowa");
                InitializeWindows();
            }

        }

        static void InitializeWindows()
        {
            WinApp = new System.Windows.Application();
            WinApp.Run(MainWindow = new Projekt.View.Okno());
        }

        /// <summary>
        /// A utility class to determine a process parent.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ParentProcessUtilities
        {
            // These members must match PROCESS_BASIC_INFORMATION
            internal IntPtr Reserved1;

            internal IntPtr PebBaseAddress;
            internal IntPtr Reserved2_0;
            internal IntPtr Reserved2_1;
            internal IntPtr UniqueProcessId;
            internal IntPtr InheritedFromUniqueProcessId;

            [DllImport("ntdll.dll")]
            private static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass,
                ref ParentProcessUtilities processInformation, int processInformationLength, out int returnLength);

            /// <summary>
            /// Gets the parent process of the current process.
            /// </summary>
            /// <returns>An instance of the Process class.</returns>
            public static Process GetParentProcess()
            {
                return GetParentProcess(Process.GetCurrentProcess().Handle);
            }

            /// <summary>
            /// Gets the parent process of specified process.
            /// </summary>
            /// <param name="id">The process id.</param>
            /// <returns>An instance of the Process class.</returns>
            public static Process GetParentProcess(int id)
            {
                Process process = Process.GetProcessById(id);
                return GetParentProcess(process.Handle);
            }

            /// <summary>
            /// Gets the parent process of a specified process.
            /// </summary>
            /// <param name="handle">The process handle.</param>
            /// <returns>An instance of the Process class or null if an error occurred.</returns>
            public static Process GetParentProcess(IntPtr handle)
            {
                ParentProcessUtilities pbi = new ParentProcessUtilities();
                int returnLength;
                int status = NtQueryInformationProcess(handle, 0, ref pbi, Marshal.SizeOf(pbi), out returnLength);
                if (status != 0)
                    return null;

                try
                {
                    return Process.GetProcessById(pbi.InheritedFromUniqueProcessId.ToInt32());
                }
                catch (ArgumentException e)
                {
                    log.Error(e);
                    return null;
                }
            }
        }
    }
}
