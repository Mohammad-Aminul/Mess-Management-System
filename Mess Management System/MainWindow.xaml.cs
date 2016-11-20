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

        string connectionString = "Data source=database.db;version=3";
        public MainWindow()
        {
            InitializeComponent();
            Date_picker.SelectedDate = DateTime.Now;
            cost_Date_picker.SelectedDate = DateTime.Now;
            fildatagrid();
            fillMemberComboBox();
            fillDepositDatagrid();
            datepickerForTotalDeposit.SelectedDate = DateTime.Now;
            fillCostdataGrid();
            fillDailyTotalCostDataGrid();
            fun_totalCostofMonth();
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
                    cmb_spentBy.Items.Add(data.GetString(0));
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
            catch (Exception)
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

        //***************************************************************************
        //             deposit section
        //***************************************************************************

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

                txt_depositemoney.Clear();
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// this method will insert deposit money of the member
        /// </summary>
        public string deposit_date;
        private void btn_deposit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txt_depositemoney.Text) && !string.IsNullOrWhiteSpace(cmb_depositeMember.Text))
                {
                    SQLiteConnection conn = new SQLiteConnection(connectionString);
                    conn.Open();
                    string query = "INSERT INTO tbl_deposit(deposit_money,deposit_date,member_ID) values('" + this.txt_depositemoney.Text + "','" + deposit_date + "','" + deposite_ID + "')";
                    SQLiteCommand command = new SQLiteCommand(query, conn);
                    command.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Deposit successfull", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                    fillDepositDatagrid();
                    fillDataGridMemberMonthlyTotal();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Date_picker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime ida = Convert.ToDateTime(Date_picker.SelectedDate);
            deposit_date = ida.ToString("dd-MM-yyyy");
        }
        private void clearDepositTextBox()
        {
            lbl_mobile.Content = "";
            lbl_room.Content = "";
            txt_depositemoney.Clear();
            cmb_depositeMember.SelectedValue = "";
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
            catch (Exception)
            {

            }
        }


        string deposit_MonthYear;

        private void datepickerForTotalDeposit_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime deposit_date = Convert.ToDateTime(datepickerForTotalDeposit.SelectedDate);
            deposit_MonthYear = deposit_date.ToString("-MM-yyyy");
            //MessageBox.Show(deposit_dateYear);
            fillDataGridMemberMonthlyTotal();

        }

        /// <summary>
        /// this method will show each member's total deposit in a month in the dataGrid
        /// </summary>
        private void fillDataGridMemberMonthlyTotal()
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection(connectionString);
                conn.Open();
                string query = "select a.member_name , sum(deposit_money) 'Total Deposit Money' from tbl_member a left join tbl_deposit b on deposit_date like  '" + "__" + deposit_MonthYear + "' and a.member_ID=b.member_ID group by member_name";
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

        /// <summary>
        /// this will update the deposit money of the member
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_updateDeposite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txt_depositemoney.Text) && deposit_ID != 0)
                {
                    SQLiteConnection conn = new SQLiteConnection(connectionString);
                    conn.Open();
                    string query = "update tbl_deposit set deposit_money='" + this.txt_depositemoney.Text + "',deposit_date='" + this.deposit_date + "' where deposit_ID='" + deposit_ID + "'";
                    SQLiteCommand command = new SQLiteCommand(query, conn);
                    command.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Update successfully", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearDepositTextBox();
                    fillDepositDatagrid();
                    fillDataGridMemberMonthlyTotal();
                    deposit_ID = 0;
                }
                else
                    MessageBox.Show("For update select a row from the Deposit table", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// to update and delete deposit information this method will give the value of selected row
        /// </summary>
        int deposit_ID = 0, depositMemberID = 0;
        private void dg_deposit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = dg_deposit.SelectedItem;
                deposit_ID = Convert.ToInt16((dg_deposit.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                depositMemberID = Convert.ToInt16((dg_deposit.SelectedCells[3].Column.GetCellContent(item) as TextBlock).Text);
                txt_depositemoney.Text = (dg_deposit.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;

                SQLiteConnection conn = new SQLiteConnection(connectionString);
                conn.Open();
                string query = "select member_name,mobile_no,room_no from tbl_member where member_ID='" + this.depositMemberID + "'";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteDataReader data = command.ExecuteReader();
                if (data.Read())
                {
                    cmb_depositeMember.SelectedValue = data.GetString(0);
                    lbl_mobile.Content = data.GetString(1);
                    lbl_room.Content = data.GetString(2);
                }

                conn.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Error");
            }
        }

        /// <summary>
        /// this method will delete the selected row from the datagrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_deleteDeposite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (deposit_ID != 0)
                {
                    if (MessageBox.Show("Are you sure?", "Warning", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        SQLiteConnection conn = new SQLiteConnection(connectionString);
                        conn.Open();
                        string query = "Delete from tbl_deposit where deposit_ID='" + deposit_ID + "'";
                        SQLiteCommand command = new SQLiteCommand(query, conn);
                        command.ExecuteNonQuery();
                        conn.Close();
                        clearDepositTextBox();
                        fillDepositDatagrid();
                        fillDataGridMemberMonthlyTotal();
                        deposit_ID = 0;
                    }
                }
                else
                    MessageBox.Show("To delete select a row from the Deposit table", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// this will clear the value of the deposit form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_clearDeposite_Click(object sender, RoutedEventArgs e)
        {
            clearDepositTextBox();
        }


        //*********************************************************************
        //                     Cost section
        //*********************************************************************
        private void btn_costSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(cmb_spentBy.Text) && !string.IsNullOrWhiteSpace(txt_costAmount.Text))
                {
                    SQLiteConnection conn = new SQLiteConnection(connectionString);
                    conn.Open();
                    string query = "INSERT INTO tbl_cost(cost_amount,cost_date,member_ID) values('" + this.txt_costAmount.Text + "','" + cost_date + "','" + costby_ID + "')";
                    SQLiteCommand command = new SQLiteCommand(query, conn);
                    command.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("cost save successfull", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                    fillCostdataGrid();
                    fillDailyTotalCostDataGrid();
                }
                else
                    MessageBox.Show("Please provide necessary information", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void fillCostdataGrid()
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection(connectionString);
                conn.Open();
                string query = "select cost_date 'Date of Cost', sum(cost_amount) 'Daily Cost By a Member',b.member_name 'Name of Member' from tbl_cost a, tbl_member b where a.member_ID=b.member_ID group by cost_date,member_name";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                DataTable data = new DataTable("tbl_cost");
                adapter.Fill(data);
                dg_showCost.ItemsSource = data.DefaultView;
                conn.Close();
            }
            catch (Exception)
            {

            }
        }

        private void fillDailyTotalCostDataGrid()
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection(connectionString);
                conn.Open();
                string query = "select cost_date 'Date of Cost', sum(cost_amount) 'Daily Total Cost' from tbl_cost group by cost_date";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                DataTable data = new DataTable();
                adapter.Fill(data);
                dg_dailyTotalCost.ItemsSource = data.DefaultView;
                conn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        string cost_date;

        private void cost_Date_picker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime c_date = Convert.ToDateTime(cost_Date_picker.SelectedDate);
            cost_date = c_date.ToString("dd-MM-yyyy");
        }


        int costby_ID = 0;
        private void cmb_spentBy_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection(connectionString);
                conn.Open();
                string query = "select member_ID from tbl_member where member_name='" + this.cmb_spentBy.Text + "'";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteDataReader data = command.ExecuteReader();
                if (data.Read())
                {
                    costby_ID = data.GetInt16(0);
                }

                conn.Close();

                txt_costAmount.Clear();
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        double val_totalCostofMonth = 0;
        private void fun_totalCostofMonth()
        {
            try
            {
                SQLiteConnection conn = new SQLiteConnection(connectionString);
                conn.Open();
                string query = "select sum(cost_amount) from tbl_cost where cost_date  like '__-11-2016'";
                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteDataReader data = command.ExecuteReader();
                if (data.Read())
                {
                    lbl_totalCost.Content = data.GetDouble(0);
                    val_totalCostofMonth = data.GetDouble(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fun_mealRateCalculation()
        {

        }


    }
}
