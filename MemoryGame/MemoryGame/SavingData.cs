using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGame
{
    class SavingData
    {
        // This should help with saving state http://stackoverflow.com/questions/33803902/asp-net-saving-in-viewstate-without-serializing
        public int lives;
        public List<int> numbersForPictures;
        public List<int> picturesTurned;
        public int timer;
        public int gamesize;
        public int flippedCounter;
        public int flipCount;
        public SavingData()
        {

        }
    }
}
