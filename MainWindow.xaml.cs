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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Threading;

namespace Projet6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public PlayerColor[,] Tiles { get; set; }
        public GameState GameState { get; set; }
        public PlayerTurn PlayerTurn;
        Ellipse selectedCircle;
        (int, int) selectedButtonPos = (0, 0);

        public MainWindow()
        {
            InitializeComponent();

            // Initialisation
            myCanvas.Focus();
            myCanvas.KeyDown += OnButtonKeyDown;
            GenerateGrid();
            Tiles = new PlayerColor[7, 7];
            selectedCircle = EllipseFactory();
            PlayerTurn = PlayerTurn.P1;
        }

        void GenerateGrid()
        {
            for (double i = 0; i <= 700; i += 100)
            {
                Rectangle rect = new Rectangle();
                rect.Width = 10;
                rect.Height = 710;
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                rect.SetValue(Canvas.LeftProperty, i);
                // Describes the brush's color using RGB values. 
                // Each value has a range of 0-255.
                mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);
                rect.Fill = mySolidColorBrush;
                myCanvas.Children.Add(rect);
            }

            for (double i = 0; i <= 700; i += 100)
            {
                Rectangle rect = new Rectangle();
                rect.Width = 700;
                rect.Height = 10;
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                rect.SetValue(Canvas.TopProperty, i);
                // Describes the brush's color using RGB values. 
                // Each value has a range of 0-255.
                mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);
                rect.Fill = mySolidColorBrush;
                myCanvas.Children.Add(rect);
            }
        }
        Ellipse EllipseFactory()
        {
            Ellipse circle = new Ellipse();

            myCanvas.Children.Add(circle);

            // Setup circle grid pos
            selectedButtonPos.Item1 = 0;
            selectedButtonPos.Item2 = -1;
            for (int i = 0; i < 7; i++)
            {
                if (Tiles[0, i] == PlayerColor.Null)
                {
                    selectedButtonPos.Item2 = i;
                    break;
                }
            }
            if (selectedButtonPos.Item2 == -1)
            {
                MessageBox.Show("No more tile left. It's a draw !\n Click on OK to restart ");
                Window lastWindow = Application.Current.MainWindow;
                Application.Current.MainWindow = new MainWindow();
                Application.Current.MainWindow.Show();
                lastWindow.Close();
            }

            // Setup circle color
            circle.Width = 100;
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            // P1 is red
            if (PlayerTurn == PlayerTurn.P1)
            {
                mySolidColorBrush.Color = Color.FromArgb(255, 255, 0, 0);
            }
            // P2 is yellow
            if (PlayerTurn == PlayerTurn.P2)
            {
                mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);
            }
            circle.Fill = mySolidColorBrush;

            // Setup circle scale and shown pos
            circle.SetValue(Canvas.WidthProperty, 90.0);
            circle.SetValue(Canvas.HeightProperty, 90.0);
            circle.SetValue(Canvas.TopProperty, 100.0 * selectedButtonPos.Item1 + 10);
            circle.SetValue(Canvas.LeftProperty, 100.0 * selectedButtonPos.Item2 + 10);

            return circle;
        }

        private async void OnButtonKeyDown(object sender, KeyEventArgs e)
        {

            if (GameState == GameState.Place)
            {
                if (e.Key == Key.Right)
                {
                    if (selectedButtonPos.Item2 + 1 >= 7)
                        return;

                    Tiles[selectedButtonPos.Item1, selectedButtonPos.Item2] = PlayerColor.Null;
                    if (Tiles[selectedButtonPos.Item1, selectedButtonPos.Item2 + 1] == PlayerColor.Null)
                    {
                        selectedButtonPos.Item2++;
                        Tiles[selectedButtonPos.Item1, selectedButtonPos.Item2] = PlayerTurn == PlayerTurn.P1 ? PlayerColor.Red : PlayerColor.Yellow;
                    }
                    else
                    {
                        int tileToCheck = 6 - selectedButtonPos.Item2;
                        for (int i = 1; i <= tileToCheck; i++)
                        {
                            if (Tiles[selectedButtonPos.Item1, selectedButtonPos.Item2 + i] == PlayerColor.Null)
                            {
                                selectedButtonPos.Item2 += i;
                                Tiles[selectedButtonPos.Item1, selectedButtonPos.Item2] = PlayerTurn == PlayerTurn.P1 ? PlayerColor.Red : PlayerColor.Yellow;
                                break;
                            }
                        }
                    }


                    selectedCircle.SetValue(Canvas.TopProperty, 100.0 * selectedButtonPos.Item1 + 10);
                    selectedCircle.SetValue(Canvas.LeftProperty, 100.0 * selectedButtonPos.Item2 + 10);
                }
                if (e.Key == Key.Left)
                {
                    if (selectedButtonPos.Item2 - 1 < 0)
                        return;

                    Tiles[selectedButtonPos.Item1, selectedButtonPos.Item2] = PlayerColor.Null;
                    if (Tiles[selectedButtonPos.Item1, selectedButtonPos.Item2 - 1] == PlayerColor.Null)
                    {
                        selectedButtonPos.Item2--;
                        Tiles[selectedButtonPos.Item1, selectedButtonPos.Item2] = PlayerTurn == PlayerTurn.P1 ? PlayerColor.Red : PlayerColor.Yellow;
                    }
                    else
                    {
                        int tileToCheck = selectedButtonPos.Item2;
                        for (int i = 1; i <= tileToCheck; i++)
                        {
                            if (Tiles[selectedButtonPos.Item1, selectedButtonPos.Item2 - i] == PlayerColor.Null)
                            {
                                selectedButtonPos.Item2 -= i;
                                Tiles[selectedButtonPos.Item1, selectedButtonPos.Item2] = PlayerTurn == PlayerTurn.P1 ? PlayerColor.Red : PlayerColor.Yellow;
                                break;
                            }
                        }
                    }
                    selectedCircle.SetValue(Canvas.TopProperty, 100.0 * selectedButtonPos.Item1 + 10);
                    selectedCircle.SetValue(Canvas.LeftProperty, 100.0 * selectedButtonPos.Item2 + 10);
                }

                if (e.Key == Key.Down)
                {
                    GameState = GameState.Drop;
                    if (selectedButtonPos.Item1 == 0 && Tiles[selectedButtonPos.Item1 + 1, selectedButtonPos.Item2] != PlayerColor.Null)
                    {
                        Tiles[selectedButtonPos.Item1, selectedButtonPos.Item2] = PlayerTurn == PlayerTurn.P1 ? PlayerColor.Red : PlayerColor.Yellow;
                    }
                    else
                    {
                        while (selectedButtonPos.Item1 + 1 < 7
                                && Tiles[selectedButtonPos.Item1 + 1, selectedButtonPos.Item2] == PlayerColor.Null)
                        {
                            Tiles[selectedButtonPos.Item1, selectedButtonPos.Item2] = PlayerColor.Null;
                            selectedButtonPos.Item1++;
                            Tiles[selectedButtonPos.Item1, selectedButtonPos.Item2] = PlayerTurn == PlayerTurn.P1 ? PlayerColor.Red : PlayerColor.Yellow;

                            selectedCircle.SetValue(Canvas.TopProperty, 100.0 * selectedButtonPos.Item1 + 10);
                            selectedCircle.SetValue(Canvas.LeftProperty, 100.0 * selectedButtonPos.Item2 + 10);
                            await Task.Delay(1);
                        }
                    }
                    CheckWin();
                    if (PlayerTurn == PlayerTurn.P1)
                    {
                        PlayerTurn = PlayerTurn.P2;
                    }
                    else if (PlayerTurn == PlayerTurn.P2)
                    {
                        PlayerTurn = PlayerTurn.P1;
                    }
                    selectedCircle = EllipseFactory();
                    GameState = GameState.Place;


                }
            }
        }
        public void CheckWin()
        {
            PlayerColor currentPlayer = PlayerTurn == PlayerTurn.P1 ? PlayerColor.Red : PlayerColor.Yellow;
            int points = 1;

            // get right points
            for (int i = 1; i < 7; i++)
            {
                if (selectedButtonPos.Item2 + i < 7 && Tiles[selectedButtonPos.Item1, selectedButtonPos.Item2 + i] == currentPlayer)
                {
                    points++;
                }
                else
                {
                    break;
                }
            }

            // get left points
            for (int i = 1; i < 7; i++)
            {
                if (selectedButtonPos.Item2 - i >= 0 && Tiles[selectedButtonPos.Item1, selectedButtonPos.Item2 - i] == currentPlayer)
                {
                    points++;
                }
                else
                {
                    break;
                }
            }

            points = points >= 4 ? 4 : 1;

            // get bottom points
            for (int i = 1; i < 7; i++)
            {
                if (selectedButtonPos.Item1 + i < 7 && Tiles[selectedButtonPos.Item1 + i, selectedButtonPos.Item2] == currentPlayer)
                {
                    points++;
                }
                else
                {
                    break;
                }
            }

            points = points >= 4 ? 4 : 1;

            // get top right diagonal points
            for (int i = 1; i < 7; i++)
            {
                if (selectedButtonPos.Item2 + i < 7 &&
                    selectedButtonPos.Item1 - i >= 0 &&
                    Tiles[selectedButtonPos.Item1 - i, selectedButtonPos.Item2 + i] == currentPlayer)
                {
                    points++;
                }
                else
                {
                    break;
                }
            }

            // get bottom left diagonal points
            for (int i = 1; i < 7; i++)
            {
                if (selectedButtonPos.Item2 - i >= 0 &&
                    selectedButtonPos.Item1 + i < 7 &&
                    Tiles[selectedButtonPos.Item1 + i, selectedButtonPos.Item2 - i] == currentPlayer)
                {
                    points++;
                }
                else
                {
                    break;
                }
            }

            points = points >= 4 ? 4 : 1;

            // get bottom right diagonal points
            for (int i = 1; i < 7; i++)
            {
                if (selectedButtonPos.Item2 + i < 7 &&
                    selectedButtonPos.Item1 + i < 7 &&
                    Tiles[selectedButtonPos.Item1 + i, selectedButtonPos.Item2 + i] == currentPlayer)
                {
                    points++;
                }
                else
                {
                    break;
                }
            }


            // get top left  diagonal points
            for (int i = 1; i < 4; i++)
            {
                if (selectedButtonPos.Item2 - i >= 0 &&
                    selectedButtonPos.Item1 - i >= 0 &&
                    Tiles[selectedButtonPos.Item1 - i, selectedButtonPos.Item2 - i] == currentPlayer)
                {
                    points++;
                }
                else
                {
                    break;
                }
            }

            if (points >= 4)
            {
                MessageBoxResult messageBoxResult =
                MessageBox.Show(currentPlayer.ToString() + " won !\nReplay?", "My App", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    Window lastWindow = Application.Current.MainWindow;
                    Application.Current.MainWindow = new MainWindow();
                    Application.Current.MainWindow.Show();
                    lastWindow.Close();
                }
            }
        }
    }
}

public enum PlayerColor
{
    Null = 0,
    Red = 1,
    Yellow = 2
}
public enum PlayerTurn
{
    P1 = 0,
    P2 = 1
}
public enum GameState
{
    Place = 0,
    Drop = 1
}