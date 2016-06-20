using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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


namespace PruebaGrupo10_WPF
{

    

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Variables
        DatabaseOperation.ConcesionarioOperation callQuery;
        int cod;

        public MainWindow()
        {
            InitializeComponent();
            
            //Inicializamos las variables
            callQuery = new DatabaseOperation.ConcesionarioOperation();

            rellenarListBox();
            CargarModelosTabla();
        }
        
        
        //***********************************************
        //                Funciones privadas
        //***********************************************
        public void rellenarListBox()
        {
            //llamamos a la función
            List<string> result= callQuery.getTipos();

            //Rellenamos el comboBox
            foreach (var tip in result)
            {
                cbTipos.Items.Add(tip.ToString());
            }
        }

        //Accion al cambiar de comboBox
        private void cbTipos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Limpiamos los valores de los listBox
            mListView.Items.Clear();
            mListbox.Items.Clear();

            //llamamos a la función
            List<string> result = callQuery.getModelosPorTipo(cbTipos.SelectedItem.ToString());

            //Rellenamos el comboBox
            foreach (var tip in result)
            {
                mListView.Items.Add(tip.ToString());
                mListbox.Items.Add(tip.ToString());
            }
        }

        //Cargamos DataGrid de Tipos
        private void CargarModelosTabla()
        {
            using (ConcesionarioDB concesDB = new ConcesionarioDB())
            {
                var query = from md in concesDB.Modelos
                            select new { md.CodTipo, md.Marca, md.Descripcion, md.Imagen };
                dataGrid2.ItemsSource = query.ToList();
            }
           
        }


        //Menu para abrir ruta del fichero
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                string direccion = openFileDialog.FileName;
                string archivo = Path.GetFileName(openFileDialog.FileName);
                txtRuta.Text = String.Concat(direccion,"\\",archivo);
            }
        }

        //Funcion para el botón insertar
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (txtMarca.Text != "")
            {
                if (botonInsertar.Content.ToString() == "Insertar")
                {
                    //Insertamos un nuevo campo
                    callQuery.insertarRegistro(txtMarca.Text, txtDesc.Text, txtRuta.Text);
                    CargarModelosTabla();
                }
                else
                {
                    try
                    {
                        callQuery.modificarRegistro(cod, txtMarca.Text, txtDesc.Text, txtRuta.Text);
                    }
                    catch (Exception ex) 
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else 
            {
                MessageBox.Show("Inserte el nombre de la marca del vehiculo.");
            }
        }

        //Cambiar el valor del boton al seleccionar el checkbox
        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {                
                botonInsertar.Content = "Editar";
        }

        private void chEdicion_Unchecked(object sender, RoutedEventArgs e)
        {
                botonInsertar.Content = "Insertar";
        }


        private void dataGrid2_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (chEdicion.IsChecked == true)
            {
                try
                {
                    foreach (var item in e.AddedCells)
                    {
                        var col = item.Column as DataGridColumn;

                        //MessageBox.Show("" + col.Header);
                        var fc = col.GetCellContent(item.Item);

                        switch (Convert.ToString(col.Header))
                        {
                            case "codTipo":
                                cod = Convert.ToInt16((fc as TextBlock).Text);
                                break;
                            case "Marca":
                                txtMarca.Text = (fc as TextBlock).Text;
                                break;
                            case "Descripcion":
                                txtDesc.Text = (fc as TextBlock).Text;
                                break;
                            case "Imagen":
                                //txtRuta.Text=(fc as TextBlock).Text;
                                break;
                        }

                    }
                }catch(Exception){
                }
            }

           
        }
    }
}
