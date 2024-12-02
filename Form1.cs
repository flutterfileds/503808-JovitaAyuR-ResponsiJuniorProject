using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace responsijunpro
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private NpgsqlConnection conn;
        string connstring = "Host=localhost; Port=5432; Username=postgres; Password=informatika; Database=responsijunpro";
        public DataTable dt;
        public static NpgsqlCommand cmd;
        private string sql = null;
        private DataGridViewRow r;

        public void Form1_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connstring);
        }

        public void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                dgvData.DataSource = null;
                sql = "select * from st_select";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                dgvData.DataSource = dt;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message, "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"select * from st_insert(_nama, _nama_dep)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_nama", tbNama.Text);
                cmd.Parameters.AddWithValue("_nama_dep");
                if((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Data berhasil diinputkan", "Well Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    btnLoad.PerformClick();
                    tbNama.Text = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message, "Insert Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                r = dgvData.Rows[e.RowIndex];
                tbNama.Text = r.Cells["_nama"].Value.ToString();
            }
        }

        public void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"select * from st_update(_nama, _nama_dep)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id_karyawan", r.Cells["_id_karyawan"].Value.ToString());
                cmd.Parameters.AddWithValue("_nama", tbNama.Text);
                cmd.Parameters.AddWithValue("_nama_dep");
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Data berhasil diupdate", "Well Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    btnLoad.PerformClick();
                    tbNama.Text = null;
                    r=null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message, "Update Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Mohon pilih baris yang akan didelete", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Information );
            }

            if (MessageBox.Show("Apakah bemar anda ingin menghapus data " + r.Cells["_nama"].Value.ToString() + " ?", "Hapus data terkonfirmasi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                try
                {
                    conn.Open();
                    sql = @"select * from st_delete(_id_karyawan)";
                    cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("_id_karyawan", r.Cells["_id_karyawan"].Value.ToString());
                    if ((int)cmd.ExecuteScalar() == 1)
                    {
                        MessageBox.Show("Data berhasil diupdate", "Well Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        conn.Close();
                        btnLoad.PerformClick();
                        tbNama.Text = null;
                        r = null;
                    }
                }
                catch (Exception ex )
                {
                    MessageBox.Show("Error" + ex.Message, "Delete Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } 
    }
}
