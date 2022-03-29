using Core;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MarsUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        public HubConnection Connection;
        public IHubProxy MyHub;
        public async Task Baglan()
        {
            Connection = new HubConnection("http://online.arvesoft.com/");
            //Connection = new HubConnection("http://localhost:3048/");
            MyHub = Connection.CreateHubProxy("LisansHub");
            //client Masaüstü
            MyHub.On("cMSagaCevir", CMSagaCevir);
            MyHub.On("cMSolaCevir", CMSolaCevir);
            MyHub.On("cMIleriGit", CMIleriGit);
            await Connection.Start().ContinueWith(task =>
            {
            });
        }
        private void CMSagaCevir()
        {
            DispatchService.InvokeOnUi(() =>
            {
                var rover = (Rover)DataContext;
                if (rover.Yon == (int)YonEnum.Bati)
                {
                    rover.Yon = (int)YonEnum.Kuzey;
                }
                else
                    rover.Yon = rover.Yon + 1;

                rover.Acisi = rover.Yon * 90;
                DataContext = rover;
                var rotateTransform = new RotateTransform(rover.Acisi);
                BtnArac.RenderTransform = rotateTransform;
            });           
        }

        private void CMSolaCevir()
        {
            DispatchService.InvokeOnUi(() =>
            {
                var rover = (Rover)DataContext;
                if (rover.Yon == (int)YonEnum.Kuzey)
                {
                    rover.Yon = (int)YonEnum.Bati;
                }
                else
                    rover.Yon = rover.Yon - 1;

                rover.Acisi = rover.Yon * 90;

                DataContext = rover;
                var rotateTransform = new RotateTransform(rover.Acisi);
                BtnArac.RenderTransform = rotateTransform;
            });
        }

        private void CMIleriGit()
        {

            DispatchService.InvokeOnUi(() =>
            {
                var rover = (Rover)DataContext;
                if (rover.Yon == (int)YonEnum.Kuzey)
                {
                    rover.XPosition--;
                    if (rover.XPosition < 0)
                    {
                        rover.XPosition = 0;
                    }
                }
                else if (rover.Yon == (int)YonEnum.Guney)
                {
                    rover.XPosition++;
                    if (rover.XPosition >= GridPlato.RowDefinitions.Count)
                    {
                        rover.XPosition = GridPlato.RowDefinitions.Count - 1;
                    }
                }
                else if (rover.Yon == (int)YonEnum.Dogu)
                {
                    rover.YPosition++;
                    if (rover.YPosition >= GridPlato.ColumnDefinitions.Count)
                    {
                        rover.YPosition = GridPlato.ColumnDefinitions.Count - 1;
                    }
                }

                else if (rover.Yon == (int)YonEnum.Bati)
                {
                    rover.YPosition--;
                    if (rover.YPosition < 0)
                    {
                        rover.YPosition = 0;
                    }
                }
                DataContext = rover;
                BtnArac.SetValue(Grid.RowProperty, rover.XPosition);
                BtnArac.SetValue(Grid.ColumnProperty, rover.YPosition);
            });
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var rover = new Rover();
            rover.YPosition = 0;
            rover.XPosition = GridPlato.RowDefinitions.Count - 1;
            rover.Yon = (int)YonEnum.Kuzey;
            rover.Acisi = rover.Yon * 90;
            DataContext = rover;

            Baglan();
        }
    }
}