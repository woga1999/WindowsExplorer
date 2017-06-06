using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowsExplorer
{
    class DirectoryExcept
    {//폴더 주소 있는지 확인
        //MainExplorer mainEx;
        //public DirectoryExcept(MainExplorer main)
        //{
        //    mainEx = main;
        //}
        public string judgeExistDirectory(string path)
        {
            string ofText = null;
            if (path != "" && !path.Contains(" "))
            {
                DirectoryInfo subInfo = new DirectoryInfo(path);
                if (subInfo.Exists)
                {
                    ofText = path;
                    return ofText;
                }
                else
                {
                    ofText = "Don't Exist";
                }
            }
            else
            {
                ofText = "Don't Exist";
            }
            return ofText;
        }
        //public int judgePathIndex(string path)
        //{

        //}
    }
}
