using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MemoryGame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        public GamePage()
        {
            this.InitializeComponent();
            CreateGrid();
        }
        private GameBoard gameBoard = new GameBoard();
        private GamePicture gamePictures = new GamePicture();
        private SolidColorBrush black = new SolidColorBrush(Windows.UI.Colors.Black);
        private SolidColorBrush blue = new SolidColorBrush(Windows.UI.Colors.CadetBlue);

        private void boardCanvas_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int numberOfCells = gameBoard.GridSize;
            int rectSize = (int)boardCanvas.Width / numberOfCells;
            //Rectangle rect = sender as Rectangle;

            Point mousePosition = e.GetPosition(boardCanvas);
            int row = (int)(mousePosition.Y) / rectSize;
            int col = (int)(mousePosition.X) / rectSize;
            // var rowCol = (Point)rect.Tag;
            gameBoard.Move(row, col);
            // Redraw the board
            DrawGrid();
            GameCompleted();

        }
        private async void GameCompleted()
        {
            if (gameBoard.IsGameOver())
            {
                MessageDialog msgDialog = new MessageDialog("Congratulations!  You've won!");
                // Add an OK button
                msgDialog.Commands.Add(new UICommand("OK"));
                // Show the message box and wait aynchrously for a button press
                IUICommand command = await msgDialog.ShowAsync();
            }
        }
        private void CreateGrid()
        {
            int numberOfCells = gameBoard.GridSize;
            // Remove all previously-existing rectangles
            boardCanvas.Children.Clear();

            //grid = new bool[numberOfCells, numberOfCells];
            int rectSize = (int)boardCanvas.Width / numberOfCells;
            //gameBoard.NewGame();

            // Turn entire grid on and create rectangles to represent it
            for (int r = 0; r < numberOfCells; r++)
            {
                for (int c = 0; c < numberOfCells; c++)
                {
                    //grid[r, c] = true;

                    Rectangle rect = new Rectangle();

                    rect.Fill = blue;
                    rect.Width = rectSize + 1;
                    rect.Height = rect.Width + 1;
                    rect.Stroke = black;

                    // Store each row and col as a Point
                    rect.Tag = new Point(r, c);
                    //rect. += Rect_MouseLeftButtonDown;

                    int x = c * rectSize;
                    int y = r * rectSize;

                    Canvas.SetTop(rect, y);
                    Canvas.SetLeft(rect, x);

                    // Add the new rectangle to the canvas' children
                    boardCanvas.Children.Add(rect);
                }
            }
        }
        private void DrawGrid()
        {
            int index = 0;
            int numberOfCells = gameBoard.GridSize;
            // Set colors of each rectangle based on grid values
            for (int r = 0; r < numberOfCells; r++)
            {
                for (int c = 0; c < numberOfCells; c++)
                {
                    Rectangle rect = boardCanvas.Children[index] as Rectangle;
                    index++;

                    if (gameBoard.GetGridValue(r, c))
                    {
                        // On
                        rect.Fill = blue;
                        rect.Stroke = black;
                    }
                    else
                    {
                        // Off
                        // put the picture here 

                        rect.Stroke = black;
                    }
                }
            }
        }



        private void newGame_Click(object sender, RoutedEventArgs e)
        {
            gameBoard.NewGame();
            DrawGrid();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var navManager = SystemNavigationManager.GetForCurrentView();
            if (this.Frame.CanGoBack)
            {
                // Show Back button in title bar
                navManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else
            {
                // Remove Back button from title bar
                navManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }

            // Register BackRequested handler
            navManager.BackRequested += SecondPage_BackRequested;
        }
        private void SecondPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (this.Frame.CanGoBack)
                this.Frame.GoBack();
        }
    }
}
