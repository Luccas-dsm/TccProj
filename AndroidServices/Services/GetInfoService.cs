using Android.App;
using Android.Content;

namespace TccProj.Droid.Services
{
    public class GetInfoService
    {

        //public  string GetMemRam()
        //{

        // //   return GetRam();
        //}

        public string GetRam(Context context)
        {
          
            ActivityManager.MemoryInfo memoryInfo = new ActivityManager.MemoryInfo();
            ActivityManager activityManager = (ActivityManager)context.GetSystemService(Context.ActivityService);
            activityManager.GetMemoryInfo(memoryInfo);
            return memoryInfo.TotalMem.ToString();
        }
    }
}