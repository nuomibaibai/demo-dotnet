using Aspose.Cells;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo_Aspose
{
    class Program
    {
        static void Main(string[] args)
        {
            string filepath = $@"E:\Desktop\20220804174004.xlsx";

            //@"E:\Desktop\Account Detail by CostCenter_20210906144014773_2021_20210906144156.xlsx";
            //@"E:\Desktop\企业微信文件\WXWork\1688850695525719\Cache\File\2021-09\2020.11 sales report RDU&ALS.xls";
            try
            {
                ArrayList ColTitle = new ArrayList()
  { "ASN编号", "SKU", "产品描述", "预期数量", "收货数量",
  "单位","收货库位","收货时间","所属客户","ASN状态","ASN创建时间" };
                string[] strTitle = new string[] { "ASNNo", "SKU", "SKUDescrC", "ExpectedQty", "ReceivedQty", "UOM",
                "ReceivingLocation", "ReceivedTime", "CustomerID", "CodeName_C" };
                //if (getAsnData.ToList().Count > 0)
                {
                    Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook(filepath);
                    //创建一个sheet
                    Aspose.Cells.Worksheet sheet = workbook.Worksheets[0];
                    //为单元格添加样式
                    Aspose.Cells.Style style = workbook.CreateStyle();
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    style.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin; //应用边界线 左边界线
                    style.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin; //应用边界线 右边界线
                    style.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin; //应用边界线 上边界线
                    style.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin; //应用边界线 下边界线
                                                                                                                      //给各列的标题行PutValue赋值
                    int currow = 0;
                    byte curcol = 0;
                    //sheet.Cells.ImportCustomObjects((System.Collections.ICollection)getAsnData,
                    //strTitle, true, 0, 0, getAsnData.Count, true, "yyyy/MM/dd HH:mm", false);
                    //sheet.Cells.ImportCustomObjects((System.Collections.ICollection)getAsnData,
                    //null, true, 0, 0, getAsnData.Count, true, "yyyy/MM/dd HH:mm", false);
                    //// 设置内容样式
                    //for (int i = 0; i < getAsnData.ToList().Count; i++)
                    //{
                    //    for (int j = 0; j < 11; j++)
                    //    {
                    //        sheet.Cells[i + 1, j].Style = style;
                    //        sheet.Cells[i + 1, 2].Style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Left;
                    //        sheet.Cells[i + 1, 7].Style.Custom = "yyyy/MM/dd HH:mm";
                    //        sheet.Cells[i + 1, 10].Style.Custom = "yyyy/MM/dd HH:mm";
                    //    }
                    //}
                    // 设置标题样式及背景色
                    foreach (string s in ColTitle)
                    {
                        sheet.Cells[currow, curcol].PutValue(s);
                        style.ForegroundColor = System.Drawing.Color.FromArgb(225, 0, 15);
                        style.Pattern = Aspose.Cells.BackgroundType.Solid;
                        style.Font.IsBold = true;
                        sheet.Cells[currow, curcol].SetStyle(style);
                        curcol++;
                    }
                    Aspose.Cells.Cells cells = sheet.Cells;
                    //设置标题行高
                    cells.SetRowHeight(0, 30);
                    //让各列自适应宽度
                    sheet.AutoFitColumns();

                    workbook.Save(filepath);
                    Console.WriteLine("111");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
