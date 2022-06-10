using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace JewelApp.Model
{
    public partial class Product
    {


        public BitmapImage FullPhotoPath
        {

            get
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;

                if (ProductPhoto is null)
                {
                    
                    return null;
                }


                image.UriSource = new Uri(Directory.GetCurrentDirectory() + "/product/" + ProductPhoto);
                try
                {
                    image.EndInit();
                }
                catch
                {

                    return null;
                }
               
                return image;

            }


        }

        public string ProductBackground
        {

            get
            {

                if(ProductQuantityInStock  == 0)
                {
                    return "#898989";
                }
                else
                {
                    return "White";
                }


            }


        }


    }
}
