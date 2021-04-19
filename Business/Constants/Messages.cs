using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
   public static class Messages
   {
       public static string ProductAdded = "Ürün Eklendi";
       public static string ProductDeleted = "Ürün Silindi";
       public static string ProductUpdated = "Ürün Güncellendi";
        internal static User UserNotFound;

        public static string AccesTokenCreated = " Access Token olusturuldu";

        public static string UserRegistered = "kullanıcı eklendi";
        public static string WrongPassword = "girilen şifre hatalı";
   }
}
