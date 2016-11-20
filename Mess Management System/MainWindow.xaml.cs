using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Data.SQLite;
using System.Data;
namespace Mess_Management_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string current_date;
        string connectionString = "Data source=database.db;version=3";
        public MainWindow()
        {
            InitializeComponent();
            Date_picker.SelectedDate = DateTime.Now;
            Date_picker2.SelectedDate = DateTime.Now;
            fildatagrid();
            fillMemberComboBox();
            fillDepositDatagrid();
            datepickerForTotalDeposit.SelectedDate = DateTime.Now;
        }
        /// <summary>
        /// this method will fill the member comboBox of deposit form
        /// </summary>
        private void fillMemberComboBox()
        {
            try
            {
                cmb_depositeMember.Items.Clear();
                SQLiteConnection conn = new SQLiteConnection(connectionString);
                conn.Open();
                string query = "select member_name from tbl_member";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteDataReader data = command.ExecuteReader();
                while (data.Read())
                {
                    cmb_depositeMember.Items.Add(data.GetString(0));
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// this method will return all the member into the datagrid
        /// </summary>
        private void fildatagrid()
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection(connectionString);
                conn.Open();
                string query = "select * from tbl_member";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                DataTable data = new DataTable("tbl_member");
                adapter.Fill(data);
                dg_member.ItemsSource = data.DefaultView;
                conn.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void tb_control_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btn_addMember_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txt_name.Text) && !string.IsNullOrWhiteSpace(txt_name.Text) && !string.IsNullOrWhiteSpace(txt_name.Text))
                {
                    SQLiteConnection conn = new SQLiteConnection(connectionString);
                    conn.Open();
                    string query = "INSERT INTO tbl_member(member_name,mobile_no,room_no) values('" + this.txt_name.Text + "','" + this.txt_mobile.Text + "','" + this.txt_room.Text + "')";
                    SQLiteCommand command = new SQLiteCommand(query, conn);
                    command.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Member added successfully", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                    fildatagrid();
                    fillMemberComboBox();
                    clearMemberAddTextBox();
                }
                else
                    MessageBox.Show("Please provide necessary information", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// it will clear the textBox of new member add form
        /// </summary>
        private void clearMemberAddTextBox()
        {
            txt_name.Clear();
            txt_room.Clear();
            txt_mobile.Clear();
        }

        int search_id = 0;
        private void dg_member_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = dg_member.SelectedItem;
                search_id = Convert.ToInt16((dg_member.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                txt_name.Text = (dg_member.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
                txt_mobile.Text = (dg_member.SelectedCells[2].Column.GetCellContent(item) as TextBlock).Text;
                txt_room.Text = (dg_member.SelectedCells[3].Column.GetCellContent(item) as TextBlock).Text;
            }
            catch (FormatException)
            {
                MessageBox.Show("Error");
            }
        }

        private void btn_updateMember_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txt_name.Text) && !string.IsNullOrWhiteSpace(txt_name.Text) && !string.IsNullOrWhiteSpace(txt_name.Text))
                {
                    SQLiteConnection conn = new SQLiteConnection(connectionString);
                    conn.Open();
                    string query = "update tbl_member set member_name='" + this.txt_name.Text + "',mobile_no='" + this.txt_mobile.Text + "',room_no='" + this.txt_room.Text + "' where member_ID='" + search_id + "'";
                    SQLiteCommand command = new SQLiteCommand(query, conn);
                    command.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Member Update successfully", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                    fildatagrid();
                    fillMemberComboBox();
                    clearMemberAddTextBox();
                }
                else
                    MessageBox.Show("Please provide necessary information", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_deleteMember_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txt_name.Text) && !string.IsNullOrWhiteSpace(txt_name.Text) && !string.IsNullOrWhiteSpace(txt_name.Text))
                {
                    SQLiteConnection conn = new SQLiteConnection(connectionString);
                    conn.Open();
                    string query = "delete from tbl_member where member_ID='" + search_id + "'";
                    SQLiteCommand command = new SQLiteCommand(query, conn);
                    command.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Member Deleted successfully", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                    fildatagrid();
                    fillMemberComboBox();
                    clearMemberAddTextBox();
                }
                else
                    MessageBox.Show("Please provide necessary information", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        int deposite_ID = 0;
        private void cmb_depositeMember_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection(connectionString);
                conn.Open();
                string query = "select member_ID,mobile_no,room_no from tbl_member where member_name='" + this.cmb_depositeMember.Text + "'";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteDataReader data = command.ExecuteReader();
                if (data.Read())
                {
                    deposite_ID = data.GetInt16(0);
                    lbl_mobile.Content = data.GetString(1);
                    lbl_room.Content = data.GetString(2);
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void btn_deposit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txt_depositemoney.Text) && !string.IsNullOrWhiteSpace(cmb_depositeMember.Text))
                {
                    SQLiteConnection conn = new SQLiteConnection(connectionString);
                    conn.Open();
                    string query = "INSERT INTO tbl_deposit(deposit_money,deposit_date,member_ID) values('" + this.txt_depositemoney.Text + "','" + current_date + "','" + deposite_ID + "')";
                    SQLiteCommand command = new SQLiteCommand(query, conn);
                    command.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("successfully", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                    fillDepositDatagrid();
                    clearDepositTextBox();
                }
                else
                    MessageBox.Show("Please provide necessary information", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void clearDepositTextBox()
        {
            lbl_mobile.Content = "";
            lbl_room.Content = "";
            txt_depositemoney.Clear();
        }

        /// <summary>
        /// this method will show the all deposit money of the member in the dataGrid
        /// </summary>
        private void fillDepositDatagrid()
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection(connectionString);
                conn.Open();
                string query = "select * from  tbl_deposit";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                DataTable data = new DataTable("tbl_deposit");
                adapter.Fill(data);
                dg_deposit.ItemsSource = data.DefaultView;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Date_picker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime ida = Convert.ToDateTime(Date_picker.SelectedDate);
            current_date = ida.ToString("dd-MM-yyyy");
        }
        string deposit_dateYear;

        private void datepickerForTotalDeposit_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime deposit_date = Convert.ToDateTime(datepickerForTotalDeposit.SelectedDate);
            deposit_dateYear = deposit_date.ToString("-MM-yyyy");
            //MessageBox.Show(deposit_dateYear);
            fillDataGridMemberMonthlyTotal();

        }

        private void fillDataGridMemberMonthlyTotal()
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection(connectionString);
                conn.Open();
                string query = "select a.member_name , sum(deposit_money) 'Total Deposit Money' from tbl_member a left join tbl_deposit b on deposit_date like  '" + "__" + deposit_dateYear + "' and a.member_ID=b.member_ID group by member_name";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                DataTable data = new DataTable();
                adapter.Fill(data);
                dg_individualTotalInMonth.ItemsSource = data.DefaultView;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}
