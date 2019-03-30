using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dodo
{
    class EarthCacl
    {
        public static double getDiss(int localx,int localy,int tlocalx,int tlocaly)
        {
            double diss = Math.Sqrt( Math.Pow((Math.Abs(localx - tlocalx)),2) + Math.Pow((Math.Abs(localy - tlocaly)), 2));
            return diss;
        }   
        public static void calcAllEarthDiss(ref List<EarthInfo> listData)
        {            
            foreach(var eti in listData)
            {
                eti.dictOtherDiss.Clear();
                foreach(var calcEti in listData)
                {
                    if (eti == calcEti || eti.dictOtherDiss.ContainsKey(calcEti.name) == true)
                    {
                        continue;
                    }
                    double diss = EarthCacl.getDiss(eti.localx, eti.localy, calcEti.localx, calcEti.localy);
                    eti.dictOtherDiss[calcEti.name] = diss;
                    calcEti.dictOtherDiss[eti.name] = diss;
                }
            }
        }
        /// <summary>
        /// 获取圆点对象，通过名字
        /// </summary>
        /// <param name="name"></param>
        /// <param name="listData"></param>
        /// <returns></returns>
        public static EarthInfo getEarthForName(string name, List<EarthInfo> listData)
        {
            foreach (var eti in listData)
            {
                if(name == eti.name)
                {
                    return eti;
                }
            }
            return null;
        }
        /// <summary>
        /// 融合两个圆点
        /// </summary>
        /// <param name="listData"></param>
        public static void combineEarth(ref List<EarthInfo> listData)
        {
            EarthInfo[] buffTempEti= new EarthInfo[listData.Count];
            int index = 0;
            foreach (var eti in listData)
            {
                try
                {
                    if (buffTempEti.Contains(eti) == true)
                        continue;
                    foreach (KeyValuePair<string, double> kvp in eti.dictOtherDiss)
                    {
                        EarthInfo tempEti = EarthCacl.getEarthForName(kvp.Key, listData);
                        // if tempEti will move
                        if (buffTempEti.Contains(tempEti) == true)
                            continue;
                        if (tempEti.roil + eti.roil >= (int)kvp.Value)
                        {
                            // 发生融合
                            eti.localx = (int)((eti.localx + tempEti.localx) / 2);
                            eti.localy = (int)((eti.localy + tempEti.localy) / 2);
                            eti.calcEarthNewRoil(tempEti.roil);
                            eti.oldName.Add(tempEti.name);
                            //移除这个位
                            buffTempEti[index++] = tempEti; 
                                                                                   
                        }
                    }//foreach 
                }//try
                catch (Exception)
                {

                    System.Console.Write("name:" + eti.name + " 不存在");
                }                
            }// foreach
            foreach(var tt in buffTempEti)
            {
                listData.Remove(tt);
            }
        }

        public static void calcEarthExec(ref List<EarthInfo> listData)
        {
            foreach (EarthInfo eti in listData)
            {
                try
                {
                    double diffX = Math.Cos(eti.direct) * eti.speed;
                    double diffY = Math.Sin(eti.direct) * eti.speed;
                    eti.localx += (int)diffX;
                    eti.localy += (int)diffY;
                }//try
                catch (Exception)
                {
                    System.Console.Write("角度计算错误");
                }
            }// foreach
        }
    }

}
