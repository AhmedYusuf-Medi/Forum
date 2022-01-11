﻿//Local
using Forum.Service.Contracts;
//Nuget packets
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.IO;
//Public
using System.Threading.Tasks;

namespace Forum.Service
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public async Task<string[]> UploadPictureAsync(IFormFile pictureFile, string fileName, string username)
        {
            byte[] destinationData;

            using (var ms = new MemoryStream())
            {
                await pictureFile.CopyToAsync(ms);
                destinationData = ms.ToArray();
            }

            UploadResult uploadResult = null;

            using (var ms = new MemoryStream(destinationData))
            {
                ImageUploadParams uploadParams = new ImageUploadParams
                {
                    Folder = username,
                    File = new FileDescription(fileName, ms),
                };

                uploadResult = await this.cloudinary.UploadAsync(uploadParams);
            }

            string[] uploadResults = new string[2];
            uploadResults[0] = uploadResult?.SecureUrl.AbsoluteUri;
            uploadResults[1]= uploadResult?.PublicId;

            return uploadResults;
        }

        public async Task DeleteImageAsync(string fileName)
        {
            await this.cloudinary.DeleteResourcesAsync(fileName);
        }
    }
}