using Models;
using Controllers;

namespace Views{
    public enum Option { Edit, Delete }

    public class Produto : Form
    {
        ListView list;

        public Produto()
        {
            this.Text = "Produto";
            this.Size = new Size(720, 370);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;

            list = new ListView();
            list.Size = new Size(605, 180);
            list.Location = new Point(50, 50);
            list.View = View.Details;
            list.Columns.Add("Id");
            list.Columns.Add("Brand");
            list.Columns.Add("Model");
            list.Columns.Add("Year");
            list.Columns.Add("Color");
            list.Columns.Add("License Plate");
            list.Columns.Add("Type");
            list.Columns.Add("Price");
            list.Columns[0].Width = 30;
            list.Columns[1].Width = 80;
            list.Columns[2].Width = 100;
            list.Columns[3].Width = 50;
            list.Columns[4].Width = 80;
            list.Columns[5].Width = 100;
            list.Columns[6].Width = 70;
            list.Columns[7].Width = 70;
            list.FullRowSelect = true; // permite selecionar a linha inteira ao clicar
            this.Controls.Add(list);

            RefreshList(); 

            Button btnAdd = new Button();
            btnAdd.Text = "Adicionar";
            btnAdd.Size = new Size(100, 30);
            btnAdd.Location = new Point(50, 270);
            btnAdd.Click += new EventHandler(btnAdd_Click);
            this.Controls.Add(btnAdd);

            Button btnEdit = new Button();
            btnEdit.Text = "Editar";
            btnEdit.Size = new Size(100, 30);
            btnEdit.Location = new Point(170, 270);
            btnEdit.Click += new EventHandler(btnUpdate_Click);
            this.Controls.Add(btnEdit);

            Button btnDelete = new Button();
            btnDelete.Text = "Delete";
            btnDelete.Size = new Size(100, 30);
            btnDelete.Location = new Point(290, 270);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            this.Controls.Add(btnDelete);
            
            Button btnClose = new Button();
            btnClose.Text = "Fechar";
            btnClose.Size = new Size(100, 30);
            btnClose.Location = new Point(550, 270);
            btnClose.Click += new EventHandler(btnClose_Click);
            this.Controls.Add(btnClose);
        }
        private void AddToListView(Models.Car car)
        {
            string[] row = { car.Id.ToString(), car.Brand, car.Model, car.Year.ToString(), car.Color, car.LicensePlate, car.Type, car.Price.ToString() };
            ListViewItem item = new ListViewItem(row);
            list.Items.Add(item);
        }

        public void RefreshList()
        {
            list.Items.Clear();

            List<Models.Car> cars = Controllers.CarController.Read();


            foreach (Models.Car car in cars)
            {
                AddToListView(car);
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            var registerCar = new Views.RegisterCar();
            registerCar.Show();
        }
        
        private void btnUpdate_Click(object sender, EventArgs e)
        {

            try
            {
                Car car = SelectedIndexChanged(Option.Edit);
                var editCar = new Views.ModifyCar(car);
                editCar.ShowDialog();
                RefreshList(); // update the list after editing a car
            }
            catch (System.Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }



        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Car car = SelectedIndexChanged(Option.Delete);
                DialogResult result = MessageBox.Show("Tem certeza que deseja deletar esse carro?", "Confirmar exclusão", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Controllers.CarController.Delete(car);
                    RefreshList(); 
                }
            }
            catch (System.Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        
        public Models.Car SelectedIndexChanged(Option option)
        {
            if (list.SelectedItems.Count > 0)
            {
                int selectedCarId = int.Parse(list.SelectedItems[0].Text);
                return Controllers.CarController.ReadById(selectedCarId);
            }
            else
            {
                throw new System.Exception($"Por gentileza, selecione um carro para {(option == Option.Edit ? "editar" : "deletar")}");
            }
        }
    }
}