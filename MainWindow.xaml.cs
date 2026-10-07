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
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;

namespace KachalinSemen_PRAK4_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    class Order : INotifyPropertyChanged
    {
        private int id = -1;
        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        private string name = "Order";
        public string NAME
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        private double price = 0;
        public double PRICE
        {
            get
            {
                return price;
            }
            set
            {
                price = value;
                NewEndPrice();
                OnPropertyChanged();
            }
        }
        private int count = 0;
        public int COUNT
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
                NewEndPrice();
                OnPropertyChanged();
            }
        }
        private double Skidka = 0;
        public double SKIDKA
        {
            get
            {
                return Skidka;
            }
            set
            {
                Skidka = value;
                NewEndPrice();
                OnPropertyChanged();
            }
        }
        private double EndPrice = 0;
        public double ENDPRICE
        {
            get
            {
                return EndPrice;
            }
            set
            {
                EndPrice = value;
                OnPropertyChanged();
            }
        }
        public void NewId()
        {
            string[] g = Directory.GetFiles("JSON");
            while (true)
            {
                ID = g.Length + 1;
                if (File.Exists($"JSON\\{g.Length+1}_Order.json"))
                {
                    continue;
                }
                else
                {
                    string text = JsonSerializer.Serialize(this);          
                    File.WriteAllText($"JSON\\{g.Length + 1}_Order.json", text);
                    break;
                }
            }
        }
        void NewEndPrice()
        {
            ENDPRICE = (PRICE * COUNT) * (1 - Skidka / 100);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propName = null)
        {
            PropertyChanged?.Invoke(this, new
            PropertyChangedEventArgs(propName));
        }
    }
    public partial class MainWindow : Window
    {
        private Order myOrder = new Order();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = myOrder;
            myOrder.NewId();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            string text = JsonSerializer.Serialize(myOrder);
            File.WriteAllText($"JSON\\{myOrder.ID}_Order.json",text);
            MessageBox.Show($"Ваш заказ сохранился в {myOrder.ID}_Order.json");
        }
        private void New(object sender, RoutedEventArgs e)
        {
            myOrder = new Order();
            myOrder.NewId();
            DataContext = myOrder;
        }

        private void SearchID(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] f = Directory.GetFiles("JSON");
                for (int i = 0; i < f.Length; i++)
                {
                    if (Convert.ToInt32(f[i].Split('\\')[1].Split('_')[0]) == Convert.ToInt32(TextBoxSearchID.Text))
                    {
                        myOrder = JsonSerializer.Deserialize<Order>(File.ReadAllText(f[i]));
                        DataContext = myOrder;
                        break;
                    }
                }
                MessageBox.Show("JSON с таким ID нету");
            }
            catch
            {
                MessageBox.Show("Пустая строка или не тот тип данных");
            }
        }
    }
}