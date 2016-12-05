using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MemoryGame
{
    class GamePicture
    {
        public ImageBrush image;
        public int count;
        public int counter;  
        public GamePicture(ImageBrush image, int count, int counter)
        {
            // load up the pictures to a list
            this.image = image;
            this.count = count;
            this.counter = counter;
        }

        
    }
}
