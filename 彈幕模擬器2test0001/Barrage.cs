using System;
namespace Barrage1
{
    class Barrage
    {
        float enemyX0 = 0;//原始X
        float enemyY0 = 0;//原始Y
        float enemydistance = 0;
        float enemycoordinate = 0;
        int coordinate = 0;
        int width = 0, height = 0;
        int tra2 = 0, tra4 = 0, tra5 = 0;
        /// <summary>
        /// 建構函式
        /// </summary>
       public  Barrage(int tra02, int tra04, int tra05, int coordinate1, int w, int h)
        {
            tra2 = tra02;
            tra4 = tra04;
            tra5 = tra05;
            coordinate = coordinate1;
            width = w;
            height = h;
            enemyX0 = tra4;//原始X
            enemyY0 = tra5;//原始Y
            enemydistance = 0;
            enemycoordinate = coordinate;
        }
        private void GameTest()
        {
            enemydistance += tra2;
            double x = 0, y = enemydistance;
            double x2 = (x * Math.Cos(enemycoordinate) - y * Math.Sin(enemycoordinate));
            double y2 = (x * Math.Sin(enemycoordinate) + y * Math.Cos(enemycoordinate));
            x2 = enemyX0 - x2;
            y2 = enemyY0 - y2;
        }
        public int Game()
        {
            enemydistance += tra2;
            double x = 0, y = enemydistance;
            double x2 = (x * Math.Cos(enemycoordinate) - y * Math.Sin(enemycoordinate));
            double y2 = (x * Math.Sin(enemycoordinate) + y * Math.Cos(enemycoordinate));
            x2 = enemyX0 - x2;
            y2 = enemyY0 - y2;
            if (x2 > 0 && x2 < width && y2 > 0 && y2 < height)
                return (int)y2 * width + (int)x2;
            else
                return -100;
        }
    }
}
