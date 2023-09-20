using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DB_16_September
{
    public partial class MainWindow : Window
    {
        Material current = null;

        public MainWindow()
        {
            InitializeComponent();
            
            DB.open_connection();
            
            Material.MaterialTypeLoad();
            
            upd_materials();

            btn_delete.Click += Btn_delete_Click;
            btn_insert.Click += Btn_insert_Click;
            btn_update.Click += Btn_update_Click;
            lv_table.SelectionChanged += Lv_table_SelectionChanged;
        }

        //reload materials from database
        private void upd_materials()
        {
            lv_table.Items.Clear();

            DataTable dt = DB.query("SELECT * FROM Material_and_type ORDER BY ID");
            foreach (DataRow dr in dt.Rows)
            {
                 Material m = new Material(dr);
                 lv_table.Items.Add(m);

            }

            cb_material_type.ItemsSource = Material.types.Values;
        }

        //Confirm window
        public bool ConfirmAction(string message)
        {
            MessageBoxResult result = MessageBox.Show(message, "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        //Update interface
        private void Lv_table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            current = lv_table.SelectedItem as Material;

            if (current == null) return;

            tb_title.Text = current.title ;
            tb_description.Text = current.description;
            tb_cost.Text = current.cost;
            tb_countinstock.Text = current.count_in_stock;
            tb_countinpack.Text = current.count_in_pack;
            tb_mincount.Text = current.min_count;
            tb_image.Text = current.image;
            tb_unit.Text = current.unit;

            cb_material_type.Text = current.material_type.name;
        }

        private void Btn_update_Click(object sender, RoutedEventArgs e)
        {
            Material newm = lv_table.SelectedItem as Material;

            if (newm == null) { MessageBox.Show("Ошибка: не выбран материал"); return; }
            if (!ConfirmAction("Вы действительно хотите обновить материал, названный ранее " + newm.title + "?"))
            {
                return;
            }

            //Collect info from interface about material
            newm.title = tb_title.Text;
            newm.description = tb_description.Text;
            newm.cost = tb_cost.Text;
            newm.count_in_stock = tb_countinstock.Text;
            newm.count_in_pack = tb_countinpack.Text;
            newm.min_count = tb_mincount.Text;
            newm.image = tb_image.Text;
            newm.unit = tb_unit.Text;
            newm.material_type = cb_material_type.SelectedItem as MaterialType;

            newm.update_in_db();

            upd_materials();
        }

        private void Btn_insert_Click(object sender, RoutedEventArgs e)
        {
            Material newm = new Material();

            //Collect info from interface about material
            newm.title = tb_title.Text;
            newm.description = tb_description.Text;
            newm.cost = tb_cost.Text;
            newm.count_in_stock = tb_countinstock.Text;
            newm.count_in_pack = tb_countinpack.Text;
            newm.min_count = tb_mincount.Text;
            newm.image = tb_image.Text;
            newm.unit = tb_unit.Text;
            newm.material_type = cb_material_type.SelectedItem as MaterialType;

            newm.insert_as_new(); //Saving to database
            upd_materials();
        }

        private void Btn_delete_Click(object sender, RoutedEventArgs e)
        {
            if (current == null)
            {
                MessageBox.Show("Не выбран материал");
                return;
            }

            current.delete_from_db();
            upd_materials();
        }
    }
}
