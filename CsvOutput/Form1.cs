using CsvOutput.Model;
using System;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows.Forms;

namespace CsvOutput
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (var db = new Model1())
            {
                try
                {
                    //SQL出力
                    db.Database.Log = (log) => System.Diagnostics.Debug.WriteLine(log);

                    DateTime fromDt = DateTime.Now.AddYears(-8);
                    DateTime toDt = DateTime.Now.AddYears(-7);
                    var query = from left in db.SalesPerson
                                join sub in db.SalesTerritory on left.TerritoryID equals sub.TerritoryID into subjoin
                                from right in subjoin.DefaultIfEmpty()
                                where left.ModifiedDate >= fromDt && left.ModifiedDate <= toDt
                                group new { left, right } by new { left.TerritoryID, right.Name } into grouped
                                orderby grouped.Key.TerritoryID
                                select new
                                {
                                    territoryID = grouped.Key.TerritoryID,
                                    name = grouped.Key.Name,
                                    count = grouped.Count()
                                };
                    this.bindingSource1.DataSource = query.ToList();
                    this.dataGridView1.DataSource = bindingSource1;
                    this.label1.Text = query.Count().ToString();

                    //CSV出力
                    try
                    {
                        // appendをtrueにすると，既存のファイルに追記。alseにすると，ファイルを新規作成する
                        var append = false;
                        using (var sw = new System.IO.StreamWriter(@"test.csv", append))
                        {
                            foreach (var x in query.ToList().Select((item, index) => new { item, index }))
                            {
                                if (x.index == 0)
                                {
                                    for (int i = 0; i < x.item.GetType().GetProperties().Length; i++)
                                    {
                                        sw.Write(x.item.GetType().GetProperties().ElementAt(i).Name + ",");
                                    }
                                    sw.WriteLine();
                                }
                                sw.WriteLine(x.item.territoryID + "," + x.item.name + "," + x.item.count );
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        // ファイルを開くのに失敗したときエラーメッセージを表示
                        System.Console.WriteLine(ex.Message);
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    // 例外処理
                    var errorMessage = "";
                    ex.EntityValidationErrors.SelectMany(error => error.ValidationErrors).ToList()
                        .ForEach(model => errorMessage = errorMessage + model.PropertyName + " - " + model.ErrorMessage);
                    MessageBox.Show(errorMessage);
                }
            }

        }
    }
}
