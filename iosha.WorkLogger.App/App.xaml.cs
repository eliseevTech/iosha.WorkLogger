using Autofac;
using iosha.WorkLogger.CloudSender;
using iosha.WorkLogger.Data;
using iosha.WorkLogger.Data.XmlStorage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace iosha.WorkLogger.App
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        IContainer container;
        public App()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<Hooker>().AsSelf();
            containerBuilder.RegisterType<WorkTimer>().AsSelf();

            containerBuilder.RegisterType<MainWindow>().AsSelf();

            containerBuilder.RegisterType<WorkLogXmlManager>().As(typeof(IWorkLogManager));

            containerBuilder.RegisterType<AutoStartManager>().AsSelf();

            containerBuilder.RegisterType<CloudSender.CloudSender>().As(typeof(ICloudSender));

            container = containerBuilder.Build();

            MainWindow mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
        }
    }

}
