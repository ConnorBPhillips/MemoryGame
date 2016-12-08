using MemoryGame;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
        List<int> gridNumbers = new List<int>();
        private List<int> bestTimes = new List<int>();
        public int flipCount = 1;
        private int livesCount = GameBoard.ChosenDifficulty;
        private GameBoard gameBoard = new GameBoard();
        private Rectangle FlippedFirst;
        private Rectangle FlippedSecond;
        public int flippedCounter = 1;
        public int gameSize = GameBoard.MinGridSize;
        private string json;
        private string jsonTime;
        HighScores scores = new HighScores();
        //private GamePicture gamePictures = new GamePicture();
        private SolidColorBrush black = new SolidColorBrush(Windows.UI.Colors.Black);
        private SolidColorBrush blue = new SolidColorBrush(Windows.UI.Colors.SlateGray);
        public List<int> numbers = new List<int>();
        private int startTimer = 0;
        static Random rng = new Random();

        private bool completed = false;
        private bool loseGame;

        public GamePage()
        {
            this.InitializeComponent();

            bestTimes.Add(0);
            bestTimes.Add(0);
            bestTimes.Add(0);

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("jsonTime"))
            {
                jsonTime = ApplicationData.Current.LocalSettings.Values["jsonTime"] as string;
                bestTimes = JsonConvert.DeserializeObject<List<int>>(jsonTime);  
            }    
            string game = ApplicationData.Current.LocalSettings.Values["game"] as string;
            if (game != "new")
            {
                json = ApplicationData.Current.LocalSettings.Values["json"] as string;
                if (json == null || completed == true)
                {

                    LoadingPictures();
                    NumberList(gameSize);
                    CreateGrid();
                    CreateLivesGrid();
                    Timer();
                }
                else
                {
                    SavingData saveGame = JsonConvert.DeserializeObject<SavingData>(json);
                    flipCount = saveGame.flipCount;
                    flippedCounter = saveGame.flippedCounter;
                    numbers = saveGame.numbersForPictures;
                    gridNumbers = saveGame.picturesTurned;
                    startTimer = saveGame.timer;
                    gameSize = saveGame.gameSize;
                    //lives = saveGame.lives;
                    LoadingPictures();
                    ResumeGrid();
                    CreateLivesGrid();
                    Timer();
                }
            }
            else
            {
                json = null;
                LoadingPictures();
                NumberList(gameSize);
                CreateGrid();
                CreateLivesGrid();
                Timer();
            }
            PrintTimes();
        }

        private void ResumeGrid()
        {
            int numberOfCells = gameSize;
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

                    //rect.Fill = blue;
                    rect.Width = rectSize + 1;
                    rect.Height = rect.Width + 1;
                    rect.Stroke = black;
                    rect.Name = (numbers[i].ToString());

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

            foreach (var rectangle in rectangles)
            {
                rectangle.Fill = blue;
            }
            foreach (var rectangle in rectangles)
            {
                foreach (var numberList in gridNumbers)
                {
                    if (Convert.ToInt32(rectangle.Name) == numberList)
                    {
                        rectangle.Fill = pictureList[Convert.ToInt32(rectangle.Name)].image;
                    }
                }
            }
        }
        public List<int> NumberList(int size)
        {
            numbers.Clear();
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
        
        private async void GameCompleted()
        {
            if (loseGame)
            {
                flippedCounter = 1;
                completed = true;
                json = null;
                ApplicationData.Current.LocalSettings.Values["json"] = json;
                MessageDialog msgDialog = new MessageDialog("You have lost the game, sorry.");
                // Add an OK button
                msgDialog.Commands.Add(new UICommand("OK"));
                // Show the message box and wait aynchrously for a button press
                IUICommand command = await msgDialog.ShowAsync();
                reset();
                await Task.Delay(TimeSpan.FromSeconds(1.1));
                LoadingPictures();
                NumberList(gameSize);
                CreateGrid();
                CreateLivesGrid();
                Timer();
                loseGame = false;
            }
            if (gameBoard.IsGameOver(flippedCounter))
            {
                CheckTimes();
                completed = true;
                flippedCounter = 1;
                PrintTimes();
                MessageDialog msgDialog = new MessageDialog("Congratulations!  You've won! Your time is " + startTimer + " seconds");
                // Add an OK button
                msgDialog.Commands.Add(new UICommand("OK"));
                // Show the message box and wait aynchrously for a button press
                IUICommand command = await msgDialog.ShowAsync();
                json = null;
                ApplicationData.Current.LocalSettings.Values["json"] = json;
            }
        }
        private void CheckTimes()
        {
            if (bestTimes[0] > startTimer || bestTimes[0] == 0)
            {
                bestTimes[2] = bestTimes[1];
                bestTimes[1] = bestTimes[0];
                bestTimes[0] = startTimer;
            }
            else if (bestTimes[1] > startTimer || bestTimes[1] == 0)
            {
                bestTimes[2] = bestTimes[1];
                bestTimes[1] = startTimer;
            }
            else if (bestTimes[2] > startTimer || bestTimes[2] == 0)
            {
                bestTimes[2] = startTimer;
            }
            jsonTime = JsonConvert.SerializeObject(bestTimes);
            ApplicationData.Current.LocalSettings.Values["jsonTime"] = jsonTime;
        }
        private void CreateLivesGrid()
        {
            ImageBrush heart = new ImageBrush();
            BitmapImage bitmapHeartImage = new BitmapImage(new Uri("ms-appx:///Assets/Heart-icon.png"));
            heart.ImageSource = bitmapHeartImage;

            // Remove all previously-existing rectangles
            heartsCanvas.Children.Clear();

            int rectSize = (int)heartsCanvas.Width;

            // Turn entire grid on and create rectangles to represent it
                for (int c = 0; c < livesCount ; c++)
                {
                    //grid[r, c] = true;

                    Rectangle rect = new Rectangle();

                    //rect.Fill = blue;
                    rect.Width = rectSize;
                    rect.Height = rect.Width;

                    rect.Fill = heart;
                    // Store each row and col as a Point
                    rect.Tag = new Point(c, c);
                    //rect. += Rect_MouseLeftButtonDown;

                    int x = c;
                    int y = c * rectSize;

                    Canvas.SetTop(rect, y);
                    Canvas.SetLeft(rect, x);
                    // Add the new rectangle to the canvas' children
                    heartsCanvas.Children.Add(rect);
                }
        }
        private async void CreateGrid()
        {
            int numberOfCells = gameSize;
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

                    //rect.Fill = blue;
                    rect.Width = rectSize + 1;
                    rect.Height = rect.Width + 1;
                    rect.Stroke = black;
                    rect.Name = (numbers[i].ToString());

                    rect.Fill = pictureList[numbers[i]].image;
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
            await Task.Delay(TimeSpan.FromSeconds(1));
            foreach (var rectangle in rectangles)
            {
                rectangle.Fill = blue;
            }
        }
        async void Timer()
        {
           if (completed)
            {
                completed = false;
            }
            do
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                startTimer++;
                timer.Text = startTimer.ToString();
                timer.UpdateLayout();
            } while (!completed);
            startTimer = 0;    
                    
        }
        async void RectangleTapped(object sender, TappedRoutedEventArgs e)
        {
            
            Rectangle rect = sender as Rectangle;
            Rectangle life = new Rectangle();
            string position = rect.Name;
            int temp1 = Convert.ToInt32(position);
            if (rect.Fill == blue && flipCount <3)
            {
                rect.Fill = pictureList[temp1].image;

                if (flipCount > 1)
                {
                    FlippedSecond = rect;

                    if (!CheckFlipped(FlippedFirst, FlippedSecond))
                    {
                        
                        //Thread.Sleep(milliseconds);
                        await Task.Delay(TimeSpan.FromSeconds(.35));
                        FlippedFirst.Fill = blue;
                        FlippedSecond.Fill = blue;
                        livesCount -= 1;
                        CreateLivesGrid();
                        if (livesCount == 0)
                        {
                            loseGame = true;
                            GameCompleted();
                        }
                    }
                    else
                    {
                        gridNumbers.Add(Convert.ToInt32(FlippedFirst.Name));
                        flippedCounter++;
                    }
                    flipCount = 1;
                    SaveGameStatus();
                }
                else
                {
                    FlippedFirst = rect;
                    flipCount++;

                }
               
            }
            GameCompleted();
            

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
            reset();
            DrawGrid();
        }
        private void reset()
        {
            livesCount = GameBoard.ChosenDifficulty;
            flippedCounter = 1;
            completed = true;
            gridNumbers.Clear();
            
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



        private async void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            reset();
            await Task.Delay(TimeSpan.FromSeconds(1.1));
            LoadingPictures();
            NumberList(gameSize);
            CreateGrid();
            Timer();
        }
        private void SaveGameStatus()
        {
            SavingData save = new SavingData();
            //save.lives = ;
            save.numbersForPictures = numbers;
            save.picturesTurned = gridNumbers;
            save.timer = startTimer;
            save.gameSize = gameSize;
            save.flippedCounter = flippedCounter;
            save.flipCount = flipCount;
            json = JsonConvert.SerializeObject(save);
            ApplicationData.Current.LocalSettings.Values["json"] = json;
        }

        private void PrintTimes()
        {
            bestTimesText.Text = "Best Times!" +
                System.Environment.NewLine + bestTimes[0] +
                System.Environment.NewLine + bestTimes[1] +
                System.Environment.NewLine + bestTimes[2];
        }
    }
    
}
