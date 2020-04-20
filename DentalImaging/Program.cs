using System;
using System.Threading;
using System.Windows.Forms;
using WareHouseMis.UI;
using DentalImaging.Model;
using log4net;
using System.IO;
using log4net.Config;
using System.Configuration;
using DentalImaging.Help;
using DentalImaging.NewForm;

namespace DentalImaging
{
    public class Portal
    {
        public static GlobalControl gc = new GlobalControl();
        static ILog logger;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {              
                var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
                XmlConfigurator.ConfigureAndWatch(logCfg);
                logger = LogManager.GetLogger(typeof(Portal));

                TimeClass.CreateRegedit();
                User.Language = ConfigurationManager.AppSettings["Language"];
                User.Date = TimeClass.InitRegedit();
                User.IsRegist = User.Date >= 0;
                //SetUIConstants();
                GlobalMutex();

                DevExpress.UserSkins.BonusSkins.Register();
                DevExpress.Skins.SkinManager.EnableFormSkins();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                //处理未捕获的异常
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                //处理UI线程异常
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                //处理非UI线程异常
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);                

                ChannelChoose choose = new ChannelChoose();
                if (choose.ShowDialog() == DialogResult.OK)
                {
                    gc.MainDialog = new MainForm();
                    gc.MainDialog.StartPosition = FormStartPosition.CenterScreen;
                    Application.Run(gc.MainDialog);
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        private static Mutex mutex = null;
        private static void GlobalMutex()
        {
            // 是否第一次创建mutex
            bool newMutexCreated = false;
            string mutexName = "Global\\" + "WareHouseMis";
            try
            {
                mutex = new Mutex(false, mutexName, out newMutexCreated);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Thread.Sleep(1000);
                Environment.Exit(1);
            }

            // 第一次创建mutex
            if (newMutexCreated)
            {
                Console.WriteLine("程序已启动");
            }
            else
            {
                CommHelp.ShowTips("另一个窗口已在运行，不能重复运行。");
                Thread.Sleep(1000);
                Environment.Exit(1);//退出程序
            }
        }

        /// <summary>
        /// 是否退出应用程序
        /// </summary>
        public static bool glExitApp = false;

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Error("IsTerminating : " + e.IsTerminating.ToString());
            logger.Error(e.ExceptionObject.ToString());

            while (true)
            {//循环处理，否则应用程序将会退出
                if (glExitApp)
                {//标志应用程序可以退出，否则程序退出后，进程仍然在运行
                    logger.Error("ExitApp");                    
                    return;
                }
                Thread.Sleep(2 * 1000);
            };
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs ex)
        {
            logger.Error(ex.Exception);

            //string message = string.Format("{0}\r\n操作发生错误，您需要退出系统么？", ex.Exception.Message);
            //if (DialogResult.Yes == MessageDxUtil.ShowYesNoAndError(message))
            //{
            //    Application.Exit();
            //}
        }
    }
}
