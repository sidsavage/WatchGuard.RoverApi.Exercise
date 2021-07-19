using System;
using System.Collections.Generic;
using System.Text;

namespace WatchGuard.RoverApi.Exercise.Models
{
    public class MarsRoverPhotoData
    {
        public int Id { get; set; }

        public int Sol { get; set; }

        public Camera Camera { get; set; }

        public string Img_Src { get; set; }

        public string Earth_Date { get; set; }

        public Rover Rover { get; set; }
    }

    public class PhotoList
    {
        public List<MarsRoverPhotoData> Photos { get; set; }
    }
}
