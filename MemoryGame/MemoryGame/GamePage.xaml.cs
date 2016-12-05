using MemoryGame;
using System;
using System.Collections;
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
using Windows.UI.Xaml.Media.Imaging;
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
        List<GamePicture> pictureList = new List<GamePicture>();
        List<Rectangle> rectangles = new List<Rectangle>();
        public GamePage()
        {
            this.InitializeComponent();
            LoadingPictures();
            NumberList(gameSize);
            CreateGrid();
        }
        public int flipCount = 1;
        private GameBoard gameBoard = new GameBoard();
        private Rectangle FlippedFirst;
        private Rectangle FlippedSecond;
        public int gameSize = GameBoard.MinGridSize;
        //private GamePicture gamePictures = new GamePicture();
        private SolidColorBrush black = new SolidColorBrush(Windows.UI.Colors.Black);
        private SolidColorBrush blue = new SolidColorBrush(Windows.UI.Colors.CadetBlue);
        public List<int> numbers = new List<int>();

        static Random rng = new Random();
       

        public List<int> NumberList(int size)
        {
            
            if (size == 4)
            {
                for (int i = 0; i < 8; i++)
                {
                    numbers.Add(i);
                }
                for (int i = 0; i < 8; i++)
                {
                    numbers.Add(i);
                }
            }
            else
            {
                for (int i = 0; i < 18; i++)
                {
                    numbers.Add(i);
                }
                for (int i = 0; i < 18; i++)
                {
                    numbers.Add(i);
                }
            }
            for (int i = numbers.Count - 1; i > 0; i--)

            {
                int swapIndex = rng.Next(i + 1);
                if (swapIndex != i)
                {
                    int tmp = numbers[swapIndex];
                    numbers[swapIndex] = numbers[i];
                    numbers[i] = tmp;
                }
            }
            return numbers;
            
        }
        
        private void LoadingPictures()
        {
            pictureList.Clear();
            //1
            ImageBrush add = new ImageBrush();
            BitmapImage bitmapImage = new BitmapImage(new Uri("ms-appx:///Assets/Add.png"));
            add.ImageSource = bitmapImage;
            //2
            ImageBrush bomb = new ImageBrush();
             bitmapImage = new BitmapImage(new Uri("ms-appx:///Assets/App-core-bomb-icon.png"));
            bomb.ImageSource = bitmapImage;
            //3
            ImageBrush attention = new ImageBrush();
             bitmapImage = new BitmapImage(new Uri("ms-appx:///Assets/attention-icon.png"));
            attention.ImageSource = bitmapImage;
            //4
            ImageBrush book = new ImageBrush();
             bitmapImage = new BitmapImage(new Uri("ms-appx:///Assets/BeOS_Help_book.png"));
            book.ImageSource = bitmapImage;
            //5
            ImageBrush clip = new ImageBrush();
             bitmapImage = new BitmapImage(new Uri("ms-appx:///Assets/Clip-icon.png"));
            clip.ImageSource = bitmapImage;
            //6
            ImageBrush furniture = new ImageBrush();
             bitmapImage = new BitmapImage(new Uri("ms-appx:///Assets/furniture-icon.png"));
            furniture.ImageSource = bitmapImage;
            //7
            ImageBrush gem = new ImageBrush();
             bitmapImage = new BitmapImage(new Uri("ms-appx:///Assets/Gem-icon.png"));
            gem.ImageSource = bitmapImage;
            //8  Last one for the 4 x 4
            ImageBrush speakers = new ImageBrush();
             bitmapImage = new BitmapImage(new Uri("ms-appx:///Assets/speakers.png"));
            speakers.ImageSource = bitmapImage;
            //9
            ImageBrush home = new ImageBrush();
             bitmapImage = new BitmapImage(new Uri("ms-appx:///Assets/home_w.png"));
            home.ImageSource = bitmapImage;
            //10
            ImageBrush star = new ImageBrush();
            bitmapImage = new BitmapImage(new Uri("ms-appx:///Assets/keditbookmarks.png"));
            star.ImageSource = bitmapImage;
            //11
            ImageBrush key = new ImageBrush();
            bitmapImage = new BitmapImage(new Uri("ms-appx:///Assets/Key.png"));
            key.ImageSource = bitmapImage;
            //12
            ImageBrush downArrow = new ImageBrush();
            bitmapImage = new BitmapImage(new Uri("ms-appx:///Assets/Nuvola_apps_download_manager.png"));
            downArrow.ImageSource = bitmapImage;
            //13
            ImageBrush pencil = new ImageBrush();
            bitmapImage = new BitmapImage(new Uri("ms-appx:///Assets/pencil-128.png"));
            pencil.ImageSource = bitmapImage;
            //14
            ImageBrush pill = new ImageBrush();
            bitmapImage = new BitmapImage(new Uri("ms-appx:///Assets/pills-5-icon.png"));
            pill.ImageSource = bitmapImage;
            //15
            ImageBrush puzzle = new ImageBrush();
            bitmapImage = new BitmapImage(new Uri("ms-appx:///Assets/puzzle.png"));
            puzzle.ImageSource = bitmapImage;
            //16
            ImageBrush record = new ImageBrush();
            bitmapImage = new BitmapImage(new Uri("ms-appx:///Assets/record.png"));
            record.ImageSource = bitmapImage;

            ImageBrush basketball = new ImageBrush();
            bitmapImage = new BitmapImage(new Uri("ms-appx:///Assets/Basketball-icon.png"));
            basketball.ImageSource = bitmapImage;

            ImageBrush firewall = new ImageBrush();
            bitmapImage = new BitmapImage(new Uri("ms-appx:///Assets/firewall.png"));
            firewall.ImageSource = bitmapImage;

            if (gameBoard.GridSize == 4)
            {
                pictureList.Add(new GamePicture(add, 1, 1));
                pictureList.Add(new GamePicture(bomb, 2, 1));
                pictureList.Add(new GamePicture(attention, 3, 1));
                pictureList.Add(new GamePicture(book, 4, 1));
                pictureList.Add(new GamePicture(clip, 5, 1));
                pictureList.Add(new GamePicture(furniture, 6, 1));
                pictureList.Add(new GamePicture(gem, 7, 1));
                pictureList.Add(new GamePicture(speakers, 8, 1));

            }
            else
            {
                pictureList.Add(new GamePicture(add, 1, 1));
                pictureList.Add(new GamePicture(bomb, 2, 1));
                pictureList.Add(new GamePicture(attention, 3, 1));
                pictureList.Add(new GamePicture(book, 4, 1));
                pictureList.Add(new GamePicture(clip, 5, 1));
                pictureList.Add(new GamePicture(furniture, 6, 1));
                pictureList.Add(new GamePicture(gem, 7, 1));
                pictureList.Add(new GamePicture(speakers, 8, 1));
                pictureList.Add(new GamePicture(home, 9, 1));
                pictureList.Add(new GamePicture(star, 10, 1));
                pictureList.Add(new GamePicture(key, 11, 1));
                pictureList.Add(new GamePicture(downArrow, 12, 1));
                pictureList.Add(new GamePicture(pencil, 13, 1));
                pictureList.Add(new GamePicture(pill, 14, 1));
                pictureList.Add(new GamePicture(puzzle, 15, 1));
                pictureList.Add(new GamePicture(record, 16, 1));
                pictureList.Add(new GamePicture(basketball, 17, 1));
                pictureList.Add(new GamePicture(firewall, 18, 1));
            }
        }
        private void boardCanvas_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //int numberOfCells = gameBoard.GridSize;
            //int rectSize = (int)boardCanvas.Width / numberOfCells;
            //Rectangle rect = sender as Rectangle;
            
            //Point mousePosition = e.GetPosition(boardCanvas);
            //int row = (int)(mousePosition.Y) / rectSize;
            //int col = (int)(mousePosition.X) / rectSize;
            //// var rowCol = (Point)rect.Tag;

            ////  Erno de Weerd  http://stackoverflow.com/questions/18914493/how-to-know-if-mouse-clicked-point-belongs-to-a-rectangle-or-not

           
            //foreach (var rectangle in rectangles)
            //{
               
            //}
            //int temp = row * col;
            //string position = rect.Name;
            //int temp1 = Convert.ToInt32(position);
            //rect.Fill = pictureList[temp1].image;
            //gameBoard.Flip(row, col);
            //// Redraw the board
            //DrawGrid();
            //GameCompleted();

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
            int i = 0;
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
                    rect.Name = (numbers[i].ToString());
                    
                    //rect.Fill = pictureList[numbers[i]].image;
                    i++;
                    // Store each row and col as a Point
                    rect.Tag = new Point(r, c);
                    //rect. += Rect_MouseLeftButtonDown;

                    int x = c * rectSize;
                    int y = r * rectSize;

                    Canvas.SetTop(rect, y);
                    Canvas.SetLeft(rect, x);
                    rectangles.Add(rect);
                    // Add the new rectangle to the canvas' children
                    boardCanvas.Children.Add(rect);
                    rect.Tapped += RectangleTapped;
                }
            }
        }
        void RectangleTapped (object sender, TappedRoutedEventArgs e)
        {
            Rectangle rect = sender as Rectangle;
            string position = rect.Name;
            int temp1 = Convert.ToInt32(position);
            rect.Fill = pictureList[temp1].image;

            if (flipCount > 1)
            {
                FlippedSecond = rect;

                if(!CheckFlipped(FlippedFirst, FlippedSecond))
                {
                    FlippedFirst.Fill = blue;
                    FlippedSecond.Fill = blue;
                }
                flipCount = 1;
            }
            else
            {
                FlippedFirst = rect;
                flipCount++;

            }

        }
        private bool CheckFlipped(Rectangle first, Rectangle second)
        {
            if (first.Name == second.Name)
            {
                return true;
            }
            else
            {
                return false;
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

        private void boardCanvas_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            //int numberOfCells = gameBoard.GridSize;
            //int rectSize = (int)boardCanvas.Width / numberOfCells;
            //Rectangle rect = sender as Rectangle;

            //Point mousePosition = e.GetPosition(boardCanvas);
            //int row = (int)(mousePosition.Y) / rectSize;
            //int col = (int)(mousePosition.X) / rectSize;
            //// var rowCol = (Point)rect.Tag;
            //int temp = row * col;
            //string position = rect.Name;
            //int temp1 = Convert.ToInt32(position);
            //rect.Fill = pictureList[temp1].image;
            //gameBoard.Flip(row, col);
            //// Redraw the board
            //DrawGrid();
            //GameCompleted();

        }
    }
}
