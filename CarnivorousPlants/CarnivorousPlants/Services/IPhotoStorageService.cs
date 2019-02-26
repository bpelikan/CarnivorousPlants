﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CarnivorousPlants.Services
{
    public interface IPhotoStorageService
    {
        Task<string> SavePhotoAsync(Stream CvStream, string fileName);
        //Task<bool> DeletePhotoAsync(string photoId);
        string UriFor(string photoId);
    }
}
